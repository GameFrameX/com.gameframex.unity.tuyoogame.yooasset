namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    public abstract class FSRequestPackageVersionOperation : AsyncOperationBase
    {
        /// <summary>
        /// 资源版本
        /// </summary>
        [UnityEngine.Scripting.Preserve]
        public string PackageVersion { set; get; }
    }
}