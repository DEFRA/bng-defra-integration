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
	
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.RequestProxyAttribute("bng_clonegainsiteregistration_webapi")]
	public partial class bng_clonegainsiteregistration_webapiRequest : Microsoft.Xrm.Sdk.OrganizationRequest
	{
		
		public string bng_gainsiteidinput
		{
			get
			{
				if (this.Parameters.Contains("bng_gainsiteidinput"))
				{
					return ((string)(this.Parameters["bng_gainsiteidinput"]));
				}
				else
				{
					return default(string);
				}
			}
			set
			{
				this.Parameters["bng_gainsiteidinput"] = value;
			}
		}
		
		public bng_clonegainsiteregistration_webapiRequest()
		{
			this.RequestName = "bng_clonegainsiteregistration_webapi";
			this.bng_gainsiteidinput = default(string);
		}
	}
	
	[System.Runtime.Serialization.DataContractAttribute(Namespace="http://schemas.microsoft.com/xrm/2011/new/")]
	[Microsoft.Xrm.Sdk.Client.ResponseProxyAttribute("bng_clonegainsiteregistration_webapi")]
	public partial class bng_clonegainsiteregistration_webapiResponse : Microsoft.Xrm.Sdk.OrganizationResponse
	{
		
		public bng_clonegainsiteregistration_webapiResponse()
		{
		}
		
		public string bng_clonegainsiteresponse
		{
			get
			{
				if (this.Results.Contains("bng_clonegainsiteresponse"))
				{
					return ((string)(this.Results["bng_clonegainsiteresponse"]));
				}
				else
				{
					return default(string);
				}
			}
		}
	}
}
#pragma warning restore CS1591
