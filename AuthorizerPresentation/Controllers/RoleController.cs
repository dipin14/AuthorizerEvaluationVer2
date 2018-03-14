using AuthorizerBLL.Services;
using AuthorizerDAL.DatabaseContext;
using AuthorizerDAL.Models;
using AuthorizerPresentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthorizerPresentation.Controllers
{
    //[Authorize(Roles = "admin")]
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
            var roleProfiles = _roleService.FindAll();/*db.Roles.ToList();*/
            var roleViewModels = roleProfiles.Select(roleProfile => new RoleViewModel()
            {
                RoleId = roleProfile.RoleId,
                RoleName = roleProfile.RoleName,
                pageDetails = roleProfile.Pages.Select(p => new PrivilegeData()
                {
                    PageId = p.pageId,
                    PageName = p.pageName
                }).ToList()
            });     

            return View(roleViewModels);
        }

        public ActionResult Create()
        {
            var roleViewModel = new RoleViewModel { pageDetails = PopulatePageData() };

            return View(roleViewModel);
        }

        [HttpPost]
        public ActionResult Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var roleProfile = new Role { roleName = roleViewModel.RoleName };

                AddOrUpdatePages(roleProfile, roleViewModel.pageDetails);
                db.Roles.Add(roleProfile);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(roleViewModel);
        }

        public ActionResult Edit(int id = 0)
        {
            // Get all courses
            var allDbPages = db.Pages.ToList();

            // Get the user we are editing and include the courses already subscribed to
            var roleProfile = db.Roles.Include("Pages").FirstOrDefault(x => x.roleId == id);
            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel = roleViewModel.ToViewModel(roleProfile, allDbPages);

            return View(roleViewModel);
        }

        [HttpPost]
        public ActionResult Edit(RoleViewModel roleProfileViewModel)
        {
            if (ModelState.IsValid)
            {

                var originalRoleProfile = db.Roles.Find(roleProfileViewModel.RoleId);
                AddOrUpdatePages(originalRoleProfile, roleProfileViewModel.pageDetails);
                db.Entry(originalRoleProfile).CurrentValues.SetValues(roleProfileViewModel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(roleProfileViewModel);
        }

        public ActionResult Details(int id = 0)
        {
            // Get all pages
            var allDbPages = db.Pages.ToList();

            // Get the role we are editing and include the pages it already has access to
            var roleProfile = db.Roles.Include("Pages").FirstOrDefault(x => x.roleId == id);

            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel = roleViewModel.ToViewModel(roleProfile, allDbPages);

            return View(roleViewModel);
        }

        public ActionResult Delete(int id = 0)
        {
            //var roleProfileIQueryable = from r in db.Roles.Include("Pages")
            //                            where r.roleId == id
            //                            select r;

            //if (!roleProfileIQueryable.Any())
            //{
            //    return HttpNotFound("Role not found.");
            //}

            //var roleProfile = roleProfileIQueryable.First();
            var roleProfile = _roleService.GetByRoleId(id);
            if (roleProfile == null)
            {
                return HttpNotFound("Role not found.");
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
            //var roleProfile = db.Roles.Include("Pages").Single(r => r.roleId == id);           

            //DeleteRole(roleProfile);
            _roleService.Delete(id);

            return RedirectToAction("Index");
        }

        //private void DeleteRole(Role roleProfile)
        //{
        //    if (roleProfile.Pages != null)
        //    {
        //        foreach (var page in roleProfile.Pages.ToList())
        //        {
        //            roleProfile.Pages.Remove(page);
        //        }
        //    }

        //    db.Roles.Remove(roleProfile);
        //    db.SaveChanges();
        //}

        private void AddOrUpdatePages(Role roleProfile, IEnumerable<PrivilegeData> privilegeAccessed)
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
                }
                else
                {
                    foreach (var pageAccess in privilegeAccessed.Where(p => p.Access))
                    {
                        var page = new Page { pageId = pageAccess.PageId };
                        db.Pages.Attach(page);
                        roleProfile.Pages.Add(page);

                    }
                }
            }
            else
                return;
        }

        private ICollection<PrivilegeData> PopulatePageData()
        {
            UserDbContext entities = new UserDbContext();
            var pages = entities.Pages;
            var accessiblePrivileges = new List<PrivilegeData>();

            foreach (var item in pages)
            {
                accessiblePrivileges.Add(new PrivilegeData
                {
                    PageId = item.pageId,
                    PageName = item.pageName,
                    Access = false
                });
            }

            return accessiblePrivileges;
        }


    }
}