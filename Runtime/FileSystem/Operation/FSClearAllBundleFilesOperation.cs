namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class FSClearAllBundleFilesOperation : AsyncOperationBase
    {
    }

    [UnityEngine.Scripting.Preserve]
    public sealed class FSClearAllBundleFilesCompleteOperation : FSClearAllBundleFilesOperation
    {
        [UnityEngine.Scripting.Preserve]
        public FSClearAllBundleFilesCompleteOperation()
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