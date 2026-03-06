using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    [Serializable]
    public class DebugPackageData
    {
        /// <summary>
        /// 包裹名称
        /// </summary>
        public string PackageName;

        /// <summary>
        /// 调试数据列表
        /// </summary>
        public List<DebugProviderInfo> ProviderInfos = new(1000);
    }
}