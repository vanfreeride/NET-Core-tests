using System.ComponentModel;
using System.Reflection;
using OfficeOpenXml;
using System;

    public class AccountColumnMap
    {        
        public AccountColumnMap(ExcelWorksheet sheet)
        {
            var props = GetType().GetProperties();

            for (int i = 1; sheet.Cells[1, i].Value != null; i++)
            {
                var val = sheet.Cells[1, i].Value.ToString().ToLower().Trim();

                bool columnIsMapped = false;

                foreach (var prop in props)
                {
                    var descAttr = prop.GetCustomAttribute<DescriptionAttribute>();

                    if (descAttr != null && val.Contains(prop.GetCustomAttribute<DescriptionAttribute>().Description.ToLower()))
                    {
                        prop.SetValue(this, i);

                        if (ResultCol < i + 1)
                            ResultCol = i + 1;
                            
                        columnIsMapped = true;
                        break;
                    }
                }

                if (!columnIsMapped)
                    throw new Exception($"Не опознана колонка шаблона '{val}'");
            }
        }

        [Description("проживающих")]
        public int LivingPersonsCol { get; private set; }

        [Description("отапливаемая")]
        public int HeatedSquareCol { get; private set; }

        [Description("жилая")]
        public int ResidentialSquareCol { get; private set; }

        [Description("адрес")]
        public int AddressCol { get; private set; }

        [Description("№ помещения")]
        public int PremiseNumCol { get; private set; }

        [Description("№ комнаты")]
        public int LivingRoomNumCol { get; private set; }

        [Description("номер лицевого счета")]
        public int NumberCol { get; private set; }

        [Description("фамилия")]
        public int LastNameCol { get; private set; }

        [Description("имя")]
        public int FirstNameCol { get; private set; }

        [Description("отчество")]
        public int SecondNameCol { get; private set; }

        [Description("общая площадь")]
        public int TotalSquareCol { get; private set; }

        [Description("жилое/нежилое")]
        public int LivingOrNotCol { get; private set; }

        [Description("наниматель")]
        public int IsRenterCol { get; private set; }

        [Description("для кап. ремонта")]
        public int IsCrCol { get; private set; }

        public int ResultCol { get; set; }
    }

