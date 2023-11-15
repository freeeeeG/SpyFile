using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CompoundRendererFeature.PostProcess
{
	// Token: 0x0200004D RID: 77
	[VolumeComponentMenu("Quibli/Stylized Color Grading")]
	[Serializable]
	public class ColorGrading : VolumeComponent
	{
		// Token: 0x04000086 RID: 134
		[Tooltip("Controls the amount to which image colors are modified.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f, true);

		// Token: 0x04000087 RID: 135
		[Space]
		public ClampedFloatParameter blueShadows = new ClampedFloatParameter(0f, 0f, 1f, true);

		// Token: 0x04000088 RID: 136
		public ClampedFloatParameter greenShadows = new ClampedFloatParameter(0f, 0f, 1f, true);

		// Token: 0x04000089 RID: 137
		public ClampedFloatParameter redHighlights = new ClampedFloatParameter(0f, 0f, 1f, true);

		// Token: 0x0400008A RID: 138
		public ClampedFloatParameter contrast = new ClampedFloatParameter(0f, 0f, 1f, true);

		// Token: 0x0400008B RID: 139
		[Space]
		public ClampedFloatParameter vibrance = new ClampedFloatParameter(0f, 0f, 1f, true);

		// Token: 0x0400008C RID: 140
		public ClampedFloatParameter saturation = new ClampedFloatParameter(0f, 0f, 1f, true);
	}
}
