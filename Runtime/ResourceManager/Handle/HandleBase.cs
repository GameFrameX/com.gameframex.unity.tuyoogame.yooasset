using System;
using System.Collections;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class HandleBase : IEnumerator
    {
        private readonly AssetInfo _assetInfo;
        internal ProviderOperation Provider { private set; get; }

        [UnityEngine.Scripting.Preserve]
        internal HandleBase(ProviderOperation provider)
        {
            Provider = provider;
            _assetInfo = provider.MainAssetInfo;
        }

        [UnityEngine.Scripting.Preserve]
        internal abstract void InvokeCallback();

        /// <summary>
        /// 获取资源信息
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public AssetInfo GetAssetInfo()
        {
            return _assetInfo;
        }

        /// <summary>
        /// 获取下载报告
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public DownloadStatus GetDownloadStatus()
        {
            if (IsValidWithWarning == false)
            {
                return DownloadStatus.CreateDefaultStatus();
            }

            return Provider.GetDownloadStatus();
        }

        /// <summary>
        /// 当前状态
        /// </summary>
        public EOperationStatus Status
        {
            get
            {
                if (IsValidWithWarning == false)
                {
                    return EOperationStatus.None;
                }

                return Provider.Status;
            }
        }

        /// <summary>
        /// 最近的错误信息
        /// </summary>
        public string LastError
        {
            get
            {
                if (IsValidWithWarning == false)
                {
                    return string.Empty;
                }

                return Provider.Error;
            }
        }

        /// <summary>
        /// 加载进度
        /// </summary>
        public float Progress
        {
            get
            {
                if (IsValidWithWarning == false)
                {
                    return 0;
                }

                return Provider.Progress;
            }
        }

        /// <summary>
        /// 加载耗时
        /// </summary>
        public long Duration
        {
            get
            {
                if (IsValidWithWarning == false)
                {
                    return 0;
                }

                return Provider.Duration;
            }
        }

        /// <summary>
        /// 是否加载完毕
        /// </summary>
        public bool IsDone
        {
            get
            {
                if (IsValidWithWarning == false)
                {
                    return false;
                }

                return Provider.IsDone;
            }
        }

        /// <summary>
        /// 句柄是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (Provider != null && Provider.IsDestroyed == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 句柄是否有效
        /// </summary>
        internal bool IsValidWithWarning
        {
            get
            {
                if (Provider != null && Provider.IsDestroyed == false)
                {
                    return true;
                }
                else
                {
                    if (Provider == null)
                    {
                        YooLogger.Warning($"Operation handle is released : {_assetInfo.AssetPath}");
                    }
                    else if (Provider.IsDestroyed)
                    {
                        YooLogger.Warning($"Provider is destroyed : {_assetInfo.AssetPath}");
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// 释放句柄
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        internal void ReleaseInternal()
        {
            if (IsValidWithWarning == false)
            {
                return;
            }

            Provider.ReleaseHandle(this);
            Provider = null;
        }

        #region 异步操作相关

        /// <summary>
        /// 异步操作任务
        /// </summary>
        public System.Threading.Tasks.Task Task
        {
            get { return Provider.Task; }
        }

        // 协程相关
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
            get { return Provider; }
        }

        #endregion
    }
}
