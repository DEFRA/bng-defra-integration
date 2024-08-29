namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class DefraIdAccount
    {
        public string defra_addrregcountryid { get; set; }
        public string name { get; set; }
        public bool defra_isuk { get; set; }
        public string defra_type { get; set; }
        public string defra_cmcrn { get; set; }
        public bool defra_validatedwithcompanyhouse { get; set; }
        public string defra_addrregbuildingnumber { get; set; }
        public string defra_addrregbuildingname { get; set; }
        public string defra_addrregsubbuildingname { get; set; }
        public string defra_addrregstreet { get; set; }
        public string defra_addrreglocality { get; set; }
        public string defra_addrregdependentlocality { get; set; }
        public string defra_addrregcounty { get; set; }
        public string defra_addrregtown { get; set; }
        public string defra_addrregpostcode { get; set; }
        public string defra_addrreginternationalpostalcode { get; set; }
        public string defra_addrcorcountryid { get; set; }
        public string emailaddress1 { get; set; }
        public string telephone1 { get; set; }
        public string accountid { get; set; }
        public string defra_dateofincorporation { get; set; }
        public bool defra_dateofincorporation_hasvalue { get; set; }
        public bool name_hasvalue { get; set; }
        public bool defra_type_hasvalue { get; set; }
        public bool defra_cmcrn_hasvalue { get; set; }
        public bool defra_addrregcountryid_hasvalue { get; set; }
        public bool emailaddress1_hasvalue { get; set; }
        public bool telephone1_hasvalue { get; set; }
    }
}
