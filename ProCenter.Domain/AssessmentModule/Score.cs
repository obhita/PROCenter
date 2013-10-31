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
    using CommonModule;
    using CommonModule.Lookups;

    #endregion

    public class Score
    {

        public Score()
        {
            ScoreItems = new List<ScoreItem> ();
        }
        
        internal Score(CodedConcept codedConcept)
        {
            CodedConcept = codedConcept;
        }

        public CodedConcept CodedConcept { get; private set; }

        public IEnumerable<ScoreItem> ScoreItems { get; private set; }

        public CodedConcept Guidance { get; internal set; }

        public Lookup ValueType { get; internal set; } // when to assign its value more from Tony

        public object Value { get; internal set; }

        public ItemMetadata ItemMetadata { get; set; }

        public void AddScoreItem(ScoreItem scoreItem)
        {
            (ScoreItems as IList<ScoreItem>).Add(scoreItem);
        }

        public void UpdateScoreItem ( ScoreItem scoreItem )
        {
            var old = FindScoreItem ( scoreItem.ItemDefinitionCode );
            var index = (ScoreItems as IList<ScoreItem>).IndexOf(old);
            (ScoreItems as IList<ScoreItem>).Remove(old);
            (ScoreItems as IList<ScoreItem>).Insert(index, scoreItem);
        }

        public ScoreItem FindScoreItem(string name)
        {
            foreach (var scoreItem in ScoreItems)
            {
                var si = FindScoreItemHelper(scoreItem, name);
                if (si != null)
                {
                    return si;
                }
            }
            return null;
        }

        private ScoreItem FindScoreItemHelper(ScoreItem score, string name)
        {
            if (score.ItemDefinitionCode == name)
            {
                return score;
            }
            if (score.ScoreItems != null)
            {
                foreach (var scoreItem in score.ScoreItems)
                {
                    var si = FindScoreItemHelper(scoreItem, name);
                    if (si != null)
                    {
                        return si;
                    }
                }
            }
            return null;
        }
    }
}