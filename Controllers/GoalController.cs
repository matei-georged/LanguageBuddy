using LanguageBuddy.Models;
using LanguageBuddy.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageBuddy.Controllers
{
    public class GoalController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<Goal> goals = context.Goals.Where(x => x.Profile.ApplicationUserID == userId).ToList();

            return View(goals);
        }

        [HttpGet]
        public ActionResult Add()
        {
            GoalViewModel goalViewModel = new GoalViewModel
            {
                Profile = context.Profiles.Find(User.Identity.GetUserId()),
                Languages = context.Languages.ToList()
            };

            return View(goalViewModel);
        }

        [HttpPost]
        public ActionResult Add(GoalViewModel addRequest)
        {
            if (ModelState.IsValid)
            {
                Goal toADd = new Goal
                {
                    StartDate = DateTime.Now,
                    LanguageFk = addRequest.SelectedLanguage,
                    ProfileFk = User.Identity.GetUserId()
                };

                context.Goals.Add(toADd);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            GoalViewModel goalViewModel = new GoalViewModel
            {
                Goal = addRequest.Goal,
                Profile = context.Profiles.Find(User.Identity.GetUserId()),
                Languages = context.Languages.ToList()
            };
            return View(goalViewModel);
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            if ( context.Profiles
                .Find( User.Identity.GetUserId() )
                .Goals
                .Where( f => f.Id == id )
                .Count() > 0 
            ) {
                Goal toDelete = context.Goals.Find(id);
                if (toDelete != null)
                {
                    context.Goals.Remove(toDelete);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit( int id )
        {
            Goal toEdit = context.Goals.Find(id);
            if ( toEdit != null )
            {
                GoalViewModel goalViewModel = new GoalViewModel
                {
                    Goal = toEdit,
                    Profile = context.Profiles.Find(User.Identity.GetUserId()),
                    Languages = context.Languages.ToList()
                };
                return View(goalViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPut]
        [Authorize]
        public ActionResult Edit( int id, GoalViewModel updateReq )
        {
            if ( ModelState.IsValid )
            {
                Goal toUpdate = context.Goals.Find(id);
                if ( TryUpdateModel(toUpdate) )
                {
                    toUpdate.LanguageFk = updateReq.Goal.LanguageFk;
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(updateReq);
        }

    }
}