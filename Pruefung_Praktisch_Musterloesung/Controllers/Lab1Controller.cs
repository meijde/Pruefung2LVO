using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Web.Mvc;
using System.Linq;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab1Controller : Controller
    {
        /**
         * 
         * ANTWORTEN BITTE HIER
         * 
         * 1)  Directory Listing / File Enumeration Attacken
         * 2)  - http://localhost:50374/Lab1/index?type=../images/Lion1.jpg
		 *     - http://localhost:50374/Lab1/index?file=bear1.jpg?type=bears
		 * 
		 * 3.   Je nach Konfiguration des Webservers werden Zugriffe auf Ordner (und nicht Einzeldokumente) akzeptiert oder nicht. Ist der Zugriff auf einen Ordner zugelassen, wird der Inhalt dieses Ornders entsprechend angezeigt und die darin enthaltenen Dokumente als Link dargestellt.
         * 
         * 
         * 
         * */


        public ActionResult Index()
        {
            var type = Request.QueryString["type"];

            if (string.IsNullOrEmpty(type))
            {
                type = "Lion1.jpg";                
            }
            else
            {
                //Falls type nicht etwas vorhandenes ist, soll standard gesetzt werden.
                if (type != "bears" && type != "elephants" && type != "lions")
                {
                    type = "lions"; 
                }
            }

            var path = "~/Content/images/" + type;

            List<List<string>> fileUriList = new List<List<string>>();

            if (Directory.Exists(Server.MapPath(path)))
            {
                var scheme = Request.Url.Scheme; 
                var host = Request.Url.Host; 
                var port = Request.Url.Port;
                
                string[] fileEntries = Directory.GetFiles(Server.MapPath(path));
                foreach (var filepath in fileEntries)
                {
                    var filename = Path.GetFileName(filepath);
                    var imageuri = scheme + "://" + host + ":" + port + path.Replace("~", "") + "/" + filename;

                    var urilistelement = new List<string>();
                    urilistelement.Add(filename);
                    urilistelement.Add(imageuri);
                    urilistelement.Add(type);

                    fileUriList.Add(urilistelement);
                }
            }
            
            return View(fileUriList);
        }

        public ActionResult Detail()
        {
            var file = Request.QueryString["file"];
            var type = Request.QueryString["type"];



            if (string.IsNullOrEmpty(file))
            {
                file = "bears/";
            }
            if (string.IsNullOrEmpty(type))
            {
                file = "bear1.jpg";
            }
            //Falls type nicht etwas vorhandenes ist, soll standard gesetzt werden.
            if (type != "bears" && type != "elephants" && type != "lions")
            {
                type = "lions";
            }

            var relpath = "~/Content/images/" + type + "/" + file;

            List<List<string>> fileUriItem = new List<List<string>>();
            var path = Server.MapPath(relpath);

            if (System.IO.File.Exists(path))
            {
                var scheme = Request.Url.Scheme;
                var host = Request.Url.Host;
                var port = Request.Url.Port;
                var absolutepath = Request.Url.AbsolutePath;

                var filename = Path.GetFileName(file);
                var imageuri = scheme + "://" + host + ":" + port + "/Content/images/" + type + "/" + filename;

                var urilistelement = new List<string>();
                urilistelement.Add(filename);
                urilistelement.Add(imageuri);
                urilistelement.Add(type);

                fileUriItem.Add(urilistelement);
            }
            
            return View(fileUriItem);
        }
    }
}