namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public struct EncryptResult
    {
        /// <summary>
        /// 文件是否加密
        /// </summary>
        public bool Encrypted;

        /// <summary>
        /// 加密后的文件数据
        /// </summary>
        public byte[] EncryptedData;
    }

    [UnityEngine.Scripting.Preserve]
    public struct EncryptFileInfo
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string BundleName;

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileLoadPath;
    }

    [UnityEngine.Scripting.Preserve]
    public interface IEncryptionServices
    {
        [UnityEngine.Scripting.Preserve]
        EncryptResult Encrypt(EncryptFileInfo fileInfo);
    }
}