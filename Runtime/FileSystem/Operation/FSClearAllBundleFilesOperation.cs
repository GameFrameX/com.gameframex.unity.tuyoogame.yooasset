namespace YooAsset
{
    internal abstract class FSClearAllBundleFilesOperation : AsyncOperationBase
    {
    }

    internal sealed class FSClearAllBundleFilesCompleteOperation : FSClearAllBundleFilesOperation
    {
        internal FSClearAllBundleFilesCompleteOperation()
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