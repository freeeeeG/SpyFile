using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D2 RID: 722
	public class UnityButtonSource : InputControlSource
	{
		// Token: 0x06000EFD RID: 3837 RVA: 0x000485FE File Offset: 0x000469FE
		public UnityButtonSource()
		{
			UnityButtonSource.SetupButtonQueries();
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0004860B File Offset: 0x00046A0B
		public UnityButtonSource(int buttonId)
		{
			this.ButtonId = buttonId;
			UnityButtonSource.SetupButtonQueries();
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004861F File Offset: 0x00046A1F
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0004863C File Offset: 0x00046A3C
		public bool GetState(InputDevice inputDevice)
		{
			int joystickId = (inputDevice as UnityInputDevice).JoystickId;
			string buttonKey = UnityButtonSource.GetButtonKey(joystickId, this.ButtonId);
			return Input.GetKey(buttonKey);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00048668 File Offset: 0x00046A68
		private static void SetupButtonQueries()
		{
			if (UnityButtonSource.buttonQueries == null)
			{
				UnityButtonSource.buttonQueries = new string[11, 20];
				for (int i = 1; i <= 11; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityButtonSource.buttonQueries[i - 1, j] = string.Concat(new object[]
						{
							"joystick ",
							i,
							" button ",
							j
						});
					}
				}
			}
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x000486EE File Offset: 0x00046AEE
		private static string GetButtonKey(int joystickId, int buttonId)
		{
			return UnityButtonSource.buttonQueries[joystickId - 1, buttonId];
		}

		// Token: 0x04000BB7 RID: 2999
		private static string[,] buttonQueries;

		// Token: 0x04000BB8 RID: 3000
		public int ButtonId;
	}
}
