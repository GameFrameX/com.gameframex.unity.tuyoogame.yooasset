namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal abstract class FSDownloadFileOperation : AsyncOperationBase
    {
        public PackageBundle Bundle { private set; get; }

        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount { private set; get; }

        /// <summary>
        /// HTTP返回码
        /// </summary>
        public long HttpCode { protected set; get; }

        /// <summary>
        /// 当前下载的字节数
        /// </summary>
        public long DownloadedBytes { protected set; get; }

        /// <summary>
        /// 当前下载进度（0f - 1f）
        /// </summary>
        public float DownloadProgress { protected set; get; }


        [UnityEngine.Scripting.Preserve]
        public FSDownloadFileOperation(PackageBundle bundle)
        {
            Bundle = bundle;
            RefCount = 0;
            HttpCode = 0;
            DownloadedBytes = 0;
            DownloadProgress = 0;
        }

        [UnityEngine.Scripting.Preserve]
        public void Release()
        {
            RefCount--;
        }

        [UnityEngine.Scripting.Preserve]
        public void Reference()
        {
            RefCount++;
        }
    }
}