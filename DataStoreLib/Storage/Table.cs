using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    public class Table : IStore
    {
        protected CloudTable _table;
        protected Table(CloudTable table)
        {
            _table = table;
        }
        
        internal static Table CreateTable(CloudTable table)
        {
            return new Table(table);
        }

        public List<Models.MovieEntity> GetMoviesByid(List<string> ids)
        {
            Debug.Assert(_table != null);

            foreach (var id in ids)
            {
                //var query =
                //    new TableQuery<MovieEntity>().Where();
               

            }

            
            throw new NotImplementedException();
        }

        public List<Models.ReviewEntity> GetReviewsById(List<string> id)
        {
            throw new NotImplementedException();
        }

        public List<bool> UpdateMoviesById(List<Models.MovieEntity> movies)
        {
            throw new NotImplementedException();
        }

        public List<bool> UpdateReviewsById(List<Models.ReviewEntity> reviews)
        {
            throw new NotImplementedException();
        }
    }
}
