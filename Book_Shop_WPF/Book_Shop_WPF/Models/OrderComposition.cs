using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class OrderComposition
    {
        public int? IdOrderComposition { get; set; }
        public int? OrderId { get; set; }
        public int? BasketId { get; set; }
        public int? UserId { get; set; }
        public int? IsDeleted { get; set; }

        public string NameStatusOrder { get; set; }
        public string NumberOrder { get; set; }
        public DateTime DateOrder { get; set; }
        public decimal PriceOrder { get; set; }
    }
}
