using IntegrationTests.Common.Types;
using Jcg.CategorizedRepository.Api;

namespace IntegrationTests.Common.Parts
{
    public class AggregateMapper : IAggregateMapper<Customer, CustomerDataModel>
    {
        /// <inheritdoc />
        public Customer ToAggregate(CustomerDataModel databaseModel)
        {
            var result = new Customer(databaseModel.Id,
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
                Id = aggregate.Id
            };
        }
    }
}