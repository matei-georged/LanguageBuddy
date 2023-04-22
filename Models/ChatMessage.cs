using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageBuddy.Models
{
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Message { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        public int    IsAudio { get; set; }


        public virtual Profile Receiver { get; set; }
        public virtual Profile Sender { get; set; }

    }
}