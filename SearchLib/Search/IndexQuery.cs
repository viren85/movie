using System.Diagnostics;
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

        #region singleton

        private static IndexQuery _indexQuery;
        private static object lockObj = new object();

        private IndexQuery()
        {
        }

        public ISearch Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_indexQuery == null)
                    {
                        _indexQuery = new IndexQuery();
                    }

                    return _indexQuery;
                }
            }
        }

        #endregion

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
