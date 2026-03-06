using System;
using System.Collections.Generic;

namespace YooAsset
{
    /// <summary>
    /// 默认的构建管线
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public enum EDefaultBuildPipeline
    {
        /// <summary>
        /// 内置构建管线
        /// </summary>
        BuiltinBuildPipeline,

        /// <summary>
        /// 可编程构建管线
        /// </summary>
        ScriptableBuildPipeline,

        /// <summary>
        /// 原生文件构建管线
        /// </summary>
        RawFileBuildPipeline,
    }

    /// <summary>
    /// 运行模式
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public enum EPlayMode
    {
        /// <summary>
        /// 编辑器下的模拟模式
        /// </summary>
        EditorSimulateMode,

        /// <summary>
        /// 离线运行模式
        /// </summary>
        OfflinePlayMode,

        /// <summary>
        /// 联机运行模式
        /// </summary>
        HostPlayMode,

        /// <summary>
        /// WebGL运行模式
        /// </summary>
        WebPlayMode,
    }

    /// <summary>
    /// 文件系统参数
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class FileSystemParameters
    {
        internal Dictionary<string, object> CreateParameters = new Dictionary<string, object>();

        /// <summary>
        /// 文件系统类
        /// 格式: "namespace.class,assembly"
        /// 格式: "命名空间.类型名,程序集"
        /// </summary>
        public string FileSystemClass { private set; get; }

        /// <summary>
        /// 文件系统的根目录
        /// </summary>
        public string RootDirectory { private set; get; }


        [UnityEngine.Scripting.Preserve]
        public FileSystemParameters(string fileSystemClass, string rootDirectory)
        {
            FileSystemClass = fileSystemClass;
            RootDirectory = rootDirectory;
        }

        /// <summary>
        /// 添加自定义参数
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public void AddParameter(string name, object value)
        {
            CreateParameters.Add(name, value);
        }


        /// <summary>
        /// 创建默认的编辑器文件系统参数
        /// <param name="simulateBuildResult">模拟构建结果</param>
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateDefaultEditorFileSystemParameters(SimulateBuildResult simulateBuildResult)
        {
            var fileSystemClass = typeof(DefaultEditorFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, simulateBuildResult.PackageRootDirectory);
            return fileSystemParams;
        }

        /// <summary>
        /// 创建默认的内置文件系统参数
        /// </summary>
        /// <param name="decryptionServices">加密文件解密服务类</param>
        /// <param name="verifyLevel">缓存文件的校验等级</param>
        /// <param name="rootDirectory">内置文件的根路径</param>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateDefaultBuildinFileSystemParameters(IDecryptionServices decryptionServices = null, EFileVerifyLevel verifyLevel = EFileVerifyLevel.Middle, string rootDirectory = null)
        {
            var fileSystemClass = typeof(DefaultBuildinFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, rootDirectory);
            fileSystemParams.AddParameter(FileSystemParametersDefine.DECRYPTION_SERVICES, decryptionServices);
            fileSystemParams.AddParameter(FileSystemParametersDefine.FILE_VERIFY_LEVEL, verifyLevel);
            return fileSystemParams;
        }

        /// <summary>
        /// 创建默认的内置文件系统参数（原生文件）
        /// </summary>
        /// <param name="decryptionServices">加密文件解密服务类</param>
        /// <param name="verifyLevel">缓存文件的校验等级</param>
        /// <param name="rootDirectory">内置文件的根路径</param>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateDefaultBuildinRawFileSystemParameters(IDecryptionServices decryptionServices = null, EFileVerifyLevel verifyLevel = EFileVerifyLevel.Middle, string rootDirectory = null)
        {
            var fileSystemClass = typeof(DefaultBuildinFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, rootDirectory);
            fileSystemParams.AddParameter(FileSystemParametersDefine.DECRYPTION_SERVICES, decryptionServices);
            fileSystemParams.AddParameter(FileSystemParametersDefine.FILE_VERIFY_LEVEL, verifyLevel);
            fileSystemParams.AddParameter(FileSystemParametersDefine.APPEND_FILE_EXTENSION, true);
            fileSystemParams.AddParameter(FileSystemParametersDefine.RAW_FILE_BUILD_PIPELINE, true);
            return fileSystemParams;
        }

        /// <summary>
        /// 创建默认的缓存文件系统参数
        /// </summary>
        /// <param name="remoteServices">远端资源地址查询服务类</param>
        /// <param name="decryptionServices">加密文件解密服务类</param>
        /// <param name="verifyLevel">缓存文件的校验等级</param>
        /// <param name="rootDirectory">文件系统的根目录</param>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateDefaultCacheFileSystemParameters(IRemoteServices remoteServices, IDecryptionServices decryptionServices = null, EFileVerifyLevel verifyLevel = EFileVerifyLevel.Middle, string rootDirectory = null)
        {
            var fileSystemClass = typeof(DefaultCacheFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, rootDirectory);
            fileSystemParams.AddParameter(FileSystemParametersDefine.REMOTE_SERVICES, remoteServices);
            fileSystemParams.AddParameter(FileSystemParametersDefine.DECRYPTION_SERVICES, decryptionServices);
            fileSystemParams.AddParameter(FileSystemParametersDefine.FILE_VERIFY_LEVEL, verifyLevel);
            return fileSystemParams;
        }

        /// <summary>
        /// 创建默认的缓存文件系统参数（原生文件）
        /// </summary>
        /// <param name="remoteServices">远端资源地址查询服务类</param>
        /// <param name="decryptionServices">加密文件解密服务类</param>
        /// <param name="verifyLevel">缓存文件的校验等级</param>
        /// <param name="rootDirectory">文件系统的根目录</param>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateDefaultCacheRawFileSystemParameters(IRemoteServices remoteServices, IDecryptionServices decryptionServices = null, EFileVerifyLevel verifyLevel = EFileVerifyLevel.Middle, string rootDirectory = null)
        {
            var fileSystemClass = typeof(DefaultCacheFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, rootDirectory);
            fileSystemParams.AddParameter(FileSystemParametersDefine.REMOTE_SERVICES, remoteServices);
            fileSystemParams.AddParameter(FileSystemParametersDefine.DECRYPTION_SERVICES, decryptionServices);
            fileSystemParams.AddParameter(FileSystemParametersDefine.FILE_VERIFY_LEVEL, verifyLevel);
            fileSystemParams.AddParameter(FileSystemParametersDefine.APPEND_FILE_EXTENSION, true);
            fileSystemParams.AddParameter(FileSystemParametersDefine.RAW_FILE_BUILD_PIPELINE, true);
            return fileSystemParams;
        }

        /// <summary>
        /// 创建默认的Web文件系统参数
        /// </summary>
        /// <param name="disableUnityWebCache">禁用Unity的网络缓存</param>
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateDefaultWebFileSystemParameters(bool disableUnityWebCache = false)
        {
            var fileSystemClass = typeof(DefaultWebFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, null);
            fileSystemParams.AddParameter(FileSystemParametersDefine.DISABLE_UNITY_WEB_CACHE, disableUnityWebCache);
            return fileSystemParams;
        }
    }

    /// <summary>
    /// 初始化参数
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public abstract class InitializeParameters
    {
    }

    /// <summary>
    /// 编辑器下模拟运行模式的初始化参数
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class EditorSimulateModeParameters : InitializeParameters
    {
        public FileSystemParameters EditorFileSystemParameters;
    }

    /// <summary>
    /// 离线运行模式的初始化参数
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class OfflinePlayModeParameters : InitializeParameters
    {
        public FileSystemParameters BuildinFileSystemParameters;
    }

    /// <summary>
    /// 联机运行模式的初始化参数
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class HostPlayModeParameters : InitializeParameters
    {
        public FileSystemParameters BuildinFileSystemParameters;
        public FileSystemParameters DeliveryFileSystemParameters;
        public FileSystemParameters CacheFileSystemParameters;
    }

    /// <summary>
    /// WebGL运行模式的初始化参数
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public class WebPlayModeParameters : InitializeParameters
    {
        public FileSystemParameters WebFileSystemParameters;
    }
}