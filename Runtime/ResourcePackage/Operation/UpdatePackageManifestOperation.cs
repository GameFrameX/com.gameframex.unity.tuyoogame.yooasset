using System;
using System.IO;
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

        public void SavePackageVersion()
        {
            if (_impl.ActiveManifest == null)
                return;

            // 版本记录统一写入沙盒包根目录（与 LoadCachePackageVersionOperation 的读取路径一致，
            // 也与包内版本文件的目录约定相同）。不能写 _fileSystem.FileRoot：
            // 离线模式的内置目录在移动端位于 APK 内（只读），Web 模式的根目录也不是可写路径。
            var fileName = YooAssetSettingsData.GetPackageVersionFileName(_fileSystem.PackageName);
            var recordFileSystem = new DefaultCacheFileSystem();
            recordFileSystem.OnCreate(_fileSystem.PackageName, null);
            var filePath = PathUtility.Combine(recordFileSystem.FileRoot, fileName);

            try
            {
                FileUtility.WriteAllText(filePath, _packageVersion);
                Debug.LogWarning("保存沙盒版本文件" + _packageVersion);
            }
            catch (Exception e)
            {
                // 版本记录仅用于诊断与回退，写入失败不允许中断补丁流程
                Debug.LogWarning($"保存沙盒版本文件失败:{filePath} {e.Message}");
            }
        }
    }
}