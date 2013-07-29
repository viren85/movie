using System.Text;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Search(string q)
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;  

            if (string.IsNullOrWhiteSpace(q))
            {
                q = "testsample";
            }
            var movie = TableManager.Instance.GetMovieById(q);

            var resp = new StringBuilder();
            if (movie != null)
            {
                resp.Append("Movie Name : ");
                resp.Append(movie.Name);

                var reviews = movie.GetReviewIds();
                var reviewList = TableManager.Instance.GetReviewsById(reviews);
                foreach (var reviewEntity in reviewList)
                {
                    resp.Append("\r\n With review -- ");
                    resp.Append(reviewEntity.Value.Review);
                }
            }
            else
            {
                resp.Append("No movie found");
            }
            
            ViewBag.Message = "You searched for " + q + "and the response you got was: " + resp;
            
            return View();
        }

        public ActionResult Test()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
            
            string q = "testsample";
            
            var movie = TableManager.Instance.GetMovieById(q);
            @ViewBag.MovieEntity = movie;

            var resp = new StringBuilder();
            if (movie != null)
            {
                resp.Append("Movie Name : ");
                resp.Append(movie.Name);

                var reviews = movie.GetReviewIds();
                var reviewList = TableManager.Instance.GetReviewsById(reviews);
                foreach (var reviewEntity in reviewList)
                {
                    resp.Append("\r\n With review -- ");
                    resp.Append(reviewEntity.Value.Review);
                }
            }
            else
            {
                resp.Append("No movie found");
            }
            
            ViewBag.Message = "You searched for " + q + "and the response you got was: " + resp;
            
            return View();
        }
    }
}
