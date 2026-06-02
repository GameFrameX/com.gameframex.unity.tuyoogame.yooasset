#if UNITY_WEBGL && ENABLE_KUAISHOU_MINI_GAME
using YooAsset;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class KSFSLoadPackageManifestOperation : FSLoadPackageManifestOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            RequestRemotePackageHash,
            LoadRemotePackageManifest,
            Done,
        }

        private readonly KuaiShouFileSystem _fileSystem;
        private readonly string _packageVersion;
        private readonly int _timeout;
        private RequestKuaiShouPackageHashOperation _requestRemotePackageHashOp;
        private LoadKuaiShouPackageManifestOperation _loadRemotePackageManifestOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        public KSFSLoadPackageManifestOperation(KuaiShouFileSystem fileSystem, string packageVersion, int timeout)
        {
            _fileSystem = fileSystem;
            _packageVersion = packageVersion;
            _timeout = timeout;
        }
        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.RequestRemotePackageHash;
        }
        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.RequestRemotePackageHash)
            {
                if (_requestRemotePackageHashOp == null)
                {
                    _requestRemotePackageHashOp = new RequestKuaiShouPackageHashOperation(_fileSystem, _packageVersion, _timeout);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _requestRemotePackageHashOp);
                }

                if (_requestRemotePackageHashOp.IsDone == false)
                {
                    return;
                }

                if (_requestRemotePackageHashOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.LoadRemotePackageManifest;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _requestRemotePackageHashOp.Error;
                }
            }

            if (_steps != ESteps.LoadRemotePackageManifest)
            {
                return;
            }

            if (_loadRemotePackageManifestOp == null)
            {
                string packageHash = _requestRemotePackageHashOp.PackageHash;
                _loadRemotePackageManifestOp = new LoadKuaiShouPackageManifestOperation(_fileSystem, _packageVersion, packageHash, _timeout);
                OperationSystem.StartOperation(_fileSystem.PackageName, _loadRemotePackageManifestOp);
            }

            Progress = _loadRemotePackageManifestOp.Progress;
            if (_loadRemotePackageManifestOp.IsDone == false)
            {
                return;
            }

            if (_loadRemotePackageManifestOp.Status == EOperationStatus.Succeed)
            {
                _steps = ESteps.Done;
                Manifest = _loadRemotePackageManifestOp.Manifest;
                Status = EOperationStatus.Succeed;
            }
            else
            {
                _steps = ESteps.Done;
                Status = EOperationStatus.Failed;
                Error = _loadRemotePackageManifestOp.Error;
            }
        }
    }
}
#endif