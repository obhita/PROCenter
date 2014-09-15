#region License Header

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

namespace ProCenter.Mvc.PermissionDescriptor
{
    #region Using Statements

    using Common.Permission;
    using Controllers;
    using Pillar.Security.AccessControl;
    using ProCenter.Infrastructure.Security;

    #endregion

    /// <summary>The basic access permission descriptor class.</summary>
    public class BasicAccessPermissionDescriptor : IInternalPermissionDescriptor
    {
        #region Fields

        private readonly ResourceList _resourceList = new ResourceListBuilder ()
            .AddResource<HomeController> ( BasicAccessPermission.AccessUserInterfacePermission )
            .AddResource<AccountController> ( BasicAccessPermission.AccessUserInterfacePermission )
            .AddResource<ErrorController> ( BasicAccessPermission.AccessUserInterfacePermission );

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether [is internal].
        /// </summary>
        /// <value>
        ///   <c>True</c> if [is internal]; otherwise, <c>false</c>.
        /// </value>
        public bool IsInternal
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        public ResourceList Resources
        {
            get { return _resourceList; }
        }

        #endregion
    }
}