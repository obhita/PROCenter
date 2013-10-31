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