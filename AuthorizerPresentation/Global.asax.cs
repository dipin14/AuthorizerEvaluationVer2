﻿using AuthorizerBLL.Services;
using AuthorizerDAL.DatabaseContext;
using AuthorizerDAL.Models;
using AuthorizerDAL.Repositories;
using AuthorizerPresentation.App_Start;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Linq;
using System.Reflection;
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
                    //Creating default superuser role
                    Role suRole = new Role();
                    suRole.roleName = "superuser";
                    db.Roles.Add(suRole);             
                    int suId = suRole.roleId;                    

                    //Creating default superuser user credentials
                    User suUser = new User();
                    suUser.userName = "superuser";
                    suUser.password = "superuser";
                    suUser.firstName = "dipin";
                    suUser.lastName = "dinesh";
                    suUser.roleId = suId;
                    db.Users.Add(suUser);
                    db.SaveChanges();

                    if (db.Users.Any(u => u.Role.roleName.ToLower() == "admin"))
                    {
                        //Creating default admin role
                        Role adminRole = new Role();
                        adminRole.roleName = "admin";
                        db.Roles.Add(adminRole);
                        int adminId = adminRole.roleId;

                        //Creating default admin user credentials
                        User adminUser = new User();
                        adminUser.userName = "admin";
                        adminUser.password = "admin";
                        adminUser.firstName = "peter";
                        adminUser.lastName = "parker";
                        adminUser.roleId = suId;

                        db.Users.Add(adminUser);
                        db.SaveChanges();
                    }
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
                var set = entities.Set<Page>();
                if (set.Any())
                {
                    return;
                }
                else
                {
                    entities.Pages.Add(pageA);
                    entities.Pages.Add(pageB);
                    entities.Pages.Add(pageC);
                    entities.SaveChanges();
                }
            }                          
        }  
    }
}
