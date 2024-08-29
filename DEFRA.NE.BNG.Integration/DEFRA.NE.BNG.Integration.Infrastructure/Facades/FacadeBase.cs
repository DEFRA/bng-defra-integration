using DEFRA.NE.BNG.Integration.Domain.Interfaces;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Facades
{
    public abstract class FacadeBase
    {
        protected IMailServiceAgent mailService;
        protected readonly IConfigurationReader environmentVariableReader;
        protected readonly IDataverseService dataverseService;

        protected FacadeBase(IConfigurationReader environmentVariableReader,
                          IMailServiceAgent mailService,
                          IDataverseService dataverseService)
        {
            this.mailService = mailService;
            this.environmentVariableReader = environmentVariableReader;
            this.dataverseService = dataverseService;
        }
    }
}