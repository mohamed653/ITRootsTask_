using Azure.Core;
using ITRootsTask.Data;
using ITRootsTask.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ITRootsTask.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHtmlLocalizer<AccountController> _localizer;

        public AccountController(ApplicationDbContext context, IHtmlLocalizer<AccountController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (user != null)
            {
                //1. Check if User Exists in DB 
                var userCheck = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
                //2. Check Correct Password and correct user type
                if (userCheck.Password == user.Password && userCheck.UserType == user.UserType)
                {
                    //4. Make User Active

                    userCheck.IsActive = true;
                    _context.Update(userCheck);
                    _context.SaveChanges();

                    //3. Redirect to Main Page 
                    MyTempData.CurrentUser = userCheck;
                    return RedirectToAction("Index", "Home");
                }

            }
            return View(user);
        }
        [HttpPost]
        public IActionResult SignOut()
        {
            if (ModelState.IsValid&& MyTempData.CurrentUser!=null)
            {
                var currentUser = _context.Users.Find(MyTempData.CurrentUser.Id);
                currentUser.IsActive = false;
                _context.Update(currentUser);
                _context.SaveChanges();
                MyTempData.CurrentUser = null;

            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Password = user.Password,
                    Email = user.Email,
                    Phone = user.Phone,
                };
                // add admin if there is no users
                if (!_context.Users.Any())
                {
                    newUser.UserType = UserType.Admin;
                }
                else newUser.UserType = UserType.User;


                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Verify user by mail
                VerifyUserByMail(user);

            }
            return View(user);
        }
        public void VerifyUserByMail(User user)
        {
            if (user != null)
            {
                UserVerification userVerification = new UserVerification();
                userVerification.User = user;
                userVerification.Email = user.Email;
                userVerification.VerificationToken = Guid.NewGuid().ToString();
                userVerification.VerificationTokenExpiry = DateTime.UtcNow.AddDays(1);
                _context.UserVerifications.Add(userVerification);
                _context.SaveChanges();
                PrepareMail(user, userVerification);
            }
        }
        public void PrepareMail(User user, UserVerification userVerification)
        {
            //string emailDomain = user.Email.Split('@').First();

            // this link will be modified after the website domain is set

            string verificationLink = $"https://localhost:7095/Account/VerifyEmail?token={userVerification.VerificationToken}";
            string emailBody = $"Click this link to verify your email: {verificationLink}";
            SendEmail(user.Email, "Verify your email", emailBody);
        }

        private void SendEmail(string email, string subject, string emailBody)
        {
                                                // company mail
            MailMessage mail = new MailMessage("mohamedhamed3343@gmail.com", email);

            mail.Subject = subject;
            mail.Body = emailBody;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "mohamedhamed3343@gmail.com",
                // this the email password you can get from google security    watch this video  https://www.youtube.com/watch?v=gZfi8f8yQYY&list=PL8LWcPWUaTad2f9o9nMZg-loXLIvE8QFk&index=3
                Password = "your email pass"
            };
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            // Look up the user by the verification token
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserVerification.VerificationToken == token);

            if (user == null)
            {
                // Token not found, display an error message
                ViewBag.ErrorMessage = "Invalid verification token.";
                return View("Error");
            }

            // Update the user's IsEmailVerified property and save changes
            user.IsEmailVerified = true;
            _context.Update(user);
            await _context.SaveChangesAsync();

            // Redirect to the login page with a success message
            TempData["SuccessMessage"] = "Your email address has been verified. Please log in.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

            return RedirectToAction("Login");
        }

       
    }
}
