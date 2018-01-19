using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab3Controller : Controller
    {

        /**
        * 
        * ANTWORTEN BITTE HIER
        * 
        * 1) SQL Injection / Stored XSS
        * 
        * 2) Stored XSS -       Es wird ein Script File in zb einem Kommentar auf einem Blog publiziert. Dieses wird dann in die DB gespeichert.
        *    SQL Injection -    Es wird verscuht SQL in inputfelder der Website einzuschleusen, sodass diese ausgeführt werden in einer DB Abfrage im Code.
        *       --> Es kann alles aus der DB geholt werden, auch sensitive Daten!
        *       --> DB kann abgeschossen werden wenn zb. ein Stern bzw. All eingeschleust wird, sodass die DB soviele Daten schaufeln muss dass sie abstürzt.
        *       
        * 3) 
        * 
        * 
        * 
        * */

        public ActionResult Index() {

            Lab3Postcomments model = new Lab3Postcomments();

            return View(model.getAllData());
        }

        public ActionResult Backend()
        {
            return View();
        }

        [ValidateInput(false)] // -> we allow that html-tags are submitted!
        [HttpPost]
        public ActionResult Comment()
        {
            var comment = Request["comment"];
            var postid = Int32.Parse(Request["postid"]);

            Lab3Postcomments model = new Lab3Postcomments();

            if (model.storeComment(postid, comment))
            {  
                return RedirectToAction("Index", "Lab3");
            }
            else
            {
                ViewBag.message = "Failed to Store Comment";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            Lab3User model = new Lab3User();

            if (model.checkCredentials(username, password))
            {
                return RedirectToAction("Backend", "Lab3");
            }
            else
            {
                ViewBag.message = "Wrong Credentials";
                return View();
            }
        }
    }
}