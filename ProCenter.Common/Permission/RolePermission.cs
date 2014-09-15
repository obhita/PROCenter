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

namespace ProCenter.Common.Permission
{
    #region Using Statements

    using Pillar.Security.AccessControl;

    #endregion

    /// <summary>The role permission class.</summary>
    public class RolePermission
    {
        #region Static Fields

        /// <summary>
        /// Gets the role add permission permission.
        /// </summary>
        /// <value>
        /// The role add permission permission.
        /// </value>
        public static Permission RoleAddPermissionPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "securitynmodule/roleaddpermission",
                       };
            }
        }

        /// <summary>
        /// Gets the role edit permission.
        /// </summary>
        /// <value>
        /// The role edit permission.
        /// </value>
        public static Permission RoleEditPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "securitynmodule/roleedit",
                       };
            }
        }

        /// <summary>
        /// Gets the role remove permission permission.
        /// </summary>
        /// <value>
        /// The role remove permission permission.
        /// </value>
        public static Permission RoleRemovePermissionPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "securitynmodule/roleremovepermission",
                       };
            }
        }

        /// <summary>
        /// Gets the role view permission.
        /// </summary>
        /// <value>
        /// The role view permission.
        /// </value>
        public static Permission RoleViewPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "securitynmodule/roleview",
                       };
            }
        }

        #endregion
    }
}