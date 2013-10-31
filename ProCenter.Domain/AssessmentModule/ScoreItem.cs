namespace ProCenter.Domain.AssessmentModule
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using CommonModule.Lookups;
    using Event;

    #endregion

    public class ScoreItem : IEquatable<ScoreItem>
    {
        public bool Equals ( ScoreItem other )
        {
            if ( ReferenceEquals ( null, other ) )
                return false;
            if ( ReferenceEquals ( this, other ) )
                return true;
            return string.Equals ( ItemDefinitionCode, other.ItemDefinitionCode ) && Equals ( Value, other.Value );
        }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals ( null, obj ) )
                return false;
            if ( ReferenceEquals ( this, obj ) )
                return true;
            if ( obj.GetType () != this.GetType () )
                return false;
            return Equals ( (ScoreItem) obj );
        }

        public override int GetHashCode ()
        {
            unchecked
            {
                return ( ( ItemDefinitionCode != null ? ItemDefinitionCode.GetHashCode () : 0 ) * 397 ) ^ ( Value != null ? Value.GetHashCode () : 0 );
            }
        }

        public static bool operator == ( ScoreItem left, ScoreItem right )
        {
            return Equals ( left, right );
        }

        public static bool operator != ( ScoreItem left, ScoreItem right )
        {
            return !Equals ( left, right );
        }

        public ScoreItem(string itemDefinitionCode, object value, params ScoreItem[] scoreItems)
        {
            ItemDefinitionCode = itemDefinitionCode;
            Value = value;
            ScoreItems = scoreItems;
        }

        public string ItemDefinitionCode { get; private set; }

        public Lookup ValueType { get; private set; } //todo: need this or list of SocreItems?

        public object Value { get; private set; }

        public ItemMetadata ItemMetadata { get; set; }

        public IEnumerable<ScoreItem> ScoreItems { get; private set; }

        public void UpdateValue ( object value )
        {
            Value = value;
        }
    }
}