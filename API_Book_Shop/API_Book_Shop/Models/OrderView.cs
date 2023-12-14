using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class OrderView
    {
        public string НомерЗаказа { get; set; } = null!;
        public string СтатусЗаказа { get; set; } = null!;
        public DateTime ДатаОформления { get; set; }
        public decimal СтоимостьЗаказа { get; set; }
        public string СоставЗаказа { get; set; } = null!;
        public string Фамилия { get; set; } = null!;
        public string Имя { get; set; } = null!;
        public string Отчество { get; set; } = null!;
        public string ЭлектроннаяПочта { get; set; } = null!;
        public DateTime ДатаРождения { get; set; }
    }
}
