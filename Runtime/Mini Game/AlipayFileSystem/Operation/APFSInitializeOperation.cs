#if UNITY_WEBGL && UNITY_ALIMINIGAME
using YooAsset;

internal partial class APFSInitializeOperation : FSInitializeFileSystemOperation
{
    private readonly AlipayFileSystem _fileSystem;

    public APFSInitializeOperation(AlipayFileSystem fileSystem)
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