using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class LoadBuildinCatalogFileOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadCatalog,
            Done,
        }

        private readonly DefaultBuildinFileSystem _fileSystem;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal LoadBuildinCatalogFileOperation(DefaultBuildinFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadCatalog;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadCatalog)
            {
                var catalogFilePath = _fileSystem.GetBuildinCatalogFileLoadPath();
                var catalog = Resources.Load<DefaultBuildinFileCatalog>(catalogFilePath);
                if (catalog == null)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Failed to load catalog file : {catalogFilePath}";
                    return;
                }

                if (catalog.PackageName != _fileSystem.PackageName)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Catalog file package name {catalog.PackageName} cannot match the file system package name {_fileSystem.PackageName}";
                    return;
                }

                foreach (var wrapper in catalog.Wrappers)
                {
                    var fileWrapper = new DefaultBuildinFileSystem.FileWrapper(wrapper.FileName);
                    _fileSystem.RecordFile(wrapper.BundleGUID, fileWrapper);
                }

                YooLogger.Log($"Package '{_fileSystem.PackageName}' buildin catalog files count : {catalog.Wrappers.Count}");
                _steps = ESteps.Done;
                Status = EOperationStatus.Succeed;
            }
        }
    }
}