using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Portfolio.Models;
using Portfolio.Pages.Shared;
using Portfolio.Repositories;

namespace Portfolio.Pages.Admin
{
    public class AboutModel : PageModel
    {
        [BindProperty]
        public About About { get; set; }
        [BindProperty]
        public List<Translation> Translations { get; set; }
        [BindProperty]
        public IFormFile Photo { get; set; }

        public List<Languages> Languages { get; set; } = LanguageRepository.GetLanguages();

        public string Error { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("loggedIn") != null)
                return Page();
            return RedirectToPage("Login");
        }

        public IActionResult OnPost()
        {
            About about = new About();

            about.Link_Id = LanguageRepository.AddTranslation(Translations);
            UserMethods d = new UserMethods();
            about.Image_Id = d.SaveImage(Photo, about.Link_Id);
            AboutRepository.AddAbout(about);

            return Page();
        }
    }
}
