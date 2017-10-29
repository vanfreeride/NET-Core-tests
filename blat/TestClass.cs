using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using OfficeOpenXml;
using Xunit;

namespace blat
{
    public class TestClass
    {
        [Fact]
        public void TestMeth()
        {            
            var myFile = new FileInfo("/home/van/Dev/test2.xlsx");

            using (var pack = new ExcelPackage(myFile))
            {
                pack.Workbook.Worksheets.Add("NewSheet2");

                pack.Workbook.Worksheets["NewSheet2"].Cells[1,1].Value = "Hello Excel World2!";
                pack.Save();
            }
         
    }
}
