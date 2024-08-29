namespace DEFRA.NE.BNG.Integration.Domain.Request
{
    public class DefraIdContact
    {
        public string defra_b2cobjectid { get; set; }
        public string firstname { get; set; }
        public bool firstname_hasvalue { get; set; }
        public string middlename { get; set; }
        public bool middlename_hasvalue { get; set; }
        public string lastname { get; set; }
        public bool lastname_hasvalue { get; set; }
        public string contactid { get; set; }
        public bool contactid_hasvalue { get; set; }
        public string emailaddress1 { get; set; }
        public bool emailaddress1_hasvalue { get; set; }
        public string birthdate { get; set; }
        public bool birthdate_hasvalue { get; set; }
        public string telephone1 { get; set; }
        public bool telephone1_hasvalue { get; set; }
        public string defra_addrcorbuildingname { get; set; }
        public string defra_addrcorlocality { get; set; }
        public string defra_addrcorstreet { get; set; }
        public string defra_addrcorsubbuildingname { get; set; }
        public string defra_addrcorbuildingnumber { get; set; }
        public string defra_addrcortown { get; set; }
        public string defra_addrcoruprn { get; set; }
        public string defra_addrcorcounty { get; set; }
        public string defra_addrcorcountryid { get; set; }
        public string defra_addrcordependentlocality { get; set; }
        public string defra_addrcorinternationalpostalcode { get; set; }
        public string defra_addrcorpostcode { get; set; }
        public string nationalityid1 { get; set; }
        public string nationalityid2 { get; set; }
        public string nationalityid3 { get; set; }
        public string nationalityid4 { get; set; }
    }
}
