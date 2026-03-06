using System;

namespace YooAsset
{
    /// <summary>
    /// 模拟文件系统
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal class DefaultEditorFileSystem : IFileSystem
    {
        protected string _packageRoot;

        /// <summary>
        /// 包裹名称
        /// </summary>
        public string PackageName { private set; get; }

        /// <summary>
        /// 文件根目录
        /// </summary>
        public string FileRoot
        {
            get { return _packageRoot; }
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount
        {
            get { return 0; }
        }


        [UnityEngine.Scripting.Preserve]
        public DefaultEditorFileSystem()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSInitializeFileSystemOperation InitializeFileSystemAsync()
        {
            var operation = new DEFSInitializeOperation(this);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSRequestPackageVersionOperation LoadLocalPackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DEFSRequestPackageVersionOperation(this);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSLoadPackageManifestOperation LoadLocalPackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DEFSLoadPackageManifestOperation(this, packageVersion);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSLoadPackageManifestOperation RequestRemotePackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DEFSLoadPackageManifestOperation(this, packageVersion);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual FSRequestPackageVersionOperation RequestRemotePackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DEFSRequestPackageVersionOperation(this);
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
            var operation = new DEFSLoadBundleOperation(this, bundle);
            OperationSystem.StartOperation(PackageName, operation);
            return operation;
        }

        [UnityEngine.Scripting.Preserve]
        public virtual void UnloadBundleFile(PackageBundle bundle, object result)
        {
        }

        [UnityEngine.Scripting.Preserve]
        public virtual void SetParameter(string name, object value)
        {
            YooLogger.Warning($"Invalid parameter : {name}");
        }

        [UnityEngine.Scripting.Preserve]
        public virtual void OnCreate(string packageName, string rootDirectory)
        {
            PackageName = packageName;

            if (string.IsNullOrEmpty(rootDirectory))
            {
                throw new Exception($"{nameof(DefaultEditorFileSystem)} root directory is null or empty !");
            }

            // 注意：基础目录即为包裹目录
            _packageRoot = rootDirectory;
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
        public string GetEditorPackageVersionFilePath()
        {
            var fileName = YooAssetSettingsData.GetPackageVersionFileName(PackageName);
            return PathUtility.Combine(FileRoot, fileName);
        }

        [UnityEngine.Scripting.Preserve]
        public string GetEditorPackageHashFilePath(string packageVersion)
        {
            var fileName = YooAssetSettingsData.GetPackageHashFileName(PackageName, packageVersion);
            return PathUtility.Combine(FileRoot, fileName);
        }

        [UnityEngine.Scripting.Preserve]
        public string GetEditorPackageManifestFilePath(string packageVersion)
        {
            var fileName = YooAssetSettingsData.GetManifestBinaryFileName(PackageName, packageVersion);
            return PathUtility.Combine(FileRoot, fileName);
        }

        #endregion
    }
}