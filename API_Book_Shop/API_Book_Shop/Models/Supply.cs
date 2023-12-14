using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class Supply
    {
        public int? IdSupply { get; set; }
        public int? DeliveryNoteId { get; set; }
        public int? WarehouseId { get; set; }
        public string? NumberSupply { get; set; }
        public DateTime? DateSupply { get; set; }
        public int? IsDeleted { get; set; }

    }
}
