using System;
using UnityEngine;

// Token: 0x02000426 RID: 1062
public class PerformanceMonitor : DebugDisplay
{
	// Token: 0x0600133D RID: 4925 RVA: 0x0006BC83 File Offset: 0x0006A083
	public override void OnSetUp()
	{
		this.m_displayString = "PStats: Idle";
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0006BC90 File Offset: 0x0006A090
	public override void OnUpdate()
	{
		if (this.m_bStarted)
		{
			this.AddSample(1f / Time.deltaTime);
		}
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x0006BCAE File Offset: 0x0006A0AE
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		base.DrawText(ref rect, style, this.m_displayString);
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x0006BCBE File Offset: 0x0006A0BE
	public void Begin()
	{
		this.Reset();
		this.m_bStarted = true;
		this.m_displayString = "PStats: Recording";
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0006BCD8 File Offset: 0x0006A0D8
	public void End()
	{
		this.m_bStarted = false;
		this.m_displayString = string.Concat(new string[]
		{
			"PStats:  10th:",
			this.GetPercentile(10f).ToString(),
			"  25th:",
			this.GetPercentile(25f).ToString(),
			"  50th:",
			this.GetPercentile(50f).ToString(),
			"  75th:",
			this.GetPercentile(75f).ToString(),
			"  90th:",
			this.GetPercentile(90f).ToString(),
			"  Av:",
			this.GetMeanFPS().ToString(),
			"  Sd:",
			this.GetStdDev().ToString()
		});
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0006BDF8 File Offset: 0x0006A1F8
	public float GetMedianFPS()
	{
		return this.GetPercentile(50f);
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0006BE08 File Offset: 0x0006A208
	public float GetMeanFPS()
	{
		float num = 0f;
		for (int i = 0; i < 501; i++)
		{
			num += (float)(i * this.m_FPSCounts[i]);
		}
		return num / (float)this.m_numSamples;
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0006BE48 File Offset: 0x0006A248
	public float GetStdDev()
	{
		float meanFPS = this.GetMeanFPS();
		float num = 0f;
		for (int i = 0; i < 501; i++)
		{
			if (this.m_FPSCounts[i] > 0)
			{
				float num2 = (float)i - meanFPS;
				num += num2 * num2 * (float)this.m_FPSCounts[i];
			}
		}
		float f = num / (float)this.m_numSamples;
		return Mathf.Sqrt(f);
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x0006BEB0 File Offset: 0x0006A2B0
	private void AddSample(float fps)
	{
		int num = Mathf.Clamp((int)(fps + 0.5f), 0, 500);
		this.m_FPSCounts[num]++;
		this.m_numSamples++;
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x0006BEF0 File Offset: 0x0006A2F0
	private void Reset()
	{
		this.m_numSamples = 0;
		for (int i = 0; i < 501; i++)
		{
			this.m_FPSCounts[i] = 0;
		}
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x0006BF24 File Offset: 0x0006A324
	private float GetPercentile(float percentile)
	{
		percentile = Mathf.Clamp(percentile, 0f, 100f);
		int num = Mathf.CeilToInt((float)this.m_numSamples * (percentile / 100f));
		int num2 = 0;
		int i;
		for (i = 0; i < 501; i++)
		{
			num2 += this.m_FPSCounts[i];
			if (num <= num2)
			{
				break;
			}
		}
		return (float)i;
	}

	// Token: 0x04000F2E RID: 3886
	private const int MAX_FPS = 500;

	// Token: 0x04000F2F RID: 3887
	private int[] m_FPSCounts = new int[501];

	// Token: 0x04000F30 RID: 3888
	private int m_numSamples;

	// Token: 0x04000F31 RID: 3889
	private bool m_bStarted;

	// Token: 0x04000F32 RID: 3890
	private string m_displayString = string.Empty;
}
