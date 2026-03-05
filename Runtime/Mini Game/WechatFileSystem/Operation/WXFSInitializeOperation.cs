#if UNITY_WEBGL && WEIXINMINIGAME
using YooAsset;

internal partial class WXFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly WechatFileSystem _fileSystem;

    public WXFSInitializeOperation(WechatFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }
    protected override void InternalStart()
    {
        Status = EOperationStatus.Succeed;
    }
    protected override void InternalUpdate()
    {
    }
}
#endif