using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Models.Auth
{
    /// <summary>
    /// Represents a Profile used for Basic Auth
    /// </summary>
    public class BasicAuthProfile
    {
        /// <summary>
        /// The Username of the user
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The Password for the user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The Roles the user has
        /// </summary>
        public IEnumerable<BasicAuthRole> Roles { get; set; }
    }
}
