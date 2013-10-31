namespace ProCenter.Service.Message.Common.Lookups
{
    #region Using Statements

    using System;

    #endregion

    /// <summary>
    ///     Data transfer object for Lookup.
    /// </summary>
    public class LookupDto : IEquatable<LookupDto>, IComparable<LookupDto>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        public bool IsDefault { get; set; }

        public int? SortOrder { get; set; }

        #region IEquatable<LookupDto> Members

        public bool Equals(LookupDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Code, other.Code);
        }

        #endregion

        public int CompareTo(LookupDto other)
        {
            if (Code == null && other.Code == null)
            {
                return 0;
            }
            if (Code == null)
            {
                return other.Code.CompareTo(Code);
            }
            return Code.CompareTo(other.Code);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((LookupDto) obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}