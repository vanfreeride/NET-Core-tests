using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using OfficeOpenXml;
using RcOnline.Enum;

namespace RcOnline.Models
{
    public class TaskManager
    {
        private const string BASE_URL = "https://api.roskvartal.ru/api/bo/integration";
        private readonly string _path;
        private int startTaskCount;
        private readonly string _apikey;

        public TaskManager(string path)
        {
            _path = path;  
            _apikey = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "apikey")
                            .Trim();
        }

        public void StartPendingTasks()
        {
            using (var pack = new ExcelPackage(new FileInfo(_path)))
            {
                var compSheet = pack.Workbook.Worksheets["Companies"];
                var taskSheet = pack.Workbook.Worksheets["Tasks"];

                int rowNum = 2;

                for (; compSheet.Cells[rowNum, 2].Value != null; rowNum++)
                {
                    string ogrn = compSheet.Cells[rowNum, 2].Value.ToString();
                    int agentId = compSheet.Cells[rowNum, 3].GetValue<int>();
                    var accCell = compSheet.Cells[rowNum, 4];
                    var mdCell = compSheet.Cells[rowNum, 5];
                    var pdCell = compSheet.Cells[rowNum, 6];

                    if (accCell.Value == null)
                    {
                        int? taskId = StartTask(ogrn, agentId, TaskType.Accounts);
                        if (taskId != null)
                            AddTaskToList(taskId, taskSheet, accCell);
                        
                        continue;
                    }
                    
                    if (mdCell.Value == null 
                            && TaskIsCompleted(accCell.GetValue<int>(), taskSheet))
                    {
                        int? taskId = StartTask(ogrn, agentId, TaskType.Meterings);
                        if (taskId != null)
                            AddTaskToList(taskId, taskSheet, mdCell);
                            
                        continue;                            
                    }

                    if (pdCell.Value == null
                            && TaskIsCompleted(accCell.GetValue<int>(), taskSheet)
                            && TaskIsCompleted(mdCell.GetValue<int>(), taskSheet))
                    {
                        int? taskId = StartTask(ogrn, agentId, TaskType.Payments);
                        if (taskId != null)
                            AddTaskToList(taskId, taskSheet, pdCell);
                            
                        continue;                            
                    }
                }

                pack.Save();

                Logger.WriteLineSuccess($"Все прошло круто! (Запущено {startTaskCount} тасок)\n");
            }
        }

        private bool TaskIsCompleted(int taskId, ExcelWorksheet taskSheet)
        {
            int rowNum = 2;

            for(;taskSheet.Cells[rowNum,1].Value != null; rowNum++)
            {
                if (taskSheet.Cells[rowNum,1].GetValue<int>() == taskId)
                    return taskSheet.Cells[rowNum,2].GetValue<int>() == 3;
            }

            return false;
        }

        private void AddTaskToList(int? taskId, ExcelWorksheet taskSheet, ExcelRange cell)
        {
            int rowNum = 2;

            for(;taskSheet.Cells[rowNum,1].Value != null; rowNum++);

            taskSheet.Cells[rowNum,1].Value = taskId;
            taskSheet.Cells[rowNum,2].Value = 0;
            cell.Value = taskId;
        }

        private int? StartTask(string ogrn, int agentId, TaskType type)
        {
            try
            {
                var date = (int)type < 2 ? DateTime.Now : DateTime.Now.AddMonths(-1);
                var obj = new {Ogrn = ogrn, RcOnlineId = agentId, Type = (int)type, Month = date.Month, Year = date.Year};
                var json = JsonConvert.SerializeObject(obj);

                var wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/json");
                wc.Headers.Add("Authorization", _apikey);

                string response = wc.UploadString($"{BASE_URL}/start", "POST", json);

                var idObj = JsonConvert.DeserializeObject<TaskStartResponse>(response);
                startTaskCount++;

                return idObj.IntegrationId;            
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        public void UpdateTaskStatuses()
        {
            using (var pack = new ExcelPackage(new FileInfo(_path)))
            {
                var taskSheet = pack.Workbook.Worksheets["Tasks"];

                int rowNum = 2;

                for (; taskSheet.Cells[rowNum, 1].Value != null; rowNum++)
                {
                    int taskId = taskSheet.Cells[rowNum, 1].GetValue<int>();
                    int taskStatus = taskSheet.Cells[rowNum, 2].GetValue<int>();

                    if (taskStatus != 3)
                    {
                        try 
                        {
                            RcTaskStatusDto status = GetTaskStatus(taskId);
                            taskSheet.Cells[rowNum, 2].Value = status.Status;
                            taskSheet.Cells[rowNum, 4].Value = status.Total;
                            taskSheet.Cells[rowNum, 5].Value = status.Success;
                            taskSheet.Cells[rowNum, 6].Value = status.Errors;
                            taskSheet.Cells[rowNum, 7].Value = status.ErrorMessage;
                        }
                        catch(Exception ex){}
                    }
                }

                pack.Save();

                Logger.WriteLineSuccess("Все прошло круто!\n");
            }
        }

        private RcTaskStatusDto GetTaskStatus(int taskId)
        {
            var wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/json");
            wc.Headers.Add("Authorization", _apikey);

            string response = wc.DownloadString($"{BASE_URL}/{taskId}/status");

            return JsonConvert.DeserializeObject<RcTaskStatusDto>(response);
        }
    }
}