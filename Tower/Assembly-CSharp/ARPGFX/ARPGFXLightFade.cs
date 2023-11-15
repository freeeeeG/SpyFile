using System;
using UnityEngine;

namespace ARPGFX
{
	// Token: 0x0200006E RID: 110
	public class ARPGFXLightFade : MonoBehaviour
	{
		// Token: 0x0600019A RID: 410 RVA: 0x00007430 File Offset: 0x00005630
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

		// Token: 0x0600019B RID: 411 RVA: 0x0000748C File Offset: 0x0000568C
		private void OnEnable()
		{
			if (base.gameObject.GetComponent<Light>())
			{
				this.li = base.gameObject.GetComponent<Light>();
				this.initIntensity = this.li.intensity;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000074C4 File Offset: 0x000056C4
		private void Update()
		{
			if (base.gameObject.GetComponent<Light>())
			{
				this.li.intensity -= this.initIntensity * (Time.deltaTime / this.life);
				if (this.killAfterLife && this.li.intensity <= 0f)
				{
					Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x04000183 RID: 387
		[Header("Seconds to dim the light")]
		public float life = 0.2f;

		// Token: 0x04000184 RID: 388
		public bool killAfterLife = true;

		// Token: 0x04000185 RID: 389
		private Light li;

		// Token: 0x04000186 RID: 390
		private float initIntensity;
	}
}
