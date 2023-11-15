using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x0200000C RID: 12
	public class SimplePulsatingOutline : SimplePulsating
	{
		// Token: 0x06000035 RID: 53 RVA: 0x00002988 File Offset: 0x00000B88
		protected override void SetHighlighterSettings(float pong)
		{
			float meshOutlineThickness = Mathf.Lerp(this.defaultThickness, this.maxThickness, pong);
			float power = Mathf.Lerp(this.defaultPower, this.maxPower, pong);
			this.highlighter.Settings.MeshOutlineThickness = meshOutlineThickness;
			this.highlighter.Settings.InnerGlowFront.Power = power;
		}

		// Token: 0x0400001C RID: 28
		[Range(0f, 0.1f)]
		public float maxThickness;

		// Token: 0x0400001D RID: 29
		[Range(0f, 0.1f)]
		public float defaultThickness;

		// Token: 0x0400001E RID: 30
		[Range(0f, 3f)]
		public float maxPower;

		// Token: 0x0400001F RID: 31
		[Range(0f, 3f)]
		public float defaultPower;
	}
}
