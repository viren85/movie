using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLib.Interfaces
{
    public interface ISearch
    {
        List<string> GetAllMoviesWith(string textSearch, IDictionary<string, string> filters = null);
    }
}
