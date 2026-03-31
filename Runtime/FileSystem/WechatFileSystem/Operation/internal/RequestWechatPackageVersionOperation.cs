#if UNITY_WEBGL && ENABLE_WECHAT_MINI_GAME
using System;
using YooAsset;

[UnityEngine.Scripting.Preserve]
internal class RequestWechatPackageVersionOperation : AsyncOperationBase
{
    [UnityEngine.Scripting.Preserve]
    private enum ESteps
    {
        None,
        RequestPackageVersion,
        Done,
    }

    private readonly WechatFileSystem _fileSystem;
    private readonly bool _appendTimeTicks;
    private readonly int _timeout;
    private UnityWebTextRequestOperation _webTextRequestOp;
    private int _requestCount = 0;
    private ESteps _steps = ESteps.None;

    /// <summary>
    /// 包裹版本
    /// </summary>
    public string PackageVersion { private set; get; }


    [UnityEngine.Scripting.Preserve]
    public RequestWechatPackageVersionOperation(WechatFileSystem fileSystem, bool appendTimeTicks, int timeout)
    {
        _fileSystem = fileSystem;
        _appendTimeTicks = appendTimeTicks;
        _timeout = timeout;
    }

    [UnityEngine.Scripting.Preserve]
    public override void InternalOnStart()
    {
        _requestCount = WebRequestCounter.GetRequestFailedCount(_fileSystem.PackageName, nameof(RequestWechatPackageVersionOperation));
        _steps = ESteps.RequestPackageVersion;
    }

    [UnityEngine.Scripting.Preserve]
    public override void InternalOnUpdate()
    {
        if (_steps == ESteps.None || _steps == ESteps.Done)
            return;

        if (_steps == ESteps.RequestPackageVersion)
        {
            if (_webTextRequestOp == null)
            {
                string fileName = YooAssetSettingsData.GetPackageVersionFileName(_fileSystem.PackageName);
                string url = GetRequestURL(fileName);
                if (_appendTimeTicks)
                {
                    url += $"?time_ticks={DateTime.Now.Ticks}";
                }

                _webTextRequestOp = new UnityWebTextRequestOperation(url, _timeout);
                OperationSystem.StartOperation(_fileSystem.PackageName, _webTextRequestOp);
            }

            Progress = _webTextRequestOp.Progress;
            if (_webTextRequestOp.IsDone == false)
                return;

            if (_webTextRequestOp.Status == EOperationStatus.Succeed)
            {
                PackageVersion = _webTextRequestOp.Result;
                if (string.IsNullOrEmpty(PackageVersion))
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Wechat package version file content is empty !";
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
                WebRequestCounter.RecordRequestFailed(_fileSystem.PackageName, nameof(RequestWechatPackageVersionOperation));
            }
        }
    }

    [UnityEngine.Scripting.Preserve]
    private string GetRequestURL(string fileName)
    {
        // 轮流返回请求地址
        if (_requestCount % 2 == 0)
            return _fileSystem.RemoteServices.GetRemoteMainURL(fileName, "");
        else
            return _fileSystem.RemoteServices.GetRemoteFallbackURL(fileName, "");
    }
}
#endif