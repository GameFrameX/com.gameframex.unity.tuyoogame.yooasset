namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class GameAsyncOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            OnStart();
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            OnUpdate();
        }

        [UnityEngine.Scripting.Preserve]
        internal override void InternalOnAbort()
        {
            OnAbort();
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalWaitForAsyncComplete()
        {
            OnWaitForAsyncComplete();
        }

        /// <summary>
        /// 异步操作开始
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected abstract void OnStart();

        /// <summary>
        /// 异步操作更新
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected abstract void OnUpdate();

        /// <summary>
        /// 异步操作终止
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected abstract void OnAbort();

        /// <summary>
        /// 异步等待完成
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected virtual void OnWaitForAsyncComplete()
        {
        }

        /// <summary>
        /// 异步操作系统是否繁忙
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected bool IsBusy()
        {
            return OperationSystem.IsBusy;
        }
    }
}