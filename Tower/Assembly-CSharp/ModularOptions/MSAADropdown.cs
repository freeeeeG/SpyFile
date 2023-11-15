using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x02000085 RID: 133
	[AddComponentMenu("Modular Options/Display/Builtin Render Pipeline/MultiSample Anti-Aliasing Dropdown")]
	public sealed class MSAADropdown : DropdownOption
	{
		// Token: 0x060001ED RID: 493 RVA: 0x000086A9 File Offset: 0x000068A9
		protected override void ApplySetting(int _value)
		{
			_value = Mathf.Min(_value, this.options.Length - 1);
			QualitySettings.antiAliasing = (int)this.options[_value];
		}

		// Token: 0x040001BB RID: 443
		[Tooltip("Setting for the corresponding dropdown index.")]
		public MSAADropdown.MSAASamples[] options = new MSAADropdown.MSAASamples[]
		{
			MSAADropdown.MSAASamples.None,
			MSAADropdown.MSAASamples.MSAA2x,
			MSAADropdown.MSAASamples.MSAA4x,
			MSAADropdown.MSAASamples.MSAA8x
		};

		// Token: 0x02000105 RID: 261
		public enum MSAASamples
		{
			// Token: 0x040003C6 RID: 966
			None = 1,
			// Token: 0x040003C7 RID: 967
			MSAA2x,
			// Token: 0x040003C8 RID: 968
			MSAA4x = 4,
			// Token: 0x040003C9 RID: 969
			MSAA8x = 8
		}
	}
}
