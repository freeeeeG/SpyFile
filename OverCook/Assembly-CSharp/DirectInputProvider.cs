using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class DirectInputProvider : Singleton<DirectInputProvider>, IGamepadInputProvider
{
	// Token: 0x0600081B RID: 2075 RVA: 0x000319B4 File Offset: 0x0002FDB4
	static DirectInputProvider()
	{
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.A, ButtonCode.Button0);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.B, ButtonCode.Button1);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.X, ButtonCode.Button2);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.Y, ButtonCode.Button3);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.LB, ButtonCode.Button4);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.RB, ButtonCode.Button5);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.Back, ButtonCode.Button6);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.Start, ButtonCode.Button7);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.LeftAnalog, ButtonCode.Button8);
		DirectInputProvider.sc_buttonsLookup.Add(ControlPadInput.Button.RightAnalog, ButtonCode.Button9);
		DirectInputProvider.sc_axisLookup = new Dictionary<ControlPadInput.Value, AxisCode>();
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.LStickX, AxisCode.Axis1);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.LStickY, AxisCode.Axis2);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.RStickX, AxisCode.Axis4);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.RStickY, AxisCode.Axis5);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.DPadX, AxisCode.Axis6);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.DPadY, AxisCode.Axis7);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.LTrigger, AxisCode.Axis9);
		DirectInputProvider.sc_axisLookup.Add(ControlPadInput.Value.RTrigger, AxisCode.Axis10);
		DirectInputProvider.sc_keyNames = new string[12, 10];
		DirectInputProvider.sc_valueNames = new string[12, 10];
		for (int i = 0; i < 12; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				DirectInputProvider.sc_keyNames[i, j] = string.Concat(new object[]
				{
					"joystick ",
					i + 1,
					" button ",
					j
				});
			}
			for (int k = 0; k < 10; k++)
			{
				DirectInputProvider.sc_valueNames[i, k] = string.Concat(new object[]
				{
					"joystick ",
					i + 1,
					" analog ",
					k
				});
			}
		}
		DirectInputProvider.sc_orderedJoysticks = new string[Enum.GetValues(typeof(ControlPadInput.PadNum)).Length];
		DirectInputProvider.UpdateAssignedJoysticks();
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00031BB0 File Offset: 0x0002FFB0
	public bool IsPadAttached(ControlPadInput.PadNum _pad)
	{
		string[] joystickNames = Input.GetJoystickNames();
		string joystickName = this.GetJoystickName(_pad);
		return joystickName != string.Empty && joystickNames.Contains(joystickName);
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00031BE4 File Offset: 0x0002FFE4
	private string GetJoystickName(ControlPadInput.PadNum _padNum)
	{
		return DirectInputProvider.sc_orderedJoysticks[(int)_padNum];
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00031BF0 File Offset: 0x0002FFF0
	private static void UpdateAssignedJoysticks()
	{
		string[] joystickNames = Input.GetJoystickNames();
		if (joystickNames != null)
		{
			for (int i = 0; i < DirectInputProvider.sc_orderedJoysticks.Length; i++)
			{
				if (DirectInputProvider.sc_orderedJoysticks[i] == null)
				{
					DirectInputProvider.sc_orderedJoysticks[i] = string.Empty;
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < joystickNames.Length; j++)
					{
						if (joystickNames[j] != null && joystickNames[j].Equals(DirectInputProvider.sc_orderedJoysticks[i]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						DirectInputProvider.sc_orderedJoysticks[i] = string.Empty;
					}
				}
			}
			for (int k = 0; k < joystickNames.Length; k++)
			{
				if (joystickNames[k] != null)
				{
					bool flag2 = false;
					for (int l = 0; l < DirectInputProvider.sc_orderedJoysticks.Length; l++)
					{
						if (DirectInputProvider.sc_orderedJoysticks[l].Equals(joystickNames[k]))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						for (int m = 0; m < DirectInputProvider.sc_orderedJoysticks.Length; m++)
						{
							if (DirectInputProvider.sc_orderedJoysticks[m].Equals(string.Empty))
							{
								DirectInputProvider.sc_orderedJoysticks[m] = joystickNames[k];
								break;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00031D38 File Offset: 0x00030138
	public bool IsDown(ControlPadInput.PadNum _pad, ControlPadInput.Button _button)
	{
		if (_button == ControlPadInput.Button.LTrigger)
		{
			return this.GetValue(_pad, ControlPadInput.Value.LTrigger) > 0.5f;
		}
		if (_button == ControlPadInput.Button.RTrigger)
		{
			return this.GetValue(_pad, ControlPadInput.Value.RTrigger) > 0.5f;
		}
		if (_button == ControlPadInput.Button.DPadLeft)
		{
			return this.GetValue(_pad, ControlPadInput.Value.DPadX) < -0.5f;
		}
		if (_button == ControlPadInput.Button.DPadRight)
		{
			return this.GetValue(_pad, ControlPadInput.Value.DPadX) > 0.5f;
		}
		if (_button == ControlPadInput.Button.DPadUp)
		{
			return this.GetValue(_pad, ControlPadInput.Value.DPadY) < -0.5f;
		}
		if (_button == ControlPadInput.Button.DPadDown)
		{
			return this.GetValue(_pad, ControlPadInput.Value.DPadY) > 0.5f;
		}
		string unityKeyName = DirectInputProvider.GetUnityKeyName(_button, _pad);
		return Input.GetKey(unityKeyName);
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00031DE4 File Offset: 0x000301E4
	public float GetValue(ControlPadInput.PadNum _pad, ControlPadInput.Value _value)
	{
		switch (_value)
		{
		case ControlPadInput.Value.LStickX:
			return DirectInputProvider.GetDeadenedAxis(_pad, ControlPadInput.Value.LStickX, ControlPadInput.Value.LStickY).x;
		case ControlPadInput.Value.LStickY:
			return DirectInputProvider.GetDeadenedAxis(_pad, ControlPadInput.Value.LStickX, ControlPadInput.Value.LStickY).y;
		case ControlPadInput.Value.RStickX:
			return DirectInputProvider.GetDeadenedAxis(_pad, ControlPadInput.Value.RStickX, ControlPadInput.Value.RStickY).x;
		case ControlPadInput.Value.RStickY:
			return DirectInputProvider.GetDeadenedAxis(_pad, ControlPadInput.Value.RStickX, ControlPadInput.Value.RStickY).y;
		case ControlPadInput.Value.DPadY:
			return -DirectInputProvider.GetDeadenedAxis(_pad, ControlPadInput.Value.DPadX, ControlPadInput.Value.DPadY).y;
		}
		string unityAxisName = DirectInputProvider.GetUnityAxisName(_value, _pad);
		return Input.GetAxis(unityAxisName);
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00031E7D File Offset: 0x0003027D
	private static string GetUnityKeyName(ControlPadInput.Button _button, ControlPadInput.PadNum _pad)
	{
		return DirectInputProvider.sc_keyNames[(int)_pad, (int)DirectInputProvider.sc_buttonsLookup[_button]];
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00031E95 File Offset: 0x00030295
	private static string GetUnityAxisName(ControlPadInput.Value _value, ControlPadInput.PadNum _pad)
	{
		return DirectInputProvider.sc_valueNames[(int)_pad, (int)DirectInputProvider.sc_axisLookup[_value]];
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00031EB0 File Offset: 0x000302B0
	private static Vector2 GetDeadenedAxis(ControlPadInput.PadNum _pad, ControlPadInput.Value _valuex, ControlPadInput.Value _valuey)
	{
		string unityAxisName = DirectInputProvider.GetUnityAxisName(_valuex, _pad);
		string unityAxisName2 = DirectInputProvider.GetUnityAxisName(_valuey, _pad);
		Vector2 result = new Vector2(Input.GetAxis(unityAxisName), Input.GetAxis(unityAxisName2));
		if (result.sqrMagnitude > 0.16000001f)
		{
			return result;
		}
		return Vector2.zero;
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00031EF8 File Offset: 0x000302F8
	public static void Update()
	{
		DirectInputProvider.UpdateAssignedJoysticks();
	}

	// Token: 0x040006F6 RID: 1782
	public const float sc_axisDeadZone = 0.4f;

	// Token: 0x040006F7 RID: 1783
	public static int ControllerCount = 11;

	// Token: 0x040006F8 RID: 1784
	private static string[] sc_orderedJoysticks;

	// Token: 0x040006F9 RID: 1785
	private static Dictionary<ControlPadInput.Button, ButtonCode> sc_buttonsLookup = new Dictionary<ControlPadInput.Button, ButtonCode>();

	// Token: 0x040006FA RID: 1786
	private static Dictionary<ControlPadInput.Value, AxisCode> sc_axisLookup;

	// Token: 0x040006FB RID: 1787
	private static string[,] sc_keyNames;

	// Token: 0x040006FC RID: 1788
	private static string[,] sc_valueNames;
}
