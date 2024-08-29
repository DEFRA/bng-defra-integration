
using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Request;
using Microsoft.Xrm.Sdk;

[assembly: ExcludeFromCodeCoverage]

namespace DEFRA.NE.BNG.Integration.Function.UnitTests
{
    public abstract class TestBase
    {
        protected const string ConnectionString = "TestConnectionString";

        protected Mock<IDataverseService> dataverseService;
        protected Mock<IBlobClientAccess> blobClientAccess;
        protected Mock<IHttpClient> HttpClient;
        protected Mock<IMailServiceAgent> mailService;
        protected Mock<IConfigurationReader> environmentVariableReader;
        protected Mock<IMappingManager> mappingManager;
        protected Mock<INotifyManager> notifyManager;
        protected Mock<IEmailNotificationRequestGenerator> emailNotificationRequestGenerator;

        public TestBase()
        {
            dataverseService = new Mock<IDataverseService>();
            blobClientAccess = new Mock<IBlobClientAccess>();
            HttpClient = new Mock<IHttpClient>();
            mailService = new Mock<IMailServiceAgent>();
            environmentVariableReader = new Mock<IConfigurationReader>();
            mappingManager = new Mock<IMappingManager>();
            notifyManager = new Mock<INotifyManager>();
            emailNotificationRequestGenerator = new Mock<IEmailNotificationRequestGenerator>();
        }

        public static DefraIdAccount CreateTestDefraIdAccount()
        {
            var account = new DefraIdAccount()
            {
                accountid = Guid.NewGuid().ToString(),
                defra_addrregcountryid = "UK",
                name = "Test Account",
                defra_isuk = true,
                defra_type = "Type",
                defra_cmcrn = "23FRGT",
                defra_validatedwithcompanyhouse = true,
                defra_addrregbuildingnumber = "12",
                defra_addrregbuildingname = "Building Name",
                defra_addrregsubbuildingname = "Sub Building Name",
                defra_addrregstreet = "Some Street",
                defra_addrreglocality = "Locality",
                defra_addrregdependentlocality = "Locality",
                defra_addrregcounty = "Some county",
                defra_addrregtown = "Some Town",
                defra_addrregpostcode = "TH8 GRT6",
                defra_addrreginternationalpostalcode = "TH8 GRT6",
                defra_addrcorcountryid = "UK",
                emailaddress1 = "test@test.com",
                telephone1 = "372958623859",
                defra_dateofincorporation = DateTime.Now.ToString(),
                defra_dateofincorporation_hasvalue = true,
                name_hasvalue = true,
                defra_type_hasvalue = true,
                defra_cmcrn_hasvalue = true,
                defra_addrregcountryid_hasvalue = true,
                emailaddress1_hasvalue = true,
                telephone1_hasvalue = true
            };

            return account;
        }

        public static DefraIdContact CreateTestDefraIdContact()
        {
            var contact = new DefraIdContact()
            {
                defra_b2cobjectid = "b2cobjectid",
                firstname = "Jonny",
                firstname_hasvalue = true,
                middlename = "",
                middlename_hasvalue = true,
                lastname = "Bravo",
                lastname_hasvalue = true,
                contactid = Guid.NewGuid().ToString(),
                contactid_hasvalue = true,
                emailaddress1 = "",
                emailaddress1_hasvalue = true,
                birthdate = "1/1/1900",
                birthdate_hasvalue = true,
                telephone1 = "826537639876",
                telephone1_hasvalue = true,
                defra_addrcorbuildingname = "Exciting",
                defra_addrcorlocality = "Local",
                defra_addrcorstreet = "High Street",
                defra_addrcorsubbuildingname = "The Stallion",
                defra_addrcorbuildingnumber = "23",
                defra_addrcortown = "Bristol",
                defra_addrcoruprn = "WER1234",
                defra_addrcorcounty = "Bristol",
                defra_addrcorcountryid = "UK",
                defra_addrcordependentlocality = "locality",
                defra_addrcorinternationalpostalcode = "WE3 8DF",
                defra_addrcorpostcode = "QW3 7GR",
                nationalityid1 = "44FE6514-787A-4D74-A11C-8A61107C34D3",
                nationalityid2 = "78924988-B923-4710-A5D9-535D85EF5664",
                nationalityid3 = "1F308C19-448D-481E-9204-CE34C7A1E619",
                nationalityid4 = "B9421FBC-5E9C-40E4-8AF5-75A777277ED9"
            };

            return contact;
        }

        protected static Entity GenerateTestContactEntity()
        {
            var contact = new Entity("contact", Guid.NewGuid());

            contact["firstname"] = "Jonny";
            contact["surname"] = "Bravo";

            return contact;
        }

        protected static Entity GenerateTestAccountEntity()
        {
            var account = new Entity("account", Guid.NewGuid());

            account["name"] = "Jonny Bravo Inc";

            return account;
        }

        protected static void SetupEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationReplyToEmailId, "testreply@test.com");
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationDoNotReplyToEmailId, "testnoreply@test.com");

            Environment.SetEnvironmentVariable(EnvironmentConstants.environment, "TestEnvironment");
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdCreated, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDeveloperSuccess, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdFail, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdOnHold, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentRequest, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentConfirmation, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentRemainder, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdWithdraw, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdConfirmationOfDocsAndRequestPayment, Guid.NewGuid().ToString());
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdSuccess, Guid.NewGuid().ToString());
        }
    }
}