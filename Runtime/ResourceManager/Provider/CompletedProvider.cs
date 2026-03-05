
namespace YooAsset
{
    internal sealed class CompletedProvider : ProviderOperation
    {
        public CompletedProvider(ResourceManager manager, AssetInfo assetInfo) : base(manager, string.Empty, assetInfo)
        {
        }

        public override void InternalOnStart()
        {
        }
        public override void InternalOnUpdate()
        {
        }

        public void SetCompleted(string error)
        {
            if (_steps == ESteps.None)
            {
                InvokeCompletion(error, EOperationStatus.Failed);
            }
        }
    }
}