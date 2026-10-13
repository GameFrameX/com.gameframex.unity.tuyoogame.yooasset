using System.Reflection;
using NUnit.Framework;
using YooAsset;

namespace YooAsset.Runtime.Tests
{
    public class WebPlayModeFileSystemProviderRegistryTests
    {
        private class StubProvider : IWebPlayModeFileSystemProvider
        {
            public string ChannelName { get; }
            public int Priority { get; }
            public int CreateCallCount { get; private set; }
            public WebPlayModeProviderContext LastContext { get; private set; }

            public StubProvider(string channelName, int priority)
            {
                ChannelName = channelName;
                Priority = priority;
            }

            public FileSystemParameters CreateFileSystemParameters(WebPlayModeProviderContext context)
            {
                CreateCallCount++;
                LastContext = context;
                return new FileSystemParameters(typeof(StubProvider).FullName, null);
            }
        }

        /// <summary>
        /// 与各平台适配包 Registrar 完全同构的自注册闭环存根（私有静态无参 Register）
        /// </summary>
        internal static class StubRegistrar
        {
            private static void Register()
            {
                WebPlayModeFileSystemProviderRegistry.Register(new StubProvider("Stub", 100));
            }
        }

        [SetUp]
        public void SetUp()
        {
            WebPlayModeFileSystemProviderRegistry.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            WebPlayModeFileSystemProviderRegistry.Reset();
        }

        [Test]
        public void Register_DuplicateChannelName_KeepsFirstRegistration()
        {
            var first = new StubProvider("stub", 1);
            var second = new StubProvider("stub", 9);
            WebPlayModeFileSystemProviderRegistry.Register(first);
            WebPlayModeFileSystemProviderRegistry.Register(second);

            Assert.AreSame(first, WebPlayModeFileSystemProviderRegistry.Resolve(), "重复注册应保留首个");
        }

        [Test]
        public void Register_NullProvider_IsIgnored()
        {
            WebPlayModeFileSystemProviderRegistry.Register(null);

            Assert.IsNull(WebPlayModeFileSystemProviderRegistry.Resolve());
        }

        [Test]
        public void Resolve_HighestPriorityWins()
        {
            var low = new StubProvider("low", 0);
            var high = new StubProvider("high", 5);
            WebPlayModeFileSystemProviderRegistry.Register(low);
            WebPlayModeFileSystemProviderRegistry.Register(high);

            Assert.AreSame(high, WebPlayModeFileSystemProviderRegistry.Resolve());
        }

        [Test]
        public void Resolve_SamePriority_ReturnsFirstRegistered()
        {
            var first = new StubProvider("first", 3);
            var second = new StubProvider("second", 3);
            WebPlayModeFileSystemProviderRegistry.Register(first);
            WebPlayModeFileSystemProviderRegistry.Register(second);

            Assert.AreSame(first, WebPlayModeFileSystemProviderRegistry.Resolve(), "同优先级应取注册序首个");
        }

        [Test]
        public void Unregister_RemovesProviderAndAllowsReRegister()
        {
            var provider = new StubProvider("stub", 1);
            WebPlayModeFileSystemProviderRegistry.Register(provider);

            WebPlayModeFileSystemProviderRegistry.Unregister("stub");
            Assert.IsNull(WebPlayModeFileSystemProviderRegistry.Resolve());

            var reRegistered = new StubProvider("stub", 1);
            WebPlayModeFileSystemProviderRegistry.Register(reRegistered);
            Assert.AreSame(reRegistered, WebPlayModeFileSystemProviderRegistry.Resolve());
        }

        [Test]
        public void Reset_ClearsAllRegistrations()
        {
            WebPlayModeFileSystemProviderRegistry.Register(new StubProvider("stub", 1));

            WebPlayModeFileSystemProviderRegistry.Reset();

            Assert.IsNull(WebPlayModeFileSystemProviderRegistry.Resolve());
        }

        [Test]
        public void Reset_IsWiredToSubsystemRegistration_ForDomainReloadSafety()
        {
            var method = typeof(WebPlayModeFileSystemProviderRegistry).GetMethod("Reset", BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(method);

            var attribute = method.GetCustomAttribute<UnityEngine.RuntimeInitializeOnLoadMethodAttribute>();
            Assert.IsNotNull(attribute, "Reset 必须挂 RuntimeInitializeOnLoadMethod 以在关闭 Domain Reload 时不产生重复注册");
            Assert.AreEqual(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration, attribute.loadType);
        }

        [Test]
        public void CreateFileSystemParameters_NoProvider_FallsBackToDefaultWebFileSystem()
        {
            var parameters = WebPlayModeFileSystemProviderRegistry.CreateFileSystemParameters("https://host", "https://fallback");

            Assert.IsNotNull(parameters);
            Assert.AreEqual(typeof(DefaultWebFileSystem).FullName, parameters.FileSystemClass, "无注册时应回落默认 WebGL 文件系统");
        }

        [Test]
        public void CreateFileSystemParameters_RegisteredProvider_PassesContextThrough()
        {
            var provider = new StubProvider("stub", 1);
            WebPlayModeFileSystemProviderRegistry.Register(provider);

            var parameters = WebPlayModeFileSystemProviderRegistry.CreateFileSystemParameters("https://host", "https://fallback");

            Assert.AreEqual(typeof(StubProvider).FullName, parameters.FileSystemClass, "门面应返回 provider 的构造结果");
            Assert.AreEqual(1, provider.CreateCallCount);
            Assert.AreEqual("https://host", provider.LastContext.HostServerURL);
            Assert.AreEqual("https://fallback", provider.LastContext.FallbackHostServerURL);
        }

        [Test]
        public void Registrar_SelfRegistrationClosedLoop_RegistersViaReflection()
        {
            // 适配包 Registrar 均为嵌套私有静态无参 Register，用反射验证该形态可驱动注册闭环
            var registerMethod = typeof(StubRegistrar).GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(registerMethod);
            registerMethod.Invoke(null, null);

            var resolved = WebPlayModeFileSystemProviderRegistry.Resolve();
            Assert.IsInstanceOf<StubProvider>(resolved);
            Assert.AreEqual("Stub", resolved.ChannelName);
        }

        [Test]
        public void WeChatAdapterRegistrar_WhenInstalled_RegistersWeChatProvider()
        {
            // wechat 适配包条件编译未激活（EditMode 非 WebGL）时类型不存在，跳过；真机/构建侧生效
            var registrarType = System.Type.GetType("YooAsset.WeChat.WeChatWebPlayModeFileSystemProvider+Registrar, YooAsset.WeChat");
            Assume.That(registrarType, Is.Not.Null, "未安装 wechat 适配包或平台条件未激活时跳过");
            var registerMethod = registrarType.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static);
            registerMethod.Invoke(null, null);

            var resolved = WebPlayModeFileSystemProviderRegistry.Resolve();
            Assert.IsNotNull(resolved);
            Assert.AreEqual("WeChat", resolved.ChannelName);
        }
    }
}
