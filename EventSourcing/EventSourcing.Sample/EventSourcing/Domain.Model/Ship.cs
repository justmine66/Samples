using System.Collections.Generic;

namespace EventSourcing.Sample.EventSourcing.Domain.Model
{
    /// <summary>
    /// 船
    /// </summary>
    public class Ship : EventSourcedRootEntity
    {
        public Ship(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        public Ship(string name, string location)
        {
            Name = name;
            Location = location;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; private set; }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return Name;
        }
    }
}
