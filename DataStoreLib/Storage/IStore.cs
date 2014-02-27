using DataStoreLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Storage
{
    public interface IStore
    {
        IDictionary<string, MovieEntity> GetMoviesByid(List<string> id);
        IDictionary<string, ReviewEntity> GetReviewsById(List<string> id);

        /* added a new method for getting all movies*/
        IDictionary<string, MovieEntity> GetAllMovies();
        /* end */

        IDictionary<MovieEntity, bool> UpdateMoviesById(List<MovieEntity> movies);
        IDictionary<ReviewEntity, bool> UpdateReviewsById(List<ReviewEntity> reviews);
    }

    public static class IStoreHelpers
    {
        public static MovieEntity GetMovieById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetMoviesByid(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static ReviewEntity GetReviewById(this IStore store, string id)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(id));
            var list = new List<string> { id };
            var retList = store.GetReviewsById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static bool UpdateMovieById(this IStore store, MovieEntity movie)
        {
            Debug.Assert( movie != null );
            var list = new List<MovieEntity> { movie };
            var retList = store.UpdateMoviesById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        public static bool UpdateReviewById(this IStore store, ReviewEntity review)
        {
            Debug.Assert(review != null);
            var list = new List<ReviewEntity> { review };
            var retList = store.UpdateReviewsById(list);

            Debug.Assert(retList.Count == 1);
            return retList[retList.Keys.FirstOrDefault()];
        }

        /* Method added by vasim */
        /// <summary>
        /// get list of current running (in theaters) movies
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static List<MovieEntity> GetCurrentMovies(this IStore store)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> currentMovies = new List<MovieEntity>();

            foreach (var currentMovie in retList.Values)
            {
                string currentMonth = DateTime.Now.ToString("MMM");
                string year = DateTime.Now.Year.ToString();

                if (currentMovie.Month == currentMonth && currentMovie.Year == year)
                {
                    currentMovies.Add(currentMovie);
                }
            }

            return currentMovies;
        }

        /// <summary>
        /// search movies and return a list of movies according to search text
        /// </summary>
        /// <param name="store">IStore interface type object</param>
        /// <param name="searchText">search text like name of the movie etc</param>
        /// <returns></returns>
        public static List<MovieEntity> GetMoviesBySearch(this IStore store, string searchText)
        {
            var retList = store.GetAllMovies();
            Debug.Assert(retList.Count == 1);

            List<MovieEntity> currentMovies = new List<MovieEntity>();

            foreach (var currentMovie in retList.Values)
            {
                if (currentMovie.Name.Contains(searchText))
                {
                    currentMovies.Add(currentMovie);
                }
            }

            return currentMovies;
        }
    }
}
