# Unity Wireless Debug Manager

Unity Editor에서 Android 기기와 무선 디버깅을 쉽게 관리할 수 있는 도구입니다.

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
1. 도구 설치 → 위 방법 중 하나 선택
2. Tools의 Wireless Debug Manager 클릭
2. Android 설정 → 개발자 옵션 + 무선 디버깅 활성화  
3. 페어링 (1회) → IP, 포트, 6자리 코드 입력
4. 연결 (매번) → 핸드폰에서 제공되는 IP로 연결
5. 무선 개발 → Build And Run으로 케이블 없이 배포 및 테스트!
```

### ✅ 설치 확인
Unity 메뉴에서 **Tools → Wireless Debug Manager**가 나타나면 설치 완료!

## 📸 인터페이스 미리보기

### 메인 인터페이스
- 📊 **시스템 정보**: ADB 상태, 로컬 IP, Unity 버전
- 🔗 **페어링 섹션**: 원클릭 페어링 및 상태 확인
- 🔌 **연결 관리**: 기기 연결/해제 및 포트 설정
- 📱 **기기 목록**: 연결된 모든 기기 실시간 표시

### 고급 기능
- 🛠️ **유틸리티**: ADB 재시작, 버전 확인, 전체 해제
- 📖 **도움말**: 상세한 설정 가이드 및 문제 해결
- 🌐 **언어 설정**: 실시간 한국어/영어 전환

## 📋 시스템 요구사항

| 구분 | 요구사항 |
|------|----------|
| **Unity** | 2019.4 LTS 이상 (테스트: 6000.0.47f1) |
| **Android** | Android 11+ (API 30 이상) |
| **플랫폼** | Windows, macOS, Linux |
| **네트워크** | 같은 WiFi 연결 필수 |

## 📖 상세 가이드

| Language | Quick Guide | Detailed Guide |
|----------|-------------|----------------|
| 🇰🇷 **한국어** | 위 빠른 시작 참고 | [📚 상세 설정 가이드](Documentation/setup-guide-ko.md) |
| 🇺🇸 **English** | See quick start above | [📚 Detailed Setup Guide](Documentation/setup-guide-en.md) |

모든 문서는 [Documentation 폴더](Documentation/)에서 확인할 수 있습니다.

## 🛡️ 플랫폼별 지원

| 플랫폼 | 페어링 | 연결 | 특징 |
|---------|--------|------|------|
| **Windows** | ✅ 수동 | ✅ 자동 | PowerShell 명령어 제공 |
| **macOS** | ✅ 자동 | ✅ 자동 | 완전 자동화 지원 |
| **Linux** | ✅ 자동 | ✅ 자동 | 터미널 명령어 제공 |

## 🔧 문제 해결

### 연결이 안될 때
1. **"ADB 재시작"** 버튼 클릭
2. **"연결 상태 새로고침"** 실행
3. Android에서 무선 디버깅 재활성화
4. [상세 문제 해결 가이드](Documentation/setup-guide-ko.md#-문제-해결) 참고

### 자주 묻는 질문
- **Q: 페어링은 언제 해야 하나요?** A: 최초 1회만! 이후엔 연결만 하면 됩니다.
- **Q: 여러 기기를 연결할 수 있나요?** A: 네, 동시에 여러 기기 연결 가능합니다.
- **Q: 회사 WiFi에서 안되요.** A: 방화벽 설정을 확인하거나 IT팀에 문의하세요.

## 🤝 기여하기

버그 리포트, 기능 제안, 번역 개선은 언제나 환영합니다!

- **Issues**: [GitHub Issues](https://github.com/ovthemoon/unity-wireless-debug-manager/issues)
- **Pull Requests**: 코드 개선 및 새 기능 추가
- **번역**: 새로운 언어 지원 추가

## 📄 라이선스

MIT License - 자유롭게 사용, 수정, 배포할 수 있습니다.

---

**💡 팁**: 한 번 설정하면 케이블 없이 계속 개발할 수 있어요! 무선의 자유를 만끽하세요! 🎉