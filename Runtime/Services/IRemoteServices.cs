namespace YooAsset
{
    public interface IRemoteServices
    {
        /// <summary>
        /// 获取主资源站的资源地址
        /// </summary>
        /// <param name="fileName">请求的文件名称</param>
        /// <param name="packageVersion">资源包版本</param>
        string GetRemoteMainURL(string fileName, string packageVersion);

        /// <summary>
        /// 获取备用资源站的资源地址
        /// </summary>
        /// <param name="fileName">请求的文件名称</param>
        /// <param name="packageVersion">资源包版本</param>
        string GetRemoteFallbackURL(string fileName, string packageVersion);
    }
}