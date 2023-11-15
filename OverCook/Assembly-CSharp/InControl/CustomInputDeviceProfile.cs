using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002DA RID: 730
	[Obsolete("Custom profiles are deprecated. Use the bindings API instead.", false)]
	public class CustomInputDeviceProfile : InputDeviceProfile
	{
		// Token: 0x06000F1F RID: 3871 RVA: 0x00048CA8 File Offset: 0x000470A8
		public CustomInputDeviceProfile()
		{
			base.Name = "Custom Device Profile";
			base.Meta = "Custom Device Profile";
			base.SupportedPlatforms = new string[]
			{
				"Windows",
				"Mac",
				"Linux"
			};
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0f;
			base.UpperDeadZone = 1f;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00048D16 File Offset: 0x00047116
		public sealed override bool IsKnown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00048D19 File Offset: 0x00047119
		public sealed override bool IsJoystick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00048D1C File Offset: 0x0004711C
		public sealed override bool HasJoystickName(string joystickName)
		{
			return false;
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00048D1F File Offset: 0x0004711F
		public sealed override bool HasLastResortRegex(string joystickName)
		{
			return false;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00048D22 File Offset: 0x00047122
		public sealed override bool HasJoystickOrRegexName(string joystickName)
		{
			return false;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00048D25 File Offset: 0x00047125
		protected static InputControlSource KeyCodeButton(params KeyCode[] keyCodeList)
		{
			return new UnityKeyCodeSource(keyCodeList);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x00048D2D File Offset: 0x0004712D
		protected static InputControlSource KeyCodeComboButton(params KeyCode[] keyCodeList)
		{
			return new UnityKeyCodeComboSource(keyCodeList);
		}
	}
}
