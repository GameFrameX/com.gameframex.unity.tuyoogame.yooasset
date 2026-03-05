
namespace YooAsset
{
    internal class DEFSInitializeOperation : FSInitializeFileSystemOperation
    {
        private readonly DefaultEditorFileSystem _fileSytem;

        internal DEFSInitializeOperation(DefaultEditorFileSystem fileSystem)
        {
            _fileSytem = fileSystem;
        }
        protected override void InternalStart()
        {
            Status = EOperationStatus.Succeed;
        }
        protected override void InternalUpdate()
        {
        }
    }
}