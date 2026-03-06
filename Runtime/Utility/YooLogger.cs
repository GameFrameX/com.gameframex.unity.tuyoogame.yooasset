using System;
using System.Diagnostics;

namespace YooAsset
{
    /// <summary>
    /// 自定义日志处理
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public interface ILogger
    {
        [UnityEngine.Scripting.Preserve]
        void Log(string message);
        [UnityEngine.Scripting.Preserve]
        void Warning(string message);
        [UnityEngine.Scripting.Preserve]
        void Error(string message);
        [UnityEngine.Scripting.Preserve]
        void Exception(Exception exception);
    }

    [UnityEngine.Scripting.Preserve]
    public static class YooLogger
    {
        public static ILogger Logger = null;

        /// <summary>
        /// 日志
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        [Conditional("DEBUG")]
        public static void Log(string info)
        {
            if (Logger != null)
            {
                Logger.Log(GetTime() + info);
            }
            else
            {
                UnityEngine.Debug.Log(GetTime() + info);
            }
        }

        /// <summary>
        /// 警告
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void Warning(string info)
        {
            if (Logger != null)
            {
                Logger.Warning(GetTime() + info);
            }
            else
            {
                UnityEngine.Debug.LogWarning(GetTime() + info);
            }
        }

        /// <summary>
        /// 错误
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void Error(string info)
        {
            if (Logger != null)
            {
                Logger.Error(GetTime() + info);
            }
            else
            {
                UnityEngine.Debug.LogError(GetTime() + info);
            }
        }

        [UnityEngine.Scripting.Preserve]
        private static string GetTime()
        {
            return $"[YooAsset]:[{DateTime.Now:HH:mm:ss.fff}]:";
        }

        /// <summary>
        /// 异常
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void Exception(Exception exception)
        {
            if (Logger != null)
            {
                Logger.Exception(exception);
            }
            else
            {
                UnityEngine.Debug.LogException(exception);
            }
        }
    }
}