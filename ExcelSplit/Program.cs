using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace ExcelSplit
{
    class Program
    {
        static void Main(string[] args)
        {
            List<RowModel> accounts;
            List<OgfModel> ogfs;

            string path = @"C:\Users\van\Documents\ExcelParsing\норильск\Шаблон последний для РК ЗАПОЛНЕН.xlsx";
            using (var pack = new ExcelPackage(new FileInfo(path)))
            {
                var sheet = pack.Workbook.Worksheets[1];

                var map = new AccountColumnMap(sheet);

                accounts = GetAccountModels(map, sheet);
            }

            string pathOgf = @"C:\Users\van\Documents\ExcelParsing\норильск\норильск_ожф.xlsx";
            using (var pack = new ExcelPackage(new FileInfo(pathOgf)))
            {
                var sheet = pack.Workbook.Worksheets[1];

                var map = new OgfMap(sheet);

                ogfs = GetOgfModels(map, sheet);
            }

            foreach (var acc in accounts)
            {
                var adrParts = acc.Address.Split(',');

                foreach (var ogf in ogfs)
                {
                    var ogfParts = ogf.Address.Split(',');

                    if (adrParts[2] == ogfParts[2] && adrParts[3] == ogfParts[3] && adrParts[4] == ogfParts[4]
                        && acc.PremiseNum == ogf.PremiseNum)
                    {
                        acc.UniquePremiseNum = ogf.UniquePremiseNum;
                        acc.FullAddress = ogf.Address;
                        break;
                    }
                }
            }

            var accsToUpload = accounts.Where(x => x.UniquePremiseNum == null).Select(x => x.Number).ToList();

            using (var pack = new ExcelPackage(new FileInfo(path)))
            {
                var sheet = pack.Workbook.Worksheets[1];

                for (int i = 1; sheet.Cells[i,5].Value != null; i++)
                {
                    if (!accsToUpload.Contains(sheet.Cells[i,5].Text.Trim()))
                    {
                        sheet.Cells[i,1].Value = null;
                    }
                }

                pack.SaveAs(new FileInfo(@"C:\Users\van\Documents\ExcelParsing\норильск\остатки.xlsx"));
            }




            // for (int i = 13; i <= 14; i++)
            // {
            //     var accs = accsToUpload.Skip((i - 13) * 1000).Take(1000);

            //     using(var pack = new ExcelPackage(new FileInfo(@"C:\Users\van\Documents\ExcelParsing\норильск\template.xlsx")))
            //     {
            //         var sheet1 = pack.Workbook.Worksheets[1];
            //         var sheet2 = pack.Workbook.Worksheets[2];

            //         int count = 0;
            //         foreach (var acc in accs)
            //         {
            //             count++;
            //             sheet1.Cells[count + 2, 1].Value = count;
            //             sheet1.Cells[count + 2, 2].Value = acc.Number;
            //             sheet1.Cells[count + 2, 4].Value = "ЛС УО";
            //             sheet1.Cells[count + 2, 5].Value = acc.IsRenter;
            //             sheet1.Cells[count + 2, 6].Value = "Нет";
            //             sheet1.Cells[count + 2, 7].Value = acc.LastName;
            //             sheet1.Cells[count + 2, 8].Value = acc.FirstName;
            //             sheet1.Cells[count + 2, 9].Value = acc.SecondName;
            //             sheet1.Cells[count + 2, 18].Value = acc.TotalSquare;
            //             sheet1.Cells[count + 2, 19].Value = acc.ResidentialSquare;
            //             sheet1.Cells[count + 2, 21].Value = acc.LivingPersons;

            //             sheet2.Cells[count + 2, 1].Value = count;
            //             sheet2.Cells[count + 2, 2].Value = acc.FullAddress;
            //             sheet2.Cells[count + 2, 7].Value = acc.UniquePremiseNum;
            //             sheet2.Cells[count + 2, 8].Value = 100;
            //         }

            //         pack.SaveAs(new FileInfo($@"C:\Users\van\Documents\ExcelParsing\норильск\4gis\{i}.xlsx"));
            //     }

            // }

        }

        private static List<OgfModel> GetOgfModels(OgfMap map, ExcelWorksheet sheet)
        {
            var models = new List<OgfModel>();

            for (int i = 3; sheet.Cells[i, 1].Value != null; i++)
            {
                models.Add(new OgfModel(sheet, i, map));
            }

            return models;
        }

        private static List<RowModel> GetAccountModels(AccountColumnMap map, ExcelWorksheet sheet)
        {
            var models = new List<RowModel>();

            for (int i = 2; sheet.Cells[i, 1].Value != null; i++)
            {
                var resultCell = sheet.Cells[i, map.ResultCol];

                if (resultCell.Value != null &&
                    (resultCell.Value.ToString().Contains("успешно") || resultCell.Value.ToString().Contains("Уже есть в ГИС")))
                    continue;

                models.Add(new RowModel(sheet, i, map));
            }

            return models;
        }
    }
}
