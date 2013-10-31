namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Web.Mvc;
    using Common;
    using Primitive;
    using ProCenter.Service.Message.Assessment;

    #endregion

    public class ResourceModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        private readonly IResourcesManager _resourcesManager;

        public ResourceModelMetadataProvider (IResourcesManager resourcesManager)
        {
            _resourcesManager = resourcesManager;
        }

        #region Public Methods and Operators

        public override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, string propertyName)
        {
            var modelMetadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyName);
            if (typeof (IPrimitive).IsAssignableFrom(containerType))
            {
                modelMetadata.IsRequired = false;
            }
            if (modelMetadata.DisplayName == null)
            {
                if (typeof (IPrimitive).IsAssignableFrom(containerType))
                {
                    modelMetadata.DisplayName = _resourcesManager.GetResourceManagerByName ( containerType.Name ).GetString ( propertyName );
                }
                else
                {
                    var name = containerType.Namespace.Split('.').Last() + "Resources";
                    var resourceManager = _resourcesManager.GetResourceManagerByName(name);
                    modelMetadata.DisplayName = resourceManager.GetString(containerType.Name.Replace("Dto", "_") + propertyName);
                }
            }

            return modelMetadata;
        }

        #endregion
    }
}