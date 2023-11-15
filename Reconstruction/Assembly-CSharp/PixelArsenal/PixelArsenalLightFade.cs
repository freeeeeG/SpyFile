using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace PixelArsenal
{
	// Token: 0x020002B1 RID: 689
	public class PixelArsenalLightFade : MonoBehaviour
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x0002FF5C File Offset: 0x0002E15C
		private void Awake()
		{
			if (base.gameObject.GetComponent<Light2D>())
			{
				this.li = base.gameObject.GetComponent<Light2D>();
				this.initIntensity = this.li.intensity;
				return;
			}
			MonoBehaviour.print("No light object found on " + base.gameObject.name);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0002FFB8 File Offset: 0x0002E1B8
		private void OnEnable()
		{
			this.li.intensity = this.initIntensity;
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0002FFCB File Offset: 0x0002E1CB
		private void Update()
		{
			if (this.li.intensity > 0f)
			{
				this.li.intensity -= this.initIntensity * (Time.deltaTime / this.life);
			}
		}

		// Token: 0x0400093A RID: 2362
		[Header("Seconds to dim the light")]
		public float life = 0.2f;

		// Token: 0x0400093B RID: 2363
		public bool killAfterLife = true;

		// Token: 0x0400093C RID: 2364
		private Light2D li;

		// Token: 0x0400093D RID: 2365
		private float initIntensity;
	}
}
