# Unity Wireless Debug Manager

**🌍 Languages**: [한국어](#korean) | [English](#english) | [Documentation~](/Documentation~/)

---

<details id="korean" open>
<summary><strong>🇰🇷 한국어 (Korean)</strong></summary>

Unity Editor에서 Android 기기와 무선 디버깅을 쉽게 관리할 수 있는 올인원 도구입니다.

## 🎯 개요

케이블 없이 Unity에서 Android 앱을 빌드하고 배포할 수 있도록 ADB 무선 연결을 간편하게 관리합니다. 복잡한 명령어 없이 GUI로 페어링부터 연결까지 모든 과정을 처리할 수 있습니다.

## ✨ 주요 기능

### 🔗 **스마트 페어링 시스템**
- **원클릭 페어링**: Android 페어링 코드를 입력만 하면 자동 연결
- **자동 실패 복구**: 페어링 실패시 플랫폼별 수동 명령어 제공
- **페어링 상태 확인**: 연결된 기기 실시간 모니터링

### 📱 **멀티 디바이스 관리**
- **여러 기기 동시 연결**: 최대 16개 Android 기기 관리
- **개별 연결 제어**: 특정 기기만 연결/해제 가능
- **연결 상태 표시**: 실시간 연결 상태 및 IP 주소 확인

### 🛠️ **ADB 통합 관리**
- **자동 ADB 감지**: Unity 내장 SDK 및 시스템 ADB 자동 탐지
- **ADB 상태 관리**: 재시작, 버전 확인, 서버 상태 모니터링
- **경로 복사**: ADB 경로를 클립보드로 복사

### 🌐 **다국어 지원**
- **한국어/English**: 시스템 언어 자동 감지
- **언어 전환**: 실시간 언어 변경 지원
- **현지화된 오류 메시지**: 언어별 상세한 오류 설명

### 🔧 **개발자 친화적 기능**
- **시스템 정보 표시**: 로컬 IP, Unity 버전, 플랫폼 정보
- **상세한 가이드**: 단계별 설정 및 문제 해결 가이드
- **스크롤 가능 UI**: 많은 기기도 편리하게 관리

## 🚀 빠른 시작

### 📦 설치 방법

#### 방법 1: Unity Package Manager (UPM) ⭐ 권장
1. Unity에서 **Window → Package Manager** 열기
2. **"+" → Add package from git URL** 선택
3. URL 입력: `https://github.com/ovthemoon/unity-wireless-debug-manager.git`

#### 방법 2: Git Clone 
```bash
git clone https://github.com/ovthemoon/unity-wireless-debug-manager.git
cp unity-wireless-debug-manager/Editor/WirelessDebugManager.cs /path/to/Assets/Editor/
```

#### 방법 3: 수동 다운로드
1. [Releases](https://github.com/ovthemoon/unity-wireless-debug-manager/releases)에서 최신 버전 다운로드
2. `WirelessDebugManager.cs`를 Unity 프로젝트의 `Assets/Editor/` 폴더에 복사

### 🎯 사용 흐름

```
1️⃣ 도구 설치 → 위 방법 중 하나 선택
2️⃣ 상단의 Tools - Wireless Debug Manager 클릭
3️⃣ Android 설정 → 개발자 옵션 + 무선 디버깅 활성화  
4️⃣ 페어링 (1회) → "페어링 코드로 기기페어링" 활성화 후 IP, 포트, 6자리 코드 입력
5️⃣ 연결 (매번) → 무선 디버깅 화면의 IP 주소 및 포트를 에디터에 작성 후 연결
6️⃣ 무선 디버깅 → Build And Run으로 케이블 없이 배포!
```

### ✅ 설치 확인
Unity 메뉴에서 **Tools → Wireless Debug Manager**가 나타나면 설치 완료!

## 🚀 빠른 시작

### 📦 설치 방법

#### 방법 1: Unity Package Manager (UPM) ⭐ 권장
1. Unity에서 **Window → Package Manager** 열기
2. **"+" → Add package from git URL** 선택
3. URL 입력: `https://github.com/ovthemoon/unity-wireless-debug-manager.git`

#### 방법 2: Git Clone 
```bash
git clone https://github.com/ovthemoon/unity-wireless-debug-manager.git
cp unity-wireless-debug-manager/Editor/WirelessDebugManager.cs /path/to/Assets/Editor/
```

#### 방법 3: 수동 다운로드
1. https://github.com/ovthemoon/unity-wireless-debug-manager에서 다운로드
2. `WirelessDebugManager.cs`,`WirelessDebugHelpWindow.cs` 를 Unity 프로젝트의 `Assets/Editor/` 폴더에 복사

### 🎯 사용 흐름

```
1. 도구 설치 → 위 방법 중 하나 선택
2. 상단의 Tools - Wireless Debug Manager 클릭
3. Android 설정 → 개발자 옵션 + 무선 디버깅 활성화  
4. 페어링 (1회) → IP, 포트, 6자리 코드 입력
5. 연결 (매번) → IP:5555로 연결
6. 무선 디버깅 → Build And Run으로 케이블 없이 배포!
```

### ✅ 설치 확인
Unity 메뉴에서 **Tools → Wireless Debug Manager**가 나타나면 설치 완료!

## 📋 시스템 요구사항

| 구분 | 요구사항 |
|------|----------|
| **Unity** | 2019.4 LTS 이상 (테스트: 6000.0.47f1) |
| **Android** | Android 11+ (API 30 이상) |
| **플랫폼** | Windows, macOS, Linux |
| **네트워크** | 같은 WiFi 연결 필수 |

## 📖 상세 가이드

- 🇰🇷 [한국어 상세 설정 가이드](Documentation~/setup-guide-ko.md)
- 📚 [모든 문서 보기](Documentation~/)

## 🔧 빠른 문제 해결

- **연결 안됨**: "ADB 재시작" 버튼 클릭
- **페어링 실패**: 6자리 코드 재확인
- **기기 인식 안됨**: 무선 디버깅 재활성화

💡 **팁**: 한 번 설정하면 케이블 없이 계속 개발 가능!

</details>

---

<details id="english">
<summary><strong>🇺🇸 English</strong></summary>

All-in-one Unity Editor tool for easy Android wireless debugging management.

## 🎯 Overview

Manage ADB wireless connections seamlessly to build and deploy Android apps from Unity without cables. Handle everything from pairing to connection through a simple GUI without complex commands.

## ✨ Key Features

### 🔗 **Smart Pairing System**
- **One-click Pairing**: Automatic connection with just Android pairing code input
- **Auto Recovery**: Platform-specific manual commands when auto-pairing fails
- **Pairing Status Check**: Real-time monitoring of connected devices

### 📱 **Multi-Device Management**
- **Multiple Device Support**: Manage up to 16 Android devices simultaneously
- **Individual Control**: Connect/disconnect specific devices
- **Status Display**: Real-time connection status and IP address monitoring

### 🛠️ **Integrated ADB Management**
- **Auto ADB Detection**: Automatically detect Unity built-in SDK and system ADB
- **ADB Status Control**: Restart, version check, server status monitoring
- **Path Copy**: Copy ADB path to clipboard

### 🌐 **Multi-language Support**
- **Korean/English**: Automatic system language detection
- **Language Switching**: Real-time language change support
- **Localized Error Messages**: Detailed error descriptions in each language

### 🔧 **Developer-Friendly Features**
- **System Information**: Local IP, Unity version, platform info display
- **Detailed Guides**: Step-by-step setup and troubleshooting guides
- **Scrollable UI**: Convenient management even with many devices

## 🚀 Quick Start

### 📦 Installation Methods

#### Method 1: Unity Package Manager (UPM) ⭐ Recommended
1. Open **Window → Package Manager** in Unity
2. Select **"+" → Add package from git URL**
3. Enter URL: `https://github.com/ovthemoon/unity-wireless-debug-manager.git`

#### Method 2: Git Clone
```bash
git clone https://github.com/ovthemoon/unity-wireless-debug-manager.git
cp unity-wireless-debug-manager/Editor/WirelessDebugManager.cs /path/to/Assets/Editor/
```

#### Method 3: Manual Download
1. Download latest version from [Releases](https://github.com/ovthemoon/unity-wireless-debug-manager/releases)
2. Copy `WirelessDebugManager.cs` to `Assets/Editor/` folder in your Unity project

### 🎯 Usage Flow

```
1️⃣ Install Tool → Choose one method above
2️⃣ Click Tools - Wireless Debug Manager in Unity menu
3️⃣ Android Setup → Enable Developer Options + Wireless Debugging
4️⃣ Pairing (One-time) → Enable "Pair device with pairing code" then enter IP, port, 6-digit code
5️⃣ Connect (Every time) → Enter IP address & port from wireless debugging screen into editor and connect
6️⃣ Wireless Debugging → Build And Run without cables!
```

### ✅ Installation Check
Installation complete when **Tools → Wireless Debug Manager** appears in Unity menu!

## 📋 System Requirements

| Component | Requirements |
|-----------|-------------|
| **Unity** | 2019.4 LTS or later (Tested: 6000.0.47f1) |
| **Android** | Android 11+ (API 30 or later) |
| **Platform** | Windows, macOS, Linux |
| **Network** | Same WiFi connection required |

## 📖 Detailed Guides

- 🇺🇸 [English Detailed Setup Guide](Documentation~/setup-guide-en.md)
- 📚 [View All Documentation~](Documentation~/)

## 🔧 Quick Troubleshooting

- **Connection Failed**: Click "Restart ADB" button
- **Pairing Failed**: Recheck 6-digit code
- **Device Not Recognized**: Re-enable wireless debugging

💡 **Tip**: Once set up, develop wirelessly forever!

</details>

---

## 🤝 Contributing

Bug reports, feature requests, and translation improvements are always welcome!

- **Issues**: [GitHub Issues](https://github.com/ovthemoon/unity-wireless-debug-manager/issues)
- **Pull Requests**: Code improvements and new features
- **Translations**: Add new language support

## 📄 License

MIT License - Feel free to use, modify, and distribute.