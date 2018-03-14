using AuthorizerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataTransferObjects
{
    public class PageDTO
    {
        int pageId;
        string pageName;
        public virtual ICollection<Role> Roles { get; set; }

        public int PageId
        {
            get
            {
                return pageId;
            }

            set
            {
                pageId = value;
            }
        }

        public string PageName
        {
            get
            {
                return pageName;
            }

            set
            {
                pageName = value;
            }
        }
        /// <summary>
        /// Implicit Conversion of RoleDTO to Role
        /// </summary>
        /// <param name="roleDto"></param>
        public static implicit operator Page(PageDTO pageDto)
        {
            if (pageDto != null)
            {
                return new Page
                {
                    pageId = pageDto.PageId,
                    pageName = pageDto.PageName
                };
            }
            else
            {
                return null;
            }
        }
        
        public static implicit operator PageDTO(Page page)
        {
            if (page != null)
            {
                return new PageDTO
                {
                    pageId = page.pageId,
                    pageName = page.pageName
                };
            }
            else
            {
                return null;
            }
        }
    }
}
