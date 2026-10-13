<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# GameFrameX YooAsset

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.tuyoogame.yooasset)](https://github.com/GameFrameX/com.gameframex.unity.tuyoogame.yooasset/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#빠른-시작) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 프로젝트 개요

YooAsset은 Unity3D용 리소스 관리 시스템으로, 개발 팀이 게임을 빠르게 배포하고 전달할 수 있도록 도와줍니다. 상업용 게임의 다양한 요구를 충족하며, 수백만 DAU를 가진 여러 게임 제품을 통해 검증되었습니다.

자세한 내용은 공식 리포지토리를 참조하세요: https://github.com/tuyoogame/YooAsset

## 빠른 시작

### 설치

다음 방법 중 하나를 선택하세요:

1. Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:
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

   `scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

2. `manifest.json`의 `dependencies`에 직접 추가:
   ```json
   {
      "com.gameframex.unity.tuyoogame.yooasset": "https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git"
   }
   ```
3. Unity의 **Package Manager**에서 **Git URL**을 사용하여 추가: `https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset.git`
4. 리포지토리를 Unity 프로젝트의 `Packages` 디렉토리에 클론하세요. 자동으로 로드됩니다.
## 문서 및 자료

- [공식 문서](https://gameframex.doc.alianblank.com)
- [YooAsset 문서](https://github.com/tuyoogame/YooAsset)

## 커뮤니티 및 지원

- QQ 그룹: [가입](https://qm.qq.com/q/3dIpogITg)

## 변경 로그

변경 로그는 [Releases](https://github.com/gameframex/com.gameframex.unity.tuyoogame.yooasset/releases)에서 확인하세요.


## 의존성

| 패키지 | 설명 |
|--------|------|
| `com.unity.modules.assetbundle` | 1.0.0 |
| `com.unity.modules.unitywebrequest` | 1.0.0 |
| `com.unity.modules.unitywebrequestassetbundle` | 1.0.0 |
| `com.unity.scriptablebuildpipeline` | 2.1.0 |

## 등록 즉 디스패치(등록 기반 분기) (미니게임 WebPlay 모드)

3.1.0에서 추가되었습니다. `Runtime/MiniGame/`의 공개 API가 미니게임 플랫폼 적응을 호스트 패키지에서 분리하여, 호스트 패키지는 플랫폼 비의존 상태를 유지합니다:

- `IWebPlayModeFileSystemProvider`: 어댑터 계약으로, 채널 이름(ChannelName) / `Priority` / `CreateFileSystemParameters`를 제공합니다.
- `WebPlayModeProviderContext`: 초기화 컨텍스트로, `HostServerURL` / `FallbackHostServerURL`을 전달합니다.
- `WebPlayModeFileSystemProviderRegistry`: 멱등 레지스트리로, `Register` / `Unregister` / `Resolve`와 asset 추상화 계층이 소비하는 `CreateFileSystemParameters` 퍼사드를 제공합니다.

해석 규칙:

- 상호 배제적 해석: `Priority`가 가장 높은 프로바이더가 선택되며, 우선순위가 같으면 먼저 등록된 프로바이더가 우선합니다. 초기화 시 선택되는 플랫폼 파일 시스템은 하나뿐입니다.
- 레지스트리는 `SubsystemRegistration` 단계에서 자동으로 리셋되어 도메인 리로드 시 이중 등록을 방지합니다.
- 등록된 프로바이더가 없으면 퍼사드는 기본 WebGL 파일 시스템으로 폴백합니다.
- 각 플랫폼 적응 패키지(WeChat, Douyin 등)의 Registrar는 `BeforeSceneLoad`에서 자동 등록되며, 첫 번째 WebPlay 모드 초기화보다 먼저 실행됩니다 — 등록하면 디스패치됩니다.

## 라이선스

자세한 내용은 [LICENSE.md](LICENSE.md) 파일을 참조하세요.
