namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DWFSLoadPackageManifestOperation : FSLoadPackageManifestOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            RequestWebPackageVersion,
            RequestWebPackageHash,
            LoadWebPackageManifest,
            Done,
        }

        private readonly DefaultWebFileSystem _fileSystem;
        private readonly int _timeout;
        private RequestWebPackageVersionOperation _requestWebPackageVersionOp;
        private RequestWebPackageHashOperation _requestWebPackageHashOp;
        private LoadWebPackageManifestOperation _loadWebPackageManifestOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        public DWFSLoadPackageManifestOperation(DefaultWebFileSystem fileSystem, int timeout)
        {
            _fileSystem = fileSystem;
            _timeout = timeout;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.RequestWebPackageVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.RequestWebPackageVersion)
            {
                if (_requestWebPackageVersionOp == null)
                {
                    _requestWebPackageVersionOp = new RequestWebPackageVersionOperation(_fileSystem, _timeout, false);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _requestWebPackageVersionOp);
                }

                if (_requestWebPackageVersionOp.IsDone == false)
                {
                    return;
                }

                if (_requestWebPackageVersionOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.RequestWebPackageHash;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _requestWebPackageVersionOp.Error;
                }
            }

            if (_steps == ESteps.RequestWebPackageHash)
            {
                if (_requestWebPackageHashOp == null)
                {
                    var packageVersion = _requestWebPackageVersionOp.PackageVersion;
                    _requestWebPackageHashOp = new RequestWebPackageHashOperation(_fileSystem, packageVersion, _timeout, false);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _requestWebPackageHashOp);
                }

                if (_requestWebPackageHashOp.IsDone == false)
                {
                    return;
                }

                if (_requestWebPackageHashOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.LoadWebPackageManifest;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _requestWebPackageHashOp.Error;
                }
            }

            if (_steps == ESteps.LoadWebPackageManifest)
            {
                if (_loadWebPackageManifestOp == null)
                {
                    var packageVersion = _requestWebPackageVersionOp.PackageVersion;
                    var packageHash = _requestWebPackageHashOp.PackageHash;
                    _loadWebPackageManifestOp = new LoadWebPackageManifestOperation(_fileSystem, packageVersion, packageHash);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _loadWebPackageManifestOp);
                }

                Progress = _loadWebPackageManifestOp.Progress;
                if (_loadWebPackageManifestOp.IsDone == false)
                {
                    return;
                }

                if (_loadWebPackageManifestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    Manifest = _loadWebPackageManifestOp.Manifest;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadWebPackageManifestOp.Error;
                }
            }
        }
    }
}