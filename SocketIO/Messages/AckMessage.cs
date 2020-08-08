using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SocketIOClient.Messages
{
	public sealed class AckMessage : Message
	{
		private static Regex reAckId = new Regex(@"^(\d{1,})");
 		private static Regex reAckPayload = new Regex(@"(?:[\d\+]*)(?<data>.*)$");
		private static Regex reAckComplex = new Regex(@"^\[(?<payload>.*)\]$");

		private static object ackLock = new object();
		private static int _akid = 0;
		public static int NextAckID
		{
			get
			{
				lock (ackLock)
				{
					_akid++;
					if (_akid < 0)
						_akid = 0;
					return _akid;
				}
			}
		}

		public Action<dynamic> Callback;

		public AckMessage()
			: base()
        {
            this.MessageType = SocketIOMessageTypes.ACK;
        }
		
		public static AckMessage Deserialize(string rawMessage)
        {
			AckMessage msg = new AckMessage();
	
			msg.RawMessage = rawMessage;

            string askId = rawMessage.Substring(2, rawMessage.IndexOf("[") - 2);
            int id;
            if (int.TryParse(askId, out id))
                msg.AckId = id;
            var groups = new Regex(@"\[([\s\S]*)\]", RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(rawMessage).Groups;
            msg.RawMessage = groups[0].Value.Replace("\\", "");
            //jsonMsg.Event = groups[1].Value;
            msg.MessageText = groups[1].Value.Replace("\\", "").Trim('"');
            return msg;
        }
		public override string Encoded
		{
			get
			{
				int msgId = (int)this.MessageType;
				if (this.AckId.HasValue)
				{
					if (this.Callback == null)
						return string.Format("{0}:{1}:{2}:{3}", msgId, this.AckId ?? -1, this.Endpoint, this.MessageText);
					else
						return string.Format("{0}:{1}+:{2}:{3}", msgId, this.AckId ?? -1, this.Endpoint, this.MessageText);
				}
				else
					return string.Format("{0}::{1}:{2}", msgId, this.Endpoint, this.MessageText);
			}
		}
	}
}
