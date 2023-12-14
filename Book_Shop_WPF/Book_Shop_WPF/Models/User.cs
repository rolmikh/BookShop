using System;
using System.Collections.Generic;

namespace Book_Shop_WPF.Models
{
    public partial class User
    {
        public int? IdUser { get; set; }
        public string SurnameUser { get; set; }
        public string NameUser { get; set; } 
        public string MiddleNameUser { get; set; } 
        public string EmailUser { get; set; } 
        public string PasswordUser { get; set; } 
        public DateTime? DateBirthUser { get; set; }
        public string SaltUser { get; set; }
        public int? RoleId { get; set; }
        public int? IsDeleted { get; set; }

        public string FIO { get; set; }
        public string date { get; set; }

    }
}
