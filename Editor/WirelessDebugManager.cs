using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
public enum Language
{
    Korean,
    English
}


public class WirelessDebugManager : EditorWindow
{
    private string deviceIP = "";
    private string devicePort = "5555";
    private string pairingPort = "";
    private string pairingCode = "";
    private bool isConnected = false;
    private List<string> connectedDevices = new List<string>();
    private Vector2 scrollPosition;
    private string localIP = "";

    // 에러 메시지 표시용
    private string lastError = "";
    private double lastErrorTime = 0;

    private Language currentLanguage = Language.Korean;
    private Dictionary<string, Dictionary<Language, string>> texts;

    [MenuItem("Tools/Wireless Debug Manager")]
    public static void ShowWindow()
    {
        var window = GetWindow<WirelessDebugManager>("Wireless Debug");
        window.minSize = new Vector2(350, 500);
    }

    private void OnEnable()
    {
        InitializeLanguageSystem();
        RefreshConnectedDevices();
        localIP = GetLocalIPAddress();
    }
    private void InitializeLanguageSystem()
    {
        // 시스템 언어에 따라 자동 설정
        SystemLanguage systemLang = Application.systemLanguage;
        currentLanguage = (systemLang == SystemLanguage.Korean) ? Language.Korean : Language.English;

        // 저장된 언어 설정 불러오기
        string savedLang = EditorPrefs.GetString("WirelessDebugManager_Language", "");
        if (!string.IsNullOrEmpty(savedLang))
        {
            if (System.Enum.TryParse<Language>(savedLang, out Language lang))
            {
                currentLanguage = lang;
            }
        }

        InitializeTexts();
    }
    private void InitializeTexts()
    {
        texts = new Dictionary<string, Dictionary<Language, string>>
        {
            ["title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "Unity Wireless Debug Manager",
                [Language.English] = "Unity Wireless Debug Manager"
            },
            ["system_info"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "시스템 정보",
                [Language.English] = "System Information"
            },
            ["platform"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "플랫폼",
                [Language.English] = "Platform"
            },
            ["unity_version"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "Unity 버전",
                [Language.English] = "Unity Version"
            },
            ["local_ip"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "로컬 IP",
                [Language.English] = "Local IP"
            },
            ["adb_found"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "발견됨",
                [Language.English] = "Found"
            },
            ["adb_not_found"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "찾을 수 없음",
                [Language.English] = "Not Found"
            },
            ["copy_adb_path"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "ADB 경로 복사",
                [Language.English] = "Copy ADB Path"
            },
            ["copy_complete"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "복사 완료",
                [Language.English] = "Copy Complete"
            },
            ["copy_complete_msg"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "ADB 경로가 클립보드에 복사되었습니다:",
                [Language.English] = "ADB path has been copied to clipboard:"
            },
            ["connection_status"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결 상태",
                [Language.English] = "Connection Status"
            },
            ["status"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "상태",
                [Language.English] = "Status"
            },
            ["connected"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결됨",
                [Language.English] = "Connected"
            },
            ["disconnected"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결 안됨",
                [Language.English] = "Disconnected"
            },
            ["refresh_status"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결 상태 새로고침",
                [Language.English] = "Refresh Connection Status"
            },
            ["pairing_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "1. 페어링 (최초 1회만)",
                [Language.English] = "1. Pairing (One-time Only)"
            },
            ["pairing_help"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "🔒 최초 1회만 페어링하면 됩니다!\nAndroid 기기에서 '페어링 코드로 기기 페어링'을 선택하면\nIP주소, 포트, 6자리 페어링 코드가 표시됩니다.",
                [Language.English] = "🔒 You only need to pair once!\nSelect 'Pair device with pairing code' on your Android device\nto see IP address, port, and 6-digit pairing code."
            },
            ["device_ip"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "기기 IP:",
                [Language.English] = "Device IP:"
            },
            ["pairing_port"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "페어링 포트:",
                [Language.English] = "Pairing Port:"
            },
            ["pairing_code"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "페어링 코드:",
                [Language.English] = "Pairing Code:"
            },
            ["start_pairing"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "🔗 페어링 시작",
                [Language.English] = "🔗 Start Pairing"
            },
            ["check_pairing"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "✅ 페어링 확인",
                [Language.English] = "✅ Check Pairing"
            },
            ["connection_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "2. 기기 연결 (매번 사용)",
                [Language.English] = "2. Device Connection (Every Time)"
            },
            ["connection_help"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "🚀 페어링 완료 후 매번 이걸로 연결하세요!\n연결 포트는 페어링 포트와 다릅니다. (보통 5555)",
                [Language.English] = "🚀 Use this to connect every time after pairing!\nConnection port is different from pairing port. (Usually 5555)"
            },
            ["connection_port"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결 포트:",
                [Language.English] = "Connection Port:"
            },
            ["connect"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "🔌 연결",
                [Language.English] = "🔌 Connect"
            },
            ["disconnect"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "❌ 연결 해제",
                [Language.English] = "❌ Disconnect"
            },
            ["connected_devices"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결된 기기",
                [Language.English] = "Connected Devices"
            },
            ["no_devices"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결된 기기가 없습니다.",
                [Language.English] = "No connected devices."
            },
            ["utilities"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "유틸리티",
                [Language.English] = "Utilities"
            },
            ["restart_adb"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "ADB 재시작",
                [Language.English] = "Restart ADB"
            },
            ["help"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "도움말",
                [Language.English] = "Help"
            },
            ["check_adb_version"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "ADB 버전 확인",
                [Language.English] = "Check ADB Version"
            },
            ["disconnect_all"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "모든 기기 연결 해제",
                [Language.English] = "Disconnect All Devices"
            },
            ["language"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "언어",
                [Language.English] = "Language"
            },
            ["korean"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "한국어",
                [Language.English] = "Korean"
            },
            ["english"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "English",
                [Language.English] = "English"
            },
            ["disconnect_device"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결 해제",
                [Language.English] = "Disconnect"
            },
            ["enter_device_ip"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "기기 IP를 입력해주세요.",
                [Language.English] = "Please enter device IP."
            },
            ["success"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "성공",
                [Language.English] = "Success"
            },
            ["connection_success"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "기기가 성공적으로 연결되었습니다!",
                [Language.English] = "Device connected successfully!"
            },
            ["confirm"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "확인",
                [Language.English] = "OK"
            },
            ["connection_failed"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결 실패",
                [Language.English] = "Connection failed"
            },
            ["complete"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "완료",
                [Language.English] = "Complete"
            },
            ["device_disconnected"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "기기 연결이 해제되었습니다.",
                [Language.English] = "Device has been disconnected."
            },
            ["all_devices_disconnected"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "모든 기기 연결이 해제되었습니다.",
                [Language.English] = "All devices have been disconnected."
            },
            ["pairing_in_progress"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "페어링 중",
                [Language.English] = "Pairing"
            },
            ["pairing_device"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "기기와 페어링하는 중...",
                [Language.English] = "Pairing with device..."
            },
            ["invalid_pairing_code"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "페어링 코드는 6자리 숫자여야 합니다. (예: 123456)",
                [Language.English] = "Pairing code must be 6 digits. (e.g., 123456)"
            },
            ["pairing_success_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "페어링 성공!",
                [Language.English] = "Pairing Success!"
            },
            ["pairing_success_msg"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "✅ 기기와 성공적으로 페어링되었습니다!\n\n이제 아래 '기기 연결' 섹션에서\n연결 포트로 연결하세요.\n💡 연결 포트는 보통 5555입니다.",
                [Language.English] = "✅ Successfully paired with device!\n\nNow connect using the connection port\nin the 'Device Connection' section below.\n💡 Connection port is usually 5555."
            },
            ["pairing_failed"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "페어링 실패",
                [Language.English] = "Pairing failed"
            },
            ["adb_version_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "ADB 버전",
                [Language.English] = "ADB Version"
            },
            ["adb_restarted"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "ADB가 재시작되었습니다.",
                [Language.English] = "ADB has been restarted."
            },
            // 🆕 InitializeTexts() 메서드에 추가할 도움말 텍스트들
            ["help_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "도움말",
                [Language.English] = "Help"
            },
            ["connection_port_help"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "💡 연결 포트 확인 방법:\n안드로이드 무선 디버깅 메인 화면에서\n'IP 주소 및 포트' 섹션을 확인하세요.",
                [Language.English] = "💡 How to check connection port:\nGo to Android Wireless Debugging main screen\nand check 'IP address & Port' section."
            },
            // help_content 텍스트 업데이트
            ["help_content"] = new Dictionary<Language, string>
            {
                [Language.Korean] = @"Unity Wireless Debug Manager 사용법:

플랫폼: {0}
로컬 IP: {1}

🔥 간단한 3단계 과정:

0️⃣ 안드로이드 설정 (최초 설정):
   - 개발자 옵션 활성화 (빌드번호 7번 터치)
   - 무선 디버깅 활성화
   - 같은 WiFi 네트워크 연결

1️⃣ 페어링 (최초 1회만):
   - 안드로이드: 설정 → 개발자 옵션 → 무선 디버깅
   - '페어링 코드로 기기 페어링' 선택
   - IP, 페어링 포트, 6자리 코드를 Unity에 입력
   - '🔗 페어링 시작' 클릭
   - ✅ 성공하면 더 이상 페어링 안해도 됨!

2️⃣ 연결 (매번 사용):
   - 안드로이드에서 연결 포트 확인 (보통 5555)
   - Unity에서 IP:연결포트 입력 후 '🔌 연결'
   - 🚀 이제 무선으로 개발!

⚠️ 중요:
   - Android 11+ 필요
   - 페어링 포트 ≠ 연결 포트
   - 페어링은 딱 한 번만!
   - 연결은 개발할 때마다

🔧 문제 해결:
   - ADB 재시작
   - 방화벽 확인
   - WiFi 네트워크 확인
   - 개발자 옵션 재활성화

플랫폼별 지원:
- Windows ✓ (수동 페어링)
- macOS ✓ (자동 페어링)  
- Linux ✓ (자동 페어링)",

                [Language.English] = @"Unity Wireless Debug Manager Usage:

Platform: {0}
Local IP: {1}

🔥 Simple 3-Step Process:

0️⃣ Android Setup (Initial Setup):
   - Enable Developer Options (tap Build number 7 times)
   - Enable Wireless Debugging
   - Connect to same WiFi network

1️⃣ Pairing (One-time Only):
   - Android: Settings → Developer Options → Wireless Debugging
   - Select 'Pair device with pairing code'
   - Enter IP, pairing port, 6-digit code in Unity
   - Click '🔗 Start Pairing'
   - ✅ Once successful, no need to pair again!

2️⃣ Connection (Every Time):
   - Check connection port on Android (usually 5555)
   - Enter IP:connection port in Unity and click '🔌 Connect'
   - 🚀 Now develop wirelessly!

⚠️ Important:
   - Requires Android 11+
   - Pairing port ≠ Connection port
   - Pairing is one-time only!
   - Connect every time you develop

🔧 Troubleshooting:
   - Restart ADB
   - Check firewall settings
   - Check WiFi network
   - Re-enable Developer Options

Platform Support:
- Windows ✓ (Manual pairing)
- macOS ✓ (Auto pairing)  
- Linux ✓ (Auto pairing)"
            },
            ["manual_pairing_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "수동 페어링 가이드",
                [Language.English] = "Manual Pairing Guide"
            },
            ["manual_pairing_failed"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "🔧 자동 페어링이 실패했습니다. 수동으로 시도해보세요:",
                [Language.English] = "🔧 Auto pairing failed. Please try manually:"
            },
            ["manual_pairing_windows"] = new Dictionary<Language, string>
            {
                [Language.Korean] = @"Windows PowerShell에서 실행:
{0}

또는 CMD에서:
1. {1} pair {2}:{3}
2. 페어링 코드 입력: {4}

💡 팁: 위 PowerShell 명령어가 클립보드에 복사되었습니다!",
                [Language.English] = @"Run in Windows PowerShell:
{0}

Or in CMD:
1. {1} pair {2}:{3}
2. Enter pairing code: {4}

💡 Tip: The PowerShell command has been copied to clipboard!"
            },
            ["manual_pairing_unix"] = new Dictionary<Language, string>
            {
                [Language.Korean] = @"터미널에서 실행:
{0}

또는 수동으로:
1. {1} pair {2}:{3}
2. 페어링 코드 입력: {4}

💡 팁: 위 명령어가 클립보드에 복사되었습니다!",
                [Language.English] = @"Run in Terminal:
{0}

Or manually:
1. {1} pair {2}:{3}
2. Enter pairing code: {4}

💡 Tip: The command has been copied to clipboard!"
            },
            ["current_connected_devices"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "현재 연결된 기기",
                [Language.English] = "Currently Connected Devices"
            },
            ["no_connected_devices"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "연결된 기기가 없습니다.",
                [Language.English] = "No connected devices."
            },
            // 🆕 InitializeTexts() 메서드에 추가할 0단계 텍스트들
            ["step0_title"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "0. 안드로이드 설정 (최초 설정)",
                [Language.English] = "0. Android Setup (Initial Setup)"
            },
            ["step0_help"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "📱 안드로이드 기기에서 개발자 모드와 무선 디버깅을 활성화해야 합니다.",
                [Language.English] = "📱 You need to enable Developer Mode and Wireless Debugging on your Android device."
            },
            ["android_setup_guide"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "안드로이드 설정 가이드",
                [Language.English] = "Android Setup Guide"
            },
            ["show_android_setup"] = new Dictionary<Language, string>
            {
                [Language.Korean] = "📱 안드로이드 설정 방법",
                [Language.English] = "📱 Android Setup Guide"
            },
            ["android_setup_content"] = new Dictionary<Language, string>
            {
                [Language.Korean] = @"안드로이드 개발자 모드 & 무선 디버깅 활성화:

🔧 1단계: 개발자 옵션 활성화
   ① 설정(Settings) 앱 열기
   ② '휴대전화 정보' 또는 '디바이스 정보' 선택
   ③ '빌드 번호(Build Number)' 항목 찾기
   ④ 빌드 번호를 연속으로 7번 터치
   ⑤ '개발자가 되었습니다!' 메시지 확인

📶 2단계: 무선 디버깅 활성화
   ① 설정 → 개발자 옵션 이동
   ② 'USB 디버깅' 활성화 (권장)
   ③ '무선 디버깅' 토글 ON
   ④ WiFi 네트워크 허용 확인창에서 '허용' 클릭

🌐 3단계: 네트워크 확인
   ① 컴퓨터와 안드로이드가 같은 WiFi 연결 확인
   ② 5GHz WiFi 사용 권장 (2.4GHz보다 빠름)
   ③ 회사 WiFi는 방화벽으로 차단될 수 있음

💡 참고사항:
   • Android 11+ 에서만 무선 디버깅 지원
   • 일부 제조사는 메뉴 위치가 다를 수 있음
   • 삼성: 설정 → 휴대전화 정보 → 소프트웨어 정보 → 빌드번호
   • LG: 설정 → 일반 → 휴대전화 정보 → 소프트웨어 정보 → 빌드번호",

                [Language.English] = @"Enable Android Developer Mode & Wireless Debugging:

🔧 Step 1: Enable Developer Options
   ① Open Settings app
   ② Select 'About phone' or 'About device'
   ③ Find 'Build number' entry
   ④ Tap 'Build number' 7 times consecutively
   ⑤ See 'You are now a developer!' message

📶 Step 2: Enable Wireless Debugging
   ① Go to Settings → Developer options
   ② Enable 'USB debugging' (recommended)
   ③ Turn ON 'Wireless debugging' toggle
   ④ Click 'Allow' on WiFi network permission dialog

🌐 Step 3: Network Check
   ① Ensure computer and Android use same WiFi
   ② 5GHz WiFi recommended (faster than 2.4GHz)
   ③ Corporate WiFi may be blocked by firewall

💡 Notes:
   • Wireless debugging only supported on Android 11+
   • Menu locations may vary by manufacturer
   • Samsung: Settings → About phone → Software info → Build number
   • LG: Settings → General → About phone → Software info → Build number"
            }
        };

    }

    private string GetText(string key)
    {
        if (texts != null && texts.ContainsKey(key) && texts[key].ContainsKey(currentLanguage))
        {
            return texts[key][currentLanguage];
        }
        return key; // 키를 찾을 수 없으면 키 자체를 반환
    }

    private void SetLanguage(Language language)
    {
        currentLanguage = language;
        EditorPrefs.SetString("WirelessDebugManager_Language", language.ToString());
        Repaint();
    }

    private void OnGUI()
{
    GUILayout.Label(GetText("title"), EditorStyles.boldLabel);
    EditorGUILayout.Space();

    // 스크롤 뷰 시작
    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
    
    DrawSystemInfo();
    EditorGUILayout.Space();

    DrawConnectionStatus();
    EditorGUILayout.Space();

    DrawAndroidSetup();
    EditorGUILayout.Space();

    DrawPairingSection();
    EditorGUILayout.Space();

    DrawDirectConnection();
    EditorGUILayout.Space();

    DrawConnectedDevices();
    EditorGUILayout.Space();

    DrawUtilityButtons();

    DrawErrorMessage();
    
    // 스크롤 뷰 끝
    EditorGUILayout.EndScrollView();
}

    private void DrawAndroidSetup()
    {
        EditorGUILayout.LabelField(GetText("step0_title"), EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(GetText("step0_help"), MessageType.Info);

        if (GUILayout.Button(GetText("show_android_setup")))
        {
            ShowAndroidSetupGuide();
        }
    }

    private void ShowAndroidSetupGuide()
    {
        // 🆕 별도 창으로 표시
        WirelessDebugHelpWindow.ShowHelp(GetText("android_setup_guide"), GetText("android_setup_content"));
    }

    private void DrawSystemInfo()
    {
        EditorGUILayout.LabelField(GetText("system_info"), EditorStyles.boldLabel);

        // 🆕 언어 선택 UI 추가
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GetText("language") + ":", GUILayout.Width(60));
        Language newLanguage = (Language)EditorGUILayout.EnumPopup(currentLanguage);
        if (newLanguage != currentLanguage)
        {
            SetLanguage(newLanguage);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField($"{GetText("platform")}: {GetCurrentPlatform()}");
        EditorGUILayout.LabelField($"{GetText("unity_version")}: {Application.unityVersion}");
        EditorGUILayout.LabelField($"{GetText("local_ip")}: {localIP}");

        string adbPath = GetADBPath();
        if (!string.IsNullOrEmpty(adbPath))
        {
            bool isUnitySDK = adbPath.Contains("PlaybackEngines");
            string sdkType = isUnitySDK ? "Unity" : "System";
            EditorGUILayout.LabelField($"ADB ({sdkType}): {GetText("adb_found")}");

            if (GUILayout.Button(GetText("copy_adb_path")))
            {
                EditorGUIUtility.systemCopyBuffer = adbPath;
                EditorUtility.DisplayDialog(GetText("copy_complete"),
                    $"{GetText("copy_complete_msg")}\n{adbPath}", "OK");
            }
        }
        else
        {
            EditorGUILayout.LabelField($"ADB: {GetText("adb_not_found")}", EditorStyles.helpBox);
        }
    }

    private void DrawConnectionStatus()
    {
        EditorGUILayout.LabelField(GetText("connection_status"), EditorStyles.boldLabel);

        string statusText = isConnected ? GetText("connected") : GetText("disconnected");
        Color statusColor = isConnected ? Color.green : Color.red;

        var oldColor = GUI.color;
        GUI.color = statusColor;
        EditorGUILayout.LabelField($"{GetText("status")}: {statusText}");
        GUI.color = oldColor;

        if (GUILayout.Button(GetText("refresh_status")))
        {
            RefreshConnectedDevices();
        }
    }

    private void DrawDirectConnection()
    {
        EditorGUILayout.LabelField(GetText("connection_title"), EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(GetText("connection_help"), MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GetText("device_ip"), GUILayout.Width(80));
        deviceIP = EditorGUILayout.TextField(deviceIP);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GetText("connection_port"), GUILayout.Width(80));
        devicePort = EditorGUILayout.TextField(devicePort);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(GetText("connect")))
        {
            ConnectToDevice();
        }
        if (GUILayout.Button(GetText("disconnect")))
        {
            DisconnectDevice();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox(GetText("connection_port_help"), MessageType.Info);
    }

    private void DrawPairingSection()
    {
        EditorGUILayout.LabelField(GetText("pairing_title"), EditorStyles.boldLabel);

        EditorGUILayout.HelpBox(GetText("pairing_help"), MessageType.Info);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GetText("device_ip"), GUILayout.Width(100));
        string pairingIP = EditorGUILayout.TextField(deviceIP);
        if (pairingIP != deviceIP) deviceIP = pairingIP;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GetText("pairing_port"), GUILayout.Width(100));
        pairingPort = EditorGUILayout.TextField(pairingPort);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GetText("pairing_code"), GUILayout.Width(100));
        pairingCode = EditorGUILayout.TextField(pairingCode);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(GetText("start_pairing")))
        {
            if (string.IsNullOrEmpty(deviceIP) || string.IsNullOrEmpty(pairingPort) || string.IsNullOrEmpty(pairingCode))
            {
                ShowError(currentLanguage == Language.Korean ?
                    "모든 정보를 입력해주세요." :
                    "Please enter all information.");
                return;
            }
            StartPairing(deviceIP, pairingPort, pairingCode);
        }

        if (GUILayout.Button(GetText("check_pairing")))
        {
            CheckPairedDevices();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawConnectedDevices()
    {
        EditorGUILayout.LabelField(GetText("connected_devices"), EditorStyles.boldLabel);

        if (connectedDevices.Count == 0)
        {
            EditorGUILayout.HelpBox(GetText("no_devices"), MessageType.Info);
        }
        else
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(100));
            foreach (string device in connectedDevices)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(device);
                if (GUILayout.Button(GetText("disconnect_device"), GUILayout.Width(100)))
                {
                    DisconnectSpecificDevice(device);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private void DrawUtilityButtons()
    {
        EditorGUILayout.LabelField(GetText("utilities"), EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(GetText("restart_adb")))
        {
            RestartADB();
        }
        if (GUILayout.Button(GetText("help")))
        {
            ShowHelp();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(GetText("check_adb_version")))
        {
            CheckADBVersion();
        }
        if (GUILayout.Button(GetText("disconnect_all")))
        {
            DisconnectAllDevices();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawErrorMessage()
    {
        if (!string.IsNullOrEmpty(lastError) && EditorApplication.timeSinceStartup - lastErrorTime < 5)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(lastError, MessageType.Error);
        }
    }

    private void ShowError(string message)
    {
        lastError = message;
        lastErrorTime = EditorApplication.timeSinceStartup;
        Repaint();
    }

    private string GetCurrentPlatform()
    {
#if UNITY_EDITOR_WIN
        return "Windows";
#elif UNITY_EDITOR_OSX
        return "macOS";
#elif UNITY_EDITOR_LINUX
        return "Linux";
#else
        return "Unknown";
#endif
    }

    private void ConnectToDevice()
    {
        if (string.IsNullOrEmpty(deviceIP))
        {
            ShowError(GetText("enter_device_ip"));
            return;
        }

        string command = $"connect {deviceIP}:{devicePort}";
        ExecuteADBCommand(command, (output) =>
        {
            if (output.Contains("connected"))
            {
                EditorUtility.DisplayDialog(GetText("success"), GetText("connection_success"), GetText("confirm"));
                RefreshConnectedDevices();
            }
            else
            {
                ShowError($"{GetText("connection_failed")}: {output}");
            }
        });
    }

    private void DisconnectDevice()
    {
        if (string.IsNullOrEmpty(deviceIP))
        {
            DisconnectAllDevices();
            return;
        }

        string command = $"disconnect {deviceIP}:{devicePort}";
        ExecuteADBCommand(command, (output) =>
        {
            EditorUtility.DisplayDialog(GetText("complete"), GetText("device_disconnected"), GetText("confirm"));
            RefreshConnectedDevices();
        });
    }

    private void DisconnectAllDevices()
    {
        ExecuteADBCommand("disconnect", (output) =>
        {
            EditorUtility.DisplayDialog(GetText("complete"), GetText("all_devices_disconnected"), GetText("confirm"));
            RefreshConnectedDevices();
        });
    }

    private void DisconnectSpecificDevice(string device)
    {
        string command = $"disconnect {device}";
        ExecuteADBCommand(command, (output) =>
        {
            RefreshConnectedDevices();
        });
    }

    private void StartPairing(string ip, string pairingPort, string pairingCode)
    {
        if (!IsValidPairingCode(pairingCode))
        {
            ShowError(GetText("invalid_pairing_code"));
            return;
        }

        string command = $"pair {ip}:{pairingPort}";

        EditorUtility.DisplayProgressBar(GetText("pairing_in_progress"), GetText("pairing_device"), 0.5f);

        ExecuteADBCommandWithInput(command, pairingCode, (output) =>
        {
            EditorUtility.ClearProgressBar();

            if (output.Contains("Successfully paired") || output.Contains("성공"))
            {
                EditorUtility.DisplayDialog(GetText("pairing_success_title"), GetText("pairing_success_msg"), GetText("confirm"));
                RefreshConnectedDevices();
            }
            else
            {
                ShowError($"{GetText("pairing_failed")}: {output}");
                ShowManualPairingGuide(ip, pairingPort, pairingCode);
            }
        });
    }
    private void ExecuteADBCommandWithInput(string arguments, string input, System.Action<string> onComplete)
    {
        try
        {
            string adbPath = GetADBPath();
            if (string.IsNullOrEmpty(adbPath))
            {
                ShowError("ADB를 찾을 수 없습니다. Android SDK가 설치되어 있는지 확인하세요.");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = adbPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                if (!string.IsNullOrEmpty(input))
                {
                    process.StandardInput.WriteLine(input);
                    process.StandardInput.Flush();
                    process.StandardInput.Close();
                }

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                string result = string.IsNullOrEmpty(error) ? output : error;
                onComplete?.Invoke(result);
            }
        }
        catch (System.Exception ex)
        {
            ShowError($"ADB 명령 실행 실패: {ex.Message}");
            onComplete?.Invoke($"오류: {ex.Message}");
        }
    }
    private void ShowManualPairingGuide(string ip, string pairingPort, string pairingCode)
    {
        string adbPath = GetADBPath();
        string instructions = "";

#if UNITY_EDITOR_WIN
    // Windows: echo 파이프 사용
    string windowsCommand = $"echo {pairingCode} | \"{adbPath}\" pair {ip}:{pairingPort}";
    instructions = string.Format(GetText("manual_pairing_windows"), 
        windowsCommand, adbPath, ip, pairingPort, pairingCode);
    EditorGUIUtility.systemCopyBuffer = windowsCommand;

#elif UNITY_EDITOR_OSX
        // macOS: echo 사용
        string macCommand = $"echo '{pairingCode}' | '{adbPath}' pair {ip}:{pairingPort}";
        instructions = string.Format(GetText("manual_pairing_unix"),
            macCommand, adbPath, ip, pairingPort, pairingCode);
        EditorGUIUtility.systemCopyBuffer = macCommand;

#else
    // Linux
    string linuxCommand = $"echo '{pairingCode}' | '{adbPath}' pair {ip}:{pairingPort}";
    instructions = string.Format(GetText("manual_pairing_unix"), 
        linuxCommand, adbPath, ip, pairingPort, pairingCode);
    EditorGUIUtility.systemCopyBuffer = linuxCommand;
#endif

        string fullMessage = GetText("manual_pairing_failed") + "\n\n" + instructions;

        EditorUtility.DisplayDialog(GetText("manual_pairing_title"), fullMessage, GetText("confirm"));
        UnityEngine.Debug.Log($"Manual pairing command: {EditorGUIUtility.systemCopyBuffer}");
    }
    private bool IsValidPairingCode(string code)
    {
        // 6자리 숫자인지 확인
        if (string.IsNullOrEmpty(code) || code.Length != 6)
            return false;

        // 숫자만 포함하는지 확인
        foreach (char c in code)
        {
            if (!char.IsDigit(c))
                return false;
        }

        return true;
    }
    private void CheckPairedDevices()
    {
        RefreshConnectedDevices();

        string deviceList = connectedDevices.Count > 0
            ? string.Join("\n", connectedDevices)
            : GetText("no_connected_devices");

        EditorUtility.DisplayDialog(GetText("current_connected_devices"), deviceList, GetText("confirm"));
    }

    private void RefreshConnectedDevices()
    {
        ExecuteADBCommand("devices", (output) =>
        {
            connectedDevices.Clear();
            string[] lines = output.Split('\n');

            foreach (string line in lines)
            {
                if (line.Contains("device") && !line.Contains("List of devices"))
                {
                    string deviceInfo = line.Trim();
                    if (!string.IsNullOrEmpty(deviceInfo))
                    {
                        connectedDevices.Add(deviceInfo.Split('\t')[0]);
                    }
                }
            }

            isConnected = connectedDevices.Count > 0;
            Repaint();
        });
    }

    private string GetLocalIPAddress()
    {
        try
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    var properties = networkInterface.GetIPProperties();
                    foreach (var address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            string ip = address.Address.ToString();
                            if (ip.StartsWith("192.168.") || ip.StartsWith("10.") || ip.StartsWith("172."))
                            {
                                return ip;
                            }
                        }
                    }
                }
            }

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"IP 주소 가져오기 실패: {ex.Message}");
        }
        return "Unknown";
    }

    private void ExecuteADBCommand(string arguments, System.Action<string> onComplete)
    {
        try
        {
            string adbPath = GetADBPath();
            if (string.IsNullOrEmpty(adbPath))
            {
                ShowError("ADB를 찾을 수 없습니다. Android SDK가 설치되어 있는지 확인하세요.");
                return;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = adbPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                string result = string.IsNullOrEmpty(error) ? output : error;
                onComplete?.Invoke(result);
            }
        }
        catch (System.Exception ex)
        {
            ShowError($"ADB 명령 실행 실패: {ex.Message}");
            onComplete?.Invoke($"오류: {ex.Message}");
        }
    }

    private string GetADBPath()
    {
        // 🆕 1. Unity 내장 Android SDK 확인 (최우선으로 변경!)
        string unityAdbPath = GetUnityAndroidSDKPath();
        if (!string.IsNullOrEmpty(unityAdbPath))
        {
            return unityAdbPath;
        }

        // 2. Unity Editor 설정에서 확인
        string androidSdkPath = EditorPrefs.GetString("AndroidSdkRoot");
        if (!string.IsNullOrEmpty(androidSdkPath))
        {
            string adbPath = Path.Combine(androidSdkPath, "platform-tools", GetADBExecutableName());
            if (File.Exists(adbPath))
            {
                return adbPath;
            }
        }

        // 3. 시스템 기본 경로들 확인
        string[] possiblePaths = GetPlatformSpecificADBPaths();
        foreach (string path in possiblePaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        // 4. PATH 환경변수에서 확인
        string adbFromPath = FindInPath(GetADBExecutableName());
        if (!string.IsNullOrEmpty(adbFromPath))
        {
            return adbFromPath;
        }

        return null;
    }

    private string GetADBExecutableName()
    {
#if UNITY_EDITOR_WIN
        return "adb.exe";
#else
        return "adb";
#endif
    }

    private string[] GetPlatformSpecificADBPaths()
    {
        List<string> paths = new List<string>();

        // 🆕 Unity Editor 내장 Android SDK 경로 우선 추가
        string unityAndroidSDK = GetUnityAndroidSDKPath();
        if (!string.IsNullOrEmpty(unityAndroidSDK))
        {
            paths.Add(unityAndroidSDK);
        }

#if UNITY_EDITOR_WIN
    // Windows 기본 경로들
    paths.AddRange(new string[]
    {
        Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), 
            "AppData", "Local", "Android", "Sdk", "platform-tools", "adb.exe"),
        @"C:\Android\platform-tools\adb.exe",
        @"C:\android-sdk\platform-tools\adb.exe"
    });
#elif UNITY_EDITOR_OSX
        // macOS 기본 경로들
        paths.AddRange(new string[]
        {
        Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile),
            "Library", "Android", "sdk", "platform-tools", "adb"),
        "/usr/local/bin/adb",
        "/opt/homebrew/bin/adb"
        });
#else
    // Linux 기본 경로들
    paths.AddRange(new string[]
    {
        Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), 
            "Android", "Sdk", "platform-tools", "adb"),
        "/usr/bin/adb",
        "/usr/local/bin/adb"
    });
#endif

        return paths.ToArray();
    }
    private string GetUnityAndroidSDKPath()
    {
        try
        {
            // Unity Editor 경로 가져오기
            string editorPath = EditorApplication.applicationPath;

#if UNITY_EDITOR_OSX
            // macOS: /Applications/Unity/Hub/Editor/6000.0.47f1/Unity.app
            // 🆕 Unity.app을 제거하고 상위 디렉토리에서 PlaybackEngines 찾기
            string editorDir = Path.GetDirectoryName(editorPath); // Unity.app 제거
            string androidSDKPath = Path.Combine(editorDir, "PlaybackEngines", "AndroidPlayer", "SDK", "platform-tools", "adb");

#elif UNITY_EDITOR_WIN
        // Windows: C:\Program Files\Unity\Hub\Editor\6000.0.47f1\Editor\Unity.exe
        string editorDir = Path.GetDirectoryName(editorPath);
        string androidSDKPath = Path.Combine(editorDir, "Data", "PlaybackEngines", "AndroidPlayer", "SDK", "platform-tools", "adb.exe");
        
#else
        // Linux: 유사한 구조
        string editorDir = Path.GetDirectoryName(editorPath);
        string androidSDKPath = Path.Combine(editorDir, "Data", "PlaybackEngines", "AndroidPlayer", "SDK", "platform-tools", "adb");
#endif


            if (File.Exists(androidSDKPath))
            {
                return androidSDKPath;
            }
            else
            {

                // 🆕 디버깅을 위해 실제 디렉토리 구조 확인
                if (Directory.Exists(editorDir))
                {
                    string playbackEnginesPath = Path.Combine(editorDir, "PlaybackEngines");
                    if (Directory.Exists(playbackEnginesPath))
                    {
                        string[] subDirs = Directory.GetDirectories(playbackEnginesPath);
                    }
                    else
                    {
                        UnityEngine.Debug.Log($"PlaybackEngines 디렉토리 없음: {playbackEnginesPath}");
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"Unity Android SDK 경로 찾기 실패: {ex.Message}");
        }

        return null;
    }
    private string FindInPath(string fileName)
    {
        string pathVar = System.Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(pathVar)) return null;

        foreach (string path in pathVar.Split(Path.PathSeparator))
        {
            string fullPath = Path.Combine(path, fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }
        return null;
    }

    private void CheckADBVersion()
    {
        ExecuteADBCommand("version", (output) =>
        {
            EditorUtility.DisplayDialog(GetText("adb_version_title"), output, GetText("confirm"));
        });
    }

    private void RestartADB()
    {
        ExecuteADBCommand("kill-server", (output1) =>
        {
            ExecuteADBCommand("start-server", (output2) =>
            {
                EditorUtility.DisplayDialog(GetText("complete"), GetText("adb_restarted"), GetText("confirm"));
                RefreshConnectedDevices();
            });
        });
    }

    private void ShowHelp()
    {
        string platform = GetCurrentPlatform();
        string helpText = string.Format(GetText("help_content"), platform, localIP);

        // 🆕 별도 창으로 표시
        WirelessDebugHelpWindow.ShowHelp(GetText("help_title"), helpText);
    }
}