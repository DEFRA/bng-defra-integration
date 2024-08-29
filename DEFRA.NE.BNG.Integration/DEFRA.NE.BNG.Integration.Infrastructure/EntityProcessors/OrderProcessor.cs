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
    public class OrderProcessor
    {
        /// <summary>
        /// Creates an Organisation in Dataverse
        /// </summary>
        /// <param name="entityToCreate"></param>
        /// <returns></returns>
        public async Task<Guid> Create(Order entityToCreate, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(Create));
            Entity existingOrder = null;

            if (entityToCreate.CreditReference != null)
            {
                var query = DataverseExtensions.GetQuery<SalesOrder>();
                query.AddEqualOperatorCondition<SalesOrder>(nameof(SalesOrder.OrderNumber), entityToCreate.CreditReference);

                existingOrder = await dataverseService.RetrieveFirstRecord<SalesOrder>(query);
            }

            var entity = new SalesOrder
            {
                bng_ponumber = entityToCreate.PurchaseOrderNumber
            };

            if (existingOrder == null)
            {
                entity.OrderNumber = entityToCreate.CreditReference;
            }

            if (entityToCreate.Customer != null)
            {
                entity.CustomerId = entityToCreate.Customer;
            }

            if (entityToCreate.CustomerContact != null)
            {
                entity.bng_CustomerContactID = entityToCreate.CustomerContact;
            }

            if (entityToCreate.OwningTeam != null)
            {
                entity.OwnerId = entityToCreate.OwningTeam;
            }

            entity.bng_DeveloperRegistration = entityToCreate.DevelopmentGuid.GetEntityReference<bng_DeveloperRegistration>();
            entity.bng_Source = bng_source.Online;

            var orderId = await dataverseService.CreateAsync(entity);
            return orderId;
        }

        public async Task<List<Guid>> CreateList(List<Order> entityListToCreate, IDataverseService dataverseService, ILogger logger)
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

        public async Task CreateCdd(Guid orderId, decimal orderTotalAmountThreshold, EntityReference customer, IDataverseService dataverseService, ILogger logger)
        {
            logger.LogDebug("Executing {method}", nameof(CreateCdd));

            var query = DataverseExtensions.GetQuery<SalesOrder>();
            query.AddEqualOperatorCondition<SalesOrder>(nameof(SalesOrder.SalesOrderId), orderId);

            var order = await dataverseService.RetrieveFirstRecord<SalesOrder>(query);

            if (order != null)
            {
                var totalamount = order.GetAttributeValue<Money>("totalamount");

                if (totalamount != null && totalamount.Value >= orderTotalAmountThreshold)
                {
                    var cddEntity = new Entity(bng_CustomerDueDiligenceCheck.EntityLogicalName);
                    cddEntity[DataverseExtensions.AttributeLogicalName<bng_CustomerDueDiligenceCheck>(nameof(bng_CustomerDueDiligenceCheck.bng_ApplicantID))] = customer;
                    cddEntity[DataverseExtensions.AttributeLogicalName<bng_CustomerDueDiligenceCheck>(nameof(bng_CustomerDueDiligenceCheck.bng_OrderID))] = orderId.GetEntityReference<SalesOrder>();
                    cddEntity[DataverseExtensions.AttributeLogicalName<bng_CustomerDueDiligenceCheck>(nameof(bng_CustomerDueDiligenceCheck.bng_CheckType))] = new OptionSetValue(759150000);

                    await dataverseService.CreateAsync(cddEntity);
                }
            }
        }
    }
}