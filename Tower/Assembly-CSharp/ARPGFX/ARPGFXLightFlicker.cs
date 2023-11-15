using System;
using UnityEngine;

namespace ARPGFX
{
	// Token: 0x0200006F RID: 111
	public class ARPGFXLightFlicker : MonoBehaviour
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00007547 File Offset: 0x00005747
		private void Start()
		{
			this.originalColor = base.GetComponent<Light>().color;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000755A File Offset: 0x0000575A
		private void Update()
		{
			base.GetComponent<Light>().color = this.originalColor * this.EvalWave();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00007578 File Offset: 0x00005778
		private float EvalWave()
		{
			float num = (Time.time + this.phase) * this.frequency;
			num -= Mathf.Floor(num);
			float num2;
			if (this.waveFunction == "sin")
			{
				num2 = Mathf.Sin(num * 2f * 3.1415927f);
			}
			else if (this.waveFunction == "tri")
			{
				if (num < 0.5f)
				{
					num2 = 4f * num - 1f;
				}
				else
				{
					num2 = -4f * num + 3f;
				}
			}
			else if (this.waveFunction == "sqr")
			{
				if (num < 0.5f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = -1f;
				}
			}
			else if (this.waveFunction == "saw")
			{
				num2 = num;
			}
			else if (this.waveFunction == "inv")
			{
				num2 = 1f - num;
			}
			else if (this.waveFunction == "noise")
			{
				num2 = 1f - Random.value * 2f;
			}
			else
			{
				num2 = 1f;
			}
			return num2 * this.amplitude + this.startValue;
		}

		// Token: 0x04000187 RID: 391
		[Header("sin,tri,sqr,saw,inv,noise")]
		public string waveFunction = "sin";

		// Token: 0x04000188 RID: 392
		public float startValue;

		// Token: 0x04000189 RID: 393
		[Header("Amplitude of wave")]
		public float amplitude = 1f;

		// Token: 0x0400018A RID: 394
		[Header("Start point inside on wave cycle")]
		public float phase;

		// Token: 0x0400018B RID: 395
		[Header("Frequency per second")]
		public float frequency = 0.5f;

		// Token: 0x0400018C RID: 396
		private Color originalColor;
	}
}
