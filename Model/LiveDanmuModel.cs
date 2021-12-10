namespace Xamarin.Dfm.Model
{
    public sealed class LiveDanmuModel
    {
        public int Viewer { get; set; }
        public LiveDanmuTypes Type { get; set; }
        public DanmuMsgModel Value { get; set; }
    }
}