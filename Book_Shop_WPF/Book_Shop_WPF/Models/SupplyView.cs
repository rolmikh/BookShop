using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class SupplyView
    {
        public string НомерПоставки { get; set; } 
        public DateTime? ДатаПоставки { get; set; }
        public string НомерДоговора { get; set; } 
        public DateTime? ДатаПодписания { get; set; }
        public string НомерНакладной { get; set; } 
        public DateTime? ДатаНакладной { get; set; }
        public string НомерСклада { get; set; } 
        public string АдресСклада { get; set; } 
        public string НазваниеКниги { get; set; } 
        public int КоличествоПоставки { get; set; }
        public decimal ЗакупочнаяЦена { get; set; }
        public decimal ОтпускнаяЦена { get; set; }
        public string АртикулТовара { get; set; } 
    }
}
