using AuthorizerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizerPresentation.ViewModels
{
    public class PrivilegeData
    {
        int pageId;
        string pageName;
        bool access;

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

        public bool Access
        {
            get
            {
                return access;
            }

            set
            {
                access = value;
            }
        }

        public static implicit operator Page(PrivilegeData pageData)
        {
            var pages = new Page();
            pages.pageId = pageData.PageId;
            pages.pageName = pageData.PageName;
            return pages;            
        }
    }
}