using UnityEngine;

namespace YooAsset
{
    public abstract class LoadWebAssetBundleOperation : AsyncOperationBase
    {
        /// <summary>
        /// AssetBundle对象
        /// </summary>
        public AssetBundle Result;

        /// <summary>
        /// 下载进度
        /// </summary>
        public float DownloadProgress { protected set; get; } = 0;

        /// <summary>
        /// 下载大小
        /// </summary>
        public long DownloadedBytes { protected set; get; } = 0;
    }
}