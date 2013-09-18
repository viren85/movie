using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcWebRole1.Controllers
{
    public class SearchApiController : BaseApiController
    {
        protected override string ProcessRequest()
        {
            return "blah blah blah blah" + this.Request.RequestUri.ToString();
        }
    }
}
