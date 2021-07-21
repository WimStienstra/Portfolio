using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brides.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Portfolio.Models;
using Portfolio.Repositories;

namespace Portfolio.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        public string Error { get; set; }

        AppConfiguration _config;
        public IConfiguration Configuration { get; }

        public RegisterModel(AppConfiguration config)
        {
            _config = config;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid || ModelState.ErrorCount == 0)
            {
                var userId = AddUser();
                if (userId != 0)
                    return RedirectToPage("Login");
            }
            return Page();
        }

        /// <summary>
        /// Handles user adding
        /// </summary>
        /// <returns>Created UserId or 0 if failed</returns>
        private int AddUser()
        {
            User result;
            try
            {
                User.Salt = Convert.ToBase64String(UserMethods.GetSalt(32));
                User.Password = Convert.ToBase64String(UserMethods.GenerateSaltedHash(Encoding.ASCII.GetBytes(User.Password), Convert.FromBase64String(User.Salt)));
                if (_config.RegisterSecret == User.RegisterSecret)
                {
                    result = UserRepository.AddUser(User);
                }
                else
                {
                    Error += "Secret key is incorrect";
                    return 0;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicate entry") && e.Message.Contains("email"))
                {
                    Error += "Email already in use";
                    return 0;
                }
                Error += "Error appeared in registering the user, try and follow the red errors.";
                return 0;
            }
            return result.Id;
        }
    }
}
