using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000068 RID: 104
	public class ETFXLightFade : MonoBehaviour
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00007004 File Offset: 0x00005204
		private void Start()
		{
			if (base.gameObject.GetComponent<Light>())
			{
				this.li = base.gameObject.GetComponent<Light>();
				this.initIntensity = this.li.intensity;
				return;
			}
			MonoBehaviour.print("No light object found on " + base.gameObject.name);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007060 File Offset: 0x00005260
		private void Update()
		{
			if (base.gameObject.GetComponent<Light>())
			{
				this.li.intensity -= this.initIntensity * (Time.deltaTime / this.life);
				if (this.killAfterLife && this.li.intensity <= 0f)
				{
					Object.Destroy(base.gameObject.GetComponent<Light>());
				}
			}
		}

		// Token: 0x04000173 RID: 371
		[Header("Seconds to dim the light")]
		public float life = 0.2f;

		// Token: 0x04000174 RID: 372
		public bool killAfterLife = true;

		// Token: 0x04000175 RID: 373
		private Light li;

		// Token: 0x04000176 RID: 374
		private float initIntensity;
	}
}
