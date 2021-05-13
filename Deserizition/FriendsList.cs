namespace Deserizition
{
    public class FriendsList
    {
        public int Friend_count { get; set; }
        public Friendlist[] Friendlist { get; set; }
        public int GetfriendCount { get; set; }
        public int StartIndex { get; set; }
        public int Totoal_friend_count { get; set; }
    }

    public class Friendlist
    {
        public long FriendUin { get; set; }
        public bool IsRemark { get; set; }
        public string NickName { get; set; }
        public string OnlineStr { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }
}