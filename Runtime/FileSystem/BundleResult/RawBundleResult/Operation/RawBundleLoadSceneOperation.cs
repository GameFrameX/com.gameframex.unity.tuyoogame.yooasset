
namespace YooAsset
{
    internal class RawBundleLoadSceneOperation : FSLoadSceneOperation
    {
        protected override void InternalStart()
        {
            Error = $"{nameof(RawBundleLoadSceneOperation)} not support load scene !";
            Status = EOperationStatus.Failed;
        }
        protected override void InternalUpdate()
        {
        }
        public override void UnSuspendLoad()
        {
        }
    }
}