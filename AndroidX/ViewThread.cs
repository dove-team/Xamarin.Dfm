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
                                    var models = SurfaceView.DanmakuModels.Where(x => x.Time <= Time && x.Enable);
                                    if (models != null && models.Any())
                                    {
                                        foreach (var model in models)
                                        {
                                            try
                                            {
                                                switch (model.Location)
                                                {
                                                    case DanmakuLocation.Top:
                                                        {
                                                            Paint paint = InitPaint(model);
                                                            if (model.StartPoint == 0)
                                                                model.StartPoint = Time;
                                                            if (model.Y == 0)
                                                            {
                                                                int height = paint.GetDisplayHeight();
                                                                var count = Convert.ToInt32(SurfaceView.Rect / height);
                                                                model.Y = SurfaceView.Random.Next(1, count) * height;
                                                            }
                                                            if (model.X == 0)
                                                            {
                                                                var width = paint.MeasureText(model.Text);
                                                                model.X = Convert.ToInt32((SurfaceView.Width - width) / 2);
                                                            }
                                                            canvas.DrawText(model.Text, model.X, model.Y, paint);
                                                            if (model.DisplayTime - 5 >= model.StartPoint)
                                                                model.Enable = false;
                                                            else
                                                                model.DisplayTime = Time;
                                                            break;
                                                        }
                                                    case DanmakuLocation.Bottom:
                                                        {
                                                            Paint paint = InitPaint(model);
                                                            if (model.StartPoint == 0)
                                                                model.StartPoint = Time;
                                                            if (model.Y == 0)
                                                            {
                                                                int height = paint.GetDisplayHeight();
                                                                var count = Convert.ToInt32(SurfaceView.Rect / height);
                                                                model.Y = (SurfaceView.Random.Next(1, count) * height) + SurfaceView.Rect;
                                                            }
                                                            if (model.X == 0)
                                                            {
                                                                var width = paint.MeasureText(model.Text);
                                                                model.X = Convert.ToInt32((SurfaceView.Width - width) / 2);
                                                            }
                                                            canvas.DrawText(model.Text, model.X, model.Y, paint);
                                                            if (model.DisplayTime - 5 >= model.StartPoint)
                                                                model.Enable = false;
                                                            else
                                                                model.DisplayTime = Time;
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
                                                            if (model.Y == 0)
                                                            {
                                                                int height = paint.GetDisplayHeight();
                                                                model.Y = height + SurfaceView.Random.Next(SurfaceView.Height - height);
                                                            }
                                                            if (model.X == 0)
                                                            {
                                                                var width = paint.MeasureText(model.Text);
                                                                model.W = -width;
                                                                model.X = (SurfaceView.Width + width).ToInt32();
                                                            }
                                                            canvas.DrawText(model.Text, model.X -= model.Speed, model.Y, paint);
                                                            if (model.X < model.W && model.Y != 0)
                                                                model.Enable = false;
                                                            break;
                                                        }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Debug.WriteLine("ViewThread.Foreach:" + ex.ToString());
                                            }
                                        }
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
                                            if (model.Y == 0)
                                            {
                                                int height = SurfaceView.Paint.GetDisplayHeight();
                                                model.Y = height + SurfaceView.Random.Next(SurfaceView.Height - height);
                                            }
                                            if (model.X == 0)
                                            {
                                                var width = SurfaceView.Paint.MeasureText(model.Text);
                                                model.W = -width;
                                                model.X = Convert.ToInt32((SurfaceView.Width + width) * 1.2);
                                            }
                                            canvas.DrawText(model.Text, model.X -= SurfaceView.Speed, model.Y, SurfaceView.Paint);
                                            if (model.X < model.W && model.Y != 0)
                                                SurfaceView.DanmuMsgModels.Remove(model);
                                        }
                                        catch (Exception ex)
                                        {
                                            Debug.WriteLine("ViewThread.Foreach:" + ex.ToString());
                                        }
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
                    Debug.WriteLine("ViewThread.Catch" + ex.ToString());
                }
            }
        }
        private Paint InitPaint(DanmakuModel model)
        {
            var p = new Paint(SurfaceView.Paint)
            {
                TextSize = Convert.ToSingle(model.Size),
                Color = model.Color
            };
            return p;
        }
    }
}