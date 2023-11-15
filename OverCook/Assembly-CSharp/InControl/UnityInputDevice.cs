using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200033E RID: 830
	public class UnityInputDevice : InputDevice
	{
		// Token: 0x06000FAD RID: 4013 RVA: 0x0005B6AC File Offset: 0x00059AAC
		public UnityInputDevice(InputDeviceProfile profile, int joystickId) : base(profile.Name)
		{
			this.Initialize(profile, joystickId);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0005B6C2 File Offset: 0x00059AC2
		public UnityInputDevice(InputDeviceProfile profile) : base(profile.Name)
		{
			this.Initialize(profile, 0);
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0005B6D8 File Offset: 0x00059AD8
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x0005B6E0 File Offset: 0x00059AE0
		internal int JoystickId { get; private set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0005B6E9 File Offset: 0x00059AE9
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x0005B6F1 File Offset: 0x00059AF1
		public InputDeviceProfile Profile { get; protected set; }

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0005B6FC File Offset: 0x00059AFC
		private void Initialize(InputDeviceProfile profile, int joystickId)
		{
			this.Profile = profile;
			base.Meta = this.Profile.Meta;
			int analogCount = this.Profile.AnalogCount;
			for (int i = 0; i < analogCount; i++)
			{
				InputControlMapping inputControlMapping = this.Profile.AnalogMappings[i];
				InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Handle);
				inputControl.Sensitivity = Mathf.Min(this.Profile.Sensitivity, inputControlMapping.Sensitivity);
				inputControl.LowerDeadZone = Mathf.Max(this.Profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
				inputControl.UpperDeadZone = Mathf.Min(this.Profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
				inputControl.Raw = inputControlMapping.Raw;
			}
			int buttonCount = this.Profile.ButtonCount;
			for (int j = 0; j < buttonCount; j++)
			{
				InputControlMapping inputControlMapping2 = this.Profile.ButtonMappings[j];
				base.AddControl(inputControlMapping2.Target, inputControlMapping2.Handle);
			}
			this.JoystickId = joystickId;
			if (joystickId != 0)
			{
				this.SortOrder = 100 + joystickId;
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0005B820 File Offset: 0x00059C20
		public override void Update(ulong updateTick, float deltaTime)
		{
			if (this.Profile == null)
			{
				return;
			}
			int analogCount = this.Profile.AnalogCount;
			for (int i = 0; i < analogCount; i++)
			{
				InputControlMapping inputControlMapping = this.Profile.AnalogMappings[i];
				float value = inputControlMapping.Source.GetValue(this);
				InputControl control = base.GetControl(inputControlMapping.Target);
				if (!inputControlMapping.IgnoreInitialZeroValue || !control.IsOnZeroTick || !Utility.IsZero(value))
				{
					float value2 = inputControlMapping.MapValue(value);
					control.UpdateWithValue(value2, updateTick, deltaTime);
				}
			}
			int buttonCount = this.Profile.ButtonCount;
			for (int j = 0; j < buttonCount; j++)
			{
				InputControlMapping inputControlMapping2 = this.Profile.ButtonMappings[j];
				bool state = inputControlMapping2.Source.GetState(this);
				base.UpdateWithState(inputControlMapping2.Target, state, updateTick, deltaTime);
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0005B90B File Offset: 0x00059D0B
		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return this.Profile != null && this.Profile.IsSupportedOnThisPlatform;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0005B926 File Offset: 0x00059D26
		public override bool IsKnown
		{
			get
			{
				return this.Profile != null && this.Profile.IsKnown;
			}
		}

		// Token: 0x04000BD6 RID: 3030
		public const int MaxDevices = 11;

		// Token: 0x04000BD7 RID: 3031
		public const int MaxButtons = 20;

		// Token: 0x04000BD8 RID: 3032
		public const int MaxAnalogs = 20;
	}
}
