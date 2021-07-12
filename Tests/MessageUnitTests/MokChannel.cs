using nanoFramework.M2Mqtt;
using System;
using System.Diagnostics;

namespace MessageUnitTests
{
    internal class MokChannel : IMqttNetworkChannel
    {
        private byte[] _mokData;
        private int _index = 0;

        public MokChannel(byte[] mokData)
        {
            _mokData = mokData;
        }

        public bool DataAvailable => _index < _mokData.Length;

        public void Accept()
        { }

        public void Close()
        { }

        public void Connect()
        { }

        public int Receive(byte[] buffer)
        {
            Array.Copy(_mokData, _index, buffer, 0, buffer.Length);
            _index += buffer.Length;
            return buffer.Length;
        }

        public int Receive(byte[] buffer, int timeout)
        {
            Array.Copy(_mokData, _index, buffer, 0, buffer.Length);
            _index += buffer.Length;
            return buffer.Length;
        }

        public int Send(byte[] buffer)
        {
            Array.Copy(buffer, 0, _mokData, _index, buffer.Length);
            _index += buffer.Length;
            return buffer.Length;
        }
    }
}
