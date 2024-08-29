using DEFRA.NE.BNG.Integration.Domain.Constants;
using DEFRA.NE.BNG.Integration.Domain.Interfaces;
using DEFRA.NE.BNG.Integration.Domain.Models;

namespace DEFRA.NE.BNG.Integration.Domain.Entities
{
    public class MappingManager : IMappingManager
    {
        private readonly Dictionary<int, int> creationChoiceDictionary = new()
        {
                 { 0,759150000},
                   {1,759150001},
                   { 2,759150002},
                   { 3,759150003},
                   { 4,759150004},
                   { 5,759150005},
                   { 6, 759150006},
                   { 7, 759150007},
                   { 8, 759150008},
                   { 9, 759150009},
                   { 10, 759150010},
                   { 11,759150011},
                   { 12,759150012},
                   { 13, 759150013},
                   { 14, 759150014},
                   { 15,759150015},
                   { 16, 759150016},
                   { 17, 759150017},
                   { 18,759150018},
                   { 19,759150019},
                   { 20,759150020},
                   { 21,759150021},
                   { 22,759150022},
                   { 23,759150023},
                   { 24,759150024},
                   { 25,759150025},
                   { 26,759150026},
                   { 27,759150027},
                   { 28,759150028},
                   { 29,759150029},
                   { 30,759150030},
            };
        public Dictionary<int, string> GetReplyEmailMapping()
        {
            var result = new Dictionary<int, string>
                    {
                        {1, Environment.GetEnvironmentVariable(EnvironmentConstants. NotificationReplyToEmailId) },
                        {2, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationDoNotReplyToEmailId) }
                    };
            return result;
        }

        public Dictionary<bng_casetype, IDictionary<int, string>> GetActionTemplateMappingForCase()
        {
            var result = new Dictionary<bng_casetype, IDictionary<int, string>>
                {
                    {bng_casetype.Registration,  GetLandOwnerActionTemplateMapping()},
                    {bng_casetype.Allocation,   GetDeveloperActionTemplateMapping()},
                    {bng_casetype.Combined,   GetActionTemplateMapping()},
                    {bng_casetype.AmendGainSite,   GetAmendmentActionTemplateMapping()},
                    {bng_casetype.AmendAllocation,   GetAmendmentActionTemplateMapping()},
                    {bng_casetype.Removals,   GetAmendmentActionTemplateMapping()}
                };
            return result;
        }


        public Dictionary<int, string> GetOrderTemplateMapping()
        {
            var result = new Dictionary<int, string>
                {
                    {(int)bng_notifytype.OrderCreated,Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdOrderCreated)},
                };
            return result;

        }

        public bng_habitatconditionchoices MapHabitatConditionDic(string input)
        {
            bng_habitatconditionchoices optionSet;

            switch (input)
            {
                case "Poor":
                    optionSet = bng_habitatconditionchoices.Poor;
                    break;

                case "Fairly Poor":
                    optionSet = bng_habitatconditionchoices.FairlyPoor;
                    break;

                case "Moderate":
                    optionSet = bng_habitatconditionchoices.Moderate;
                    break;

                case "Fairly Good":
                    optionSet = bng_habitatconditionchoices.FairlyGood;
                    break;

                case "Good":
                    optionSet = bng_habitatconditionchoices.Good;
                    break;

                case "Condition Assessment N/A":
                    optionSet = bng_habitatconditionchoices.ConditionAssessmentNA;
                    break;

                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public bng_strategicsignificance MapHabitatStrategicSignificanceDic(string input)
        {
            bng_strategicsignificance optionSet;

            switch (input)
            {
                case "Formally identified in local strategy":
                    optionSet = bng_strategicsignificance.Formallyidentifiedinlocalstrategy;
                    break;

                case "Location ecologically desirable but not in local strategy":
                    optionSet = bng_strategicsignificance.Locationecologicallydesirablebutnotinlocalstrategy;
                    break;

                case "Area/compensation not in local strategy/ no local strategy":
                    optionSet = bng_strategicsignificance.Areacompensationnotinlocalstrategynolocalstrategy;
                    break;
                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public bng_paymentmethodchoice MapPaymentMethod(string input)
        {
            bng_paymentmethodchoice optionSet;
            switch (input)
            {
                case "Card":
                    optionSet = bng_paymentmethodchoice.Card;
                    break;

                case "BACS":
                    optionSet = bng_paymentmethodchoice.BACS;
                    break;
                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public bng_days1to30and30 MapCreationChoices(int input)
        {
            if (!creationChoiceDictionary.TryGetValue(input, out int result))
            {
                result = -1;
            }


            if (result > 0)
            {
                return (bng_days1to30and30)result;
            }
            else if (input > 30)
            {
                return bng_days1to30and30._301;
            }
            else
            {
                throw new InvalidDataException($"Supplied input {input} is not supported!");
            }
        }

        public bng_habitattypechoices MapHabitatState(string input)
        {
            bng_habitattypechoices optionSet;

            switch (input)
            {
                case "Habitat":
                    optionSet = bng_habitattypechoices.Area;
                    break;

                case "Hedge":
                    optionSet = bng_habitattypechoices.Hedgerow;
                    break;

                case "Watercourse":
                    optionSet = bng_habitattypechoices.Watercourses;
                    break;
                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public bng_habitatstatechoices MapHabitatInterventionTypes(string input)
        {
            bng_habitatstatechoices optionSet;

            switch (input)
            {
                case "Created":
                    optionSet = bng_habitatstatechoices.Creation;
                    break;

                case "Enhanced":
                    optionSet = bng_habitatstatechoices.Enhanced;
                    break;
                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public bng_extentofencroachmentbothbanks MapExtentOfEncroachmentBothBanks(string input)
        {
            bng_extentofencroachmentbothbanks optionSet;

            switch (input)
            {
                case "Major/Major":
                    optionSet = bng_extentofencroachmentbothbanks.MajorMajor;
                    break;

                case "Major/Moderate":
                    optionSet = bng_extentofencroachmentbothbanks.MajorModerate;
                    break;

                case "Major/Minor":
                    optionSet = bng_extentofencroachmentbothbanks.MajorMinor;
                    break;

                case "Major/No Encroachment":
                    optionSet = bng_extentofencroachmentbothbanks.MajorNoEncroachment;
                    break;

                case "Moderate/ Moderate":
                    optionSet = bng_extentofencroachmentbothbanks.ModerateModerate;
                    break;

                case "Moderate/ Minor":
                    optionSet = bng_extentofencroachmentbothbanks.ModerateMinor;
                    break;

                case "Moderate/ No Encroachment":
                    optionSet = bng_extentofencroachmentbothbanks.ModerateNoEncroachment;
                    break;

                case "Minor/ Minor":
                    optionSet = bng_extentofencroachmentbothbanks.MinorMinor;
                    break;

                case "Minor/ No Encroachment":
                    optionSet = bng_extentofencroachmentbothbanks.MinorNoEncroachment;
                    break;

                case "No Encroachment/ No Encroachment":
                    optionSet = bng_extentofencroachmentbothbanks.NoEncroachmentNoEncroachment;
                    break;
                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public bng_extentofencroachment MapExtentOfEncroachment(string input)
        {
            bng_extentofencroachment optionSet;

            switch (input)
            {
                case "No Encroachment":
                    optionSet = bng_extentofencroachment.NoEncroachment;
                    break;

                case "Minor":
                    optionSet = bng_extentofencroachment.Minor;
                    break;

                case "Major":
                    optionSet = bng_extentofencroachment.Major;
                    break;
                default:
                    throw new InvalidDataException($"Supplied input {input} is not supported!");
            }

            return optionSet;
        }

        public void CreateNewOrUpdateExisting<TKey, TValue>(IDictionary<TKey, TValue> map, TKey key, TValue value)
        {
            if (map.ContainsKey(key))
            {
                map[key] = value;
            }
            else
            {
                map.Add(key, value);
            }
        }

        private static Dictionary<int, string> GetLandOwnerActionTemplateMapping()
        {
            var result = GetCommonTemplateMapping();
            result.Add((int)bng_notifytype.Accepted,
                        Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdSuccess)
                    );
            return result;
        }

        private static Dictionary<int, string> GetDeveloperActionTemplateMapping()
        {
            var result = GetCommonTemplateMapping();
            result.Add((int)bng_notifytype.Accepted,
                        Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDeveloperSuccess)
                        );
            return result;
        }

        private static Dictionary<int, string> GetActionTemplateMapping()
        {
            var result = GetCommonTemplateMapping();
            result.Add((int)bng_notifytype.Accepted, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdCombinedSuccess));
            result.Add((int)bng_notifytype.FullAcceptance, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdAmendmentFullSuccess));
            result.Add((int)bng_notifytype.PartialAcceptance, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdAmendmentPartialSuccess));
            return result;
        }

        private static Dictionary<int, string> GetAmendmentActionTemplateMapping()
        {
            Dictionary<int, string> amenment = GetActionTemplateMapping();
            amenment.Add((int)bng_notifytype.ProceedwithAmendment, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateProceedwithAmendment));
            amenment.Add((int)bng_notifytype.Closed, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateCloseAmendment));
            amenment.Add((int)bng_notifytype.InternalAmendmentConfirmation, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdInternalAmendmentConfirmation));
            amenment.Add((int)bng_notifytype.RemovalCaseAccepted, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdRemovalCaseAccepted));
            amenment.Add((int)bng_notifytype.AmendmentAllocationCaseAccepted, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdAmendmentAllocationCaseAccepted));

            return amenment;
        }

        private static Dictionary<int, string> GetCommonTemplateMapping()
        {
            var result = new Dictionary<int, string>
                {
                    {(int)bng_notifytype.Created,Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdCreated)},
                    {(int)bng_notifytype.Rejected, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdFail)},
                    {(int)bng_notifytype.FurtherInformation, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdOnHold)},
                    {(int)bng_notifytype.RequestDocumentsPayment, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentRequest)},
                    {(int)bng_notifytype.TriageComplete, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentConfirmation)},
                    {(int)bng_notifytype.PaymentRemainder, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdDocumentAndPaymentRemainder)},
                    {(int)bng_notifytype.Withdraw, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdWithdraw)},
                    {(int)bng_notifytype.ConfirmdocumentsreceivedRequestpayment, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdConfirmationOfDocsAndRequestPayment)},
                    {(int)bng_notifytype.NoticeofIntent, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationTemplateIdNoticeOfIntent)}
                };

            return result;
        }
    }
}