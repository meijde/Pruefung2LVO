using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using Pruefung_Praktisch_Musterloesung.Models;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab2Controller : Controller
    {

        /**
        * 
        * ANTWORTEN BITTE HIER
        * 
        * 1) Session Fixation / XST
        * 
        * 2) Session Fixation: (Eine weitere Attacke ist möglich, wenn Webapplikation „per default“ jedem Benutzer eine Session ID vergeben. Folgendes Szenario ist dann denkbar:)
        *   1. Mallory besucht eine Webseite, welche jedem Benutzer eine Session ID vergibt. Z.B. http://localhost/ und prüft die retournierte Session ID. Z.B. via Set-Cookie-Header SID=0D6441FEA4496C2.
            2. Mallory kann nun Alice eine Email mit folgendem Inhalt senden: "Check out this new cool feature on our bank, http:/localhost:/?SID=0D6441FEA4496C2.“
            3. Alice loggt sich dann ein und die “fixierte” Session-ID 0D6441FEA4496C2 wird aktiv.
            4. Mallory kann dann http://localhost/?SID=0D6441FEA4496C2 besuchen und hat uneingeschränkten Zugriff auf Alice's account.
        * 
        *     XST: 
        *     1. Mallory weiss z.B., dass Bob stets in einem Online-Forum aktiv ist.
              2. Mallory weiss beispielsweise auch, bei welcher Bank Bob ein Konto hat und dass Bob e-Banking betreibt.
              3. Mallory stellt nun folgende Nachricht bzw. folgendes Bild ins Online-Forum, welches Bob’s Bank referenziert.
              4. Hat Bob nun eine aktive Session beim e-Banking, wird beim Ansehen des Bildes durch Bob eine Transaktion von Bobs Account zu Mallory ausgelöst.
        *       --> <img src="ubs.com/wihtdraw?account=lars&amount=10&for=hurni" 
        * 
        * 
        * 
        * */

        public ActionResult Index() {

            var sessionid = Request.QueryString["sid"];

            if (string.IsNullOrEmpty(sessionid))
            {
                var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
                sessionid = string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
            }

            ViewBag.sessionid = sessionid;

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];
            var sessionid = Request.QueryString["sid"];

            // hints:
            //var used_browser = Request.Browser.Platform;
            //var ip = Request.UserHostAddress;

            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkCredentials(username, password))
            {
                model.storeSessionInfos(username, password, sessionid);

                HttpCookie c = new HttpCookie("sid");
                c.Expires = DateTime.Now.AddMonths(2);
                c.Value = sessionid;
                Response.Cookies.Add(c);

                return RedirectToAction("Backend", "Lab2");
            }
            else
            {
                ViewBag.message = "Wrong Credentials";
                return View();
            }
        }

        public ActionResult Backend()
        {
            var sessionid = "";

            if (Request.Cookies.AllKeys.Contains("sid"))
            {
                sessionid = Request.Cookies["sid"].Value.ToString();
            }           

            if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
            {
                sessionid = Request.QueryString["sid"];
            }
            
            // hints:
            //var used_browser = Request.Browser.Platform;
            //var ip = Request.UserHostAddress;

            Lab2Userlogin model = new Lab2Userlogin();

            if (model.checkSessionInfos(sessionid))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Lab2");
            }              
        }
    }
}