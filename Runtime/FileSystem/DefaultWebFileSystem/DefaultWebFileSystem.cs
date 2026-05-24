using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    /// <summary>
    /// Web文件系统
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class DefaultWebFileSystem : IFileSystem
    {
        [UnityEngine.Scripting.Preserve]
        public class FileWrapper
        {
            public string FileName { private set; get; }

            [UnityEngine.Scripting.Preserve]
            public FileWrapper(string fileName)
            {
                FileName = fileName;
            }
        }

        protected readonly Dictionary<string, FileWrapper> _wrappers = new Dictionary<string, FileWrapper>(10000);
        protected readonly Dictionary<string, string> _webFilePaths = new Dictionary<string, string>(10000);
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


        [UnityEngine.Scripting.Preserve]
        public DefaultWebFileSystem()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSInitializeFileSystemOperation InitializeFileSystemAsync()
        {
            var operation = new DWFSInitializeOperation(this);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSRequestPackageVersionOperation LoadLocalPackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DWFSRequestPackageVersionOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSLoadPackageManifestOperation LoadLocalPackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DWFSLoadPackageManifestOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSLoadPackageManifestOperation RequestRemotePackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DWFSLoadPackageManifestOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSRequestPackageVersionOperation RequestRemotePackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DWFSRequestPackageVersionOperation(this, timeout);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSClearAllBundleFilesOperation ClearAllBundleFilesAsync()
        {
            var operation = new FSClearAllBundleFilesCompleteOperation();
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSClearUnusedBundleFilesOperation ClearUnusedBundleFilesAsync(PackageManifest manifest)
        {
            var operation = new FSClearUnusedBundleFilesCompleteOperation();
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSDownloadFileOperation DownloadFileAsync(PackageBundle bundle, DownloadParam param)
        {
            throw new NotImplementedException();
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSLoadBundleOperation LoadBundleFile(PackageBundle bundle)
        {
            var operation = new DWFSLoadAssetBundleOperation(this, bundle);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
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

        [UnityEngine.Scripting.Preserve]
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

        [UnityEngine.Scripting.Preserve]
        public virtual void OnCreate(string packageName, string rootDirectory)
        {
            PackageName = packageName;

            if (string.IsNullOrEmpty(rootDirectory))
            {
                rootDirectory = GetDefaultWebRoot();
            }

            _webPackageRoot = PathUtility.Combine(rootDirectory, packageName);
        }

        [UnityEngine.Scripting.Preserve]
        public virtual void OnUpdate()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public virtual bool Belong(PackageBundle bundle)
        {
            return true;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual bool Exists(PackageBundle bundle)
        {
            return true;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual bool NeedDownload(PackageBundle bundle)
        {
            return false;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual bool NeedUnpack(PackageBundle bundle)
        {
            return false;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual bool NeedImport(PackageBundle bundle)
        {
            return false;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual byte[] ReadFileData(PackageBundle bundle)
        {
            throw new NotImplementedException();
        }

        [UnityEngine.Scripting.Preserve]
        public virtual string ReadFileText(PackageBundle bundle)
        {
            throw new NotImplementedException();
        }

        #region 内部方法

        [UnityEngine.Scripting.Preserve]
        protected string GetDefaultWebRoot()
        {
            var path = PathUtility.Combine(Application.streamingAssetsPath, YooAssetSettingsData.Setting.DefaultYooFolderName);
            return path;
        }

        [UnityEngine.Scripting.Preserve]
        public string GetWebFileLoadPath(PackageBundle bundle)
        {
            if (_webFilePaths.TryGetValue(bundle.BundleGUID, out var filePath) == false)
            {
                filePath = PathUtility.Combine(_webPackageRoot, bundle.FileName);
                _webFilePaths.Add(bundle.BundleGUID, filePath);
            }

            return filePath;
        }

        [UnityEngine.Scripting.Preserve]
        public string GetCatalogFileLoadPath()
        {
            var fileName = Path.GetFileNameWithoutExtension(DefaultBuildinFileSystemDefine.BuildinCatalogFileName);
            return PathUtility.Combine(YooAssetSettingsData.Setting.DefaultYooFolderName, PackageName, fileName);
        }

        [UnityEngine.Scripting.Preserve]
        public string GetWebPackageVersionFilePath()
        {
            var fileName = YooAssetSettingsData.GetPackageVersionFileName(PackageName);
            Debug.LogError(FileRoot + "    " + fileName);
            return PathUtility.Combine(FileRoot, fileName);
        }

        [UnityEngine.Scripting.Preserve]
        public string GetWebPackageHashFilePath(string packageVersion)
        {
            Debug.LogError("packageVersion   " + packageVersion);
            var fileName = YooAssetSettingsData.GetPackageHashFileName(PackageName, packageVersion);
            return PathUtility.Combine(FileRoot, fileName);
        }

        [UnityEngine.Scripting.Preserve]
        public string GetWebPackageManifestFilePath(string packageVersion)
        {
            var fileName = YooAssetSettingsData.GetManifestBinaryFileName(PackageName, packageVersion);
            return PathUtility.Combine(FileRoot, fileName);
        }

        [UnityEngine.Scripting.Preserve]
        public string GetStreamingAssetsPackageRoot()
        {
            var rootPath = PathUtility.Combine(Application.dataPath, "StreamingAssets", YooAssetSettingsData.Setting.DefaultYooFolderName);
            return PathUtility.Combine(rootPath, PackageName);
        }

        /// <summary>
        /// 记录文件信息
        /// </summary>
        [UnityEngine.Scripting.Preserve]
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