using System;
using UnityEngine;
using UnityEngine.Audio;

namespace ModularOptions
{
	// Token: 0x0200007D RID: 125
	[AddComponentMenu("Modular Options/Audio/Volume Slider")]
	public sealed class VolumeSlider : SliderOption
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x0000834E File Offset: 0x0000654E
		protected override void ApplySetting(float _value)
		{
			this.mixer.SetFloat(this.optionName, this.ConvertToDecibel(_value / this.slider.maxValue));
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00008375 File Offset: 0x00006575
		public float ConvertToDecibel(float _value)
		{
			return Mathf.Log10(Mathf.Max(_value, 0.0001f)) * 20f;
		}

		// Token: 0x040001B2 RID: 434
		[Tooltip("Mixer with exposed Volume parameter matching OptionName.")]
		public AudioMixer mixer;
	}
}
