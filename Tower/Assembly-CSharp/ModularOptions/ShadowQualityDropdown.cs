using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000086 RID: 134
	[AddComponentMenu("Modular Options/Display/Builtin Render Pipeline/Shadow Quality Dropdown")]
	public sealed class ShadowQualityDropdown : DropdownOption
	{
		// Token: 0x060001EF RID: 495 RVA: 0x000086EC File Offset: 0x000068EC
		protected override void ApplySetting(int _value)
		{
			if (_value == 0)
			{
				QualitySettings.shadows = ShadowQuality.Disable;
				return;
			}
			_value = Mathf.Min(_value, this.shadowResolutionOptions.Length) - 1;
			QualitySettings.shadows = ShadowQuality.All;
			QualitySettings.shadowResolution = this.shadowResolutionOptions[_value];
			QualitySettings.shadowDistance = this.shadowDistanceOptions[_value];
			QualitySettings.shadowCascades = (int)this.shadowCascadeOptions[_value];
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008744 File Offset: 0x00006944
		public ShadowQualityDropdown()
		{
			ShadowResolution[] array = new ShadowResolution[4];
			RuntimeHelpers.InitializeArray(array, fieldof(<PrivateImplementationDetails>.BAED642339816AFFB3FE8719792D0E4CE82F12DB72B7373D244EAA65445800FE).FieldHandle);
			this.shadowResolutionOptions = array;
			this.shadowDistanceOptions = new float[]
			{
				50f,
				70f,
				90f,
				120f
			};
			this.shadowCascadeOptions = new ShadowQualityDropdown.ShadowCascades[]
			{
				ShadowQualityDropdown.ShadowCascades.Two,
				ShadowQualityDropdown.ShadowCascades.Two,
				ShadowQualityDropdown.ShadowCascades.Four,
				ShadowQualityDropdown.ShadowCascades.Four
			};
			base..ctor();
		}

		// Token: 0x040001BC RID: 444
		[Tooltip("Setting for the corresponding dropdown index plus 1, index 0 is 'Off'.")]
		public ShadowResolution[] shadowResolutionOptions;

		// Token: 0x040001BD RID: 445
		[Tooltip("Setting for the corresponding dropdown index plus 1, index 0 is 'Off'.")]
		public float[] shadowDistanceOptions;

		// Token: 0x040001BE RID: 446
		[Tooltip("Setting for the corresponding dropdown index plus 1, index 0 is 'Off'.")]
		public ShadowQualityDropdown.ShadowCascades[] shadowCascadeOptions;

		// Token: 0x02000106 RID: 262
		public enum ShadowCascades
		{
			// Token: 0x040003CB RID: 971
			None = 1,
			// Token: 0x040003CC RID: 972
			Two,
			// Token: 0x040003CD RID: 973
			Four = 4
		}
	}
}
