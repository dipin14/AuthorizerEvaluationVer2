using AuthorizerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizerDAL.Repositories
{
    public interface IPageRepository
    {
        /// <summary>
        /// Insert a Page in database
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        int Create(Page page);

        
        int Update(Page page);

        
        int Delete(Page page);
                
        IList<Page> FindAll();


        Page GetByPageName(string pageName);

        Page GetPagePriveleges(int pageId);
    }
}
