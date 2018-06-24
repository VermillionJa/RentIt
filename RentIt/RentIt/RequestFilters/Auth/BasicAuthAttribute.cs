using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentIt.RequestFilters.Auth
{
    /// <summary>
    /// Represents an MVC Action Attribute for validating the Authorization Request Header using Basic Authentication
    /// </summary>
    public class BasicAuthAttribute : TypeFilterAttribute
    {
        private const string DefaultProfile = "Employee";

        /// <summary>
        /// Initializes a new instance of the BasicAuthAttribute class using the default Profile
        /// </summary>
        public BasicAuthAttribute() : base(typeof(BasicAuthAttributeImpl))
        {
            Arguments = new[] { DefaultProfile };
        }

        /// <summary>
        /// Initializes a new instance of the BasicAuthAttribute class using the given Profile
        /// </summary>
        /// <param name="profile">The Profile to use for validating the Basic Authentication</param>
        public BasicAuthAttribute(string profile) : base(typeof(BasicAuthAttributeImpl))
        {
            Arguments = new[] { profile };
        }

        private class BasicAuthAttributeImpl : IActionFilter
        {
            private const string BasicEncodingFormat = "ISO-8859-1";

            private readonly string _username;
            private readonly string _password;

            public BasicAuthAttributeImpl(string profile, IConfiguration config)
            {
                _username = config[$"Auth:{profile}:Username"];
                _password = config[$"Auth:{profile}:Password"];
            }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (!CheckIfAuthorized(context))
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Result = new UnauthorizedResult();
                }
            }

            private bool CheckIfAuthorized(ActionExecutingContext context)
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];

                if (authHeader?.ToUpper().StartsWith("BASIC") == true)
                {
                    var encodedCredentials = authHeader.Replace("Basic", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
                    var encoding = Encoding.GetEncoding(BasicEncodingFormat);
                    var rawCredentials = encoding.GetString(Convert.FromBase64String(encodedCredentials));

                    var credentials = rawCredentials.Split(':', StringSplitOptions.RemoveEmptyEntries);

                    if (credentials.Length == 2)
                    {
                        var username = credentials[0];
                        var password = credentials[1];

                        if (username == _username && password == _password)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                
            }
        }
    }
}
