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
namespace ProCenter.Domain.CommonModule
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>
    ///     Class for holding standardization information.
    /// </summary>
    public class CodedConcept : IValueObject, IEquatable<CodedConcept>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="CodedConcept" /> class.
        /// </summary>
        /// <param name="codeSystem">The code system.</param>
        /// <param name="code">The code.</param>
        /// <param name="name">The name.</param>
        public CodedConcept ( CodeSystem codeSystem, string code, string name )
        {
            CodeSystem = codeSystem;
            Code = code;
            Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        /// <value>
        ///     The code.
        /// </value>
        public string Code { get; protected set; }

        /// <summary>
        ///     Gets or sets the code system.
        /// </summary>
        /// <value>
        ///     The code system.
        /// </value>
        public CodeSystem CodeSystem { get; protected set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; protected set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     ==s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator == ( CodedConcept left, CodedConcept right )
        {
            return Equals ( left, right );
        }

        /// <summary>
        ///     !=s the specified left.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool operator != ( CodedConcept left, CodedConcept right )
        {
            return !Equals ( left, right );
        }

        /// <summary>
        ///     Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals ( CodedConcept other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return Equals ( CodeSystem, other.CodeSystem ) && string.Equals ( Code, other.Code );
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="System.Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( null, obj ) )
                return false;
            if ( ReferenceEquals ( this, obj ) )
                return true;
            if ( obj.GetType () != this.GetType () )
                return false;
            return Equals ( (CodedConcept) obj );
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode ()
        {
            unchecked
            {
                return ( ( CodeSystem != null ? CodeSystem.GetHashCode () : 0 ) * 397 ) ^ ( Code != null ? Code.GetHashCode () : 0 );
            }
        }

        #endregion
    }
}