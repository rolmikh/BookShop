using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class Order
    {
        public int? IdOrder { get; set; }
        public int? StatusOrderId { get; set; }
        public string? NumberOrder { get; set; }
        public DateTime? DateOrder { get; set; }
        public int? IsDeleted { get; set; }
        public decimal? PriceOrder { get; set; }

    }
}
