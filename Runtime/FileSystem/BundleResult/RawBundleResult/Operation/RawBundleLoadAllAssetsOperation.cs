
namespace YooAsset
{
    internal class RawBundleLoadAllAssetsOperation : FSLoadAllAssetsOperation
    {
        protected override void InternalStart()
        {
            Error = $"{nameof(RawBundleLoadAllAssetsOperation)} not support load all assets !";
            Status = EOperationStatus.Failed;
        }
        protected override void InternalUpdate()
        {
        }
    }
}