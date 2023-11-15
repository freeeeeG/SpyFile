using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D1 RID: 721
	public class UnityAnalogSource : InputControlSource
	{
		// Token: 0x06000EF7 RID: 3831 RVA: 0x00048508 File Offset: 0x00046908
		public UnityAnalogSource()
		{
			UnityAnalogSource.SetupAnalogQueries();
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00048515 File Offset: 0x00046915
		public UnityAnalogSource(int analogId)
		{
			this.AnalogId = analogId;
			UnityAnalogSource.SetupAnalogQueries();
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0004852C File Offset: 0x0004692C
		public float GetValue(InputDevice inputDevice)
		{
			int joystickId = (inputDevice as UnityInputDevice).JoystickId;
			string analogKey = UnityAnalogSource.GetAnalogKey(joystickId, this.AnalogId);
			return Input.GetAxisRaw(analogKey);
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00048558 File Offset: 0x00046958
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00048568 File Offset: 0x00046968
		private static void SetupAnalogQueries()
		{
			if (UnityAnalogSource.analogQueries == null)
			{
				UnityAnalogSource.analogQueries = new string[11, 20];
				for (int i = 1; i <= 11; i++)
				{
					for (int j = 0; j < 20; j++)
					{
						UnityAnalogSource.analogQueries[i - 1, j] = string.Concat(new object[]
						{
							"joystick ",
							i,
							" analog ",
							j
						});
					}
				}
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x000485EE File Offset: 0x000469EE
		private static string GetAnalogKey(int joystickId, int analogId)
		{
			return UnityAnalogSource.analogQueries[joystickId - 1, analogId];
		}

		// Token: 0x04000BB5 RID: 2997
		private static string[,] analogQueries;

		// Token: 0x04000BB6 RID: 2998
		public int AnalogId;
	}
}
