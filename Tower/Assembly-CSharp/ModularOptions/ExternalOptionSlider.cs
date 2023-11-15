using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x0200009E RID: 158
	[AddComponentMenu("Modular Options/External/Slider")]
	public class ExternalOptionSlider : SliderOption
	{
		// Token: 0x06000229 RID: 553 RVA: 0x00008F91 File Offset: 0x00007191
		protected override void ApplySetting(float _value)
		{
			this.onValueChange.Invoke(_value);
		}

		// Token: 0x040001DE RID: 478
		public Slider.SliderEvent onValueChange;
	}
}
