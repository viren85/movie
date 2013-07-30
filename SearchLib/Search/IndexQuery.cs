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

        public List<string> GetAllMoviesWith(string textSearch, IDictionary<string, string> filters = null)
        {
            throw new NotImplementedException();
        }
    }
}
