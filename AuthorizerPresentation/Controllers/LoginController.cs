using AuthorizerBLL.Services;
using AuthorizerPresentation.ViewModels;
using Common.DataTransferObjects;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;

namespace AuthorizerPresentation.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public LoginController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {                                
                var loginObj = _userService.Login(loginUser);

                //Successful login
                if(loginObj != null)
                {
                    Session["Username"] = loginObj.UserName.ToString();
                    Session["RoleId"] = loginObj.RoleId;
                    getPriveleges(loginObj);

                    FormsAuthentication.SetAuthCookie(loginObj.UserName.ToString(), loginUser.RememberMe);
                    return RedirectToAction("UserDashBoard");
                }
                //Unsuccessful login
                else
                {
                    ViewBag.Message = "Invalid Credentials";
                }
            }
            return View(loginUser);
        }

        /// <summary>
        /// Signing out of user
        /// </summary>
        /// <returns></returns>
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
            Session.Abandon();
            return View("Login");
        }

        /// <summary>
        /// User home page
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UserDashBoard()
        {
            try
            {
                if (Session["Username"] != null)
                {
                    return View("UserDashBoard");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// Sets individual page privileges to RoleViewModel
        /// </summary>
        /// <param name="loginObj"></param>
        protected void getPriveleges(UserDTO loginObj)
        {
            var allDbPages = _roleService.FindAllPages().ToList();

            // Get the role we are editing and include the pages it already has access to
            var roleProfile = _roleService.FindAndInclude(loginObj.RoleId);

            RoleViewModel roleViewModel = new RoleViewModel();
            roleViewModel = roleViewModel.ToViewModel(roleProfile, allDbPages);

            var pages = roleViewModel.pageDetails.Where(p => p.Access).Select(p => p.PageName).ToList();
            
            Session["Pages"] = pages;    
        }
    }
}
