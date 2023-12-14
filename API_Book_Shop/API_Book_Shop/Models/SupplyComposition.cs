using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class SupplyComposition
    {
        public int? IdSupplyComposition { get; set; }
        public int? ProductId { get; set; }
        public int? SupplyId { get; set; }
        public int? CountSupply { get; set; }
        public decimal? PriceSupply { get; set; }
        public int? IsDeleted { get; set; }
    }
}
