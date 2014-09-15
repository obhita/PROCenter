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

    /// <summary>The staff permission class.</summary>
    public class StaffPermission
    {
        #region Static Fields

        /// <summary>
        /// Gets the staff add role permission.
        /// </summary>
        /// <value>
        /// The staff add role permission.
        /// </value>
        public static Permission StaffAddRolePermission
        {
            get
            {
                return new Permission
                       {
                           Name = "organizationmodule/staffaddrole"
                       };
            }
        }

        /// <summary>
        /// Gets the staff create account permission.
        /// </summary>
        /// <value>
        /// The staff create account permission.
        /// </value>
        public static Permission StaffCreateAccountPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "organizationmodule/staffcreateaccount"
                       };
            }
        }

        /// <summary>
        /// Gets the staff edit permission.
        /// </summary>
        /// <value>
        /// The staff edit permission.
        /// </value>
        public static Permission StaffEditPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "organizationmodule/staffedit"
                       };
            }
        }

        /// <summary>
        /// Gets the staff link account permission.
        /// </summary>
        /// <value>
        /// The staff link account permission.
        /// </value>
        public static Permission StaffLinkAccountPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "organizationmodule/stafflinkaccount"
                       };
            }
        }

        /// <summary>
        /// Gets the staff remove role permission.
        /// </summary>
        /// <value>
        /// The staff remove role permission.
        /// </value>
        public static Permission StaffRemoveRolePermission
        {
            get
            {
                return new Permission
                       {
                           Name = "organizationmodule/staffremoverole"
                       };
            }
        }

        /// <summary>
        /// Gets the staff view permission.
        /// </summary>
        /// <value>
        /// The staff view permission.
        /// </value>
        public static Permission StaffViewPermission
        {
            get
            {
                return new Permission
                       {
                           Name = "organizationmodule/staffview"
                       };
            }
        }

        #endregion
    }
}