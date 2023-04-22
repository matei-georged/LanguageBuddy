using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LanguageBuddy.Models
{
    public class Profile
    {
        [Key]
        public string ApplicationUserID { get; set; }

        [RegularExpression(@"^[a-zA-z ]{1,32}$", ErrorMessage = "Doar litere")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-z ]{1,32}$", ErrorMessage = "Doar litere")]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }

        [RegularExpression(@"^[a-zA-z0-9 ]{1,256}$", ErrorMessage = "Doar litere si numere")]
        public string Description { get; set; }

        public virtual ApplicationUser UserAccount {get; set;}

        public virtual ICollection<Language> Languages { get; set; }

        public virtual ICollection<Goal> Goals{ get; set; }

        public virtual ICollection<ChatMessage> MessagesSent { get; set; }

        public virtual ICollection<ChatMessage> MessagesReceived { get; set; }

        public virtual ICollection<Atention> Attentions { get; set; }

        public virtual ICollection<Tip> Tips { get; set; }

        [NotMapped]
        public List<String> LanguageIdList { get; set; }

    }
}