using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
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
            Speed = 16;
            Paint = new Paint
            {
                Color = Color.WhiteSmoke,
                AntiAlias = true,
                StrokeWidth = 2,
                TextSize = 36
            };
            SetZOrderOnTop(true);
            Holder.AddCallback(this);
            Random = new Random(1);
            Holder.SetFormat(Format.Translucent);
            SetBackgroundColor(Color.Transparent);
            Paint.SetShadowLayer(2, 1, 1, Color.Black);
        }
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            Rect = h / 2;
        }
        public void Pause() => Thread.Runable = false;
        public void Resume() => Thread.Runable = true;
        public void AddDanmaku(LiveDanmuModel danmaku)
        {
            try
            {
                if (danmaku.Type == LiveDanmuTypes.Danmu && DanmakuType == DanmakuType.Live)
                    DanmuMsgModels.Add(danmaku.Value);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AddDanmaku:" + ex.ToString());
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
                Debug.WriteLine("SurfaceCreated:" + ex.ToString());
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