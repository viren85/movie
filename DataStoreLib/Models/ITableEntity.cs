using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.StorageClient;

namespace DataStoreLib.Models
{
    public class TableEntity : TableServiceEntity
    {
        #region table elements
        // none here, implements the base classs
        #endregion

        protected TableEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }
    }
}
