using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class Contract
    {
        public int? IdContract { get; set; }
        public string? NumberContract { get; set; } 
        public DateTime? DateContract { get; set; }
        public int? IsDeletedContract { get; set; }
    }
}
