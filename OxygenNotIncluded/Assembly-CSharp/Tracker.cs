﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052C RID: 1324
public abstract class Tracker
{
	// Token: 0x06001F96 RID: 8086 RVA: 0x000A8574 File Offset: 0x000A6774
	public global::Tuple<float, float>[] ChartableData(float periodLength)
	{
		float time = GameClock.Instance.GetTime();
		List<global::Tuple<float, float>> list = new List<global::Tuple<float, float>>();
		int num = this.dataPoints.Count - 1;
		while (num >= 0 && this.dataPoints[num].periodStart >= time - periodLength)
		{
			list.Add(new global::Tuple<float, float>(this.dataPoints[num].periodStart, this.dataPoints[num].periodValue));
			num--;
		}
		if (list.Count == 0)
		{
			if (this.dataPoints.Count > 0)
			{
				list.Add(new global::Tuple<float, float>(this.dataPoints[this.dataPoints.Count - 1].periodStart, this.dataPoints[this.dataPoints.Count - 1].periodValue));
			}
			else
			{
				list.Add(new global::Tuple<float, float>(0f, 0f));
			}
		}
		list.Reverse();
		return list.ToArray();
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x000A866C File Offset: 0x000A686C
	public float GetDataTimeLength()
	{
		float num = 0f;
		for (int i = this.dataPoints.Count - 1; i >= 0; i--)
		{
			num += this.dataPoints[i].periodEnd - this.dataPoints[i].periodStart;
		}
		return num;
	}

	// Token: 0x06001F98 RID: 8088
	public abstract void UpdateData();

	// Token: 0x06001F99 RID: 8089
	public abstract string FormatValueString(float value);

	// Token: 0x06001F9A RID: 8090 RVA: 0x000A86BE File Offset: 0x000A68BE
	public float GetCurrentValue()
	{
		if (this.dataPoints.Count == 0)
		{
			return 0f;
		}
		return this.dataPoints[this.dataPoints.Count - 1].periodValue;
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x000A86F0 File Offset: 0x000A68F0
	public float GetMinValue(float sampleHistoryLengthSeconds)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData(sampleHistoryLengthSeconds);
		if (array.Length == 0)
		{
			return 0f;
		}
		if (array.Length == 1)
		{
			return array[0].second;
		}
		float num = array[array.Length - 1].second;
		int num2 = array.Length - 1;
		while (num2 >= 0 && time - array[num2].first <= sampleHistoryLengthSeconds)
		{
			num = Mathf.Min(num, array[num2].second);
			num2--;
		}
		return num;
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x000A8764 File Offset: 0x000A6964
	public float GetMaxValue(int sampleHistoryLengthSeconds)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData((float)sampleHistoryLengthSeconds);
		if (array.Length == 0)
		{
			return 0f;
		}
		if (array.Length == 1)
		{
			return array[0].second;
		}
		float num = array[array.Length - 1].second;
		int num2 = array.Length - 1;
		while (num2 >= 0 && time - array[num2].first <= (float)sampleHistoryLengthSeconds)
		{
			num = Mathf.Max(num, array[num2].second);
			num2--;
		}
		return num;
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000A87DC File Offset: 0x000A69DC
	public float GetAverageValue(float sampleHistoryLengthSeconds)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData(sampleHistoryLengthSeconds);
		float num = 0f;
		float num2 = 0f;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (array[i].first >= time - sampleHistoryLengthSeconds)
			{
				float num3 = (i == array.Length - 1) ? (time - array[i].first) : (array[i + 1].first - array[i].first);
				num2 += num3;
				if (!float.IsNaN(array[i].second))
				{
					num += num3 * array[i].second;
				}
			}
		}
		float result;
		if (num2 == 0f)
		{
			if (array.Length == 0)
			{
				result = 0f;
			}
			else
			{
				result = array[array.Length - 1].second;
			}
		}
		else
		{
			result = num / num2;
		}
		return result;
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x000A88B0 File Offset: 0x000A6AB0
	public float GetDelta(float secondsAgo)
	{
		float time = GameClock.Instance.GetTime();
		global::Tuple<float, float>[] array = this.ChartableData(secondsAgo);
		if (array.Length < 2)
		{
			return 0f;
		}
		float num = -1f;
		float second = array[array.Length - 1].second;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (time - array[i].first >= secondsAgo)
			{
				num = array[i].second;
			}
		}
		return second - num;
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x000A8920 File Offset: 0x000A6B20
	protected void AddPoint(float value)
	{
		if (float.IsNaN(value))
		{
			value = 0f;
		}
		this.dataPoints.Add(new DataPoint((this.dataPoints.Count == 0) ? GameClock.Instance.GetTime() : this.dataPoints[this.dataPoints.Count - 1].periodEnd, GameClock.Instance.GetTime(), value));
		int count = Math.Max(0, this.dataPoints.Count - this.maxPoints);
		this.dataPoints.RemoveRange(0, count);
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x000A89B4 File Offset: 0x000A6BB4
	public List<DataPoint> GetCompressedData()
	{
		int num = 10;
		List<DataPoint> list = new List<DataPoint>();
		float num2 = (this.dataPoints[this.dataPoints.Count - 1].periodEnd - this.dataPoints[0].periodStart) / (float)num;
		for (int i = 0; i < num; i++)
		{
			float num3 = num2 * (float)i;
			float num4 = num3 + num2;
			float num5 = 0f;
			for (int j = 0; j < this.dataPoints.Count; j++)
			{
				DataPoint dataPoint = this.dataPoints[j];
				num5 += dataPoint.periodValue * Mathf.Max(0f, Mathf.Min(num4, dataPoint.periodEnd) - Mathf.Max(dataPoint.periodStart, num3));
			}
			list.Add(new DataPoint(num3, num4, num5 / (num4 - num3)));
		}
		return list;
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x000A8A97 File Offset: 0x000A6C97
	public void OverwriteData(List<DataPoint> newData)
	{
		this.dataPoints = newData;
	}

	// Token: 0x040011A5 RID: 4517
	private const int standardSampleRate = 4;

	// Token: 0x040011A6 RID: 4518
	private const int defaultCyclesTracked = 5;

	// Token: 0x040011A7 RID: 4519
	protected List<DataPoint> dataPoints = new List<DataPoint>();

	// Token: 0x040011A8 RID: 4520
	private int maxPoints = Mathf.CeilToInt(750f);
}
