namespace Deserizition
{
    public class GroupMemberList
    {
        public int Count { get; set; }
        public long GroupUin { get; set; }
        public long LastUin { get; set; }
        public Memberlist[] MemberList { get; set; }

        public class Memberlist
        {
            public int Age { get; set; }
            public string AutoRemark { get; set; }
            public int CreditLevel { get; set; }
            public string Email { get; set; }
            public int FaceId { get; set; }
            public int Gender { get; set; }
            public int GroupAdmin { get; set; }
            public string GroupCard { get; set; }
            public int JoinTime { get; set; }
            public int LastSpeakTime { get; set; }
            public int MemberLevel { get; set; }
            public long MemberUin { get; set; }
            public string Memo { get; set; }
            public string NickName { get; set; }
            public string ShowName { get; set; }
            public string SpecialTitle { get; set; }
            public int Status { get; set; }
        }
    }
}
