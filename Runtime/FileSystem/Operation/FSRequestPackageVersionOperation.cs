
namespace YooAsset
{
    public abstract class FSRequestPackageVersionOperation : AsyncOperationBase
    {
        /// <summary>
        /// 资源版本
        /// </summary>
        public string PackageVersion { set; get; }
    }
}