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

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace DEFRA.NE.BNG.Integration.Domain.Models
{
	
	
	/// <summary>
	/// Represents a source of entities bound to a Dataverse service. It tracks and manages changes made to the retrieved entities.
	/// </summary>
	public partial class OrgContext : Microsoft.Xrm.Sdk.Client.OrganizationServiceContext
	{
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public OrgContext(Microsoft.Xrm.Sdk.IOrganizationService service) : 
				base(service)
		{
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.Account"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.Account> AccountSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.Account>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.Annotation"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.Annotation> AnnotationSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.Annotation>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_AllocatedHabitats"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_AllocatedHabitats> bng_AllocatedHabitatsSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_AllocatedHabitats>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_AmendmentsChecksandTasks"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_AmendmentsChecksandTasks> bng_AmendmentsChecksandTasksSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_AmendmentsChecksandTasks>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_BankDetails"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_BankDetails> bng_BankDetailsSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_BankDetails>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_BaselineHabitat"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_BaselineHabitat> bng_BaselineHabitatSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_BaselineHabitat>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_BiodiversityValueUnitChange"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_BiodiversityValueUnitChange> bng_BiodiversityValueUnitChangeSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_BiodiversityValueUnitChange>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_BNGConfiguration"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_BNGConfiguration> bng_BNGConfigurationSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_BNGConfiguration>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_Case"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case> bng_CaseSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_Case>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_Country"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Country> bng_CountrySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_Country>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_CustomerDueDiligenceCheck"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_CustomerDueDiligenceCheck> bng_CustomerDueDiligenceCheckSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_CustomerDueDiligenceCheck>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration> bng_DeveloperRegistrationSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_DeveloperRegistration>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_Emailcontent"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Emailcontent> bng_EmailcontentSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_Emailcontent>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_EnforcementBody"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_EnforcementBody> bng_EnforcementBodySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_EnforcementBody>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation> bng_EscalationSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_Escalation>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_fees"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_fees> bng_feesSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_fees>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSitePropertyLandowners"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSitePropertyLandowners> bng_GainSitePropertyLandownersSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSitePropertyLandowners>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration> bng_GainSiteRegistrationSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_GainSiteRegistration>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatSubType"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatSubType> bng_HabitatSubTypeSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatSubType>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatType"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatType> bng_HabitatTypeSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_HabitatType>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_LegalAgreementParty"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_LegalAgreementParty> bng_LegalAgreementPartySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_LegalAgreementParty>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_LocalPlanningAuthority"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_LocalPlanningAuthority> bng_LocalPlanningAuthoritySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_LocalPlanningAuthority>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_Nationality"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Nationality> bng_NationalitySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_Nationality>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify> bng_NotifySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_Notify>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_OrganisationType"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_OrganisationType> bng_OrganisationTypeSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_OrganisationType>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_PaymentDetails"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_PaymentDetails> bng_PaymentDetailsSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_PaymentDetails>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_QATasks"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_QATasks> bng_QATasksSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_QATasks>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_RejectionReason"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_RejectionReason> bng_RejectionReasonSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_RejectionReason>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_ResponsibleBody"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_ResponsibleBody> bng_ResponsibleBodySet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_ResponsibleBody>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.bng_WithdrawalReason"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.bng_WithdrawalReason> bng_WithdrawalReasonSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.bng_WithdrawalReason>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.Contact"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.Contact> ContactSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.Contact>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.Invoice"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.Invoice> InvoiceSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.Invoice>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.Product"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.Product> ProductSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.Product>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder> SalesOrderSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail> SalesOrderDetailSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.Team"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.Team> TeamSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.Team>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.UoM"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.UoM> UoMSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.UoM>();
			}
		}
		
		/// <summary>
		/// Gets a binding to the set of all <see cref="DEFRA.NE.BNG.Integration.Domain.Models.UoMSchedule"/> entities.
		/// </summary>
		public System.Linq.IQueryable<DEFRA.NE.BNG.Integration.Domain.Models.UoMSchedule> UoMScheduleSet
		{
			get
			{
				return this.CreateQuery<DEFRA.NE.BNG.Integration.Domain.Models.UoMSchedule>();
			}
		}
	}
}
#pragma warning restore CS1591
