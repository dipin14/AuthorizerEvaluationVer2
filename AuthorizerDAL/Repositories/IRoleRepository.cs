using AuthorizerDAL.Models;
using System.Collections.Generic;

namespace AuthorizerDAL.Repositories
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Insert a Role in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int Create(Role role);

        /// <summary>
        /// Edit a Role in database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int Update(Role role);

        /// <summary>
        /// Remove a Role from database
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int Delete(int? id);


        /// <summary>
        /// List all roles
        /// </summary>
        /// <returns></returns>
        IList<Role> FindAllRoles();

        /// <summary>
        /// List all pages
        /// </summary>
        /// <returns></returns>
        IList<Page> FindAllPages();

        /// <summary>
        /// Get role details using role id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Role GetByRoleId(int? roleId);

        /// <summary>
        /// Get page details using page id
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        Page GetByPageId(int pageId);

        /// <summary>
        /// Attach page model to entity
        /// </summary>
        /// <param name="page"></param>
        void attachPageToPage(Page page);

        /// <summary>
        /// Retrieve page privileges using roleid
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Role FindAndInclude(int roleId);
    }
}
