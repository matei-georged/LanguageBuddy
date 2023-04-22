using LanguageBuddy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageBuddy.Controllers
{
    public class ProfileController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
            try {
                var tempUserId = User.Identity.GetUserId();
                Profile prof = db.Profiles.Where(spak => spak.ApplicationUserID == tempUserId).First();

                return View(prof);
            }
            catch( Exception e )
            {
                return CreateDummy();
            }
        }

        private ActionResult CreateDummy()
        {

            db.Profiles.Add(
                new Profile
                {
                    ApplicationUserID = User.Identity.GetUserId(),
                    Birthday = DateTime.Now,
                    FirstName = "Little",
                    LastName = "Bird",
                    Description = "Please make this your own "
                }
             );
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit( string id )
        {
            try
            {
                var tempUserId = User.Identity.GetUserId();
                Profile prof = db.Profiles.Where(spak => spak.ApplicationUserID == tempUserId).First();
                prof.Languages = db.Languages.ToList();
                prof.LanguageIdList = new List<string>();

                return View(prof);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult Edit( string id, Profile profileRequest )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Profile toReplace = db.Profiles.Find(id);

                    if (TryUpdateModel(toReplace))
                    {

                        
                        toReplace.FirstName = profileRequest.FirstName;
                        toReplace.Description = profileRequest.Description;
                        toReplace.Birthday = profileRequest.Birthday;
                        toReplace.LastName = profileRequest.LastName;
                        toReplace.Languages.Clear();
                        if (profileRequest.LanguageIdList != null )
                        {
                            List<Language> newLangs = db.Languages.Where(
                                x => profileRequest.LanguageIdList.Contains(x.Id.ToString())
                            ).ToList();

                            foreach (var lang in newLangs)
                            {
                                toReplace.Languages.Add(lang);
                            }
                        }

                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }

                profileRequest.Languages = db.Languages.ToList();
                profileRequest.LanguageIdList = new List<string>();
                return View(profileRequest);
            }
            catch (Exception e)
            {
                return View(profileRequest);
            }
        }
   
    
        public ActionResult GetVisit( string id )
        {
            Profile prof = db.Profiles.Where(f => f.ApplicationUserID == id).ToList().First();

            return View(prof);
        }
    
    }



}