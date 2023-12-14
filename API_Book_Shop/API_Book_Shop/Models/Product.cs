﻿using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class Product
    {
        
        public int? IdProduct { get; set; }
        public string? NameBook { get; set; }
        public string? Author { get; set; } 
        public int? Count { get; set; }
        public string? ArticleProduct { get; set; } 
        public int? CategoryId { get; set; }
        public string? Series { get; set; } 
        public string? CoverType { get; set; } 
        public string? YearOfPublication { get; set; }
        public string? PublishingHouse { get; set; }
        public string? AgeRestriction { get; set; }
        public string? PhotoBook { get; set; }
        public int? NumberOfPages { get; set; }
        public decimal? PriceBook { get; set; }
        public string? Annotation { get; set; } 
        public int? IsDeleted { get; set; }
 }
}
