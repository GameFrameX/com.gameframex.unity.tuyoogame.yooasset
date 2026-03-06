using System.IO;
using UnityEngine;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public struct DecryptFileInfo
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string BundleName;

        /// <summary>
        /// 文件加载路径
        /// </summary>
        public string FileLoadPath;

        /// <summary>
        /// Unity引擎用于内容校验的CRC
        /// </summary>
        public uint FileLoadCRC;
    }

    [UnityEngine.Scripting.Preserve]
    public interface IDecryptionServices
    {
        /// <summary>
        /// 同步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        AssetBundle LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream);

        /// <summary>
        /// 异步方式获取解密的资源包对象
        /// 注意：加载流对象在资源包对象释放的时候会自动释放
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        AssetBundleCreateRequest LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream);

        /// <summary>
        /// 获取解密的字节数据
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        byte[] ReadFileData(DecryptFileInfo fileInfo);

        /// <summary>
        /// 获取解密的文本数据
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        string ReadFileText(DecryptFileInfo fileInfo);
    }
}