using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ExcelSplit
{
    public class OgfMap
    {
        public OgfMap(ExcelWorksheet sheet)
        {
            var props = GetType().GetProperties();

            for (int i = 1; sheet.Cells[1, i].Value != null; i++)
            {
                var val = sheet.Cells[1, i].Value.ToString().ToLower().Trim();

                foreach (var prop in props)
                {
                    var descAttr = prop.GetCustomAttribute<DescriptionAttribute>();

                    if (descAttr != null && val.Contains(prop.GetCustomAttribute<DescriptionAttribute>().Description.ToLower()))
                    {
                        prop.SetValue(this, i);
                        break;
                    }
                }
            }

            if (!IsValid())
                throw new Exception("Не все колонки опознаны");
        }

        [Description("Адрес ОЖФ")]
        public int AddressCol { get; private set; }

        [Description("Номер дома")]
        public int HouseNumberCol { get; private set; }

        [Description("Номер жилого помещения")]
        public int PremiseNumCol { get; private set; }

        [Description("Уникальный номер помещения")]
        public int UniquePremiseNumCol { get; private set; }

        public bool IsValid()
        {
            var props = GetType().GetProperties();

            foreach (var prop in props)
            {
                if ((int)prop.GetValue(this) == 0)
                    return false;                
            }

            return true;
        }
    }
}
