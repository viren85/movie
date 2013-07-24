using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStoreLib.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Storage
{
    public class Table
    {
        protected CloudTable _table;
        protected Table(CloudTable table)
        {
            _table = table;
        }
        
        internal static Table CreateTable(CloudTable table)
        {
            return new Table(table);
        }

        public List<ITableEntity> GetItemsById(List<string> ids)
        {
            return new List<ITableEntity>();
        }

        public List<bool> UpdateItemsById(List<ITableEntity> movies)
        {
            return new List<bool>{true};
        }
    }
}
