using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class Warehouse
    {
        public int? IdWarehouse { get; set; }
        public string? NumberWarehouse { get; set; }
        public string? Address { get; set; }
        public int? IsDeletedWarehouse { get; set; }
    }
}
