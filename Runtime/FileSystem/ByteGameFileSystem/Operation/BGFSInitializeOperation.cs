#if UNITY_WEBGL && DOUYIN_MINI_GAME
using YooAsset;

[UnityEngine.Scripting.Preserve]
internal partial class BGFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly ByteGameFileSystem _fileSystem;

    [UnityEngine.Scripting.Preserve]
    public BGFSInitializeOperation(ByteGameFileSystem fileSystem)
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