#region Using Statements

using System;
using Pillar.Common.Utility;
using Pillar.Domain.Attributes;

#endregion

namespace ProCenter.Domain.CommonModule.ValueObjects
{
    /// <summary>
    ///     The Money defines a money object.
    /// </summary>
    [Component]
    public class Money : IEquatable<Money>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Money" /> class.
        /// </summary>
        protected Money()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Money" /> class.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <param name="amount">The amount.</param>
        public Money(Currency currency, decimal amount)
        {
            Check.IsNotNull(currency, () => Currency);

            Currency = currency;
            Amount = amount;
        }

        /// <summary>
        ///     Gets the currency.
        /// </summary>
        public virtual Currency Currency { get; protected set; }

        /// <summary>
        ///     Gets the amount.
        /// </summary>
        public virtual decimal Amount { get; protected set; }

        #region Implementation of IEquatable<Money>

        bool IEquatable<Money>.Equals(Money other)
        {
            return Equals(other);
        }

        #endregion

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Money other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Amount == Amount && Equals(other.Currency, Currency);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (Money))
            {
                return false;
            }
            return Equals((Money) obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Amount.GetHashCode()*397) ^ (Currency != null ? Currency.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///     Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Money left, Money right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Money left, Money right)
        {
            return !Equals(left, right);
        }


        /// <summary>
        ///     Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency.CodedConcept.Code.Equals(right.Currency.CodedConcept.Code))
            {
                return new Money(left.Currency, left.Amount + right.Amount);
            }
            throw new NotImplementedException(
                string.Format(
                    "Currency conversion between {0} and {1} is not implemented.", left.Currency.CodedConcept.Name,
                    right.Currency.CodedConcept.Name));
        }

        /// <summary>
        ///     Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency.CodedConcept.Code.Equals(right.Currency.CodedConcept.Code))
            {
                return new Money(left.Currency, left.Amount - right.Amount);
            }
            throw new NotImplementedException(
                string.Format(
                    "Currency conversion between {0} and {1} is not implemented.", left.Currency.CodedConcept.Name,
                    right.Currency.CodedConcept.Name));
        }

        /// <summary>
        ///     Performs an implicit conversion from <see cref="Money" /> to
        ///     <see
        ///         cref="System.Decimal" />
        ///     .
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns>The decimal amount of the money.</returns>
        public static implicit operator decimal(Money money)
        {
            return money.Amount;
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0:c}", Amount);
        }
    }
}