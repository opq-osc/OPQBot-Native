using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketIOClient.Messages
{
    /// <summary>
    /// Signals a connection to the endpoint. Once the server receives it, it's echoed back to the client
    /// </summary>
    /// <remarks>If the client is trying to connect to the endpoint /test, a message like this will be delivered:
    ///		'1::' [path] [query]
    /// </remarks>
    public class ConnectMessage : Message
    {
        public object ConnectMsg { get; private set; }

        public override string Event
        {
            get { return "connect"; }
        }

        public ConnectMessage() : base()
        {
            this.MessageType = SocketIOMessageTypes.Connect;
        }
        public ConnectMessage(string endPoint) : this()
        {
            this.Endpoint = endPoint;
        }
        public static ConnectMessage Deserialize(string rawMessage)
        {
            ConnectMessage msg = new ConnectMessage();
            msg.RawMessage = rawMessage;
            msg.ConnectMsg = JsonConvert.DeserializeObject<object>(rawMessage);
            return msg;
        }
        public override string Encoded
        {
            get
            {
                return string.Format("1::{0}{1}", this.Endpoint, string.Empty, string.Empty);
            }
        }
    }
}
