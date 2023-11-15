using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class PixelArsenalLightFlicker : MonoBehaviour
{
	// Token: 0x06000074 RID: 116 RVA: 0x00003FFB File Offset: 0x000021FB
	private void Start()
	{
		this.originalColor = base.GetComponent<Light>().color;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000400E File Offset: 0x0000220E
	private void Update()
	{
		base.GetComponent<Light>().color = this.originalColor * this.EvalWave();
	}

	// Token: 0x06000076 RID: 118 RVA: 0x0000402C File Offset: 0x0000222C
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

	// Token: 0x0400003F RID: 63
	public string waveFunction = "sin";

	// Token: 0x04000040 RID: 64
	public float startValue;

	// Token: 0x04000041 RID: 65
	public float amplitude = 1f;

	// Token: 0x04000042 RID: 66
	public float phase;

	// Token: 0x04000043 RID: 67
	public float frequency = 0.5f;

	// Token: 0x04000044 RID: 68
	private Color originalColor;
}
