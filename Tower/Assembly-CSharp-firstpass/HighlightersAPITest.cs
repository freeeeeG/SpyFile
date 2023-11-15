using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000006 RID: 6
	public class HighlightersAPITest : MonoBehaviour
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002376 File Offset: 0x00000576
		private void GetHighlighter()
		{
			if (!this.highlighter)
			{
				this.highlighter = base.GetComponent<Highlighter>();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002391 File Offset: 0x00000591
		public void TestEnablingDisabling()
		{
			this.GetHighlighter();
			this.highlighter.enabled = !this.highlighter.enabled;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023B4 File Offset: 0x000005B4
		public void TestAddingScripts()
		{
			this.highlighter = base.gameObject.AddComponent<Highlighter>();
			this.highlighter.GetRenderersInChildren();
			this.highlighter.Settings.DepthMask = DepthMask.FrontOnly;
			this.highlighter.Settings.UseOuterGlow = true;
			this.highlighter.Settings.BlurIterations = 50f;
			this.highlighter.Settings.BoxBlurSize = 0.0243f;
			this.highlighter.Settings.UseOverlay = true;
			this.highlighter.Settings.OverlayFront.Color = new Color(0.6f, 0.6f, 0f, 0f);
			this.highlighter.HighlighterValidate();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002473 File Offset: 0x00000673
		public void TestSettings()
		{
			this.GetHighlighter();
			base.StartCoroutine(HighlighterUtilities.ImpulseCurve(this.animationCurve, this.duration, new Action<float>(this.updateTest), null, null));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024A4 File Offset: 0x000006A4
		private void updateTest(float value)
		{
			Color color = this.highlighter.Settings.OverlayFront.Color;
			color.a = value;
			this.highlighter.Settings.OverlayFront.Color = color;
		}

		// Token: 0x04000008 RID: 8
		private Highlighter highlighter;

		// Token: 0x04000009 RID: 9
		public AnimationCurve animationCurve;

		// Token: 0x0400000A RID: 10
		public float duration;
	}
}
