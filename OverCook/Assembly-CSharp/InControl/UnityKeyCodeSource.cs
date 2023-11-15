using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D7 RID: 727
	public class UnityKeyCodeSource : InputControlSource
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x000488CF File Offset: 0x00046CCF
		public UnityKeyCodeSource()
		{
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x000488D7 File Offset: 0x00046CD7
		public UnityKeyCodeSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000488E6 File Offset: 0x00046CE6
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00048904 File Offset: 0x00046D04
		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (Input.GetKey(this.KeyCodeList[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000BC1 RID: 3009
		public KeyCode[] KeyCodeList;
	}
}
