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
namespace ProCenter.Service.Message.Common
{
    #region Using Statements

    using System.ComponentModel.DataAnnotations;
    using Lookups;

    #endregion

    /// <summary>
    ///     Address data transfer object.
    /// </summary>
    public class AddressDto
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the name of the city.
        /// </summary>
        /// <value>
        ///     The name of the city.
        /// </value>
        public string CityName { get; set; }

        /// <summary>
        ///     Gets or sets the first street address.
        /// </summary>
        /// <value>
        ///     The first street address.
        /// </value>
        public string FirstStreetAddress { get; set; }

        /// <summary>
        ///     Gets or sets the postal code.
        /// </summary>
        /// <value>
        ///     The postal code.
        /// </value>
        public string PostalCode { get; set; }

        /// <summary>
        ///     Gets or sets the second street address.
        /// </summary>
        /// <value>
        ///     The second street address.
        /// </value>
        [Display(Name = "Street address line 2")]
        public string SecondStreetAddress { get; set; }

        /// <summary>
        ///     Gets or sets the state province.
        /// </summary>
        /// <value>
        ///     The state province.
        /// </value>
        public LookupDto StateProvince { get; set; }

        #endregion
    }
}