﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizerDAL.Models
{
    public class Role
    {
        public Role()
        {
            Pages = new List<Page>();
        }
        [Key]
        public int roleId { get; set; }
        public string roleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
    }
}
