using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D9 RID: 729
	public class UnityMouseButtonSource : InputControlSource
	{
		// Token: 0x06000F1B RID: 3867 RVA: 0x0004897B File Offset: 0x00046D7B
		public UnityMouseButtonSource()
		{
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00048983 File Offset: 0x00046D83
		public UnityMouseButtonSource(int buttonId)
		{
			this.ButtonId = buttonId;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00048992 File Offset: 0x00046D92
		public float GetValue(InputDevice inputDevice)
		{
			return (!this.GetState(inputDevice)) ? 0f : 1f;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000489AF File Offset: 0x00046DAF
		public bool GetState(InputDevice inputDevice)
		{
			return Input.GetMouseButton(this.ButtonId);
		}

		// Token: 0x04000BC3 RID: 3011
		public int ButtonId;
	}
}
