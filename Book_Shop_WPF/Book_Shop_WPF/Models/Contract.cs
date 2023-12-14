using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class Contract
    {
        public int? IdContract { get; set; }
        public string NumberContract { get; set; } 
        public DateTime? DateContract { get; set; }
        public int? IsDeletedContract { get; set; }

    }
}
