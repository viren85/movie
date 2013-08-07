using System.Diagnostics;
using Lucene.Net.Search;
using SearchLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib.Search
{
    public class IndexQuery : ISearch
    {
        protected string _location;
        protected IndexSearcher _searcher;

        public static IndexQuery GetIndexReader(string location)
        {
            var indexer = new IndexQuery(location);
            indexer.LoadIndex();

            return indexer;
        }

        protected IndexQuery(string indexLocation)
        {
            _location = indexLocation;
        }

        private void LoadIndex()
        {
            
        }

        public void GetAllMoviesWith(string textSearch, out List<string> movies, out List<string> reviews, IDictionary<string, string> filters = null)
        {
            reviews  = new List<string>();
            movies = new List<string>();

            try
            {
                
            }
            catch (Exception err)
            {
               Trace.TraceError("Get all movies failed with exception {0}", err);
                throw;
            }

        }

         
    }
}
