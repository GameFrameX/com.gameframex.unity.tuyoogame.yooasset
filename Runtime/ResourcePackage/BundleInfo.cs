namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class BundleInfo
    {
        private readonly string _importFilePath;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// 资源包对象
        /// </summary>
        public readonly PackageBundle Bundle;

        /// <summary>
        /// 注意：该字段只用于帮助编辑器下的模拟模式。
        /// </summary>
        public string[] IncludeAssetsInEditor;


        [UnityEngine.Scripting.Preserve]
        public BundleInfo(IFileSystem fileSystem, PackageBundle bundle)
        {
            _fileSystem = fileSystem;
            Bundle = bundle;
            _importFilePath = null;
        }

        [UnityEngine.Scripting.Preserve]
        public BundleInfo(IFileSystem fileSystem, PackageBundle bundle, string importFilePath)
        {
            _fileSystem = fileSystem;
            Bundle = bundle;
            _importFilePath = importFilePath;
        }

        /// <summary>
        /// 加载资源文件
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public FSLoadBundleOperation LoadBundleFile()
        {
            return _fileSystem.LoadBundleFile(Bundle);
        }

        /// <summary>
        /// 卸载资源文件
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void UnloadBundleFile(object result)
        {
            _fileSystem.UnloadBundleFile(Bundle, result);
        }

        /// <summary>
        /// 创建下载器
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public FSDownloadFileOperation CreateDownloader(int failedTryAgain, int timeout)
        {
            var downloadParam = new DownloadParam(failedTryAgain, timeout);
            downloadParam.ImportFilePath = _importFilePath;
            return _fileSystem.DownloadFileAsync(Bundle, downloadParam);
        }

        /// <summary>
        /// 是否需要从远端下载
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool IsNeedDownloadFromRemote()
        {
            return _fileSystem.NeedDownload(Bundle);
        }

        /// <summary>
        /// 下载器合并识别码
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public string GetDownloadCombineGUID()
        {
            return $"{_fileSystem.GetHashCode()}_{Bundle.BundleGUID}";
        }
    }
}