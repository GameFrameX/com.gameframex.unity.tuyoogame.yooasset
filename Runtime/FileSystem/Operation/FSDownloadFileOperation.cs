namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class FSDownloadFileOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        public PackageBundle Bundle { private set; get; }

        /// <summary>
        /// 引用计数
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public int RefCount { private set; get; }

        /// <summary>
        /// HTTP返回码
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public long HttpCode { protected set; get; }

        /// <summary>
        /// 当前下载的字节数
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public long DownloadedBytes { protected set; get; }

        /// <summary>
        /// 当前下载进度（0f - 1f）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
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