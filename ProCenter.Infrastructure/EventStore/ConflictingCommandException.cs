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
namespace ProCenter.Infrastructure.EventStore
{
    #region Using Statements

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     Represents a command that could not be executed because it conflicted with the command of another user or actor.
    /// </summary>
    [Serializable]
    public class ConflictingCommandException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the ConflictingCommandException class.
        /// </summary>
        public ConflictingCommandException ()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ConflictingCommandException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ConflictingCommandException ( string message )
            : base ( message )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ConflictingCommandException class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The message that is the cause of the current exception.</param>
        public ConflictingCommandException ( string message, Exception innerException )
            : base ( message, innerException )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the ConflictingCommandException class.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data of the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected ConflictingCommandException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }

        #endregion
    }
}