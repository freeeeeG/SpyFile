using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000087 RID: 135
	[AddComponentMenu("Modular Options/Display/Field Of View Slider")]
	public sealed class FieldOfViewSlider : SliderOption
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x0000879C File Offset: 0x0000699C
		protected override void ApplySetting(float _value)
		{
			this.cam.fieldOfView = _value;
		}

		// Token: 0x040001BF RID: 447
		public Camera cam;
	}
}
