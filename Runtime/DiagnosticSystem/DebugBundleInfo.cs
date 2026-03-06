using System;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    [Serializable]
    public class DebugBundleInfo : IComparer<DebugBundleInfo>, IComparable<DebugBundleInfo>
    {
        /// <summary>
        /// 包裹名
        /// </summary>
        public string PackageName { set; get; }

        /// <summary>
        /// 资源包名称
        /// </summary>
        public string BundleName;

        /// <summary>
        /// 引用计数
        /// </summary>
        public int RefCount;

        /// <summary>
        /// 加载状态
        /// </summary>
        public EOperationStatus Status;

        [UnityEngine.Scripting.Preserve]
        public int CompareTo(DebugBundleInfo other)
        {
            return Compare(this, other);
        }

        [UnityEngine.Scripting.Preserve]
        public int Compare(DebugBundleInfo a, DebugBundleInfo b)
        {
            return string.CompareOrdinal(a.BundleName, b.BundleName);
        }
    }
}