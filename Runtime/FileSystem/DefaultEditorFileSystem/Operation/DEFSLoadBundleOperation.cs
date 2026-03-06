namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class DEFSLoadBundleOperation : FSLoadBundleOperation
    {
        private readonly DefaultEditorFileSystem _fileSystem;
        private readonly PackageBundle _bundle;

        [UnityEngine.Scripting.Preserve]
        internal DEFSLoadBundleOperation(DefaultEditorFileSystem fileSystem, PackageBundle bundle)
        {
            _fileSystem = fileSystem;
            _bundle = bundle;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            DownloadProgress = 1f;
            DownloadedBytes = _bundle.FileSize;
            Status = EOperationStatus.Succeed;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalWaitForAsyncComplete()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public override void AbortDownloadOperation()
        {
        }
    }
}