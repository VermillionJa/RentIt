using System;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// Represents a movie that was rented on a Rental Transaction
    /// </summary>
    public class RentalTransactionItems
    {
        /// <summary>
        /// The Id of the Transaction Item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Transaction this item belongs to
        /// </summary>
        public RentalTransaction Transaction { get; set; }

        /// <summary>
        /// The Movie that was rented
        /// </summary>
        public InventoryItem Movie { get; set; }

        /// <summary>
        /// The date the movie must be returned by to avoid penalties
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// The date the movie was returned
        /// </summary>
        public DateTime DateReturned { get; set; }
    }
}