
namespace YooAsset
{
    public abstract class FSClearUnusedBundleFilesOperation : AsyncOperationBase
    {
    }

    public sealed class FSClearUnusedBundleFilesCompleteOperation : FSClearUnusedBundleFilesOperation
    {
        public FSClearUnusedBundleFilesCompleteOperation()
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