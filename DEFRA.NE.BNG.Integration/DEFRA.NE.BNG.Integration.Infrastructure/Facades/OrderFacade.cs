using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Entities;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;
using DEFRA.NE.BNG.Integration.Infrastructure.EntityProcessors;
using Microsoft.Extensions.Logging;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public class OrderFacade : FacadeBase, IOrderFacade
    {

        private readonly ILogger<OrderFacade> logger;

        public OrderFacade(ILogger<OrderFacade> logger,
                           IConfigurationReader environmentVariableReader,
                           IMailServiceAgent mailService,
                           IDataverseService dataverseService) : base(environmentVariableReader,
                                                                      mailService,
                                                                      dataverseService)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Function facilitate the sequence of methods
        /// </summary>
        /// <param name="order"></param>
        /// <param name="applicantProcessor"></param>
        /// <param name="organisationProcessor"></param>
        /// <param name="developmentProcessor"></param>
        /// <param name="orderProcessor"></param>
        /// <param name="productProcessor"></param>
        /// <param name="fileProcessor"></param>
        /// <returns></returns>
        public async Task<Guid> OrchestrationBNG(Order order, IConfigurationReader environmentVariableReader)
        {
            var orderId = Guid.Empty;
            Guid applicantId = Guid.Empty;

            if (order != null)
            {
                if (order.Organisation != null)
                {
                    var organisationProcessor = new OrganisationProcessor();
                    var organisationId = await organisationProcessor.Create(order.Organisation, dataverseService, logger);

                    order.Customer = organisationId.GetEntityReference<Account>();
                }

                if (order.Applicant != null)
                {
                    await ProcessApplicantNationality(order);
                    applicantId = await SetOrderCustomer(order);
                }

                await CreateOrderDevelopmentRegistration(order, applicantId);

                if (Guid.TryParse(environmentVariableReader.Read(EnvironmentConstants.OrderOwningTeamGuid), out Guid orderOwner))
                {
                    order.OwningTeam = orderOwner.GetEntityReference<Team>();
                }

                var orderTotalAmountThreshold = Convert.ToInt32(environmentVariableReader.Read(EnvironmentConstants.OrderTotalAmountThreshold));

                var orderProcessor = new OrderProcessor();
                orderId = await orderProcessor.Create(order, dataverseService, logger);

                if (order.Products?.Count > 0)
                {
                    await ManageOrderProducts(order, orderId);
                }

                await orderProcessor.CreateCdd(orderId, orderTotalAmountThreshold, order.Customer, dataverseService, logger);

                await dataverseService.CreateAttachments(order.Files, orderId.GetEntityReference<SalesOrder>());
            }
            return orderId;
        }

        public async Task CreateOrderDevelopmentRegistration(Order order, Guid applicantId)
        {

            var planningProcessor = new LocalPlanningAuthorityProcessor();
            var planningAuthorityGuid = Guid.Empty;

            if (order?.Development != null)
            {
                if (order.Development.LocalPlanningAuthority != null)
                {
                    planningAuthorityGuid = await planningProcessor.FindFirst(order.Development.LocalPlanningAuthority, dataverseService, logger);
                }

                var developmentProcessor = new DevelopmentProcessor();
                var developmentTuple = await developmentProcessor.Create(order.Development,
                            null,
                            applicantId.GetEntityReference<Contact>(),
                            planningAuthorityGuid.GetEntityReference<bng_LocalPlanningAuthority>(),
                            dataverseService, logger);

                order.DevelopmentGuid = developmentTuple.Item1;
            }
        }

        private async Task ManageOrderProducts(Order order, Guid orderId)
        {
            var configuration = await dataverseService.RetrieveConfigurations(EnvironmentConstants.BNGConfigurationDetails);
            decimal configurationStandard = 1;
            if (configuration?.Count > 0)
            {
                var standard = configuration[0].bng_Standard;
                if (standard != null)
                {
                    configurationStandard = standard.Value;
                }
            }

            var productProcessor = new ProductProcessor();
            await productProcessor.CreateList(order.Products.Where(x => x.Qty > 0).Select(x =>
            {
                x.OrderGuid = orderId;
                x.Percentage = configurationStandard;
                return x;
            }).ToList(), dataverseService, logger);
        }

        private async Task<Guid> SetOrderCustomer(Order order)
        {
            var applicantProcessor = new ApplicantProcessor();
            var applicantId = await applicantProcessor.Create(order.Applicant, dataverseService, logger);

            if (order.Organisation != null)
            {
                order.CustomerContact = applicantId.GetEntityReference<Contact>();
            }
            else
            {
                order.Customer = applicantId.GetEntityReference<Contact>();
            }

            return applicantId;
        }

        private async Task ProcessApplicantNationality(Order order)
        {
            if (order.Applicant.Nationality?.Count > 0)
            {
                var applicantNationalities = order.Applicant.Nationality
                                    .Select(x => new Nationality { Name = x })
                                    .ToList();

                var nationalityProcessor = new NationalityProcessor();

                order.Applicant.NationalityIdList = await nationalityProcessor.CreateList(applicantNationalities, dataverseService, logger);
            }
        }
    }
}
