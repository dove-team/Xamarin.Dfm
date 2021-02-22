﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Dfm.Model;
using JavaObject = Java.Lang.Object;

namespace Xamarin.Dfm
{
    public sealed class DanmakuAdapter : JavaObject
    {
        public DanmakuSurfaceView DanmakuView { get; set; }
        public DanmakuAdapter(DanmakuSurfaceView danmakuSurfaceView)
        {
            DanmakuView = danmakuSurfaceView;
        }
        public void InitDanmu(List<DanmakuModel> danmakus)
        {
            try
            {
                DanmakuView.DanmakuModels = danmakus;
                SetDanmakuState(DanmakuState.Running);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("InitDanmu" + ex.Message);
            }
        }
        public void ShowLiveDanmaku(LiveDanmuModel model)
        {
            DanmakuView.AddDanmaku(model);
        }
        public void SetDanmakuState(DanmakuState danmakuState)
        {
            try
            {
                switch (danmakuState)
                {
                    case DanmakuState.Pause:
                        {
                            DanmakuView.Pause();
                            break;
                        }
                    case DanmakuState.Running:
                        {
                            DanmakuView.Resume();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SetState" + ex.Message);
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            try
            {
                SetDanmakuState(DanmakuState.Pause);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DanmakuAdapter-Dispose" + ex.Message);
            }
        }
    }
}