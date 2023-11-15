using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x0200000D RID: 13
	public class SimplePulsatingOverlay : SimplePulsating
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000029EC File Offset: 0x00000BEC
		protected override void SetHighlighterSettings(float pong)
		{
			float a = Mathf.Lerp(this.defaultAlpha, this.maxAlpha, pong);
			Color color = this.highlighter.Settings.OverlayFront.Color;
			color.a = a;
			this.highlighter.Settings.OverlayFront.Color = color;
		}

		// Token: 0x04000020 RID: 32
		[Range(0f, 1f)]
		public float maxAlpha;

		// Token: 0x04000021 RID: 33
		[Range(0f, 1f)]
		public float defaultAlpha;
	}
}
