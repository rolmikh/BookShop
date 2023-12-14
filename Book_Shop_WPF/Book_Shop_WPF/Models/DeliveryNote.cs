using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class DeliveryNote
    {
        public int? IdDeliveryNote { get; set; }
        public string NumberDeliveryNote { get; set; }
        public DateTime? DateDeliveryNote { get; set; }
        public int? ContractId { get; set; }
        public int? IsDeletedNote { get; set; }


        public string NumberContract { get; set; }
        public DateTime? DateContract { get; set; }
    }
}
