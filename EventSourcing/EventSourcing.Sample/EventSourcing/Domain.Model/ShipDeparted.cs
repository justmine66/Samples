namespace EventSourcing.Sample.EventSourcing.Domain.Model
{
    public class ShipDeparted : ShippingEvent
    {
        public Ship Ship { get; set; }
        public string Port { get; set; }
    }
}
