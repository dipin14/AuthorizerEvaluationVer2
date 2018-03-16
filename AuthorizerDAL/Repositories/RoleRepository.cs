using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthorizerDAL.Models;
using AuthorizerDAL.DatabaseContext;
using System.Data.Entity;

namespace AuthorizerDAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {        
        public int Create(Role role)
        {
            try
            {
                if (role != null)
                {
                    using (var db = new UserDbContext())
                    {
                        db.Roles.Add(role);
                        db.SaveChanges();
                    }
                    return 1;
                }
                return 0;
            }
            catch(Exception)
            {
                return -1;
            }
        }
        
        public int Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    using (var db = new UserDbContext())
                    {
                        var roleProfile = db.Roles.Include("Pages").Single(r => r.roleId == id);

                        if (roleProfile.Pages != null)
                        {
                            foreach (var page in roleProfile.Pages.ToList())
                            {
                                roleProfile.Pages.Remove(page);
                            }
                        }
                        db.Roles.Remove(roleProfile);
                        db.Entry(roleProfile).State = EntityState.Deleted;
                        db.SaveChanges();
                    }
                    return 1;
                }
                return 0;
            }
            catch(Exception)
            {
                return -1;
            }
        }
        
        public IList<Role> FindAllRoles()
        {
            IList<Role> roleList;
            using (var db = new UserDbContext())
            {
                roleList = db.Roles.ToList();
            }
            return roleList;
        }

        public IList<Page> FindAllPages()
        {
            IList<Page> pageList;
            using (var db = new UserDbContext())
            {
                pageList = db.Pages.ToList();
            }
            return pageList;
        }

        public Role GetByRoleId(int? id)
        {
            IQueryable<Role> roleProfileIQueryable;
            UserDbContext db = new UserDbContext();
            roleProfileIQueryable = from r in db.Roles.Include("Pages")
                                    where r.roleId == id
                                    select r;

            if (!roleProfileIQueryable.Any())
            {
                return null /*HttpNotFound("Role not found.")*/;
            }

            var roleProfile = roleProfileIQueryable.First();

            return roleProfile;
        }

        public Page GetByPageId(int id)
        {
            using (var db = new UserDbContext())
            {
                var page = db.Pages.Find(id);
                return page;
            }
        }

        private void DeleteRole(Role roleProfile)
        {
            if (roleProfile.Pages != null)
            {
                foreach (var page in roleProfile.Pages.ToList())
                {
                    roleProfile.Pages.Remove(page);
                }
            }
            using (var db = new UserDbContext())
            {
                db.Roles.Remove(roleProfile);
                db.SaveChanges();
            }
        }

        public int Update(Role role)
        {
            throw new NotImplementedException();
        }

        public void attachPageToPage(Page page)
        {
            using (var db = new UserDbContext())
            {
                db.Pages.Attach(page);
            }
        }

        public Role FindAndInclude(int roleId)
        {
            using (var db = new UserDbContext())
            {
                var roleProfile = db.Roles.Include("Pages").FirstOrDefault(x => x.roleId == roleId);
                return roleProfile;
            }
        }
    }
}
