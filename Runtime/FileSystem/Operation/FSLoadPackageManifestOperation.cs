
namespace YooAsset
{
    public abstract class FSLoadPackageManifestOperation : AsyncOperationBase
    {
        /// <summary>
        /// 资源清单
        /// </summary>
        public PackageManifest Manifest { set; get; }
    }
}