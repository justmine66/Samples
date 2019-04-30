using System;

namespace EventSourcing.Sample.EventSourcing
{
    public interface IDomainEvent
    {
        int EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
    }
}
