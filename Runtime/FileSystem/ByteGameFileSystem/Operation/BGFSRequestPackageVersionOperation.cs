#if UNITY_WEBGL && DOUYIN_MINI_GAME
using YooAsset;

[UnityEngine.Scripting.Preserve]
internal class BGFSRequestPackageVersionOperation : FSRequestPackageVersionOperation
{
    [UnityEngine.Scripting.Preserve]
    private enum ESteps
    {
        None,
        RequestPackageVersion,
        Done,
    }

    private readonly ByteGameFileSystem _fileSystem;
    private readonly int _timeout;
    private RequestByteGamePackageVersionOperation _requestWebPackageVersionOp;
    private ESteps _steps = ESteps.None;


    [UnityEngine.Scripting.Preserve]
    internal BGFSRequestPackageVersionOperation(ByteGameFileSystem fileSystem, int timeout)
    {
        _fileSystem = fileSystem;
        _timeout = timeout;
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
            if (_requestWebPackageVersionOp == null)
            {
                _requestWebPackageVersionOp = new RequestByteGamePackageVersionOperation(_fileSystem, _timeout);
                OperationSystem.StartOperation(_fileSystem.PackageName, _requestWebPackageVersionOp);
            }

            Progress = _requestWebPackageVersionOp.Progress;
            if (_requestWebPackageVersionOp.IsDone == false)
            {
                return;
            }

            if (_requestWebPackageVersionOp.Status == EOperationStatus.Succeed)
            {
                _steps = ESteps.Done;
                PackageVersion = _requestWebPackageVersionOp.PackageVersion;
                Status = EOperationStatus.Succeed;
            }
            else
            {
                _steps = ESteps.Done;
                Status = EOperationStatus.Failed;
                Error = _requestWebPackageVersionOp.Error;
            }
        }
    }
}
#endif