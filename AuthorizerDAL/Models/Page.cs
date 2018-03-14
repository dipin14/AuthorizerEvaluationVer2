using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizerDAL.Models
{
    public class Page
    {
        [Key]
        public int pageId { get; set; }
        public string pageName { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
