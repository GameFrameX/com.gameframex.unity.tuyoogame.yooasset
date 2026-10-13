<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX YooAsset

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Project Overview

YooAsset is a resource management system for Unity3D that helps development teams quickly deploy and deliver games. It can meet the various needs of commercial games and has been validated by multiple game products with millions of DAU.

For more information, visit the official repository: https://github.com/tuyoogame/YooAsset

## Quick Start

### Installation

Choose one of the following methods:

1. Edit your Unity project's `Packages/manifest.json` and add the `scopedRegistries` section:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.tuyoogame.yooasset": "2.9.3"
     }
   }
   ```

   `scopes` controls which packages are resolved through this registry. Only packages whose names start with `com.gameframex` will be fetched from it.

2. Add to `manifest.json` dependencies:
   ```json
   {
      "com.gameframex.unity.tuyoogame.yooasset": "https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git"
   }
   ```
3. Use **Package Manager** in Unity with **Git URL**: `https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git`
4. Clone the repository into your Unity project's `Packages` directory. It will be loaded automatically.
## Documentation & Resources

- [Official Documentation](https://gameframex.doc.alianblank.com)
- [YooAsset Documentation](https://github.com/tuyoogame/YooAsset)

## Community & Support

- QQ Group: [Join](https://qm.qq.com/q/3dIpogITg)

## Changelog

See [Releases](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/releases) for changelog.


## Dependencies

| Package | Description |
|---------|-------------|
| `com.unity.modules.assetbundle` | 1.0.0 |
| `com.unity.modules.unitywebrequest` | 1.0.0 |
| `com.unity.modules.unitywebrequestassetbundle` | 1.0.0 |
| `com.unity.scriptablebuildpipeline` | 2.1.0 |

## Registry-Based Dispatch (MiniGame Web Play Mode)

Added in 3.1.0. The public API under `Runtime/MiniGame/` decouples mini-game platform adaptation from this host package, keeping it platform-agnostic:

- `IWebPlayModeFileSystemProvider`: provider contract exposing `ChannelName` / `Priority` / `CreateFileSystemParameters`.
- `WebPlayModeProviderContext`: initialization context carrying `HostServerURL` / `FallbackHostServerURL`.
- `WebPlayModeFileSystemProviderRegistry`: idempotent registry with `Register` / `Unregister` / `Resolve`, plus the `CreateFileSystemParameters` facade consumed by the asset abstraction layer.

Resolution rules:

- Mutual exclusion: the provider with the highest `Priority` wins; on a tie the first registered provider wins, so only one platform file system is created per initialization.
- The registry resets in the `SubsystemRegistration` phase, preventing duplicate registration across domain reloads.
- With no provider registered, the facade falls back to the default WebGL file system.
- Platform adapter packages (WeChat, Douyin, etc.) self-register their Registrar in `BeforeSceneLoad`, which runs earlier than the first Web play mode initialization — register means dispatch.

## License

See [LICENSE.md](LICENSE.md) for license information.
