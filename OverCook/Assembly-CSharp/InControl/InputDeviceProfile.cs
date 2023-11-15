using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200033D RID: 829
	public abstract class InputDeviceProfile
	{
		// Token: 0x06000F89 RID: 3977 RVA: 0x000489BC File Offset: 0x00046DBC
		public InputDeviceProfile()
		{
			this.Name = string.Empty;
			this.Meta = string.Empty;
			this.AnalogMappings = new InputControlMapping[0];
			this.ButtonMappings = new InputControlMapping[0];
			this.SupportedPlatforms = new string[0];
			this.ExcludePlatforms = new string[0];
			this.MinUnityVersion = new VersionInfo(3, 0, 0, 0);
			this.MaxUnityVersion = new VersionInfo(2061, 7, 28, 0);
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00048A4E File Offset: 0x00046E4E
		// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00048A56 File Offset: 0x00046E56
		[SerializeField]
		public string Name { get; protected set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00048A5F File Offset: 0x00046E5F
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x00048A67 File Offset: 0x00046E67
		[SerializeField]
		public string Meta { get; protected set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000F8E RID: 3982 RVA: 0x00048A70 File Offset: 0x00046E70
		// (set) Token: 0x06000F8F RID: 3983 RVA: 0x00048A78 File Offset: 0x00046E78
		[SerializeField]
		public InputControlMapping[] AnalogMappings { get; protected set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000F90 RID: 3984 RVA: 0x00048A81 File Offset: 0x00046E81
		// (set) Token: 0x06000F91 RID: 3985 RVA: 0x00048A89 File Offset: 0x00046E89
		[SerializeField]
		public InputControlMapping[] ButtonMappings { get; protected set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00048A92 File Offset: 0x00046E92
		// (set) Token: 0x06000F93 RID: 3987 RVA: 0x00048A9A File Offset: 0x00046E9A
		[SerializeField]
		public string[] SupportedPlatforms { get; protected set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000F94 RID: 3988 RVA: 0x00048AA3 File Offset: 0x00046EA3
		// (set) Token: 0x06000F95 RID: 3989 RVA: 0x00048AAB File Offset: 0x00046EAB
		[SerializeField]
		public string[] ExcludePlatforms { get; protected set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000F96 RID: 3990 RVA: 0x00048AB4 File Offset: 0x00046EB4
		// (set) Token: 0x06000F97 RID: 3991 RVA: 0x00048ABC File Offset: 0x00046EBC
		[SerializeField]
		public VersionInfo MinUnityVersion { get; protected set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00048AC5 File Offset: 0x00046EC5
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x00048ACD File Offset: 0x00046ECD
		[SerializeField]
		public VersionInfo MaxUnityVersion { get; protected set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x00048AD6 File Offset: 0x00046ED6
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x00048ADE File Offset: 0x00046EDE
		[SerializeField]
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			protected set
			{
				this.sensitivity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000F9C RID: 3996 RVA: 0x00048AEC File Offset: 0x00046EEC
		// (set) Token: 0x06000F9D RID: 3997 RVA: 0x00048AF4 File Offset: 0x00046EF4
		[SerializeField]
		public float LowerDeadZone
		{
			get
			{
				return this.lowerDeadZone;
			}
			protected set
			{
				this.lowerDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000F9E RID: 3998 RVA: 0x00048B02 File Offset: 0x00046F02
		// (set) Token: 0x06000F9F RID: 3999 RVA: 0x00048B0A File Offset: 0x00046F0A
		[SerializeField]
		public float UpperDeadZone
		{
			get
			{
				return this.upperDeadZone;
			}
			protected set
			{
				this.upperDeadZone = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x00048B18 File Offset: 0x00046F18
		public bool IsSupportedOnThisPlatform
		{
			get
			{
				if (!this.IsSupportedOnThisVersionOfUnity)
				{
					return false;
				}
				if (this.ExcludePlatforms != null)
				{
					foreach (string text in this.ExcludePlatforms)
					{
						if (InputManager.Platform.Contains(text.ToUpperInvariant()))
						{
							return false;
						}
					}
				}
				if (this.SupportedPlatforms == null || this.SupportedPlatforms.Length == 0)
				{
					return true;
				}
				foreach (string text2 in this.SupportedPlatforms)
				{
					if (InputManager.Platform.Contains(text2.ToUpperInvariant()))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00048BCC File Offset: 0x00046FCC
		private bool IsSupportedOnThisVersionOfUnity
		{
			get
			{
				VersionInfo a = VersionInfo.UnityVersion();
				return a >= this.MinUnityVersion && a <= this.MaxUnityVersion;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000FA2 RID: 4002
		public abstract bool IsKnown { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000FA3 RID: 4003
		public abstract bool IsJoystick { get; }

		// Token: 0x06000FA4 RID: 4004
		public abstract bool HasJoystickName(string joystickName);

		// Token: 0x06000FA5 RID: 4005
		public abstract bool HasLastResortRegex(string joystickName);

		// Token: 0x06000FA6 RID: 4006
		public abstract bool HasJoystickOrRegexName(string joystickName);

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00048BFF File Offset: 0x00046FFF
		public bool IsNotJoystick
		{
			get
			{
				return !this.IsJoystick;
			}
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00048C0A File Offset: 0x0004700A
		internal static void Hide(Type type)
		{
			InputDeviceProfile.hideList.Add(type);
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000FA9 RID: 4009 RVA: 0x00048C18 File Offset: 0x00047018
		internal bool IsHidden
		{
			get
			{
				return InputDeviceProfile.hideList.Contains(base.GetType());
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x00048C2A File Offset: 0x0004702A
		public int AnalogCount
		{
			get
			{
				return this.AnalogMappings.Length;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00048C34 File Offset: 0x00047034
		public int ButtonCount
		{
			get
			{
				return this.ButtonMappings.Length;
			}
		}

		// Token: 0x04000BCC RID: 3020
		private static HashSet<Type> hideList = new HashSet<Type>();

		// Token: 0x04000BCD RID: 3021
		private float sensitivity = 1f;

		// Token: 0x04000BCE RID: 3022
		private float lowerDeadZone;

		// Token: 0x04000BCF RID: 3023
		private float upperDeadZone = 1f;

		// Token: 0x04000BD0 RID: 3024
		protected static InputControlSource MouseButton0 = new UnityMouseButtonSource(0);

		// Token: 0x04000BD1 RID: 3025
		protected static InputControlSource MouseButton1 = new UnityMouseButtonSource(1);

		// Token: 0x04000BD2 RID: 3026
		protected static InputControlSource MouseButton2 = new UnityMouseButtonSource(2);

		// Token: 0x04000BD3 RID: 3027
		protected static InputControlSource MouseXAxis = new UnityMouseAxisSource("x");

		// Token: 0x04000BD4 RID: 3028
		protected static InputControlSource MouseYAxis = new UnityMouseAxisSource("y");

		// Token: 0x04000BD5 RID: 3029
		protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource("z");
	}
}
