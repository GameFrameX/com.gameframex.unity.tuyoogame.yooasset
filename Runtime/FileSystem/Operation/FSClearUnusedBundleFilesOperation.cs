namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class FSClearUnusedBundleFilesOperation : AsyncOperationBase
    {
    }

    [UnityEngine.Scripting.Preserve]
    public sealed class FSClearUnusedBundleFilesCompleteOperation : FSClearUnusedBundleFilesOperation
    {
        [UnityEngine.Scripting.Preserve]
        public FSClearUnusedBundleFilesCompleteOperation()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            Status = EOperationStatus.Succeed;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
        }
    }
}