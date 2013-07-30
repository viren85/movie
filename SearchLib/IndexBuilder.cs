using SearchLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib
{
    public class IndexBuilder : IIndexer
    {
        protected IndexBuilder()
        {
            
        }

        public static IIndexer CreateIndexer()
        {
            return new IndexBuilder();
        }

        public void IndexAllMovies()
        {
            throw new NotImplementedException();
        }

        public void IndexAllReviews()
        {
            throw new NotImplementedException();
        }

        public void IndexSelectedMovies(IList<string> movieIds)
        {
            throw new NotImplementedException();
        }

        public void IndexSelectedReviews(IList<string> reviewIds)
        {
            throw new NotImplementedException();
        }
    }
}
