
namespace YooAsset
{
    public abstract class FSLoadSceneOperation : AsyncOperationBase
    {
        public UnityEngine.SceneManagement.Scene Result;

        public abstract void UnSuspendLoad();
    }
}