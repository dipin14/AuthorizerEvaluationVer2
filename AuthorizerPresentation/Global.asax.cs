using AuthorizerBLL.Services;
using AuthorizerDAL.DatabaseContext;
using AuthorizerDAL.Models;
using AuthorizerDAL.Repositories;
using AuthorizerPresentation.ViewModels;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace AuthorizerPresentation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Registering dependency injection for Autofac
            var builder = new ContainerBuilder();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AdminExists();

            MockPagesInitializer();

            
        }

        /// <summary>
        /// Considering the condition there is no existing Admin in database.
        /// An admin with username 'admin' and password 'admin' is created by default.
        /// </summary>
        /// <returns></returns>
        protected int AdminExists()
        {
            using (UserDbContext db = new UserDbContext())
            {
                if (db.Users.Any(u => u.Role.roleName.ToLower() == "superuser"))
                {
                    return 0;
                }
                else
                {
                    Role role = new Role();
                    Role role2 = new Role();

                    role.roleName = "superuser";
                    role2.roleName = "admin";

                    db.Roles.Add(role);
                    db.Roles.Add(role2);
                    db.SaveChanges();

                    int suId = role.roleId;
                    int adminId = role2.roleId;

                    User user = new User();
                    user.userName = "superuser";
                    user.password = "superuser";
                    user.firstName = "dipin";
                    user.lastName = "dinesh";
                    user.roleId = suId;

                    User user2 = new User();
                    user2.userName = "admin";
                    user2.password = "admin";
                    user2.firstName = "peter";
                    user2.lastName = "parker";
                    user2.roleId = suId;

                    db.Users.Add(user);
                    db.Users.Add(user2);
                    db.SaveChanges();

                    return 1;
                }
            }
        }

        /// <summary>
        /// Authentication logic retrieves list of roles from database and adds to principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //Obtaining username from cookie               
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (UserDbContext entities = new UserDbContext())
                        {
                            User user = entities.Users.SingleOrDefault(u => u.userName == username);
                            roles = user.Role.roleName;
                        }
                        //Extracting roles from database


                        //Setting principal with user specific details
                        e.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }


        public void MockPagesInitializer()
        {
            Page pageA = new Page { pageId = 1, pageName = "MockPage A" };
            Page pageB = new Page { pageId = 2, pageName = "MockPage B" };
            Page pageC = new Page { pageId = 3, pageName = "MockPage C" };
            using (UserDbContext entities = new UserDbContext())
            {
                entities.Pages.Add(pageA);
                entities.Pages.Add(pageB);
                entities.Pages.Add(pageC);
            }                            
        }

        
    }
}
