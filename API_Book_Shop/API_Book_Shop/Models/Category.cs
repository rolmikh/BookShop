﻿using System;
using System.Collections.Generic;

namespace API_Book_Shop.Models
{
    public partial class Category
    {
        public int? IdCategory { get; set; }
        public string? NameCategory { get; set; } 
        public int? IsDeletedCategory { get; set; }
    }
}
