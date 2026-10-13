using System;
using System.Collections.Generic;

namespace YooAsset
{
    /// <summary>
    /// Web 运行模式文件系统提供者注册表
    /// </summary>
    /// <remarks>
    /// 互斥解析语义：一次 Web 运行模式初始化只解析一个平台适配器——<see cref="Resolve"/> 返回 <see cref="IWebPlayModeFileSystemProvider.Priority"/>
    /// 最高的已注册提供者，同优先级时取注册序首个。
    /// 平台适配包通过自身的 Registrar（<see cref="UnityEngine.RuntimeInitializeOnLoadMethod"/>，BeforeSceneLoad）在启动时自注册；
    /// 注册表在 <see cref="UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration"/> 阶段自动清空（早于适配包的 BeforeSceneLoad 注册），
    /// 关闭 Domain Reload 时不会产生重复注册。
    /// 时序约束：适配包注册（BeforeSceneLoad）必须先于首次 Web 运行模式初始化调用；若宿主将资源初始化提前到更早的启动阶段，
    /// 须同步前移适配包的注册阶段，否则解析结果为空并回落默认 WebGL 文件系统。
    /// 兜底行为：<see cref="CreateFileSystemParameters(string, string)"/> 在无任何注册时回落
    /// <see cref="FileSystemParameters.CreateDefaultWebFileSystemParameters"/>，未安装适配包的 WebGL 工程行为不变。
    /// 注意：本注册表不参与 GameFrameworkEntry 的模块 Update/Shutdown 生命周期。
    /// </remarks>
    [UnityEngine.Scripting.Preserve]
    public static class WebPlayModeFileSystemProviderRegistry
    {
        private static readonly List<IWebPlayModeFileSystemProvider> Providers = new List<IWebPlayModeFileSystemProvider>(4);

        /// <summary>
        /// 重置Web运行模式文件系统提供者注册表
        /// </summary>
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        [UnityEngine.Scripting.Preserve]
        public static void Reset()
        {
            Providers.Clear();
        }

        /// <summary>
        /// 注册平台提供者（同渠道名重复注册时保留首个并告警）
        /// </summary>
        /// <param name="provider">平台提供者</param>
        [UnityEngine.Scripting.Preserve]
        public static void Register(IWebPlayModeFileSystemProvider provider)
        {
            if (provider == null)
            {
                return;
            }

            for (int i = 0; i < Providers.Count; i++)
            {
                if (string.Equals(Providers[i].ChannelName, provider.ChannelName, StringComparison.Ordinal))
                {
                    YooLogger.Warning($"Web play mode provider '{provider.ChannelName}' is already registered. The duplicate registration is ignored.");
                    return;
                }
            }

            Providers.Add(provider);
        }

        /// <summary>
        /// 注销平台提供者
        /// </summary>
        /// <param name="channelName">渠道唯一标识</param>
        [UnityEngine.Scripting.Preserve]
        public static void Unregister(string channelName)
        {
            for (int i = 0; i < Providers.Count; i++)
            {
                if (string.Equals(Providers[i].ChannelName, channelName, StringComparison.Ordinal))
                {
                    Providers.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 解析当前生效的平台提供者（Priority 最高者，同优先级取注册序首个）
        /// </summary>
        /// <returns>平台提供者；无注册返回null</returns>
        [UnityEngine.Scripting.Preserve]
        public static IWebPlayModeFileSystemProvider Resolve()
        {
            IWebPlayModeFileSystemProvider resolved = null;
            for (int i = 0; i < Providers.Count; i++)
            {
                var provider = Providers[i];
                if (resolved == null || provider.Priority > resolved.Priority)
                {
                    resolved = provider;
                }
            }

            return resolved;
        }

        /// <summary>
        /// 创建Web运行模式文件系统参数的便捷门面
        /// </summary>
        /// <remarks>
        /// 内部解析当前平台提供者并转发构造；无任何注册时回落默认 WebGL 文件系统参数。
        /// </remarks>
        /// <param name="hostServerURL">主机服务器URL</param>
        /// <param name="fallbackHostServerURL">备用主机服务器URL</param>
        /// <returns>文件系统参数</returns>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateFileSystemParameters(string hostServerURL, string fallbackHostServerURL)
        {
            var provider = Resolve();
            if (provider == null)
            {
                return FileSystemParameters.CreateDefaultWebFileSystemParameters();
            }

            var context = new WebPlayModeProviderContext(hostServerURL, fallbackHostServerURL);
            return provider.CreateFileSystemParameters(context);
        }
    }
}
