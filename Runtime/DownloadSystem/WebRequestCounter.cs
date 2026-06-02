using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public sealed class WebRequestCounter
    {
        private static readonly object Lock = new object();
        private static readonly Dictionary<string, int> RequestFailedRecorder = new Dictionary<string, int>(1000);

        /// <summary>
        /// 记录请求失败事件
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void RecordRequestFailed(string packageName, string eventName)
        {
            var key = $"{packageName}_{eventName}";
            lock (Lock)
            {
                if (RequestFailedRecorder.ContainsKey(key) == false)
                {
                    RequestFailedRecorder.Add(key, 0);
                }

                RequestFailedRecorder[key]++;
            }
        }

        /// <summary>
        /// 获取请求失败的次数
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static int GetRequestFailedCount(string packageName, string eventName)
        {
            var key = $"{packageName}_{eventName}";
            lock (Lock)
            {
                if (RequestFailedRecorder.ContainsKey(key) == false)
                {
                    RequestFailedRecorder.Add(key, 0);
                }

                return RequestFailedRecorder[key];
            }
        }
    }
}
