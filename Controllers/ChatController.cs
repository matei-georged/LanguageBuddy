using LanguageBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;

namespace LanguageBuddy.Controllers
{
    public class ChatController : Controller
    {

        private ApplicationDbContext bd = new ApplicationDbContext();

        public ActionResult Index()
        {
            string iden = User.Identity.GetUserId();
            Profile selfProf = bd.Profiles.Where(f => f.ApplicationUserID == iden).FirstOrDefault();
            List<Profile> allUsers = new List<Profile>();

            foreach( var mess in selfProf.MessagesReceived )
            {
                if( !allUsers.Contains(mess.Sender) )
                {
                    allUsers.Add(mess.Sender);
                }
            }

            foreach (var mess in selfProf.MessagesSent)
            {
                if (!allUsers.Contains(mess.Receiver))
                {
                    allUsers.Add(mess.Receiver);
                }
            }

            //doar pentru debug
            //allUsers = bd.Profiles.ToList();

            return View(allUsers);
        }

        public ActionResult IndividualChat( string PartenerId )
        {
            string id = User.Identity.GetUserId();

            ICollection<ChatMessage> pass_messages = bd.ChatMessages.
                Where( t => 
                    ( t.SenderId == id && t.ReceiverId == PartenerId ) ||
                    ( t.SenderId == PartenerId && t.ReceiverId == id )
                )
                .ToList();

            ViewBag.parid = PartenerId;
            ViewBag.selfId = id;
            ViewBag.parName = bd.Profiles.Find(PartenerId).FirstName + " " + bd.Profiles.Find(PartenerId).LastName;

            return View(pass_messages);
        }

        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
        {
            try {
                string upload = Request.Files.AllKeys.First();

                if (Request.Files[upload] != null)
                {
                    Request.Files[upload].SaveAs(
                        Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory + "uploads/",
                            Request.Form[0]
                        )
                    );
                }
                return Json(Request.Form[0]);
            } catch( Exception e ) {
                return RedirectToAction("Index");
            }
        }

    }
}