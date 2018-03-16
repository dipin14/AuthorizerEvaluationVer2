using AuthorizerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataTransferObjects
{
    public class PrivilegeDTO
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

        public static implicit operator PrivilegeDTO(Page page)
        {
            if (page != null)
            {
                return new PrivilegeDTO
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

        public static implicit operator PrivilegeDTO(PageDTO pageDto)
        {
            if (pageDto != null)
            {
                return new PrivilegeDTO
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
    }
}
