using System;
using System.Collections.Generic;
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
        private int reStartTaskCount;
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

                for (int rowNum = 2; compSheet.Cells[rowNum, 2].Value != null; rowNum++)
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

        public void RestartSuccessTasks()
        {
            TaskType type = GetTaskType();
            int columnNum = type == TaskType.Accounts 
                    ? 4
                    : (type == TaskType.Meterings ? 5 : 6);

            using (var pack = new ExcelPackage(new FileInfo(_path)))
            {
                var compSheet = pack.Workbook.Worksheets["Companies"];
                var taskSheet = pack.Workbook.Worksheets["Tasks"];

                for (int i = 2; compSheet.Cells[i,columnNum].Value != null; i++)
                {
                    int taskId = compSheet.Cells[i,columnNum].GetValue<int>();

                    if (TaskIsCompletedButUnDone(taskId, taskSheet))
                    {
                        RestartTask(taskId);
                        ClearTaskStatus(taskId, taskSheet);
                        reStartTaskCount++;                        
                    }
                }                

                if (reStartTaskCount > 0)
                    pack.Save();                
            }

            Logger.WriteLineSuccess($"Все прошло круто! (Перезапущено {reStartTaskCount} тасок)\n");
        }

        private void ClearTaskStatus(int taskId, ExcelWorksheet taskSheet)
        {
            for(int rowNum = 2; taskSheet.Cells[rowNum,1].Value != null; rowNum++)
            {
                if (taskSheet.Cells[rowNum,1].GetValue<int>() == taskId)
                    taskSheet.Cells[rowNum,2].Value = 0;
            }
        }

        private TaskType GetTaskType()
        {
            while (true)
            {
                Console.WriteLine("Введите тип тасок: 0 - ЛС, 1 - ПУ, 3 - ПД: ");
                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "0":
                        return TaskType.Accounts;
                    case "1":
                        return TaskType.Meterings;
                    case "3":
                        return TaskType.Payments;                    
                }
            }
        }

        internal void RestartErrorTasks()
        {
            using (var pack = new ExcelPackage(new FileInfo(_path)))
            {
                var taskSheet = pack.Workbook.Worksheets["Tasks"];

                for(int rowNum = 2; taskSheet.Cells[rowNum,1].Value != null; rowNum++)
                {
                    int taskId = taskSheet.Cells[rowNum,1].GetValue<int>();
                    int status = taskSheet.Cells[rowNum,2].GetValue<int>();

                    if (status == 4)
                    {
                        RestartTask(taskId);
                        reStartTaskCount++;
                    }
                }
            }

            Logger.WriteLineSuccess($"Все прошло круто! (Перезапущено {reStartTaskCount} тасок)\n");
        }

        private void RestartTask(int taskId)
        {
            try
            {
                var wc = new WebClient();
                wc.Headers.Add("Content-Type", "application/json");
                wc.Headers.Add("Authorization", _apikey);

                wc.DownloadString($"{BASE_URL}/{taskId}/restart");
            }
            catch(Exception ex){ }
        }

        private bool TaskIsCompleted(int taskId, ExcelWorksheet taskSheet)
        {
            for(int rowNum = 2; taskSheet.Cells[rowNum,1].Value != null; rowNum++)
            {
                if (taskSheet.Cells[rowNum,1].GetValue<int>() == taskId)
                    return taskSheet.Cells[rowNum,2].GetValue<int>() == 3;
            }

            return false;
        }

        private bool TaskIsCompletedButUnDone(int taskId, ExcelWorksheet taskSheet)
        {
            for(int rowNum = 2; taskSheet.Cells[rowNum,1].Value != null; rowNum++)
            {
                if (taskSheet.Cells[rowNum,1].GetValue<int>() == taskId)
                    return taskSheet.Cells[rowNum,2].GetValue<int>() == 3 &&
                           taskSheet.Cells[rowNum,5].GetValue<int>() > 0 &&
                           taskSheet.Cells[rowNum,6].GetValue<int>() > 0;
            }

            return false;
        }

        private void AddTaskToList(int? taskId, ExcelWorksheet taskSheet, ExcelRange cell)
        {
            int rowNum = 2;

            for(;taskSheet.Cells[rowNum,1].Value != null; rowNum++);

            taskSheet.Cells[rowNum,1].Value = taskId;
            taskSheet.Cells[rowNum,2].Value = 0;
            taskSheet.Cells[rowNum,3].Value = 1;
            cell.Value = taskId;
        }

        private int? StartTask(string ogrn, int agentId, TaskType type)
        {
            try
            {
                var date = (int)type < 2 ? DateTime.Now : DateTime.Now.AddMonths(-1);
                var obj = new {Ogrn = ogrn, AgentId = agentId, Type = (int)type, Month = date.Month, Year = date.Year};
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

                for (int rowNum = 2; taskSheet.Cells[rowNum, 1].Value != null; rowNum++)
                {
                    int taskId = taskSheet.Cells[rowNum, 1].GetValue<int>();
                    int taskStatus = taskSheet.Cells[rowNum, 2].GetValue<int>();

                    if (taskStatus != 3)
                    {
                        try 
                        {
                            TaskStatusResponse status = GetTaskStatus(taskId);
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

        private TaskStatusResponse GetTaskStatus(int taskId)
        {
            var wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/json");
            wc.Headers.Add("Authorization", _apikey);

            string response = wc.DownloadString($"{BASE_URL}/{taskId}/status");

            return JsonConvert.DeserializeObject<TaskStatusResponse>(response);
        }
    }
}