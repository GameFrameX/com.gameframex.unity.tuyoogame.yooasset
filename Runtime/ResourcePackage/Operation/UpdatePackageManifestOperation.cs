using System.IO;
using System.Text;
using UnityEngine;

namespace YooAsset
{
    /// <summary>
    /// 向远端请求并更新清单
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public abstract class UpdatePackageManifestOperation : AsyncOperationBase
    {
    }

    [UnityEngine.Scripting.Preserve]
    internal sealed class UpdatePackageManifestImplOperation : UpdatePackageManifestOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            CheckParams,
            CheckActiveManifest,
            LoadPackageManifest,
            Done,
        }

        private readonly IPlayMode _impl;
        private readonly IFileSystem _fileSystem;
        private readonly string _packageVersion;
        private readonly int _timeout;
        private FSLoadPackageManifestOperation _loadPackageManifestOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal UpdatePackageManifestImplOperation(IPlayMode impl, IFileSystem fileSystem, string packageVersion, int timeout)
        {
            _impl = impl;
            _fileSystem = fileSystem;
            _packageVersion = packageVersion;
            _timeout = timeout;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.CheckParams;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.CheckParams)
            {
                if (string.IsNullOrEmpty(_packageVersion))
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = "Package version is null or empty.";
                }
                else
                {
                    _steps = ESteps.CheckActiveManifest;
                }
            }

            if (_steps == ESteps.CheckActiveManifest)
            {
                // 检测当前激活的清单对象	
                if (_impl.ActiveManifest != null && _impl.ActiveManifest.PackageVersion == _packageVersion)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.LoadPackageManifest;
                }
            }

            if (_steps == ESteps.LoadPackageManifest)
            {
                if (_loadPackageManifestOp == null)
                {
                    _loadPackageManifestOp = _fileSystem.RequestRemotePackageManifestAsync(_packageVersion, _timeout);
                }

                if (_loadPackageManifestOp.IsDone == false)
                {
                    return;
                }

                if (_loadPackageManifestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    _impl.ActiveManifest = _loadPackageManifestOp.Manifest;
                    Debug.Log($"LoadPackageManifest Succeed:{_impl.ActiveManifest.PackageName}  {_impl.ActiveManifest.PackageVersion}");
                    SavePackageVersion();
                    DefaultCacheFileSystemDefine.PackageVersion = _packageVersion;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadPackageManifestOp.Error;
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void SavePackageVersion()
        {
            if (_impl.ActiveManifest != null)
            {
                var fileName = YooAssetSettingsData.GetPackageVersionFileName(_fileSystem.PackageName);
                var _manifestFileRoot = PathUtility.Combine(_fileSystem.FileRoot, DefaultCacheFileSystemDefine.ManifestFilesFolderName);
                var filePath = Path.Combine(_manifestFileRoot, fileName);

                //if (!File.Exists(filePath))
                //    File.Create(filePath).Close();

                //FileInfo fi = new FileInfo(filePath);
                //FileStream fs = fi.OpenWrite();
                //byte[] bytes = Encoding.UTF8.GetBytes(_packageVersion);
                //fs.Write(bytes, 0, bytes.Length);
                //fs.Flush();
                //fs.Close();
                //fs.Dispose();
                FileUtility.WriteAllText(filePath, _packageVersion);
                Debug.LogWarning("保存沙盒版本文件" + _packageVersion);
            }
        }
    }
}