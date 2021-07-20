using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WimStienstra.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} length must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords do not match with eachother.")]
        public string ConfirmPassword { get; set; }
        public string RegisterSecret { get; set; }

        public string Salt { get; set; }
    }
}
