using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class TableEntity : TableServiceEntity, ITableEntity
    {
        #region table elements
        // none here, implements the base classs
        #endregion

        protected TableEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string ETag { get; set; }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public new DateTimeOffset Timestamp { get; set; }

        public IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            throw new NotImplementedException();
        }
    }
}
