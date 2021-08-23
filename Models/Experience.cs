using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class Experience
    {
        public int Id { get; set; }
        public int Type_Id { get; set; }
        public int? Image_Id { get; set; }
        public int Link_Id { get; set; }
        public int Experience_Skill_Id { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public DateTime Date_From { get; set; }
        public DateTime Date_To { get; set; }
        public string? URL { get; set; }
    }
}
