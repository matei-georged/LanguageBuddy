using LanguageBuddy.Models;
using LanguageBuddy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageBuddy.Controllers
{
    public class LanguageController : Controller
    {
        private ApplicationDbContext bd = new ApplicationDbContext();

        public ActionResult Index()
        {

            ICollection<Language> languages = bd.Languages.ToList();

            return View(languages);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create( Language languageRequestt )
        {
            if (ModelState.IsValid)
            {
                bd.Languages.Add(languageRequestt);
                bd.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(languageRequestt);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Language language = bd.Languages.Find(id);

            if ( language != null )
            {
                return View(language);
            }

            return HttpNotFound("Cant find the specific language");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Language languageRequest)
        {   

            try
            {
                if ( ModelState.IsValid )
                {
                    Language toReplace = bd.Languages.Find(id);

                    if( TryUpdateModel( toReplace ) )
                    {
                        toReplace.Name = languageRequest.Name;
                        toReplace.Description = languageRequest.Description;
                        toReplace.ImageUrl = languageRequest.ImageUrl;
                        bd.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }

                return View(languageRequest);
            }
            catch( Exception e )
            {
                return View(languageRequest);
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete( int id )
        {
            Language toDelete = bd.Languages.Find(id);
            if ( toDelete != null )
            {
                bd.Languages.Remove(toDelete);
                bd.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details( int id )
        {
           
            double totalSeconds = (DateTime.Now - DateTime.Parse("2021-08-1")).TotalSeconds;

            List<Tip> tips = bd.Tips
            .Where(f => f.Language.Id == id )
            .ToList()
            .OrderBy(f =>
                Math.Log(Math.Abs(f.Attentions.ToList().Select(d => d.Value).Sum()))
                + Math.Sign(f.Attentions.ToList().Select(d => d.Value).Sum()) * totalSeconds / 4500.0
            )
            .Reverse()
            .ToList();

            List<Profile> profiles = bd.Languages.Find(id).Profiles.ToList();

            LanguageDetailsViewModel mod = new LanguageDetailsViewModel
            {
                Profiles = profiles,
                Tips = tips
            };

            return View(mod);
        }

    }
}