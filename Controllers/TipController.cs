using LanguageBuddy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageBuddy.Controllers
{
    public class TipController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            double totalSeconds = (DateTime.Now - DateTime.Parse("2021-08-1")).TotalSeconds;

            List<Tip> tips = db.Tips.ToList().OrderBy(f => 
                Math.Log( Math.Abs( f.Attentions.ToList().Select(d => d.Value).Sum() ) ) 
                + Math.Sign( f.Attentions.ToList().Select(d => d.Value).Sum() ) * totalSeconds / 4500.0
            ).Reverse().ToList();

            return View(tips);
        }

        [HttpGet]
        public ActionResult Create()
        {

            IEnumerable<SelectListItem> languages = db.Languages.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
            );

            ViewBag.Languages = languages;
            return View();
        }

        [HttpPost]
        public ActionResult Create( Tip toAdd )
        {
            if (toAdd != null)
            {
                toAdd.ProfileFk = User.Identity.GetUserId();
                if (ModelState.IsValid)
                {
                    db.Tips.Add(toAdd);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            IEnumerable<SelectListItem> languages = db.Languages.ToList().Select(
                x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
            );

            ViewBag.Languages = languages;

            return View(toAdd);
        }

        public ActionResult Delete(int id)
        {
            Tip toDelete = db.Tips.Find(id);
            if ( toDelete != null )
            {
                db.Tips.Remove(toDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit( int id )
        {
            Tip toEdit = db.Tips.Find(id);
            if (toEdit != null)
            {

                IEnumerable<SelectListItem> languages = db.Languages.ToList().Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }
                );

                ViewBag.Languages = languages;

                return View(toEdit);
            }

            return RedirectToAction("Index");
        }

        [HttpPut]
        public ActionResult Edit( Tip tipRequest )
        {
            if ( ModelState.IsValid )
            {
                Tip toUpdate = db.Tips.Find(tipRequest.Id);
                if ( TryUpdateModel(toUpdate) )
                {
                    toUpdate.LanguageId = tipRequest.LanguageId;
                    toUpdate.Description = tipRequest.Description;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            IEnumerable<SelectListItem> languages = db.Languages.ToList().Select(
                    x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }
                );

            ViewBag.Languages = languages;
            return View(tipRequest);
        }

        public void GiveAtention(int id, int value)
        {
            if (value == 1 || value == -1)
            {
                string Idem = User.Identity.GetUserId();
                Tip toAttent = db.Tips.Find( id );
                
                if ( toAttent != null )
                {
                    if ( toAttent
                        .Attentions
                        .Where(f => f.ProfileFk == Idem)
                        .ToList().Count() == 0
                    ) {
                        db.Atentions.Add(
                            new Atention
                            {
                                Value = value,
                                TipFk = id,
                                ProfileFk = Idem
                            }
                        );
                        db.SaveChanges();
                    }
                }
            }

        }

    }

}