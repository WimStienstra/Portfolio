using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Portfolio.Models;
using Portfolio.Pages.Shared;
using Portfolio.Repositories;

namespace Portfolio.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        public string Error { get; set; }

        AppConfiguration _config;

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
                if (AddUser())
                    return RedirectToPage("Login");
            }
            return Page();
        }

        /// <summary>
        /// Handles user adding
        /// </summary>
        /// <returns>True if success</returns>
        private bool AddUser()
        {
            bool result;
            if (_config.RegisterSecret != User.RegisterSecret)
            {
                Error += "Secret key is incorrect";
                return false;
            }
            try
            {
                User.Salt = Convert.ToBase64String(UserMethods.GetSalt(32));
                User.Password = Convert.ToBase64String(UserMethods.GenerateSaltedHash(Encoding.ASCII.GetBytes(User.Password), Convert.FromBase64String(User.Salt)));
                result = UserRepository.AddUser(User);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicate entry") && e.Message.Contains("email"))
                {
                    Error += "Email already in use";
                    return false;
                }
                Error += "Error appeared in registering the user, try and follow the red errors.";
                return false;
            }

            return result;
        }
    }
}
