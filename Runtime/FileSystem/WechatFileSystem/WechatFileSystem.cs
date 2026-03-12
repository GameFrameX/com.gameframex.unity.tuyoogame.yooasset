#if UNITY_WEBGL && WECHAT_MINI_GAME
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using WeChatWASM;

[UnityEngine.Scripting.Preserve]
public static class WechatFileSystemCreater
{
    [UnityEngine.Scripting.Preserve]
    public static FileSystemParameters CreateWechatFileSystemParameters(IRemoteServices remoteServices = null)
    {
        string fileSystemClass = typeof(WechatFileSystem).FullName;
        var fileSystemParams = new FileSystemParameters(fileSystemClass, null);
        fileSystemParams.AddParameter(FileSystemParametersDefine.REMOTE_SERVICES, remoteServices);
        return fileSystemParams;
    }

    [UnityEngine.Scripting.Preserve]
    public static FileSystemParameters CreateWechatPathFileSystemParameters(string buildinPackRoot)
    {
        string fileSystemClass = typeof(WechatFileSystem).FullName;
        var fileSystemParams = new FileSystemParameters(fileSystemClass, null);
        IRemoteServices remoteServices = new WechatFileSystem.WebRemoteServices(buildinPackRoot);
        fileSystemParams.AddParameter(FileSystemParametersDefine.REMOTE_SERVICES, remoteServices);
        return fileSystemParams;
    }
}

/// <summary>
/// 微信小游戏文件系统
/// 参考：https://wechat-miniprogram.github.io/minigame-unity-webgl-transform/Design/UsingAssetBundle.html
/// </summary>
[UnityEngine.Scripting.Preserve]
internal class WechatFileSystem : IFileSystem
{
    [UnityEngine.Scripting.Preserve]
    public class WebRemoteServices : IRemoteServices
    {
        private readonly string _webPackageRoot;
        protected readonly Dictionary<string, string> _mapping = new Dictionary<string, string>(10000);

        [UnityEngine.Scripting.Preserve]
        public WebRemoteServices(string buildinPackRoot)
        {
            _webPackageRoot = buildinPackRoot;
        }

        [UnityEngine.Scripting.Preserve]
        string IRemoteServices.GetRemoteMainURL(string fileName,string packageVersion)
        {
            return GetFileLoadURL(fileName, packageVersion);
        }

        [UnityEngine.Scripting.Preserve]
        string IRemoteServices.GetRemoteFallbackURL(string fileName, string packageVersion)
        {
            return GetFileLoadURL(fileName, packageVersion);
        }

        [UnityEngine.Scripting.Preserve]
        private string GetFileLoadURL(string fileName, string packageVersion)
        {
            if (_mapping.TryGetValue(fileName, out string url) == false)
            {
                var filePath = PathUtility.Combine(_webPackageRoot, fileName);
                url = DownloadSystemHelper.ConvertToWWWPath(filePath);
                _mapping.Add(fileName, url);
            }
            //Debug.LogError($"WeChatFileSystem GetFileLoadURL url:{url}");
            return url;
        }
    }

    private readonly Dictionary<string, string> _cacheFilePaths = new Dictionary<string, string>(10000);
    private WXFileSystemManager _fileSystemManager;
    private string _fileCacheRoot = string.Empty;

    /// <summary>
    /// 包裹名称
    /// </summary>
    public string PackageName { private set; get; }

    /// <summary>
    /// 文件根目录
    /// </summary>
    public string FileRoot
    {
        get { return _fileCacheRoot; }
    }

    /// <summary>
    /// 文件数量
    /// </summary>
    public int FileCount
    {
        get { return 0; }
    }

    public string PackageVersion { get; set; }

    #region 自定义参数

    /// <summary>
    /// 自定义参数：远程服务接口
    /// </summary>
    public IRemoteServices RemoteServices { private set; get; } = null;

    #endregion

    [UnityEngine.Scripting.Preserve]
    public WechatFileSystem()
    {
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSInitializeFileSystemOperation InitializeFileSystemAsync()
    {
        var operation = new WXFSInitializeOperation(this);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSLoadPackageManifestOperation LoadPackageManifestAsync(string packageVersion, int timeout)
    {
        PackageVersion = packageVersion;
        var operation = new WXFSLoadPackageManifestOperation(this, packageVersion, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSRequestPackageVersionOperation RequestPackageVersionAsync(bool appendTimeTicks, int timeout)
    {
        var operation = new WXFSRequestPackageVersionOperation(this, appendTimeTicks, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSClearAllBundleFilesOperation ClearAllBundleFilesAsync()
    {
        var operation = new FSClearAllBundleFilesCompleteOperation();
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSClearUnusedBundleFilesOperation ClearUnusedBundleFilesAsync(PackageManifest manifest)
    {
        var operation = new FSClearUnusedBundleFilesCompleteOperation();
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSDownloadFileOperation DownloadFileAsync(PackageBundle bundle, DownloadParam param)
    {
        param.MainURL = RemoteServices.GetRemoteMainURL(bundle.FileName,PackageVersion);
        param.FallbackURL = RemoteServices.GetRemoteFallbackURL(bundle.FileName,PackageVersion);
        var operation = new WXFSDownloadFileOperation(this, bundle, param);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSLoadBundleOperation LoadBundleFile(PackageBundle bundle)
    {
        var operation = new WXFSLoadBundleOperation(this, bundle,PackageVersion);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual void UnloadBundleFile(PackageBundle bundle, object result)
    {
        AssetBundle assetBundle = result as AssetBundle;
        if (assetBundle != null)
        {
            assetBundle.Unload(true);
        }
    }

    [UnityEngine.Scripting.Preserve]
    public virtual void SetParameter(string name, object value)
    {
        if (name == FileSystemParametersDefine.REMOTE_SERVICES)
        {
            RemoteServices = (IRemoteServices)value;
        }
        else
        {
            YooLogger.Warning($"Invalid parameter : {name}");
        }
    }

    [UnityEngine.Scripting.Preserve]
    public virtual void OnCreate(string packageName, string rootDirectory)
    {
        PackageName = packageName;

        // 注意：CDN服务未启用的情况下，使用微信WEB服务器
        if (RemoteServices == null)
        {
            string webRoot = PathUtility.Combine(Application.streamingAssetsPath, YooAssetSettingsData.Setting.DefaultYooFolderName, packageName);
            RemoteServices = new WebRemoteServices(webRoot);
        }
        
        _fileSystemManager = WXBase.GetFileSystemManager();
        _fileCacheRoot = WX.env.USER_DATA_PATH; //注意：如果有子目录，请修改此处！
    }

    [UnityEngine.Scripting.Preserve]
    public virtual void OnUpdate()
    {
    }

    [UnityEngine.Scripting.Preserve]
    public virtual bool Belong(PackageBundle bundle)
    {
        return true;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual bool Exists(PackageBundle bundle)
    {
        string filePath = GetCacheFileLoadPath(bundle);
        string result = _fileSystemManager.AccessSync(filePath);
        return result.Equals("access:ok");
    }

    [UnityEngine.Scripting.Preserve]
    public virtual bool NeedDownload(PackageBundle bundle)
    {
        if (Belong(bundle) == false)
        {
            return false;
        }

        return Exists(bundle) == false;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual bool NeedUnpack(PackageBundle bundle)
    {
        return false;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual bool NeedImport(PackageBundle bundle)
    {
        return false;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual byte[] ReadFileData(PackageBundle bundle)
    {
        throw new System.NotImplementedException();
    }

    [UnityEngine.Scripting.Preserve]
    public virtual string ReadFileText(PackageBundle bundle)
    {
        throw new System.NotImplementedException();
    }

    #region 内部方法

    [UnityEngine.Scripting.Preserve]
    private string GetCacheFileLoadPath(PackageBundle bundle)
    {
        if (_cacheFilePaths.TryGetValue(bundle.BundleGUID, out string filePath) == false)
        {
            filePath = PathUtility.Combine(_fileCacheRoot, bundle.FileName);
            _cacheFilePaths.Add(bundle.BundleGUID, filePath);
        }

        return filePath;
    }

    [UnityEngine.Scripting.Preserve]
    public FSRequestPackageVersionOperation LoadLocalPackageVersionAsync(bool appendTimeTicks, int timeout)
    {
        var operation = new WXFSRequestPackageVersionOperation(this, appendTimeTicks, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public FSLoadPackageManifestOperation LoadLocalPackageManifestAsync(string packageVersion, int timeout)
    {
        PackageVersion = packageVersion;
        var operation = new WXFSLoadPackageManifestOperation(this, packageVersion, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public FSLoadPackageManifestOperation RequestRemotePackageManifestAsync(string packageVersion, int timeout)
    {
        PackageVersion = packageVersion;
        var operation = new WXFSLoadPackageManifestOperation(this, packageVersion, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public FSRequestPackageVersionOperation RequestRemotePackageVersionAsync(bool appendTimeTicks, int timeout)
    {
        var operation = new WXFSRequestPackageVersionOperation(this, appendTimeTicks, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    #endregion
}
#endif