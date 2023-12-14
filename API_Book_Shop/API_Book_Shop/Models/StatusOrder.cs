using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class StatusOrder
    {
        public int? IdStatusOrder { get; set; }
        public string? NameStatusOrder { get; set; }
        public int? IsDeleted { get; set; }
    }
}
