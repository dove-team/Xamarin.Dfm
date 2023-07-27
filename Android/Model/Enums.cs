namespace Xamarin.Dfm.Model
{
    public enum DanmakuType
    {
        /// <summary>
        /// 直播弹幕
        /// </summary>
        Live,
        /// <summary>
        /// 视频弹幕
        /// </summary>
        Video
    }
    public enum LiveDanmuTypes
    {
        /// <summary>
        /// 观众
        /// </summary>
        Viewer,
        /// <summary>
        /// 弹幕
        /// </summary>
        Danmu,
        /// <summary>
        /// 礼物
        /// </summary>
        Gift,
        /// <summary>
        /// 欢迎
        /// </summary>
        Welcome,
        /// <summary>
        /// 系统信息
        /// </summary>
        SystemMsg
    }
    public enum DanmakuLocation
    {
        /// <summary>
        /// 滚动弹幕Model1-3
        /// </summary>
        Roll = -1,
        /// <summary>
        /// 顶部弹幕Model5
        /// </summary>
        Top = 0,
        /// <summary>
        /// 底部弹幕Model4
        /// </summary>
        Bottom = 1,
        /// <summary>
        /// 定位弹幕Model7
        /// </summary>
        Position = 2,
        /// <summary>
        /// 其它暂未支持的类型
        /// </summary>
        Other = 3
    }
    public enum DanmakuSite
    {
        Bilibili,
        Acfun,
        Tantan
    }
    public enum DanmakuState
    {
        Running,
        Pause
    }
}