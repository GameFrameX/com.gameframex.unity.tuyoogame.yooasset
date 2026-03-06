using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class LoadLocalManifestOperation : AsyncOperationBase
    {
    }

    [UnityEngine.Scripting.Preserve]
    internal sealed class LoadLocalManifestImplOperation : LoadLocalManifestOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            CheckParams,
            CheckActiveManifest,
            TryLoadCacheManifest,
            LoadBuildinManifest,
            Done,
        }

        private readonly IPlayMode _impl;
        private readonly IFileSystem _buildinFileSystem;
        private readonly IFileSystem _cacheFileSystem;
        private readonly string _packageVersion;
        private readonly int _timeout;
        private FSLoadPackageManifestOperation _loadBuildinManifestOp;
        private FSLoadPackageManifestOperation _loadCacheManifestOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal LoadLocalManifestImplOperation(IPlayMode impl, IFileSystem fileSystem, IFileSystem cacheSystem, string packageVersion, int timeout)
        {
            _impl = impl;
            _buildinFileSystem = fileSystem;
            _cacheFileSystem = cacheSystem;
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
                    _steps = ESteps.TryLoadCacheManifest;
                }
            }

            if (_steps == ESteps.TryLoadCacheManifest)
            {
                if (_loadCacheManifestOp == null)
                {
                    _loadCacheManifestOp = _cacheFileSystem.LoadLocalPackageManifestAsync(_packageVersion, _timeout);
                }

                if (_loadCacheManifestOp.IsDone == false)
                {
                    return;
                }

                if (_loadCacheManifestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    _impl.ActiveManifest = _loadCacheManifestOp.Manifest;
                    Debug.Log($"TryLoadCacheManifest Succeed:{_impl.ActiveManifest.PackageName}  {_impl.ActiveManifest.PackageVersion}");
                    // SavePackageVersion();
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.LoadBuildinManifest;
                    // Status = EOperationStatus.Failed;
                    Error = _loadCacheManifestOp.Error;
                }
            }

            if (_steps == ESteps.LoadBuildinManifest)
            {
                if (_loadBuildinManifestOp == null)
                {
                    _loadBuildinManifestOp = _buildinFileSystem.RequestRemotePackageManifestAsync(_packageVersion, _timeout);
                }

                if (_loadBuildinManifestOp.IsDone == false)
                {
                    return;
                }

                if (_loadBuildinManifestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    _impl.ActiveManifest = _loadBuildinManifestOp.Manifest;
                    Debug.Log($"LoadBuildinManifest Succeed:{_impl.ActiveManifest.PackageName}  {_impl.ActiveManifest.PackageVersion}");
                    SavePackageVersion();
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadBuildinManifestOp.Error;
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public void SavePackageVersion()
        {
            if (_impl.ActiveManifest != null)
            {
                var fileName = YooAssetSettingsData.GetPackageVersionFileName(_buildinFileSystem.PackageName);
                var _manifestFileRoot = PathUtility.Combine(_cacheFileSystem.FileRoot, DefaultCacheFileSystemDefine.ManifestFilesFolderName);
                var filePath = PathUtility.Combine(_manifestFileRoot, fileName);

                //FileUtility.CreateFileDirectory(filePath);

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