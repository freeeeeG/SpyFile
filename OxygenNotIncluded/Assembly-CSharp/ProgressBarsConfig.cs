using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BCA RID: 3018
public class ProgressBarsConfig : ScriptableObject
{
	// Token: 0x06005ECD RID: 24269 RVA: 0x0022CE4A File Offset: 0x0022B04A
	public static void DestroyInstance()
	{
		ProgressBarsConfig.instance = null;
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06005ECE RID: 24270 RVA: 0x0022CE52 File Offset: 0x0022B052
	public static ProgressBarsConfig Instance
	{
		get
		{
			if (ProgressBarsConfig.instance == null)
			{
				ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>("ProgressBarsConfig");
				ProgressBarsConfig.instance.Initialize();
			}
			return ProgressBarsConfig.instance;
		}
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x0022CE80 File Offset: 0x0022B080
	public void Initialize()
	{
		foreach (ProgressBarsConfig.BarData barData in this.barColorDataList)
		{
			this.barColorMap.Add(barData.barName, barData);
		}
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x0022CEE0 File Offset: 0x0022B0E0
	public string GetBarDescription(string barName)
	{
		string result = "";
		if (this.IsBarNameValid(barName))
		{
			result = Strings.Get(this.barColorMap[barName].barDescriptionKey);
		}
		return result;
	}

	// Token: 0x06005ED1 RID: 24273 RVA: 0x0022CF1C File Offset: 0x0022B11C
	public Color GetBarColor(string barName)
	{
		Color result = Color.clear;
		if (this.IsBarNameValid(barName))
		{
			result = this.barColorMap[barName].barColor;
		}
		return result;
	}

	// Token: 0x06005ED2 RID: 24274 RVA: 0x0022CF4B File Offset: 0x0022B14B
	public bool IsBarNameValid(string barName)
	{
		if (string.IsNullOrEmpty(barName))
		{
			global::Debug.LogError("The barName provided was null or empty. Don't do that.");
			return false;
		}
		if (!this.barColorMap.ContainsKey(barName))
		{
			global::Debug.LogError(string.Format("No BarData found for the entry [ {0} ]", barName));
			return false;
		}
		return true;
	}

	// Token: 0x04004000 RID: 16384
	public GameObject progressBarPrefab;

	// Token: 0x04004001 RID: 16385
	public GameObject progressBarUIPrefab;

	// Token: 0x04004002 RID: 16386
	public GameObject healthBarPrefab;

	// Token: 0x04004003 RID: 16387
	public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();

	// Token: 0x04004004 RID: 16388
	public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();

	// Token: 0x04004005 RID: 16389
	private static ProgressBarsConfig instance;

	// Token: 0x02001B0C RID: 6924
	[Serializable]
	public struct BarData
	{
		// Token: 0x04007B8C RID: 31628
		public string barName;

		// Token: 0x04007B8D RID: 31629
		public Color barColor;

		// Token: 0x04007B8E RID: 31630
		public string barDescriptionKey;
	}
}
