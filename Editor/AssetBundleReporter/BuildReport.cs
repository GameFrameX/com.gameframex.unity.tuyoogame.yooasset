using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YooAsset.Editor
{
    /// <summary>
    /// 构建报告
    /// </summary>
    [Serializable]
    public class BuildReport
    {
        /// <summary>
        /// 汇总信息
        /// </summary>
        public ReportSummary Summary = new ReportSummary();

        /// <summary>
        /// 资源对象列表
        /// </summary>
        public List<ReportAssetInfo> AssetInfos = new List<ReportAssetInfo>();

        /// <summary>
        /// 资源包列表
        /// </summary>
        public List<ReportBundleInfo> BundleInfos = new List<ReportBundleInfo>();

        /// <summary>
        /// 未被依赖的资源列表
        /// </summary>
        public List<ReportIndependAsset> IndependAssets = new List<ReportIndependAsset>();

        [NonSerialized]
        private Dictionary<string, ReportBundleInfo> _bundleInfoCache;
        [NonSerialized]
        private Dictionary<string, ReportAssetInfo> _assetInfoCache;

        private void BuildIndexCache()
        {
            if (_bundleInfoCache != null && _assetInfoCache != null)
            {
                return;
            }

            _bundleInfoCache = new Dictionary<string, ReportBundleInfo>(BundleInfos.Count);
            foreach (var bundleInfo in BundleInfos)
            {
                _bundleInfoCache[bundleInfo.BundleName] = bundleInfo;
            }

            _assetInfoCache = new Dictionary<string, ReportAssetInfo>(AssetInfos.Count);
            foreach (var assetInfo in AssetInfos)
            {
                _assetInfoCache[assetInfo.AssetPath] = assetInfo;
            }
        }

        /// <summary>
        /// 获取资源包信息类
        /// </summary>
        public ReportBundleInfo GetBundleInfo(string bundleName)
        {
            BuildIndexCache();
            if (_bundleInfoCache.TryGetValue(bundleName, out ReportBundleInfo bundleInfo))
            {
                return bundleInfo;
            }
            throw new Exception($"Not found bundle : {bundleName}");
        }

        /// <summary>
        /// 获取资源信息类
        /// </summary>
        public ReportAssetInfo GetAssetInfo(string assetPath)
        {
            BuildIndexCache();
            if (_assetInfoCache.TryGetValue(assetPath, out ReportAssetInfo assetInfo))
            {
                return assetInfo;
            }
            throw new Exception($"Not found asset : {assetPath}");
        }


        public static void Serialize(string savePath, BuildReport buildReport)
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            string json = JsonUtility.ToJson(buildReport, true);
            FileUtility.WriteAllText(savePath, json);
        }
        public static BuildReport Deserialize(string jsonData)
        {
            BuildReport report = JsonUtility.FromJson<BuildReport>(jsonData);
            return report;
        }
    }
}