using System.IO;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class LoadCachePackageVersionOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            RequestPackageVersion,
            Done,
        }

        private readonly DefaultCacheFileSystem _fileSystem;
        private UnityWebTextRequestOperation _webTextRequestOp;
        private ESteps _steps = ESteps.None;

        /// <summary>
        /// 包裹版本
        /// </summary>
        public string PackageVersion { private set; get; }


        [UnityEngine.Scripting.Preserve]
        internal LoadCachePackageVersionOperation(DefaultCacheFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.RequestPackageVersion;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.RequestPackageVersion)
            {
                var fileName = YooAssetSettingsData.GetPackageVersionFileName(_fileSystem.PackageName);
                var filePath = PathUtility.Combine(_fileSystem.FileRoot, fileName);
                if (File.Exists(filePath))
                {
                    if (_webTextRequestOp == null)
                    {
                        var url = DownloadSystemHelper.ConvertToWWWPath(filePath);
                        _webTextRequestOp = new UnityWebTextRequestOperation(url);
                        OperationSystem.StartOperation(_fileSystem.PackageName, _webTextRequestOp);
                    }

                    if (_webTextRequestOp.IsDone == false)
                    {
                        return;
                    }

                    if (_webTextRequestOp.Status == EOperationStatus.Succeed)
                    {
                        PackageVersion = _webTextRequestOp.Result;
                        Debug.Log($"LoadCachePackageVersionOperation 加载本地沙盒版本成功：{PackageVersion}");
                        if (string.IsNullOrEmpty(PackageVersion))
                        {
                            _steps = ESteps.Done;
                            Status = EOperationStatus.Failed;
                            Error = $"cache package version file content is empty !";
                        }
                        else
                        {
                            _steps = ESteps.Done;
                            Status = EOperationStatus.Succeed;
                        }
                    }
                    else
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = _webTextRequestOp.Error;
                    }
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Can not found cache version file : {filePath}";
                }
            }
        }
    }
}