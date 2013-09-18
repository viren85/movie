using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcWebRole1.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        // GET api/default1/5
        public string Get()
        {
            // todo :: set the correct media type
            // todo :: return a httpmessage
            var resp = ProcessRequest();

            return resp;
        }

        protected abstract string ProcessRequest();
    }
}
