using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class SupplyView
    {
        public string НомерПоставки { get; set; } = null!;
        public DateTime? ДатаПоставки { get; set; }
        public string НомерДоговора { get; set; } = null!;
        public DateTime? ДатаПодписания { get; set; }
        public string НомерНакладной { get; set; } = null!;
        public DateTime? ДатаНакладной { get; set; }
        public string НомерСклада { get; set; } = null!;
        public string АдресСклада { get; set; } = null!;
        public string НазваниеКниги { get; set; } = null!;
        public int КоличествоПоставки { get; set; }
        public decimal ЗакупочнаяЦена { get; set; }
        public decimal ОтпускнаяЦена { get; set; }
        public string АртикулТовара { get; set; } = null!;
    }
}
