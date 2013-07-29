using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public abstract class TableEntity : TableServiceEntity, ITableEntity
    {
        #region table elements
        // none here, implements the base classs
        #endregion

        protected TableEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string ETag { get; set; }
        public new DateTimeOffset Timestamp { get; set; }

        public virtual void ReadEntity(IDictionary<string, EntityProperty> properties, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            ETag = ReadString(properties, "ETag");
            Timestamp = ReadTimestamp(properties, "Timestamp");
        }

        public IDictionary<string, EntityProperty> WriteEntity(Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        #region typecasts

        internal static string ReadString(IDictionary<string, EntityProperty> properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                return properties[key].StringValue;
            }
            else
            {
                return null;
            }
        }

        internal static Guid ReadGuid(IDictionary<string, EntityProperty> properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].GuidValue;
                if (!val.HasValue)
                {
                    return Guid.Empty;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return Guid.Empty;
            }
        }

        internal static bool ReadBool(IDictionary<string, EntityProperty> properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].BooleanValue;
                if (!val.HasValue)
                {
                    return false;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return false;
            }
        }

        internal static int ReadInt(IDictionary<string, EntityProperty> properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].Int32Value;
                if (!val.HasValue)
                {
                    return 0;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return 0;
            }
        }

        internal static DateTimeOffset ReadTimestamp(IDictionary<string, EntityProperty> properties, string key)
        {
            if (properties.ContainsKey(key))
            {
                var val = properties[key].DateTimeOffsetValue;
                if (!val.HasValue)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return val.Value;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        #endregion
    }
}
