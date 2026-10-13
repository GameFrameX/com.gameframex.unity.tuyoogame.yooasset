<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX YooAsset

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#クイックスタート) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## プロジェクト概要

YooAsset は Unity3D 向けのリソース管理システムで、開発チームがゲームを迅速にデプロイおよび配信できるようにします。商業ゲームの様々なニーズを満たし、百万 DAU を持つ複数のゲーム製品で検証されています。

詳細は公式リポジトリをご覧ください：https://github.com/tuyoogame/YooAsset

## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：
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

   `scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

2. `manifest.json` の `dependencies` に直接追加：
   ```json
   {
      "com.gameframex.unity.tuyoogame.yooasset": "https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git"
   }
   ```
3. Unity の **Package Manager** で **Git URL** を使用して追加：`https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git`
4. リポジトリを Unity プロジェクトの `Packages` ディレクトリにクローンしてください。自動的に読み込まれます。
## ドキュメントとリソース

- [公式ドキュメント](https://gameframex.doc.alianblank.com)
- [YooAsset ドキュメント](https://github.com/tuyoogame/YooAsset)

## コミュニティとサポート

- QQグループ: [参加](https://qm.qq.com/q/3dIpogITg)

## 変更履歴

変更履歴は [Releases](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/releases) をご覧ください。


## 依存関係

| パッケージ | 説明 |
|----------|------|
| `com.unity.modules.assetbundle` | 1.0.0 |
| `com.unity.modules.unitywebrequest` | 1.0.0 |
| `com.unity.modules.unitywebrequestassetbundle` | 1.0.0 |
| `com.unity.scriptablebuildpipeline` | 2.1.0 |

## 登録即ディスパッチ機構（ミニゲーム WebPlay モード）

3.1.0 で追加。`Runtime/MiniGame/` のパブリック API により、ミニゲームプラットフォーム適応をホストパッケージから分離し、ホスト側はプラットフォーム非依存を保ちます：

- `IWebPlayModeFileSystemProvider`：アダプター契約。チャネル名（ChannelName）/ `Priority` / `CreateFileSystemParameters` を提供します。
- `WebPlayModeProviderContext`：初期化コンテキスト。`HostServerURL` / `FallbackHostServerURL` を保持します。
- `WebPlayModeFileSystemProviderRegistry`：冪等なレジストリ。`Register` / `Unregister` / `Resolve` に加え、asset 抽象層が利用する `CreateFileSystemParameters` ファサードを提供します。

解決ルール：

- 相互排他的な解決：`Priority` が最も高いプロバイダーが選択され、優先度が同じ場合は先に登録されたものが優先されます。初期化ごとに選択されるプラットフォームファイルシステムは 1 つだけです。
- レジストリは `SubsystemRegistration` フェーズで自動的にリセットされ、ドメインリロードに伴う二重登録を防ぎます。
- 登録がない場合、ファサードはデフォルトの WebGL ファイルシステムへフォールバックします。
- 各プラットフォーム適応パッケージ（WeChat、Douyin など）の Registrar は `BeforeSceneLoad` で自己登録され、初回の WebPlay モード初期化より前に実行されます——登録すればディスパッチされます。

## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
