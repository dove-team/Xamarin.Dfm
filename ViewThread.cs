using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Android.Graphics;
using Java.Lang;
using Xamarin.Dfm.Extensions;
using Xamarin.Dfm.Model;
using Exception = System.Exception;

namespace Xamarin.Dfm
{
    public sealed class ViewThread : Thread
    {
        private Timer Timer { get; }
        private double Time { get; set; } = 0;
        public bool Runable { get; internal set; } = true;
        private DanmakuSurfaceView SurfaceView { get; }
        public ViewThread(DanmakuSurfaceView surfaceView)
        {
            SurfaceView = surfaceView;
            Timer = new Timer(10);
            Timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time += 0.01;
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            try
            {
                Timer.Stop();
                Timer.Dispose();
            }
            catch { }
        }
        public override void Run()
        {
            base.Run();
            Timer.Start();
            while (Runable)
            {
                try
                {
                    switch (SurfaceView.DanmakuType)
                    {
                        case DanmakuType.Video:
                            {
                                if (SurfaceView.DanmakuModels != null && SurfaceView.DanmakuModels.Count > 0)
                                {
                                    var canvas = SurfaceView.Holder.LockCanvas();
                                    if (canvas == null)
                                        break;
                                    canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
                                    var models = SurfaceView.DanmakuModels.Where(x => x.time <= Time && x.enable).ToList();
                                    if (models != null)
                                    {
                                        models.ForEach(model =>
                                        {
                                            try
                                            {
                                                switch (model.location)
                                                {
                                                    case DanmakuLocation.Top:
                                                        {
                                                            Paint paint = InitPaint(model);
                                                            if (model.startpoint == 0)
                                                                model.startpoint = Time;
                                                            if (model.y == 0)
                                                            {
                                                                int height = paint.GetDisplayHeight();
                                                                var count = Convert.ToInt32(SurfaceView.Rect / height);
                                                                model.y = SurfaceView.Random.Next(1, count) * height;
                                                            }
                                                            if (model.x == 0)
                                                            {
                                                                var width = paint.MeasureText(model.text);
                                                                model.x = Convert.ToInt32((SurfaceView.Width - width) / 2);
                                                            }
                                                            canvas.DrawText(model.text, model.x, model.y, paint);
                                                            if (model.displayTime - 5 >= model.startpoint)
                                                                model.enable = false;
                                                            else
                                                                model.displayTime = Time;
                                                            break;
                                                        }
                                                    case DanmakuLocation.Bottom:
                                                        {
                                                            Paint paint = InitPaint(model);
                                                            if (model.startpoint == 0)
                                                                model.startpoint = Time;
                                                            if (model.y == 0)
                                                            {
                                                                int height = paint.GetDisplayHeight();
                                                                var count = Convert.ToInt32(SurfaceView.Rect / height);
                                                                model.y = (SurfaceView.Random.Next(1, count) * height) + SurfaceView.Rect;
                                                            }
                                                            if (model.x == 0)
                                                            {
                                                                var width = paint.MeasureText(model.text);
                                                                model.x = Convert.ToInt32((SurfaceView.Width - width) / 2);
                                                            }
                                                            canvas.DrawText(model.text, model.x, model.y, paint);
                                                            if (model.displayTime - 5 >= model.startpoint)
                                                                model.enable = false;
                                                            else
                                                                model.displayTime = Time;
                                                            break;
                                                        }
                                                    case DanmakuLocation.Position:
                                                        {
                                                            //暂时不想弄,🧠＝🍜
                                                            break;
                                                        }
                                                    default:
                                                        {
                                                            Paint paint = InitPaint(model);
                                                            if (model.y == 0)
                                                            {
                                                                int height = paint.GetDisplayHeight();
                                                                model.y = height + SurfaceView.Random.Next(SurfaceView.Height - height);
                                                            }
                                                            if (model.x == 0)
                                                            {
                                                                var width = paint.MeasureText(model.text);
                                                                model.w = -width;
                                                                model.x = (SurfaceView.Width + width).ToInt32();
                                                            }
                                                            canvas.DrawText(model.text, model.x -= model.Speed, model.y, paint);
                                                            if (model.x < model.w && model.y != 0)
                                                                model.enable = false;
                                                            break;
                                                        }
                                                }
                                            }
                                            catch { }
                                        });
                                    }
                                    SurfaceView.Holder.UnlockCanvasAndPost(canvas);
                                    Sleep(16);
                                }
                                break;
                            }
                        case DanmakuType.Live:
                            {
                                if (SurfaceView.DanmuMsgModels != null && SurfaceView.DanmuMsgModels.Count > 0)
                                {
                                    var canvas = SurfaceView.Holder.LockCanvas();
                                    if (canvas == null)
                                        break;
                                    canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
                                    for (var index = 0; index < SurfaceView.DanmuMsgModels.Count; index++)
                                    {
                                        try
                                        {
                                            var model = SurfaceView.DanmuMsgModels[index];
                                            if (model.y == 0)
                                            {
                                                int height = SurfaceView.Paint.GetDisplayHeight();
                                                model.y = height + SurfaceView.Random.Next(SurfaceView.Height - height);
                                            }
                                            if (model.x == 0)
                                            {
                                                var width = SurfaceView.Paint.MeasureText(model.text);
                                                model.w = -width;
                                                model.x = Convert.ToInt32((SurfaceView.Width + width) * 1.2);
                                            }
                                            canvas.DrawText(model.text, model.x -= SurfaceView.Speed, model.y, SurfaceView.Paint);
                                            if (model.x < model.w && model.y != 0)
                                                SurfaceView.DanmuMsgModels.Remove(model);
                                        }
                                        catch { }
                                    }
                                    SurfaceView.Holder.UnlockCanvasAndPost(canvas);
                                    Sleep(16);
                                }
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("ViewThread" + ex.Message);
                }
            }
        }
        private Paint InitPaint(DanmakuModel model)
        {
            var p = new Paint(SurfaceView.Paint)
            {
                TextSize = Convert.ToSingle(model.size),
                Color = model.color
            };
            return p;
        }
    }
}