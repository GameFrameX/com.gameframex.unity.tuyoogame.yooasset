namespace YooAsset
{
    /// <summary>
    /// Web 运行模式初始化上下文
    /// </summary>
    /// <remarks>
    /// 携带宿主传入的主机地址与备用主机地址；<see cref="FallbackHostServerURL"/> 的真实消费策略由各平台适配器自行决定。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public sealed class WebPlayModeProviderContext
    {
        /// <summary>
        /// 主机服务器URL
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public string HostServerURL { get; }

        /// <summary>
        /// 备用主机服务器URL
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public string FallbackHostServerURL { get; }

        [UnityEngine.Scripting.Preserve]
        public WebPlayModeProviderContext(string hostServerURL, string fallbackHostServerURL)
        {
            HostServerURL = hostServerURL;
            FallbackHostServerURL = fallbackHostServerURL;
        }
    }
}
