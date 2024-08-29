using System.Reflection;
using DEFRA.NE.BNG.Integration.Domain.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace DEFRA.NE.BNG.Integration.Domain.Entities
{
    public static class DataverseExtensions
    {
        public static string AttributeLogicalName(this Entity entity, string propertyName)
        {
            var properties = entity.GetType().GetProperties();
            var attributeName = RetrieveAttributeName(propertyName, properties);

            return attributeName;
        }

        public static string AttributeLogicalName<T>(string propertyName) where T : Entity
        {
            var properties = typeof(T).GetProperties();
            var attributeName = RetrieveAttributeName(propertyName, properties);

            return attributeName;
        }

        public static void AddEqualOperatorCondition<T>(this QueryExpression query, string propertyName, object value) where T : Entity
        {
            query.Criteria.AddCondition(AttributeLogicalName<T>(propertyName), ConditionOperator.Equal, value);
        }

        public static QueryExpression GetQuery<T>() where T : Entity
        {
            QueryExpression query = null;

            query = GenerateQueryForOutOfBoxEntities<T>(query);

            if (query == null)
            {
                query = GenerateQueryForCustomEntities<T>(query);
            }

            if (query == null)
            {
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported!");
            }

            query.Criteria = new FilterExpression(LogicalOperator.And);
            return query;
        }

        public static EntityReference GetEntityReference<T>(this Guid id) where T : Entity
        {
            var entity = (Entity)Activator.CreateInstance(typeof(T));
            EntityReference entityReference = new(entity.LogicalName, id);

            return entityReference;
        }

        public static string GetRelationshipSchemaName<T>(string propertyName) where T : Entity
        {
            string attributeName = null;

            var properties = typeof(T).GetProperties();

            if (properties?.Length > 0)
            {
                var prop = Array.Find(properties, x => x.Name == propertyName);

                if (prop != null)
                {
                    var attributes = Attribute.GetCustomAttributes(prop);

                    if (attributes?.Length > 0)
                    {
                        var propertyAttribute = Array.Find(attributes, x => x.GetType().Name == nameof(RelationshipSchemaNameAttribute));

                        if (propertyAttribute != null)
                        {
                            attributeName = ((RelationshipSchemaNameAttribute)propertyAttribute).SchemaName;
                        }
                        else
                        {
                            throw new InvalidOperationException("Relationship not found!");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Relationship not found!");
                }
            }

            return attributeName;
        }

        public static ColumnSet AddTableColumns<T>(this ColumnSet columnSet, List<string> columns) where T : Entity
        {
            foreach (var column in columns)
            {
                columnSet.Columns.Add(AttributeLogicalName<T>(column));
            }

            return columnSet;
        }

        private static QueryExpression GenerateQueryForCustomEntities<T>(QueryExpression query) where T : Entity
        {
            if (typeof(T) == typeof(bng_LocalPlanningAuthority))
            {
                query = new(bng_LocalPlanningAuthority.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                        AttributeLogicalName<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_lpaid)),
                                       AttributeLogicalName<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_Name)),
                                      AttributeLogicalName<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_EastingsCoordinate)),
                                      AttributeLogicalName<bng_LocalPlanningAuthority>(nameof(bng_LocalPlanningAuthority.bng_NorthingsCoordinates)))
                };
            }
            else if (typeof(T) == typeof(bng_BankDetails))
            {
                query = new(bng_BankDetails.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                  AttributeLogicalName<bng_BankDetails>(nameof(bng_BankDetails.bng_AccountName)),
              AttributeLogicalName<bng_BankDetails>(nameof(bng_BankDetails.bng_AccountNumber)),
             AttributeLogicalName<bng_BankDetails>(nameof(bng_BankDetails.bng_SortCode))
             )
                };
            }
            else if (typeof(T) == typeof(bng_Nationality))
            {
                query = new(bng_Nationality.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                  AttributeLogicalName<bng_Nationality>(nameof(bng_Nationality.bng_Name))
                                             )
                };
            }
            else if (typeof(T) == typeof(bng_Country))
            {
                query = new(bng_Country.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<bng_Country>(nameof(bng_Country.bng_Name)),
                                                AttributeLogicalName<bng_Country>(nameof(bng_Country.bng_CountryId))
                                             )
                };
            }
            else if (typeof(T) == typeof(bng_fees))
            {
                query = new(bng_fees.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                  AttributeLogicalName<bng_fees>(nameof(bng_fees.bng_casetype)),
                                                  AttributeLogicalName<bng_fees>(nameof(bng_fees.bng_fee))
                                             )
                };
            }
            else if (typeof(T) == typeof(bng_GainSiteRegistration))
            {
                query = new(bng_GainSiteRegistration.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_CaseType)),
                                                AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.bng_GainSiteReference)),
                                                AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.statecode)),
                                                AttributeLogicalName<bng_GainSiteRegistration>(nameof(bng_GainSiteRegistration.statuscode))
                                            )
                };
            }
            else if (typeof(T) == typeof(bng_HabitatType))
            {
                query = new(bng_HabitatType.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_HabitatName)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_Size)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_HabitatState)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_unitsdelivered)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_UnitofMeasure)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_HabitatType1)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_ProposedHabitatSubTypeLookup)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_Size)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_Allocated)),
                                                AttributeLogicalName<bng_HabitatType>(nameof(bng_HabitatType.bng_Remaining))
                                            )
                };
            }
            else if (typeof(T) == typeof(bng_AllocatedHabitats))
            {
                query = new(bng_AllocatedHabitats.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<bng_AllocatedHabitats>(nameof(bng_AllocatedHabitats.bng_HabitatID)),
                                                AttributeLogicalName<bng_AllocatedHabitats>(nameof(bng_AllocatedHabitats.bng_DeveloperRegistrationID)),
                                                AttributeLogicalName<bng_AllocatedHabitats>(nameof(bng_AllocatedHabitats.bng_GainSiteRegistrationID)),
                                                AttributeLogicalName<bng_AllocatedHabitats>(nameof(bng_AllocatedHabitats.bng_AllocatedtothisDevelopment))
                                            )
                };
            }
            else if (typeof(T) == typeof(bng_BNGConfiguration))
            {
                query = new(bng_BNGConfiguration.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<bng_BNGConfiguration>(nameof(bng_BNGConfiguration.bng_Standard)),
                                                AttributeLogicalName<bng_BNGConfiguration>(nameof(bng_BNGConfiguration.bng_Reduced)),
                                                AttributeLogicalName<bng_BNGConfiguration>(nameof(bng_BNGConfiguration.bng_Zero)),
                                                AttributeLogicalName<bng_BNGConfiguration>(nameof(bng_BNGConfiguration.bng_Name))
                                              )
                };
            }
            else if (typeof(T) == typeof(bng_DeveloperRegistration))
            {
                query = new(bng_DeveloperRegistration.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                        AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_ApplicantID)),
                                        AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_ReferenceNumber)),
                                        AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_localplanningauthority)),
                                        AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_PlanningReference)),
                                        AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_ProjectName)),
                                        AttributeLogicalName<bng_DeveloperRegistration>(nameof(bng_DeveloperRegistration.bng_source))
                                            )
                };
            }
            else if (typeof(T) == typeof(bng_Case))
            {
                query = new(bng_Case.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_CaseId)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_GainSiteRegistrationID)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_DeveloperRegistrationId)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_RejectionReasonID)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Case_Type)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_paymentdetails)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Deadline)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_withdrawalreason)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.CreatedOn)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_CaseReference)),
                                              AttributeLogicalName<bng_Case>(nameof(bng_Case.bng_Applyingonbehalfof))
                                            )
                };
            }
            else if (typeof(T) == typeof(bng_Notify))
            {
                query = new(bng_Notify.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ActionType)),
                                                AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_ApplicantID)),
                                                AttributeLogicalName<bng_Notify>(nameof(bng_Notify.RegardingObjectId)),
                                                AttributeLogicalName<bng_Notify>(nameof(bng_Notify.bng_RetryCount)),
                                                AttributeLogicalName<bng_Notify>(nameof(bng_Notify.Description))
                                             )
                };
            }

            return query;
        }

        private static QueryExpression GenerateQueryForOutOfBoxEntities<T>(QueryExpression query) where T : Entity
        {
            if (typeof(T) == typeof(Contact))
            {
                query = new(Contact.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                              AttributeLogicalName<Contact>(nameof(Contact.ContactId)),
                                              AttributeLogicalName<Contact>(nameof(Contact.bng_DefraID)),
                                              AttributeLogicalName<Contact>(nameof(Contact.LastName)),
                                             AttributeLogicalName<Contact>(nameof(Contact.JobTitle)),
                                             AttributeLogicalName<Contact>(nameof(Contact.EMailAddress1))
                                            )
                };
            }
            if (typeof(T) == typeof(Models.Product))
            {
                query = new(Models.Product.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<Models.Product>(nameof(Models.Product.ProductId)),
                                                AttributeLogicalName<Models.Product>(nameof(Models.Product.ProductNumber)),
                                                AttributeLogicalName<Models.Product>(nameof(Models.Product.Name)),
                                                AttributeLogicalName<Models.Product>(nameof(Models.Product.DefaultUoMId)),
                                                AttributeLogicalName<Models.Product>(nameof(Models.Product.QuantityDecimal)),
                                                AttributeLogicalName<Models.Product>(nameof(Models.Product.Price))
                                            )
                };
            }
            else if (typeof(T) == typeof(Account))
            {
                query = new(Account.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<Account>(nameof(Account.bng_DefraID)),
                                                AttributeLogicalName<Account>(nameof(Account.Name)),
                                                AttributeLogicalName<Account>(nameof(Account.bng_DefraID)),
                                                AttributeLogicalName<Account>(nameof(Account.EMailAddress1)),
                                                AttributeLogicalName<Account>(nameof(Account.AccountId))
                                            )
                };
            }
            else if (typeof(T) == typeof(SalesOrder))
            {
                query = new(SalesOrder.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.CustomerId)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.bng_ponumber)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.bng_DeveloperRegistration)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.bng_CustomerContactID)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.bng_Source)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.OwnerId)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.SalesOrderId)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.TotalAmount)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.OrderNumber)),
                                                AttributeLogicalName<SalesOrder>(nameof(SalesOrder.Name))
                                             )
                };
            }
            else if (typeof(T) == typeof(UoM))
            {
                query = new(UoM.EntityLogicalName)
                {
                    ColumnSet = new ColumnSet(
                  AttributeLogicalName<UoM>(nameof(UoM.Name))
             )
                };
            }

            return query;
        }

        private static string RetrieveAttributeName(string propertyName, PropertyInfo[] properties)
        {
            string attributeName = null;
            if (properties?.Length > 0)
            {
                var prop = Array.Find(properties, x => x.Name == propertyName);

                if (prop != null)
                {
                    var attributes = Attribute.GetCustomAttributes(prop);

                    if (attributes?.Length > 0)
                    {
                        var propertyAttribute = Array.Find(attributes, x => x.GetType().Name == nameof(AttributeLogicalNameAttribute));

                        if (propertyAttribute != null)
                        {
                            attributeName = ((AttributeLogicalNameAttribute)propertyAttribute).LogicalName;
                        }
                        else
                        {
                            throw new InvalidOperationException("Attribute not found!");
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Attribute not found!");
                }
            }

            return attributeName;
        }
    }
}