using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000097 RID: 151
	[AddComponentMenu("Modular Options/Display/Texture Resolution Dropdown")]
	public sealed class TextureResolutionDropdown : DropdownOption
	{
		// Token: 0x0600021A RID: 538 RVA: 0x00008E0C File Offset: 0x0000700C
		protected override void ApplySetting(int _value)
		{
			_value = Mathf.Min(_value, this.textureResolutionOptions.Length - 1);
			QualitySettings.globalTextureMipmapLimit = (int)this.textureResolutionOptions[_value];
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008E2D File Offset: 0x0000702D
		public TextureResolutionDropdown()
		{
			TextureResolutionDropdown.TextureResolution[] array = new TextureResolutionDropdown.TextureResolution[3];
			array[0] = TextureResolutionDropdown.TextureResolution.Quarter;
			array[1] = TextureResolutionDropdown.TextureResolution.Half;
			this.textureResolutionOptions = array;
			base..ctor();
		}

		// Token: 0x040001D6 RID: 470
		[Tooltip("Setting for the corresponding dropdown index.")]
		public TextureResolutionDropdown.TextureResolution[] textureResolutionOptions;

		// Token: 0x02000108 RID: 264
		public enum TextureResolution
		{
			// Token: 0x040003D4 RID: 980
			Full,
			// Token: 0x040003D5 RID: 981
			Half,
			// Token: 0x040003D6 RID: 982
			Quarter,
			// Token: 0x040003D7 RID: 983
			Eighth
		}
	}
}
