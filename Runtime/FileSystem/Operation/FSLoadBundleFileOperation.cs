using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class FSLoadBundleOperation : AsyncOperationBase
    {
        /// <summary>
        /// 加载结果
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public object Result { protected set; get; }

        /// <summary>
        /// 下载进度
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public float DownloadProgress { protected set; get; } = 0;

        /// <summary>
        /// 下载大小
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public long DownloadedBytes { protected set; get; } = 0;

        /// <summary>
        /// 终止下载任务
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public abstract void AbortDownloadOperation();
    }
}