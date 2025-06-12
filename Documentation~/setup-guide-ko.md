# Unity Wireless Debug Manager - 설정 가이드

Unity에서 Android 기기와 무선 디버깅을 설정하는 간단한 가이드입니다.

## 📋 목차

- [시스템 요구사항](#-시스템-요구사항)
- [1단계: Unity 도구 설치](#1단계-unity-도구-설치)
- [2단계: Android 기기 설정](#2단계-android-기기-설정)
- [3단계: 페어링 (최초 1회)](#3단계-페어링-최초-1회)
- [4단계: 기기 연결 (매번 사용)](#4단계-기기-연결-매번-사용)
- [문제 해결](#-문제-해결)

---

## 📋 시스템 요구사항

- **Unity**: 2019.4 LTS 이상 (테스트 완료: 6000.0.47f1)
- **Android**: Android 11 (API 30) 이상
- **네트워크**: PC와 Android가 같은 WiFi 연결
- **ADB**: Unity Android Build Support 모듈 필요

---

## 1단계: Unity 도구 설치

### 방법 1: 직접 다운로드 ⭐ 권장

```bash
# 리포지토리 다운로드
git clone https://github.com/ovthemoon/unity-wireless-debug-manager.git

# Unity 프로젝트에 복사
cp unity-wireless-debug-manager/Editor/WirelessDebugManager.cs /path/to/your/unity/project/Assets/Editor/
```

### 방법 2: Unity Package Manager

1. Unity → **Window → Package Manager**
2. **"+" → Add package from git URL**
3. URL 입력: `https://github.com/ovthemoon/unity-wireless-debug-manager.git`

### ✅ 설치 확인
Unity 메뉴에서 **Tools → Wireless Debug Manager**가 보이면 완료!

---

## 2단계: Android 기기 설정

### 🔧 개발자 옵션 활성화

1. **설정** → **휴대전화 정보** (또는 디바이스 정보)
2. **빌드 번호**를 **7번 연속 터치**
3. "개발자가 되었습니다!" 메시지 확인

#### 제조사별 빌드 번호 위치
- **Samsung**: 설정 → 휴대전화 정보 → 소프트웨어 정보 → 빌드번호
- **Xiaomi**: 설정 → 내 기기 → 전체 사양 및 정보 → MIUI 버전
- **OnePlus**: 설정 → 휴대전화 정보 → 빌드 번호

### 📶 무선 디버깅 활성화

1. **설정** → **개발자 옵션**
2. **USB 디버깅** ON (권장)
3. **무선 디버깅** ON
4. 권한 허용 대화상자에서 **허용**

---

## 3단계: 페어링 (최초 1회)

### 📱 Android에서

1. **설정** → **개발자 옵션** → **무선 디버깅**
2. **"페어링 코드로 기기 페어링"** 터치
3. 표시된 정보 확인:
   ```
   IP 주소: 192.168.1.100
   포트: 37852
   페어링 코드: 123456
   ```

### 🔗 Unity에서

1. **Tools** → **Wireless Debug Manager** 열기
2. **"1. 페어링"** 섹션에서:
   - **기기 IP**: `192.168.1.100`
   - **페어링 포트**: `37852`
   - **페어링 코드**: `123456`
3. **"🔗 페어링 시작"** 클릭

### ✅ 성공 확인
- Unity: "페어링 성공!" 메시지
- Android: "페어링됨" 상태 표시
- **🎉 이제 더 이상 페어링 불필요!**

---

## 4단계: 기기 연결 (매번 사용)

### 📱 연결 포트 확인
**무선 디버깅** 메인 화면에서:
```
IP 주소 및 포트: 192.168.1.100:5555
```

### 🔌 Unity에서 연결
1. **"2. 기기 연결"** 섹션에서:
   - **기기 IP**: `192.168.1.100`
   - **연결 포트**: `5555`
2. **"🔌 연결"** 클릭

### 🚀 완료!
- "연결된 기기"에 기기 표시
- **Build And Run**으로 무선 배포 가능!

---

## 🔧 문제 해결

### 연결이 안될 때

#### ✅ 체크리스트
- [ ] Android 11 이상인가?
- [ ] 개발자 옵션 활성화됨?
- [ ] 무선 디버깅 켜짐?
- [ ] 같은 WiFi 네트워크?

#### 🛠️ Unity에서 해결
1. **"ADB 재시작"** 클릭
2. **"연결 상태 새로고침"** 클릭
3. 모든 기기 연결 해제 후 재연결

### 자주 발생하는 오류

#### "ADB를 찾을 수 없습니다"
- Unity **Android Build Support** 모듈 설치 확인
- **Edit → Preferences → External Tools**에서 Android SDK 경로 설정

#### "페어링 실패"
- 페어링 코드가 **6자리 숫자**인지 확인
- 페어링 코드 **유효시간 만료** (새로 생성)
- 자동 실패시 **수동 페어링 가이드** 사용

#### "연결 실패"
- **페어링 포트** ≠ **연결 포트** 확인
- Android에서 무선 디버깅 재활성화
- 방화벽 설정 확인

#### 네트워크 테스트
```bash
# PC에서 Android로 연결 테스트
ping 192.168.1.100

# Windows: 포트 테스트
telnet 192.168.1.100 5555

# macOS/Linux: 포트 테스트
nc -zv 192.168.1.100 5555
```

---

## 💡 팁

### 개발 효율성
- **5GHz WiFi** 사용 (더 빠름)
- **Development Build** + **Script Debugging** 활성화
- **로그 실시간 확인**: `adb logcat` 병행 사용

### 보안
- 개발 완료 후 **무선 디버깅 비활성화**
- **공용 WiFi 사용 금지**
- 회사 WiFi는 방화벽으로 차단될 수 있음

---

## 📞 도움말

- **GitHub Issues**: [버그 리포트](https://github.com/ovthemoon/unity-wireless-debug-manager/issues)
- **Unity 문서**: [Android 빌드](https://docs.unity3d.com/Manual/android-BuildProcess.html)
- **ADB 가이드**: [공식 문서](https://developer.android.com/studio/command-line/adb)

---

**🎉 이제 케이블 없이 자유롭게 개발하세요!**