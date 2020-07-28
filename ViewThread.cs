using System;
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
        private double Time { get; set; } = 0;
        private Timer Timer { get; }
        private DanmakuSurfaceView SurfaceView { get; }
        public bool Runable { get; internal set; } = true;
        public ViewThread(DanmakuSurfaceView surfaceView)
        {
            SurfaceView = surfaceView;
            Timer = new Timer(100);
            Timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Time += 0.1;
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
                                                            if (model.y == 0)
                                                            {
                                                                var size = Convert.ToInt32(paint.TextSize * 1.1);
                                                                var count = Convert.ToInt32(StaticValue.WindowHeight / 3 / size);
                                                                SurfaceView.Random.Next(0, count);
                                                                model.y = size * count;
                                                            }
                                                            if (model.x == 0)
                                                            {
                                                                model.x = Convert.ToInt32(StaticValue.WindowWidth / 2) -
                                                                Convert.ToInt32(SurfaceView.Paint.MeasureText(model.text));
                                                            }
                                                            canvas.DrawText(model.text, model.x, model.y, paint);
                                                            if (model.remind == 0)
                                                                model.enable = false;
                                                            else
                                                                model.remind--;
                                                            break;
                                                        }
                                                    case DanmakuLocation.Bottom:
                                                        {
                                                            Paint paint = InitPaint(model);
                                                            if (model.y == 0)
                                                            {
                                                                var size = Convert.ToInt32(paint.TextSize * 1.1);
                                                                var count = Convert.ToInt32(StaticValue.WindowHeight / 3 / size);
                                                                SurfaceView.Random.Next(0, count);
                                                                model.y = SurfaceView.BottomSize + (size * count);
                                                            }
                                                            if (model.x == 0)
                                                            {
                                                                model.x = Convert.ToInt32(StaticValue.WindowWidth / 2) -
                                                                Convert.ToInt32(SurfaceView.Paint.MeasureText(model.text));
                                                            }
                                                            canvas.DrawText(model.text, model.x, model.y, paint);
                                                            if (model.remind == 0)
                                                                model.enable = false;
                                                            else
                                                                model.remind--;
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
                                                                var size = Convert.ToInt32(paint.TextSize);
                                                                model.y = size + SurfaceView.Random.Next(SurfaceView.Height - size);
                                                            }
                                                            if (model.x == 0)
                                                                model.x = SurfaceView.Width + Convert.ToInt32(System.Math.Ceiling(paint.MeasureText(model.text)) * 1.2);
                                                            canvas.DrawText(model.text, model.x -= model.Speed, model.y, paint);
                                                            if (model.x <= 0 && model.y != 0)
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
                                    foreach (var model in SurfaceView.DanmuMsgModels)
                                    {
                                        try
                                        {
                                            if (model.y == 0)
                                            {
                                                var size = Convert.ToInt32(SurfaceView.Paint.TextSize);
                                                model.y = size + SurfaceView.Random.Next(SurfaceView.Height - size);
                                            }
                                            if (model.x == 0)
                                                model.x = SurfaceView.Width + Convert.ToInt32(System.Math.Ceiling(SurfaceView.Paint.MeasureText(model.text)) * 1.2);
                                            canvas.DrawText(model.text, model.x -= SurfaceView.Speed, model.y, SurfaceView.Paint);
                                            if (model.x <= 0 && model.y != 0)
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
                    LogManager.Instance.LogError("ViewThread", ex);
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