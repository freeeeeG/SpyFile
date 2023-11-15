using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
[Serializable]
public class LanguageSet
{
	// Token: 0x0600003B RID: 59 RVA: 0x00003A10 File Offset: 0x00001C10
	public string GetString()
	{
		int language = (int)Setting.Inst.language;
		if (this.langs == null)
		{
			Debug.LogError("Error_LangsNull");
			return "Error_LangsNull";
		}
		if (language >= this.langs.Length)
		{
			Debug.LogError("Error_LangsLengthInvalid");
			return "Error_LangsLengthInvalid";
		}
		string text = this.langs[language];
		if (text == null || text == "")
		{
			return this.langs[1];
		}
		return text;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003A7E File Offset: 0x00001C7E
	public void SetStrings(string[] strings)
	{
		if (strings == null || strings.Length == 0)
		{
			Debug.LogError("Error_StringsEmpty!");
		}
		this.langs = strings;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003A98 File Offset: 0x00001C98
	public void SetStrings(string eng, string cn)
	{
		string[] array = new string[]
		{
			eng,
			cn
		};
		this.langs = array;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003ABB File Offset: 0x00001CBB
	public static implicit operator string(LanguageSet set)
	{
		return set.GetString();
	}

	// Token: 0x0400002E RID: 46
	[SerializeField]
	private string[] langs = new string[2];
}
