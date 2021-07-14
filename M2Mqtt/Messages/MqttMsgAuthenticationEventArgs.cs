

using System;

namespace nanoFramework.M2Mqtt.Messages
{
    /// <summary>
    /// Event Args class for CONNECT message received from client
    /// </summary>
    public class MqttMsgAuthenticationEventArgs : EventArgs
    {
        /// <summary>
        /// Message received from client
        /// </summary>
        public MqttMsgAuthentication Message { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authentication">CONNECT message received from client</param>
        public MqttMsgAuthenticationEventArgs(MqttMsgAuthentication authentication)
        {
            Message = authentication;
        }
    }
}
