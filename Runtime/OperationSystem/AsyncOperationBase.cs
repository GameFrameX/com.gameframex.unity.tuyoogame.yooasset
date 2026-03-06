using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class AsyncOperationBase : IEnumerator, IComparable<AsyncOperationBase>
    {
        private Action<AsyncOperationBase> _callback;
        private string _packageName = null;
        private int _whileFrame = 1000;

        /// <summary>
        /// 是否已经完成
        /// </summary>
        internal bool IsFinish = false;

        /// <summary>
        /// 优先级
        /// </summary>
        public uint Priority { set; get; } = 0;

        /// <summary>
        /// 状态
        /// </summary>
        public EOperationStatus Status { get; protected set; } = EOperationStatus.None;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; protected set; }

        /// <summary>
        /// 处理进度
        /// </summary>
        public float Progress { get; protected set; }

        /// <summary>
        /// 是否已经完成
        /// </summary>
        public bool IsDone
        {
            get { return Status == EOperationStatus.Failed || Status == EOperationStatus.Succeed; }
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        public event Action<AsyncOperationBase> Completed
        {
            add
            {
                if (IsDone)
                {
                    value.Invoke(this);
                }
                else
                {
                    _callback += value;
                }
            }
            remove { _callback -= value; }
        }

        /// <summary>
        /// 异步操作任务
        /// </summary>
        public Task Task
        {
            get
            {
                if (_taskCompletionSource == null)
                {
                    _taskCompletionSource = new TaskCompletionSource<object>();
                    if (IsDone)
                    {
                        _taskCompletionSource.SetResult(null);
                    }
                }

                return _taskCompletionSource.Task;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public abstract void InternalOnStart();
        [UnityEngine.Scripting.Preserve]
        public abstract void InternalOnUpdate();

        [UnityEngine.Scripting.Preserve]
        internal virtual void InternalOnAbort()
        {
        }

        [UnityEngine.Scripting.Preserve]
        public virtual void InternalWaitForAsyncComplete()
        {
            throw new NotImplementedException(GetType().Name);
        }

        [UnityEngine.Scripting.Preserve]
        internal string GetPackageName()
        {
            return _packageName;
        }

        [UnityEngine.Scripting.Preserve]
        internal void SetPackageName(string packageName)
        {
            _packageName = packageName;
        }

        [UnityEngine.Scripting.Preserve]
        internal void SetStart()
        {
            Status = EOperationStatus.Processing;
            InternalOnStart();
        }

        [UnityEngine.Scripting.Preserve]
        internal void SetFinish()
        {
            IsFinish = true;

            // 进度百分百完成
            Progress = 1f;

            //注意：如果完成回调内发生异常，会导致Task无限期等待
            _callback?.Invoke(this);

            if (_taskCompletionSource != null)
            {
                _taskCompletionSource.TrySetResult(null);
            }
        }

        [UnityEngine.Scripting.Preserve]
        internal void SetAbort()
        {
            if (IsDone == false)
            {
                Status = EOperationStatus.Failed;
                Error = "user abort";
                YooLogger.Warning($"Async operaiton {GetType().Name} has been abort !");
                InternalOnAbort();
            }
        }

        /// <summary>
        /// 执行While循环
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected bool ExecuteWhileDone()
        {
            if (IsDone == false)
            {
                // 执行更新逻辑
                InternalOnUpdate();

                // 当执行次数用完时
                _whileFrame--;
                if (_whileFrame == 0)
                {
                    Status = EOperationStatus.Failed;
                    Error = $"Operation {GetType().Name} failed to wait for async complete !";
                    YooLogger.Error(Error);
                }
            }

            return IsDone;
        }

        /// <summary>
        /// 清空完成回调
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        protected void ClearCompletedCallback()
        {
            _callback = null;
        }

        /// <summary>
        /// 等待异步执行完毕
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void WaitForAsyncComplete()
        {
            if (IsDone)
            {
                return;
            }

            InternalWaitForAsyncComplete();
        }

        #region 排序接口实现

        [UnityEngine.Scripting.Preserve]
        public int CompareTo(AsyncOperationBase other)
        {
            return other.Priority.CompareTo(Priority);
        }

        #endregion

        #region 异步编程相关

        [UnityEngine.Scripting.Preserve]
        bool IEnumerator.MoveNext()
        {
            return !IsDone;
        }

        [UnityEngine.Scripting.Preserve]
        void IEnumerator.Reset()
        {
        }

        object IEnumerator.Current
        {
            get { return null; }
        }

        private TaskCompletionSource<object> _taskCompletionSource;

        #endregion
    }
}