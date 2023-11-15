using System;

namespace InControl
{
	// Token: 0x02000344 RID: 836
	public sealed class UnknownUnityDeviceProfile : UnityInputDeviceProfile
	{
		// Token: 0x06000FE7 RID: 4071 RVA: 0x0005C438 File Offset: 0x0005A838
		public UnknownUnityDeviceProfile(string joystickName)
		{
			base.Name = "Unknown Controller";
			base.Meta = "\"" + joystickName + "\"";
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0.2f;
			base.UpperDeadZone = 0.9f;
			base.SupportedPlatforms = null;
			this.JoystickNames = new string[]
			{
				joystickName
			};
			base.AnalogMappings = new InputControlMapping[24];
			base.AnalogMappings[0] = UnityInputDeviceProfile.LeftStickLeftMapping(UnityInputDeviceProfile.Analog0);
			base.AnalogMappings[1] = UnityInputDeviceProfile.LeftStickRightMapping(UnityInputDeviceProfile.Analog0);
			base.AnalogMappings[2] = UnityInputDeviceProfile.LeftStickUpMapping(UnityInputDeviceProfile.Analog1);
			base.AnalogMappings[3] = UnityInputDeviceProfile.LeftStickDownMapping(UnityInputDeviceProfile.Analog1);
			for (int i = 0; i < 20; i++)
			{
				base.AnalogMappings[i + 4] = new InputControlMapping
				{
					Handle = "Analog " + i,
					Source = UnityInputDeviceProfile.Analog(i),
					Target = InputControlType.Analog0 + i
				};
			}
			base.ButtonMappings = new InputControlMapping[20];
			for (int j = 0; j < 20; j++)
			{
				base.ButtonMappings[j] = new InputControlMapping
				{
					Handle = "Button " + j,
					Source = UnityInputDeviceProfile.Button(j),
					Target = InputControlType.Button0 + j
				};
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0005C5A4 File Offset: 0x0005A9A4
		public sealed override bool IsKnown
		{
			get
			{
				return false;
			}
		}
	}
}
