using AuthorizerDAL.Models;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        RoleDTO GetByRoleId(int? id);

        PageDTO GetByPageId(int id);

        void attachPageToPage(PageDTO page);

        RoleDTO FindAndInclude(int roleId);
    }
}
