#pragma warning disable CS1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DEFRA.NE.BNG.Integration.Domain.Models
{
	
	
	/// <summary>
	/// Status of the Development
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum bng_developerregistration_statecode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// Reason for the status of the Development
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum bng_developerregistration_statuscode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 2,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Rejected = 4,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Registered = 759150001,
	}
	
	/// <summary>
	/// Holds Developer Registration details
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("bng_developerregistration")]
	public partial class bng_DeveloperRegistration : Microsoft.Xrm.Sdk.Entity
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public bng_DeveloperRegistration() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "bng_developerregistration";
		
		public const string EntityLogicalCollectionName = "bng_developerregistrations";
		
		public const string EntitySetName = "bng_developerregistrations";
		
		/// <summary>
		/// The Person who submitted the Development.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_applicantid")]
		public Microsoft.Xrm.Sdk.EntityReference bng_ApplicantID
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_applicantid");
			}
			set
			{
				this.SetAttributeValue("bng_applicantid", value);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_areaunitchange")]
		public System.Nullable<decimal> bng_AreaUnitChange
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_areaunitchange");
			}
			set
			{
				this.SetAttributeValue("bng_areaunitchange", value);
			}
		}
		
		/// <summary>
		/// The Case lookup related to Development.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_caseid")]
		public Microsoft.Xrm.Sdk.EntityReference bng_CaseID
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_caseid");
			}
			set
			{
				this.SetAttributeValue("bng_caseid", value);
			}
		}
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_developerregistrationid")]
		public System.Nullable<System.Guid> bng_DeveloperRegistrationId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("bng_developerregistrationid");
			}
			set
			{
				this.SetAttributeValue("bng_developerregistrationid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_developerregistrationid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.bng_DeveloperRegistrationId = value;
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_hedgerowunitchange")]
		public System.Nullable<decimal> bng_HedgerowUnitChange
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_hedgerowunitchange");
			}
			set
			{
				this.SetAttributeValue("bng_hedgerowunitchange", value);
			}
		}
		
		/// <summary>
		/// This field is only for internal use. Not to be displayed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_jsonforcasecreation")]
		public string bng_JSONForCaseCreation
		{
			get
			{
				return this.GetAttributeValue<string>("bng_jsonforcasecreation");
			}
			set
			{
				this.SetAttributeValue("bng_jsonforcasecreation", value);
			}
		}
		
		/// <summary>
		/// The Local Authority relating to the Developer Registration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_localauthority")]
		public string bng_LocalAuthority
		{
			get
			{
				return this.GetAttributeValue<string>("bng_localauthority");
			}
			set
			{
				this.SetAttributeValue("bng_localauthority", value);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_localplanningauthority")]
		public Microsoft.Xrm.Sdk.EntityReference bng_localplanningauthority
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_localplanningauthority");
			}
			set
			{
				this.SetAttributeValue("bng_localplanningauthority", value);
			}
		}
		
		/// <summary>
		/// The Development submitted related to Organisation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_organisationid")]
		public Microsoft.Xrm.Sdk.EntityReference bng_OrganisationID
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_organisationid");
			}
			set
			{
				this.SetAttributeValue("bng_organisationid", value);
			}
		}
		
		/// <summary>
		/// The Planning Reference relating to the Developments.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_planningreference")]
		public string bng_PlanningReference
		{
			get
			{
				return this.GetAttributeValue<string>("bng_planningreference");
			}
			set
			{
				this.SetAttributeValue("bng_planningreference", value);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_portalreferencenumber")]
		public string bng_PortalReferenceNumber
		{
			get
			{
				return this.GetAttributeValue<string>("bng_portalreferencenumber");
			}
			set
			{
				this.SetAttributeValue("bng_portalreferencenumber", value);
			}
		}
		
		/// <summary>
		/// The Project Name relating to the Development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_projectname")]
		public string bng_ProjectName
		{
			get
			{
				return this.GetAttributeValue<string>("bng_projectname");
			}
			set
			{
				this.SetAttributeValue("bng_projectname", value);
			}
		}
		
		/// <summary>
		/// Set by integration user when Development and related rows are created in PowerApps
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_readyforprocessingon")]
		public System.Nullable<System.DateTime> bng_ReadyForProcessingOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("bng_readyforprocessingon");
			}
			set
			{
				this.SetAttributeValue("bng_readyforprocessingon", value);
			}
		}
		
		/// <summary>
		/// The Reference Number for the development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_referencenumber")]
		public string bng_ReferenceNumber
		{
			get
			{
				return this.GetAttributeValue<string>("bng_referencenumber");
			}
			set
			{
				this.SetAttributeValue("bng_referencenumber", value);
			}
		}
		
		/// <summary>
		/// Types of Development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_registrationtype")]
		public virtual bng_registrationtype? bng_RegistrationType
		{
			get
			{
				return ((bng_registrationtype?)(EntityOptionSetEnum.GetEnum(this, "bng_registrationtype")));
			}
			set
			{
				this.SetAttributeValue("bng_registrationtype", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_rejectionreason")]
		public Microsoft.Xrm.Sdk.EntityReference bng_rejectionreason
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_rejectionreason");
			}
			set
			{
				this.SetAttributeValue("bng_rejectionreason", value);
			}
		}
		
		/// <summary>
		/// The Source column to identify the record created from which source.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_source")]
		public virtual bng_source? bng_source
		{
			get
			{
				return ((bng_source?)(EntityOptionSetEnum.GetEnum(this, "bng_source")));
			}
			set
			{
				this.SetAttributeValue("bng_source", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_triagecomplete")]
		public System.Nullable<bool> bng_TriageComplete
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("bng_triagecomplete");
			}
			set
			{
				this.SetAttributeValue("bng_triagecomplete", value);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_watercoursesunitchange")]
		public System.Nullable<decimal> bng_WatercoursesUnitChange
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_watercoursesunitchange");
			}
			set
			{
				this.SetAttributeValue("bng_watercoursesunitchange", value);
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
			}
		}
		
		/// <summary>
		/// Date and time when the record was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
		public System.Nullable<System.DateTime> CreatedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who created the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
			}
		}
		
		/// <summary>
		/// Sequence number of the import that created this record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("importsequencenumber")]
		public System.Nullable<int> ImportSequenceNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("importsequencenumber");
			}
			set
			{
				this.SetAttributeValue("importsequencenumber", value);
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who modified the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
			}
		}
		
		/// <summary>
		/// Date and time when the record was modified.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who modified the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
		}
		
		/// <summary>
		/// Date and time that the record was migrated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overriddencreatedon")]
		public System.Nullable<System.DateTime> OverriddenCreatedOn
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overriddencreatedon");
			}
			set
			{
				this.SetAttributeValue("overriddencreatedon", value);
			}
		}
		
		/// <summary>
		/// Owner Id
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ownerid")]
		public Microsoft.Xrm.Sdk.EntityReference OwnerId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("ownerid");
			}
			set
			{
				this.SetAttributeValue("ownerid", value);
			}
		}
		
		/// <summary>
		/// Unique identifier for the business unit that owns the record
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningbusinessunit")]
		public Microsoft.Xrm.Sdk.EntityReference OwningBusinessUnit
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningbusinessunit");
			}
		}
		
		/// <summary>
		/// Unique identifier for the team that owns the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		public Microsoft.Xrm.Sdk.EntityReference OwningTeam
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owningteam");
			}
		}
		
		/// <summary>
		/// Unique identifier for the user that owns the record.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owninguser")]
		public Microsoft.Xrm.Sdk.EntityReference OwningUser
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("owninguser");
			}
		}
		
		/// <summary>
		/// Status of the Development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public virtual bng_developerregistration_statecode? statecode
		{
			get
			{
				return ((bng_developerregistration_statecode?)(EntityOptionSetEnum.GetEnum(this, "statecode")));
			}
			set
			{
				this.SetAttributeValue("statecode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Reason for the status of the Development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual bng_developerregistration_statuscode? statuscode
		{
			get
			{
				return ((bng_developerregistration_statuscode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			set
			{
				this.SetAttributeValue("statuscode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("timezoneruleversionnumber")]
		public System.Nullable<int> TimeZoneRuleVersionNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("timezoneruleversionnumber");
			}
			set
			{
				this.SetAttributeValue("timezoneruleversionnumber", value);
			}
		}
		
		/// <summary>
		/// Time zone code that was in use when the record was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("utcconversiontimezonecode")]
		public System.Nullable<int> UTCConversionTimeZoneCode
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("utcconversiontimezonecode");
			}
			set
			{
				this.SetAttributeValue("utcconversiontimezonecode", value);
			}
		}
		
		/// <summary>
		/// Version Number
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
		public System.Nullable<long> VersionNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
			}
		}
		
		/// <summary>
		/// 1:N bng_biodiversityvalueunitchange_DevelopmentID
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_biodiversityvalueunitchange_DevelopmentID")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_BiodiversityValueUnitChange> bng_biodiversityvalueunitchange_DevelopmentID
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_BiodiversityValueUnitChange>("bng_biodiversityvalueunitchange_DevelopmentID", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_BiodiversityValueUnitChange>("bng_biodiversityvalueunitchange_DevelopmentID", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_bng_allocatedhabitats_DeveloperRegistrati
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_allocatedhabitats_DeveloperRegistrati")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_AllocatedHabitats> bng_bng_allocatedhabitats_DeveloperRegistrati
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_AllocatedHabitats>("bng_bng_allocatedhabitats_DeveloperRegistrati", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_AllocatedHabitats>("bng_bng_allocatedhabitats_DeveloperRegistrati", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_bng_case_DeveloperRegistrationId_bng_deve
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_case_DeveloperRegistrationId_bng_deve")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case> bng_bng_case_DeveloperRegistrationId_bng_deve
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_bng_case_DeveloperRegistrationId_bng_deve", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_bng_case_DeveloperRegistrationId_bng_deve", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_developerregistration_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_developerregistration_Annotations")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.Annotation> bng_developerregistration_Annotations
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.Annotation>("bng_developerregistration_Annotations", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.Annotation>("bng_developerregistration_Annotations", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_developerregistration_bng_Escalations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_developerregistration_bng_Escalations")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation> bng_developerregistration_bng_Escalations
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation>("bng_developerregistration_bng_Escalations", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation>("bng_developerregistration_bng_Escalations", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_developerregistration_bng_Notifies
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_developerregistration_bng_Notifies")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify> bng_developerregistration_bng_Notifies
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify>("bng_developerregistration_bng_Notifies", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify>("bng_developerregistration_bng_Notifies", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_DeveloperRegistration_bng_Order
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_DeveloperRegistration_bng_Order")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder> bng_DeveloperRegistration_bng_Order
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>("bng_DeveloperRegistration_bng_Order", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>("bng_DeveloperRegistration_bng_Order", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_DeveloperRegistrations
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_DeveloperRegistrations")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case> bng_DeveloperRegistrations
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_DeveloperRegistrations", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_DeveloperRegistrations", null, value);
			}
		}
		
		/// <summary>
		/// 1:N bng_invoice_DevelopmentID_bng_developerregist
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_invoice_DevelopmentID_bng_developerregist")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.Invoice> bng_invoice_DevelopmentID_bng_developerregist
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.Invoice>("bng_invoice_DevelopmentID_bng_developerregist", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.Invoice>("bng_invoice_DevelopmentID_bng_developerregist", null, value);
			}
		}
		
		/// <summary>
		/// N:N bng_DeveloperRegistration_GainSiteReg
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_DeveloperRegistration_GainSiteReg")]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration> bng_DeveloperRegistration_GainSiteReg
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>("bng_DeveloperRegistration_GainSiteReg", null);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>("bng_DeveloperRegistration_GainSiteReg", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_bng_developerregistration_localplanningau
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_localplanningauthority")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_developerregistration_localplanningau")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_LocalPlanningAuthority bng_bng_developerregistration_localplanningau
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_LocalPlanningAuthority>("bng_bng_developerregistration_localplanningau", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_LocalPlanningAuthority>("bng_bng_developerregistration_localplanningau", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_bng_developerregistration_rejectionreason
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_rejectionreason")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_developerregistration_rejectionreason")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_RejectionReason bng_bng_developerregistration_rejectionreason
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_RejectionReason>("bng_bng_developerregistration_rejectionreason", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_RejectionReason>("bng_bng_developerregistration_rejectionreason", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_developerregistration_ApplicantID
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_applicantid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_developerregistration_ApplicantID")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Contact bng_developerregistration_ApplicantID
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Contact>("bng_developerregistration_ApplicantID", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Contact>("bng_developerregistration_ApplicantID", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_DeveloperRegistration_bng_CaseID
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_caseid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_DeveloperRegistration_bng_CaseID")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_Case bng_DeveloperRegistration_bng_CaseID
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_DeveloperRegistration_bng_CaseID", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_DeveloperRegistration_bng_CaseID", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_developerregistration_OrganisationID
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_organisationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_developerregistration_OrganisationID")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Account bng_developerregistration_OrganisationID
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Account>("bng_developerregistration_OrganisationID", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Account>("bng_developerregistration_OrganisationID", null, value);
			}
		}
		
		/// <summary>
		/// N:1 team_bng_developerregistration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_bng_developerregistration")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Team team_bng_developerregistration
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Team>("team_bng_developerregistration", null);
			}
		}
	}
}
#pragma warning restore CS1591
