using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services
{
    /// <summary>
    /// A Service for retrieving Pricing information for Rentals and Late Fees
    /// </summary>
    public class PricingLookup
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the PricingLookup class
        /// </summary>
        /// <param name="config">The Site Configuration object injected via DI</param>
        public PricingLookup(IConfiguration config)
        {
            _config = config;
        }


    }
}
