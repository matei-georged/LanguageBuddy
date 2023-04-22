using LanguageBuddy.Models.Validatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models.ViewModels
{
    public class GoalViewModel
    {
        public Goal Goal { get; set; }
        public List<Language> Languages { get; set; }
        public Profile Profile { get; set; }

        public int SelectedLanguage { get; set; }
    }
}