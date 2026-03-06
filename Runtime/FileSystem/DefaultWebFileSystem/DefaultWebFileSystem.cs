using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    /// <summary>
    /// Web文件系统
    /// </summary>
    internal class DefaultWebFileSystem : IFileSystem
    {
        public class FileWrapper
        {
            public string FileName { private set; get; }

            public FileWrapper(string fileName)
            {
                FileName = fileName;
            }
        }

        protected readonly Dictionary<string, FileWrapper> _wrappers = new(10000);
        protected readonly Dictionary<string, string> _webFilePaths = new(10000);
        protected string _webPackageRoot = string.Empty;

        /// <summary>
        /// 包裹名称
        /// </summary>
        public string PackageName { private set; get; }

        /// <summary>
        /// 文件根目录
        /// </summary>
        public string FileRoot
        {
            get { return _webPackageRoot; }
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount
        {
            get { return 0; }
        }

        #region 自定义参数

        /// <summary>
        /// 禁用Unity的网络缓存
        /// </summary>
        public bool DisableUnityWebCache { private set; get; } = false;

        #endregion


        public DefaultWebFileSystem()
        {
        }

        public virtual FSInitializeFileSystemOperation InitializeFileSystemAsync()
        {
            var operation = new DWFSInitializeOperation(this);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSRequestPackageVersionOperation LoadLocalPackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DWFSRequestPackageVersionOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSLoadPackageManifestOperation LoadLocalPackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DWFSLoadPackageManifestOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSLoadPackageManifestOperation RequestRemotePackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DWFSLoadPackageManifestOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSRequestPackageVersionOperation RequestRemotePackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DWFSRequestPackageVersionOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSClearAllBundleFilesOperation ClearAllBundleFilesAsync()
        {
            var operation = new FSClearAllBundleFilesCompleteOperation();
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSClearUnusedBundleFilesOperation ClearUnusedBundleFilesAsync(PackageManifest manifest)
        {
            var operation = new FSClearUnusedBundleFilesCompleteOperation();
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual FSDownloadFileOperation DownloadFileAsync(PackageBundle bundle, DownloadParam param)
        {
            throw new NotImplementedException();
        }

        public virtual FSLoadBundleOperation LoadBundleFile(PackageBundle bundle)
        {
            var operation = new DWFSLoadAssetBundleOperation(this, bundle);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        public virtual void UnloadBundleFile(PackageBundle bundle, object result)
        {
            var assetBundle = result as AssetBundle;
            if (assetBundle == null)
            {
                return;
            }

            if (assetBundle != null)
            {
                assetBundle.Unload(true);
            }
        }

        public virtual void SetParameter(string name, object value)
        {
            if (name == FileSystemParametersDefine.DISABLE_UNITY_WEB_CACHE)
            {
                DisableUnityWebCache = (bool)value;
            }
            else
            {
                YooLogger.Warning($"Invalid parameter : {name}");
            }
        }

        public virtual void OnCreate(string packageName, string rootDirectory)
        {
            PackageName = packageName;

            if (string.IsNullOrEmpty(rootDirectory))
            {
                rootDirectory = GetDefaultWebRoot();
            }

            _webPackageRoot = PathUtility.Combine(rootDirectory, packageName);
        }

        public virtual void OnUpdate()
        {
        }

        public virtual bool Belong(PackageBundle bundle)
        {
            return true;
        }

        public virtual bool Exists(PackageBundle bundle)
        {
            return true;
        }

        public virtual bool NeedDownload(PackageBundle bundle)
        {
            return false;
        }

        public virtual bool NeedUnpack(PackageBundle bundle)
        {
            return false;
        }

        public virtual bool NeedImport(PackageBundle bundle)
        {
            return false;
        }

        public virtual byte[] ReadFileData(PackageBundle bundle)
        {
            throw new NotImplementedException();
        }

        public virtual string ReadFileText(PackageBundle bundle)
        {
            throw new NotImplementedException();
        }

        #region 内部方法

        protected string GetDefaultWebRoot()
        {
            var path = PathUtility.Combine(Application.streamingAssetsPath, YooAssetSettingsData.Setting.DefaultYooFolderName);
            return path;
        }

        public string GetWebFileLoadPath(PackageBundle bundle)
        {
            if (_webFilePaths.TryGetValue(bundle.BundleGUID, out var filePath) == false)
            {
                filePath = PathUtility.Combine(_webPackageRoot, bundle.FileName);
                _webFilePaths.Add(bundle.BundleGUID, filePath);
            }

            return filePath;
        }

        public string GetCatalogFileLoadPath()
        {
            var fileName = Path.GetFileNameWithoutExtension(DefaultBuildinFileSystemDefine.BuildinCatalogFileName);
            return PathUtility.Combine(YooAssetSettingsData.Setting.DefaultYooFolderName, PackageName, fileName);
        }

        public string GetWebPackageVersionFilePath()
        {
            var fileName = YooAssetSettingsData.GetPackageVersionFileName(PackageName);
            Debug.LogError(FileRoot + "    " + fileName);
            return PathUtility.Combine(FileRoot, fileName);
        }

        public string GetWebPackageHashFilePath(string packageVersion)
        {
            Debug.LogError("packageVersion   " + packageVersion);
            var fileName = YooAssetSettingsData.GetPackageHashFileName(PackageName, packageVersion);
            return PathUtility.Combine(FileRoot, fileName);
        }

        public string GetWebPackageManifestFilePath(string packageVersion)
        {
            var fileName = YooAssetSettingsData.GetManifestBinaryFileName(PackageName, packageVersion);
            return PathUtility.Combine(FileRoot, fileName);
        }

        public string GetStreamingAssetsPackageRoot()
        {
            var rootPath = PathUtility.Combine(Application.dataPath, "StreamingAssets", YooAssetSettingsData.Setting.DefaultYooFolderName);
            return PathUtility.Combine(rootPath, PackageName);
        }

        /// <summary>
        /// 记录文件信息
        /// </summary>
        public bool RecordFile(string bundleGUID, FileWrapper wrapper)
        {
            if (_wrappers.ContainsKey(bundleGUID))
            {
                YooLogger.Error($"{nameof(DefaultWebFileSystem)} has element : {bundleGUID}");
                return false;
            }

            _wrappers.Add(bundleGUID, wrapper);
            return true;
        }

        #endregion
    }
}