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

namespace ProCenter.Mvc.Infrastructure.Binder
{
    #region Using Statements

    using System;
    using System.Globalization;
    using System.Web.Mvc;

    #endregion

    /// <summary>The nullable handling value provider result class.</summary>
    public class NullableHandlingValueProviderResult : ValueProviderResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NullableHandlingValueProviderResult"/> class.
        /// </summary>
        /// <param name="valueProviderResult">The value provider result.</param>
        public NullableHandlingValueProviderResult ( ValueProviderResult valueProviderResult )
            : base ( valueProviderResult.RawValue, valueProviderResult.AttemptedValue, valueProviderResult.Culture )
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>Converts the value that is encapsulated by this result to the specified type by using the specified culture information.</summary>
        /// <param name="type">The target type.</param>
        /// <param name="culture">The culture to use in the conversion.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertTo ( Type type, CultureInfo culture )
        {
            var value = RawValue;
            if ( RawValue == null || type.IsInstanceOfType ( value ) )
            {
                return RawValue;
            }
            var nullableType = Nullable.GetUnderlyingType ( type );
            return base.ConvertTo ( nullableType ?? type, culture );
        }

        #endregion
    }
}