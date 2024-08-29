using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Xrm.Sdk;

namespace DEFRA.NE.BNG.Integration.Domain.UnitTests.Entities
{
    public class DataverseExtensionsTests
    {
        [Fact]
        public void GetAttributeLogicalName_PropertyDoesNotExists()
        {
            FluentActions.Invoking(() => DataverseExtensions.AttributeLogicalName<Account>("Does not exists"))
                         .Should()
                         .Throw<InvalidOperationException>()
                         .WithMessage("Attribute not found!");
        }

        [Fact]
        public void GetAttributeLogicalName_PropertyExists_NoAttribute()
        {
            FluentActions.Invoking(() => DataverseExtensions.AttributeLogicalName<Entity>("Id"))
                         .Should()
                         .Throw<InvalidOperationException>()
                         .WithMessage("Attribute not found!");
        }

        [Fact]
        public void GetAttributeLogicalName_Account_bng_DefraID()
        {
            var actual = DataverseExtensions.AttributeLogicalName<Account>(nameof(Account.bng_DefraID));

            actual.Should().Be("bng_defraid");
        }

        [Fact]
        public void GetAttributeLogicalName_bng_Case_bng_IsConfirmDocsReceivedAndRequestPayment()
        {
            var actual = DataverseExtensions.AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_IsConfirmDocsReceivedAndRequestPayment));

            actual.Should().Be("bng_isconfirmdocsreceivedandrequestpayment");
        }

        [Fact]
        public void GetQuery_bng_LocalPlanningAuthority()
        {
            var actual = DataverseExtensions.GetQuery<bng_LocalPlanningAuthority>();

            actual.EntityName.Should().Be(bng_LocalPlanningAuthority.EntityLogicalName);
            actual.ColumnSet.Columns.Count.Should().Be(4);
        }

        [Fact]
        public void GetQuery_Contact()
        {
            var actual = DataverseExtensions.GetQuery<Contact>();

            actual.EntityName.Should().Be(Contact.EntityLogicalName);
        }

        [Fact]
        public void GetQuery_bng_BankDetails()
        {
            var actual = DataverseExtensions.GetQuery<bng_BankDetails>();

            actual.EntityName.Should().Be(bng_BankDetails.EntityLogicalName);
        }

        [Fact]
        public void GetQuery_Account()
        {
            var actual = DataverseExtensions.GetQuery<Account>();

            actual.EntityName.Should().Be(Account.EntityLogicalName);
        }

        [Fact]
        public void GetQuery_bng_BNGConfiguration()
        {
            var actual = DataverseExtensions.GetQuery<bng_BNGConfiguration>();

            actual.EntityName.Should().Be(bng_BNGConfiguration.EntityLogicalName);
        }

        [Fact]
        public void GetQuery_SalesOrder()
        {
            var actual = DataverseExtensions.GetQuery<SalesOrder>();

            actual.EntityName.Should().Be(SalesOrder.EntityLogicalName);
        }

        [Fact]
        public void GetQuery_FakeEntity()
        {
            FluentActions.Invoking(() => DataverseExtensions.GetQuery<FakeEntity>())
                         .Should()
                         .Throw<NotSupportedException>()
                         .WithMessage($"Type {typeof(FakeEntity).Name} is not supported!");
        }

        [Fact]
        public void GetEntityReference_bng_LocalPlanningAuthority()
        {
            var systemUnderTest = Guid.NewGuid();

            var actual = systemUnderTest.GetEntityReference<bng_LocalPlanningAuthority>();

            actual.LogicalName.Should().Be(bng_LocalPlanningAuthority.EntityLogicalName);
            actual.Id.Should().Be(systemUnderTest);
        }

        [Fact]
        public void GetEntityReference_bng_Case()
        {
            var systemUnderTest = Guid.NewGuid();

            var actual = systemUnderTest.GetEntityReference<bng_Case>();

            actual.LogicalName.Should().Be(bng_Case.EntityLogicalName);
            actual.Id.Should().Be(systemUnderTest);
        }

        [Fact]
        public void GetEntityReference_bng_BNGConfiguration()
        {
            var systemUnderTest = Guid.NewGuid();

            var actual = systemUnderTest.GetEntityReference<bng_BNGConfiguration>();

            actual.LogicalName.Should().Be(bng_BNGConfiguration.EntityLogicalName);
            actual.Id.Should().Be(systemUnderTest);
        }

        [Fact]
        public void GetEntityReference_bng_BaselineHabitat()
        {
            var systemUnderTest = Guid.NewGuid();

            var actual = systemUnderTest.GetEntityReference<bng_BaselineHabitat>();

            actual.LogicalName.Should().Be(bng_BaselineHabitat.EntityLogicalName);
            actual.Id.Should().Be(systemUnderTest);
        }

        [Fact]
        public void GetGetRelationshipSchemaName_PropertyDoesNotExist()
        {
            FluentActions.Invoking(() => DataverseExtensions.GetRelationshipSchemaName<bng_DeveloperRegistration>("Does not exists"))
                         .Should()
                         .Throw<InvalidOperationException>()
                         .WithMessage("Relationship not found!");
        }

        [Fact]
        public void GetGetRelationshipSchemaName_PropertyExist_NoAttribute()
        {
            FluentActions.Invoking(() => DataverseExtensions.GetRelationshipSchemaName<bng_DeveloperRegistration>("bng_AreaUnitChange"))
                         .Should()
                         .Throw<InvalidOperationException>()
                         .WithMessage("Relationship not found!");
        }

        [Fact]
        public void GetGetRelationshipSchemaName_bng_DeveloperRegistration_bng_DeveloperRegistration_GainSiteReg()
        {
            var actual = DataverseExtensions.GetRelationshipSchemaName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_DeveloperRegistration_GainSiteReg));

            actual.Should().Be("bng_DeveloperRegistration_GainSiteReg");
        }

        [Fact]
        public void AddEqualOperatorCondition()
        {
            var query = new QueryExpression(bng_Case.EntityLogicalName);

            query.AddEqualOperatorCondition<bng_Case>(nameof(bng_Case.bng_Case_Type), bng_casetype.Combined);

            var condition = query.Criteria.Conditions[0];
            condition.Operator.Should().Be(ConditionOperator.Equal);
            var value = (bng_casetype)condition.Values[0];
            value.Should().Be(bng_casetype.Combined);
        }

        [Fact]
        public void AddTableColumns()
        {
            var columnSet = new ColumnSet()
                            .AddTableColumns<bng_Case>(
                                                    [
                                                        nameof(bng_Case.bng_DeveloperRegistrationId),
                                                        nameof(bng_Case.bng_GainSiteRegistrationID),
                                                        nameof(bng_Case.bng_RejectionReasonID),
                                                        nameof(bng_Case.bng_Case_Type),
                                                        nameof(bng_Case.bng_Deadline),
                                                        nameof(bng_Case.bng_InternalJustification),
                                                        nameof(bng_Case.bng_NoticeofIntent),
                                                        nameof(bng_Case.bng_withdrawalreason),
                                                        nameof(bng_Case.bng_Informationtobeamended),
                                                        nameof(bng_Case.bng_InformationnotAmended),
                                                        nameof(bng_Case.bng_AmendmentReason)
                                                    ]
                                                );

            columnSet.Columns.Count.Should().Be(11);
        }
    }
}