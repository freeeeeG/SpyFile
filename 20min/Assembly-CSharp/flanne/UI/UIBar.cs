using System;
using UnityEngine;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x0200023B RID: 571
	public class UIBar : MonoBehaviour
	{
		// Token: 0x06000C8D RID: 3213 RVA: 0x0002DF91 File Offset: 0x0002C191
		public void SetValue(int value)
		{
			this.slider.value = (float)value;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0002DFA0 File Offset: 0x0002C1A0
		public void SetMax(int maxValue)
		{
			this.slider.maxValue = (float)maxValue;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0002DFAF File Offset: 0x0002C1AF
		public void SetValue(float value)
		{
			this.slider.value = value;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0002DFBD File Offset: 0x0002C1BD
		public void SetMax(float maxValue)
		{
			this.slider.maxValue = maxValue;
		}

		// Token: 0x040008CE RID: 2254
		[SerializeField]
		private Slider slider;
	}
}
