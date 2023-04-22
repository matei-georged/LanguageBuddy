using LanguageBuddy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageBuddy.Controllers
{

    public class BuddyController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Buddy
        public ActionResult Index()
        {

            string userId = User.Identity.GetUserId();
            List<int> user_knows = GetknowLanguages(userId);
            List<int> user_wants = GetWannaKnownLanguages(userId);

            List<Profile> profiles = db.Profiles.Where(x => x.ApplicationUserID != userId).ToList();
            List<Buddy> buddies = new List<Buddy>();

            foreach ( var profil in profiles )  
            {
                List<int> buddy_knows = GetknowLanguages(profil.ApplicationUserID);
                List<int> buddy_wants = GetWannaKnownLanguages(profil.ApplicationUserID);

                if ( user_knows.Intersect(buddy_knows).Count() == 0 )
                {
                    continue;
                }

                if ( user_knows != null && user_wants != null &&
                     buddy_wants != null && buddy_knows != null 
                ){

                    foreach( var lang_user_wants_id in user_wants )
                    {
                        if ( buddy_knows.Contains( lang_user_wants_id) )
                        {
                            foreach( var lang_buddy_wants_id in buddy_wants )
                            {
                                if ( user_knows.Contains( lang_buddy_wants_id ) )
                                {
                                    buddies.Add(
                                        new Buddy {
                                            Name = profil.LastName,
                                            Description = profil.Description,
                                            WantedLnaguage = db.Languages.Find( lang_user_wants_id ).Name,
                                            Profile = profil
                                        }
                                    );
                                    break;
                                }
                            }
                        }
                    }

                }
            }

            return View( buddies );
        }

        public List<int> GetknowLanguages( string userId )
        {
            return db.Profiles.Find(userId).Languages.Select(x => x.Id).ToList();
        }

        public List<int> GetWannaKnownLanguages( string userId )
        {
            return db.Goals.Where(x => x.ProfileFk == userId).Select(x => x.LanguageFk).ToList();
        }

    }
}