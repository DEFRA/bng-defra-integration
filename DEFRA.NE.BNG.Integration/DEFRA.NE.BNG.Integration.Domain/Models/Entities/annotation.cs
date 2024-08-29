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
	/// Note that is attached to one or more objects, including other notes.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("annotation")]
	public partial class Annotation : Microsoft.Xrm.Sdk.Entity
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Annotation() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "annotation";
		
		public const string EntityLogicalCollectionName = "annotations";
		
		public const string EntitySetName = "annotations";
		
		/// <summary>
		/// Unique identifier of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("annotationid")]
		public System.Nullable<System.Guid> AnnotationId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("annotationid");
			}
			set
			{
				this.SetAttributeValue("annotationid", value);
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("annotationid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.AnnotationId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the note.
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
		/// Date and time when the note was created.
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
		/// Unique identifier of the delegate user who created the annotation.
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
		/// Contents of the note's attachment.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("documentbody")]
		public string DocumentBody
		{
			get
			{
				return this.GetAttributeValue<string>("documentbody");
			}
			set
			{
				this.SetAttributeValue("documentbody", value);
			}
		}
		
		/// <summary>
		/// File name of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filename")]
		public string FileName
		{
			get
			{
				return this.GetAttributeValue<string>("filename");
			}
			set
			{
				this.SetAttributeValue("filename", value);
			}
		}
		
		/// <summary>
		/// File size of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filesize")]
		public System.Nullable<int> FileSize
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("filesize");
			}
		}
		
		/// <summary>
		/// Unique identifier of the data import or data migration that created this record.
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
		/// Specifies whether the note is an attachment.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isdocument")]
		public System.Nullable<bool> IsDocument
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isdocument");
			}
			set
			{
				this.SetAttributeValue("isdocument", value);
			}
		}
		
		/// <summary>
		/// Language identifier for the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("langid")]
		public string LangId
		{
			get
			{
				return this.GetAttributeValue<string>("langid");
			}
			set
			{
				this.SetAttributeValue("langid", value);
			}
		}
		
		/// <summary>
		/// MIME type of the note's attachment.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mimetype")]
		public string MimeType
		{
			get
			{
				return this.GetAttributeValue<string>("mimetype");
			}
			set
			{
				this.SetAttributeValue("mimetype", value);
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the note.
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
		/// Date and time when the note was last modified.
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
		/// Unique identifier of the delegate user who last modified the annotation.
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
		/// Text of the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("notetext")]
		public string NoteText
		{
			get
			{
				return this.GetAttributeValue<string>("notetext");
			}
			set
			{
				this.SetAttributeValue("notetext", value);
			}
		}
		
		/// <summary>
		/// Unique identifier of the object with which the note is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		public Microsoft.Xrm.Sdk.EntityReference ObjectId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("objectid");
			}
			set
			{
				this.SetAttributeValue("objectid", value);
			}
		}
		
		/// <summary>
		/// Type of entity with which the note is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objecttypecode")]
		public string ObjectTypeCode
		{
			get
			{
				return this.GetAttributeValue<string>("objecttypecode");
			}
			set
			{
				this.SetAttributeValue("objecttypecode", value);
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
		/// Unique identifier of the user or team who owns the note.
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
		/// Unique identifier of the business unit that owns the note.
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
		/// Unique identifier of the team who owns the note.
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
		/// Unique identifier of the user who owns the note.
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
		/// Prefix of the file pointer in blob storage.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("prefix")]
		public string Prefix
		{
			get
			{
				return this.GetAttributeValue<string>("prefix");
			}
		}
		
		/// <summary>
		/// workflow step id associated with the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stepid")]
		public string StepId
		{
			get
			{
				return this.GetAttributeValue<string>("stepid");
			}
			set
			{
				this.SetAttributeValue("stepid", value);
			}
		}
		
		/// <summary>
		/// Subject associated with the note.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("subject")]
		public string Subject
		{
			get
			{
				return this.GetAttributeValue<string>("subject");
			}
			set
			{
				this.SetAttributeValue("subject", value);
			}
		}
		
		/// <summary>
		/// Version number of the note.
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
		/// N:1 Account_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Account_Annotation")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Account Account_Annotation
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Account>("Account_Annotation", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Account>("Account_Annotation", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_case_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_case_Annotations")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_Case bng_case_Annotations
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_case_Annotations", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>("bng_case_Annotations", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_developerregistration_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_developerregistration_Annotations")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration bng_developerregistration_Annotations
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration>("bng_developerregistration_Annotations", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration>("bng_developerregistration_Annotations", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_escalation_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_escalation_Annotations")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation bng_escalation_Annotations
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation>("bng_escalation_Annotations", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation>("bng_escalation_Annotations", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_gainsiteregistration_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_gainsiteregistration_Annotations")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration bng_gainsiteregistration_Annotations
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>("bng_gainsiteregistration_Annotations", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>("bng_gainsiteregistration_Annotations", null, value);
			}
		}
		
		/// <summary>
		/// N:1 bng_notify_Annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("bng_notify_Annotations")]
		public DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify bng_notify_Annotations
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify>("bng_notify_Annotations", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify>("bng_notify_Annotations", null, value);
			}
		}
		
		/// <summary>
		/// N:1 Contact_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Contact_Annotation")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Contact Contact_Annotation
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Contact>("Contact_Annotation", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Contact>("Contact_Annotation", null, value);
			}
		}
		
		/// <summary>
		/// N:1 Invoice_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Invoice_Annotation")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Invoice Invoice_Annotation
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Invoice>("Invoice_Annotation", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Invoice>("Invoice_Annotation", null, value);
			}
		}
		
		/// <summary>
		/// N:1 Product_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("Product_Annotation")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Product Product_Annotation
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Product>("Product_Annotation", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Product>("Product_Annotation", null, value);
			}
		}
		
		/// <summary>
		/// N:1 SalesOrder_Annotation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("objectid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("SalesOrder_Annotation")]
		public DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder SalesOrder_Annotation
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>("SalesOrder_Annotation", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>("SalesOrder_Annotation", null, value);
			}
		}
		
		/// <summary>
		/// N:1 team_annotations
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_annotations")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Team team_annotations
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Team>("team_annotations", null);
			}
		}
	}
}
#pragma warning restore CS1591