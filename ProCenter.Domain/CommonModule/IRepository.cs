namespace ProCenter.Domain.CommonModule
{
    using System;

    public interface IRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        TAggregate GetByKey(Guid key);
        DateTime? GetLastModifiedDate(Guid key);
    }
}