using AuthorizerDAL.Models;
using Common.DataTransferObjects;
using System.Collections.Generic;

namespace AuthorizerBLL.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Create a role model
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int Create(Role role);

        /// <summary>
        /// Update existing role model
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        int Update(RoleDTO role);

        /// <summary>
        /// Remove role model
        /// </summary>
        /// <param name="role id"></param>
        /// <returns></returns>
        int Delete(int? id);

        /// <summary>
        /// List all roles
        /// </summary>
        /// <returns></returns>
        IList<RoleDTO> FindAllRoles();

        /// <summary>
        /// List all pages
        /// </summary>
        /// <returns></returns>
        IList<Page> FindAllPages();

        /// <summary>
        /// Get role details using role id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoleDTO GetByRoleId(int? id);

        /// <summary>
        /// Get page details using page id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PageDTO GetByPageId(int id);

        /// <summary>
        /// Attach Page model eith page entity
        /// </summary>
        /// <param name="page"></param>
        void attachPageToPage(PageDTO page);

        /// <summary>
        /// Retrive role details along with privileged pages
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleDTO FindAndInclude(int roleId);
    }
}
