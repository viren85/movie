using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using SearchLib;

namespace IndexerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("IndexerRole entry point called", "Information");

            while (true)
            {
                //Run once a day
                Trace.TraceInformation("Working", "Information");

                Trace.TraceInformation("Reindexing all entities");

                var builder = IndexBuilder.CreateIndexer();
                builder.IndexAllMovies();
                builder.IndexAllReviews();

                int timeout = 24*60*60*1000;
                Thread.Sleep(timeout);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
