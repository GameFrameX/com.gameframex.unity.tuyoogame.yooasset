namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal interface IPlayMode
    {
        /// <summary>
        /// 当前激活的清单
        /// </summary>
        PackageManifest ActiveManifest { set; get; }

        /// <summary>
        /// 更新游戏模式
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        void UpdatePlayMode();

        /// <summary>
        /// 获取本地最新的资源版本
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        LoadLocalVersionOperation LoadLocalVersionAsync(bool appendTimeTicks, int timeout);

        /// <summary>
        /// 获取本地最新的资源清单
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        LoadLocalManifestOperation LoadLocalManifestAsync(string packageVersion, int timeout);

        /// <summary>
        /// 向网络端请求最新的资源版本
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        RequestPackageVersionOperation RequestPackageVersionAsync(bool appendTimeTicks, int timeout);

        /// <summary>
        /// 向网络端请求并更新清单
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        UpdatePackageManifestOperation UpdatePackageManifestAsync(string packageVersion, int timeout);

        /// <summary>
        /// 预下载指定版本的包裹内容
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        PreDownloadContentOperation PreDownloadContentAsync(string packageVersion, int timeout);

        /// <summary>
        /// 清空所有文件
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        ClearAllBundleFilesOperation ClearAllBundleFilesAsync();

        /// <summary>
        /// 清空未使用的文件
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        ClearUnusedBundleFilesOperation ClearUnusedBundleFilesAsync();

        // 下载相关
        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation CreateResourceDownloaderByAll(int downloadingMaxNumber, int failedTryAgain, int timeout);
        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation CreateResourceDownloaderByTags(string[] tags, int downloadingMaxNumber, int failedTryAgain, int timeout);
        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation CreateResourceDownloaderByPaths(AssetInfo[] assetInfos, int downloadingMaxNumber, int failedTryAgain, int timeout);

        // 解压相关
        [UnityEngine.Scripting.Preserve]
        ResourceUnpackerOperation CreateResourceUnpackerByAll(int upackingMaxNumber, int failedTryAgain, int timeout);
        [UnityEngine.Scripting.Preserve]
        ResourceUnpackerOperation CreateResourceUnpackerByTags(string[] tags, int upackingMaxNumber, int failedTryAgain, int timeout);

        // 导入相关
        [UnityEngine.Scripting.Preserve]
        ResourceImporterOperation CreateResourceImporterByFilePaths(string[] filePaths, int importerMaxNumber, int failedTryAgain, int timeout);
    }
}