using System.Linq;
using OfficeOpenXml;

public class RowModel
    {

        public RowModel(ExcelWorksheet sheet, int row, AccountColumnMap map)
        {
            RowNum = row;
            Address = GetStringValue(sheet, row, map.AddressCol);
            PremiseNum = GetStringValue(sheet, row, map.PremiseNumCol);
            LivingOrNot = GetStringValue(sheet, row, map.LivingOrNotCol);
            LivingRoomNum = GetStringValue(sheet, row, map.LivingRoomNumCol);
            Number = GetStringValue(sheet, row, map.NumberCol);
            LastName = GetStringValue(sheet, row, map.LastNameCol);
            FirstName = GetStringValue(sheet, row, map.FirstNameCol);
            SecondName = ToLettersOnly(GetStringValue(sheet, row, map.SecondNameCol));
            TotalSquare = GetStringValue(sheet, row, map.TotalSquareCol);
            ResidentialSquare = GetStringValue(sheet, row, map.ResidentialSquareCol);
            HeatedSquare = GetStringValue(sheet, row, map.HeatedSquareCol);
            LivingPersons = GetStringValue(sheet, row, map.LivingPersonsCol);
            IsRenter = GetStringValue(sheet, row, map.IsRenterCol);
        }

        private string GetStringValue(ExcelWorksheet sheet, int row, int col)
        {
            if (col == 0)
                return null;

            return sheet.Cells[row, col].Value?.ToString().Trim();
        }
        
        private string ToLettersOnly(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var chars = str.Where(x => char.IsLetter(x)).ToArray();

            return new string(chars);
        }

        public string LivingPersons { get; set; }
        public string HeatedSquare { get; set; }
        public string ResidentialSquare { get; set; }
        public int RowNum { get; set; }
        public string Address { get; set; }
        public string PremiseNum { get; set; }
        public string LivingRoomNum { get; set; }
        public string Number { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string TotalSquare { get; set; }
        public string LivingOrNot { get; set; }
        public string IsRenter { get; set; }
        public string UniquePremiseNum { get; set; }
        public string FullAddress { get; set; }
}