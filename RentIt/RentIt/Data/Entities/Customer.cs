using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Data.Entities
{
    /// <summary>
    /// Represents information regarding a customer who rented a movie
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// The Id of the customer
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The first name of the customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The phone number of the customer
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The street address of the customer
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The additional street address of the customer
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// The city the customer lives in
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The state the customer lives in
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The zip code the customer lives in
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// A collection of Rental Transactions
        /// </summary>
        public List<RentalTransaction> RentalTransactions { get; set; }
    }
}
