using DEFRA.NE.BNG.Integration.Domain.Models;

namespace DEFRA.NE.BNG.Integration.Domain.Interfaces
{
    public interface IMappingManager
    {
        Dictionary<int, string> GetReplyEmailMapping();

        Dictionary<bng_casetype, IDictionary<int, string>> GetActionTemplateMappingForCase();

        Dictionary<int, string> GetOrderTemplateMapping();

        bng_habitatconditionchoices MapHabitatConditionDic(string input);

        bng_strategicsignificance MapHabitatStrategicSignificanceDic(string input);

        bng_paymentmethodchoice MapPaymentMethod(string input);

        bng_days1to30and30 MapCreationChoices(int input);

        bng_habitattypechoices MapHabitatState(string input);

        bng_habitatstatechoices MapHabitatInterventionTypes(string input);

        bng_extentofencroachmentbothbanks MapExtentOfEncroachmentBothBanks(string input);

        bng_extentofencroachment MapExtentOfEncroachment(string input);

        void CreateNewOrUpdateExisting<TKey, TValue>(IDictionary<TKey, TValue> map, TKey key, TValue value);
    }
}
