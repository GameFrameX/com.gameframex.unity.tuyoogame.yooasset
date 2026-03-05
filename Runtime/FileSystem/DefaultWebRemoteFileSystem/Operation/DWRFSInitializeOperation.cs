
namespace YooAsset
{
    internal class DWRFSInitializeOperation : FSInitializeFileSystemOperation
    {
        private readonly DefaultWebRemoteFileSystem _fileSystem;

        public DWRFSInitializeOperation(DefaultWebRemoteFileSystem fileSystem)
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
}