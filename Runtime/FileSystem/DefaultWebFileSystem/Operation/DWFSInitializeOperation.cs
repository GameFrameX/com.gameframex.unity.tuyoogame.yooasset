namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DWFSInitializeOperation : FSInitializeFileSystemOperation
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadCatalogFile,
            Done,
        }

        private readonly DefaultWebFileSystem _fileSystem;
        private LoadWebCatalogFileOperation _loadCatalogFileOp;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        public DWFSInitializeOperation(DefaultWebFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadCatalogFile;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadCatalogFile)
            {
                if (_loadCatalogFileOp == null)
                {
#if UNITY_EDITOR
                    // 兼容性初始化
                    // 说明：内置文件系统在编辑器下运行时需要动态生成
                    var packageRoot = _fileSystem.GetStreamingAssetsPackageRoot();
                    DefaultBuildinFileSystemBuild.CreateBuildinCatalogFile(_fileSystem.PackageName, packageRoot);
#endif

                    _loadCatalogFileOp = new LoadWebCatalogFileOperation(_fileSystem);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _loadCatalogFileOp);
                }

                if (_loadCatalogFileOp.IsDone == false)
                {
                    return;
                }

                if (_loadCatalogFileOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadCatalogFileOp.Error;
                }
            }
        }
    }
}