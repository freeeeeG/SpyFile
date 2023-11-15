using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace ModularOptions
{
	// Token: 0x02000089 RID: 137
	public abstract class PostProcessingDropdown<T> : DropdownOption where T : VolumeComponent
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00008805 File Offset: 0x00006A05
		protected override void Awake()
		{
			if (!this.postProcessingProfile.TryGet<T>(out this.setting))
			{
				this.setting = this.postProcessingProfile.Add<T>(true);
			}
			base.Awake();
		}

		// Token: 0x040001C1 RID: 449
		[Tooltip("Reference to global baseline profile.")]
		public VolumeProfile postProcessingProfile;

		// Token: 0x040001C2 RID: 450
		protected T setting;
	}
}
