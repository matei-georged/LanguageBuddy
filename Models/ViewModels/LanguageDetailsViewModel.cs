using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models.ViewModels
{
    public class LanguageDetailsViewModel
    {
        public List<Profile> Profiles { get; set; }
        public List<Tip> Tips { get; set; }

    }
}