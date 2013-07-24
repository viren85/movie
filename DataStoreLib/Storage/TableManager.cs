using DataStoreLib.Utils;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Storage
{
    public class TableManager
    {
        #region singleton implementation

        protected static TableManager _instance;
        protected static object lockObj = new object();

        protected TableManager()
        {
        }

        public static TableManager Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        _instance = new TableManager();
                    }
                    return _instance;
                }
            }
        }

        #endregion

        protected IDictionary<string, Table> tableList = new ConcurrentDictionary<string, Table>();
 
        public Table GetTable(string tableName)
        {
            Table table = null;
            if (!tableList.ContainsKey(tableName))
            {
                table = CreateTableIfNotExist(tableName);

                if (table == null)
                {
                    throw new ArgumentException(string.Format("failed to create/get table {0}", tableName));
                }
                tableList.Add(tableName, table);
                Trace.TraceInformation("Added {0} to the store", tableName);
            }
            
            Debug.Assert(table != null);
            return table;
        }

        private Table CreateTableIfNotExist(string tableName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(ConnectionSettingsSingleton.Instance.StorageConnectionString));
            var account = CloudStorageAccount.Parse(ConnectionSettingsSingleton.Instance.StorageConnectionString);

            var cloudTableClient = account.CreateCloudTableClient();
            var table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            var intTable = Table.CreateTable(table);
            return intTable;
        }
    }
}
