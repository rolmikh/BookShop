using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class Basket
    {
        public int? IdBasket { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? IsDeletedBasket { get; set; }


        public string NameBook { get; set; }
        public string PhotoBook { get; set; }
        public string Author { get; set; }
        public string PriceBook { get; set; }
    }
}
