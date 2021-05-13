namespace Deserizition
{
    public class GroupList
    {
        public int Count { get; set; }
        public string NextToken { get; set; }
        public Trooplist[] TroopList { get; set; }
    }

    public class Trooplist
    {
        public long GroupId { get; set; }
        public int GroupMemberCount { get; set; }
        public string GroupName { get; set; }
        public string GroupNotice { get; set; }
        public long GroupOwner { get; set; }
        public int GroupTotalCount { get; set; }
        public GroupMemberList GroupMemberList { get; set; }
    }
}
