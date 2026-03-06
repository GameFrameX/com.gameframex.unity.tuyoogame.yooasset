#if UNITY_WEBGL && WECHAT_MINI_GAME
using YooAsset;

[UnityEngine.Scripting.Preserve]
internal partial class WXFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly WechatFileSystem _fileSystem;

    [UnityEngine.Scripting.Preserve]
    public WXFSInitializeOperation(WechatFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }
    [UnityEngine.Scripting.Preserve]
    public override void InternalOnStart()
    {
        Status = EOperationStatus.Succeed;
    }
    [UnityEngine.Scripting.Preserve]
    public override void InternalOnUpdate()
    {
    }
}
#endif