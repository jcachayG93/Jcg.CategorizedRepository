using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Common.Types
{
    /// <summary>
    /// This is the aggregate, a customer that have zero to many orders. This aggregate is encapsulated with a constuctor,
    /// and the only way to add orders is to use the AddOrder method.
    /// </summary>
    public class Customer
    {
        public Customer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddOrder(Guid orderId)
        {
            var order = new Order(orderId);

            _orders.Add(order);
        }

        private List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders;

        public Guid Id { get; }

        public string Name { get; }

        /// <summary>
        /// The customer Order
        /// </summary>
        public class Order
        {
            public Order(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }

        }
    }
}
