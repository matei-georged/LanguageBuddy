using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LanguageBuddy.Models;
using LanguageBuddy.Models.Validatos;

namespace LanguageBuddy.Models
{
    public class Goal
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This is required")]
        [ForeignKey("Profile")]
        public string ProfileFk { get; set; }

        [Required(ErrorMessage = "This is required")]
        [ForeignKey("Language")]
        public int LanguageFk { get; set; }

        [Required(ErrorMessage = "This is required")]
        public DateTime StartDate { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Language Language { get; set; }

    }
}