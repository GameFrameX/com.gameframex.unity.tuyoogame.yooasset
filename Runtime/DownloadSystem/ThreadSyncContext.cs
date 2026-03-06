using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;

namespace YooAsset
{
    /// <summary>
    /// 同步其它线程里的回调到主线程里
    /// 注意：Unity3D中需要设置Scripting Runtime Version为.NET4.6
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal sealed class ThreadSyncContext : SynchronizationContext
    {
        private readonly ConcurrentQueue<Action> _safeQueue = new();

        /// <summary>
        /// 更新同步队列
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void Update()
        {
            while (true)
            {
                if (_safeQueue.TryDequeue(out var action) == false)
                {
                    return;
                }

                action.Invoke();
            }
        }

        /// <summary>
        /// 向同步队列里投递一个回调方法
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public override void Post(SendOrPostCallback callback, object state)
        {
            var action = new Action(() => { callback(state); });
            _safeQueue.Enqueue(action);
        }
    }
}