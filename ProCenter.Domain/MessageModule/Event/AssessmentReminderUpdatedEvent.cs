namespace ProCenter.Domain.MessageModule.Event
{
    #region

    using System;
    using System.Linq.Expressions;
    using CommonModule;
    using Pillar.Common.Utility;

    #endregion

    public class AssessmentReminderUpdatedEvent : CommitEventBase
    {
        public AssessmentReminderUpdatedEvent()
            : base ( Guid.Empty, -1 )
        {
        }

        public AssessmentReminderUpdatedEvent(Guid key, int version, Expression<Func<AssessmentReminder, object>> propertyExpression, object value)
            : base(key, version)
        {
            Property = PropertyUtil.ExtractPropertyName(propertyExpression);
            Value = value;
        }

        public string Property { get; set; }
        public object Value { get; set; }
    }
}