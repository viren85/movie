using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MvcWebRole1.Controllers.api
{
    public class MovieInfoController : BaseController
    {
        // get : api/MovieInfo?movieId={id}
        protected override string ProcessRequest()
        {
            var qpParams = HttpUtility.ParseQueryString(this.Request.RequestUri.Query);
            if (string.IsNullOrEmpty(qpParams["movieId"]))
            {
                throw new ArgumentException("movieId is not present");
            }
            return @"{
	""movieId"" : ""guid"",
	""poster"" : {
		""height"" : 300,
		""width"" : 200,
		""url"" = ""test""
	},
    ""name"" : ""blah blah"",
	""rating"" : {
		""system"" : 5,
		""critic"" : 6,
		""hot"" : ""no""
	},
	""info"" : {
		""synopsis"" : ""this is a brilliant scary movie"",
		""cast"" : [{
				""name"" : ""ben affleck"",
				""charactername"" : ""mickey"",
				""image"" : {
					""height"" : 300,
					""width"" : 200,
					""url"" = ""test""
				},
				""role"" : ""producer""
			}, {
				""name"" : ""jerry afflect"",
				""charactername"" : ""mouse"",
				""image"" : {
					""height"" : 300,
					""width"" : 200,
					""url"" = ""test""
				},
				""role"" : ""actor""
			}
		],
		""stats"" : {
			""budget"" : ""30,000"",
			""boxoffice"" : ""50000""
		},
		""multimedia"" : {
			""songs"" : [{
					""name"" : ""chaiyya chaiyya"",
					""url"" : ""songtest""
				}, {
					""name"" : ""chaiyya chaiyya"",
					""url"" : ""songtest""
				}
			],
			""trailers"" : [{
					""name"" : ""best movie"",
					""url"" : ""trailertest""
				}, {
					""name"" : ""chaiyya chaiyya"",
					""url"" : ""songtest""
				}
			],
			""pics"" : [{
					""caption"" : ""test caption"",
					""image"" : {
						""height"" : 300,
						""width"" : 200,
						""url"" = ""test""
					}
				}
			]
		}
	}
	""reviews"" : [{
			""name"" : ""khan"",
			""rating"" : {
				""system"" : 5,
				""critic"" : 6,
				""hot"" : ""no""
			},
			""summary"" : ""this is awesome"",
			""outlink"" : ""outlinktest""
		}
	]
}";
        }
    }
}
