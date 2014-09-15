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

namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System.Collections.Generic;

    using ProCenter.Domain.CommonModule;
    using ProCenter.Domain.CommonModule.Lookups;

    #endregion

    /// <summary>The score class.</summary>
    public class Score
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Score" /> class.
        /// </summary>
        public Score ()
        {
            ScoreItems = new List<ScoreItem> ();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Score" /> class.
        /// </summary>
        /// <param name="codedConcept">The coded concept.</param>
        internal Score ( CodedConcept codedConcept )
        {
            CodedConcept = codedConcept;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the coded concept.
        /// </summary>
        /// <value>
        ///     The coded concept.
        /// </value>
        public CodedConcept CodedConcept { get; private set; }

        /// <summary>
        ///     Gets the guidance.
        /// </summary>
        /// <value>
        ///     The guidance.
        /// </value>
        public CodedConcept Guidance { get; internal set; }

        /// <summary>
        ///     Gets or sets the item metadata.
        /// </summary>
        /// <value>
        ///     The item metadata.
        /// </value>
        public ItemMetadata ItemMetadata { get; set; }

        /// <summary>
        ///     Gets the score items.
        /// </summary>
        /// <value>
        ///     The score items.
        /// </value>
        public IEnumerable<ScoreItem> ScoreItems { get; private set; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public object Value { get; internal set; }

        /// <summary>
        ///     Gets the type of the value.
        /// </summary>
        /// <value>
        ///     The type of the value.
        /// </value>
        public Lookup ValueType { get; internal set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has report.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance has report; otherwise, <c>False</c>.
        /// </value>
        public bool HasReport { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>Adds the score item.</summary>
        /// <param name="scoreItem">The score item.</param>
        public void AddScoreItem ( ScoreItem scoreItem )
        {
            ( ScoreItems as IList<ScoreItem> ).Add ( scoreItem );
        }

        /// <summary>Finds the score item.</summary>
        /// <param name="name">The name.</param>
        /// <returns>A <see cref="ScoreItem"/>.</returns>
        public ScoreItem FindScoreItem ( string name )
        {
            foreach ( var scoreItem in ScoreItems )
            {
                var si = FindScoreItemHelper ( scoreItem, name );
                if ( si != null )
                {
                    return si;
                }
            }
            return null;
        }

        /// <summary>Updates the score item.</summary>
        /// <param name="scoreItem">The score item.</param>
        public void UpdateScoreItem ( ScoreItem scoreItem )
        {
            var old = FindScoreItem ( scoreItem.ItemDefinitionCode );
            var index = ( ScoreItems as IList<ScoreItem> ).IndexOf ( old );
            ( ScoreItems as IList<ScoreItem> ).Remove ( old );
            ( ScoreItems as IList<ScoreItem> ).Insert ( index, scoreItem );
        }

        #endregion

        #region Methods

        private ScoreItem FindScoreItemHelper ( ScoreItem score, string name )
        {
            if ( score.ItemDefinitionCode == name )
            {
                return score;
            }
            if ( score.ScoreItems != null )
            {
                foreach ( var scoreItem in score.ScoreItems )
                {
                    var si = FindScoreItemHelper ( scoreItem, name );
                    if ( si != null )
                    {
                        return si;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}