using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizerPresentation.ViewModels
{
    public class PageViewModel
    {
        int pageId;
        string pageDescription;
        public virtual ICollection<RoleViewModel> RoleProfiles { get; set; }

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

        public string PageDescription
        {
            get
            {
                return pageDescription;
            }

            set
            {
                pageDescription = value;
            }
        }

        public static implicit operator PageDTO(PageViewModel page)
        {
            if (page != null)
            {
                return new PageDTO
                {
                    PageId = page.PageId,
                    PageName = page.PageDescription
                };
            }
            else
            {
                return null;
            }
        }
    }
}