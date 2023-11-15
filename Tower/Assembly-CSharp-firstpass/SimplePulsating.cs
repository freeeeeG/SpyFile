using System;
using System.Collections;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x0200000B RID: 11
	public abstract class SimplePulsating : TriggerWrapper
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002845 File Offset: 0x00000A45
		protected override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x0000284D File Offset: 0x00000A4D
		protected virtual void Start()
		{
			this.timeElapsed = -1.5707964f / this.frequency;
			this.SetHighlighterSettings(0f);
			if (this.showHighlightAlways)
			{
				this.highlighter.enabled = true;
				return;
			}
			this.highlighter.enabled = false;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000288D File Offset: 0x00000A8D
		private void Update()
		{
			if (!this.canPulsate)
			{
				return;
			}
			if (this.highlighterTrigger.IsCurrentlyTriggered)
			{
				this.currentPongValue = this.CalculatePong();
				this.SetHighlighterSettings(this.currentPongValue);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000028BD File Offset: 0x00000ABD
		private float CalculatePong()
		{
			float result = (Mathf.Sin(this.timeElapsed * this.frequency) + 1f) / 2f;
			this.timeElapsed += Time.deltaTime;
			return result;
		}

		// Token: 0x06000030 RID: 48
		protected abstract void SetHighlighterSettings(float pong);

		// Token: 0x06000031 RID: 49 RVA: 0x000028F0 File Offset: 0x00000AF0
		protected override void TriggeringEnded()
		{
			this.canPulsate = false;
			if (this.showHighlightAlways)
			{
				if (this.returnToDefaultThickness != null)
				{
					base.StopCoroutine(this.returnToDefaultThickness);
				}
				this.returnToDefaultThickness = base.StartCoroutine(this.ReturnToDefaultThickness());
				return;
			}
			this.highlighter.enabled = false;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000293F File Offset: 0x00000B3F
		protected override void TriggeringStarted()
		{
			if (!this.showHighlightAlways)
			{
				this.highlighter.enabled = true;
				this.canPulsate = true;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000295C File Offset: 0x00000B5C
		private IEnumerator ReturnToDefaultThickness()
		{
			while (this.currentPongValue > 0f + this.threshold)
			{
				this.currentPongValue = this.CalculatePong();
				this.SetHighlighterSettings(this.currentPongValue);
				yield return null;
			}
			this.canPulsate = true;
			yield break;
		}

		// Token: 0x04000015 RID: 21
		public float frequency;

		// Token: 0x04000016 RID: 22
		public float threshold = 0.02f;

		// Token: 0x04000017 RID: 23
		public bool showHighlightAlways;

		// Token: 0x04000018 RID: 24
		private float currentPongValue;

		// Token: 0x04000019 RID: 25
		private float timeElapsed;

		// Token: 0x0400001A RID: 26
		private bool canPulsate = true;

		// Token: 0x0400001B RID: 27
		private Coroutine returnToDefaultThickness;
	}
}
