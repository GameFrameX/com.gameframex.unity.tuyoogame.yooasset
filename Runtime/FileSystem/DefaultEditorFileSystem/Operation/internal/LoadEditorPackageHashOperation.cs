using System.IO;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class LoadEditorPackageHashOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadHash,
            Done,
        }

        private readonly DefaultEditorFileSystem _fileSystem;
        private readonly string _packageVersion;
        private ESteps _steps = ESteps.None;

        /// <summary>
        /// 包裹哈希值
        /// </summary>
        public string PackageHash { private set; get; }


        [UnityEngine.Scripting.Preserve]
        internal LoadEditorPackageHashOperation(DefaultEditorFileSystem fileSystem, string packageVersion)
        {
            _fileSystem = fileSystem;
            _packageVersion = packageVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadHash;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadHash)
            {
                var hashFilePath = _fileSystem.GetEditorPackageHashFilePath(_packageVersion);
                if (File.Exists(hashFilePath))
                {
                    _steps = ESteps.Done;
                    PackageHash = FileUtility.ReadAllText(hashFilePath);
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Can not found simulation package hash file : {hashFilePath}";
                }
            }
        }
    }
}