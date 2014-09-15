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

namespace ProCenter.Infrastructure.Service
{
    /// <summary>Manager for handling Identity server account functions.</summary>
    public interface ISystemAccountIdentityServiceManager
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Changes the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns>The response from the identity service.</returns>
        IdentityServiceResponse ChangePassword ( string email, string oldPassword, string newPassword );

        /// <summary>
        ///     Creates the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        IdentityServiceResponse Create ( string email );

        /// <summary>
        ///     Locks the specified email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        IdentityServiceResponse Lock ( string email );

        /// <summary>
        ///     Resets the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        IdentityServiceResponse ResetPassword ( string email );

        /// <summary>
        ///     Uns the lock.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>The response from the identity service.</returns>
        IdentityServiceResponse UnLock ( string email );

        #endregion
    }
}