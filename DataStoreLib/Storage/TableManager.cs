using DataStoreLib.Models;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    public class TableManager : IStore
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

        internal IDictionary<string, Table> tableList = new ConcurrentDictionary<string, Table>();

        protected static readonly string MovieTableName = "Movie";
        protected static readonly string ReviewTableName = "Review";

        internal IDictionary<string, Func<CloudTable, Table>> tableDict =
            new Dictionary<string, Func<CloudTable, Table>>()
                {
                    {MovieTableName, MovieTable.CreateTable},
                    {ReviewTableName, ReviewTable.CreateTable}
                }; 

        internal Table GetTable(string tableName)
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
            table = tableList[tableName];
            
            Debug.Assert(table != null);
            return table;
        }

        internal Table CreateTableIfNotExist(string tableName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(ConnectionSettingsSingleton.Instance.StorageConnectionString));
            var account = CloudStorageAccount.Parse(ConnectionSettingsSingleton.Instance.StorageConnectionString);

            var cloudTableClient = account.CreateCloudTableClient();
            var table = cloudTableClient.GetTableReference(tableName);
            table.CreateIfNotExists();

            return tableDict[tableName](table);
        }

        public IDictionary<string, MovieEntity> GetMoviesByid(List<string> ids)
        {
            var movieTable = GetTable(MovieTableName);
            return movieTable.GetItemsById<MovieEntity>(ids);
        }

        public IDictionary<string, Models.ReviewEntity> GetReviewsById(List<string> ids)
        {
            var reviewTable = GetTable(ReviewTableName);
            return reviewTable.GetItemsById<ReviewEntity>(ids);
        }

        public IDictionary<MovieEntity, bool> UpdateMoviesById(List<Models.MovieEntity> movies)
        {
            var movieTable = GetTable(MovieTableName);
            Debug.Assert(movieTable != null);

            var movieList = new List<DataStoreLib.Models.TableEntity>(movies).ConvertAll(x => (ITableEntity) x);
            var returnOp = movieTable.UpdateItemsById(movieList);

            var returnTranslateOp = new Dictionary<MovieEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as MovieEntity, returnOp[b]);
            }
            return returnTranslateOp;
        }

        public IDictionary<ReviewEntity, bool> UpdateReviewsById(List<Models.ReviewEntity> reviews)
        {
            var reviewTable = GetTable(ReviewTableName);
            Debug.Assert(reviewTable != null);

            var reviewList = new List<DataStoreLib.Models.TableEntity>(reviews).ConvertAll(x => (ITableEntity)x);
            var returnOp = reviewTable.UpdateItemsById(reviewList);

            var returnTranslateOp = new Dictionary<ReviewEntity, bool>();
            foreach (var b in returnOp.Keys)
            {
                returnTranslateOp.Add(b as ReviewEntity, returnOp[b]);
            }
            return returnTranslateOp;
        }
    }
}
