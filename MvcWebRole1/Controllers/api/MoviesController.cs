﻿using DataStoreLib.Models;
using DataStoreLib.Storage;
using DataStoreLib.Utils;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MvcWebRole1.Controllers.api
{
    public class MoviesController : BaseController
    {
        // get : api/Movies?type={current/all order by name etc}         
        protected override string ProcessRequest()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            try
            {
                SetConnectionString();

                var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
                if (string.IsNullOrEmpty(qpParams["type"]))
                {
                    throw new ArgumentException("type is not present");
                }

                string type = qpParams["type"].ToString();

                var tableMgr = new TableManager();
                var movies = new List<MovieEntity>();

                if (type.ToLower() == "orderby")
                {
                    movies = tableMgr.GetSortedMoviesByName();
                }
                else if (type.ToLower() == "current")
                {
                    movies = tableMgr.GetCurrentMovies();
                }

                return json.Serialize(movies);                
            }
            catch (Exception ex)
            {
               return  json.Serialize(new { Status = "Error", Message = ex.Message });
            }
        }

        private void SetConnectionString()
        {
            var connectionString = CloudConfigurationManager.GetSetting("StorageTableConnectionString");
            Trace.TraceInformation("Connection str read");
            ConnectionSettingsSingleton.Instance.StorageConnectionString = connectionString;
        }
    }
}
