using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
//using Microsoft.AspNetCore.Identity;

using DAL.Service;
using DAL;

using UI.Models;
using DAL.Data;
using DAL.Repository;


namespace UILayer.Controllers
{
    public class ValidationController : Controller

    {
        private readonly IAdminRepository adminRepository;
        private readonly ICustomerRepository customerRepository;

        public ValidationController(IAdminRepository adminRepository, ICustomerRepository customerRepository)
        {
            this.adminRepository = adminRepository;
            this.customerRepository = customerRepository;
        }
        private readonly InsuranceDbContext context;

        public ValidationController()
        {
            this.context = new InsuranceDbContext();
        }


        // GET: Validation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(UserView user)
        {
            if (adminRepository.AdminExistsEmail(user.Email) || customerRepository.CustomerExistsEmail(user.Email))
            {
                // Email already registered
                ModelState.AddModelError("Email", "Email already registered with us.");
                return View("Registration", user);
            }
            else if (adminRepository.AdminExists(user.UserName) || customerRepository.CustomerExists(user.UserName))
            {
                // Username already registered
                ModelState.AddModelError("UserName", "Username already registered with us.");

                return View("Registration", user);
            }
            if (user.UserType == 2)
            {
                Customer customer = new Customer
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.UserType,
                    Password = user.Password,

                };

                customerRepository.CreateCustomer(customer);


                return RedirectToAction("CustomerLogin", "Validation");
            }
            else
            {
                Admin newadmin = new Admin
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleId = user.UserType,
                    Password = user.Password,
                };

                adminRepository.CreateAdmin(newadmin);

                return RedirectToAction("Index", "Admin");
            }
        }
        // GET: Account
        public ActionResult CustomerLogin()
        {


            return View();
        }

        public ActionResult AdminLogin()
        {


            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(LoginView loginView)
        {
            var isAdmin = Authentication.VerifyAdminCredentials(loginView.UserName, loginView.Password);
            //var isAdmin = AuthenticateAdmin(loginView.UserName, loginView.Password);

            if (isAdmin)
            {
                FormsAuthentication.SetAuthCookie(loginView.UserName, false);
                return RedirectToAction("Dashboard", "Admin");

            }
            else
            {
                // If authentication fails, you may want to show an error message.
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginView);
            }
        }
        //private bool AuthenticateAdmin(string username, string password)
        //{

        //    //var admin = context.Admins.SingleOrDefault(a => a.UserName == username && a.Password == password);


        //    // Return true if an admin is found, otherwise false.
        //    return admin != null;
        //}
        [HttpPost]
        public ActionResult CustomerLogin(LoginView loginView)
        {
            var isAdmin = Authentication.VerifyCustomerCredentials(loginView.UserName, loginView.Password);
            /*ar isAdmin = AuthenticateAdmin(loginView.UserName, loginView.Password);*/
            if (isAdmin)
            {
                var user = customerRepository.GetCustomerByUserName(loginView.UserName);
                Session["UserId"] = user.Id;
                Session["UserName"] = user.UserName;
                FormsAuthentication.SetAuthCookie(loginView.UserName, false);
                return RedirectToAction("Dashboard", "Customer");
            }
            else
            {
                // If authentication fails, you may want to show an error message.
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(loginView);
            }
        }




        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = customerRepository.GetCustomerByUserName(model.UserName);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Invalid username. Please enter a valid username.");
                    return View(model);
                }
                else
                {
                    // Assuming that model.Password is already in plain text
                    user.Password = model.Password;
                    customerRepository.customerSAveChanges();
                }

                TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                return RedirectToAction("CustomerLogin", "Validation");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = adminRepository.GetAdminByUserName(model.UserName);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Invalid username. Please enter a valid username.");
                    return View(model);
                }
                else
                {
                    // Assuming that model.Password is already in plain text
                    user.Password = model.Password;
                    adminRepository.SaveAdminchages();
                }

                TempData["SuccessMessage"] = "Password reset successfully. Please log in with your new password.";
                return RedirectToAction("AdminLogin", "Validation");
            }

            return View(model);
        }




        public ActionResult Logout()
        {
            // Add any logout logic here, such as clearing session or authentication data

            // Set cache control headers to prevent caching
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetExpires(System.DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();

            // Redirect to the home page
            return RedirectToAction("Index", "Home"); // Adjust "Index" and "Home" based on your actual home page route
        }

    }
}