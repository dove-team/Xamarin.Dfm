using Android.Graphics;
using System;
using System.IO;
using System.Threading.Tasks;
using JavaMath = Java.Lang.Math;

namespace Xamarin.Dfm.Extensions
{
    public static class Ext
    {
        public static int GetDisplayHeight(this Paint paint)
        {
            var fm = paint.GetFontMetrics();
            var height = (int)JavaMath.Ceil(fm.Descent - fm.Ascent);
            return height;
        }
        public static int ToInt32(this object token)
        {
            try
            {
                if (int.TryParse(token.ToString(), out int result))
                    return result;
                else
                    return Convert.ToInt32(token.ToString());
            }
            catch { return -1; }
        }
        public static async Task ReadBAsync(this Stream stream, byte[] buffer, int offset, int count)
        {
            try
            {
                if (offset + count > buffer.Length) throw new ArgumentException();
                int read = 0;
                while (read < count)
                {
                    var available = await stream.ReadAsync(buffer, offset, count - read);
                    if (available == 0) throw new ObjectDisposedException(null);
                    read += available;
                    offset += available;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("ReadBAsync", ex);
            }
        }
    }
}