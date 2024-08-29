namespace DEFRA.NE.BNG.Integration.Function.Tests
{
    public class ServiceRegisterTests
    {
        [Fact]
        public void RegisterCustomServices()
        {
            var services = new TestServiceCollection();

            FluentActions.Invoking(() => ServiceRegister.RegisterCustomServices(services))
                         .Should()
                         .NotThrow();

            services.Count.Should().Be(17);
            services.Single(x => x.ServiceType.Name == "IConfigurationReader").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IHttpClient").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IMailServiceAgent").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IEmailNotificationRequestGenerator").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IUkGovNotifyFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IPowerPlatformClient").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IDefraIdSyncFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IBlobClientAccess").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IDeveloperRegistrationFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IDataverseService").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IUKGovNotifyRetriggerFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "ILandOwnerRegistrationFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IOrderFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "INotifyManager").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IMappingManager").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "IGainsiteValidatorFacade").Should().NotBeNull();
            services.Single(x => x.ServiceType.Name == "ICombinedRegistrationFacade").Should().NotBeNull();
        }
    }
}