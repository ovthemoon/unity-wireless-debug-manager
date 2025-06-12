# Unity Wireless Debug Manager - Setup Guide

Simple guide to set up wireless debugging between Unity and Android devices.

## 📋 Table of Contents

- [System Requirements](#-system-requirements)
- [Step 1: Install Unity Tool](#step-1-install-unity-tool)
- [Step 2: Android Device Setup](#step-2-android-device-setup)
- [Step 3: Pairing (One-time Only)](#step-3-pairing-one-time-only)
- [Step 4: Device Connection (Every Time)](#step-4-device-connection-every-time)
- [Troubleshooting](#-troubleshooting)

---

## 📋 System Requirements

- **Unity**: 2019.4 LTS or later (Tested: 6000.0.47f1)
- **Android**: Android 11 (API 30) or later
- **Network**: PC and Android on same WiFi network
- **ADB**: Unity Android Build Support module required

---

## Step 1: Install Unity Tool

### Method 1: Direct Download ⭐ Recommended

```bash
# Download repository
git clone https://github.com/ovthemoon/unity-wireless-debug-manager.git

# Copy to Unity project
cp unity-wireless-debug-manager/Editor/WirelessDebugManager.cs /path/to/your/unity/project/Assets/Editor/
```

### Method 2: Unity Package Manager

1. Unity → **Window → Package Manager**
2. **"+" → Add package from git URL**
3. Enter URL: `https://github.com/ovthemoon/unity-wireless-debug-manager.git`

### ✅ Installation Check
If **Tools → Wireless Debug Manager** appears in Unity menu, installation complete!

---

## Step 2: Android Device Setup

### 🔧 Enable Developer Options

1. **Settings** → **About phone** (or About device)
2. **Tap Build number 7 times consecutively**
3. See "You are now a developer!" message

#### Build Number Location by Manufacturer
- **Samsung**: Settings → About phone → Software information → Build number
- **Xiaomi**: Settings → My device → All specs → MIUI version
- **OnePlus**: Settings → About phone → Build number

### 📶 Enable Wireless Debugging

1. **Settings** → **Developer options**
2. **USB debugging** ON (recommended)
3. **Wireless debugging** ON
4. **Allow** in permission dialog

---

## Step 3: Pairing (One-time Only)

### 📱 On Android

1. **Settings** → **Developer options** → **Wireless debugging**
2. Tap **"Pair device with pairing code"**
3. Note the displayed information:
   ```
   IP address: 192.168.1.100
   Port: 37852
   Pairing code: 123456
   ```

### 🔗 In Unity

1. Open **Tools** → **Wireless Debug Manager**
2. In **"1. Pairing"** section:
   - **Device IP**: `192.168.1.100`
   - **Pairing Port**: `37852`
   - **Pairing Code**: `123456`
3. Click **"🔗 Start Pairing"**

### ✅ Success Confirmation
- Unity: "Pairing Success!" message
- Android: "Paired" status shown
- **🎉 No more pairing needed!**

---

## Step 4: Device Connection (Every Time)

### 📱 Check Connection Port
On **Wireless debugging** main screen:
```
IP address & Port: 192.168.1.100:5555
```

### 🔌 Connect in Unity
1. In **"2. Device Connection"** section:
   - **Device IP**: `192.168.1.100`
   - **Connection Port**: `5555`
2. Click **"🔌 Connect"**

### 🚀 Complete!
- Device shows in "Connected Devices"
- **Build And Run** now works wirelessly!

---

## 🔧 Troubleshooting

### When Connection Fails

#### ✅ Checklist
- [ ] Android 11 or later?
- [ ] Developer options enabled?
- [ ] Wireless debugging turned on?
- [ ] Same WiFi network?

#### 🛠️ Solutions in Unity
1. Click **"Restart ADB"**
2. Click **"Refresh Connection Status"**
3. Disconnect all devices and reconnect

### Common Errors

#### "ADB not found"
- Check Unity **Android Build Support** module installed
- Set Android SDK path: **Edit → Preferences → External Tools**

#### "Pairing failed"
- Verify pairing code is **6 digits**
- Pairing code **expired** (generate new one)
- Use **manual pairing guide** if auto-pairing fails

#### "Connection failed"
- Confirm **Pairing port** ≠ **Connection port**
- Re-enable wireless debugging on Android
- Check firewall settings

#### Network Test
```bash
# Test connection from PC to Android
ping 192.168.1.100

# Windows: Port test
telnet 192.168.1.100 5555

# macOS/Linux: Port test
nc -zv 192.168.1.100 5555
```

---

## 💡 Tips

### Development Efficiency
- Use **5GHz WiFi** (faster)
- Enable **Development Build** + **Script Debugging**
- Use **real-time logs**: `adb logcat` alongside Unity Console

### Security
- **Disable wireless debugging** after development
- **Never use public WiFi**
- Corporate WiFi may block connections

---

## 📞 Help

- **GitHub Issues**: [Bug Reports](https://github.com/ovthemoon/unity-wireless-debug-manager/issues)
- **Unity Docs**: [Android Build Process](https://docs.unity3d.com/Manual/android-BuildProcess.html)
- **ADB Guide**: [Official Documentation~](https://developer.android.com/studio/command-line/adb)

---

**🎉 Now develop wirelessly with freedom!**