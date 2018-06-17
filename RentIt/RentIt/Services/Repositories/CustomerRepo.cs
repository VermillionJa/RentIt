using Microsoft.EntityFrameworkCore;
using RentIt.Data;
using RentIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.Services.Repositories
{
    /// <summary>
    /// Represents a repository for accessing and manipulating Customer resources
    /// </summary>
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the CustomersRepo class
        /// </summary>
        /// <param name="context">The AppDbContext injected via DI</param>
        public CustomerRepo(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all of the Customers
        /// </summary>
        /// <returns>A collection of Customers</returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.Include(c => c.RentalTransactions).ThenInclude(t => t.Items);
        }

        /// <summary>
        /// Get the Customer with the given Id
        /// </summary>
        /// <param name="id">The Id of the Customer to get</param>
        /// <returns>The Customer with the given Id</returns>
        public Customer GetCustomerById(int id)
        {
            return _context.Customers.Include(c => c.RentalTransactions).ThenInclude(t => t.Items).SingleOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Adds the given Customer to the Repository
        /// </summary>
        /// <param name="customer">The Customer to add</param>
        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates a Customer
        /// </summary>
        /// <param name="customer">The updated Customer data</param>
        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes the Customer with the given Id
        /// </summary>
        /// <param name="id">The Id of the Customer to remove</param>
        public void RemoveCustomer(int id)
        {
            RemoveCustomer(GetCustomerById(id));
        }

        /// <summary>
        /// Removes the given Customer
        /// </summary>
        /// <param name="customer">The Customer to remove</param>
        public void RemoveCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retrieves all of the Rental Transactions
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the Rental Transactions from</param>
        /// <returns>A collection of Rental Transactions</returns>
        public IEnumerable<RentalTransaction> GetAllRentalTransactions(int customerId)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            return customer.RentalTransactions;
        }

        /// <summary>
        /// Get the Rental Transaction with the given Id
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the Rental Transaction from</param>
        /// <param name="id">The Id of the Rental Transaction to get</param>
        /// <returns>The Rental Transaction with the given Id</returns>
        public RentalTransaction GetRentalTransactionById(int customerId, int id)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            return customer.RentalTransactions.SingleOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Adds the given Rental Transaction to the Repository
        /// </summary>
        /// <param name="customerId">The Id of the Customer to add the Rental Transaction to</param>
        /// <param name="transaction">The Rental Transaction to add</param>
        public void AddRentalTransaction(int customerId, RentalTransaction transaction)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            customer.RentalTransactions.Add(transaction);

            UpdateCustomer(customer);
        }

        /// <summary>
        /// Retrieves all of the overdue Items that the Customer has
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the overdue Items of</param>
        /// <returns>A collection of overdue Items</returns>
        public IEnumerable<RentalTransactionItems> GetOverdueItems(int customerId)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            if (customer.RentalTransactions.Any())
            {
                return customer.RentalTransactions.Last().Items.Where(i => i.DueDate.Date < DateTime.Now.Date);
            }
            else
            {
                return new List<RentalTransactionItems>();
            }
        }

        /// <summary>
        /// Retrieves all of the outstanding Late Fees that the Customer has
        /// </summary>
        /// <param name="customerId">The Id of the Customer to get the Late Fees for</param>
        /// <returns>The amount of the outstanding Late Fees</returns>
        public decimal GetOutstandingLateFees(int customerId)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            if (customer.RentalTransactions.Any())
            {
                throw new NotImplementedException();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Marks the Late Fees paid for the given Customer
        /// </summary>
        /// <param name="customerId">The Id of the Customer to mark the Late Fees paid for</param>
        public void MarkLateFeesPaid(int customerId)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            if (customer.RentalTransactions.Any())
            {
                customer.RentalTransactions.Last().LateFeesPaid = true;

                UpdateCustomer(customer);
            }
        }

        /// <summary>
        /// Marks the given Item as returned for the given Customer
        /// </summary>
        /// <param name="customerId">The Id of the Customer to mark the Item returned for</param>
        /// <param name="itemId">The Id of the Item to return</param>
        public void MarkMovieReturned(int customerId, int itemId)
        {
            var customer = GetCustomerById(customerId);

            if (customer == null)
            {
                throw new Exception($"Customer ({customerId}) Not Found");
            }

            if (!customer.RentalTransactions.Any())
            {
                return;
            }

            var item = customer.RentalTransactions.Last().Items.SingleOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                throw new Exception($"Customer ({customerId}) does not have Rental Transaction Item ({itemId}) on their last Transaction");
            }

            item.DateReturned = DateTime.Now;

            UpdateCustomer(customer);
        }
    }
}
