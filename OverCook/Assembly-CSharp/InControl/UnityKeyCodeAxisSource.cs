using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D5 RID: 725
	public class UnityKeyCodeAxisSource : InputControlSource
	{
		// Token: 0x06000F0B RID: 3851 RVA: 0x000487F7 File Offset: 0x00046BF7
		public UnityKeyCodeAxisSource()
		{
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x000487FF File Offset: 0x00046BFF
		public UnityKeyCodeAxisSource(KeyCode negativeKeyCode, KeyCode positiveKeyCode)
		{
			this.NegativeKeyCode = negativeKeyCode;
			this.PositiveKeyCode = positiveKeyCode;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00048818 File Offset: 0x00046C18
		public float GetValue(InputDevice inputDevice)
		{
			int num = 0;
			if (Input.GetKey(this.NegativeKeyCode))
			{
				num--;
			}
			if (Input.GetKey(this.PositiveKeyCode))
			{
				num++;
			}
			return (float)num;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00048851 File Offset: 0x00046C51
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x04000BBE RID: 3006
		public KeyCode NegativeKeyCode;

		// Token: 0x04000BBF RID: 3007
		public KeyCode PositiveKeyCode;
	}
}
