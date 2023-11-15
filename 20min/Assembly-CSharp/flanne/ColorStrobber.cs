using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace flanne
{
	// Token: 0x0200009C RID: 156
	public class ColorStrobber : MonoBehaviour
	{
		// Token: 0x06000577 RID: 1399 RVA: 0x0001A983 File Offset: 0x00018B83
		private void OnEnable()
		{
			base.StartCoroutine(this.StartStrobeCR());
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x0001A992 File Offset: 0x00018B92
		private IEnumerator StartStrobeCR()
		{
			int index = 0;
			for (;;)
			{
				this.targetGraphic.color = this.strobeColors[index];
				int num = index;
				index = num + 1;
				if (index >= this.strobeColors.Length)
				{
					index = 0;
				}
				yield return new WaitForSecondsRealtime(this.timeBetweenColors);
			}
			yield break;
		}

		// Token: 0x04000371 RID: 881
		[SerializeField]
		private Graphic targetGraphic;

		// Token: 0x04000372 RID: 882
		[SerializeField]
		private Color[] strobeColors;

		// Token: 0x04000373 RID: 883
		[SerializeField]
		private float timeBetweenColors;
	}
}
