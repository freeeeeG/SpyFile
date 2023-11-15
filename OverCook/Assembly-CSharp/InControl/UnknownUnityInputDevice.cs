using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000343 RID: 835
	public class UnknownUnityInputDevice : UnityInputDevice
	{
		// Token: 0x06000FE1 RID: 4065 RVA: 0x0005C2D2 File Offset: 0x0005A6D2
		internal UnknownUnityInputDevice(InputDeviceProfile profile, int joystickId) : base(profile, joystickId)
		{
			this.AnalogSnapshot = new float[20];
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0005C2E9 File Offset: 0x0005A6E9
		// (set) Token: 0x06000FE3 RID: 4067 RVA: 0x0005C2F1 File Offset: 0x0005A6F1
		internal float[] AnalogSnapshot { get; private set; }

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0005C2FC File Offset: 0x0005A6FC
		internal void TakeSnapshot()
		{
			for (int i = 0; i < 20; i++)
			{
				InputControlType inputControlType = InputControlType.Analog0 + i;
				float num = Utility.ApplySnapping(base.GetControl(inputControlType).RawValue, 0.5f);
				this.AnalogSnapshot[i] = num;
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0005C344 File Offset: 0x0005A744
		internal UnknownDeviceControl GetFirstPressedAnalog()
		{
			for (int i = 0; i < 20; i++)
			{
				InputControlType inputControlType = InputControlType.Analog0 + i;
				float num = Utility.ApplySnapping(base.GetControl(inputControlType).RawValue, 0.5f);
				float num2 = num - this.AnalogSnapshot[i];
				Debug.Log(num);
				Debug.Log(this.AnalogSnapshot[i]);
				Debug.Log(num2);
				if (num2 > 1.9f)
				{
					return new UnknownDeviceControl(inputControlType, InputRangeType.MinusOneToOne);
				}
				if (num2 < -0.9f)
				{
					return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToMinusOne);
				}
				if (num2 > 0.9f)
				{
					return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToOne);
				}
			}
			return UnknownDeviceControl.None;
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x0005C3F4 File Offset: 0x0005A7F4
		internal UnknownDeviceControl GetFirstPressedButton()
		{
			for (int i = 0; i < 20; i++)
			{
				InputControlType inputControlType = InputControlType.Button0 + i;
				if (base.GetControl(inputControlType).IsPressed)
				{
					return new UnknownDeviceControl(inputControlType, InputRangeType.ZeroToOne);
				}
			}
			return UnknownDeviceControl.None;
		}
	}
}
