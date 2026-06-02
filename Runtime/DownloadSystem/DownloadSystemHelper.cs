using UnityEngine.Networking;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public class DownloadSystemHelper
    {
        private static System.Func<string, UnityWebRequest> _unityWebRequestCreater = null;

        /// <summary>
        /// 设置自定义的 UnityWebRequest 创建委托
        /// </summary>
        public static void SetUnityWebRequestCreater(System.Func<string, UnityWebRequest> creater)
        {
            _unityWebRequestCreater = creater;
        }

        [UnityEngine.Scripting.Preserve]
        public static UnityWebRequest NewUnityWebRequestGet(string requestURL)
        {
            UnityWebRequest webRequest;
            if (_unityWebRequestCreater != null)
            {
                webRequest = _unityWebRequestCreater.Invoke(requestURL);
            }
            else
            {
                webRequest = new UnityWebRequest(requestURL, UnityWebRequest.kHttpVerbGET);
            }

            return webRequest;
        }

        /// <summary>
        /// 获取WWW加载本地资源的路径
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static string ConvertToWWWPath(string path)
        {
#if UNITY_EDITOR
            return StringUtility.Format("file:///{0}", path);
#elif UNITY_WEBGL
            return path;
#elif UNITY_IPHONE
            return StringUtility.Format("file://{0}", path);
#elif UNITY_ANDROID
            if (path.StartsWith("jar:file://"))
                return path;
            else
                return StringUtility.Format("jar:file://{0}", path);
#elif UNITY_STANDALONE_OSX
            return new System.Uri(path).ToString();
#elif UNITY_STANDALONE
            return StringUtility.Format("file:///{0}", path);
#elif UNITY_OPENHARMONY
            return StringUtility.Format("file://{0}", path);
#else
            throw new System.NotImplementedException();
#endif
        }
    }
}