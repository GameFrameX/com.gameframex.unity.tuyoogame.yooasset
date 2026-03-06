using System;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public class WebRequestCounter
    {
        /// <summary>
        /// 记录网络请求失败事件的次数
        /// </summary>
        private static readonly Dictionary<string, int> _requestFailedRecorder = new(1000);

        /// <summary>
        /// 记录请求失败事件
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static void RecordRequestFailed(string packageName, string eventName)
        {
            var key = $"{packageName}_{eventName}";
            if (_requestFailedRecorder.ContainsKey(key) == false)
            {
                _requestFailedRecorder.Add(key, 0);
            }

            _requestFailedRecorder[key]++;
        }

        /// <summary>
        /// 获取请求失败的次数
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static int GetRequestFailedCount(string packageName, string eventName)
        {
            var key = $"{packageName}_{eventName}";
            if (_requestFailedRecorder.ContainsKey(key) == false)
            {
                _requestFailedRecorder.Add(key, 0);
            }

            return _requestFailedRecorder[key];
        }
    }
}