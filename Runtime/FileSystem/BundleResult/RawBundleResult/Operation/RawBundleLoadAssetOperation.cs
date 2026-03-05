
namespace YooAsset
{
    internal class RawBundleLoadAssetOperation : FSLoadAssetOperation
    {
        protected override void InternalStart()
        {
            Error = $"{nameof(RawBundleLoadAssetOperation)} not support load asset !";
            Status = EOperationStatus.Failed;
        }
        protected override void InternalUpdate()
        {
        }
    }
}