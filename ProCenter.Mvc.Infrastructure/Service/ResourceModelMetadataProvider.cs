﻿#region License Header

// /*******************************************************************************
//  * Open Behavioral Health Information Technology Architecture (OBHITA.org)
//  * 
//  * Redistribution and use in source and binary forms, with or without
//  * modification, are permitted provided that the following conditions are met:
//  *     * Redistributions of source code must retain the above copyright
//  *       notice, this list of conditions and the following disclaimer.
//  *     * Redistributions in binary form must reproduce the above copyright
//  *       notice, this list of conditions and the following disclaimer in the
//  *       documentation and/or other materials provided with the distribution.
//  *     * Neither the name of the <organization> nor the
//  *       names of its contributors may be used to endorse or promote products
//  *       derived from this software without specific prior written permission.
//  * 
//  * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  ******************************************************************************/

#endregion

namespace ProCenter.Mvc.Infrastructure.Service
{
    #region Using Statements

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Common;
    using Primitive;

    #endregion

    /// <summary>The resource model metadata provider class.</summary>
    public class ResourceModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        #region Fields

        private readonly IResourcesManager _resourcesManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceModelMetadataProvider"/> class.
        /// </summary>
        /// <param name="resourcesManager">The resources manager.</param>
        public ResourceModelMetadataProvider ( IResourcesManager resourcesManager )
        {
            _resourcesManager = resourcesManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the metadata for the specified property using the container type and property name.
        /// </summary>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        /// The metadata for the specified property using the container type and property name.
        /// </returns>
        public override ModelMetadata GetMetadataForProperty ( Func<object> modelAccessor, Type containerType, string propertyName )
        {
            var modelMetadata = base.GetMetadataForProperty ( modelAccessor, containerType, propertyName );
            if ( typeof(IPrimitive).IsAssignableFrom ( containerType ) )
            {
                modelMetadata.IsRequired = false;
            }
            if ( modelMetadata.DisplayName == null )
            {
                if ( typeof(IPrimitive).IsAssignableFrom ( containerType ) )
                {
                    modelMetadata.DisplayName = _resourcesManager.GetResourceManagerByName ( containerType.Name ).GetString ( propertyName );
                }
                else
                {
                    var name = containerType.Namespace.Split ( '.' ).Last () + "Resources";
                    var resourceManager = _resourcesManager.GetResourceManagerByName ( name );
                    modelMetadata.DisplayName = resourceManager.GetString ( containerType.Name.Replace ( "Dto", "_" ) + propertyName );
                }
            }

            return modelMetadata;
        }

        #endregion
    }
}