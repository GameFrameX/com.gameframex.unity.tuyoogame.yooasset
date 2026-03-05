
namespace YooAsset
{
    internal class RawBundleLoadSubAssetsOperation : FSLoadSubAssetsOperation
    {
        protected override void InternalStart()
        {
            Error = $"{nameof(RawBundleLoadSubAssetsOperation)} not support load sub assets !";
            Status = EOperationStatus.Failed;
        }
        protected override void InternalUpdate()
        {
        }
    }
}