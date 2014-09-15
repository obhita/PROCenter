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

namespace ProCenter.Mvc.Infrastructure.BrowserDetection
{
    #region Using Statements

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The SupportedBrowser Interface.
    /// </summary>
    public interface ISupportedBrowser
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets or sets the name of the browser.
        /// </summary>
        /// <value>
        ///     The name of the browser.
        /// </value>
        string BrowserName { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; set; }

        /// <summary>
        ///     Gets or sets the type of the machine.
        /// </summary>
        /// <value>
        ///     The type of the machine.
        /// </value>
        string MachineType { get; set; }

        /// <summary>
        ///     Gets or sets the support status.
        /// </summary>
        /// <value>
        ///     The support status.
        /// </value>
        SupportedBrowser.SupportStatusEnum SupportStatus { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        double Version { get; set; }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="machineType">Type of the machine.</param>
        /// <returns>A list of string with formated browser information.</returns>
        List<string> GetList ( string machineType );

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>A formatted message string.</returns>
        string GetMessage ();

        #endregion
    }
}