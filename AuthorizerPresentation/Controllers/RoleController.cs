using AuthorizerBLL.Services;
using AuthorizerDAL.DatabaseContext;
using AuthorizerDAL.Models;
using AuthorizerPresentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AuthorizerPresentation.Controllers
{
    [Authorize(Roles = "admin,superuser")]
    public class RoleController : Controller
    {
        UserDbContext db = new UserDbContext();

        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            var roleProfiles = db.Roles.ToList();

            //Admin should not be able to view superuser details
            if(User.IsInRole("admin"))
            {
                var superUser = roleProfiles.Where(r => r.roleName == "superuser").FirstOrDefault();
                roleProfiles.Remove(superUser);
                var adminUser = roleProfiles.Where(r => r.roleName == "admin").FirstOrDefault();
                roleProfiles.Remove(adminUser);
            }

            //Mapping RolesDTO to RoleViewModels
            var roleViewModels = roleProfiles.Select(roleProfile => new RoleViewModel()
            {
                RoleId = roleProfile.roleId,
                RoleName = roleProfile.roleName,
                pageDetails = roleProfile.Pages.Select(p => new PrivilegeData
                {
                    PageId = p.pageId,
                    PageName = p.pageName,
                    Access = true
                }).ToList()
            });

            return View(roleViewModels);
        }

        public ActionResult Create()
        {
            var roleViewModel = new RoleViewModel { pageDetails = PopulatePageData() };
            if (roleViewModel == null)
            {
                return View();
            }
            else
            {
                return View(roleViewModel);
            }
        }

        [HttpPost]
        public ActionResult Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var roleProfile = new Role { roleName = roleViewModel.RoleName };

                var addResult = AddOrUpdatePages(roleProfile, roleViewModel.pageDetails);
                if (addResult == -1)
                {
                    ModelState.AddModelError("RoleName","Rolename already exists");
                    roleViewModel = new RoleViewModel { pageDetails = PopulatePageData() };
                    return View(roleViewModel);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View(roleViewModel);
        }

        public ActionResult Edit(int id = 0)
        {
            // Get all pages
            var allDbPages = db.Pages.ToList();

            // Get the role we are editing and include the pages already subscribed to
            var roleProfile = db.Roles.Include("Pages").FirstOrDefault(x => x.roleId == id);
            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel = roleViewModel.ToViewModel(roleProfile, allDbPages);

            return View(roleViewModel);
        }

        [HttpPost]
        public ActionResult Edit(RoleViewModel roleProfileViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var originalRoleProfile = db.Roles.Find(roleProfileViewModel.RoleId);

                    var editResult = AddOrUpdatePages(originalRoleProfile, roleProfileViewModel.pageDetails);

                    if (editResult == -1)
                    {
                        ModelState.AddModelError("RoleName", "Rolename already exists");
                        return View();
                    }
                    else
                    {
                        //Updating values of role page privileges
                        db.Entry(originalRoleProfile).CurrentValues.SetValues(roleProfileViewModel);
                        //Updating rolename seperately due to change in property names
                        db.Entry(originalRoleProfile).Property(p => p.roleName).CurrentValue = roleProfileViewModel.RoleName;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            //Exception arises when duplicate role name is entered in edit field
            //Reloading page privileges details is necessary
            catch(Exception)
            {
                ModelState.AddModelError("RoleName", "Rolename already exists");

                // Get all pages
                var allDbPages = _roleService.FindAllPages().ToList();

                // Get the role we are editing and include the pages it already has access to
                var roleProfile = _roleService.FindAndInclude(roleProfileViewModel.RoleId);

                RoleViewModel roleViewModel = new RoleViewModel();
                roleViewModel = roleViewModel.ToViewModel(roleProfile, allDbPages);

                return View(roleViewModel);
            }
            return View(roleProfileViewModel);
        }

        public ActionResult Details(int id = 0)
        {
            // Get all pages
            var allDbPages = _roleService.FindAllPages().ToList();
            
            // Get the role we are editing and include the pages it already has access to
            var roleProfile = _roleService.FindAndInclude(id);

            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel = roleViewModel.ToViewModel(roleProfile, allDbPages);

            return View(roleViewModel);
        }

        public ActionResult Delete(int id = 0)
        {
            var roleProfile = _roleService.GetByRoleId(id);
            if (roleProfile == null)
            {
                return HttpNotFound("Role not found.");
            }
            else if(User.IsInRole(roleProfile.RoleName))
            {
                ViewBag.Message = "Cannot delete your current role";
                return View();
            }
            else
            {
                RoleViewModel roleProfileViewModel = roleProfile;
                return View(roleProfileViewModel);
            }

        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            _roleService.Delete(id);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Adds or Updates rolename and access privileges for passed role
        /// </summary>
        /// <param name="roleProfile"></param>
        /// <param name="privilegeAccessed"></param>
        /// <returns></returns>
        private int AddOrUpdatePages(Role roleProfile, IEnumerable<PrivilegeData> privilegeAccessed)
        {
            try
            {
                if (privilegeAccessed != null)
                {
                    //Existing user then page privileges already exist so drop existing pages and add new if any
                    if (roleProfile.roleId != 0)
                    {
                        foreach (var page in roleProfile.Pages.ToList())
                        {
                            roleProfile.Pages.Remove(page);
                        }

                        foreach (var page in privilegeAccessed.Where(p => p.Access))
                        {
                            roleProfile.Pages.Add(db.Pages.Find(page.PageId));
                        }
                        return 1;
                    }
                    //New User to be created with selected pages
                    else
                    {
                        foreach (var pageAccess in privilegeAccessed.Where(p => p.Access))
                        {
                            var page = new Page { pageId = pageAccess.PageId };
                            db.Pages.Attach(page);
                            roleProfile.Pages.Add(page);
                        }
                        db.Roles.Add(roleProfile);
                        db.SaveChanges();
                        return 1;
                    }

                }
                //New User with no selected pages
                else
                {
                    db.Roles.Add(roleProfile);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch(Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Populates viewmodel with page values
        /// </summary>
        /// <returns></returns>
        private ICollection<PrivilegeData> PopulatePageData()
        {
            UserDbContext entities = new UserDbContext();
            var pages = entities.Pages;
            try
            {
                var accessiblePrivileges = new List<PrivilegeData>();

                //Admin has privileges of only those pages assigned by superuser
                if (User.IsInRole("admin"))
                {
                    var roleProfile = _roleService.FindAndInclude(Convert.ToInt32(Session["RoleId"]));
                    accessiblePrivileges = roleProfile.Pages.Select(p => new PrivilegeData
                    {
                        PageId = p.PageId,
                        PageName = p.PageName,
                        Access = p.Access
                    }).ToList();
                }
                else
                {
                    foreach (var item in pages)
                    {
                        accessiblePrivileges.Add(new PrivilegeData
                        {
                            PageId = item.pageId,
                            PageName = item.pageName,
                            Access = false
                        });
                    }
                }
                return accessiblePrivileges;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}