#if UNITY_WEBGL && ENABLE_KUAISHOU_MINI_GAME

// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相应法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root directory of the source code for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any legal disputes and liabilities arising from secondary development based on this project
//   本项目组织与贡献者概不承担。
//   shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository: https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:  https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:  https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

using System.Collections.Generic;
using UnityEngine;
using YooAsset;

/// <summary>
/// 快手小游戏文件系统
/// 参考：https://open.kuaishou.com/miniGameDocs/gameDev/Unity/Launchability/AssetBundle.html
/// </summary>
[UnityEngine.Scripting.Preserve]
internal class KuaiShouFileSystem : IFileSystem
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
        string IRemoteServices.GetRemoteMainURL(string fileName, string packageVersion)
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
    private KSWASM.KSFileSystemManager _fileSystemManager;
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
    public KuaiShouFileSystem()
    {
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSInitializeFileSystemOperation InitializeFileSystemAsync()
    {
        var operation = new KSFSInitializeOperation(this);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSLoadPackageManifestOperation LoadPackageManifestAsync(string packageVersion, int timeout)
    {
        PackageVersion = packageVersion;
        var operation = new KSFSLoadPackageManifestOperation(this, packageVersion, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSRequestPackageVersionOperation RequestPackageVersionAsync(bool appendTimeTicks, int timeout)
    {
        var operation = new KSFSRequestPackageVersionOperation(this, appendTimeTicks, timeout);
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
        param.MainURL = RemoteServices.GetRemoteMainURL(bundle.FileName, PackageVersion);
        param.FallbackURL = RemoteServices.GetRemoteFallbackURL(bundle.FileName, PackageVersion);
        var operation = new KSFSDownloadFileOperation(this, bundle, param);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual FSLoadBundleOperation LoadBundleFile(PackageBundle bundle)
    {
        var operation = new KSFSLoadBundleOperation(this, bundle, PackageVersion);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public virtual void UnloadBundleFile(PackageBundle bundle, object result)
    {
        var assetBundle = (result as KSWASM.KSAssetBundle);
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

        _fileSystemManager = KSWASM.KSBase.GetFileSystemManager();
        _fileCacheRoot = KSWASM.KSBase.env.USER_DATA_PATH; //注意：如果有子目录，请修改此处！
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
        var operation = new KSFSRequestPackageVersionOperation(this, appendTimeTicks, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public FSLoadPackageManifestOperation LoadLocalPackageManifestAsync(string packageVersion, int timeout)
    {
        PackageVersion = packageVersion;
        var operation = new KSFSLoadPackageManifestOperation(this, packageVersion, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public FSLoadPackageManifestOperation RequestRemotePackageManifestAsync(string packageVersion, int timeout)
    {
        PackageVersion = packageVersion;
        var operation = new KSFSLoadPackageManifestOperation(this, packageVersion, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    [UnityEngine.Scripting.Preserve]
    public FSRequestPackageVersionOperation RequestRemotePackageVersionAsync(bool appendTimeTicks, int timeout)
    {
        var operation = new KSFSRequestPackageVersionOperation(this, appendTimeTicks, timeout);
        OperationSystem.StartOperation(PackageName, operation);
        return operation;
    }

    #endregion
}
#endif