
namespace YooAsset
{
    /// <summary>
    /// 小游戏平台 Web 运行模式文件系统提供者契约
    /// </summary>
    /// <remarks>
    /// 平台适配包实现本接口，并通过自身 Registrar（RuntimeInitializeOnLoadMethod）在启动时向
    /// <see cref="WebPlayModeFileSystemProviderRegistry"/> 自注册；未安装适配包时不产生任何平台耦合。
    /// </remarks>
    public interface IWebPlayModeFileSystemProvider
    {
        /// <summary>
        /// 渠道唯一标识（用于注册去重）
        /// </summary>
        string ChannelName { get; }

        /// <summary>
        /// 解析优先级：数值最高者胜出，同优先级取注册序首个
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 创建平台文件系统参数
        /// </summary>
        /// <param name="context">Web 初始化上下文</param>
        /// <returns>文件系统参数</returns>
        FileSystemParameters CreateFileSystemParameters(WebPlayModeProviderContext context);
    }
}
