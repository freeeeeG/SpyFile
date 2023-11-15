using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ModularOptions
{
	// Token: 0x0200008F RID: 143
	[AddComponentMenu("Modular Options/Display/PostProcessing/Film Grain Slider")]
	public sealed class FilmGrainSlider : PostProcessingSlider<FilmGrain>
	{
		// Token: 0x06000202 RID: 514 RVA: 0x00008928 File Offset: 0x00006B28
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

		// Token: 0x040001C8 RID: 456
		[Tooltip("Slider value is multiplied by this for final intensity value. Default 0.01 is for use with 0 to 100% slider.")]
		public float intensityFactor = 0.01f;
	}
}
