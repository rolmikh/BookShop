using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class OrderView
    {
        public string НомерЗаказа { get; set; }
        public string СтатусЗаказа { get; set; } 
        public DateTime ДатаОформления { get; set; }
        public decimal СтоимостьЗаказа { get; set; }
        public string СоставЗаказа { get; set; } 
        public string Фамилия { get; set; } 
        public string Имя { get; set; } 
        public string Отчество { get; set; }
        public string ЭлектроннаяПочта { get; set; } 
        public DateTime ДатаРождения { get; set; }
    }
}
