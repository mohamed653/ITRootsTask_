using ITRootsTask.Data;
using ITRootsTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace ITRootsTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsSignedIn(MyTempData.CurrentUser))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrentUserName = MyTempData.CurrentUser?.UserName;
            var users = _context.Users;
            if (!MyTempData.SearchUserName.IsNullOrEmpty())
            {
                var _users = _context.Users.Where(u => u.UserName == MyTempData.SearchUserName);
                return View(_users);
            }
            return View(users);
        }


        public IActionResult EditUserPartial(int? id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return PartialView("_EditUser", new User());
            }
            return PartialView("_EditUser", user);
        }
        [HttpPost]
        public IActionResult CreateOrEditUser(User user, int? id)
        {
            if (!IsSignedIn(MyTempData.CurrentUser))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!IsAdminAuthorized(MyTempData.CurrentUser))
            {
                return View("401");
            }
            if (ModelState.IsValid)
            {
                // if this user exists then edit/update otherwise create new user
                var checkUser = _context.Users.Find(id);
                if (checkUser == null)
                {
                    //create new user 
                    var newUser = new User
                    {
                        FullName = user.FullName,
                        UserName = user.UserName,
                        Password = user.Password,
                        Email = user.Email,
                        Phone = user.Phone,
                        IsEmailVerified = true,
                    };
                    _context.Users.Add(newUser);
                }
                else
                {
                    // Update User

                    checkUser.FullName = user.FullName;
                    checkUser.UserName = user.UserName;
                    checkUser.Password = user.Password;
                    checkUser.Email = user.Email;
                    checkUser.Phone = user.Phone;
                    checkUser.IsEmailVerified = user.IsEmailVerified;
                }
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult SearchUser()
        {
            var userName = Request.Form["inputsearch"];
            if (!userName.IsNullOrEmpty())
            {
                MyTempData.SearchUserName = userName;
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            if (!IsSignedIn(MyTempData.CurrentUser))
            {
                return RedirectToAction("Login", "Account");
            }
            if (!IsAdminAuthorized(MyTempData.CurrentUser))
            {
                return View("401");
            }
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            return RedirectToAction("index");
        }

        public bool IsSignedIn(User user) => user != null && user.IsActive;
        public bool IsAdminAuthorized(User user) => user.UserType == UserType.Admin;


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}