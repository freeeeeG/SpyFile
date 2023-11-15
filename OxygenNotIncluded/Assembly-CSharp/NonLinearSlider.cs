using System;
using UnityEngine;

// Token: 0x02000BA5 RID: 2981
public class NonLinearSlider : KSlider
{
	// Token: 0x06005CF3 RID: 23795 RVA: 0x002207ED File Offset: 0x0021E9ED
	public static NonLinearSlider.Range[] GetDefaultRange(float maxValue)
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(100f, maxValue)
		};
	}

	// Token: 0x06005CF4 RID: 23796 RVA: 0x00220807 File Offset: 0x0021EA07
	protected override void Start()
	{
		base.Start();
		base.minValue = 0f;
		base.maxValue = 100f;
	}

	// Token: 0x06005CF5 RID: 23797 RVA: 0x00220825 File Offset: 0x0021EA25
	public void SetRanges(NonLinearSlider.Range[] ranges)
	{
		this.ranges = ranges;
	}

	// Token: 0x06005CF6 RID: 23798 RVA: 0x00220830 File Offset: 0x0021EA30
	public float GetPercentageFromValue(float value)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (value >= num2 && value <= this.ranges[i].peakValue)
			{
				float t = (value - num2) / (this.ranges[i].peakValue - num2);
				return Mathf.Lerp(num, num + this.ranges[i].width, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return 100f;
	}

	// Token: 0x06005CF7 RID: 23799 RVA: 0x002208D4 File Offset: 0x0021EAD4
	public float GetValueForPercentage(float percentage)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (percentage >= num && num + this.ranges[i].width >= percentage)
			{
				float t = (percentage - num) / this.ranges[i].width;
				return Mathf.Lerp(num2, this.ranges[i].peakValue, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return num2;
	}

	// Token: 0x06005CF8 RID: 23800 RVA: 0x00220970 File Offset: 0x0021EB70
	protected override void Set(float input, bool sendCallback)
	{
		base.Set(input, sendCallback);
	}

	// Token: 0x04003E87 RID: 16007
	public NonLinearSlider.Range[] ranges;

	// Token: 0x02001AD2 RID: 6866
	[Serializable]
	public struct Range
	{
		// Token: 0x06009840 RID: 38976 RVA: 0x00340D00 File Offset: 0x0033EF00
		public Range(float width, float peakValue)
		{
			this.width = width;
			this.peakValue = peakValue;
		}

		// Token: 0x04007AA8 RID: 31400
		public float width;

		// Token: 0x04007AA9 RID: 31401
		public float peakValue;
	}
}
