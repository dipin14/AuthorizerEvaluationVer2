using AuthorizerDAL.Models;
using Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AuthorizerPresentation.ViewModels
{
    public class RoleViewModel
    {
        int roleId;
        string roleName;

        public virtual ICollection<PrivilegeData> pageDetails { get; set; }
        
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

        [Required]
        [Display(Name = "Role Name")]
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

        
        public static implicit operator RoleDTO(RoleViewModel role)
        {
            if (role != null)
            {
                return new RoleDTO
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Pages = role.pageDetails.Select(p => new PrivilegeDTO
                    {
                        Access = p.Access,
                        PageId = p.PageId,
                        PageName = p.PageName
                    }).ToList()
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Implicit conversion of RoleDTO to RoleViewModel
        /// </summary>
        /// <param name="roleDto"></param>
        public static implicit operator RoleViewModel(RoleDTO roleDto)
        {
            if (roleDto != null)
            {
                return new RoleViewModel
                {
                    RoleId = roleDto.RoleId,
                    RoleName = roleDto.RoleName
                };
            }
            else
            {
                return null;
            }
        }

        public RoleViewModel ToViewModel(Role roleProfile, ICollection<Page> allDbPages)
        {
            var roleViewProfile = new RoleViewModel
            {
                RoleId = roleProfile.roleId,
                RoleName = roleProfile.roleName
            };

            // Collection for full list of pages with roles's already assigned pages included
            ICollection<PrivilegeData> allPages = new List<PrivilegeData>();

            foreach (var p in allDbPages)
            {
                // Create new setPage for each page and set Assigned = true if user already has privilege for page
                var setPage = new PrivilegeData
                {
                    PageId = p.pageId,
                    Access = roleProfile.Pages.FirstOrDefault(x => x.pageId == p.pageId) != null,
                    PageName = p.pageName
                };
                allPages.Add(setPage);
            }


            roleViewProfile.pageDetails = allPages;

            return roleViewProfile;
        }

        public static implicit operator RoleViewModel (Role roleProfile)
        {
            var roleViewProfile = new RoleViewModel
            {
                RoleId = roleProfile.roleId,
                RoleName = roleProfile.roleName
            };
            if (roleProfile.Pages != null)
            {
                foreach (var page in roleProfile.Pages)
                {
                    roleViewProfile.pageDetails.Add(new PrivilegeData
                    {
                        PageId = page.pageId,
                        PageName = page.pageName,
                        Access = true
                    });
                }
            }
            return roleViewProfile;
        }


        public static implicit operator  Role (RoleViewModel roleProfileViewModel)
        {
            var roleProfile = new Role();
            roleProfile.roleName = roleProfileViewModel.RoleName;
            roleProfile.roleId = roleProfileViewModel.RoleId;
            return roleProfile;
        }        
    }
}