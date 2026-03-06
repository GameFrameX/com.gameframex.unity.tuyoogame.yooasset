using System.IO;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DownloadPackageHashOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            CheckExist,
            DownloadFile,
            Done,
        }

        private readonly DefaultCacheFileSystem _fileSystem;
        private readonly string _packageVersion;
        private readonly int _timeout;
        private UnityWebFileRequestOperation _webFileRequestOp;
        private int _requestCount = 0;
        private ESteps _steps = ESteps.None;


        [UnityEngine.Scripting.Preserve]
        internal DownloadPackageHashOperation(DefaultCacheFileSystem fileSystem, string packageVersion, int timeout)
        {
            _fileSystem = fileSystem;
            _packageVersion = packageVersion;
            _timeout = timeout;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _requestCount = WebRequestCounter.GetRequestFailedCount(_fileSystem.PackageName, nameof(DownloadPackageHashOperation));
            _steps = ESteps.CheckExist;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.CheckExist)
            {
                var filePath = _fileSystem.GetCachePackageHashFilePath(_packageVersion);
                if (File.Exists(filePath))
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.DownloadFile;
                }
            }

            if (_steps == ESteps.DownloadFile)
            {
                if (_webFileRequestOp == null)
                {
                    var savePath = _fileSystem.GetCachePackageHashFilePath(_packageVersion);
                    var fileName = YooAssetSettingsData.GetPackageHashFileName(_fileSystem.PackageName, _packageVersion);
                    var webURL = GetWebRequestURL(fileName);
                    _webFileRequestOp = new UnityWebFileRequestOperation(webURL, savePath, _timeout);
                    OperationSystem.StartOperation(_fileSystem.PackageName, _webFileRequestOp);
                }

                if (_webFileRequestOp.IsDone == false)
                {
                    return;
                }

                if (_webFileRequestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _webFileRequestOp.Error;
                    WebRequestCounter.RecordRequestFailed(_fileSystem.PackageName, nameof(DownloadPackageHashOperation));
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        private string GetWebRequestURL(string fileName)
        {
            // 轮流返回请求地址
            if (_requestCount % 2 == 0)
            {
                return _fileSystem.RemoteServices.GetRemoteMainURL(fileName, _packageVersion);
            }
            else
            {
                return _fileSystem.RemoteServices.GetRemoteFallbackURL(fileName, _packageVersion);
            }
        }
    }
}