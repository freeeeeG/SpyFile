using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CompoundRendererFeature.PostProcess
{
	// Token: 0x0200004F RID: 79
	[VolumeComponentMenu("Quibli/Stylized Detail")]
	[Serializable]
	public class StylizedDetail : VolumeComponent
	{
		// Token: 0x0400008F RID: 143
		[Tooltip("Controls the amount of contrast added to the image details.")]
		public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 3f, true);

		// Token: 0x04000090 RID: 144
		[Tooltip("Controls smoothing amount.")]
		public ClampedFloatParameter blur = new ClampedFloatParameter(1f, 0f, 2f, true);

		// Token: 0x04000091 RID: 145
		[Tooltip("Controls structure within the image.")]
		public ClampedFloatParameter edgePreserve = new ClampedFloatParameter(1.25f, 0f, 2f, true);

		// Token: 0x04000092 RID: 146
		[Tooltip("The distance from the camera at which the effect starts.")]
		[Space]
		public MinFloatParameter rangeStart = new MinFloatParameter(10f, 0f, false);

		// Token: 0x04000093 RID: 147
		[Tooltip("The distance from the camera at which the effect reaches its maximum radius.")]
		public MinFloatParameter rangeEnd = new MinFloatParameter(30f, 0f, false);
	}
}
