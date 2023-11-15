using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000083 RID: 131
	[AddComponentMenu("Modular Options/Controls/Sensitivity Slider")]
	public class SensitivitySlider : SliderOption, ISliderDisplayFormatter
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x00008623 File Offset: 0x00006823
		protected override void ApplySetting(float _value)
		{
			if (this.cameraController != null)
			{
				this.cameraController.Sensitivity = _value / 10f;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008648 File Offset: 0x00006848
		public string OverrideFormatting(float _value)
		{
			return (_value / 10f).ToString();
		}

		// Token: 0x040001B9 RID: 441
		public FirstPersonCameraRotation cameraController;
	}
}
