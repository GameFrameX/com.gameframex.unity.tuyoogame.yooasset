#if UNITY_WEBGL && ENABLE_KUAISHOU_MINI_GAME
using YooAsset;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public static class KuaiShouFileSystemCreater
    {
        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateKuaiShouFileSystemParameters(IRemoteServices remoteServices = null)
        {
            string fileSystemClass = typeof(KuaiShouFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, null);
            fileSystemParams.AddParameter(FileSystemParametersDefine.REMOTE_SERVICES, remoteServices);
            return fileSystemParams;
        }

        [UnityEngine.Scripting.Preserve]
        public static FileSystemParameters CreateKuaiShouPathFileSystemParameters(string buildinPackRoot)
        {
            string fileSystemClass = typeof(KuaiShouFileSystem).FullName;
            var fileSystemParams = new FileSystemParameters(fileSystemClass, null);
            var remoteServices = new KuaiShouFileSystem.WebRemoteServices(buildinPackRoot);
            fileSystemParams.AddParameter(FileSystemParametersDefine.REMOTE_SERVICES, remoteServices);
            return fileSystemParams;
        }
    }
}

#endif