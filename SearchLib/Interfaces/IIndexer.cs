using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib.Interfaces
{
    public interface IIndexer
    {
        void IndexAllMovies();
        void IndexAllReviews();

        void IndexSelectedMovies(IList<string> movieIds);
        void IndexSelectedReviews(IList<string> reviewIds);
    }
}
