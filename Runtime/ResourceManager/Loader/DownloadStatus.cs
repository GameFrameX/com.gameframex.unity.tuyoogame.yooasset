namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public struct DownloadStatus
    {
        /// <summary>
        /// 下载是否已经完成
        /// </summary>
        public bool IsDone;

        /// <summary>
        /// 下载进度（0-1f)
        /// </summary>
        public float Progress;

        /// <summary>
        /// 下载文件的总大小
        /// </summary>
        public long TotalBytes;

        /// <summary>
        /// 已经下载的文件大小
        /// </summary>
        public long DownloadedBytes;

        [UnityEngine.Scripting.Preserve]
        public static DownloadStatus CreateDefaultStatus()
        {
            var status = new DownloadStatus();
            return status;
        }
    }
}