using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class Supply
    {
        public int? IdSupply { get; set; }
        public int? DeliveryNoteId { get; set; }
        public int? WarehouseId { get; set; }
        public string NumberSupply { get; set; }
        public DateTime? DateSupply { get; set; }
        public int? IsDeleted { get; set; }

        public string NumberWarehouse { get; set; }
        public string Address { get; set; }
        public string NumberDeliveryNote { get; set; }
        public DateTime? DateDeliveryNote { get; set; }

    }
}
