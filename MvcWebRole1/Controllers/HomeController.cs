using System.IO;
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
using SearchLib.Builder;
using SearchLib.Search;
using Microsoft.WindowsAzure.ServiceRuntime;

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
            var tableMgr = new TableManager();
            var movie = tableMgr.GetMovieById(q);

            var resp = new StringBuilder();
            if (movie != null)
            {
                resp.Append("Movie Name : ");
                resp.Append(movie.Name);

                var reviews = movie.GetReviewIds();
                var reviewList = tableMgr.GetReviewsById(reviews);
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
            var tableMgr = new TableManager();
            
            var movie = tableMgr.GetMovieById(q);
            @ViewBag.MovieEntity = movie;

            var resp = new StringBuilder();
            if (movie != null)
            {
                resp.Append("Movie Name : ");
                resp.Append(movie.Name);

                var reviews = movie.GetReviewIds();
                var reviewList = tableMgr.GetReviewsById(reviews);
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

            var movie1 = tableMgr.GetCurrentMovies();
            @ViewBag.MovieEntity = movie1;

            var movie3 = tableMgr.GetMoviesBySearch("search text");
            @ViewBag.MovieEntity = movie3;

            TestUpdate();

            return View();
        }

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }

        public string TestUpdate()
        {
            SetConnectionString();

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
            entity.Month = "March";
            entity.Year = "2014";

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

            var tableMgr = new TableManager();
            tableMgr.UpdateMovieById(entity);
            tableMgr.UpdateReviewsById(reviewList);

            return string.Format("Created movie id {0}", entity.MovieId);

        }

        public string TestTobeIndexedTableAdd()
        {
            try
            {
                SetConnectionString();

                var movieTable = TableStore.Instance.GetTable(TableStore.ToBeIndexedTableName) as ToBeIndexedTable;
                var list = new List<string>();

                for (int i = 0; i < 5; i++)
                {
                    list.Add(Guid.NewGuid().ToString());
                }

                var moviesAdded = movieTable.AddMovieToBeIndexed(list);

                list = new List<string>();

                for (int i = 0; i < 5; i++)
                {
                    list.Add(Guid.NewGuid().ToString());
                }

                var reviewsAdded = movieTable.AddReviewToBeIndexed(list);

                var str = new StringBuilder();

                str.Append("movies\r\n");
                foreach (var b in moviesAdded)
                {
                    str.Append((b.Key as ToBeIndexedEntity).EntityId);
                    str.Append(" : ");
                    str.Append(b.Value);
                    str.Append("\r\n");
                }

                str.Append("reviews\r\n");
                foreach (var b in reviewsAdded)
                {
                    str.Append((b.Key as ToBeIndexedEntity).EntityId);
                    str.Append(" : ");
                    str.Append(b.Value);
                    str.Append("\r\n");
                }

                return str.ToString();

            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public string TestToBeIndexedTableDelete()
        {
            try
            {
                SetConnectionString();

                var movieTable = TableStore.Instance.GetTable(TableStore.ToBeIndexedTableName) as ToBeIndexedTable;
                var list = new List<string>();

                for (int i = 0; i < 5; i++)
                {
                    list.Add(Guid.NewGuid().ToString());
                }

                var moviesAdded = movieTable.AddMovieToBeIndexed(list);
                var moviesRemoved = movieTable.IndexedMovies(list);

                list = new List<string>();

                for (int i = 0; i < 5; i++)
                {
                    list.Add(Guid.NewGuid().ToString());
                }

                var reviewsAdded = movieTable.AddReviewToBeIndexed(list);
                var reviewsRemoved = movieTable.IndexedReviews(list);

                var str = new StringBuilder();

                str.Append("movies\r\n");
                foreach (var b in moviesAdded)
                {
                    str.Append((b.Key as ToBeIndexedEntity).EntityId);
                    str.Append(" : ");
                    str.Append(b.Value);
                    str.Append("\r\n");
                }

                str.Append("reviews\r\n");
                foreach (var b in reviewsAdded)
                {
                    str.Append((b.Key as ToBeIndexedEntity).EntityId);
                    str.Append(" : ");
                    str.Append(b.Value);
                    str.Append("\r\n");
                }

                str.Append("movies\r\n");
                foreach (var b in moviesRemoved)
                {
                    str.Append(b.Key);
                    str.Append(" : ");
                    str.Append(b.Value);
                    str.Append("\r\n");
                }

                str.Append("reviews\r\n");
                foreach (var b in reviewsRemoved)
                {
                    str.Append(b.Key);
                    str.Append(" : ");
                    str.Append(b.Value);
                    str.Append("\r\n");
                }



                return str.ToString();

            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public string TestToBeIndexedTableGetAll()
        {
            try
            {
                SetConnectionString();

                var movies = TableStore.Instance.GetTable(TableStore.ToBeIndexedTableName) as ToBeIndexedTable;
                var movieList = movies.GetMoviesTobeIndexed();
                return string.Join(",", movieList.ToArray());
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public string StartIndexing()
        {
            SetConnectionString();
            
            var indexuilder = IndexBuilder.CreateIndexer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indexer"));

            var tobeIndexedTable = TableStore.Instance.GetTable(TableStore.ToBeIndexedTableName) as ToBeIndexedTable;
            
            var movies = tobeIndexedTable.GetMoviesTobeIndexed();
            var reviews = tobeIndexedTable.GetReviewsToBeIndexed();

            indexuilder.IndexSelectedMovies(movies);
            indexuilder.IndexSelectedReviews(reviews);

            return "done in " + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indexer");
        }

        public string UseIndex()
        {
            SetConnectionString();

            var index = IndexQuery.GetIndexReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "indexer"));

            Debug.Assert(index != null);

            var sb = new StringBuilder();
            List<string> movies, reviews = null;
            var query = "aishwarya";
            var filters = new List<string>();
            filters.Add(SearchLib.Constants.Constants.Field_Actors);

            index.GetAllMoviesWith(query, 100, out movies, out reviews);
            sb.Append(string.Format("search for {0} returned {1} movies and {2} reviews -- movies {3} -- reviews {4}\r\n",
                                    query, movies.Count, reviews.Count, string.Join(",", movies.ToArray()),
                                    string.Join(",", reviews.ToArray())));

            query = "shah";
            index.GetAllMoviesWith(query, 100, out movies, out reviews);
            sb.Append(string.Format("search for {0} returned {1} movies and {2} reviews -- movies {3} -- reviews {4}\r\n",
                                    query, movies.Count, reviews.Count, string.Join(",", movies.ToArray()),
                                    string.Join(",", reviews.ToArray())));

            query = "rukh";
            index.GetAllMoviesWith(query, 100, out movies, out reviews);
            sb.Append(string.Format("search for {0} returned {1} movies and {2} reviews -- movies {3} -- reviews {4}\r\n",
                                    query, movies.Count, reviews.Count, string.Join(",", movies.ToArray()),
                                    string.Join(",", reviews.ToArray())));
            filters = new List<string>();
            filters.Add(SearchLib.Constants.Constants.Field_Directors);
            query = "sippy";
            index.GetAllMoviesWith(query, 100, out movies, out reviews);
            sb.Append(string.Format("search for {0} returned {1} movies and {2} reviews -- movies {3} -- reviews {4}\r\n",
                                    query, movies.Count, reviews.Count, string.Join(",", movies.ToArray()),
                                    string.Join(",", reviews.ToArray())));

            filters = new List<string>();
            filters.Add(SearchLib.Constants.Constants.Field_Actors);
            query = "notsippy";
            index.GetAllMoviesWith(query, 100, out movies, out reviews);
            sb.Append(string.Format("search for {0} returned {1} movies and {2} reviews -- movies {3} -- reviews {4}\r\n",
                                    query, movies.Count, reviews.Count, string.Join(",", movies.ToArray()),
                                    string.Join(",", reviews.ToArray())));

            return sb.ToString();
        }
    }
}
