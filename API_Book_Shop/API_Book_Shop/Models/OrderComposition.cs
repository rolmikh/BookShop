using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class OrderComposition
    {
        public int? IdOrderComposition { get; set; }
        public int? OrderId { get; set; }
        public int? BasketId { get; set; }
        public int? UserId { get; set; }
        public int? IsDeleted { get; set; }
    }
}
