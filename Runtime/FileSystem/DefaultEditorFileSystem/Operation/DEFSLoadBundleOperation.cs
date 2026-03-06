namespace YooAsset
{
    internal class DEFSLoadBundleOperation : FSLoadBundleOperation
    {
        private readonly DefaultEditorFileSystem _fileSystem;
        private readonly PackageBundle _bundle;

        internal DEFSLoadBundleOperation(DefaultEditorFileSystem fileSystem, PackageBundle bundle)
        {
            _fileSystem = fileSystem;
            _bundle = bundle;
        }

        public override void InternalOnStart()
        {
            DownloadProgress = 1f;
            DownloadedBytes = _bundle.FileSize;
            Status = EOperationStatus.Succeed;
        }

        public override void InternalOnUpdate()
        {
        }

        public override void InternalWaitForAsyncComplete()
        {
        }

        public override void AbortDownloadOperation()
        {
        }
    }
}