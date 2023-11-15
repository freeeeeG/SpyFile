using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x0200000A RID: 10
	public class SimpleHit : TriggerWrapper
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002740 File Offset: 0x00000940
		protected virtual void Start()
		{
			this.DisableHighlighter();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002748 File Offset: 0x00000948
		protected override void HitTrigger()
		{
			if (this.hit != null)
			{
				base.StopCoroutine(this.hit);
			}
			this.hit = base.StartCoroutine(HighlighterUtilities.ImpulseCurve(this.overlayAlpha, this.duration, new Action<float>(this.UpdateOverlaySettings), new Action(this.EnableHighlighter), new Action(this.DisableHighlighter)));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000027AC File Offset: 0x000009AC
		private void UpdateOverlaySettings(float value)
		{
			Color color = this.highlighter.Settings.OverlayFront.Color;
			color.a = value;
			this.highlighter.Settings.OverlayFront.Color = color;
			Color color2 = this.highlighter.Settings.InnerGlowFront.Color;
			color2.a = value;
			this.highlighter.Settings.InnerGlowFront.Color = color2;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002821 File Offset: 0x00000A21
		private void EnableHighlighter()
		{
			this.highlighter.enabled = true;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000282F File Offset: 0x00000A2F
		private void DisableHighlighter()
		{
			this.highlighter.enabled = false;
		}

		// Token: 0x04000012 RID: 18
		public AnimationCurve overlayAlpha;

		// Token: 0x04000013 RID: 19
		public float duration;

		// Token: 0x04000014 RID: 20
		private Coroutine hit;
	}
}
