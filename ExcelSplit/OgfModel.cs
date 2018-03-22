using OfficeOpenXml;

namespace ExcelSplit
{
    internal class OgfModel
    {       

        public OgfModel(ExcelWorksheet sheet, int row, OgfMap map)
        {
            Row = row;
            Address = sheet.Cells[row, map.AddressCol].Text;
            HouseNumber = sheet.Cells[row, map.HouseNumberCol].Text;
            PremiseNum = sheet.Cells[row, map.PremiseNumCol].Text;
            UniquePremiseNum = sheet.Cells[row, map.UniquePremiseNumCol].Text;
        }
        
        public int Row { get; private set; }
        public string Address { get; private set; }
        
        public string HouseNumber { get; private set; }
        
        public string PremiseNum { get; private set; }
        
        public string UniquePremiseNum { get; private set; }
    }
}