<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX YooAsset

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使

<br />

[文档](https://gameframex.doc.alianblank.com) · [快速开始](#快速开始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 项目简介

YooAsset 是一套用于 Unity3D 的资源管理系统，用于帮助研发团队快速部署和交付游戏。它可以满足商业化游戏的各类需求，并且经历多款百万 DAU 游戏产品的验证。

更多信息请了解官方主页：https://github.com/tuyoogame/YooAsset

## 快速开始

### 安装

选择以下任一方式：

1. 编辑 Unity 项目的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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

   `scopes` 控制哪些包通过此注册表解析。只有以 `com.gameframex` 开头的包才会从这个注册表获取。

2. 直接在 `manifest.json` 的 `dependencies` 节点下添加以下内容：
   ```json
   {
      "com.gameframex.unity.tuyoogame.yooasset": "https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加库，地址为：`https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git`
4. 直接下载仓库放置到 Unity 项目的 `Packages` 目录下，会自动加载识别。
## 文档与资源

- [官方文档](https://gameframex.doc.alianblank.com)
- [YooAsset 文档](https://github.com/tuyoogame/YooAsset)

## 社区与支持

- QQ群: [加入](https://qm.qq.com/q/3dIpogITg)

## 更新日志

查看 [Releases](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/releases) 了解更新日志。


## 依赖

| 包 | 说明 |
|----|------|
| `com.unity.modules.assetbundle` | 1.0.0 |
| `com.unity.modules.unitywebrequest` | 1.0.0 |
| `com.unity.modules.unitywebrequestassetbundle` | 1.0.0 |
| `com.unity.scriptablebuildpipeline` | 2.1.0 |

## 注册即分发机制（小游戏 WebPlay 模式）

3.1.0 新增。`Runtime/MiniGame/` 公共 API 将小游戏平台适配与宿主包解耦，宿主包自身保持平台无关：

- `IWebPlayModeFileSystemProvider`：适配器契约，提供渠道名（ChannelName）/ `Priority` / `CreateFileSystemParameters`。
- `WebPlayModeProviderContext`：初始化上下文，携带 `HostServerURL` / `FallbackHostServerURL`。
- `WebPlayModeFileSystemProviderRegistry`：幂等注册表，提供 `Register` / `Unregister` / `Resolve`，以及供 asset 抽象层消费的 `CreateFileSystemParameters` 门面。

解析规则：

- 互斥解析：`Priority` 最高者胜出，同优先级时以注册顺序首个为准，一次初始化只会选中一个平台文件系统。
- 注册表在 `SubsystemRegistration` 阶段自动 Reset，防止跨域重载时的重复注册。
- 无任何注册时，门面回落到默认 WebGL 文件系统。
- 各平台适配包（微信、抖音等）的 Registrar 在 `BeforeSceneLoad` 自注册，早于首次 WebPlay 模式初始化——注册即分发。

## 开源协议

详见 [LICENSE.md](LICENSE.md) 文件。
