using System;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class LoadBundleFileOperation : AsyncOperationBase
    {
        [UnityEngine.Scripting.Preserve]
        private enum ESteps
        {
            None,
            LoadFile,
            Done,
        }

        private readonly ResourceManager _resourceManager;
        private readonly List<ProviderOperation> _providers = new(100);
        private readonly List<ProviderOperation> _removeList = new(100);
        private FSLoadBundleOperation _loadBundleOp;
        private ESteps _steps = ESteps.None;

        /// <summary>
        /// 资源包文件信息
        /// </summary>
        public BundleInfo BundleFileInfo { private set; get; }

        /// <summary>
        /// 是否已经销毁
        /// </summary>
        public bool IsDestroyed { private set; get; } = false;

        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount { private set; get; } = 0;

        /// <summary>
        /// 下载进度
        /// </summary>
        public float DownloadProgress { set; get; } = 0;

        /// <summary>
        /// 下载大小
        /// </summary>
        public long DownloadedBytes { set; get; } = 0;

        /// <summary>
        /// 加载结果
        /// </summary>
        public object Result { set; get; }


        [UnityEngine.Scripting.Preserve]
        internal LoadBundleFileOperation(ResourceManager resourceManager, BundleInfo bundleInfo)
        {
            _resourceManager = resourceManager;
            BundleFileInfo = bundleInfo;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            _steps = ESteps.LoadFile;
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
            {
                return;
            }

            if (_steps == ESteps.LoadFile)
            {
                if (_loadBundleOp == null)
                {
                    _loadBundleOp = BundleFileInfo.LoadBundleFile();
                }

                DownloadProgress = _loadBundleOp.DownloadProgress;
                DownloadedBytes = _loadBundleOp.DownloadedBytes;
                if (_loadBundleOp.IsDone == false)
                {
                    return;
                }

                if (_loadBundleOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    Result = _loadBundleOp.Result;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _loadBundleOp.Error;
                }
            }
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalWaitForAsyncComplete()
        {
            while (true)
            {
                if (_loadBundleOp != null)
                {
                    _loadBundleOp.WaitForAsyncComplete();
                }

                if (ExecuteWhileDone())
                {
                    _steps = ESteps.Done;
                    break;
                }
            }
        }

        /// <summary>
        /// 引用（引用计数递加）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void Reference()
        {
            RefCount++;
        }

        /// <summary>
        /// 释放（引用计数递减）
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void Release()
        {
            RefCount--;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void DestroyLoader()
        {
            IsDestroyed = true;

            // Check fatal
            if (RefCount > 0)
            {
                throw new Exception($"Bundle file loader ref is not zero : {BundleFileInfo.Bundle.BundleName}");
            }

            if (IsDone == false)
            {
                throw new Exception($"Bundle file loader is not done : {BundleFileInfo.Bundle.BundleName}");
            }

            BundleFileInfo.UnloadBundleFile(Result);
        }

        /// <summary>
        /// 是否可以销毁
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public bool CanDestroyLoader()
        {
            if (IsDone == false)
            {
                return false;
            }

            return RefCount <= 0;
        }

        /// <summary>
        /// 添加附属的资源提供者
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void AddProvider(ProviderOperation provider)
        {
            if (_providers.Contains(provider) == false)
            {
                _providers.Add(provider);
            }
        }

        /// <summary>
        /// 尝试销毁资源提供者
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void TryDestroyProviders()
        {
            // 获取移除列表
            _removeList.Clear();
            foreach (var provider in _providers)
            {
                if (provider.CanDestroyProvider())
                {
                    _removeList.Add(provider);
                }
            }

            // 销毁资源提供者
            foreach (var provider in _removeList)
            {
                _providers.Remove(provider);
                provider.DestroyProvider();
            }

            // 移除资源提供者
            if (_removeList.Count > 0)
            {
                _resourceManager.RemoveBundleProviders(_removeList);
                _removeList.Clear();
            }
        }

        /// <summary>
        /// 终止下载任务
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void AbortDownloadOperation()
        {
            if (_loadBundleOp != null)
            {
                _loadBundleOp.AbortDownloadOperation();
            }
        }
    }
}