namespace ProCenter.Domain.AssessmentModule
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Windows.Forms.VisualStyles;

    using DevExpress.Data.PLinq.Helpers;

    using Pillar.Common.InversionOfControl;

    using ProCenter.Domain.AssessmentModule.Attributes;
    using ProCenter.Domain.AssessmentModule.Lookups;
    using ProCenter.Domain.AssessmentModule.Metadata;
    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    /// <summary>The assessment part class.</summary>
    public abstract class AssessmentPart
    {
        /// <summary>Initializes a new instance of the <see cref="AssessmentPart"/> class.</summary>
        /// <param name="assessmentInstance">The assessment instance.</param>
        protected AssessmentPart(AssessmentInstance assessmentInstance)
        {
            if (assessmentInstance != null)
            {
                foreach (var propertyInfo in GetOrderedProperties())
                {
                    var itemDefinitionCode = propertyInfo.GetCustomAttribute<CodeAttribute>().Value;
                    var itemInstance = assessmentInstance.ItemInstances.FirstOrDefault(i => i.ItemDefinitionCode == itemDefinitionCode);
                    if (itemInstance != null)
                    {
                        if ( propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition () == typeof(IEnumerable<>) )
                        {
                            propertyInfo.SetValue(this, itemInstance.Value);
                        }
                        else if ( itemInstance.Value.GetType () != propertyInfo.PropertyType )
                        {
                            propertyInfo.SetValue ( this, Convert.ChangeType ( itemInstance.Value, propertyInfo.PropertyType ) );
                        }
                        else
                        {
                            propertyInfo.SetValue(this, itemInstance.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the ordered properties.
        /// </summary>
        /// <returns>Returns an IEnumerable of PropertyInfo.</returns>
        protected IEnumerable<PropertyInfo> GetOrderedProperties ()
        {
            return GetType ().GetProperties ( BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly ).OrderBy ( DisplayOrderAttribute.GetOrder );
        }

        /// <summary>Gets the coded concept.</summary>
        /// <typeparam name="T">The type of assessment part.</typeparam>
        /// <returns>A <see cref="CodedConcept" />.</returns>
        public static CodedConcept GetCodedConcept<T>()
        {
            return GetCodedConcept(typeof(T));
        }

        /// <summary>Gets the property coded concept.</summary>
        /// <typeparam name="T">The type of assessment part.</typeparam>
        /// <typeparam name="TPropertyType">The type of the property type.</typeparam>
        /// <param name="propertyExpression">The property expression.</param>
        /// <returns>A <see cref="CodedConcept" />.</returns>
        public static CodedConcept GetPropertyCodedConcept<T, TPropertyType>(Expression<Func<T, TPropertyType>> propertyExpression)
        {
            var type = typeof(T);

            MemberExpression memberExpression = null;

            switch (propertyExpression.Body.NodeType)
            {
                case ExpressionType.Convert:

                    var unaryExpression = propertyExpression.Body as UnaryExpression;
                    if (unaryExpression != null)
                    {
                        memberExpression = unaryExpression.Operand as MemberExpression;
                    }

                    break;

                case ExpressionType.MemberAccess:
                    memberExpression = propertyExpression.Body as MemberExpression;
                    break;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            return GetCodedConcept(propertyInfo, type.GetCustomAttribute<CodeSystemAttribute>().CodeSystem);
        }

        /// <summary>Gets the coded concept.</summary>
        /// <param name="type">The type.</param>
        /// <returns>A <see cref="CodedConcept" />.</returns>
        protected static CodedConcept GetCodedConcept(Type type)
        {
            return GetCodedConcept(type, type.GetCustomAttribute<CodeSystemAttribute>().CodeSystem);
        }

        /// <summary>
        /// Gets the type of the score.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A <see cref="ScoreTypeEnum" />.</returns>
        protected static ScoreTypeEnum GetScoreType(Type type)
        {
            return type.GetCustomAttribute<ScoreTypeAttribute>().Value;
        }

        /// <summary>Gets the coded concept.</summary>
        /// <param name="type">The type.</param>
        /// <param name="defaultCodeSystem">The default code system.</param>
        /// <returns>A <see cref="CodedConcept" />.</returns>
        protected static CodedConcept GetCodedConcept(Type type, CodeSystem defaultCodeSystem)
        {
            var codeSystemAttribute = type.GetCustomAttribute<CodeSystemAttribute>();
            var codedConcept = new CodedConcept(
                codeSystemAttribute == null
                    ? defaultCodeSystem
                    : codeSystemAttribute.CodeSystem,
                type.GetCustomAttribute<CodeAttribute>().Value,
                type.Name);
            return codedConcept;
        }

        /// <summary>Gets the coded concept.</summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="defaultCodeSystem">The default code system.</param>
        /// <returns>A <see cref="CodedConcept" />.</returns>
        protected static CodedConcept GetCodedConcept(PropertyInfo propertyInfo, CodeSystem defaultCodeSystem)
        {
            var codeSystemAttribute = propertyInfo.GetCustomAttribute<CodeSystemAttribute>();
            var codedConcept = new CodedConcept(
                codeSystemAttribute == null ? defaultCodeSystem : codeSystemAttribute.CodeSystem,
                propertyInfo.GetCustomAttribute<CodeAttribute>().Value,
                propertyInfo.Name);
            return codedConcept;
        }

        /// <summary>Creates the item definition for question property.</summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="defaultCodeSystem">The default code system.</param>
        /// <returns>A <see cref="ItemDefinition"/>.</returns>
        protected static ItemDefinition CreateItemDefinitionForQuestionProperty ( PropertyInfo propertyInfo, CodeSystem defaultCodeSystem )
        {
            var valueTypeAttribute = propertyInfo.GetCustomAttribute<ValueTypeAttribute> ();
            var valueType = valueTypeAttribute == null ? null : valueTypeAttribute.ValueType;
            IEnumerable<Lookup> lookupOptions = null;
            if ( propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition () == typeof(IEnumerable<>) &&
                 typeof(Lookup).IsAssignableFrom ( propertyInfo.PropertyType.GetGenericArguments ()[0] ) )
            {
                var lookupType = propertyInfo.PropertyType.GetGenericArguments ()[0];
                var lookupProvider = IoC.CurrentContainer.Resolve<ILookupProvider>();
                lookupOptions = lookupProvider.GetAll(lookupType.Name).ToList();
            }
            else if ( typeof(Lookup).IsAssignableFrom ( propertyInfo.PropertyType ) )
            {
                var lookupProvider = IoC.CurrentContainer.Resolve<ILookupProvider> ();
                lookupOptions = lookupProvider.GetAll ( propertyInfo.PropertyType.Name ).ToList ();
            }
            var itemDefinition = new ItemDefinition ( GetCodedConcept ( propertyInfo, defaultCodeSystem ), ItemType.Question, valueType, lookupOptions  )
                                 {
                                     ItemMetadata = new ItemMetadata ()
                                 };

            var metadataAttributes = propertyInfo.GetCustomAttributes(typeof(MetadataAttribute));

            foreach (var metadataAttribute in metadataAttributes.OfType<MetadataAttribute>())
            {
                itemDefinition.ItemMetadata.AddMetadata(metadataAttribute.MetadataItem);
            }

            if ( propertyInfo.PropertyType.IsValueType && !itemDefinition.GetIsRequired () )
            {
                itemDefinition.ItemMetadata.AddMetadata(new RequiredForCompletenessMetadataItem(CompletenessCategory.Report));
            }

            itemDefinition.ItemMetadata.AddMetadata(
                                            new ItemTemplateMetadataItem
                                            {
                                                TemplateName = itemDefinition.GetTemplateName(propertyInfo)
                                            });

            return itemDefinition;
        }
    }
}