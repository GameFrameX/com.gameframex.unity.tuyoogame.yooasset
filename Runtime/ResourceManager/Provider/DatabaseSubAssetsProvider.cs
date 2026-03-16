using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class DatabaseSubAssetsProvider : ProviderOperation
    {
        [UnityEngine.Scripting.Preserve]
        public DatabaseSubAssetsProvider(ResourceManager manager, string providerGUID, AssetInfo assetInfo) : base(manager, providerGUID, assetInfo)
        {
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnStart()
        {
            BeginLoadTimeRecord();
            DebugBeginRecording();
        }

        [UnityEngine.Scripting.Preserve]
        public override void InternalOnUpdate()
        {
#if UNITY_EDITOR
            if (IsDone)
            {
                return;
            }

            if (_steps == ESteps.None)
            {
                // 检测资源文件是否存在
                var guid = UnityEditor.AssetDatabase.AssetPathToGUID(MainAssetInfo.AssetPath);
                if (string.IsNullOrEmpty(guid))
                {
                    var error = $"Not found asset : {MainAssetInfo.AssetPath}";
                    YooLogger.Error(error);
                    InvokeCompletion(error, EOperationStatus.Failed);
                    return;
                }

                _steps = ESteps.CheckBundle;

                // 注意：模拟异步加载效果提前返回
                if (IsWaitForAsyncComplete == false)
                {
                    return;
                }
            }

            // 1. 检测资源包
            if (_steps == ESteps.CheckBundle)
            {
                if (LoadBundleFileOp.IsDone == false)
                {
                    return;
                }

                if (LoadBundleFileOp.Status != EOperationStatus.Succeed)
                {
                    InvokeCompletion(LoadBundleFileOp.Error, EOperationStatus.Failed);
                    return;
                }

                _steps = ESteps.Loading;
            }

            // 2. 加载资源对象
            if (_steps == ESteps.Loading)
            {
                if (MainAssetInfo.AssetType == null)
                {
                    AllAssetObjects = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(MainAssetInfo.AssetPath);
                }
                else
                {
                    var findAssets = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(MainAssetInfo.AssetPath);
                    var result = new List<Object>(findAssets.Length);
                    foreach (var findAsset in findAssets)
                    {
                        if (MainAssetInfo.AssetType.IsAssignableFrom(findAsset.GetType()))
                        {
                            result.Add(findAsset);
                        }
                    }

                    AllAssetObjects = result.ToArray();
                }

                _steps = ESteps.Checking;
            }

            // 3. 检测加载结果
            if (_steps == ESteps.Checking)
            {
                if (AllAssetObjects == null)
                {
                    string error;
                    if (MainAssetInfo.AssetType == null)
                    {
                        error = $"Failed to load sub assets : {MainAssetInfo.AssetPath} AssetType : null";
                    }
                    else
                    {
                        error = $"Failed to load sub assets : {MainAssetInfo.AssetPath} AssetType : {MainAssetInfo.AssetType}";
                    }

                    YooLogger.Error(error);
                    InvokeCompletion(error, EOperationStatus.Failed);
                }
                else
                {
                    InvokeCompletion(string.Empty, EOperationStatus.Succeed);
                }
            }
#endif
        }
    }
}
