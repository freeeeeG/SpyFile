using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D6 RID: 726
	public class UnityKeyCodeComboSource : InputControlSource
	{
		// Token: 0x06000F0F RID: 3855 RVA: 0x0004885F File Offset: 0x00046C5F
		public UnityKeyCodeComboSource()
		{
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00048867 File Offset: 0x00046C67
		public UnityKeyCodeComboSource(params KeyCode[] keyCodeList)
		{
			this.KeyCodeList = keyCodeList;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00048876 File Offset: 0x00046C76
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00048894 File Offset: 0x00046C94
		public bool GetState(InputDevice inputDevice)
		{
			for (int i = 0; i < this.KeyCodeList.Length; i++)
			{
				if (!Input.GetKey(this.KeyCodeList[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000BC0 RID: 3008
		public KeyCode[] KeyCodeList;
	}
}
