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
	/// Freight terms for the shipping address.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	public enum salesorderdetail_shipto_freighttermscode
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		FOB = 1,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		NoCharge = 2,
	}
	
	/// <summary>
	/// Line item in a sales order.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("salesorderdetail")]
	public partial class SalesOrderDetail : Microsoft.Xrm.Sdk.Entity
	{
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public SalesOrderDetail() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "salesorderdetail";
		
		public const string EntityLogicalCollectionName = "salesorderdetails";
		
		public const string EntitySetName = "salesorderdetails";
		
		/// <summary>
		/// Shows the total price of the order product, based on the price per unit, volume discount, and quantity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("baseamount")]
		public Microsoft.Xrm.Sdk.Money BaseAmount
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("baseamount");
			}
			set
			{
				this.SetAttributeValue("baseamount", value);
			}
		}
		
		/// <summary>
		/// Value of the Amount in base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("baseamount_base")]
		public Microsoft.Xrm.Sdk.Money BaseAmount_Base
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("baseamount_base");
			}
		}
		
		/// <summary>
		/// Percentage For The VAT Rate
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_percentage")]
		public System.Nullable<decimal> bng_Percentage
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("bng_percentage");
			}
			set
			{
				this.SetAttributeValue("bng_percentage", value);
			}
		}
		
		/// <summary>
		/// VAT Rate for the Order Product
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("bng_vatrate")]
		public virtual bng_vatrate? bng_VATRate
		{
			get
			{
				return ((bng_vatrate?)(EntityOptionSetEnum.GetEnum(this, "bng_vatrate")));
			}
			set
			{
				this.SetAttributeValue("bng_vatrate", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Shows who created the record.
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
		/// Shows who created the record on behalf of another user.
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
		/// Type additional information to describe the order product, such as manufacturing details or acceptable substitutions.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
		public string Description
		{
			get
			{
				return this.GetAttributeValue<string>("description");
			}
			set
			{
				this.SetAttributeValue("description", value);
			}
		}
		
		/// <summary>
		/// Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("exchangerate")]
		public System.Nullable<decimal> ExchangeRate
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("exchangerate");
			}
		}
		
		/// <summary>
		/// Shows the total amount due for the order product, based on the sum of the unit price, quantity, discounts, and tax.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("extendedamount")]
		public Microsoft.Xrm.Sdk.Money ExtendedAmount
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("extendedamount");
			}
			set
			{
				this.SetAttributeValue("extendedamount", value);
			}
		}
		
		/// <summary>
		/// Value of the Extended Amount in base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("extendedamount_base")]
		public Microsoft.Xrm.Sdk.Money ExtendedAmount_Base
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("extendedamount_base");
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
		/// Select whether the invoice line item is copied from another item or data source.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscopied")]
		public System.Nullable<bool> IsCopied
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("iscopied");
			}
			set
			{
				this.SetAttributeValue("iscopied", value);
			}
		}
		
		/// <summary>
		/// Select whether the price per unit is fixed at the value in the specified price list or can be overridden by users who have edit rights to the order product.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ispriceoverridden")]
		public System.Nullable<bool> IsPriceOverridden
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("ispriceoverridden");
			}
			set
			{
				this.SetAttributeValue("ispriceoverridden", value);
			}
		}
		
		/// <summary>
		/// Select whether the product exists in the Microsoft Dynamics 365 product catalog or is a write-in product specific to the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("isproductoverridden")]
		public System.Nullable<bool> IsProductOverridden
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("isproductoverridden");
			}
			set
			{
				this.SetAttributeValue("isproductoverridden", value);
			}
		}
		
		/// <summary>
		/// Type the line item number for the order product to easily identify the product in the order and make sure it's listed in the correct sequence.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("lineitemnumber")]
		public System.Nullable<int> LineItemNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("lineitemnumber");
			}
			set
			{
				this.SetAttributeValue("lineitemnumber", value);
			}
		}
		
		/// <summary>
		/// Type the manual discount amount for the order product to deduct any negotiated or other savings from the product total on the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("manualdiscountamount")]
		public Microsoft.Xrm.Sdk.Money ManualDiscountAmount
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("manualdiscountamount");
			}
			set
			{
				this.SetAttributeValue("manualdiscountamount", value);
			}
		}
		
		/// <summary>
		/// Value of the Manual Discount in base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("manualdiscountamount_base")]
		public Microsoft.Xrm.Sdk.Money ManualDiscountAmount_Base
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("manualdiscountamount_base");
			}
		}
		
		/// <summary>
		/// Shows who last updated the record.
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
		/// Shows who last updated the record on behalf of another user.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ordercreationmethod")]
		public virtual salesordercreationmethod? OrderCreationMethod
		{
			get
			{
				return ((salesordercreationmethod?)(EntityOptionSetEnum.GetEnum(this, "ordercreationmethod")));
			}
			set
			{
				this.SetAttributeValue("ordercreationmethod", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
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
		/// Choose the parent bundle associated with this product
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentbundleid")]
		public System.Nullable<System.Guid> ParentBundleId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("parentbundleid");
			}
			set
			{
				this.SetAttributeValue("parentbundleid", value);
			}
		}
		
		/// <summary>
		/// Choose the parent bundle associated with this product
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentbundleidref")]
		public Microsoft.Xrm.Sdk.EntityReference ParentBundleIdRef
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("parentbundleidref");
			}
			set
			{
				this.SetAttributeValue("parentbundleidref", value);
			}
		}
		
		/// <summary>
		/// Type the price per unit of the order product. The default is the value in the price list specified on the order for existing products.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("priceperunit")]
		public Microsoft.Xrm.Sdk.Money PricePerUnit
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("priceperunit");
			}
			set
			{
				this.SetAttributeValue("priceperunit", value);
			}
		}
		
		/// <summary>
		/// Value of the Price Per Unit in base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("priceperunit_base")]
		public Microsoft.Xrm.Sdk.Money PricePerUnit_Base
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("priceperunit_base");
			}
		}
		
		/// <summary>
		/// Select the type of pricing error, such as a missing or invalid product, or missing quantity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("pricingerrorcode")]
		public virtual qooi_pricingerrorcode? PricingErrorCode
		{
			get
			{
				return ((qooi_pricingerrorcode?)(EntityOptionSetEnum.GetEnum(this, "pricingerrorcode")));
			}
			set
			{
				this.SetAttributeValue("pricingerrorcode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Unique identifier of the product line item association with bundle in the sales order
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("productassociationid")]
		public System.Nullable<System.Guid> ProductAssociationId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("productassociationid");
			}
			set
			{
				this.SetAttributeValue("productassociationid", value);
			}
		}
		
		/// <summary>
		/// Type a name or description to identify the type of write-in product included in the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("productdescription")]
		public string ProductDescription
		{
			get
			{
				return this.GetAttributeValue<string>("productdescription");
			}
			set
			{
				this.SetAttributeValue("productdescription", value);
			}
		}
		
		/// <summary>
		/// Choose the product to include on the order to link the product's pricing and other information to the parent order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("productid")]
		public Microsoft.Xrm.Sdk.EntityReference ProductId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("productid");
			}
			set
			{
				this.SetAttributeValue("productid", value);
			}
		}
		
		/// <summary>
		/// Calculated field that will be populated by name and description of the product.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("productname")]
		public string ProductName
		{
			get
			{
				return this.GetAttributeValue<string>("productname");
			}
			set
			{
				this.SetAttributeValue("productname", value);
			}
		}
		
		/// <summary>
		/// User-defined product ID.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("productnumber")]
		public string ProductNumber
		{
			get
			{
				return this.GetAttributeValue<string>("productnumber");
			}
		}
		
		/// <summary>
		/// Product Type
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("producttypecode")]
		public virtual qooiproduct_producttype? ProductTypeCode
		{
			get
			{
				return ((qooiproduct_producttype?)(EntityOptionSetEnum.GetEnum(this, "producttypecode")));
			}
			set
			{
				this.SetAttributeValue("producttypecode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Status of the property configuration.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("propertyconfigurationstatus")]
		public virtual qooiproduct_propertiesconfigurationstatus? PropertyConfigurationStatus
		{
			get
			{
				return ((qooiproduct_propertiesconfigurationstatus?)(EntityOptionSetEnum.GetEnum(this, "propertyconfigurationstatus")));
			}
			set
			{
				this.SetAttributeValue("propertyconfigurationstatus", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Type the amount or quantity of the product ordered by the customer.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("quantity")]
		public System.Nullable<decimal> Quantity
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("quantity");
			}
			set
			{
				this.SetAttributeValue("quantity", value);
			}
		}
		
		/// <summary>
		/// Type the amount or quantity of the product that is back ordered for the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("quantitybackordered")]
		public System.Nullable<decimal> QuantityBackordered
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("quantitybackordered");
			}
			set
			{
				this.SetAttributeValue("quantitybackordered", value);
			}
		}
		
		/// <summary>
		/// Type the amount or quantity of the product that was canceled.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("quantitycancelled")]
		public System.Nullable<decimal> QuantityCancelled
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("quantitycancelled");
			}
			set
			{
				this.SetAttributeValue("quantitycancelled", value);
			}
		}
		
		/// <summary>
		/// Type the amount or quantity of the product that was shipped for the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("quantityshipped")]
		public System.Nullable<decimal> QuantityShipped
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<decimal>>("quantityshipped");
			}
			set
			{
				this.SetAttributeValue("quantityshipped", value);
			}
		}
		
		/// <summary>
		/// Unique identifier for Quote Line associated with Order Line.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("quotedetailid")]
		public Microsoft.Xrm.Sdk.EntityReference QuoteDetailId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("quotedetailid");
			}
			set
			{
				this.SetAttributeValue("quotedetailid", value);
			}
		}
		
		/// <summary>
		/// Enter the delivery date requested by the customer for the order product.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("requestdeliveryby")]
		public System.Nullable<System.DateTime> RequestDeliveryBy
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("requestdeliveryby");
			}
			set
			{
				this.SetAttributeValue("requestdeliveryby", value);
			}
		}
		
		/// <summary>
		/// Unique identifier of the product specified in the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesorderdetailid")]
		public System.Nullable<System.Guid> SalesOrderDetailId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("salesorderdetailid");
			}
			set
			{
				this.SetAttributeValue("salesorderdetailid", value);
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
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesorderdetailid")]
		public override System.Guid Id
		{
			get
			{
				return base.Id;
			}
			set
			{
				this.SalesOrderDetailId = value;
			}
		}
		
		/// <summary>
		/// Sales Order Detail Name. Added for 1:n referential relationship (internal purposes only)
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesorderdetailname")]
		public string SalesOrderDetailName
		{
			get
			{
				return this.GetAttributeValue<string>("salesorderdetailname");
			}
			set
			{
				this.SetAttributeValue("salesorderdetailname", value);
			}
		}
		
		/// <summary>
		/// Shows the order for the product. The ID is used to link product pricing and other details to the total amounts and other information on the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesorderid")]
		public Microsoft.Xrm.Sdk.EntityReference SalesOrderId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("salesorderid");
			}
			set
			{
				this.SetAttributeValue("salesorderid", value);
			}
		}
		
		/// <summary>
		/// Tells whether product pricing is locked for the order.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesorderispricelocked")]
		public System.Nullable<bool> SalesOrderIsPriceLocked
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("salesorderispricelocked");
			}
		}
		
		/// <summary>
		/// Choose the user responsible for the sale of the order product.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesrepid")]
		public Microsoft.Xrm.Sdk.EntityReference SalesRepId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("salesrepid");
			}
			set
			{
				this.SetAttributeValue("salesrepid", value);
			}
		}
		
		/// <summary>
		/// Shows the ID of the data that maintains the sequence.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sequencenumber")]
		public System.Nullable<int> SequenceNumber
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("sequencenumber");
			}
			set
			{
				this.SetAttributeValue("sequencenumber", value);
			}
		}
		
		/// <summary>
		/// Unique identifier of the shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_addressid")]
		public System.Nullable<System.Guid> ShipTo_AddressId
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("shipto_addressid");
			}
			set
			{
				this.SetAttributeValue("shipto_addressid", value);
			}
		}
		
		/// <summary>
		/// Type the city for the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_city")]
		public string ShipTo_City
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_city");
			}
			set
			{
				this.SetAttributeValue("shipto_city", value);
			}
		}
		
		/// <summary>
		/// Type the primary contact name at the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_contactname")]
		public string ShipTo_ContactName
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_contactname");
			}
			set
			{
				this.SetAttributeValue("shipto_contactname", value);
			}
		}
		
		/// <summary>
		/// Type the country or region for the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_country")]
		public string ShipTo_Country
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_country");
			}
			set
			{
				this.SetAttributeValue("shipto_country", value);
			}
		}
		
		/// <summary>
		/// Type the fax number for the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_fax")]
		public string ShipTo_Fax
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_fax");
			}
			set
			{
				this.SetAttributeValue("shipto_fax", value);
			}
		}
		
		/// <summary>
		/// Select the freight terms to make sure shipping orders are processed correctly.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_freighttermscode")]
		public virtual salesorderdetail_shipto_freighttermscode? ShipTo_FreightTermsCode
		{
			get
			{
				return ((salesorderdetail_shipto_freighttermscode?)(EntityOptionSetEnum.GetEnum(this, "shipto_freighttermscode")));
			}
			set
			{
				this.SetAttributeValue("shipto_freighttermscode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Type the first line of the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_line1")]
		public string ShipTo_Line1
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_line1");
			}
			set
			{
				this.SetAttributeValue("shipto_line1", value);
			}
		}
		
		/// <summary>
		/// Type the second line of the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_line2")]
		public string ShipTo_Line2
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_line2");
			}
			set
			{
				this.SetAttributeValue("shipto_line2", value);
			}
		}
		
		/// <summary>
		/// Type the third line of the shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_line3")]
		public string ShipTo_Line3
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_line3");
			}
			set
			{
				this.SetAttributeValue("shipto_line3", value);
			}
		}
		
		/// <summary>
		/// Type a name for the customer's shipping address, such as "Headquarters" or "Field office",  to identify the address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_name")]
		public string ShipTo_Name
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_name");
			}
			set
			{
				this.SetAttributeValue("shipto_name", value);
			}
		}
		
		/// <summary>
		/// Type the ZIP Code or postal code for the shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_postalcode")]
		public string ShipTo_PostalCode
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_postalcode");
			}
			set
			{
				this.SetAttributeValue("shipto_postalcode", value);
			}
		}
		
		/// <summary>
		/// Type the state or province for the shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_stateorprovince")]
		public string ShipTo_StateOrProvince
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_stateorprovince");
			}
			set
			{
				this.SetAttributeValue("shipto_stateorprovince", value);
			}
		}
		
		/// <summary>
		/// Type the phone number for the customer's shipping address.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("shipto_telephone")]
		public string ShipTo_Telephone
		{
			get
			{
				return this.GetAttributeValue<string>("shipto_telephone");
			}
			set
			{
				this.SetAttributeValue("shipto_telephone", value);
			}
		}
		
		/// <summary>
		/// Skip the price calculation
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("skippricecalculation")]
		public virtual qooidetail_skippricecalculation? SkipPriceCalculation
		{
			get
			{
				return ((qooidetail_skippricecalculation?)(EntityOptionSetEnum.GetEnum(this, "skippricecalculation")));
			}
			set
			{
				this.SetAttributeValue("skippricecalculation", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
			}
		}
		
		/// <summary>
		/// Type the tax amount for the order product.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("tax")]
		public Microsoft.Xrm.Sdk.Money Tax
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("tax");
			}
			set
			{
				this.SetAttributeValue("tax", value);
			}
		}
		
		/// <summary>
		/// Value of the Tax in base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("tax_base")]
		public Microsoft.Xrm.Sdk.Money Tax_Base
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("tax_base");
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
		/// Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("transactioncurrencyid")]
		public Microsoft.Xrm.Sdk.EntityReference TransactionCurrencyId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("transactioncurrencyid");
			}
			set
			{
				this.SetAttributeValue("transactioncurrencyid", value);
			}
		}
		
		/// <summary>
		/// Choose the unit of measurement for the base unit quantity for this purchase, such as each or dozen.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("uomid")]
		public Microsoft.Xrm.Sdk.EntityReference UoMId
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("uomid");
			}
			set
			{
				this.SetAttributeValue("uomid", value);
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
		/// Shows the discount amount per unit if a specified volume is purchased. Configure volume discounts in the Product Catalog in the Settings area.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("volumediscountamount")]
		public Microsoft.Xrm.Sdk.Money VolumeDiscountAmount
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("volumediscountamount");
			}
		}
		
		/// <summary>
		/// Value of the Volume Discount in base currency.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("volumediscountamount_base")]
		public Microsoft.Xrm.Sdk.Money VolumeDiscountAmount_Base
		{
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.Money>("volumediscountamount_base");
			}
		}
		
		/// <summary>
		/// Select whether the order product should be shipped to the specified address or held until the customer calls with further pick up or delivery instructions.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("willcall")]
		public System.Nullable<bool> WillCall
		{
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("willcall");
			}
			set
			{
				this.SetAttributeValue("willcall", value);
			}
		}
		
		/// <summary>
		/// 1:N salesorderdetail_parent_salesorderdetail
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("salesorderdetail_parent_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referenced)]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail> Referencedsalesorderdetail_parent_salesorderdetail
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parent_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referenced);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parent_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referenced, value);
			}
		}
		
		/// <summary>
		/// 1:N salesorderdetail_parentref_salesorderdetail
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("salesorderdetail_parentref_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referenced)]
		public System.Collections.Generic.IEnumerable<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail> Referencedsalesorderdetail_parentref_salesorderdetail
		{
			get
			{
				return this.GetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parentref_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referenced);
			}
			set
			{
				this.SetRelatedEntities<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parentref_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referenced, value);
			}
		}
		
		/// <summary>
		/// N:1 order_details
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("salesorderid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("order_details")]
		public DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder order_details
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>("order_details", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrder>("order_details", null, value);
			}
		}
		
		/// <summary>
		/// N:1 product_order_details
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("productid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("product_order_details")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Product product_order_details
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Product>("product_order_details", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Product>("product_order_details", null, value);
			}
		}
		
		/// <summary>
		/// N:1 salesorderdetail_parent_salesorderdetail
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentbundleid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("salesorderdetail_parent_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referencing)]
		public DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail Referencingsalesorderdetail_parent_salesorderdetail
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parent_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referencing);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parent_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referencing, value);
			}
		}
		
		/// <summary>
		/// N:1 salesorderdetail_parentref_salesorderdetail
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("parentbundleidref")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("salesorderdetail_parentref_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referencing)]
		public DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail Referencingsalesorderdetail_parentref_salesorderdetail
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parentref_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referencing);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.SalesOrderDetail>("salesorderdetail_parentref_salesorderdetail", Microsoft.Xrm.Sdk.EntityRole.Referencing, value);
			}
		}
		
		/// <summary>
		/// N:1 team_salesorderdetail
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("owningteam")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("team_salesorderdetail")]
		public DEFRA.NE.BNG.Integration.Domain.Models.Team team_salesorderdetail
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.Team>("team_salesorderdetail", null);
			}
		}
		
		/// <summary>
		/// N:1 unit_of_measurement_order_details
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("uomid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("unit_of_measurement_order_details")]
		public DEFRA.NE.BNG.Integration.Domain.Models.UoM unit_of_measurement_order_details
		{
			get
			{
				return this.GetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.UoM>("unit_of_measurement_order_details", null);
			}
			set
			{
				this.SetRelatedEntity<DEFRA.NE.BNG.Integration.Domain.Models.UoM>("unit_of_measurement_order_details", null, value);
			}
		}
	}
}
#pragma warning restore CS1591
