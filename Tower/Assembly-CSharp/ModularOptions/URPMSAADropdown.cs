using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ModularOptions
{
	// Token: 0x02000098 RID: 152
	[AddComponentMenu("Modular Options/Display/Universal Render Pipeline/MSAA Dropdown")]
	public sealed class URPMSAADropdown : DropdownOption
	{
		// Token: 0x0600021C RID: 540 RVA: 0x00008E49 File Offset: 0x00007049
		protected override void ApplySetting(int _value)
		{
			_value = Mathf.Min(_value, this.options.Length - 1);
			this.pipelineAsset.msaaSampleCount = (int)this.options[_value];
		}

		// Token: 0x040001D7 RID: 471
		public UniversalRenderPipelineAsset pipelineAsset;

		// Token: 0x040001D8 RID: 472
		[Tooltip("Setting for the corresponding dropdown index.")]
		public URPMSAADropdown.MSAASamples[] options = new URPMSAADropdown.MSAASamples[]
		{
			URPMSAADropdown.MSAASamples.None,
			URPMSAADropdown.MSAASamples.MSAA2x,
			URPMSAADropdown.MSAASamples.MSAA4x,
			URPMSAADropdown.MSAASamples.MSAA8x
		};

		// Token: 0x02000109 RID: 265
		public enum MSAASamples
		{
			// Token: 0x040003D9 RID: 985
			None = 1,
			// Token: 0x040003DA RID: 986
			MSAA2x,
			// Token: 0x040003DB RID: 987
			MSAA4x = 4,
			// Token: 0x040003DC RID: 988
			MSAA8x = 8
		}
	}
}
