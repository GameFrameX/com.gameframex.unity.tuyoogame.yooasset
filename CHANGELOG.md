## [2.9.3](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.9.2...2.9.3) (2026-06-02)


### Bug Fixes

* **buffer:** 移除 Conditional("DEBUG") 使边界检查始终生效 ([5bca514](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/5bca514aa9e81f543026b3d24098a7bf20b34bc7))
* **cache:** 写入缓存前确保目录存在、失败时回滚并精确删除文件 ([1d5d2bf](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/1d5d2bffec2a4499a3f796c4a2be86b53c9375e1))
* **debugger:** 未知远程命令改为警告日志而非抛异常 ([757c4dc](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/757c4dc7851efc9ebe005d21c077d3ca00bdaa0c))
* **download:** FileStream 打开失败时清理并重抛异常 ([b55ba75](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/b55ba7592f0af7ca361c576d064117bffac9ef5f))
* **download:** UNITY_IPHONE 改为 UNITY_IOS ([d545b8a](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/d545b8a9ea4e49d85979a8744afd5d0228558dc4))
* **download:** WebRequestCounter 添加线程安全锁 ([532ee51](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/532ee51e019eedb53ffced25e6815e38e22be30d))
* **download:** 合并下载器前检查对方运行状态 ([d14d78e](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/d14d78ec1da318b6d213d4037829025299832574))
* **download:** 防止进度计算除零 ([8a7f293](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/8a7f293e1aedf3eeb0f1c18146e0be56b2cfac38))
* **driver:** 修正拼写错误 LastestUpdateFrame → LatestUpdateFrame ([9b092e8](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/9b092e8578d5c0f6c3f8d123576203eb6b5233fc))
* **driver:** 移除 OnApplicationQuit 的 UNITY_EDITOR 条件编译 ([2ef2044](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/2ef20447c8612ae555c2c4531ab920b4b43b1115))
* **filesystem:** 文件验证异常时输出警告日志 ([791bda8](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/791bda82acd591c69dd8e46554f3745e4cf10213))
* **operation:** 防止 IsBusy 在 _watch 未初始化时空引用 ([ab7fb15](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/ab7fb15ae3e165b8ec0cdb6b31da6f0a124d7ac6))
* **package:** 防御性检查改为始终生效并清理失败遗留 ([36ceb1b](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/36ceb1b6f65e8545e798bbe4601519c61c30c3e5))
* **params:** 使用索引器替代 Dictionary.Add 防止重复 key 异常 ([ff39413](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/ff3941363b01a1aab65340f354102a20edbb617f))
* **utility:** 修复 ThreadStatic 字段初始化问题 ([354ed84](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/354ed845b784cd5626da3eb2043e8b9b5b64324f))
* **webfs:** 移除遗留 Debug.LogError 调试输出 ([99b6f39](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/99b6f39a9bba66963f9ecdbb2e381860f75e1c1a))


### Performance Improvements

* **report:** 构建报告查找改用字典索引 ([2fcf0b4](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/2fcf0b47c45afcb72fe549c2ab261ff654e8f5cc))

## [2.9.2](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.9.1...2.9.2) (2026-05-28)


### Bug Fixes

* **ci:** 统一 .github 工作流配置 ([7d13f4a](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/7d13f4aac3053167b4609d87ed7c1f313676ede2))

## [2.9.1](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.9.0...2.9.1) (2026-05-16)


### Bug Fixes

* **PlayModeHelper:** 支持从所有已加载程序集中搜索文件系统类类型 ([7dce4b4](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/7dce4b49bd8c06dc2c233ffbdaa4a982493ffded))

# [2.9.0](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.8.1...2.9.0) (2026-04-10)


### Bug Fixes

* **DefaultBuildinFileSystem:** 未找到内置目录文件时操作视为成功 ([88ee8c3](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/88ee8c3c35e813a47d697756e42952e866188d5a))
* **FileSystem:** 当StreamingAssets根目录不存在时跳过生成目录 ([4a23518](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/4a235189b1f6fe2e1cd3e8a89ac9f27407018ea3))
* 在编辑器环境下禁用KSWASM预加载并调整文件缓存路径 ([bb7414f](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/bb7414f32c65d57a17592f7bad0bf05fca083ce9))


### Features

* **filesystem:** 新增快手小游戏文件系统支持 ([b6361ab](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/b6361ab81e87c65bfd3af6353d64ff91860e7539))
* 新增快手文件系统相关元数据文件 ([993489c](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/993489c75e4498a6b772cec3bdf2b14ca247ea5e))
* 添加快手和抖音小游戏平台支持 ([376b571](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/376b571e0b61cd35862e5036a631b3f3927aa14b))

## [2.8.1](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.8.0...2.8.1) (2026-03-31)


### Bug Fixes

* **编译条件:** 将微信小游戏条件编译符号从WECHAT_MINI_GAME改为ENABLE_WECHAT_MINI_GAME ([22ce828](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/22ce828db5b3ad59bd3cc3cbb14f78a7682bdbc6))

# [2.8.0](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.7.0...2.8.0) (2026-03-29)


### Features

* **文件系统:** 添加抖音小游戏配置处理器 ([7930af8](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/7930af8dc7496e0bc92e1ec5e31e36e5ca94b395))

# [2.7.0](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.6.0...2.7.0) (2026-03-27)


### Features

* **微信小程序:** 添加 WeChatConfigHandler 以优化预加载性能 ([829c732](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/829c7323a124f11d6cb3a794d7913f7adc9e1618))

# [2.6.0](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.5.1...2.6.0) (2026-03-25)


### Features

* **FGUI:** FGUI的打包规则 ([d44f300](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/d44f3002b21d6f1a7490607fb8ab6e2bb04ebe6d))

## [2.5.1](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.5.0...2.5.1) (2026-03-16)


### Bug Fixes

* **OperationSystem:** 将 GetPackageName 方法从 internal 改为 public ([9ddcdf7](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/9ddcdf7fced77c3f12e9572daa12c04e4d438098))

# [2.5.0](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.4.0...2.5.0) (2026-03-16)


### Features

* **ResourceManager:** 添加资源加载耗时统计功能 ([fa5cf5f](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/fa5cf5fdc2beda52f5f926e033c738b7b6ea881f))

# [2.4.0](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.3.3...2.4.0) (2026-03-14)


### Features

* **DownloadSystem:** 添加 URL 时间戳参数支持 ([a4cb97f](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/a4cb97fb55d2d56eda699e317d96c4073a8db44c))
* **FileSystem:** 使用时间戳参数替代 HTTP 缓存头 ([6c71656](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/6c7165665cb8febd419da9a4695fd296be27a694))

## [2.3.3](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.3.2...2.3.3) (2026-03-12)


### Bug Fixes

* **FileSystem:** 统一微信文件系统URL生成逻辑 ([d377664](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/d377664037a782567b4b9b51a22a02e8835bc711))

## [2.3.2](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.3.1...2.3.2) (2026-03-11)


### Bug Fixes

* 移除未使用的命名空间以简化代码 ([b689da8](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/b689da8d4fba08899bc269675332614ca67d996d))

## [2.3.1](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/compare/2.3.0...2.3.1) (2026-03-06)


### Bug Fixes

* 为多个类型添加Preserve属性以防止代码裁剪 ([632480a](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/commit/632480aa4a006da99cf1f41f0208b7affa7167dc))

# CHANGELOG

All notable changes to this package will be documented in this file.

## [2.2.4-preview] - 2024-08-15

### Fixed

- 修复了HostPlayMode初始化卡死的问题。

## [2.2.3-preview] - 2024-08-13

### Fixed

- (#311) 修复了断点续传下载器极小概率报错 : “416 Range Not Satisfiable”

### Improvements

- 原生文件构建管线支持原生文件加密。

- HostPlayMode模式下内置文件系统初始化参数可以为空。

- 场景加载增加了LocalPhysicsMode参数来控制物理运行模式。

- 默认的内置文件系统和缓存文件系统增加解密方法。

  ```csharp
  /// <summary>
  /// 创建默认的内置文件系统参数
  /// </summary>
  /// <param name="decryptionServices">加密文件解密服务类</param>
  /// <param name="verifyLevel">缓存文件的校验等级</param>
  /// <param name="rootDirectory">内置文件的根路径</param>
  public static FileSystemParameters CreateDefaultBuildinFileSystemParameters(IDecryptionServices decryptionServices, EFileVerifyLevel verifyLevel, string rootDirectory);
  
  /// <summary>
  /// 创建默认的缓存文件系统参数
  /// </summary>
  /// <param name="remoteServices">远端资源地址查询服务类</param>
  /// <param name="decryptionServices">加密文件解密服务类</param>
  /// <param name="verifyLevel">缓存文件的校验等级</param>
  /// <param name="rootDirectory">文件系统的根目录</param>
  public static FileSystemParameters CreateDefaultCacheFileSystemParameters(IRemoteServices remoteServices, IDecryptionServices decryptionServices, EFileVerifyLevel verifyLevel, string rootDirectory);
  ```

## [2.2.2-preview] - 2024-07-31

### Fixed

- (#321) 修复了在Unity2022里编辑器下离线模式运行失败的问题。
- (#325) 修复了在Unity2019里编译报错问题。

## [2.2.1-preview] - 2024-07-10

统一了所有PlayMode的初始化逻辑，EditorSimulateMode和OfflinePlayMode初始化不再主动加载资源清单！

### Added

- 新增了IFileSystem.ReadFileData方法，支持原生文件自定义获取文本和二进制数据。

### Improvements

- 优化了DefaultWebFileSystem和DefaultBuildFileSystem文件系统的内部初始化逻辑。

## [2.2.0-preview] - 2024-07-07

重构了运行时代码，新增了文件系统接口（IFileSystem）方便开发者扩展特殊需求。

新增微信小游戏文件系统示例代码，详细见Extension Sample/Runtime/WechatFileSystem

### Added

- 新增了ResourcePackage.DestroyAsync方法

- 新增了FileSystemParameters类帮助初始化文件系统

  内置了编辑器文件系统参数，内置文件系统参数，缓存文件系统参数，Web文件系统参数。

  ```csharp
  public class FileSystemParameters
  {
      /// <summary>
      /// 文件系统类
      /// </summary>
      public string FileSystemClass { private set; get; }
      
      /// <summary>
      /// 文件系统的根目录
      /// </summary>
      public string RootDirectory { private set; get; }   
      
      /// <summary>
      /// 添加自定义参数
      /// </summary>
      public void AddParameter(string name, object value)    
  }
  ```

### Changed

- 重构了InitializeParameters初始化参数
- 重命名YooAssets.DestroyPackage方法为RemovePackage
- 重命名ResourcePackage.UpdatePackageVersionAsync方法为RequestPackageVersionAsync
- 重命名ResourcePackage.UnloadUnusedAssets方法为UnloadUnusedAssetsAsync
- 重命名ResourcePackage.ForceUnloadAllAssets方法为UnloadAllAssetsAsync
- 重命名ResourcePackage.ClearUnusedCacheFilesAsync方法为ClearUnusedBundleFilesAsync
- 重命名ResourcePackage.ClearAllCacheFilesAsync方法为ClearAllBundleFilesAsync

### Removed

- 移除了YooAssets.Destroy方法
- 移除了YooAssets.SetDownloadSystemClearFileResponseCode方法
- 移除了YooAssets.SetCacheSystemDisableCacheOnWebGL方法
- 移除了ResourcePackage.GetPackageBuildinRootDirectory方法
- 移除了ResourcePackage.GetPackageSandboxRootDirectory方法
- 移除了ResourcePackage.ClearPackageSandbox方法
- 移除了IBuildinQueryServices接口
- 移除了IDeliveryLoadServices接口
- 移除了IDeliveryQueryServices接口


## [2.1.2] - 2024-05-16

SBP库依赖版本升级至2.1.3

### Fixed

- (#236) 修复了资源配置界面AutoCollectShader复选框没有刷新的问题。
- (#244) 修复了导入器在安卓平台导入本地下载的资源失败的问题。
- (#268) 修复了挂起场景未解除状态前无法卸载的问题。
- (#269) 优化场景挂起流程，支持中途取消挂起操作。
- (#276) 修复了HostPlayMode模式下，如果内置清单是最新版本，每次运行都会触发拷贝行为。
- (#289) 修复了Unity2019版本脚本IWebRequester编译报错。
- (#295) 解决了在安卓移动平台，华为和三星真机上有极小概率加载资源包失败 : Unable to open archive file

### Added

- 新增GetAllCacheFileInfosOperation()获取缓存文件信息的方法。

- 新增LoadSceneSync()同步加载场景的方法。

- 新增IIgnoreRule接口，资源收集流程可以自定义。

- 新增IWechatQueryServices接口，用于微信平台本地文件查询。

  后续将会通过虚拟文件系统来支持！

### Changed

- 调整了UnloadSceneOperation代码里场景的卸载顺序。

### Improvements

- 优化了资源清单的解析过程。
- 移除资源包名里的空格字符。
- 支持华为鸿蒙系统。

## [2.1.1] - 2024-01-17

### Fixed

- (#224)  修复了编辑器模式打包时 SimulateBuild 报错的问题。
- (#223)  修复了资源构建界面读取配置导致的报错问题。

### Added

- 支持共享资源打包规则，可以定制化独立的构建规则。

  ```c#
  public class BuildParameters
  {
     /// <summary>
      /// 是否启用共享资源打包
      /// </summary>
      public bool EnableSharePackRule = false; 
  }
  ```

- 微信小游戏平台，资源下载器支持底层缓存查询。

## [2.1.0] - 2023-12-27

升级了 Scriptable build pipeline (SBP) 的版本，来解决图集引用的精灵图片冗余问题。

### Fixed

- (#195) 修复了在EditorPlayMode模式下，AssetHandle.GetDownloadStatus()发生异常的问题。
- (#201) 修复了断点续传失效的问题。
- (#202) 修复了打包参数FileNameStyle设置为BundleName后，IQueryServices会一直返回true的问题。
- (#205) 修复了HybridCLR插件里创建资源下载器触发的异常。
- (#210) 修复了DownloaderOperation在未开始下载前，内部的PackageName为空的问题。
- (#220) 修复了资源收集界面关闭后，撤回操作还会生效的问题。
- 修复了下载器合并后重新计算下载字节数不正确的问题。

### Improvements

- (#198) 资源收集界面禁用的分组不再检测合法性。
- (#203) 资源构建类容许自定义打包的输出目录。
- 资源构建报告增加未依赖的资源信息列表。

### Changed

- IBuildinQueryServices和IDeliveryQueryServices查询方法变更。

  ```c#
      public interface IBuildinQueryServices
      {
          /// <summary>
          /// 查询是否为应用程序内置的资源文件
          /// </summary>
          /// <param name="packageName">包裹名称</param>
          /// <param name="fileName">文件名称（包含文件的后缀格式）</param>
          /// <param name="fileCRC">文件哈希值</param>
          /// <returns>返回查询结果</returns>
          bool Query(string packageName, string fileName, string fileCRC);
      }
  
     	public interface IDeliveryQueryServices
      {
          /// <summary>
          /// 查询是否为开发者分发的资源文件
          /// </summary>
          /// <param name="packageName">包裹名称</param>
          /// <param name="fileName">文件名称（包含文件的后缀格式）</param>
          /// <param name="fileCRC">文件哈希值</param>
          /// <returns>返回查询结果</returns>
          bool Query(string packageName, string fileName, string fileCRC);
      }
  ```

  

### Removed

- (#212)  移除了构建报告里的资源冗余信息列表。
