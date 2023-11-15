using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C0 RID: 704
	public class InputManager
	{
		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000DED RID: 3565 RVA: 0x000449EC File Offset: 0x00042DEC
		// (remove) Token: 0x06000DEE RID: 3566 RVA: 0x00044A20 File Offset: 0x00042E20
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnSetup;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000DEF RID: 3567 RVA: 0x00044A54 File Offset: 0x00042E54
		// (remove) Token: 0x06000DF0 RID: 3568 RVA: 0x00044A88 File Offset: 0x00042E88
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<ulong, float> OnUpdate;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000DF1 RID: 3569 RVA: 0x00044ABC File Offset: 0x00042EBC
		// (remove) Token: 0x06000DF2 RID: 3570 RVA: 0x00044AF0 File Offset: 0x00042EF0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnReset;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000DF3 RID: 3571 RVA: 0x00044B24 File Offset: 0x00042F24
		// (remove) Token: 0x06000DF4 RID: 3572 RVA: 0x00044B58 File Offset: 0x00042F58
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnDeviceAttached;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000DF5 RID: 3573 RVA: 0x00044B8C File Offset: 0x00042F8C
		// (remove) Token: 0x06000DF6 RID: 3574 RVA: 0x00044BC0 File Offset: 0x00042FC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnDeviceDetached;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000DF7 RID: 3575 RVA: 0x00044BF4 File Offset: 0x00042FF4
		// (remove) Token: 0x06000DF8 RID: 3576 RVA: 0x00044C28 File Offset: 0x00043028
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<InputDevice> OnActiveDeviceChanged;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000DF9 RID: 3577 RVA: 0x00044C5C File Offset: 0x0004305C
		// (remove) Token: 0x06000DFA RID: 3578 RVA: 0x00044C90 File Offset: 0x00043090
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<ulong, float> OnUpdateDevices;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000DFB RID: 3579 RVA: 0x00044CC4 File Offset: 0x000430C4
		// (remove) Token: 0x06000DFC RID: 3580 RVA: 0x00044CF8 File Offset: 0x000430F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<ulong, float> OnCommitDevices;

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00044D2C File Offset: 0x0004312C
		// (set) Token: 0x06000DFE RID: 3582 RVA: 0x00044D33 File Offset: 0x00043133
		public static bool MenuWasPressed { get; private set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x00044D3B File Offset: 0x0004313B
		// (set) Token: 0x06000E00 RID: 3584 RVA: 0x00044D42 File Offset: 0x00043142
		public static bool InvertYAxis { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x00044D4A File Offset: 0x0004314A
		// (set) Token: 0x06000E02 RID: 3586 RVA: 0x00044D51 File Offset: 0x00043151
		public static bool IsSetup { get; private set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000E03 RID: 3587 RVA: 0x00044D59 File Offset: 0x00043159
		// (set) Token: 0x06000E04 RID: 3588 RVA: 0x00044D60 File Offset: 0x00043160
		internal static string Platform { get; private set; }

		// Token: 0x06000E05 RID: 3589 RVA: 0x00044D68 File Offset: 0x00043168
		[Obsolete("Calling InputManager.Setup() directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Setup()
		{
			InputManager.SetupInternal();
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00044D70 File Offset: 0x00043170
		internal static bool SetupInternal()
		{
			if (InputManager.IsSetup)
			{
				return false;
			}
			InputManager.Platform = (Utility.GetWindowsVersion() + " " + SystemInfo.deviceModel).ToUpperInvariant();
			InputManager.initialTime = 0f;
			InputManager.currentTime = 0f;
			InputManager.lastUpdateTime = 0f;
			InputManager.currentTick = 0UL;
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
			InputManager.devices.Clear();
			InputManager.Devices = new ReadOnlyCollection<InputDevice>(InputManager.devices);
			InputManager.activeDevice = InputDevice.Null;
			InputManager.playerActionSets.Clear();
			InputManager.IsSetup = true;
			if (InputManager.EnableXInput)
			{
				XInputDeviceManager.Enable();
			}
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			bool flag = true;
			if (flag)
			{
				InputManager.AddDeviceManager<UnityInputDeviceManager>();
			}
			return true;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x00044E4A File Offset: 0x0004324A
		[Obsolete("Calling InputManager.Reset() method directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Reset()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00044E54 File Offset: 0x00043254
		internal static void ResetInternal()
		{
			if (InputManager.OnReset != null)
			{
				InputManager.OnReset();
			}
			InputManager.OnSetup = null;
			InputManager.OnUpdate = null;
			InputManager.OnReset = null;
			InputManager.OnActiveDeviceChanged = null;
			InputManager.OnDeviceAttached = null;
			InputManager.OnDeviceDetached = null;
			InputManager.OnUpdateDevices = null;
			InputManager.OnCommitDevices = null;
			InputManager.DestroyDeviceManagers();
			InputManager.DestroyDevices();
			InputManager.playerActionSets.Clear();
			InputManager.IsSetup = false;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00044EBF File Offset: 0x000432BF
		[Obsolete("Calling InputManager.Update() directly is no longer supported. Use the InControlManager component to manage the lifecycle of the input manager instead.", true)]
		public static void Update()
		{
			InputManager.UpdateInternal();
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00044EC8 File Offset: 0x000432C8
		internal static void UpdateInternal()
		{
			InputManager.AssertIsSetup();
			if (InputManager.OnSetup != null)
			{
				InputManager.OnSetup();
				InputManager.OnSetup = null;
			}
			InputManager.currentTick += 1UL;
			InputManager.UpdateCurrentTime();
			float num = InputManager.currentTime - InputManager.lastUpdateTime;
			InputManager.UpdateDeviceManagers(num);
			InputManager.MenuWasPressed = false;
			InputManager.UpdateDevices(num);
			InputManager.CommitDevices(num);
			InputManager.UpdatePlayerActionSets(num);
			InputManager.UpdateActiveDevice();
			if (InputManager.OnUpdate != null)
			{
				InputManager.OnUpdate(InputManager.currentTick, num);
			}
			InputManager.lastUpdateTime = InputManager.currentTime;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00044F59 File Offset: 0x00043359
		public static void Reload()
		{
			InputManager.ResetInternal();
			InputManager.SetupInternal();
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00044F66 File Offset: 0x00043366
		private static void AssertIsSetup()
		{
			if (!InputManager.IsSetup)
			{
				throw new Exception("InputManager is not initialized. Call InputManager.Setup() first.");
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00044F80 File Offset: 0x00043380
		private static void SetZeroTickOnAllControls()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputControl[] controls = InputManager.devices[i].Controls;
				int num = controls.Length;
				for (int j = 0; j < num; j++)
				{
					InputControl inputControl = controls[j];
					if (inputControl != null)
					{
						inputControl.SetZeroTick();
					}
				}
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00044FEC File Offset: 0x000433EC
		public static void ClearInputState()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.devices[i].ClearInputState();
			}
			int count2 = InputManager.playerActionSets.Count;
			for (int j = 0; j < count2; j++)
			{
				InputManager.playerActionSets[j].ClearInputState();
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00045053 File Offset: 0x00043453
		internal static void OnApplicationFocus(bool focusState)
		{
			if (!focusState)
			{
				InputManager.SetZeroTickOnAllControls();
			}
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00045060 File Offset: 0x00043460
		internal static void OnApplicationPause(bool pauseState)
		{
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00045062 File Offset: 0x00043462
		internal static void OnApplicationQuit()
		{
			InputManager.ResetInternal();
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00045069 File Offset: 0x00043469
		internal static void OnLevelWasLoaded()
		{
			InputManager.SetZeroTickOnAllControls();
			InputManager.UpdateInternal();
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00045078 File Offset: 0x00043478
		public static void AddDeviceManager(InputDeviceManager deviceManager)
		{
			InputManager.AssertIsSetup();
			Type type = deviceManager.GetType();
			if (InputManager.deviceManagerTable.ContainsKey(type))
			{
				Logger.LogError("A device manager of type '" + type.Name + "' already exists; cannot add another.");
				return;
			}
			InputManager.deviceManagers.Add(deviceManager);
			InputManager.deviceManagerTable.Add(type, deviceManager);
			deviceManager.Update(InputManager.currentTick, InputManager.currentTime - InputManager.lastUpdateTime);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000450E9 File Offset: 0x000434E9
		public static void AddDeviceManager<T>() where T : InputDeviceManager, new()
		{
			InputManager.AddDeviceManager(Activator.CreateInstance<T>());
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x000450FC File Offset: 0x000434FC
		public static T GetDeviceManager<T>() where T : InputDeviceManager
		{
			InputDeviceManager inputDeviceManager;
			if (InputManager.deviceManagerTable.TryGetValue(typeof(T), out inputDeviceManager))
			{
				return inputDeviceManager as T;
			}
			return (T)((object)null);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00045136 File Offset: 0x00043536
		public static bool HasDeviceManager<T>() where T : InputDeviceManager
		{
			return InputManager.deviceManagerTable.ContainsKey(typeof(T));
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0004514C File Offset: 0x0004354C
		private static void UpdateCurrentTime()
		{
			if (InputManager.initialTime < 1E-45f)
			{
				InputManager.initialTime = Time.realtimeSinceStartup;
			}
			InputManager.currentTime = Mathf.Max(0f, Time.realtimeSinceStartup - InputManager.initialTime);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00045184 File Offset: 0x00043584
		private static void UpdateDeviceManagers(float deltaTime)
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000451C4 File Offset: 0x000435C4
		private static void DestroyDeviceManagers()
		{
			int count = InputManager.deviceManagers.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.deviceManagers[i].Destroy();
			}
			InputManager.deviceManagers.Clear();
			InputManager.deviceManagerTable.Clear();
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00045214 File Offset: 0x00043614
		private static void DestroyDevices()
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.StopVibration();
				inputDevice.IsAttached = false;
			}
			InputManager.devices.Clear();
			InputManager.activeDevice = InputDevice.Null;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0004526C File Offset: 0x0004366C
		private static void UpdateDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Update(InputManager.currentTick, deltaTime);
			}
			if (InputManager.OnUpdateDevices != null)
			{
				InputManager.OnUpdateDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x000452C8 File Offset: 0x000436C8
		private static void CommitDevices(float deltaTime)
		{
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice = InputManager.devices[i];
				inputDevice.Commit(InputManager.currentTick, deltaTime);
				if (inputDevice.MenuWasPressed)
				{
					InputManager.MenuWasPressed = true;
				}
			}
			if (InputManager.OnCommitDevices != null)
			{
				InputManager.OnCommitDevices(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00045338 File Offset: 0x00043738
		private static void UpdateActiveDevice()
		{
			InputDevice inputDevice = InputManager.ActiveDevice;
			int count = InputManager.devices.Count;
			for (int i = 0; i < count; i++)
			{
				InputDevice inputDevice2 = InputManager.devices[i];
				if (inputDevice2.LastChangedAfter(InputManager.ActiveDevice))
				{
					InputManager.ActiveDevice = inputDevice2;
				}
			}
			if (inputDevice != InputManager.ActiveDevice && InputManager.OnActiveDeviceChanged != null)
			{
				InputManager.OnActiveDeviceChanged(InputManager.ActiveDevice);
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000453B0 File Offset: 0x000437B0
		public static void AttachDevice(InputDevice inputDevice)
		{
			InputManager.AssertIsSetup();
			if (!inputDevice.IsSupportedOnThisPlatform)
			{
				return;
			}
			if (InputManager.devices.Contains(inputDevice))
			{
				inputDevice.IsAttached = true;
				return;
			}
			InputManager.devices.Add(inputDevice);
			InputManager.devices.Sort((InputDevice d1, InputDevice d2) => d1.SortOrder.CompareTo(d2.SortOrder));
			inputDevice.IsAttached = true;
			if (InputManager.OnDeviceAttached != null)
			{
				InputManager.OnDeviceAttached(inputDevice);
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00045434 File Offset: 0x00043834
		public static void DetachDevice(InputDevice inputDevice)
		{
			if (!inputDevice.IsAttached)
			{
				return;
			}
			if (!InputManager.IsSetup)
			{
				inputDevice.IsAttached = false;
				return;
			}
			if (!InputManager.devices.Contains(inputDevice))
			{
				inputDevice.IsAttached = false;
				return;
			}
			InputManager.devices.Remove(inputDevice);
			inputDevice.IsAttached = false;
			if (InputManager.ActiveDevice == inputDevice)
			{
				InputManager.ActiveDevice = InputDevice.Null;
			}
			if (InputManager.OnDeviceDetached != null)
			{
				InputManager.OnDeviceDetached(inputDevice);
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x000454B4 File Offset: 0x000438B4
		public static void HideDevicesWithProfile(Type type)
		{
			if (type.IsSubclassOf(typeof(UnityInputDeviceProfile)))
			{
				InputDeviceProfile.Hide(type);
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x000454D1 File Offset: 0x000438D1
		internal static void AttachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			if (InputManager.playerActionSets.Contains(playerActionSet))
			{
				Logger.LogWarning("Player action set is already attached.");
			}
			else
			{
				InputManager.playerActionSets.Add(playerActionSet);
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x000454FD File Offset: 0x000438FD
		internal static void DetachPlayerActionSet(PlayerActionSet playerActionSet)
		{
			InputManager.playerActionSets.Remove(playerActionSet);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x0004550C File Offset: 0x0004390C
		internal static void UpdatePlayerActionSets(float deltaTime)
		{
			int count = InputManager.playerActionSets.Count;
			for (int i = 0; i < count; i++)
			{
				InputManager.playerActionSets[i].Update(InputManager.currentTick, deltaTime);
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x0004554C File Offset: 0x0004394C
		public static bool AnyKeyIsPressed
		{
			get
			{
				return KeyCombo.Detect(true).Count > 0;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000E25 RID: 3621 RVA: 0x0004556A File Offset: 0x0004396A
		// (set) Token: 0x06000E26 RID: 3622 RVA: 0x00045585 File Offset: 0x00043985
		public static InputDevice ActiveDevice
		{
			get
			{
				return (InputManager.activeDevice != null) ? InputManager.activeDevice : InputDevice.Null;
			}
			private set
			{
				InputManager.activeDevice = ((value != null) ? value : InputDevice.Null);
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000E27 RID: 3623 RVA: 0x0004559D File Offset: 0x0004399D
		// (set) Token: 0x06000E28 RID: 3624 RVA: 0x000455A4 File Offset: 0x000439A4
		public static bool EnableXInput { get; internal set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x000455AC File Offset: 0x000439AC
		// (set) Token: 0x06000E2A RID: 3626 RVA: 0x000455B3 File Offset: 0x000439B3
		public static uint XInputUpdateRate { get; internal set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x000455BB File Offset: 0x000439BB
		// (set) Token: 0x06000E2C RID: 3628 RVA: 0x000455C2 File Offset: 0x000439C2
		public static uint XInputBufferSize { get; internal set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x000455CA File Offset: 0x000439CA
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x000455D1 File Offset: 0x000439D1
		public static bool EnableICade { get; internal set; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x000455D9 File Offset: 0x000439D9
		internal static VersionInfo UnityVersion
		{
			get
			{
				if (InputManager.unityVersion == null)
				{
					InputManager.unityVersion = new VersionInfo?(VersionInfo.UnityVersion());
				}
				return InputManager.unityVersion.Value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00045603 File Offset: 0x00043A03
		internal static ulong CurrentTick
		{
			get
			{
				return InputManager.currentTick;
			}
		}

		// Token: 0x04000AE6 RID: 2790
		public static readonly VersionInfo Version = VersionInfo.InControlVersion();

		// Token: 0x04000AEF RID: 2799
		private static List<InputDeviceManager> deviceManagers = new List<InputDeviceManager>();

		// Token: 0x04000AF0 RID: 2800
		private static Dictionary<Type, InputDeviceManager> deviceManagerTable = new Dictionary<Type, InputDeviceManager>();

		// Token: 0x04000AF1 RID: 2801
		private static InputDevice activeDevice = InputDevice.Null;

		// Token: 0x04000AF2 RID: 2802
		private static List<InputDevice> devices = new List<InputDevice>();

		// Token: 0x04000AF3 RID: 2803
		private static List<PlayerActionSet> playerActionSets = new List<PlayerActionSet>();

		// Token: 0x04000AF4 RID: 2804
		public static ReadOnlyCollection<InputDevice> Devices;

		// Token: 0x04000AF9 RID: 2809
		private static float initialTime;

		// Token: 0x04000AFA RID: 2810
		private static float currentTime;

		// Token: 0x04000AFB RID: 2811
		private static float lastUpdateTime;

		// Token: 0x04000AFC RID: 2812
		private static ulong currentTick;

		// Token: 0x04000AFD RID: 2813
		private static VersionInfo? unityVersion;
	}
}
