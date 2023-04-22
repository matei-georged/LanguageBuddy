using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models
{
    public class Tip
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This is required")]
        [RegularExpression(@"^[a-zA-z0-9 ]{1,256}$", ErrorMessage = "Doar litere si numere")]
        public string Description { get; set; }

        [ForeignKey("Language")]
        [Required(ErrorMessage = "This is required")]
        public int LanguageId { get; set; }

        [ForeignKey("Profile")]
        public string ProfileFk { get; set; }

        public virtual Language Language { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Atention> Attentions { get; set; }
    }
}