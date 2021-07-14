﻿using System;
using System.Diagnostics;
using System.Text;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Exceptions;
using nanoFramework.M2Mqtt.Messages;
using nanoFramework.TestFramework;

namespace MessageUnitTests
{
    [TestClass]
    public class UnsubackTests
    {
        [TestMethod]
        public void UnsubackBasicEncodeTestv311()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 176, 2, 0, 42 };
            MqttMsgUnsuback unsuback = new();
            unsuback.MessageId = 42;
            // Act
            byte[] encoded = unsuback.GetBytes(MqttProtocolVersion.Version_3_1_1);
            Helpers.DumpBuffer(encoded);
            // Assert
            Assert.Equal(encodedCorrect, encoded);
        }

        [TestMethod]
        public void UnsubackBasicEncodeTestv5()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 176, 4, 0, 42, 0, 0 };
            MqttMsgUnsuback unsuback = new();
            unsuback.MessageId = 42;
            // Act
            byte[] encoded = unsuback.GetBytes(MqttProtocolVersion.Version_5);
            // Assert
            Assert.Equal(encodedCorrect, encoded);
        }

        [TestMethod]
        public void UnsubackAdvancedEncodeTestv5()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 176,51,0,42,151,47,31,0,26,89,111,117,32,104,97,118,101,32,101,120,99,101,101,100,
                32,121,111,117,114,32,113,117,111,116,97,38,0,6,80,114,111,112,32,49,38,0,6,80,114,
                111,112,32,50 };
            MqttMsgUnsuback unsuback = new();
            unsuback.MessageId = 42;
            unsuback.ReasonCode = MqttReasonCode.QuotaExceeded;
            unsuback.Reason = "You have exceed your quota";
            unsuback.UserProperties.Add("Prop 1");
            unsuback.UserProperties.Add("Prop 2");
            // Act
            byte[] encoded = unsuback.GetBytes(MqttProtocolVersion.Version_5);
            // Assert
            Assert.Equal(encodedCorrect, encoded);
        }

        [TestMethod]
        public void UnsubackAdvancedEncodeMaximumPacketSizeTestv5()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 176, 4, 0, 42, 0x97, 0 };
            MqttMsgUnsuback unsuback = new();
            unsuback.MessageId = 42;
            unsuback.MaximumPacketSize = 5;
            // This should not be send at all as exceeding the maximum packet size
            unsuback.ReasonCode = MqttReasonCode.QuotaExceeded;
            unsuback.Reason = "You have exceed your quota";
            unsuback.UserProperties.Add("Prop 1");
            unsuback.UserProperties.Add("Prop 2");
            // Act
            byte[] encoded = unsuback.GetBytes(MqttProtocolVersion.Version_5);
            // Assert
            Assert.Equal(encodedCorrect, encoded);
        }

        [TestMethod]
        public void UnsubackBasicDecodeTestv311()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 2, 0, 42 };
            MokChannel mokChannel = new(encodedCorrect);
            // Act
            MqttMsgUnsuback unsuback = MqttMsgUnsuback.Parse(176, MqttProtocolVersion.Version_3_1_1, mokChannel);
            // Assert
            Assert.Equal((ushort)42, unsuback.MessageId);
        }

        [TestMethod]
        public void UnsubackBasicDecodeTestv5()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 4, 0, 42, 0x97, 0 };
            MokChannel mokChannel = new(encodedCorrect);
            // Act
            MqttMsgUnsuback unsuback = MqttMsgUnsuback.Parse(176, MqttProtocolVersion.Version_5, mokChannel);
            // Assert
            Assert.Equal((byte)MqttMessageType.UnsubscribeAck, (byte)unsuback.Type);
            Assert.Equal((ushort)42, unsuback.MessageId);
            Assert.Equal((byte)0x97, (byte)unsuback.ReasonCode);
        }

        [TestMethod]
        public void UnsubackAdvanceDecodeTestv5()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 51,0,42,151,47,31,0,26,89,111,117,32,104,97,118,101,32,101,120,99,101,101,100,
                32,121,111,117,114,32,113,117,111,116,97,38,0,6,80,114,111,112,32,49,38,0,6,80,114,
                111,112,32,50 };
            MokChannel mokChannel = new(encodedCorrect);
            // Act
            MqttMsgUnsuback unsuback = MqttMsgUnsuback.Parse(176, MqttProtocolVersion.Version_5, mokChannel);
            // Assert
            Assert.Equal((ushort)42, unsuback.MessageId);
            Assert.Equal((byte)unsuback.ReasonCode, (byte)MqttReasonCode.QuotaExceeded);
            Assert.Equal(unsuback.Reason, "You have exceed your quota");
            Assert.Equal(unsuback.UserProperties.Count, 2);
            Assert.Equal((string)unsuback.UserProperties[0], "Prop 1");
            Assert.Equal((string)unsuback.UserProperties[1], "Prop 2");
        }

    }
}