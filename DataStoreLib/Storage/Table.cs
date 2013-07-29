using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    internal abstract class Table
    {
        protected CloudTable _table;
        protected Table(CloudTable table)
        {
            _table = table;
        }

        public IDictionary<string, TEntity> GetItemsById<TEntity>(List<string> ids) where TEntity : DataStoreLib.Models.TableEntity
        {
            Debug.Assert(ids.Count != 0);
            Debug.Assert(_table != null);

            var operationList = new Dictionary<string, TableResult>();
            foreach (var id in ids)
            {
                operationList[id] = _table.Execute(TableOperation.Retrieve<TEntity>(GetParitionKey(), id));
            }
            
            var returnDict = new Dictionary<string, TEntity>();
            int iter = 0;
            foreach (var tableResult in operationList)
            {
                TEntity entity = null;
                if (tableResult.Value.HttpStatusCode != (int)HttpStatusCode.OK)
                {
                    Trace.TraceWarning("Couldn't retrieve info for id {0}", ids[iter]);
                }
                else
                {
                    entity = tableResult.Value.Result as TEntity;
                }
                returnDict.Add(ids[iter], entity);
                iter++;
            }

            return returnDict;
        }

        public List<bool> UpdateItemsById(List<ITableEntity> movies)
        {
            return new List<bool>{true};
        }

        protected abstract string GetParitionKey();
    }

    internal class MovieTable : Table
    {
        protected MovieTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new MovieTable(table);
        }

        protected override string GetParitionKey()
        {
            return MovieEntity.PARTITION_KEY;
        }
    }

    internal class ReviewTable : Table
    {
        protected ReviewTable(CloudTable table)
            : base(table)
        {
        }

        internal static Table CreateTable(CloudTable table)
        {
            return new ReviewTable(table);
        }

        protected override string GetParitionKey()
        {
            return ReviewEntity.PARTITION_KEY;
        }
    }
}
