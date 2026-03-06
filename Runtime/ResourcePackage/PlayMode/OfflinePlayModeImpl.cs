using System;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class OfflinePlayModeImpl : IPlayMode, IBundleQuery
    {
        public readonly string PackageName;
        public IFileSystem BuildinFileSystem { set; get; }


        [UnityEngine.Scripting.Preserve]
        public OfflinePlayModeImpl(string packageName)
        {
            PackageName = packageName;
        }

        /// <summary>
        /// 异步初始化
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public InitializationOperation InitializeAsync(OfflinePlayModeParameters initParameters)
        {
            var operation = new OfflinePlayModeInitializationOperation(this, initParameters);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        #region IPlayMode接口

        public PackageManifest ActiveManifest { set; get; }

        [UnityEngine.Scripting.Preserve]
        void IPlayMode.UpdatePlayMode()
        {
            if (BuildinFileSystem != null)
            {
                BuildinFileSystem.OnUpdate();
            }
        }

        [UnityEngine.Scripting.Preserve]
        LoadLocalVersionOperation IPlayMode.LoadLocalVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new LoadLocalVersionImplOperation(BuildinFileSystem, null, appendTimeTicks, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        LoadLocalManifestOperation IPlayMode.LoadLocalManifestAsync(string packageVersion, int timeout)
        {
            var operation = new LoadLocalManifestImplOperation(this, BuildinFileSystem, null, packageVersion, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        RequestPackageVersionOperation IPlayMode.RequestPackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new RequestPackageVersionImplOperation(BuildinFileSystem, appendTimeTicks, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        UpdatePackageManifestOperation IPlayMode.UpdatePackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new UpdatePackageManifestImplOperation(this, BuildinFileSystem, packageVersion, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        PreDownloadContentOperation IPlayMode.PreDownloadContentAsync(string packageVersion, int timeout)
        {
            var operation = new OfflinePlayModePreDownloadContentOperation(this);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ClearAllBundleFilesOperation IPlayMode.ClearAllBundleFilesAsync()
        {
            var operation = new ClearAllBundleFilesImplOperation(this, BuildinFileSystem, null, null);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ClearUnusedBundleFilesOperation IPlayMode.ClearUnusedBundleFilesAsync()
        {
            var operation = new ClearUnusedBundleFilesImplOperation(this, BuildinFileSystem, null, null);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation IPlayMode.CreateResourceDownloaderByAll(int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = PlayModeHelper.GetDownloadListByAll(ActiveManifest, BuildinFileSystem);
            var operation = new ResourceDownloaderOperation(PackageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation IPlayMode.CreateResourceDownloaderByTags(string[] tags, int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = PlayModeHelper.GetDownloadListByTags(ActiveManifest, tags, BuildinFileSystem);
            var operation = new ResourceDownloaderOperation(PackageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation IPlayMode.CreateResourceDownloaderByPaths(AssetInfo[] assetInfos, int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = PlayModeHelper.GetDownloadListByPaths(ActiveManifest, assetInfos, BuildinFileSystem);
            var operation = new ResourceDownloaderOperation(PackageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceUnpackerOperation IPlayMode.CreateResourceUnpackerByAll(int upackingMaxNumber, int failedTryAgain, int timeout)
        {
            var unpcakList = PlayModeHelper.GetUnpackListByAll(ActiveManifest, BuildinFileSystem);
            var operation = new ResourceUnpackerOperation(PackageName, unpcakList, upackingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceUnpackerOperation IPlayMode.CreateResourceUnpackerByTags(string[] tags, int upackingMaxNumber, int failedTryAgain, int timeout)
        {
            var unpcakList = PlayModeHelper.GetUnpackListByTags(ActiveManifest, tags, BuildinFileSystem);
            var operation = new ResourceUnpackerOperation(PackageName, unpcakList, upackingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceImporterOperation IPlayMode.CreateResourceImporterByFilePaths(string[] filePaths, int importerMaxNumber, int failedTryAgain, int timeout)
        {
            var importerList = PlayModeHelper.GetImporterListByFilePaths(ActiveManifest, filePaths, BuildinFileSystem);
            var operation = new ResourceImporterOperation(PackageName, importerList, importerMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        #endregion

        #region IBundleQuery接口

        [UnityEngine.Scripting.Preserve]
        private BundleInfo CreateBundleInfo(PackageBundle packageBundle, AssetInfo assetInfo)
        {
            if (packageBundle == null)
            {
                throw new Exception("Should never get here !");
            }

            if (BuildinFileSystem.Belong(packageBundle))
            {
                var bundleInfo = new BundleInfo(BuildinFileSystem, packageBundle);
                return bundleInfo;
            }

            throw new Exception($"Can not found belong file system : {packageBundle.BundleName}");
        }

        [UnityEngine.Scripting.Preserve]
        BundleInfo IBundleQuery.GetMainBundleInfo(AssetInfo assetInfo)
        {
            if (assetInfo.IsInvalid)
            {
                throw new Exception("Should never get here !");
            }

            // 注意：如果清单里未找到资源包会抛出异常！
            var packageBundle = ActiveManifest.GetMainPackageBundle(assetInfo.AssetPath);
            return CreateBundleInfo(packageBundle, assetInfo);
        }

        [UnityEngine.Scripting.Preserve]
        BundleInfo[] IBundleQuery.GetDependBundleInfos(AssetInfo assetInfo)
        {
            if (assetInfo.IsInvalid)
            {
                throw new Exception("Should never get here !");
            }

            // 注意：如果清单里未找到资源包会抛出异常！
            var depends = ActiveManifest.GetAllDependencies(assetInfo.AssetPath);
            var result = new List<BundleInfo>(depends.Length);
            foreach (var packageBundle in depends)
            {
                var bundleInfo = CreateBundleInfo(packageBundle, assetInfo);
                result.Add(bundleInfo);
            }

            return result.ToArray();
        }

        [UnityEngine.Scripting.Preserve]
        string IBundleQuery.GetMainBundleName(AssetInfo assetInfo)
        {
            if (assetInfo.IsInvalid)
            {
                throw new Exception("Should never get here !");
            }

            // 注意：如果清单里未找到资源包会抛出异常！
            var packageBundle = ActiveManifest.GetMainPackageBundle(assetInfo.AssetPath);
            return packageBundle.BundleName;
        }

        [UnityEngine.Scripting.Preserve]
        string[] IBundleQuery.GetDependBundleNames(AssetInfo assetInfo)
        {
            if (assetInfo.IsInvalid)
            {
                throw new Exception("Should never get here !");
            }

            // 注意：如果清单里未找到资源包会抛出异常！
            var depends = ActiveManifest.GetAllDependencies(assetInfo.AssetPath);
            var result = new List<string>(depends.Length);
            foreach (var packageBundle in depends)
            {
                result.Add(packageBundle.BundleName);
            }

            return result.ToArray();
        }

        [UnityEngine.Scripting.Preserve]
        bool IBundleQuery.ManifestValid()
        {
            return ActiveManifest != null;
        }

        #endregion
    }
}