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

namespace ProCenter.Domain.Nida
{
    #region Using Statements

    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

    using ProCenter.Common.Report;

    #endregion

    /// <summary>
    ///     Data collection for NIDA Report.
    /// </summary>
    public class NidaReportDataCollection : ArrayList, ITypedList
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Returns the <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties on
        ///     each item used to bind data.
        /// </summary>
        /// <param name="listAccessors">
        ///     An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects to find in
        ///     the collection as bindable. This can be null.
        /// </param>
        /// <returns>
        ///     The <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> that represents the properties on each item
        ///     used to bind data.
        /// </returns>
        public PropertyDescriptorCollection GetItemProperties ( PropertyDescriptor[] listAccessors )
        {
            if ( listAccessors != null && listAccessors.Length > 0 )
            {
                var listAccessor = listAccessors[listAccessors.Length - 1];
                if ( listAccessor.PropertyType == typeof(List<ReportString>) )
                {
                    return TypeDescriptor.GetProperties ( typeof(ReportString) );
                }
            }
            return TypeDescriptor.GetProperties ( typeof(NidaReportData) );
        }

        /// <summary>
        ///     Returns the name of the list.
        /// </summary>
        /// <param name="listAccessors">
        ///     An array of <see cref="T:System.ComponentModel.PropertyDescriptor" /> objects, for which
        ///     the list name is returned. This can be null.
        /// </param>
        /// <returns>
        ///     The name of the list.
        /// </returns>
        public string GetListName ( PropertyDescriptor[] listAccessors )
        {
            return "NidaReportData";
        }

        #endregion
    }
}