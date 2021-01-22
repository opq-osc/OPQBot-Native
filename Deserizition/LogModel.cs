using SqlSugar;

namespace Deserizition
{
    [SugarTable("log")]
    public class LogModel
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long time { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public int priority { get; set; }
        /// <summary>
        /// 日志来源
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// 日志处理状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string detail { get; set; }
    }
}
