using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200033F RID: 831
	public class UnityInputDeviceManager : InputDeviceManager
	{
		// Token: 0x06000FB7 RID: 4023 RVA: 0x0005B941 File Offset: 0x00059D41
		public UnityInputDeviceManager()
		{
			this.AddSystemDeviceProfiles();
			this.QueryJoystickInfo();
			this.AttachDevices();
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0005B974 File Offset: 0x00059D74
		public override void Update(ulong updateTick, float deltaTime)
		{
			this.deviceRefreshTimer += deltaTime;
			if (this.deviceRefreshTimer >= 1f)
			{
				this.deviceRefreshTimer = 0f;
				this.QueryJoystickInfo();
				if (this.JoystickInfoHasChanged)
				{
					Logger.LogInfo("Change in attached Unity joysticks detected; refreshing device list.");
					this.DetachDevices();
					this.AttachDevices();
				}
			}
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0005B9D4 File Offset: 0x00059DD4
		private void QueryJoystickInfo()
		{
			this.joystickNames = Input.GetJoystickNames();
			this.joystickCount = this.joystickNames.Length;
			if (this.joystickCount > 11)
			{
				Debug.LogWarning("Oli: Joytstick entries are present beyond the index  unity itself allows access too...");
				this.joystickCount = 11;
				Array.Resize<string>(ref this.joystickNames, this.joystickCount);
			}
			this.joystickHash = 527 + this.joystickCount;
			for (int i = 0; i < this.joystickCount; i++)
			{
				this.joystickHash = this.joystickHash * 31 + this.joystickNames[i].GetHashCode();
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0005BA70 File Offset: 0x00059E70
		private bool JoystickInfoHasChanged
		{
			get
			{
				return this.joystickHash != this.lastJoystickHash || this.joystickCount != this.lastJoystickCount;
			}
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0005BA97 File Offset: 0x00059E97
		private void AttachDevices()
		{
			this.AttachKeyboardDevices();
			this.AttachJoystickDevices();
			this.lastJoystickCount = this.joystickCount;
			this.lastJoystickHash = this.joystickHash;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0005BAC0 File Offset: 0x00059EC0
		private void DetachDevices()
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.DetachDevice(this.devices[i]);
			}
			this.devices.Clear();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0005BB07 File Offset: 0x00059F07
		public void ReloadDevices()
		{
			this.QueryJoystickInfo();
			this.DetachDevices();
			this.AttachDevices();
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0005BB1B File Offset: 0x00059F1B
		private void AttachDevice(UnityInputDevice device)
		{
			this.devices.Add(device);
			InputManager.AttachDevice(device);
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0005BB30 File Offset: 0x00059F30
		private void AttachKeyboardDevices()
		{
			int count = this.systemDeviceProfiles.Count;
			for (int i = 0; i < count; i++)
			{
				InputDeviceProfile inputDeviceProfile = this.systemDeviceProfiles[i];
				if (inputDeviceProfile.IsNotJoystick && inputDeviceProfile.IsSupportedOnThisPlatform)
				{
					this.AttachDevice(new UnityInputDevice(inputDeviceProfile));
				}
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0005BB8C File Offset: 0x00059F8C
		private void AttachJoystickDevices()
		{
			try
			{
				for (int i = 0; i < this.joystickCount; i++)
				{
					this.DetectJoystickDevice(i + 1, this.joystickNames[i]);
				}
			}
			catch (Exception ex)
			{
				Logger.LogError(ex.Message);
				Logger.LogError(ex.StackTrace);
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0005BBF4 File Offset: 0x00059FF4
		private bool HasAttachedDeviceWithJoystickId(int unityJoystickId)
		{
			int count = this.devices.Count;
			for (int i = 0; i < count; i++)
			{
				UnityInputDevice unityInputDevice = this.devices[i] as UnityInputDevice;
				if (unityInputDevice != null && unityInputDevice.JoystickId == unityJoystickId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x0005BC48 File Offset: 0x0005A048
		private void DetectJoystickDevice(int unityJoystickId, string unityJoystickName)
		{
			if (this.HasAttachedDeviceWithJoystickId(unityJoystickId))
			{
				return;
			}
			if (unityJoystickName.IndexOf("webcam", StringComparison.OrdinalIgnoreCase) != -1)
			{
				return;
			}
			if (InputManager.UnityVersion < new VersionInfo(4, 5, 0, 0) && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) && unityJoystickName == "Unknown Wireless Controller")
			{
				return;
			}
			if (InputManager.UnityVersion >= new VersionInfo(4, 6, 3, 0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && string.IsNullOrEmpty(unityJoystickName))
			{
				return;
			}
			InputDeviceProfile inputDeviceProfile = null;
			if (inputDeviceProfile == null)
			{
				inputDeviceProfile = this.customDeviceProfiles.Find((InputDeviceProfile config) => config.HasJoystickName(unityJoystickName));
			}
			if (inputDeviceProfile == null)
			{
				inputDeviceProfile = this.systemDeviceProfiles.Find((InputDeviceProfile config) => config.HasJoystickName(unityJoystickName));
			}
			if (inputDeviceProfile == null)
			{
				inputDeviceProfile = this.customDeviceProfiles.Find((InputDeviceProfile config) => config.HasLastResortRegex(unityJoystickName));
			}
			if (inputDeviceProfile == null)
			{
				inputDeviceProfile = this.systemDeviceProfiles.Find((InputDeviceProfile config) => config.HasLastResortRegex(unityJoystickName));
			}
			if (inputDeviceProfile == null)
			{
				Logger.LogWarning(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" with name \"",
					unityJoystickName,
					"\" does not match any supported profiles and will be considered an unknown controller."
				}));
				UnknownUnityDeviceProfile profile = new UnknownUnityDeviceProfile(unityJoystickName);
				UnknownUnityInputDevice device = new UnknownUnityInputDevice(profile, unityJoystickId);
				this.AttachDevice(device);
				return;
			}
			if (!inputDeviceProfile.IsHidden)
			{
				UnityInputDevice device2 = new UnityInputDevice(inputDeviceProfile, unityJoystickId);
				this.AttachDevice(device2);
				Logger.LogInfo(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" matched profile ",
					inputDeviceProfile.GetType().Name,
					" (",
					inputDeviceProfile.Name,
					")"
				}));
			}
			else
			{
				Logger.LogInfo(string.Concat(new object[]
				{
					"Device ",
					unityJoystickId,
					" matching profile ",
					inputDeviceProfile.GetType().Name,
					" (",
					inputDeviceProfile.Name,
					") is hidden and will not be attached."
				}));
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0005BEA0 File Offset: 0x0005A2A0
		private void AddSystemDeviceProfile(UnityInputDeviceProfile deviceProfile)
		{
			if (deviceProfile.IsSupportedOnThisPlatform)
			{
				this.systemDeviceProfiles.Add(deviceProfile);
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0005BEBC File Offset: 0x0005A2BC
		private void AddSystemDeviceProfiles()
		{
			foreach (string typeName in UnityInputDeviceProfileList.Profiles)
			{
				UnityInputDeviceProfile deviceProfile = (UnityInputDeviceProfile)Activator.CreateInstance(Type.GetType(typeName));
				this.AddSystemDeviceProfile(deviceProfile);
			}
		}

		// Token: 0x04000BDB RID: 3035
		private const float deviceRefreshInterval = 1f;

		// Token: 0x04000BDC RID: 3036
		private float deviceRefreshTimer;

		// Token: 0x04000BDD RID: 3037
		private List<InputDeviceProfile> systemDeviceProfiles = new List<InputDeviceProfile>();

		// Token: 0x04000BDE RID: 3038
		private List<InputDeviceProfile> customDeviceProfiles = new List<InputDeviceProfile>();

		// Token: 0x04000BDF RID: 3039
		private string[] joystickNames;

		// Token: 0x04000BE0 RID: 3040
		private int lastJoystickCount;

		// Token: 0x04000BE1 RID: 3041
		private int lastJoystickHash;

		// Token: 0x04000BE2 RID: 3042
		private int joystickCount;

		// Token: 0x04000BE3 RID: 3043
		private int joystickHash;
	}
}
