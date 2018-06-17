using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// Represents a movie rental transaction
    /// </summary>
    public class RentalTransaction
    {
        /// <summary>
        /// The Id of the Record
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The date the movies were rented on
        /// </summary>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// A collection of movies rented
        /// </summary>
        public List<RentalTransactionItems> Items { get; set; }

        /// <summary>
        /// The total Late Fees paid in this transaction
        /// </summary>
        public decimal LateFeesCharged { get; set; }

        /// <summary>
        /// The total Rental Fees paid in this transaction
        /// </summary>
        public decimal RentalFeesCharged { get; set; }

        /// <summary>
        /// The total amount paid in this transaction
        /// </summary>
        public decimal TotalAmountCharged { get; set; }

        /// <summary>
        /// Have the Late Fees been paid, ignored if no Late Fees were owed
        /// </summary>
        public bool LateFeesPaid { get; set; }
    }
}
