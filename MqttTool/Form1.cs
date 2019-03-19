using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace MqttTool
{
    public partial class Form1 : Form
    {
        static String broker;
        static String topic;
        MqttClient client;
        ushort msgId;
        static List<String> lines_txt;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                lines_txt.Add("Disconnected!");
                SetText();
                client.Disconnect();
            }
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lines_txt = new List<string>();
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            topic = ((TextBox)sender).Text;
        }

        public void button2_Click(object sender, EventArgs e)                  //Subscribe
        {
            client = new MqttClient(broker);
            byte code = client.Connect(Guid.NewGuid().ToString(), null, null,
                                        false, // will retain flag
                                        MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // will QoS
                                        true, // will flag
                                        "/lwtCsharp", // will topic
                                        "i died! sadlife...", // will message
                                        true,
                                        60);
            if (code == 0)
            {
                lines_txt.Add("Connection Successful!");
                SetText();

            }
            msgId = client.Subscribe(new string[] { topic },
                                            new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        }
        public void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            lines_txt.Add("Received: " + Encoding.UTF8.GetString(e.Message));
            SetText();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            broker = ((TextBox)sender).Text;
        }

        delegate void SetTextCallback();

        private void SetText()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] {});
            }
            else
            {
                this.textBox3.Lines = lines_txt.ToArray();
            }
        }


    }
}
