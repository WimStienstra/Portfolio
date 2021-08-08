using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class Translation
    {
        public int Id { get; set; }
        public int Language_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
