using System;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class HostPlayModeImpl : IPlayMode, IBundleQuery
    {
        public readonly string PackageName;
        public IFileSystem BuildinFileSystem { set; get; } //可以为空！
        public IFileSystem DeliveryFileSystem { set; get; } //可以为空！
        public IFileSystem CacheFileSystem { set; get; }


        [UnityEngine.Scripting.Preserve]
        public HostPlayModeImpl(string packageName)
        {
            PackageName = packageName;
        }

        /// <summary>
        /// 异步初始化
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public InitializationOperation InitializeAsync(HostPlayModeParameters initParameters)
        {
            var operation = new HostPlayModeInitializationOperation(this, initParameters);
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

            if (DeliveryFileSystem != null)
            {
                DeliveryFileSystem.OnUpdate();
            }

            if (CacheFileSystem != null)
            {
                CacheFileSystem.OnUpdate();
            }
        }

        /// <summary>
        /// 获取本地版本号
        /// 优先获取沙盒目录，如果没有则用包内的版本号。
        /// </summary>
        /// <param name="appendTimeTicks"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        LoadLocalVersionOperation IPlayMode.LoadLocalVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new LoadLocalVersionImplOperation(BuildinFileSystem, CacheFileSystem, appendTimeTicks, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        /// <summary>
        /// 获取本地资源清单
        /// 优化获取沙盒目录，如果没有则用包内的
        /// </summary>
        /// <param name="packageVersion"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        [UnityEngine.Scripting.Preserve]
        LoadLocalManifestOperation IPlayMode.LoadLocalManifestAsync(string packageVersion, int timeout)
        {
            var operation = new LoadLocalManifestImplOperation(this, BuildinFileSystem, CacheFileSystem, packageVersion, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        RequestPackageVersionOperation IPlayMode.RequestPackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new RequestPackageVersionImplOperation(CacheFileSystem, appendTimeTicks, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        UpdatePackageManifestOperation IPlayMode.UpdatePackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new UpdatePackageManifestImplOperation(this, CacheFileSystem, packageVersion, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        PreDownloadContentOperation IPlayMode.PreDownloadContentAsync(string packageVersion, int timeout)
        {
            var operation = new HostPlayModePreDownloadContentOperation(this, packageVersion, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ClearAllBundleFilesOperation IPlayMode.ClearAllBundleFilesAsync()
        {
            var operation = new ClearAllBundleFilesImplOperation(this, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ClearUnusedBundleFilesOperation IPlayMode.ClearUnusedBundleFilesAsync()
        {
            var operation = new ClearUnusedBundleFilesImplOperation(this, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation IPlayMode.CreateResourceDownloaderByAll(int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = PlayModeHelper.GetDownloadListByAll(ActiveManifest, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            var operation = new ResourceDownloaderOperation(PackageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation IPlayMode.CreateResourceDownloaderByTags(string[] tags, int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = PlayModeHelper.GetDownloadListByTags(ActiveManifest, tags, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            var operation = new ResourceDownloaderOperation(PackageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceDownloaderOperation IPlayMode.CreateResourceDownloaderByPaths(AssetInfo[] assetInfos, int downloadingMaxNumber, int failedTryAgain, int timeout)
        {
            var downloadList = PlayModeHelper.GetDownloadListByPaths(ActiveManifest, assetInfos, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            var operation = new ResourceDownloaderOperation(PackageName, downloadList, downloadingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceUnpackerOperation IPlayMode.CreateResourceUnpackerByAll(int upackingMaxNumber, int failedTryAgain, int timeout)
        {
            var unpcakList = PlayModeHelper.GetUnpackListByAll(ActiveManifest, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            var operation = new ResourceUnpackerOperation(PackageName, unpcakList, upackingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceUnpackerOperation IPlayMode.CreateResourceUnpackerByTags(string[] tags, int upackingMaxNumber, int failedTryAgain, int timeout)
        {
            var unpcakList = PlayModeHelper.GetUnpackListByTags(ActiveManifest, tags, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
            var operation = new ResourceUnpackerOperation(PackageName, unpcakList, upackingMaxNumber, failedTryAgain, timeout);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        ResourceImporterOperation IPlayMode.CreateResourceImporterByFilePaths(string[] filePaths, int importerMaxNumber, int failedTryAgain, int timeout)
        {
            var importerList = PlayModeHelper.GetImporterListByFilePaths(ActiveManifest, filePaths, BuildinFileSystem, DeliveryFileSystem, CacheFileSystem);
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

            if (BuildinFileSystem != null && BuildinFileSystem.Belong(packageBundle))
            {
                var bundleInfo = new BundleInfo(BuildinFileSystem, packageBundle);
                return bundleInfo;
            }

            if (DeliveryFileSystem != null && DeliveryFileSystem.Belong(packageBundle))
            {
                var bundleInfo = new BundleInfo(DeliveryFileSystem, packageBundle);
                return bundleInfo;
            }

            if (CacheFileSystem.Belong(packageBundle))
            {
                var bundleInfo = new BundleInfo(CacheFileSystem, packageBundle);
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