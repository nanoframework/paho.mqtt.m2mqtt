
using System;

namespace nanoFramework.M2Mqtt.Messages
{
    /// <summary>
    /// Event Args class for CONNECT message received from client
    /// </summary>
    public class ConnectionOpenedEventArgs : EventArgs
    {
        /// <summary>
        /// Message received from client
        /// </summary>
        public MqttMsgConnack Message { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authentication">CONNECT message received from client</param>
        public ConnectionOpenedEventArgs(MqttMsgConnack connact)
        {
            Message = connact;
        }
    }
}