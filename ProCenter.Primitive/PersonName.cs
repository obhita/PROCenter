#region Using Statements

using System;
using System.ComponentModel.DataAnnotations;
using Pillar.Common.Utility;
using Pillar.Domain.Attributes;

#endregion

namespace ProCenter.Primitive
{
    [Component]
    public class PersonName : IPrimitive, IEquatable<PersonName>
    {
        /// <summary>
        ///     Constructor needed for RavenDB
        /// </summary>
        public PersonName()
        {
        }

        public PersonName(string firstName, string lastName)
            : this(null, firstName, null, lastName, null)
        {
        }

        public PersonName(string firstName, string middleName, string lastName)
            : this(null, firstName, middleName, lastName, null)
        {
        }

        public PersonName(string prefix, string firstName, string middleName, string lastName, string suffix)
        {
            Check.IsNotNullOrWhitespace(firstName, () => FirstName);
            Check.IsNotNullOrWhitespace(lastName, () => LastName);

            Prefix = prefix;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Suffix = suffix;
        }

        public virtual string Prefix { get; private set; }

        [Required(ErrorMessage = "The First Name is required.")]
        [NotNull]
        public virtual string FirstName { get; private set; }

        public virtual string MiddleName { get; private set; }

        [Required(ErrorMessage = "The Last Name is required.")]
        [NotNull]
        public virtual string LastName { get; private set; }

        public virtual string Suffix { get; private set; }

        public virtual string FullName { get { return FirstName + " " + LastName; } }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(PersonName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Suffix, Suffix) && Equals(other.LastName, LastName) &&
                   Equals(other.MiddleName, MiddleName) && Equals(other.FirstName, FirstName) &&
                   Equals(other.Prefix, Prefix);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PersonName)) return false;
            return Equals((PersonName) obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Suffix != null ? Suffix.GetHashCode() : 0);
                result = (result*397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                result = (result*397) ^ (MiddleName != null ? MiddleName.GetHashCode() : 0);
                result = (result*397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                result = (result*397) ^ (Prefix != null ? Prefix.GetHashCode() : 0);
                return result;
            }
        }

        public static bool operator ==(PersonName left, PersonName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersonName left, PersonName right)
        {
            return !Equals(left, right);
        }
    }
}