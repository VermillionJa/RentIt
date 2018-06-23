using System;

namespace RentIt.Services
{
    /// <summary>
    /// An Interface representing Service for retrieving Pricing information for Rentals and Late Fees
    /// </summary>
    public interface IPricingLookup
    {
        /// <summary>
        /// Get the total Rental Price for a movie based upon when the movie was released and how many days it will be rented
        /// </summary>
        /// <param name="numberOfDaysToRent">The number of days the movie will be rented</param>
        /// <param name="dateMovieReleased">The date the movie was released</param>
        /// <returns>The total price for renting the movie</returns>
        decimal GetRentalPricing(int numberOfDaysToRent, DateTime dateMovieReleased);

        /// <summary>
        /// Get the total Fees for a movie based upon when the movie was released and how many days late it was returned
        /// </summary>
        /// <param name="numberOfDaysLate">The number of days late the movie was returned</param>
        /// <param name="dateMovieReleased">The date the movie was released</param>
        /// <returns>The total price of late fees the movie</returns>
        decimal GetFeePricing(int numberOfDaysLate, DateTime dateMovieReleased);
    }
}