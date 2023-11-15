using System;

// Token: 0x0200094C RID: 2380
public class RunningWeightedAverage
{
	// Token: 0x06004556 RID: 17750 RVA: 0x0018726C File Offset: 0x0018546C
	public RunningWeightedAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 15, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new float[sampleCount];
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06004557 RID: 17751 RVA: 0x00187299 File Offset: 0x00185499
	public float GetWeightedAverage
	{
		get
		{
			return this.WeightedAverage();
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06004558 RID: 17752 RVA: 0x001872A1 File Offset: 0x001854A1
	public float GetUnweightedAverage
	{
		get
		{
			return this.WeightedAverage();
		}
	}

	// Token: 0x06004559 RID: 17753 RVA: 0x001872AC File Offset: 0x001854AC
	public void AddSample(float value)
	{
		if (this.ignoreZero && value == 0f)
		{
			return;
		}
		if (value > this.max)
		{
			value = this.max;
		}
		if (value < this.min)
		{
			value = this.min;
		}
		if (this.validValues < this.samples.Length)
		{
			this.validValues++;
		}
		for (int i = 0; i < this.samples.Length - 1; i++)
		{
			this.samples[i] = this.samples[i + 1];
		}
		this.samples[this.samples.Length - 1] = value;
	}

	// Token: 0x0600455A RID: 17754 RVA: 0x00187344 File Offset: 0x00185544
	private float WeightedAverage()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = this.samples.Length - 1; i > this.samples.Length - 1 - this.validValues; i--)
		{
			float num3 = (float)(i + 1) / ((float)this.validValues + 1f);
			num += this.samples[i] * num3;
			num2 += num3;
		}
		num /= num2;
		if (float.IsNaN(num))
		{
			return 0f;
		}
		return num;
	}

	// Token: 0x0600455B RID: 17755 RVA: 0x001873BC File Offset: 0x001855BC
	private float UnweightedAverage()
	{
		float num = 0f;
		for (int i = this.samples.Length - 1; i > this.samples.Length - 1 - this.validValues; i--)
		{
			num += this.samples[i];
		}
		num /= (float)this.samples.Length;
		if (float.IsNaN(num))
		{
			return 0f;
		}
		return num;
	}

	// Token: 0x04002E07 RID: 11783
	private float[] samples;

	// Token: 0x04002E08 RID: 11784
	private float min;

	// Token: 0x04002E09 RID: 11785
	private float max;

	// Token: 0x04002E0A RID: 11786
	private bool ignoreZero;

	// Token: 0x04002E0B RID: 11787
	private int validValues;
}
