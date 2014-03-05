using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcWebRole1.Controllers
{
    public class MovieController : Controller
    {

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
        //
        // GET: /Movie/
        [HttpGet]
        public ActionResult Index(string movieid)
        {
            SetConnectionString();
            return View();
        }

        public ActionResult Reviewer(string name)
        {
            return View();
        }

        public JsonResult AutoCompleteCountry(string term)
        {
            SetConnectionString();

            var tableMgr = new TableManager();
            var movie = tableMgr.SearchMovies(term);

            var users = (from u in movie
                         where u.Name.ToLower().Contains(term.ToLower())
                         select u.Name).Distinct().ToArray();

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
