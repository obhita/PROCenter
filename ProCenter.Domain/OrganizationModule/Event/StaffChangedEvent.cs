namespace ProCenter.Domain.OrganizationModule.Event
{
    using System;
    using System.Linq.Expressions;
    using CommonModule;
    using Pillar.Common.Utility;

    public class StaffChangedEvent : CommitEventBase
    {
        public StaffChangedEvent()
            : base(Guid.Empty, -1)
        {
        }


        public StaffChangedEvent(Guid staffKey, int version, Expression<Func<Staff, object>> propertyExpression, object value) : base(staffKey, version)
        {
            Property = PropertyUtil.ExtractPropertyName(propertyExpression);
            Value = value;
        }

        #region Public Properties

        /// <summary>
        ///     Gets the property.
        /// </summary>
        /// <value>
        ///     The property.
        /// </value>
        public string Property { get; private set; }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        public object Value { get; private set; }

        #endregion
    }
}