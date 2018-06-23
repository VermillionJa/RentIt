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
    public class PricingLookup : IPricingLookup
    {
        private const int DaysInYear = 365;

        private enum MoviePricingCategory { New, Regular, Old };
        
        private readonly IConfiguration _config;
        
        private readonly Dictionary<MoviePricingCategory, TimeSpan> categoryLimits;

        private readonly Dictionary<MoviePricingCategory, decimal> rentalPricing;
        private readonly Dictionary<MoviePricingCategory, decimal> feePricing;

        /// <summary>
        /// Initializes a new instance of the PricingLookup class
        /// </summary>
        /// <param name="config">The Site Configuration object injected via DI</param>
        public PricingLookup(IConfiguration config)
        {
            _config = config;
            
            categoryLimits = GetCategoryLimitsTable();

            rentalPricing = GetPricingTable("Rentals");
            feePricing = GetPricingTable("Fees");
        }

        /// <summary>
        /// Get the total Rental Price for a movie based upon when the movie was released and how many days it will be rented
        /// </summary>
        /// <param name="numberOfDaysToRent">The number of days the movie will be rented</param>
        /// <param name="dateMovieReleased">The date the movie was released</param>
        /// <returns>The total price for renting the movie</returns>
        public decimal GetRentalPricing(int numberOfDaysToRent, DateTime dateMovieReleased)
        {
            var pricingCategory = GetPricingCategory(dateMovieReleased);
            var rentalPricePerDay = rentalPricing[pricingCategory];

            return rentalPricePerDay * numberOfDaysToRent;
        }

        /// <summary>
        /// Get the total Fees for a movie based upon when the movie was released and how many days late it was returned
        /// </summary>
        /// <param name="numberOfDaysLate">The number of days late the movie was returned</param>
        /// <param name="dateMovieReleased">The date the movie was released</param>
        /// <returns>The total price of late fees the movie</returns>
        public decimal GetFeePricing(int numberOfDaysLate, DateTime dateMovieReleased)
        {
            var pricingCategory = GetPricingCategory(dateMovieReleased);
            var feePricePerDay = feePricing[pricingCategory];

            return feePricePerDay * numberOfDaysLate;
        }

        private Dictionary<MoviePricingCategory, TimeSpan> GetCategoryLimitsTable()
        {
            return new Dictionary<MoviePricingCategory, TimeSpan>
            {
                { MoviePricingCategory.New, GetCategoryLimit(MoviePricingCategory.New) },
                { MoviePricingCategory.Regular, GetCategoryLimit(MoviePricingCategory.Regular) },
                { MoviePricingCategory.Old, GetCategoryLimit(MoviePricingCategory.Old) }
            };
        }

        private TimeSpan GetCategoryLimit(MoviePricingCategory pricingCategory)
        {
            var years = double.Parse(_config[$"Pricing:CategoryLimits:{pricingCategory}"]);

            return TimeSpan.FromDays(DaysInYear * years);
        }

        private Dictionary<MoviePricingCategory, decimal> GetPricingTable(string pricingType)
        {
            return new Dictionary<MoviePricingCategory, decimal>
            {
                { MoviePricingCategory.New, GetPricing(pricingType, MoviePricingCategory.New) },
                { MoviePricingCategory.Regular, GetPricing(pricingType, MoviePricingCategory.Regular) },
                { MoviePricingCategory.Old, GetPricing(pricingType, MoviePricingCategory.Old) }
            };
        }

        private decimal GetPricing(string pricingType, MoviePricingCategory pricingCategory)
        {
            return decimal.Parse(_config[$"Pricing:{pricingType}:{pricingCategory}"]);
        }

        private MoviePricingCategory GetPricingCategory(DateTime dateMovieReleased)
        {
            var timeReleased = DateTime.Now.Date - dateMovieReleased.Date;
            var pricingCategory = MoviePricingCategory.Old;

            if (timeReleased <= categoryLimits[MoviePricingCategory.New])
            {
                pricingCategory = MoviePricingCategory.New;
            }
            else if (timeReleased <= categoryLimits[MoviePricingCategory.Regular])
            {
                pricingCategory = MoviePricingCategory.Regular;
            }

            return pricingCategory;
        }
    }
}
