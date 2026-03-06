using System.IO;
using UnityEngine;

namespace YooAsset
{
    /// <summary>
    /// 加载AssetBundle文件
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal class DBFSLoadAssetBundleOperation : FSLoadBundleOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadAssetBundle,
            CheckResult,
            Done,
        }

        private readonly DefaultBuildinFileSystem _fileSystem;
        private readonly PackageBundle _bundle;
        private AssetBundleCreateRequest _createRequest;
        private bool _isWaitForAsyncComplete = false;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DBFSLoadAssetBundleOperation(DefaultBuildinFileSystem fileSystem, PackageBundle bundle)
        {
            _fileSystem = fileSystem;
            _bundle = bundle;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            DownloadProgress = 1f;
            DownloadedBytes = _bundle.FileSize;
            _steps = ESteps.LoadAssetBundle;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadAssetBundle)
            {
                if (_bundle.Encrypted)
                {
                    if (_fileSystem.DecryptionServices == null)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = $"The {nameof(IDecryptionServices)} is null !";
                        YooLogger.Error(Error);
                        return;
                    }
                }

                if (_isWaitForAsyncComplete)
                {
                    if (_bundle.Encrypted)
                    {
                        Result = _fileSystem.LoadEncryptedAssetBundle(_bundle);
                    }
                    else
                    {
                        var filePath = _fileSystem.GetBuildinFileLoadPath(_bundle);
                        Result = AssetBundle.LoadFromFile(filePath);
                    }
                }
                else
                {
                    if (_bundle.Encrypted)
                    {
                        _createRequest = _fileSystem.LoadEncryptedAssetBundleAsync(_bundle);
                    }
                    else
                    {
                        var filePath = _fileSystem.GetBuildinFileLoadPath(_bundle);
                        _createRequest = AssetBundle.LoadFromFileAsync(filePath);
                    }
                }

                _steps = ESteps.CheckResult;
            }

            if (_steps == ESteps.CheckResult)
            {
                if (_createRequest != null)
                {
                    if (_isWaitForAsyncComplete)
                    {
                        // 强制挂起主线程（注意：该操作会很耗时）
                        YooLogger.Warning("Suspend the main thread to load unity bundle.");
                        Result = _createRequest.assetBundle;
                    }
                    else
                    {
                        if (_createRequest.isDone == false)
                        {
                            return;
                        }

                        Result = _createRequest.assetBundle;
                    }
                }

                if (Result != null)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                    return;
                }

                if (_bundle.Encrypted)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Failed to load encrypted buildin asset bundle file : {_bundle.BundleName}";
                    YooLogger.Error(Error);
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Failed to load buildin asset bundle file : {_bundle.BundleName}";
                    YooLogger.Error(Error);
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalWaitForAsyncComplete()
        {
            _isWaitForAsyncComplete = true;

            while (true)
            {
                if (ExecuteWhileDone())
                {
                    _steps = ESteps.Done;
                    break;
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public override void AbortDownloadOperation()
        {
        }
    }

    /// <summary>
    /// 加载原生文件
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal class DBFSLoadRawBundleOperation : FSLoadBundleOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadBuildinRawBundle,
            Done,
        }

        private readonly DefaultBuildinFileSystem _fileSystem;
        private readonly PackageBundle _bundle;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DBFSLoadRawBundleOperation(DefaultBuildinFileSystem fileSystem, PackageBundle bundle)
        {
            _fileSystem = fileSystem;
            _bundle = bundle;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            DownloadProgress = 1f;
            DownloadedBytes = _bundle.FileSize;
            _steps = ESteps.LoadBuildinRawBundle;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadBuildinRawBundle)
            {
                var filePath = _fileSystem.GetBuildinFileLoadPath(_bundle);
                if (File.Exists(filePath))
                {
                    _steps = ESteps.Done;
                    Result = new RawBundle(_fileSystem, _bundle, filePath);
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Can not found buildin raw bundle file : {filePath}";
                    YooLogger.Error(Error);
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalWaitForAsyncComplete()
        {
            while (true)
            {
                if (ExecuteWhileDone())
                {
                    _steps = ESteps.Done;
                    break;
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public override void AbortDownloadOperation()
        {
        }
    }
}