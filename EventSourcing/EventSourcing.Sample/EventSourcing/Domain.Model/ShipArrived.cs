namespace EventSourcing.Sample.EventSourcing.Domain.Model
{
    public class ShipArrived : ShippingEvent
    {
        public Ship Ship { get; set; }
        public string Port { get; set; }
    }
}
