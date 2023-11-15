using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x020002BF RID: 703
	public class ETFXLightFade : MonoBehaviour
	{
		// Token: 0x06001136 RID: 4406 RVA: 0x000310BC File Offset: 0x0002F2BC
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

		// Token: 0x06001137 RID: 4407 RVA: 0x00031118 File Offset: 0x0002F318
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

		// Token: 0x04000981 RID: 2433
		[Header("Seconds to dim the light")]
		public float life = 0.2f;

		// Token: 0x04000982 RID: 2434
		public bool killAfterLife = true;

		// Token: 0x04000983 RID: 2435
		private Light li;

		// Token: 0x04000984 RID: 2436
		private float initIntensity;
	}
}
