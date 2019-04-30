namespace EventSourcing.Sample.Tradition
{
    /// <summary>
    /// 跟踪服务
    /// </summary>
    public class TrackingService
    {
        /// <summary>
        /// 记录船到达港口
        /// </summary>
        /// <param name="ship">船</param>
        /// <param name="port">港口</param>
        public void RecordArrival(Ship ship, string port)
        {
            ship.Location = port;
        }

        /// <summary>
        /// 记录船离开港口
        /// </summary>
        /// <param name="ship">船</param>
        /// <param name="port">港口</param>
        public void RecordDeparture(Ship ship, string port)
        {
            ship.Location = port;
        }
    }
}
