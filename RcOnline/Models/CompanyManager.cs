using System;
using System.IO;
using OfficeOpenXml;

namespace RcOnline.Models
{
    public class CompanyManager
    {
        private readonly string _path;

        public CompanyManager(string path)
        {
            _path = path;
        }

        public void AddCompany()
        {
            string ogrn = "";

            while(!ulong.TryParse(ogrn, out ulong foo))
            {
                Console.Write("Введите ОГРН: ");
                ogrn = Console.ReadLine().Trim();
            }

            string agentId = "";

            while(!ushort.TryParse(agentId, out ushort bar))
            {
                Console.Write("Введите AgentId: ");
                agentId = Console.ReadLine().Trim();
            }

            try
            {
                AddNewCompanyToList(ogrn, agentId);
            }
            catch(Exception ex)
            {
                Logger.WriteLineError(ex.Message + Environment.NewLine);
            }
        }

        private void AddNewCompanyToList(string ogrn, string agentId)
        {
            using (var pack = new ExcelPackage(new FileInfo(_path)))
            {
                int rowNum = 1;
                var compSheet = pack.Workbook.Worksheets["Companies"];

                for (; compSheet.Cells[rowNum, 2].Value != null; rowNum++)
                {
                    if (compSheet.Cells[rowNum, 2].Value.ToString() == ogrn &&
                        compSheet.Cells[rowNum, 3].Value.ToString() == agentId)
                        throw new Exception("Ссорян, контора уже есть в списке");
                }

                compSheet.Cells[rowNum, 2].Value = ogrn;
                compSheet.Cells[rowNum, 3].Value = agentId;

                pack.Save();
                
                Logger.WriteLineSuccess("Успешно добавлена!\n");
            }
        }
    }
}