using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using RentIt.Models.Auth;
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
        private const BasicAuthRole DefaultRole = BasicAuthRole.Employee;

        /// <summary>
        /// Initializes a new instance of the BasicAuthAttribute class using the default Profile
        /// </summary>
        public BasicAuthAttribute() : this(DefaultRole)
        {

        }

        /// <summary>
        /// Initializes a new instance of the BasicAuthAttribute class using the given Profile
        /// </summary>
        /// <param name="role">The Profile to use for validating the Basic Authentication</param>
        public BasicAuthAttribute(BasicAuthRole role) : base(typeof(BasicAuthAttributeImpl))
        {
            Arguments = new[] { role }.Cast<object>().ToArray();
        }

        private class BasicAuthAttributeImpl : IActionFilter
        {
            private const string BasicEncodingFormat = "ISO-8859-1";

            private readonly IEnumerable<BasicAuthProfile> _profiles;
            private readonly BasicAuthRole _role;

            public BasicAuthAttributeImpl(BasicAuthRole role, IConfiguration config)
            {
                _role = role;

                _profiles = config.GetSection("Auth:Profiles").Get<IEnumerable<BasicAuthProfile>>();
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

                        foreach (var profile in _profiles)
                        {
                            if (profile.Username != username)
                            {
                                continue;
                            }
                            
                            if (profile.Password != password)
                            {
                                break;
                            }
                            
                            if (profile.Roles.Contains(_role))
                            {
                                return true;
                            }
                            else
                            {
                                break;
                            }
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
