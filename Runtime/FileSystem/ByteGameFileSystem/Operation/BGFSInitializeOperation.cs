#if UNITY_WEBGL && DOUYIN_MINI_GAME
using YooAsset;

internal partial class BGFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly ByteGameFileSystem _fileSystem;

    public BGFSInitializeOperation(ByteGameFileSystem fileSystem)
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