using LanguageBuddy.Models.Validatos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models
{
    public class Language
    {
        [Key]
        public int Id { get; set; }

        [LanguageReady]
        [RegularExpression( @"^[a-zA-z ]{1,32}$", ErrorMessage = "Doar litere")]
        [Required(ErrorMessage = "This is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This is required")]
        [RegularExpression(@"^[a-zA-z0-9 ]{1,256}$", ErrorMessage = "Doar litere si numere")]
        public string Description { get; set; }

        [Required(ErrorMessage = "This is required")]
        public string ImageUrl { get; set; }

        public virtual ICollection<Profile> Profiles {get; set;}
        public virtual ICollection<Tip> Tips { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }

    }
}