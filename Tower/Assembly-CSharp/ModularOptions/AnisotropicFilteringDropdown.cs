using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000084 RID: 132
	[AddComponentMenu("Modular Options/Display/Anisotropic Filtering Dropdown")]
	public sealed class AnisotropicFilteringDropdown : DropdownOption
	{
		// Token: 0x060001EB RID: 491 RVA: 0x0000866C File Offset: 0x0000686C
		protected override void ApplySetting(int _value)
		{
			_value = Mathf.Min(_value, this.anisotropicFilteringOptions.Length - 1);
			QualitySettings.anisotropicFiltering = this.anisotropicFilteringOptions[_value];
		}

		// Token: 0x040001BA RID: 442
		[Tooltip("Setting for the corresponding dropdown index. Enable is per-texture (chosen in import settings), ForceEnable means 8xAF.")]
		public AnisotropicFiltering[] anisotropicFilteringOptions = new AnisotropicFiltering[]
		{
			AnisotropicFiltering.Disable,
			AnisotropicFiltering.Enable,
			AnisotropicFiltering.ForceEnable
		};
	}
}
