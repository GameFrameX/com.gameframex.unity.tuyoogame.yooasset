#if UNITY_WEBGL && DOUYINMINIGAME
using YooAsset;

internal partial class TTFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly TiktokFileSystem _fileSystem;

    public TTFSInitializeOperation(TiktokFileSystem fileSystem)
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