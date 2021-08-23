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
            About about = new About();

            if (Photo != null)
            {
                Image image = new Image();
                var path = Path.Combine(ihostingEnvironment.WebRootPath, "images", Image.Id + " - " + Photo.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Photo.CopyToAsync(stream);
                    //image.Link_Id = Image.Link_Id = TranslationLink.Id;
                    image.Location = Image.Location = Photo.FileName;
                    about.Image_Id = ImageRepository.AddImage(image);
                }
            }

            about.Link_Id = LanguageRepository.AddTranslation(Translations);
            AboutRepository.AddAbout(about);

            return Page();
        }
    }
}
