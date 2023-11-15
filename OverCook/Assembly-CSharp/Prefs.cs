using System;
using System.Collections.Generic;

// Token: 0x02000B99 RID: 2969
public class Prefs
{
	// Token: 0x06003CDB RID: 15579 RVA: 0x0012324C File Offset: 0x0012164C
	public static void Setup(string label, int v = 0)
	{
		SaveManager saveManager = GameUtils.RequestManager<SaveManager>();
		if (saveManager != null)
		{
			MetaGameProgress metaGameProgress = saveManager.GetMetaGameProgress();
			if (metaGameProgress != null)
			{
				int value = 0;
				if (metaGameProgress.SaveData.Get("P_" + label, out value, v))
				{
					Prefs.m_prefs[label] = value;
				}
			}
		}
	}

	// Token: 0x06003CDC RID: 15580 RVA: 0x001232AA File Offset: 0x001216AA
	public static bool HasKey(string label)
	{
		return Prefs.m_prefs.ContainsKey(label);
	}

	// Token: 0x06003CDD RID: 15581 RVA: 0x001232B7 File Offset: 0x001216B7
	public static int GetInt(string label, int v)
	{
		return Prefs.m_prefs.SafeGet(label, v);
	}

	// Token: 0x06003CDE RID: 15582 RVA: 0x001232C5 File Offset: 0x001216C5
	public static void SetInt(string label, int v)
	{
		Prefs.m_prefs.SafeAdd(label, v);
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x001232D4 File Offset: 0x001216D4
	public static void Save()
	{
		SaveManager saveManager = GameUtils.RequestManager<SaveManager>();
		if (saveManager != null)
		{
			MetaGameProgress metaGameProgress = saveManager.GetMetaGameProgress();
			if (metaGameProgress != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in Prefs.m_prefs)
				{
					metaGameProgress.SaveData.Set("P_" + keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
	}

	// Token: 0x04003101 RID: 12545
	private static Dictionary<string, int> m_prefs = new Dictionary<string, int>();
}
