using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Models;

namespace DEFRA.NE.BNG.Integration.BusinessLogic.Tests.Constants
{
    public class MappingManagerTests
    {
        private MappingManager systemUnderTest;

        public MappingManagerTests()
        {
            systemUnderTest = new MappingManager();
        }

        [Fact]
        public void MapCreationChoices_LessThanZero()
        {
            var input = -1;

            FluentActions.Invoking(() => systemUnderTest.MapCreationChoices(input))
                         .Should()
                         .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapCreationChoices_0()
        {
            var input = 0;

            var actual = systemUnderTest.MapCreationChoices(input);

            actual.Should().Be(bng_days1to30and30._0);
        }

        [Fact]
        public void MapCreationChoices_31()
        {
            var input = 31;

            var actual = systemUnderTest.MapCreationChoices(input);

            actual.Should().Be(bng_days1to30and30._301);
        }

        [Fact]
        public void MapCreationChoices_17()
        {
            var input = 17;

            var actual = systemUnderTest.MapCreationChoices(input);

            actual.Should().Be(bng_days1to30and30._17);
        }

        [Fact]
        public void MapCreationChoices_1()
        {
            var input = 1;

            var actual = systemUnderTest.MapCreationChoices(input);

            actual.Should().Be(bng_days1to30and30._1);
        }

        [Fact]
        public void MapCreationChoices_30()
        {
            var input = 30;

            var actual = systemUnderTest.MapCreationChoices(input);

            actual.Should().Be(bng_days1to30and30._30);
        }

        [Fact]
        public void MapCreationChoices_LessThan1()
        {
            var input = -1;

            FluentActions.Invoking(() => systemUnderTest.MapCreationChoices(input))
                            .Should()
                            .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapCreationChoices()
        {
            for (var input = 0; input < 31; input++)
            {
                FluentActions.Invoking(() => systemUnderTest.MapCreationChoices(input))
                             .Should()
                             .NotThrow();
            }
        }

        [Fact]
        public void MapHabitatState_InputDoesNotExist()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapHabitatState(input))
                          .Should()
                          .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapHabitatState_Habitat()
        {
            var input = "Habitat";

            var actual = systemUnderTest.MapHabitatState(input);

            actual.Should().Be(bng_habitattypechoices.Area);
        }

        [Fact]
        public void MapHabitatState_Hedge()
        {
            var input = "Hedge";

            var actual = systemUnderTest.MapHabitatState(input);

            actual.Should().Be(bng_habitattypechoices.Hedgerow);
        }

        [Fact]
        public void MapHabitatState_Watercourse()
        {
            var input = "Watercourse";

            var actual = systemUnderTest.MapHabitatState(input);

            actual.Should().Be(bng_habitattypechoices.Watercourses);
        }

        [Fact]
        public void MapHabitatInterventionTypes_InputDoesNotExist()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapHabitatInterventionTypes(input))
                         .Should()
                         .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapHabitatInterventionTypes_Created()
        {
            var input = "Created";

            var actual = systemUnderTest.MapHabitatInterventionTypes(input);

            actual.Should().Be(bng_habitatstatechoices.Creation);
        }

        [Fact]
        public void MapHabitatInterventionTypes_Enhanced()
        {
            var input = "Enhanced";

            var actual = systemUnderTest.MapHabitatInterventionTypes(input);

            actual.Should().Be(bng_habitatstatechoices.Enhanced);
        }

        [Fact]
        public void MapPaymentMethod_InputDoesNotExist()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapPaymentMethod(input))
                         .Should()
                         .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapPaymentMethod_Card()
        {
            var input = "Card";

            var actual = systemUnderTest.MapPaymentMethod(input);

            actual.Should().Be(bng_paymentmethodchoice.Card);
        }

        [Fact]
        public void MapPaymentMethod_BACS()
        {
            var input = "BACS";

            var actual = systemUnderTest.MapPaymentMethod(input);

            actual.Should().Be(bng_paymentmethodchoice.BACS);
        }

        [Fact]
        public void MapHabitatStrategicSignificanceDic_InputDoesNotExist()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapHabitatStrategicSignificanceDic(input))
                        .Should()
                        .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapHabitatStrategicSignificanceDic_InputNull()
        {
            FluentActions.Invoking(() => systemUnderTest.MapHabitatStrategicSignificanceDic(null))
                    .Should()
                    .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapHabitatStrategicSignificanceDic_Formally_identified_in_local_strategy()
        {
            var input = "Formally identified in local strategy";

            var actual = systemUnderTest.MapHabitatStrategicSignificanceDic(input);

            actual.Should().Be(bng_strategicsignificance.Formallyidentifiedinlocalstrategy);
        }

        [Fact]
        public void MapHabitatStrategicSignificanceDic_Location_ecologically_desirable_but_not_in_local_strategy()
        {
            var input = "Location ecologically desirable but not in local strategy";

            var actual = systemUnderTest.MapHabitatStrategicSignificanceDic(input);

            actual.Should().Be(bng_strategicsignificance.Locationecologicallydesirablebutnotinlocalstrategy);
        }

        [Fact]
        public void MapHabitatStrategicSignificanceDic_Areacompensation_not_in_local_strategy_no_local_strategy()
        {
            var input = "Area/compensation not in local strategy/ no local strategy";

            var actual = systemUnderTest.MapHabitatStrategicSignificanceDic(input);

            actual.Should().Be(bng_strategicsignificance.Areacompensationnotinlocalstrategynolocalstrategy);
        }

        [Fact]
        public void MapHabitatConditionDic_EmptyString()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapHabitatConditionDic(input))
                         .Should()
                         .Throw<InvalidDataException>();

        }

        [Fact]
        public void MapHabitatConditionDic_Null()
        {
            FluentActions.Invoking(() => systemUnderTest.MapHabitatConditionDic(null))
                         .Should()
                         .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapHabitatConditionDic_Poor()
        {
            var input = "Poor";

            var actual = systemUnderTest.MapHabitatConditionDic(input);

            actual.Should().Be(bng_habitatconditionchoices.Poor);
        }

        [Fact]
        public void MapHabitatConditionDic_FairlyPoor()
        {
            var input = "Fairly Poor";

            var actual = systemUnderTest.MapHabitatConditionDic(input);

            actual.Should().Be(bng_habitatconditionchoices.FairlyPoor);
        }

        [Fact]
        public void MapHabitatConditionDic_Moderate()
        {
            var input = "Moderate";

            var actual = systemUnderTest.MapHabitatConditionDic(input);

            actual.Should().Be(bng_habitatconditionchoices.Moderate);
        }

        [Fact]
        public void MapHabitatConditionDic_Fairly_Good()
        {
            var input = "Fairly Good";

            var actual = systemUnderTest.MapHabitatConditionDic(input);

            actual.Should().Be(bng_habitatconditionchoices.FairlyGood);
        }

        [Fact]
        public void MapHabitatConditionDic_Good()
        {
            var input = "Good";

            var actual = systemUnderTest.MapHabitatConditionDic(input);

            actual.Should().Be(bng_habitatconditionchoices.Good);
        }

        [Fact]
        public void MapHabitatConditionDic_Condition_Assessment_NA()
        {
            var input = "Condition Assessment N/A";

            var actual = systemUnderTest.MapHabitatConditionDic(input);

            actual.Should().Be(bng_habitatconditionchoices.ConditionAssessmentNA);
        }

        [Fact]
        public void MapExtentOfEncroachment_EmptyString()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapExtentOfEncroachment(input))
                .Should()
                .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapExtentOfEncroachment_Null()
        {
            FluentActions.Invoking(() => systemUnderTest.MapExtentOfEncroachment(null))
                .Should()
                .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapExtentOfEncroachment_NoEncroachment()
        {
            var input = "No Encroachment";

            var actual = systemUnderTest.MapExtentOfEncroachment(input);

            actual.Should().Be(bng_extentofencroachment.NoEncroachment);
        }

        [Fact]
        public void MapExtentOfEncroachment_Minor()
        {
            var input = "Minor";

            var actual = systemUnderTest.MapExtentOfEncroachment(input);

            actual.Should().Be(bng_extentofencroachment.Minor);
        }

        [Fact]
        public void MapExtentOfEncroachment_Major()
        {
            var input = "Major";

            var actual = systemUnderTest.MapExtentOfEncroachment(input);

            actual.Should().Be(bng_extentofencroachment.Major);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_EmptyString()
        {
            var input = "";

            FluentActions.Invoking(() => systemUnderTest.MapExtentOfEncroachmentBothBanks(input))
                        .Should()
                        .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_Null()
        {
            FluentActions.Invoking(() => systemUnderTest.MapExtentOfEncroachmentBothBanks(null))
                    .Should()
                    .Throw<InvalidDataException>();
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_MajorMajor()
        {
            var input = "Major/Major";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.MajorMajor);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_MajorModerate()
        {
            var input = "Major/Moderate";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.MajorModerate);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_MajorMinor()
        {
            var input = "Major/Minor";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.MajorMinor);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_MajorNoEncroachment()
        {
            var input = "Major/No Encroachment";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.MajorNoEncroachment);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_ModerateModerate()
        {
            var input = "Moderate/ Moderate";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.ModerateModerate);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_ModerateMinor()
        {
            var input = "Moderate/ Minor";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.ModerateMinor);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_ModerateNoEncroachment()
        {
            var input = "Moderate/ No Encroachment";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.ModerateNoEncroachment);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_MinorMinor()
        {
            var input = "Minor/ Minor";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.MinorMinor);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_MinorNoEncroachment()
        {
            var input = "Minor/ No Encroachment";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.MinorNoEncroachment);
        }

        [Fact]
        public void MapExtentOfEncroachmentBothBanks_NoEncroachmentNoEncroachment()
        {
            var input = "No Encroachment/ No Encroachment";

            var actual = systemUnderTest.MapExtentOfEncroachmentBothBanks(input);

            actual.Should().Be(bng_extentofencroachmentbothbanks.NoEncroachmentNoEncroachment);
        }

        [Fact]
        public void GetReplyEmailMapping()
        {
            var actual = systemUnderTest.GetReplyEmailMapping();

            actual.Count.Should().Be(2);
        }

        [Fact]
        public void GetActionTemplateMappingForCase()
        {
            var actual = systemUnderTest.GetActionTemplateMappingForCase();

            actual.Count.Should().Be(6);
        }

        [Fact]
        public void CreateNewOrUpdateExisting_NewItem()
        {
            IDictionary<int, string> map = new Dictionary<int, string>();
            map.Add(new KeyValuePair<int, string>(1, "test1"));
            map.Add(new KeyValuePair<int, string>(2, "test2"));
            map.Add(new KeyValuePair<int, string>(3, "test3"));
            map.Add(new KeyValuePair<int, string>(4, "test4"));

            int key = 5;
            string value = "test5";

            systemUnderTest.CreateNewOrUpdateExisting(map, key, value);

            map.Count.Should().Be(5);
            map[5].Should().Be(value);
        }

        [Fact]
        public void CreateNewOrUpdateExisting_UpdateExistingItem()
        {
            IDictionary<int, string> map = new Dictionary<int, string>();
            map.Add(new KeyValuePair<int, string>(1, "test1"));
            map.Add(new KeyValuePair<int, string>(2, "test2"));
            map.Add(new KeyValuePair<int, string>(3, "test3"));
            map.Add(new KeyValuePair<int, string>(4, "test4"));

            int key = 3;
            string value = "testnew3";

            systemUnderTest.CreateNewOrUpdateExisting(map, key, value);

            map.Count.Should().Be(4);
            map[3].Should().Be(value);
        }

        [Fact]
        public void GetOrderTemplateMapping()
        {
            Environment.SetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdOrderCreated, Guid.NewGuid().ToString());
            var key = (int)bng_notifytype.OrderCreated;

            var actual = systemUnderTest.GetOrderTemplateMapping();

            actual.Count.Should().Be(1);
            actual[key].Should().NotBeNullOrEmpty();
        }
    }
}