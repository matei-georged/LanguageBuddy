using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models
{
    public class Buddy
    {
        public string Name { get; set; }
        public string WantedLnaguage { get; set; }

        public string Description { get; set; }

        public Profile Profile { get; set; }
    }
}