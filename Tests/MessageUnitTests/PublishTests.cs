using System;
using System.Diagnostics;
using System.Text;
using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Exceptions;
using nanoFramework.M2Mqtt.Messages;
using nanoFramework.TestFramework;

namespace MessageUnitTests
{
    [TestClass]
    public class PublishTests
    {
        private const string Topic = "thistopic/something";
        private const string TopicWildcard = "thistopic/something/#";
        private const string MessageString = "This is a string message";
        private readonly byte[] MessageByte = new byte[] { 1, 2, 3, 4, 5, 6 };
        private const ushort MessageId = 42;

        [TestMethod]
        public void PublishBasicEncodeTestv311()
        {
            // Arrange
            MqttMsgPublish publish = new(Topic, Encoding.UTF8.GetBytes(MessageString), true, MqttQoSLevel.ExactlyOnce, false);
            publish.MessageId = MessageId;
            byte[] encodedCorrect = new byte[] {60,47,0,19,116,104,105,115,116,111,112,105,99,47,115,111,109,101,116,104,105,110,
                103,0,42,84,104,105,115,32,105,115,32,97,32,115,116,114,105,110,103,32,109,101,115,
                115,97,103,101};
            // Act
            byte[] encoded = publish.GetBytes(MqttProtocolVersion.Version_3_1_1);
            // Assert
            Assert.Equal(encodedCorrect, encoded);
        }

        [TestMethod]
        public void PublishBasicDecodeTestv311()
        {
            // Arrange
            byte[] encodedCorrect = new byte[] { 47,0,19,116,104,105,115,116,111,112,105,99,47,115,111,109,101,116,104,105,110,
                103,0,42,84,104,105,115,32,105,115,32,97,32,115,116,114,105,110,103,32,109,101,115,
                115,97,103,101};
            MokChannel mokChannel = new(encodedCorrect);
            // Act
            MqttMsgPublish publish = MqttMsgPublish.Parse(60, MqttProtocolVersion.Version_3_1_1, mokChannel);
            // Assert
            Assert.Equal(Topic, publish.Topic);
            Assert.Equal(MessageId, publish.MessageId);
            Assert.Equal(MessageString, Encoding.UTF8.GetString(publish.Message, 0, publish.Message.Length));
            Assert.Equal((byte)MqttQoSLevel.ExactlyOnce, (byte)publish.QosLevel);
            Assert.Equal(true, publish.DupFlag);
            Assert.Equal(false, publish.Retain);
        }

        [TestMethod]
        public void PublishBasicEncodeExceptionTestv311()
        {
            Assert.Throws(typeof(MqttClientException), () =>
            {
                // Arrange
                MqttMsgPublish publish = new(TopicWildcard, Encoding.UTF8.GetBytes(MessageString), true, MqttQoSLevel.ExactlyOnce, false);
                publish.MessageId = MessageId;
                // Act
                byte[] encoded = publish.GetBytes(MqttProtocolVersion.Version_3_1_1);
                // Assert
            });
        }
    }
}
