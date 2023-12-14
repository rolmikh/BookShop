using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class CustomerCard
    {
        public int? IdCustomerCard { get; set; }
        public string? NumberCard { get; set; }
        public int? UserId { get; set; }
        public string? ValidityPeriod { get; set; }
        public string? CvvCode { get; set; } 
        public string? SaltCard { get; set; }

    }
}
