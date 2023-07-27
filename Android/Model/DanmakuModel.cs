using Android.Graphics;

namespace Xamarin.Dfm.Model
{
    public sealed class DanmakuModel
    {
        public string Text { get; set; }
        /// <summary>
        /// 弹幕大小
        /// </summary>
        public double Size { get; set; }
        /// <summary>
        /// 弹幕颜色
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// 弹幕出现时间
        /// </summary>
        public double Time { get; set; }
        /// <summary>
        /// 弹幕发送时间
        /// </summary>
        public string SendTime { get; set; }
        /// <summary>
        /// 弹幕池
        /// </summary>
        public string Pool { get; set; }
        /// <summary>
        /// 弹幕发送人ID
        /// </summary>
        public string SendID { get; set; }
        /// <summary>
        /// 弹幕ID
        /// </summary>
        public string RowID { get; set; }
        /// <summary>
        /// 弹幕出现位置
        /// </summary>
        public DanmakuLocation Location { get; set; }
        public DanmakuSite FromSite { get; set; }
        public string Source { get; set; }
        public int Speed { get; set; } = 8;
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public bool Enable { get; set; } = true;
        public double DisplayTime { get; internal set; }
        public double StartPoint { get; internal set; }
        public float W { get; set; }
    }
}