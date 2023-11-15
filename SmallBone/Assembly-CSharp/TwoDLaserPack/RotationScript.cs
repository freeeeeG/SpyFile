using System;
using UnityEngine;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	// Token: 0x02001660 RID: 5728
	public class RotationScript : MonoBehaviour
	{
		// Token: 0x06006D33 RID: 27955 RVA: 0x001389EA File Offset: 0x00136BEA
		private void Start()
		{
			this.transformCached = base.transform;
		}

		// Token: 0x06006D34 RID: 27956 RVA: 0x001389F8 File Offset: 0x00136BF8
		private void Update()
		{
			if (!this.rotationEnabled)
			{
				return;
			}
			this.transformCached.RotateAround(this.pivot.localPosition, Vector3.forward, this.rotationAmount);
		}

		// Token: 0x06006D35 RID: 27957 RVA: 0x00138A24 File Offset: 0x00136C24
		public void OnHSliderChanged()
		{
			this.rotationAmount = this.hSlider.value;
		}

		// Token: 0x040058FD RID: 22781
		public Slider hSlider;

		// Token: 0x040058FE RID: 22782
		public Transform pivot;

		// Token: 0x040058FF RID: 22783
		public bool rotationEnabled;

		// Token: 0x04005900 RID: 22784
		public float rotationAmount;

		// Token: 0x04005901 RID: 22785
		private Transform transformCached;
	}
}
