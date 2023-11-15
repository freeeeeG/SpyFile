using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ModularOptions
{
	// Token: 0x0200008C RID: 140
	[AddComponentMenu("Modular Options/Display/PostProcessing/Bloom Slider")]
	public sealed class BloomSlider : PostProcessingSlider<Bloom>
	{
		// Token: 0x060001FE RID: 510 RVA: 0x000088B8 File Offset: 0x00006AB8
		protected override void ApplySetting(float _value)
		{
			if (_value <= this.slider.minValue)
			{
				this.setting.active = false;
				return;
			}
			this.setting.active = true;
			this.setting.intensity.value = _value * this.intensityFactor;
		}

		// Token: 0x040001C7 RID: 455
		[Tooltip("Slider value is multiplied by this for final intensity value. Default 0.01 is for use with 0 to 100% slider.")]
		public float intensityFactor = 0.01f;
	}
}
