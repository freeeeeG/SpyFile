using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000096 RID: 150
	[AddComponentMenu("Modular Options/Display/Texture Quality Dropdown")]
	public sealed class TextureQualityDropdown : DropdownOption
	{
		// Token: 0x06000218 RID: 536 RVA: 0x00008DB2 File Offset: 0x00006FB2
		protected override void ApplySetting(int _value)
		{
			_value = Mathf.Min(_value, this.textureResolutionOptions.Length - 1);
			QualitySettings.globalTextureMipmapLimit = (int)this.textureResolutionOptions[_value];
			QualitySettings.anisotropicFiltering = this.anisotropicFilteringOptions[_value];
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008DE0 File Offset: 0x00006FE0
		public TextureQualityDropdown()
		{
			TextureQualityDropdown.TextureResolution[] array = new TextureQualityDropdown.TextureResolution[3];
			array[0] = TextureQualityDropdown.TextureResolution.Half;
			this.textureResolutionOptions = array;
			this.anisotropicFilteringOptions = new AnisotropicFiltering[]
			{
				AnisotropicFiltering.Disable,
				AnisotropicFiltering.Enable,
				AnisotropicFiltering.ForceEnable
			};
			base..ctor();
		}

		// Token: 0x040001D4 RID: 468
		[Tooltip("Setting for the corresponding dropdown index.")]
		public TextureQualityDropdown.TextureResolution[] textureResolutionOptions;

		// Token: 0x040001D5 RID: 469
		[Tooltip("Setting for the corresponding dropdown index. Enable is per-texture (chosen in import settings), ForceEnable means 8xAF.")]
		public AnisotropicFiltering[] anisotropicFilteringOptions;

		// Token: 0x02000107 RID: 263
		public enum TextureResolution
		{
			// Token: 0x040003CF RID: 975
			Full,
			// Token: 0x040003D0 RID: 976
			Half,
			// Token: 0x040003D1 RID: 977
			Quarter,
			// Token: 0x040003D2 RID: 978
			Eighth
		}
	}
}
