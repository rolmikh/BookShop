using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class DeliveryNote
    {
        public int? IdDeliveryNote { get; set; }
        public string? NumberDeliveryNote { get; set; }
        public DateTime? DateDeliveryNote { get; set; }
        public int? ContractId { get; set; }
        public int? IsDeletedNote { get; set; }
    }
}
