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
	/// Status of the Allocated Habitats
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum bng_allocatedhabitats_statecode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 1,
	}
	
	/// <summary>
	/// Reason for the status of the Allocated Habitats
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum bng_allocatedhabitats_statuscode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Active = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Inactive = 2,
	}
	
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("bng_allocatedhabitats")]
	public partial class bng_AllocatedHabitats : Microsoft.Xrm.Sdk.Entity
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public bng_AllocatedHabitats() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "bng_allocatedhabitats";
		
		public const string EntityLogicalCollectionName = "bng_allocatedhabitatses";
		
		public const string EntitySetName = "bng_allocatedhabitatses";
		
		/// <summary>
		/// Unique identifier for entity instances
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_allocatedhabitatsid")]
		public System.Nullable<System.Guid> bng_AllocatedHabitatsId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("bng_allocatedhabitatsid");
			}
			set
			{
				this.SetAttributeValue("bng_allocatedhabitatsid", value);
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_allocatedhabitatsid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.bng_AllocatedHabitatsId = value;
			}
		}
		
		/// <summary>
		/// Total Allocated Habitats to this Development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_allocatedtothisdevelopment")]
		public System.Nullable<decimal> bng_AllocatedtothisDevelopment
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_allocatedtothisdevelopment");
			}
			set
			{
				this.SetAttributeValue("bng_allocatedtothisdevelopment", value);
			}
		}
		
		/// <summary>
		/// Case lookup for Allocated Habitat
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_condition")]
		public virtual bng_habitatconditionchoices? bng_Condition
		{
			get
			{
				return ((bng_habitatconditionchoices?)(EntityOptionSetEnum.GetEnum(this, "bng_condition")));
			}
			set
			{
				this.SetAttributeValue("bng_condition", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Developer Registration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_developerregistrationid")]
		public Microsoft.Xrm.Sdk.EntityReference bng_DeveloperRegistrationID
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_developerregistrationid");
			}
			set
			{
				this.SetAttributeValue("bng_developerregistrationid", value);
			}
		}
		
		/// <summary>
		/// Gain Site Registration
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_gainsiteregistrationid")]
		public Microsoft.Xrm.Sdk.EntityReference bng_GainSiteRegistrationID
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_gainsiteregistrationid");
			}
			set
			{
				this.SetAttributeValue("bng_gainsiteregistrationid", value);
			}
		}
		
		/// <summary>
		/// Habitats for gain site and development
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_habitatid")]
		public Microsoft.Xrm.Sdk.EntityReference bng_HabitatID
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_habitatid");
			}
			set
			{
				this.SetAttributeValue("bng_habitatid", value);
			}
		}
		
		/// <summary>
		/// Habitat ID
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_habitatreference")]
		public System.Nullable<int> bng_HabitatReference
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("bng_habitatreference");
			}
			set
			{
				this.SetAttributeValue("bng_habitatreference", value);
			}
		}
		
		/// <summary>
		/// Choice field for Habitat State
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_habitatstate")]
		public virtual bng_habitatstatechoices? bng_HabitatState
		{
			get
			{
				return ((bng_habitatstatechoices?)(EntityOptionSetEnum.GetEnum(this, "bng_habitatstate")));
			}
			set
			{
				this.SetAttributeValue("bng_habitatstate", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Habitat Type choice column
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_habitattype")]
		public virtual bng_habitattypechoices? bng_HabitatType
		{
			get
			{
				return ((bng_habitattypechoices?)(EntityOptionSetEnum.GetEnum(this, "bng_habitattype")));
			}
			set
			{
				this.SetAttributeValue("bng_habitattype", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Allocated Habitat Name
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_name")]
		public string bng_Name
		{
			get
			{
				return this.GetAttributeValue<string>("bng_name");
			}
			set
			{
				this.SetAttributeValue("bng_name", value);
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_proposedhabitatsubtype")]
		public string bng_ProposedHabitatSubtype
		{
			get
			{
				return this.GetAttributeValue<string>("bng_proposedhabitatsubtype");
			}
			set
			{
				this.SetAttributeValue("bng_proposedhabitatsubtype", value);
			}
		}
		
		/// <summary>
		/// Habitat Sub Type lookup
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_proposedhabitatsubtypelookup")]
		public Microsoft.Xrm.Sdk.EntityReference bng_ProposedHabitatSubTypeLookup
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("bng_proposedhabitatsubtypelookup");
			}
			set
			{
				this.SetAttributeValue("bng_proposedhabitatsubtypelookup", value);
			}
		}
		
		/// <summary>
		/// Size
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_size")]
		public System.Nullable<decimal> bng_Size
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_size");
			}
			set
			{
				this.SetAttributeValue("bng_size", value);
			}
		}
		
		/// <summary>
		/// Unit of Measure
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_unitofmeasure")]
		public virtual bng_habitatunitofmeasurement? bng_UnitofMeasure
		{
			get
			{
				return ((bng_habitatunitofmeasurement?)(EntityOptionSetEnum.GetEnum(this, "bng_unitofmeasure")));
			}
			set
			{
				this.SetAttributeValue("bng_unitofmeasure", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Habitat Units column
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_units")]
		public System.Nullable<decimal> bng_Units
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_units");
			}
			set
			{
				this.SetAttributeValue("bng_units", value);
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
		/// Status of the Allocated Habitats
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public virtual bng_allocatedhabitats_statecode? statecode
		{
			get
			{
				return ((bng_allocatedhabitats_statecode?)(EntityOptionSetEnum.GetEnum(this, "statecode")));
			}
			set
			{
				this.SetAttributeValue("statecode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Reason for the status of the Allocated Habitats
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual bng_allocatedhabitats_statuscode? statuscode
		{
			get
			{
				return ((bng_allocatedhabitats_statuscode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
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
		/// N:1 bng_bng_allocatedhabitats_CaseID_bng_case
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_caseid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_allocatedhabitats_CaseID_bng_case")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_Case bng_bng_allocatedhabitats_CaseID_bng_case
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_bng_allocatedhabitats_CaseID_bng_case", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_bng_allocatedhabitats_CaseID_bng_case", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_bng_allocatedhabitats_DeveloperRegistrati
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_developerregistrationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_allocatedhabitats_DeveloperRegistrati")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration bng_bng_allocatedhabitats_DeveloperRegistrati
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration>("bng_bng_allocatedhabitats_DeveloperRegistrati", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration>("bng_bng_allocatedhabitats_DeveloperRegistrati", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_bng_allocatedhabitats_GainSiteRegistratio
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_gainsiteregistrationid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_allocatedhabitats_GainSiteRegistratio")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration bng_bng_allocatedhabitats_GainSiteRegistratio
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>("bng_bng_allocatedhabitats_GainSiteRegistratio", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>("bng_bng_allocatedhabitats_GainSiteRegistratio", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_bng_allocatedhabitats_HabitatID_bng_habit
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_habitatid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_allocatedhabitats_HabitatID_bng_habit")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatType bng_bng_allocatedhabitats_HabitatID_bng_habit
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatType>("bng_bng_allocatedhabitats_HabitatID_bng_habit", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatType>("bng_bng_allocatedhabitats_HabitatID_bng_habit", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_bng_allocatedhabitats_ProposedHabitatSubT
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_proposedhabitatsubtypelookup")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_bng_allocatedhabitats_ProposedHabitatSubT")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatSubType bng_bng_allocatedhabitats_ProposedHabitatSubT
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatSubType>("bng_bng_allocatedhabitats_ProposedHabitatSubT", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatSubType>("bng_bng_allocatedhabitats_ProposedHabitatSubT", null, value);
			}
		}
		
		/// <summary>
		/// N:1 team_bng_allocatedhabitats
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_bng_allocatedhabitats")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Team team_bng_allocatedhabitats
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Team>("team_bng_allocatedhabitats", null);
			}
		}
	}
}
#pragma warning restore CS1591
