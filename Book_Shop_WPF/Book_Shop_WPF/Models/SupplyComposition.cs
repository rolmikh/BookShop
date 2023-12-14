using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class SupplyComposition
    {
        public int? IdSupplyComposition { get; set; }
        public int? ProductId { get; set; }
        public int? SupplyId { get; set; }
        public int? CountSupply { get; set; }
        public decimal? PriceSupply { get; set; }
        public int? IsDeleted { get; set; }

        public string NameProduct { get; set; } = null;
        public string NumberSupply { get; set; } = null;
        public DateTime? DateSupply { get; set; } = null;
    }
}
