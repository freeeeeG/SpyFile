using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E7 RID: 231
[CreateAssetMenu(fileName = "DebugSettingData", menuName = "DebugSettingData", order = 1)]
[Serializable]
public class DebugSettingSO : ScriptableObject
{
	// Token: 0x060005C4 RID: 1476 RVA: 0x00016C2C File Offset: 0x00014E2C
	public DebugSettingSO()
	{
		if (this.list_DebugSettingData == null)
		{
			this.list_DebugSettingData = new List<DebugSettingData>();
		}
		if (this.list_DebugSettingData.Count == 0)
		{
			this.list_DebugSettingData.Add(new DebugSettingData());
		}
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00016C64 File Offset: 0x00014E64
	private void OnValidate()
	{
		bool flag = false;
		foreach (object obj in Enum.GetValues(typeof(eDebugKey)))
		{
			int num = (int)obj;
			bool flag2 = false;
			for (int i = 0; i < this.list_DebugSettingData.Count; i++)
			{
				if (this.list_DebugSettingData[i].Key == (eDebugKey)num)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				DebugSettingData debugSettingData = new DebugSettingData();
				debugSettingData.Key = (eDebugKey)num;
				this.list_DebugSettingData.Add(debugSettingData);
				flag = true;
			}
		}
		for (int j = this.list_DebugSettingData.Count - 1; j >= 0; j--)
		{
			if (!Enum.IsDefined(typeof(eDebugKey), this.list_DebugSettingData[j].Key))
			{
				this.list_DebugSettingData.RemoveAt(j);
				flag = true;
			}
		}
		if (flag)
		{
			this.list_DebugSettingData.Sort(new Comparison<DebugSettingData>(DebugSettingSO.SortByKey));
		}
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00016D88 File Offset: 0x00014F88
	private static int SortByKey(DebugSettingData o1, DebugSettingData o2)
	{
		return o1.Key.CompareTo(o2.Key);
	}

	// Token: 0x0400052C RID: 1324
	[HideInInspector]
	[SerializeField]
	public List<DebugSettingData> list_DebugSettingData;
}
