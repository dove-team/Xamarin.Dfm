using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Xamarin.Dfm.Extensions;
using Xamarin.Dfm.Model;

namespace Xamarin.Dfm
{
    public sealed class DanmakuSurfaceView : SurfaceView, ISurfaceHolderCallback
    {
        public Paint Paint { get; }
        public Random Random { get; }
        public ViewThread Thread { get; private set; }
        private DanmakuType _DanmakuType;
        public DanmakuType DanmakuType
        {
            get { return _DanmakuType; }
            set
            {
                if (_DanmakuType == DanmakuType.Live)
                    DanmuMsgModels = new List<DanmuMsgModel>();
                _DanmakuType = value;
            }
        }
        public int Rect { get; private set; }
        public int Speed { get; private set; } = 0;
        public List<DanmakuModel> DanmakuModels { get; set; }
        public List<DanmuMsgModel> DanmuMsgModels { get; private set; }
        public DanmakuSurfaceView(Context context) : this(context, default) { }
        public DanmakuSurfaceView(Context context, IAttributeSet attrs) : this(context, attrs, default) { }
        public DanmakuSurfaceView(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }
        public DanmakuSurfaceView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Holder.AddCallback(this);
            Holder.SetFormat(Format.Translucent);
            SetZOrderOnTop(true);
            Speed = 16;
            SetBackgroundColor(Color.Transparent);
            Random = new Random(1);
            Paint = new Paint
            {
                Color = Color.WhiteSmoke,
                AntiAlias = true,
                StrokeWidth = 2,
                TextSize = 36
            };
            Paint.SetShadowLayer(2, 1, 1, Color.Black);
        }
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            Rect = h / 2;
        }
        public void Pause()
        {
            Thread.Runable = false;
        }
        public void Resume()
        {
            Thread.Runable = true;
        }
        public void AddDanmaku(LiveDanmuModel danmaku)
        {
            try
            {
                if (danmaku.type == LiveDanmuTypes.Danmu && DanmakuType == DanmakuType.Live)
                {
                    var model = danmaku.value as DanmuMsgModel;
                    DanmuMsgModels.Add(model);
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("AddDanmaku", ex);
            }
        }
        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height) { }
        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                Thread = new ViewThread(this);
                Thread.Start();
            }
            catch (Exception ex)
            {
                LogManager.Instance.LogError("SurfaceCreated", ex);
            }
        }
        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            try
            {
                Thread.Runable = false;
                Thread.Dispose();
                GC.Collect();
            }
            catch { }
        }
    }
}