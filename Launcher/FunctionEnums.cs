namespace Launcher
{
    public static class FunctionEnums
    {
        public enum Functions
        {
            /// <summary>
            /// 私聊消息事件
            /// </summary>
            PrivateMsg = 1,
            /// <summary>
            /// 群聊消息事件
            /// </summary>
            GroupMsg = 2,
            /// <summary>
            /// 文件上传事件
            /// </summary>
            Upload = 4,
            /// <summary>
            /// 管理员变更事件
            /// </summary>
            AdminChange = 5,
            /// <summary>
            /// 群成员减少事件
            /// </summary>
            GroupMemberDecrease = 6,
            /// <summary>
            /// 群成员增加事件
            /// </summary>
            GroupMemberIncrease = 7,
            /// <summary>
            /// 群禁言事件
            /// </summary>
            GroupBan = 8,
            /// <summary>
            /// 好友添加完成事件
            /// </summary>
            FriendAdded = 10,
            /// <summary>
            /// 好友添加请求事件
            /// </summary>
            FriendRequest = 11,
            /// <summary>
            /// 加群申请事件
            /// </summary>
            GroupAddRequest = 12,
            /// <summary>
            /// 酷Q启动事件
            /// </summary>
            StartUp = 1001,
            /// <summary>
            /// 酷Q退出事件
            /// </summary>
            Exit = 1002,
            /// <summary>
            /// 插件启用事件
            /// </summary>
            Enable = 1003,
            /// <summary>
            /// 插件禁用事件
            /// </summary>
            Disable = 1004
        }
        public enum PipeType
        {
            Server,
            Client,
            NoPipe
        }
    }
}
