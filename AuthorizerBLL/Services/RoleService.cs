using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using AuthorizerDAL.Repositories;
using AuthorizerDAL.Models;

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

        public int Create(Role role)
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

        public PageDTO GetByPageId(int id)
        {
            return _iRoleRepository.GetByPageId(id);
        }

        public RoleDTO FindAndInclude(int roleId)
        {
            return _iRoleRepository.FindAndInclude(roleId);
        }

        public IList<RoleDTO> FindAllRoles()
        {

            return _iRoleRepository.FindAllRoles().Select(roles => new RoleDTO
            {
                RoleId = roles.roleId,
                RoleName = roles.roleName
            }).ToList(); ;
        }

        public IList<Page> FindAllPages()
        {
            return _iRoleRepository.FindAllPages().Select(pages => new Page
            {                
                pageId = pages.pageId,
                pageName = pages.pageName
            }).ToList(); ;
        }

        public void attachPageToPage(PageDTO page)
        {
            _iRoleRepository.attachPageToPage(page);
        }
    }
}
