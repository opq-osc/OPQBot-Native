using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;


namespace Core.Code
{
    public class Event_GroupMessage: IGroupMessage
    {
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            if (e.FromGroup.Id != 891787846) return;
            e.FromGroup.SendGroupMessage(e.Message);
        }
    }
}
