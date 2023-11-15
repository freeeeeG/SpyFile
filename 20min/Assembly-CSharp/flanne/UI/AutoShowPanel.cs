using System;
using System.Collections;
using UnityEngine;

namespace flanne.UI
{
	// Token: 0x02000204 RID: 516
	[RequireComponent(typeof(Panel))]
	public class AutoShowPanel : MonoBehaviour
	{
		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002B751 File Offset: 0x00029951
		private void Start()
		{
			this.panel = base.GetComponent<Panel>();
			base.StartCoroutine(this.AutoShowCR());
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002B76C File Offset: 0x0002996C
		private IEnumerator AutoShowCR()
		{
			yield return new WaitForSecondsRealtime(this.startTime);
			this.panel.Show();
			yield return new WaitForSecondsRealtime(this.duration);
			this.panel.Hide();
			yield break;
		}

		// Token: 0x04000801 RID: 2049
		[SerializeField]
		private float startTime;

		// Token: 0x04000802 RID: 2050
		[SerializeField]
		private float duration;

		// Token: 0x04000803 RID: 2051
		private Panel panel;
	}
}
