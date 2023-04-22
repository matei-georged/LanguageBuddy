using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageBuddy.Controllers
{
    public class FazanController : Controller
    {
        // GET: Fazan
        public ActionResult Index()
        {
            List<string> wordList = new List<string>();
            wordList = Directory.GetFiles(Server.MapPath("~\\WordLists\\"))
                .Select( f => Path.GetFileName(f) )
                .ToList();

            return View(wordList);        
        }
        
        public ActionResult Game(string language)
        {
            StreamReader reader = new StreamReader(Server.MapPath("~\\WordLists\\"+language));
            List<string> wordList = new List<string>();

            while( !reader.EndOfStream )
            {
                wordList.Add( reader.ReadLine() );
            }

            return View(wordList);
        }

    }
}