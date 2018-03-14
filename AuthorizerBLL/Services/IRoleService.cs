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
        int Create(RoleDTO role);

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
        IList<RoleDTO> FindAll();

        RoleDTO GetByRoleId(int? id)
    }
}
