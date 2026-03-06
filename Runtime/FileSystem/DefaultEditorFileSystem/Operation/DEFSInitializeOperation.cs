namespace YooAsset
{
    internal class DEFSInitializeOperation : FSInitializeFileSystemOperation
    {
        private readonly DefaultEditorFileSystem _fileSytem;

        internal DEFSInitializeOperation(DefaultEditorFileSystem fileSystem)
        {
            _fileSytem = fileSystem;
        }

        public override void InternalOnStart()
        {
            Status = EOperationStatus.Succeed;
        }

        public override void InternalOnUpdate()
        {
        }
    }
}