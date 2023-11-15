using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000007 RID: 7
	[RequireComponent(typeof(HighlighterTrigger))]
	public class TriggerAPITest : MonoBehaviour
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000024ED File Offset: 0x000006ED
		private void GetHighlighterTrigger()
		{
			if (this.highlighterTrigger == null)
			{
				this.highlighterTrigger = base.GetComponent<HighlighterTrigger>();
			}
			if (this.highlighter == null)
			{
				this.highlighter = base.GetComponent<Highlighter>();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002523 File Offset: 0x00000723
		private void OnEnable()
		{
			this.GetHighlighterTrigger();
			this.highlighterTrigger.OnTriggeringStarted += this.TriggeringStarted;
			this.highlighterTrigger.OnTriggeringEnded += this.TriggeringEnded;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002559 File Offset: 0x00000759
		private void OnDisable()
		{
			this.GetHighlighterTrigger();
			this.highlighterTrigger.OnTriggeringStarted -= this.TriggeringStarted;
			this.highlighterTrigger.OnTriggeringEnded -= this.TriggeringEnded;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000258F File Offset: 0x0000078F
		private void Update()
		{
			bool isCurrentlyTriggered = this.highlighterTrigger.IsCurrentlyTriggered;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025A0 File Offset: 0x000007A0
		private void TriggeringStarted()
		{
			base.StartCoroutine(HighlighterUtilities.ImpulseCurve(this.enterCurve, this.duration, new Action<float>(this.BlurIntensity), new Action(this.EnableHighlighter), null));
			base.StartCoroutine(HighlighterUtilities.ImpulseGradient(this.enterGradient, this.duration, new Action<Color>(this.OutlineColor), new Action(this.EnableHighlighter), null));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000260F File Offset: 0x0000080F
		private void TriggeringEnded()
		{
			base.StartCoroutine(HighlighterUtilities.ImpulseCurve(this.exitCurve, this.duration, new Action<float>(this.BlurIntensity), null, new Action(this.DisableHighlighter)));
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002644 File Offset: 0x00000844
		private void BlurIntensity(float value)
		{
			this.highlighter.Settings.BlurIntensity = value;
			Color outerGlowColorFront = this.highlighter.Settings.OuterGlowColorFront;
			outerGlowColorFront.a = value;
			this.highlighter.Settings.OuterGlowColorFront = outerGlowColorFront;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000268C File Offset: 0x0000088C
		private void OutlineColor(Color value)
		{
			this.highlighter.Settings.OuterGlowColorFront = value;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000269F File Offset: 0x0000089F
		private void EnableHighlighter()
		{
			this.highlighter.enabled = true;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026AD File Offset: 0x000008AD
		private void DisableHighlighter()
		{
			this.highlighter.enabled = false;
		}

		// Token: 0x0400000B RID: 11
		public AnimationCurve enterCurve;

		// Token: 0x0400000C RID: 12
		public Gradient enterGradient;

		// Token: 0x0400000D RID: 13
		public AnimationCurve exitCurve;

		// Token: 0x0400000E RID: 14
		public float duration;

		// Token: 0x0400000F RID: 15
		private HighlighterTrigger highlighterTrigger;

		// Token: 0x04000010 RID: 16
		private Highlighter highlighter;
	}
}
