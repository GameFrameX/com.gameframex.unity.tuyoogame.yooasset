namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal interface IBundleQuery
    {
        /// <summary>
        /// 获取主资源包信息
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        BundleInfo GetMainBundleInfo(AssetInfo assetInfo);

        /// <summary>
        /// 获取依赖的资源包信息集合
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        BundleInfo[] GetDependBundleInfos(AssetInfo assetPath);

        /// <summary>
        /// 获取主资源包名称
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        string GetMainBundleName(AssetInfo assetInfo);

        /// <summary>
        /// 获取依赖的资源包名称集合
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        string[] GetDependBundleNames(AssetInfo assetInfo);

        /// <summary>
        /// 清单是否有效
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        bool ManifestValid();
    }
}