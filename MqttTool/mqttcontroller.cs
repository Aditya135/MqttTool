using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttTool
{
    class Mqttcontroller
    {
        MqttClient client;
        ushort msgId;
        public Mqttcontroller(String broker,String topic)
        {
            client = new MqttClient(broker);
            byte code = client.Connect(Guid.NewGuid().ToString(), null, null,
                                        false, // will retain flag
                                        MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // will QoS
                                        true, // will flag
                                        "/LWTcsharp", // will topic
                                        "I died!.. Sedlife", // will message
                                        true,
                                        60);
            msgId = client.Subscribe(new string[] { topic}, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.MqttMsgPublishReceived += onMessageReceive;
        }
        public ushort subId()
        {
            return msgId;
        }
        public void onMessageReceive(object sender, MqttMsgPublishEventArgs e)
        {

        }
        public bool isConnected()
        {
            return client.IsConnected;
        }
    }
}
