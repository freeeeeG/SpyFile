using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[Serializable]
public class Settings_IntSet
{
	// Token: 0x06000038 RID: 56 RVA: 0x0000399C File Offset: 0x00001B9C
	public string GetString_WithIndex(int index)
	{
		if (this.languageSets == null)
		{
			Debug.LogError("Error_LangsSetsNull");
			return "Error_LangsSetsNull";
		}
		if (index >= this.languageSets.Length)
		{
			Debug.LogError("Error_LangsSetsLengthInvalid");
			return "Error_LangsSetsLengthInvalid";
		}
		return this.languageSets[index];
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000039E9 File Offset: 0x00001BE9
	public int GetIndexCount()
	{
		if (this.languageSets == null)
		{
			Debug.LogError("Error_LangsSetsNull");
		}
		return this.languageSets.Length;
	}

	// Token: 0x0400002D RID: 45
	[SerializeField]
	private LanguageSet[] languageSets;
}
