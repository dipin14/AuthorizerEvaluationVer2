using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using AuthorizerDAL.Repositories;

namespace AuthorizerBLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _iRoleRepository;

        //DI to UserRepository
        public RoleService(IRoleRepository roleRepository)
        {
            _iRoleRepository = roleRepository;
        }

        public int Create(RoleDTO role)
        {
            return _iRoleRepository.Create(role);
        }


        public int Update(RoleDTO role)
        {
            return _iRoleRepository.Update(role);
        }

        public int Delete(int? roleId)
        {
            return _iRoleRepository.Delete(roleId);
        }


        public RoleDTO GetByRoleId(int? id)
        {
            return _iRoleRepository.GetByRoleId(id);
        }

        public RoleDTO GetPagePriveleges(int roleId)
        {
            return _iRoleRepository.GetPagePriveleges(roleId);
        }

        public IList<RoleDTO> FindAll()
        {

            return _iRoleRepository.FindAll().Select(roles => new RoleDTO
            {
                RoleId = roles.roleId,
                RoleName = roles.roleName
            }).ToList(); ;
        }
    }
}
