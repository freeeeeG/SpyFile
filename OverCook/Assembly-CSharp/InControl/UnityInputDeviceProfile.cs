using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000341 RID: 833
	public class UnityInputDeviceProfile : InputDeviceProfile
	{
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00048D35 File Offset: 0x00047135
		public UnityInputDeviceProfile()
		{
			base.Sensitivity = 1f;
			base.LowerDeadZone = 0.2f;
			base.UpperDeadZone = 0.9f;
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x00048D5E File Offset: 0x0004715E
		public override bool IsKnown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x00048D64 File Offset: 0x00047164
		public override bool IsJoystick
		{
			get
			{
				return this.LastResortRegex != null || (this.JoystickNames != null && this.JoystickNames.Length > 0) || (this.JoystickRegex != null && this.JoystickRegex.Length > 0);
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00048DB4 File Offset: 0x000471B4
		public override bool HasJoystickName(string joystickName)
		{
			if (base.IsNotJoystick)
			{
				return false;
			}
			if (this.JoystickNames != null && this.JoystickNames.Contains(joystickName, StringComparer.OrdinalIgnoreCase))
			{
				return true;
			}
			if (this.JoystickRegex != null)
			{
				for (int i = 0; i < this.JoystickRegex.Length; i++)
				{
					if (Regex.IsMatch(joystickName, this.JoystickRegex[i], RegexOptions.IgnoreCase))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00048E2C File Offset: 0x0004722C
		public override bool HasLastResortRegex(string joystickName)
		{
			return !base.IsNotJoystick && this.LastResortRegex != null && Regex.IsMatch(joystickName, this.LastResortRegex, RegexOptions.IgnoreCase);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00048E55 File Offset: 0x00047255
		public override bool HasJoystickOrRegexName(string joystickName)
		{
			return this.HasJoystickName(joystickName) || this.HasLastResortRegex(joystickName);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00048E6D File Offset: 0x0004726D
		protected static InputControlSource Button(int index)
		{
			return new UnityButtonSource(index);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00048E75 File Offset: 0x00047275
		protected static InputControlSource Analog(int index)
		{
			return new UnityAnalogSource(index);
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00048E80 File Offset: 0x00047280
		protected static InputControlMapping LeftStickLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00048EC4 File Offset: 0x000472C4
		protected static InputControlMapping LeftStickRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00048F08 File Offset: 0x00047308
		protected static InputControlMapping LeftStickUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00048F4C File Offset: 0x0004734C
		protected static InputControlMapping LeftStickDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00048F90 File Offset: 0x00047390
		protected static InputControlMapping RightStickLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00048FD4 File Offset: 0x000473D4
		protected static InputControlMapping RightStickRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00049018 File Offset: 0x00047418
		protected static InputControlMapping RightStickUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0004905C File Offset: 0x0004745C
		protected static InputControlMapping RightStickDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x000490A0 File Offset: 0x000474A0
		protected static InputControlMapping LeftTriggerMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Left Trigger",
				Target = InputControlType.LeftTrigger,
				Source = analog,
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x000490EC File Offset: 0x000474EC
		protected static InputControlMapping RightTriggerMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "Right Trigger",
				Target = InputControlType.RightTrigger,
				Source = analog,
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00049138 File Offset: 0x00047538
		protected static InputControlMapping DPadLeftMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0004917C File Offset: 0x0004757C
		protected static InputControlMapping DPadRightMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x000491C0 File Offset: 0x000475C0
		protected static InputControlMapping DPadUpMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00049204 File Offset: 0x00047604
		protected static InputControlMapping DPadDownMapping(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00049248 File Offset: 0x00047648
		protected static InputControlMapping DPadUpMapping2(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = analog,
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0004928C File Offset: 0x0004768C
		protected static InputControlMapping DPadDownMapping2(InputControlSource analog)
		{
			return new InputControlMapping
			{
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = analog,
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		// Token: 0x04000BE4 RID: 3044
		[SerializeField]
		protected string[] JoystickNames;

		// Token: 0x04000BE5 RID: 3045
		[SerializeField]
		protected string[] JoystickRegex;

		// Token: 0x04000BE6 RID: 3046
		[SerializeField]
		protected string LastResortRegex;

		// Token: 0x04000BE7 RID: 3047
		protected static InputControlSource Button0 = UnityInputDeviceProfile.Button(0);

		// Token: 0x04000BE8 RID: 3048
		protected static InputControlSource Button1 = UnityInputDeviceProfile.Button(1);

		// Token: 0x04000BE9 RID: 3049
		protected static InputControlSource Button2 = UnityInputDeviceProfile.Button(2);

		// Token: 0x04000BEA RID: 3050
		protected static InputControlSource Button3 = UnityInputDeviceProfile.Button(3);

		// Token: 0x04000BEB RID: 3051
		protected static InputControlSource Button4 = UnityInputDeviceProfile.Button(4);

		// Token: 0x04000BEC RID: 3052
		protected static InputControlSource Button5 = UnityInputDeviceProfile.Button(5);

		// Token: 0x04000BED RID: 3053
		protected static InputControlSource Button6 = UnityInputDeviceProfile.Button(6);

		// Token: 0x04000BEE RID: 3054
		protected static InputControlSource Button7 = UnityInputDeviceProfile.Button(7);

		// Token: 0x04000BEF RID: 3055
		protected static InputControlSource Button8 = UnityInputDeviceProfile.Button(8);

		// Token: 0x04000BF0 RID: 3056
		protected static InputControlSource Button9 = UnityInputDeviceProfile.Button(9);

		// Token: 0x04000BF1 RID: 3057
		protected static InputControlSource Button10 = UnityInputDeviceProfile.Button(10);

		// Token: 0x04000BF2 RID: 3058
		protected static InputControlSource Button11 = UnityInputDeviceProfile.Button(11);

		// Token: 0x04000BF3 RID: 3059
		protected static InputControlSource Button12 = UnityInputDeviceProfile.Button(12);

		// Token: 0x04000BF4 RID: 3060
		protected static InputControlSource Button13 = UnityInputDeviceProfile.Button(13);

		// Token: 0x04000BF5 RID: 3061
		protected static InputControlSource Button14 = UnityInputDeviceProfile.Button(14);

		// Token: 0x04000BF6 RID: 3062
		protected static InputControlSource Button15 = UnityInputDeviceProfile.Button(15);

		// Token: 0x04000BF7 RID: 3063
		protected static InputControlSource Button16 = UnityInputDeviceProfile.Button(16);

		// Token: 0x04000BF8 RID: 3064
		protected static InputControlSource Button17 = UnityInputDeviceProfile.Button(17);

		// Token: 0x04000BF9 RID: 3065
		protected static InputControlSource Button18 = UnityInputDeviceProfile.Button(18);

		// Token: 0x04000BFA RID: 3066
		protected static InputControlSource Button19 = UnityInputDeviceProfile.Button(19);

		// Token: 0x04000BFB RID: 3067
		protected static InputControlSource Analog0 = UnityInputDeviceProfile.Analog(0);

		// Token: 0x04000BFC RID: 3068
		protected static InputControlSource Analog1 = UnityInputDeviceProfile.Analog(1);

		// Token: 0x04000BFD RID: 3069
		protected static InputControlSource Analog2 = UnityInputDeviceProfile.Analog(2);

		// Token: 0x04000BFE RID: 3070
		protected static InputControlSource Analog3 = UnityInputDeviceProfile.Analog(3);

		// Token: 0x04000BFF RID: 3071
		protected static InputControlSource Analog4 = UnityInputDeviceProfile.Analog(4);

		// Token: 0x04000C00 RID: 3072
		protected static InputControlSource Analog5 = UnityInputDeviceProfile.Analog(5);

		// Token: 0x04000C01 RID: 3073
		protected static InputControlSource Analog6 = UnityInputDeviceProfile.Analog(6);

		// Token: 0x04000C02 RID: 3074
		protected static InputControlSource Analog7 = UnityInputDeviceProfile.Analog(7);

		// Token: 0x04000C03 RID: 3075
		protected static InputControlSource Analog8 = UnityInputDeviceProfile.Analog(8);

		// Token: 0x04000C04 RID: 3076
		protected static InputControlSource Analog9 = UnityInputDeviceProfile.Analog(9);

		// Token: 0x04000C05 RID: 3077
		protected static InputControlSource Analog10 = UnityInputDeviceProfile.Analog(10);

		// Token: 0x04000C06 RID: 3078
		protected static InputControlSource Analog11 = UnityInputDeviceProfile.Analog(11);

		// Token: 0x04000C07 RID: 3079
		protected static InputControlSource Analog12 = UnityInputDeviceProfile.Analog(12);

		// Token: 0x04000C08 RID: 3080
		protected static InputControlSource Analog13 = UnityInputDeviceProfile.Analog(13);

		// Token: 0x04000C09 RID: 3081
		protected static InputControlSource Analog14 = UnityInputDeviceProfile.Analog(14);

		// Token: 0x04000C0A RID: 3082
		protected static InputControlSource Analog15 = UnityInputDeviceProfile.Analog(15);

		// Token: 0x04000C0B RID: 3083
		protected static InputControlSource Analog16 = UnityInputDeviceProfile.Analog(16);

		// Token: 0x04000C0C RID: 3084
		protected static InputControlSource Analog17 = UnityInputDeviceProfile.Analog(17);

		// Token: 0x04000C0D RID: 3085
		protected static InputControlSource Analog18 = UnityInputDeviceProfile.Analog(18);

		// Token: 0x04000C0E RID: 3086
		protected static InputControlSource Analog19 = UnityInputDeviceProfile.Analog(19);

		// Token: 0x04000C0F RID: 3087
		protected static InputControlSource MenuKey = new UnityKeyCodeSource(new KeyCode[]
		{
			KeyCode.Menu
		});

		// Token: 0x04000C10 RID: 3088
		protected static InputControlSource EscapeKey = new UnityKeyCodeSource(new KeyCode[]
		{
			KeyCode.Escape
		});
	}
}
