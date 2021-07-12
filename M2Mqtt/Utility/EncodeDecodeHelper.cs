﻿using nanoFramework.M2Mqtt.Messages;
using System;
using System.Collections;
using System.Text;

namespace nanoFramework.M2Mqtt.Utility
{
    internal static class EncodeDecodeHelper
    {
        public static string GetUTF8FromBuffer(byte[] buffer, ref int index)
        {
            int length = buffer[index++] << 8;
            length += buffer[index++];
            byte[] utf8Buff = new byte[length];
            Array.Copy(buffer, index, utf8Buff, 0, length);
            index += length;
            return new string(Encoding.UTF8.GetChars(utf8Buff));
        }

        public static void EncodeUTF8FromBuffer(MqttProperty mqttProperty, byte[] toEncode, byte[] buffer, ref int index)
        {
            buffer[index++] = (byte)mqttProperty;
            buffer[index++] = (byte)((toEncode.Length & 0xFF00) >> 8);
            buffer[index++] = (byte)(toEncode.Length & 0xFF);
            Array.Copy(toEncode, 0, buffer, index, toEncode.Length);
            index += toEncode.Length;
        }

        public static byte[] EncodeArray(MqttProperty mqttProperty, byte[] buffer)
        {
            byte[] encodedBuffer = new byte[3 + buffer.Length];
            Array.Copy(buffer, 0, encodedBuffer, 3, buffer.Length);
            encodedBuffer[0] = (byte)mqttProperty;
            encodedBuffer[1] = (byte)((buffer.Length & 0xFF00) >> 8);
            encodedBuffer[2] = (byte)(buffer.Length & 0xFF);
            return encodedBuffer;
        }

        public static byte[] EncodeUserProperties(ArrayList userPropers)
        {
            ArrayList userProps = new ArrayList();
            int propSize = 0;
            foreach (string prop in userPropers)
            {
                byte[] propByteArray = Encoding.UTF8.GetBytes(prop);
                propSize += propByteArray.Length;
                userProps.Add(propByteArray);
            }

            int fullSize = userProps.Count * 3 + propSize;
            byte[] userProperties = new byte[fullSize];
            propSize = 0;
            foreach (byte[] prop in userProps)
            {
                userProperties[propSize++] = (byte)MqttProperty.UserProperty;
                userProperties[propSize++] = (byte)((prop.Length & 0xFF00) << 8);
                userProperties[propSize++] = (byte)(prop.Length & 0xFF);
                Array.Copy(prop, 0, userProperties, propSize, prop.Length);
                propSize += prop.Length;
            }

            return userProperties;
        }

        public static int EncodeUint(MqttProperty mqttProperty, uint data, byte[] buffer, int index)
        {
            buffer[index++] = (byte)mqttProperty;
            buffer[index++] = (byte)((data & 0xFF000000) >> 24);
            buffer[index++] = (byte)((data & 0x00FF0000) >> 16);
            buffer[index++] = (byte)((data & 0x0000FF00) >> 8);
            buffer[index++] = (byte)(data & 0x000000FF);
            return index;
        }

        public static int EncodeUshort(MqttProperty mqttProperty, ushort data, byte[] buffer, int index)
        {
            buffer[index++] = (byte)mqttProperty;
            buffer[index++] = (byte)((data & 0xFF00) >> 8);
            buffer[index++] = (byte)(data & 0x00FF);
            return index;
        }

        public static uint DecodeUint(byte[] buffer, ref int index)
        {
            uint decode = (uint)(buffer[index++] << 24);
            decode |= (uint)(buffer[index++] << 16);
            decode |= (uint)(buffer[index++] << 8);
            decode |= buffer[index++];
            return decode;
        }

        public static int GetPropertySize(byte[] buffer, ref int index)
        {
            // size of the properties
            int multiplier = 1;
            int propSize = 0;
            int digit;
            do
            {
                digit = buffer[index++];
                // next digit from stream
                propSize += ((digit & 127) * multiplier);
                multiplier *= 128;
            } while ((digit & 128) != 0);
            return propSize;
        }

        public static int EncodeLength(int length)
        {
            // The length
            if (length <= 0x7F)
            {
                return 1;
            }
            else if (length <= 0x7FFF)
            {
                return 2;
            }
            // This is theory, nano will not support
            else if (length <= 0x7FFFFF)
            {
                return 3;
            }
            // And science fiction
            else
            {
                return 4;
            }
        }

        public static int DecodeVariableByte(byte[] buffer, ref int index)
        {
            int multiplier = 1;
            int value = 0;
            int digit;
            do
            {
                // next digit from stream
                digit = buffer[index++];
                value += ((digit & 127) * multiplier);
                multiplier *= 128;
            } while ((digit & 128) != 0);
            return value;
        }
    }
}
