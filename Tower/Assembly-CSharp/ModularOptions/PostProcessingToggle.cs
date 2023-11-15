using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ModularOptions
{
	// Token: 0x0200008B RID: 139
	public abstract class PostProcessingToggle<T> : ToggleOption where T : VolumeComponent
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000886F File Offset: 0x00006A6F
		protected override void Awake()
		{
			if (!this.postProcessingProfile.TryGet<T>(out this.setting))
			{
				this.setting = this.postProcessingProfile.Add<T>(true);
			}
			base.Awake();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000889C File Offset: 0x00006A9C
		protected override void ApplySetting(bool _value)
		{
			this.setting.active = _value;
		}

		// Token: 0x040001C5 RID: 453
		[Tooltip("Reference to global baseline profile.")]
		public VolumeProfile postProcessingProfile;

		// Token: 0x040001C6 RID: 454
		protected T setting;
	}
}
