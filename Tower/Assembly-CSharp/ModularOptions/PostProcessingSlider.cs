using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ModularOptions
{
	// Token: 0x0200008A RID: 138
	public abstract class PostProcessingSlider<T> : SliderOption where T : VolumeComponent
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x0000883A File Offset: 0x00006A3A
		protected override void Awake()
		{
			if (!this.postProcessingProfile.TryGet<T>(out this.setting))
			{
				this.setting = this.postProcessingProfile.Add<T>(true);
			}
			base.Awake();
		}

		// Token: 0x040001C3 RID: 451
		[Tooltip("Reference to global baseline profile.")]
		public VolumeProfile postProcessingProfile;

		// Token: 0x040001C4 RID: 452
		protected T setting;
	}
}
