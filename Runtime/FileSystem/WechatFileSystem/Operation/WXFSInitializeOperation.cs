#if UNITY_WEBGL && WECHAT_MINI_GAME
using YooAsset;

internal partial class WXFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly WechatFileSystem _fileSystem;

    public WXFSInitializeOperation(WechatFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }
    public override void InternalOnStart()
    {
        Status = EOperationStatus.Succeed;
    }
    public override void InternalOnUpdate()
    {
    }
}
#endif