using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class Order
    {
        public int? IdOrder { get; set; }
        public int? StatusOrderId { get; set; }
        public string NumberOrder { get; set; }
        public DateTime? DateOrder { get; set; }
        public int? IsDeleted { get; set; }
        public decimal? PriceOrder { get; set; }

    }
}
