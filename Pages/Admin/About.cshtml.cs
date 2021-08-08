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
        public Image Image { get; set; }
        [BindProperty]
        public IFormFile Photo { get; set; }

        public List<Languages> Languages { get; set; } = LanguageRepository.GetLanguages();

        public string Error { get; set; }

        private IHostingEnvironment ihostingEnvironment;

        public AboutModel(IHostingEnvironment ihostingEnvironment)
        {
            this.ihostingEnvironment = ihostingEnvironment;
        }


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("loggedIn") != null)
                return Page();
            return RedirectToPage("Login");
        }

        public IActionResult OnPost()
        {
            LanguageRepository.AddTranslation(Translations);

            //if (Photo != null)
            //{
            //    var path = Path.Combine(ihostingEnvironment.WebRootPath, "images", Image.Id + " - " + Photo.FileName);
            //    using (var stream = new FileStream(path, FileMode.Create))
            //    {
            //        Photo.CopyToAsync(stream);
            //        Image.Link_Id = TranslationLink.Id;
            //        Image.Location = Photo.FileName;
            //    }
            //}

            return Page();
        }
    }
}
