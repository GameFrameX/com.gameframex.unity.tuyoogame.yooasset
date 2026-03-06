namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal abstract class FSClearAllBundleFilesOperation : AsyncOperationBase
    {
    }

    [UnityEngine.Scripting.Preserve]
    internal sealed class FSClearAllBundleFilesCompleteOperation : FSClearAllBundleFilesOperation
    {
        [UnityEngine.Scripting.Preserve]
        internal FSClearAllBundleFilesCompleteOperation()
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