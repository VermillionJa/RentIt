using System.Collections.Generic;
using RentIt.Data.Entities;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents the interface for a repository for accessing and manipulating Customer resources
    /// </summary>
    public interface ICustomerRepo
    {
        /// <summary>
        /// Adds the given Customer to the Repository
        /// </summary>
        /// <param name="customer">The Customer to add</param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Retrieves all of the Customers
        /// </summary>
        /// <returns>A collection of Customers</returns>
        IEnumerable<Customer> GetAllCustomers();

        /// <summary>
        /// Get the Customer with the given Id
        /// </summary>
        /// <param name="id">The Id of the Customer to get</param>
        /// <returns>The Customer with the given Id</returns>
        Customer GetCustomerById(int id);

        /// <summary>
        /// Removes the Customer with the given Id
        /// </summary>
        /// <param name="id">The Id of the Customer to remove</param>
        void RemoveCustomer(int id);

        /// <summary>
        /// Removes the given Customer
        /// </summary>
        /// <param name="customer">The Customer to remove</param>
        void RemoveCustomer(Customer customer);

        /// <summary>
        /// Updates a Customer
        /// </summary>
        /// <param name="customer">The updated Customer data</param>
        void UpdateCustomer(Customer customer);
        
        /// <summary>
        /// Retrieves all of the overdue Items that the Customer has
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the overdue Items of</param>
        /// <returns>A collection of overdue Items</returns>
        IEnumerable<RentalTransactionItems> GetOverdueItems(int customerId);

        /// <summary>
        /// Retrieves all of the outstanding Late Fees that the Customer has
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the Late Fees for</param>
        /// <returns>The amount of the outstanding Late Fees</returns>
        decimal GetOutstandingLateFees(int customerId);

        
        /// <summary>
        /// Retrieves all of the Rental Transactions
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the Rental Transactions from</param>
        /// <returns>A collection of Rental Transactions</returns>
        IEnumerable<RentalTransaction> GetAllRentalTransactions(int customerId);

        /// <summary>
        /// Get the Rental Transaction with the given Id
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the Rental Transaction from</param>
        /// <param name="id">The Id of the Rental Transaction to get</param>
        /// <returns>The Rental Transaction with the given Id</returns>
        RentalTransaction GetRentalTransactionById(int customerId, int id);


        /// <summary>
        /// Adds the given Rental Transaction to the Repository
        /// </summary>
        /// <param name="customerId">The Id of the Customer to add the Rental Transaction to</param>
        /// <param name="transaction">The Rental Transaction to add</param>
        void AddRentalTransaction(int customerId, RentalTransaction transaction);


        /// <summary>
        /// Marks the Late Fees paid for the given Customer
        /// </summary>
        /// <param name="customerId">The Id of the Customer to mark the Late Fees paid for</param>
        void MarkLateFeesPaid(int customerId);

        /// <summary>
        /// Marks the given Item as returned for the given Customer
        /// </summary>
        /// <param name="customerId">The Id of the Customer to mark the Item returned for</param>
        /// <param name="itemId">The Id of the Item to return</param>
        void MarkMovieReturned(int customerId, int itemId);
    }
}