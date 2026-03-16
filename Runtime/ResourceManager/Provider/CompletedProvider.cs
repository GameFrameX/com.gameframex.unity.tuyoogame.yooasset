namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class CompletedProvider : ProviderOperation
    {
        [UnityEngine.Scripting.Preserve]
        public CompletedProvider(ResourceManager manager, AssetInfo assetInfo) : base(manager, string.Empty, assetInfo)
        {
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            BeginLoadTimeRecord();
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public void SetCompleted(string error)
        {
            if (_steps == ESteps.None)
            {
                InvokeCompletion(error, EOperationStatus.Failed);
            }
        }
    }
}
