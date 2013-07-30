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
using DataStoreLib.Models;

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

        public string TestUpdate()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;

            MovieEntity entity = new MovieEntity();
            var rand = new Random((int)DateTimeOffset.UtcNow.Ticks);

            entity.RowKey = entity.MovieId = Guid.NewGuid().ToString();
            entity.ReviewIds = string.Format("{0},{1}", Math.Abs(rand.Next()), Math.Abs(rand.Next()));
            entity.AggregateRating = Math.Abs(rand.Next(10)).ToString();
            entity.Directors = string.Format("Gabbar_{0}", rand.Next());
            entity.HotOrNot = (Math.Abs(rand.Next())%2) == 1 ? true : false;
            entity.MusicDirectors = string.Format("Rahman_{0}", Math.Abs(rand.Next()));
            entity.Name = string.Format("aashique {0}", rand.Next());
            entity.Producers = string.Format("sippy_{0}", rand.Next());
            entity.Actors = string.Format("sahruuk_{0}", rand.Next());

            var reviewIds = entity.GetReviewIds();
            var reviewList = new List<ReviewEntity>();
            foreach (var reviewId in reviewIds)
            {
                var reviewEntity = new ReviewEntity();
                reviewEntity.ReviewId = reviewEntity.RowKey = reviewId;
                reviewEntity.Review = string.Format("This is review number {0}", reviewId);
                reviewEntity.ReviewerName = string.Format("khan_{0}", rand.Next());
                reviewEntity.ReviewerRating = rand.Next(10);
                reviewEntity.SystemRating = rand.Next(10);

                reviewList.Add(reviewEntity);
            }

            TableManager.Instance.UpdateMovieById(entity);
            TableManager.Instance.UpdateReviewsById(reviewList);

            return string.Format("Created movie id {0}", entity.MovieId);

        }
    }
}
