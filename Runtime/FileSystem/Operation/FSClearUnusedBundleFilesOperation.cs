namespace YooAsset
{
    internal abstract class FSClearUnusedBundleFilesOperation : AsyncOperationBase
    {
    }

    internal sealed class FSClearUnusedBundleFilesCompleteOperation : FSClearUnusedBundleFilesOperation
    {
        internal FSClearUnusedBundleFilesCompleteOperation()
        {
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