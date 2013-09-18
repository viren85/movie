using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcWebRole1.Controllers.api
{
    public abstract class BaseController : ApiController
    {
        public string Get()
        {
            return ProcessRequest();
        }

        protected abstract string ProcessRequest();
    }
}
