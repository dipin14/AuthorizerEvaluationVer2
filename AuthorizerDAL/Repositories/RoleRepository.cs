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
                        DeleteRole(roleProfile);
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
        
        public IList<Role> FindAll()
        {
            IList<Role> roleList;
            using (var db = new UserDbContext())
            {
                roleList = db.Roles.ToList();
            }
            return roleList;
        }
                
        public Role GetByRoleId(int? id)
        {
            IQueryable<Role> roleProfileIQueryable;
            using (var db = new UserDbContext())
            {
                roleProfileIQueryable = from r in db.Roles.Include("Pages")
                                            where r.roleId == id
                                            select r;
            }

            if (!roleProfileIQueryable.Any())
            {
                return null /*HttpNotFound("Role not found.")*/;
            }

            var roleProfile = roleProfileIQueryable.First();

            return roleProfile;
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
    }
}
