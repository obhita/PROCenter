#region Licence Header
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

    using System;
    using Primitive;

    #endregion

    public class TimeSpanPicker : IConvertible, IPrimitive
    {
        public TimeSpanPicker()
        {
        }

        public TimeSpanPicker(uint? year, uint? month)
        {
            Year = year;
            Month = month;
        }

        public uint? Year { get; private set; }

        public uint? Month { get; private set; }

        /// <summary>
        ///     Returns the <see cref="T:System.TypeCode" /> for this instance.
        /// </summary>
        /// <returns>
        ///     The enumerated constant that is the <see cref="T:System.TypeCode" /> of the class or value type that implements this interface.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A Boolean value equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A Unicode character equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="T:System.Decimal" /> number using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Decimal" /> number equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="T:System.DateTime" /> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.DateTime" /> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an equivalent <see cref="T:System.String" /> using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> instance equivalent to the value of this instance.
        /// </returns>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(TotalMonths(), provider);
        }

        /// <summary>
        ///     Converts the value of this instance to an <see cref="T:System.Object" /> of the specified <see cref="T:System.Type" /> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Object" /> instance of type <paramref name="conversionType" /> whose value is equivalent to the value of this instance.
        /// </returns>
        /// <param name="conversionType">
        ///     The <see cref="T:System.Type" /> to which the value of this instance is converted.
        /// </param>
        /// <param name="provider">
        ///     An <see cref="T:System.IFormatProvider" /> interface implementation that supplies culture-specific formatting information.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        public uint? TotalMonths()
        {
            if (Year == null && Month == null)
            {
                return null;
            }
            if (Year == null)
            {
                return Month;
            }
            if (Month == null)
            {
                return Year*12;
            }
            return (Year*12) + Month;
        }
    }
}