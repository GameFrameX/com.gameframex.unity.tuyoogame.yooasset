<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX YooAsset

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#快速開始) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 項目簡介

YooAsset 是一套用於 Unity3D 的資源管理系統，用於幫助研發團隊快速部署和交付遊戲。它可以滿足商業化遊戲的各類需求，並且經歷多款百萬 DAU 遊戲產品的驗證。

更多資訊請了解官方主頁：https://github.com/tuyoogame/YooAsset

## 快速開始

### 安裝

選擇以下任一方式：

1. 編輯 Unity 專案的 `Packages/manifest.json`，添加 `scopedRegistries` 部分：
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

   `scopes` 控制哪些套件透過此註冊表解析。只有以 `com.gameframex` 開頭的套件才會從這個註冊表取得。

2. 直接在 `manifest.json` 的 `dependencies` 節點下添加以下內容：
   ```json
   {
      "com.gameframex.unity.tuyoogame.yooasset": "https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git"
   }
   ```
3. 在 Unity 的 `Package Manager` 中使用 `Git URL` 的方式添加庫，地址為：`https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git`
4. 直接下載倉庫放置到 Unity 專案的 `Packages` 目錄下，會自動載入識別。
## 文檔與資源

- [官方文檔](https://gameframex.doc.alianblank.com)
- [YooAsset 文檔](https://github.com/tuyoogame/YooAsset)

## 社區與支援

- QQ群: [加入](https://qm.qq.com/q/3dIpogITg)

## 更新日誌

查看 [Releases](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/releases) 了解更新日誌。


## 依賴

| 套件 | 說明 |
|------|------|
| `com.unity.modules.assetbundle` | 1.0.0 |
| `com.unity.modules.unitywebrequest` | 1.0.0 |
| `com.unity.modules.unitywebrequestassetbundle` | 1.0.0 |
| `com.unity.scriptablebuildpipeline` | 2.1.0 |

## 註冊即分發機制（小遊戲 WebPlay 模式）

3.1.0 新增。`Runtime/MiniGame/` 公共 API 將小遊戲平台適配與宿主套件解耦，宿主套件自身保持平台無關：

- `IWebPlayModeFileSystemProvider`：適配器契約，提供渠道名（ChannelName）/ `Priority` / `CreateFileSystemParameters`。
- `WebPlayModeProviderContext`：初始化上下文，攜帶 `HostServerURL` / `FallbackHostServerURL`。
- `WebPlayModeFileSystemProviderRegistry`：冪等註冊表，提供 `Register` / `Unregister` / `Resolve`，以及供 asset 抽象層消費的 `CreateFileSystemParameters` 門面。

解析規則：

- 互斥解析：`Priority` 最高者勝出，同優先級時以註冊順序首個為準，一次初始化只會選中一個平台檔案系統。
- 註冊表在 `SubsystemRegistration` 階段自動 Reset，防止跨網域重載時的重複註冊。
- 無任何註冊時，門面回落到預設 WebGL 檔案系統。
- 各平台適配套件（微信、抖音等）的 Registrar 在 `BeforeSceneLoad` 自註冊，早於首次 WebPlay 模式初始化——註冊即分發。

## 開源協議

詳見 [LICENSE.md](LICENSE.md) 檔案。
