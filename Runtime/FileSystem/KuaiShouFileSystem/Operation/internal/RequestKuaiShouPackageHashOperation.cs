#if UNITY_WEBGL && ENABLE_KUAISHOU_MINI_GAME
using YooAsset;

[UnityEngine.Scripting.Preserve]
internal class RequestKuaiShouPackageHashOperation : AsyncOperationBase
{
    [UnityEngine.Scripting.Preserve]
    private enum ESteps
    {
        None,
        RequestPackageHash,
        Done,
    }

    private readonly KuaiShouFileSystem _fileSystem;
    private readonly string _packageVersion;
    private readonly int _timeout;
    private UnityWebTextRequestOperation _webTextRequestOp;
    private int _requestCount = 0;
    private ESteps _steps = ESteps.None;

    /// <summary>
    /// 包裹哈希值
    /// </summary>
    public string PackageHash { private set; get; }


    [UnityEngine.Scripting.Preserve]
    public RequestKuaiShouPackageHashOperation(KuaiShouFileSystem fileSystem, string packageVersion, int timeout)
    {
        _fileSystem = fileSystem;
        _packageVersion = packageVersion;
        _timeout = timeout;
    }
    [UnityEngine.Scripting.Preserve]
    public override void InternalOnStart()
    {
        _requestCount = WebRequestCounter.GetRequestFailedCount(_fileSystem.PackageName, nameof(RequestKuaiShouPackageHashOperation));
        _steps = ESteps.RequestPackageHash;
    }
    [UnityEngine.Scripting.Preserve]
    public override void InternalOnUpdate()
    {
        if (_steps == ESteps.None || _steps == ESteps.Done)
        {
            return;
        }

        if (_steps == ESteps.RequestPackageHash)
        {
            if (_webTextRequestOp == null)
            {
                string fileName = YooAssetSettingsData.GetPackageHashFileName(_fileSystem.PackageName, _packageVersion);
                string url = GetRequestURL(fileName);
                _webTextRequestOp = new UnityWebTextRequestOperation(url, _timeout);
                OperationSystem.StartOperation(_fileSystem.PackageName, _webTextRequestOp);
            }

            Progress = _webTextRequestOp.Progress;
            if (_webTextRequestOp.IsDone == false)
            {
                return;
            }

            if (_webTextRequestOp.Status == EOperationStatus.Succeed)
            {
                PackageHash = _webTextRequestOp.Result;
                if (string.IsNullOrEmpty(PackageHash))
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Wechat package hash file content is empty !";
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
                WebRequestCounter.RecordRequestFailed(_fileSystem.PackageName, nameof(RequestKuaiShouPackageHashOperation));
            }
        }
    }

    [UnityEngine.Scripting.Preserve]
    private string GetRequestURL(string fileName)
    {
        // 轮流返回请求地址
        if (_requestCount % 2 == 0)
        {
            return _fileSystem.RemoteServices.GetRemoteMainURL(fileName,_packageVersion);
        }
        else
        {
            return _fileSystem.RemoteServices.GetRemoteFallbackURL(fileName,_packageVersion);
        }
    }
}
#endif