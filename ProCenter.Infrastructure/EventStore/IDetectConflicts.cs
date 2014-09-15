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

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     Delegate for detecting event store conflicts.
    /// </summary>
    /// <param name="uncommitted">The uncommitted.</param>
    /// <param name="committed">The committed.</param>
    /// <returns>Whether there is a conflict.</returns>
    public delegate bool ConflictDelegate ( object uncommitted, object committed );

    /// <summary>Interface for detecting event store conflicts.</summary>
    public interface IDetectConflicts
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Conflictses the with.
        /// </summary>
        /// <param name="uncommittedEvents">The uncommitted events.</param>
        /// <param name="committedEvents">The committed events.</param>
        /// <returns>Whether there are conficts.</returns>
        bool ConflictsWith ( IEnumerable<object> uncommittedEvents, IEnumerable<object> committedEvents );

        /// <summary>
        ///     Registers the specified handler.
        /// </summary>
        /// <typeparam name="TUncommitted">The type of the uncommitted.</typeparam>
        /// <typeparam name="TCommitted">The type of the committed.</typeparam>
        /// <param name="handler">The handler.</param>
        void Register<TUncommitted, TCommitted> ( ConflictDelegate handler )
            where TUncommitted : class
            where TCommitted : class;

        #endregion
    }
}