namespace Xamarin.Dfm.Model
{
    public sealed class DanmuMsgModel
    {
        public string text { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string ul { get; set; }
        /// <summary>
        /// 等级颜色
        /// </summary>
        public string ulColor { get; set; }
        /// <summary>
        /// 头衔id
        /// </summary>
        public string user_title { get; set; }
        public string vip { get; set; }
        /// <summary>
        /// 勋章
        /// </summary>
        public string medal_name { get; set; }
        /// <summary>
        /// 勋章
        /// </summary>
        public string medal_lv { get; set; }
        /// <summary>
        /// 勋章颜色
        /// </summary>
        public string medalColor { get; set; }
        public bool isAdmin { get; set; }
        public bool isVip { get; set; }
        public bool isBigVip { get; set; }
        public bool hasMedal { get; set; }
        public bool hasTitle { get; set; }
        public bool hasUL { get; set; }
        /// <summary>
        /// 显示的水平位置
        /// </summary>
        public int x { get; set; }
        /// <summary>
        /// 显示的垂直位置
        /// </summary>
        public int y { get; internal set; }
        public float w { get; set; }
    }
}