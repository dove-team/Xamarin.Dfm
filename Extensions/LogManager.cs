using System;
using System.IO;
using Serilog;
using Serilog.Core;

namespace Xamarin.Dfm.Extensions
{
    public sealed class LogManager
    {
        public static LogManager Instance { get; } = new LogManager();
        private Logger Logger { get; }
        private LogManager()
        {
            var LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "bilibili_log", "log-.txt");
            Logger = new LoggerConfiguration().MinimumLevel.Debug()
                .WriteTo.File(LogFile, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="messageTemplate">错误方法或者备注</param>
        /// <param name="exception">错误信息</param>
        /// <param name="Serious">是否严重错误</param>
        public void LogError(string messageTemplate, Exception exception, bool Serious = false)
        {
            if (!Serious) return;
            try
            {
                if (exception != null)
                    Logger.Error(exception, messageTemplate);
            }
            catch { }
        }
        public void LogInfo(string messageTemplate)
        {
            try
            {
                Logger.Information(messageTemplate);
            }
            catch { }
        }
        public void LogInfo(string messageTemplate, params object[] propertyValues)
        {
            try
            {
                Logger.Information(messageTemplate, propertyValues);
            }
            catch { }
        }
    }
}