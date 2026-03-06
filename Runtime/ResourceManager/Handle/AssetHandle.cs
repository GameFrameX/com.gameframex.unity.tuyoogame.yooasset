using System;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public sealed class AssetHandle : HandleBase, IDisposable
    {
        private Action<AssetHandle> _callback;

        [UnityEngine.Scripting.Preserve]
        internal AssetHandle(ProviderOperation provider) : base(provider)
        {
        }

        [UnityEngine.Scripting.Preserve]
        internal override void InvokeCallback()
        {
            _callback?.Invoke(this);
        }

        /// <summary>
        /// 完成委托
        /// </summary>
        public event Action<AssetHandle> Completed
        {
            add
            {
                if (IsValidWithWarning == false)
                {
                    throw new Exception($"{nameof(AssetHandle)} is invalid");
                }

                if (Provider.IsDone)
                {
                    value.Invoke(this);
                }
                else
                {
                    _callback += value;
                }
            }
            remove
            {
                if (IsValidWithWarning == false)
                {
                    throw new Exception($"{nameof(AssetHandle)} is invalid");
                }

                _callback -= value;
            }
        }

        /// <summary>
        /// 等待异步执行完毕
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void WaitForAsyncComplete()
        {
            if (IsValidWithWarning == false)
            {
                return;
            }

            Provider.WaitForAsyncComplete();
        }

        /// <summary>
        /// 释放资源句柄
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void Release()
        {
            ReleaseInternal();
        }

        /// <summary>
        /// 释放资源句柄
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void Dispose()
        {
            ReleaseInternal();
        }


        /// <summary>
        /// 资源对象
        /// </summary>
        public UnityEngine.Object AssetObject
        {
            get
            {
                if (IsValidWithWarning == false)
                {
                    return null;
                }

                return Provider.AssetObject;
            }
        }

        /// <summary>
        /// 获取资源对象
        /// </summary>
        /// <typeparam name="TAsset">资源类型</typeparam>
        [UnityEngine.Scripting.Preserve]
        public TAsset GetAssetObject<TAsset>() where TAsset : UnityEngine.Object
        {
            if (IsValidWithWarning == false)
            {
                return null;
            }

            return Provider.AssetObject as TAsset;
        }

        /// <summary>
        /// 同步初始化游戏对象
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public GameObject InstantiateSync()
        {
            return InstantiateSyncInternal(false, Vector3.zero, Quaternion.identity, null, false);
        }

        [UnityEngine.Scripting.Preserve]
        public GameObject InstantiateSync(Transform parent)
        {
            return InstantiateSyncInternal(false, Vector3.zero, Quaternion.identity, parent, false);
        }

        [UnityEngine.Scripting.Preserve]
        public GameObject InstantiateSync(Transform parent, bool worldPositionStays)
        {
            return InstantiateSyncInternal(false, Vector3.zero, Quaternion.identity, parent, worldPositionStays);
        }

        [UnityEngine.Scripting.Preserve]
        public GameObject InstantiateSync(Vector3 position, Quaternion rotation)
        {
            return InstantiateSyncInternal(true, position, rotation, null, false);
        }

        [UnityEngine.Scripting.Preserve]
        public GameObject InstantiateSync(Vector3 position, Quaternion rotation, Transform parent)
        {
            return InstantiateSyncInternal(true, position, rotation, parent, false);
        }

        /// <summary>
        /// 异步初始化游戏对象
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public InstantiateOperation InstantiateAsync()
        {
            return InstantiateAsyncInternal(false, Vector3.zero, Quaternion.identity, null, false);
        }

        [UnityEngine.Scripting.Preserve]
        public InstantiateOperation InstantiateAsync(Transform parent)
        {
            return InstantiateAsyncInternal(false, Vector3.zero, Quaternion.identity, parent, false);
        }

        [UnityEngine.Scripting.Preserve]
        public InstantiateOperation InstantiateAsync(Transform parent, bool worldPositionStays)
        {
            return InstantiateAsyncInternal(false, Vector3.zero, Quaternion.identity, parent, worldPositionStays);
        }

        [UnityEngine.Scripting.Preserve]
        public InstantiateOperation InstantiateAsync(Vector3 position, Quaternion rotation)
        {
            return InstantiateAsyncInternal(true, position, rotation, null, false);
        }

        [UnityEngine.Scripting.Preserve]
        public InstantiateOperation InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent)
        {
            return InstantiateAsyncInternal(true, position, rotation, parent, false);
        }

        [UnityEngine.Scripting.Preserve]
        private GameObject InstantiateSyncInternal(bool setPositionAndRotation, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            if (IsValidWithWarning == false)
            {
                return null;
            }

            if (Provider.AssetObject == null)
            {
                return null;
            }

            return InstantiateOperation.InstantiateInternal(Provider.AssetObject, setPositionAndRotation, position, rotation, parent, worldPositionStays);
        }

        [UnityEngine.Scripting.Preserve]
        private InstantiateOperation InstantiateAsyncInternal(bool setPositionAndRotation, Vector3 position, Quaternion rotation, Transform parent, bool worldPositionStays)
        {
            var packageName = GetAssetInfo().PackageName;
            var operation = new InstantiateOperation(this, setPositionAndRotation, position, rotation, parent, worldPositionStays);
            OperationSystem.StartOperation(packageName, operation);
            return operation;
        }
    }
}