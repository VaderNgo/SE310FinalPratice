using BaiTap4.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTap4.Controllers
{
    public class AccessController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Login(TUser user)
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                var u = db.TUsers.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
                if (u != null)
                {
                    if (u.LoaiUser == 0)
                    {
                        HttpContext.Session.SetString("Username", u.Username);
                        return RedirectToAction("Index", "Home");
                    }
                    else if (u.LoaiUser == 1)
                    {
                        HttpContext.Session.SetString("Username", u.Username);
                        return RedirectToAction("Index", "HomeAdmin", new { area = "admin" });
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Register(TUser user)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                var existingUser = db.TUsers.FirstOrDefault(x => x.Username == user.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Username already exists");
                    return View(user);
                }

                // Create new user (default to regular user type 0)
                var newUser = new TUser
                {
                    Username = user.Username,
                    Password = user.Password,
                    LoaiUser = 0  // Set as regular user by default
                };

                try
                {
                    db.TUsers.Add(newUser);
                    db.SaveChanges();

                    // Automatically log in the user after registration
                    HttpContext.Session.SetString("Username", newUser.Username);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating account. Please try again.");
                    // Log the error if you have logging configured
                    return View(user);
                }
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Access");
        }
    }
}