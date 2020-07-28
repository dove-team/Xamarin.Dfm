using Android.App;
using Android.Util;

namespace Xamarin.Dfm.Extensions
{
    public sealed class StaticValue
    {
        public static int WindowWidth { get; private set; }
        public static int WindowHeight { get; private set; }
        public static void InitDisplayInfo(Activity activity)
        {
            DisplayMetrics dm = new DisplayMetrics();
            activity.WindowManager.DefaultDisplay.GetMetrics(dm);
            WindowWidth = dm.WidthPixels;
            WindowHeight = dm.HeightPixels;
        }
    }
}