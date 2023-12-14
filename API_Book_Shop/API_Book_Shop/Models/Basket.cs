using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Book_Shop.Models
{
    public partial class Basket
    {
        
        public int? IdBasket { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public int? IsDeletedBasket { get; set; }

    }
}
