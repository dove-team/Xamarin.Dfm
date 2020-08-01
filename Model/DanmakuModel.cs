using Android.Graphics;

namespace Xamarin.Dfm.Model
{
    public sealed class DanmakuModel
    {
        public string text { get; set; }
        /// <summary>
        /// 弹幕大小
        /// </summary>
        public double size { get; set; }
        /// <summary>
        /// 弹幕颜色
        /// </summary>
        public Color color { get; set; }
        /// <summary>
        /// 弹幕出现时间
        /// </summary>
        public double time { get; set; }
        /// <summary>
        /// 弹幕发送时间
        /// </summary>
        public string sendTime { get; set; }
        /// <summary>
        /// 弹幕池
        /// </summary>
        public string pool { get; set; }
        /// <summary>
        /// 弹幕发送人ID
        /// </summary>
        public string sendID { get; set; }
        /// <summary>
        /// 弹幕ID
        /// </summary>
        public string rowID { get; set; }
        /// <summary>
        /// 弹幕出现位置
        /// </summary>
        public DanmakuLocation location { get; set; }
        public DanmakuSite fromSite { get; set; }
        public string source { get; set; }
        public int Speed { get; set; } = 8;
        public int time_s { get; set; }
        public int x { get; set; } = 0;
        public int y { get; set; } = 0;
        public bool enable { get; set; } = true;
        public double displayTime { get; internal set; }
        public double startpoint { get; internal set; }
    }
}