#if UNITY_WEBGL && ENABLE_KUAISHOU_MINI_GAME
using YooAsset;

[UnityEngine.Scripting.Preserve]
internal partial class KSFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly KuaiShouFileSystem _fileSystem;

    [UnityEngine.Scripting.Preserve]
    public KSFSInitializeOperation(KuaiShouFileSystem fileSystem)
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