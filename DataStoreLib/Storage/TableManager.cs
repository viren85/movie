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

        protected IDictionary<string, Table> tableList = new ConcurrentDictionary<string, Table>();

        protected static readonly string MovieTableName = "Movie";
        protected static readonly string ReviewTableName = "Review";
 
        protected Table GetTable(string tableName)
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

        public List<Models.MovieEntity> GetMoviesByid(List<string> ids)
        {
            var movieTable = GetTable(MovieTableName);
            var list = movieTable.GetItemsById(ids);
            var movieList = new List<MovieEntity>();

            var rag = new RandomMovieDataGenerator();

            foreach (var id in ids)
            {
                movieList.Add(rag.GetRandomMovieEntity(id));
            }
            return movieList;
        }

        public List<Models.ReviewEntity> GetReviewsById(List<string> ids)
        {
            var reviewTable = GetTable(ReviewTableName);
            var list = reviewTable.GetItemsById(ids);
            var reviewList = new List<ReviewEntity>();

            var rag = new RandomMovieDataGenerator();

            foreach (var id in ids)
            {
                reviewList.Add(rag.GetRandomReview(id));
            }
            return reviewList;
        }

        public List<bool> UpdateMoviesById(List<Models.MovieEntity> movies)
        {
            var movieList = new List<bool>();
            var r = new Random();
            foreach (var movie in movies)
            {
                movieList.Add(r.Next(2) == 1 ? true : false);
            }

            return movieList;
        }

        public List<bool> UpdateReviewsById(List<Models.ReviewEntity> reviews)
        {
            var movieList = new List<bool>();
            var r = new Random();
            foreach (var movie in reviews)
            {
                movieList.Add(r.Next(2) == 1 ? true : false);
            }

            return movieList;
        }
    }
}
