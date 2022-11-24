using Common.Api;
using IntegrationTests.Common.Types;

namespace IntegrationTests.Common.Parts
{
    public class AggregateMapper : IAggregateMapper<Customer, CustomerDataModel>
    {
        /// <inheritdoc />
        public Customer ToAggregate(CustomerDataModel databaseModel)
        {
            var result = new Customer(Guid.Parse(databaseModel.Key)!,
                databaseModel.Name);

            foreach (var order in databaseModel.Orders)
            {
                result.AddOrder(order.Id);
            }

            return result;
        }

        /// <inheritdoc />
        public CustomerDataModel ToDatabaseModel(Customer aggregate)
        {
            var orders = aggregate.Orders.Select(o =>
                new CustomerDataModel.OrderDataModel()
                {
                    Id = o.Id
                }).ToList();

            return new()
            {
                Name = aggregate.Name,
                Orders = orders,
                Key = aggregate.Id.ToString()
            };
        }
    }
}