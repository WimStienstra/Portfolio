using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Repositories;

namespace Portfolio.Pages.Admin
{
    public class LanguageModel : PageModel
    {
        [BindProperty]
        public Languages Language{ get; set; }
        public string Error { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("loggedIn") != null)
                return Page();
            return RedirectToPage("Login");
        }
        public IActionResult OnPost()
        {
            LanguageRepository.AddLanguage(Language);

            return Page();
        }
    }
}
