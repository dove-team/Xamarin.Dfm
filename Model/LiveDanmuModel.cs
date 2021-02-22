namespace Xamarin.Dfm.Model
{
    public sealed class LiveDanmuModel
    {
        public LiveDanmuTypes type { get; set; }
        public int viewer { get; set; }
        public DanmuMsgModel value { get; set; }
    }
}