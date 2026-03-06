namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DEFSLoadPackageManifestOperation : FSLoadPackageManifestOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadEditorPackageHash,
            LoadEditorPackageManifest,
            Done,
        }

        private readonly DefaultEditorFileSystem _fileSystem;
        private readonly string _packageVersion;
        private LoadEditorPackageHashOperation _loadEditorPackageHashOpe;
        private LoadEditorPackageManifestOperation _loadEditorPackageManifestOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DEFSLoadPackageManifestOperation(DefaultEditorFileSystem fileSystem, string packageVersion)
        {
            _fileSystem = fileSystem;
            _packageVersion = packageVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadEditorPackageHash;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadEditorPackageHash)
            {
                if (_loadEditorPackageHashOpe == null)
                {
                    _loadEditorPackageHashOpe = new LoadEditorPackageHashOperation(_fileSystem, _packageVersion);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _loadEditorPackageHashOpe);
                }

                if (_loadEditorPackageHashOpe.IsDone == false)
                {
                    return;
                }

                if (_loadEditorPackageHashOpe.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.LoadEditorPackageManifest;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadEditorPackageHashOpe.Error;
                }
            }

            if (_steps == ESteps.LoadEditorPackageManifest)
            {
                if (_loadEditorPackageManifestOp == null)
                {
                    var packageHash = _loadEditorPackageHashOpe.PackageHash;
                    _loadEditorPackageManifestOp = new LoadEditorPackageManifestOperation(_fileSystem, _packageVersion, packageHash);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _loadEditorPackageManifestOp);
                }

                Progress = _loadEditorPackageManifestOp.Progress;
                if (_loadEditorPackageManifestOp.IsDone == false)
                {
                    return;
                }

                if (_loadEditorPackageManifestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    Manifest = _loadEditorPackageManifestOp.Manifest;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadEditorPackageManifestOp.Error;
                }
            }
        }
    }
}