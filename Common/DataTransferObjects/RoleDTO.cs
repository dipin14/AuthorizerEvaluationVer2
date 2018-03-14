using AuthorizerDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataTransferObjects
{
    public class RoleDTO
    {
        int roleId;
        string roleName;
        public virtual ICollection<Page> Pages { get; set; }

        public int RoleId
        {
            get
            {
                return roleId;
            }

            set
            {
                roleId = value;
            }
        }

        public string RoleName
        {
            get
            {
                return roleName;
            }

            set
            {
                roleName = value;
            }
        }
        
        /// <summary>
        /// Implicit Conversion of RoleDTO to Role
        /// </summary>
        /// <param name="roleDto"></param>
        public static implicit operator Role(RoleDTO roleDto)
        {
            if (roleDto != null)
            {
                return new Role
                {
                    roleId = roleDto.RoleId,
                    roleName = roleDto.RoleName
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Implicit conversion of Role to RoleDTO
        /// </summary>
        /// <param name="role"></param>
        public static implicit operator RoleDTO(Role role)
        {
            if (role != null)
            {
                return new RoleDTO
                {
                    roleId = role.roleId,
                    roleName = role.roleName
                };
            }
            else
            {
                return null;
            }
        }

    }
}
