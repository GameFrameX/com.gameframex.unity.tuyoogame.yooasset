
namespace YooAsset
{
    public abstract class FSClearAllBundleFilesOperation : AsyncOperationBase
    {
    }

    public sealed class FSClearAllBundleFilesCompleteOperation : FSClearAllBundleFilesOperation
    {
        public FSClearAllBundleFilesCompleteOperation()
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