using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models
{
    public class Atention
    {
        public int Id { get; set; }
        public int Value { get; set; }

        [ForeignKey("Profile")]
        public string ProfileFk { get; set; }

        [ForeignKey("Tip")]
        public int TipFk { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Tip Tip { get; set; }
    }
}