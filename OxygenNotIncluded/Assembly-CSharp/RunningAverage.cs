using System;

// Token: 0x0200094B RID: 2379
public class RunningAverage
{
	// Token: 0x06004552 RID: 17746 RVA: 0x00187162 File Offset: 0x00185362
	public RunningAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 15, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new float[sampleCount];
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06004553 RID: 17747 RVA: 0x0018718F File Offset: 0x0018538F
	public float AverageValue
	{
		get
		{
			return this.GetAverage();
		}
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x00187198 File Offset: 0x00185398
	public void AddSample(float value)
	{
		if (value < this.min || value > this.max || (this.ignoreZero && value == 0f))
		{
			return;
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

	// Token: 0x06004555 RID: 17749 RVA: 0x00187220 File Offset: 0x00185420
	private float GetAverage()
	{
		float num = 0f;
		for (int i = this.samples.Length - 1; i > this.samples.Length - 1 - this.validValues; i--)
		{
			num += this.samples[i];
		}
		return num / (float)this.validValues;
	}

	// Token: 0x04002E02 RID: 11778
	private float[] samples;

	// Token: 0x04002E03 RID: 11779
	private float min;

	// Token: 0x04002E04 RID: 11780
	private float max;

	// Token: 0x04002E05 RID: 11781
	private bool ignoreZero;

	// Token: 0x04002E06 RID: 11782
	private int validValues;
}
