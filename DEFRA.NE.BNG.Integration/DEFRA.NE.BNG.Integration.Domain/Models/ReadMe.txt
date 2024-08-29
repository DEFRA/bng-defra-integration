   In order to generate early bound entities from dataverse, execute the following steps (it is assumed that the Power Apps CLI is already installed on your development device. If this is not the case please follow the instructions listed in the resource to install the CLI - https://learn.microsoft.com/en-us/power-platform/developer/cli/introduction?tabs=windows)
   1) Open a PwerShell command windows
   2) Change directory folder to the root folder of this project
   3) Copy and execute the command below: 
      
   pac modelbuilder build `
   --outdirectory .\models `
   --entitynamesfilter 'bng_allocatedhabitats;annotation;bng_bankdetails;bng_baselinehabitat;bng_case;bng_customerduediligencecheck;bng_bngconfiguration;bng_country;bng_developerregistration;bng_emailcontent;bng_enforcementbody;bng_fees;bng_gainsiteregistration;bng_habitattype;bng_habitatsubtype;contact;bng_gainsitepropertylandowners;bng_legalagreementparty;bng_localplanningauthority;bng_nationality;bng_notify;salesorder;salesorderdetail;account;bng_organisationtype;bng_paymentdetails;product;bng_responsiblebody;uom;uomschedule;invoice;bng_escalation;bng_amendmentschecksandtasks;bng_qatasks;bng_biodiversityvalueunitchange;team;bng_rejectionreason;bng_withdrawalreason' `
   --generatesdkmessages `
   --messagenamesfilter 'bng_*' `
   --namespace DEFRA.NE.BNG.Integration.Domain.Models `
   --serviceContextName OrgContext `
   --suppressGeneratedCodeAttribute `
   --suppressINotifyPattern `
   --writesettingsTemplateFile