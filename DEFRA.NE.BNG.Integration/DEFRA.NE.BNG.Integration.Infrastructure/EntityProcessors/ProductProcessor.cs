using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors
{
    /// <summary>
    /// Implements IEntityProcessor for an OrganisationProcessor
    /// </summary>
    public class ProductProcessor
    {
        /// <summary>
        /// Creates an Organisation in Dataverse
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Domain.Entities.Product entityToCreate, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));

            Guid creationId = Guid.Empty;
            var productQuery = DataverseExtensions.GetQuery<Domain.Models.Product>();
            productQuery.AddEqualOperatorCondition<Domain.Models.Product>(nameof(Domain.Models.Product.ProductNumber), entityToCreate.Code);

            var matchingProduct = await dataverseService.RetrieveFirstRecord<Domain.Models.Product>(productQuery);
            var uomEntity = await RetrieveUnitOfMeasure(dataverseService);

            if (matchingProduct != null)
            {
                var pricePerUnit = matchingProduct.GetAttributeValue<Money>(
                    DataverseExtensions.AttributeLogicalName<Domain.Models.Product>(nameof(Domain.Models.Product.Price))
                    );


                var orderDetail = new SalesOrderDetail
                {
                    ProductId = matchingProduct.Id.GetEntityReference<Domain.Models.Product>(),
                    SalesOrderId = entityToCreate.OrderGuid.GetEntityReference<SalesOrder>(),
                    Quantity = entityToCreate.Qty,
                    UoMId = uomEntity.Id.GetEntityReference<UoM>()
                };

                creationId = await dataverseService.CreateAsync(orderDetail);

                var orderDetailToUpdate = new SalesOrderDetail();
                orderDetailToUpdate.Id = creationId;
                orderDetailToUpdate.bng_Percentage = entityToCreate.Percentage;

                if (pricePerUnit != null)
                {
                    orderDetailToUpdate.Tax = CalculateTax(pricePerUnit.Value, entityToCreate.Qty, entityToCreate.Percentage);
                }

                await dataverseService.UpdateAsync(orderDetailToUpdate);
            }
            return creationId;
        }

        public async Task<List<Guid>> CreateList(List<Domain.Entities.Product> entityListToCreate, IDataverseService dataverseService, ILogger logger)
        {
            var tasks = entityListToCreate.Select(async x =>
             {
                 var result = await Create(x, dataverseService, logger);
                 return result;
             });

            var tasksArray = await Task.WhenAll(tasks);

            var combinedResults = tasksArray.ToList();
            return combinedResults;
        }

        public static Money CalculateTax(decimal pricePerUnit, decimal quantity, decimal percentage)
        {
            var result = pricePerUnit * quantity * percentage * 0.01M;

            return new Money(result);
        }

        private static async Task<Entity> RetrieveUnitOfMeasure(IDataverseService dataverseService)
        {
            var uomQuery = DataverseExtensions.GetQuery<UoM>();
            uomQuery.AddEqualOperatorCondition<UoM>(nameof(UoM.Name), 1);

            var uomEntity = await dataverseService.RetrieveFirstRecord<UoM>(uomQuery);
            return uomEntity;
        }
    }
}
