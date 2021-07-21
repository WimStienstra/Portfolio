using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Repositories;

namespace Portfolio.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        public bool IsValid { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("loggedIn") != null)
                return RedirectToPage("Admin/Home");
            return Page();
        }

        public IActionResult OnPost()
        {

            if (ModelState.IsValid || ModelState.ErrorCount == 2)
            {
                User.Email = User.Email;
                User.Password = User.Password;

                using var db = DbUtils.GetDbConnection();

                int userId = UserRepository.GetUserByLogin(User.Email, User.Password);

                if (userId != 0)
                {
                    Response.Cookies.Append("rememberMe", RememberMe.ToString());
                    HttpContext.Session.SetInt32("loggedIn", userId);
                    return RedirectToPage("Admin/Home");
                }
                IsValid = false;
            }
            return Page();
        }
    }
}
