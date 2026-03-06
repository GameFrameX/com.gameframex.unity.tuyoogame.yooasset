namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal abstract class FSClearUnusedBundleFilesOperation : AsyncOperationBase
    {
    }

    [UnityEngine.Scripting.Preserve]
    internal sealed class FSClearUnusedBundleFilesCompleteOperation : FSClearUnusedBundleFilesOperation
    {
        [UnityEngine.Scripting.Preserve]
        internal FSClearUnusedBundleFilesCompleteOperation()
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