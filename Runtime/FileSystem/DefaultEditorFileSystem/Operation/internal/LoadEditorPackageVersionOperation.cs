using System.IO;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class LoadEditorPackageVersionOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadVersion,
            Done,
        }

        private readonly DefaultEditorFileSystem _fileSystem;
        private ESteps _steps = ESteps.None;

        /// <summary>
        /// 包裹版本
        /// </summary>
        public string PackageVersion { private set; get; }


        [UnityEngine.Scripting.Preserve]
        internal LoadEditorPackageVersionOperation(DefaultEditorFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadVersion)
            {
                var versionFilePath = _fileSystem.GetEditorPackageVersionFilePath();
                if (File.Exists(versionFilePath))
                {
                    _steps = ESteps.Done;
                    PackageVersion = FileUtility.ReadAllText(versionFilePath);
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Can not found simulation package version file : {versionFilePath}";
                }
            }
        }
    }
}