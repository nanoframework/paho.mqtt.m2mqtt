/*
Copyright (c) 2013, 2014 Paolo Patierno

All rights reserved. This program and the accompanying materials
are made available under the terms of the Eclipse Public License v1.0
and Eclipse Distribution License v1.0 which accompany this distribution. 

The Eclipse Public License is available at 
   http://www.eclipse.org/legal/epl-v10.html
and the Eclipse Distribution License is available at 
   http://www.eclipse.org/org/documents/edl-v10.php.

Contributors:
   Paolo Patierno - initial API and implementation and/or initial documentation
   .NET Foundation and Contributors - nanoFramework support
*/

using nanoFramework.M2Mqtt.Exceptions;

namespace nanoFramework.M2Mqtt.Messages
{
    /// <summary>
    /// Class for DISCONNECT message from client to broker
    /// </summary>
    public class MqttMsgDisconnect : MqttMsgBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MqttMsgDisconnect()
        {
            Type = MqttMessageType.Disconnect;
        }

        /// <summary>
        /// Parse bytes for a DISCONNECT message
        /// </summary>
        /// <param name="fixedHeaderFirstByte">First fixed header byte</param>
        /// <param name="protocolVersion">MQTT Protocol Version</param>
        /// <param name="channel">Channel connected to the broker</param>
        /// <returns>DISCONNECT message instance</returns>
        public static MqttMsgDisconnect Parse(byte fixedHeaderFirstByte, MqttProtocolVersion protocolVersion, IMqttNetworkChannel channel)
        {
            MqttMsgDisconnect msg = new MqttMsgDisconnect();

            if (protocolVersion == MqttProtocolVersion.Version_3_1_1)
            {
                // [v3.1.1] check flag bits
                if ((fixedHeaderFirstByte & MSG_FLAG_BITS_MASK) != MQTT_MSG_DISCONNECT_FLAG_BITS)
                {
                    throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
                }
            }

            // get remaining length and allocate buffer
            int remainingLength = DecodeVariableByte(channel);
            // NOTE : remainingLength must be 0
            if(remainingLength !=0)
            {
                throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
            }

            return msg;
        }

        /// <summary>
        /// Returns the bytes that represents the current object.
        /// </summary>
        /// <param name="protocolVersion">MQTT protocol version</param>
        /// <returns>An array of bytes that represents the current object.</returns>
        public override byte[] GetBytes(MqttProtocolVersion protocolVersion)
        {
            byte[] buffer = new byte[2];
            int index = 0;

            // first fixed header byte
            if (protocolVersion == MqttProtocolVersion.Version_3_1_1)
            {
                buffer[index++] = ((byte)(MqttMessageType.Disconnect) << MSG_TYPE_OFFSET) | MQTT_MSG_DISCONNECT_FLAG_BITS; // [v.3.1.1]
            }
            else
            {
                buffer[index++] = ((byte)(MqttMessageType.Disconnect) << MSG_TYPE_OFFSET);
            }

            buffer[index] = 0x00;

            return buffer;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
#if DEBUG
            return this.GetTraceString(
                "DISCONNECT",
                null,
                null);
#else
            return base.ToString();
#endif
        }
    }
}
