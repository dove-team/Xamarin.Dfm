namespace Xamarin.Dfm.Model
{
    public sealed class DanmuMsgModel
    {
        public string Text { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Ul { get; set; }
        /// <summary>
        /// 等级颜色
        /// </summary>
        public string UlColor { get; set; }
        /// <summary>
        /// 头衔id
        /// </summary>
        public string UserTitle { get; set; }
        public string Vip { get; set; }
        /// <summary>
        /// 勋章
        /// </summary>
        public string MedalName { get; set; }
        /// <summary>
        /// 勋章
        /// </summary>
        public string MedalLv { get; set; }
        /// <summary>
        /// 勋章颜色
        /// </summary>
        public string MedalColor { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsVip { get; set; }
        public bool IsBigVip { get; set; }
        public bool HasMedal { get; set; }
        public bool HasTitle { get; set; }
        public bool HasUL { get; set; }
        /// <summary>
        /// 显示的水平位置
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 显示的垂直位置
        /// </summary>
        public int Y { get; internal set; }
        public float W { get; set; }
    }
}