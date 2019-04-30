using System;

namespace EventSourcing.Sample.EventSourcing.Domain.Model
{
    public abstract class ShippingEvent: DomainEvent
    {
        public DateTime RecordedOn { get; set; }
    }
}
