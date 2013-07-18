using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Models
{
    class ITableEntity
    {
        #region table elements
        public string ParitionKey {get; set;}
        public string RowKey {get; set;}

        #endregion
    }
}
