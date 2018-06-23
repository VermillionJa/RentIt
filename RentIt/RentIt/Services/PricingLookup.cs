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
        private const int DaysInYear = 365;

        private enum MoviePricingCategory { New, Regular, Old };
        
        private readonly IConfiguration _config;

        private readonly double _newCategoryLimitYears;
        private readonly double _regularCategoryLimitYears;

        private readonly Dictionary<MoviePricingCategory, decimal> rentalPricing;
        private readonly Dictionary<MoviePricingCategory, decimal> feePricing;

        /// <summary>
        /// Initializes a new instance of the PricingLookup class
        /// </summary>
        /// <param name="config">The Site Configuration object injected via DI</param>
        public PricingLookup(IConfiguration config)
        {
            _config = config;

            _newCategoryLimitYears = double.Parse(_config["Pricing:CategoryLimits:New"]);
            _regularCategoryLimitYears = double.Parse(_config["Pricing:CategoryLimits:Regular"]);

            rentalPricing = GetPricingTable("Rentals");
            feePricing = GetPricingTable("Fees");
        }

        private Dictionary<MoviePricingCategory, decimal> GetPricingTable(string pricingType)
        {
            return new Dictionary<MoviePricingCategory, decimal>
            {
                { MoviePricingCategory.New, decimal.Parse(_config[$"Pricing:{pricingType}:New"]) },
                { MoviePricingCategory.Regular, decimal.Parse(_config[$"Pricing:{pricingType}:Regular"]) },
                { MoviePricingCategory.Old, decimal.Parse(_config[$"Pricing:{pricingType}:Old"]) }
            };
        }

        public decimal GetRentalPricing(int numberOfDays, DateTime dateMovieReleased)
        {
            var pricingCategory = GetPricingCategory(dateMovieReleased);
            var rentalPricePerDay = rentalPricing[pricingCategory];

            return rentalPricePerDay * numberOfDays;
        }

        public decimal GetFeePricing(int numberOfDaysLate, DateTime dateMovieReleased)
        {
            var pricingCategory = GetPricingCategory(dateMovieReleased);
            var feePricePerDay = feePricing[pricingCategory];

            return feePricePerDay * numberOfDaysLate;
        }

        private MoviePricingCategory GetPricingCategory(DateTime dateMovieReleased)
        {
            var newCategoryLimit = TimeSpan.FromDays(DaysInYear * _newCategoryLimitYears);
            var regularCategoryLimit = TimeSpan.FromDays(DaysInYear * _regularCategoryLimitYears);

            var timeReleased = DateTime.Now.Date - dateMovieReleased.Date;

            var pricingCategory = MoviePricingCategory.Old;

            if (timeReleased <= newCategoryLimit)
            {
                pricingCategory = MoviePricingCategory.New;
            }
            else if (timeReleased <= regularCategoryLimit)
            {
                pricingCategory = MoviePricingCategory.Regular;
            }

            return pricingCategory;
        }
    }
}
