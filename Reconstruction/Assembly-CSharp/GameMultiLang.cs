using System;
using System.Collections.Generic;
using Lanuguage;
using Steamworks;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class GameMultiLang : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600005A RID: 90 RVA: 0x00003A5C File Offset: 0x00001C5C
	// (set) Token: 0x0600005B RID: 91 RVA: 0x00003A63 File Offset: 0x00001C63
	public static List<LanguageData> LanguageData { get; set; }

	// Token: 0x0600005C RID: 92 RVA: 0x00003A6C File Offset: 0x00001C6C
	private void Awake()
	{
		if (GameMultiLang.Instance == null)
		{
			GameMultiLang.Instance = this;
			if (PlayerPrefs.GetString("_language", "") == "" && SteamManager.Initialized)
			{
				string steamUILanguage = SteamUtils.GetSteamUILanguage();
				if (!(steamUILanguage == "schinese") && !(steamUILanguage == "tchinese"))
				{
					if (!(steamUILanguage == "english"))
					{
					}
					this.SetLanguage("en");
				}
				else
				{
					this.SetLanguage("ch");
				}
			}
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
		this.LoadLanguage();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00003B08 File Offset: 0x00001D08
	private void SetLanguage(string lang)
	{
		PlayerPrefs.SetString("_language", lang);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003B18 File Offset: 0x00001D18
	public void LoadLanguage()
	{
		if (GameMultiLang.Fields == null)
		{
			GameMultiLang.Fields = new Dictionary<string, string>();
		}
		if (GameMultiLang.SpriteFields == null)
		{
			GameMultiLang.SpriteFields = new Dictionary<string, Sprite>();
		}
		GameMultiLang.Fields.Clear();
		GameMultiLang.SpriteFields.Clear();
		string @string = PlayerPrefs.GetString("_language", this.defaultLang);
		PlayerPrefs.SetString("_language", @string);
		if (!(@string == "en"))
		{
			if (@string == "ch")
			{
				PlayerPrefs.SetInt("_language_index", 1);
			}
		}
		else
		{
			PlayerPrefs.SetInt("_language_index", 0);
		}
		this.LoadLanguageData();
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00003BB1 File Offset: 0x00001DB1
	internal static string GetTraduction(object p)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003BB8 File Offset: 0x00001DB8
	private void LoadLanguageData()
	{
		GameMultiLang.LanguageData = this.languageData.dataArray;
		int @int = PlayerPrefs.GetInt("_language_index", 0);
		if (@int == 0)
		{
			for (int i = 0; i < GameMultiLang.LanguageData.Count; i++)
			{
				if (GameMultiLang.Fields.ContainsKey(GameMultiLang.LanguageData[i].Key))
				{
					Debug.Log(GameMultiLang.LanguageData[i].Key);
				}
				else
				{
					GameMultiLang.Fields.Add(GameMultiLang.LanguageData[i].Key, GameMultiLang.LanguageData[i].English);
				}
			}
			for (int j = 0; j < this.SpriteDatas.Count; j++)
			{
				GameMultiLang.SpriteFields.Add(this.SpriteDatas[j].Key, this.SpriteDatas[j].English);
			}
			return;
		}
		if (@int != 1)
		{
			return;
		}
		for (int k = 0; k < GameMultiLang.LanguageData.Count; k++)
		{
			if (GameMultiLang.Fields.ContainsKey(GameMultiLang.LanguageData[k].Key))
			{
				Debug.Log(GameMultiLang.LanguageData[k].Key);
			}
			else
			{
				GameMultiLang.Fields.Add(GameMultiLang.LanguageData[k].Key, GameMultiLang.LanguageData[k].Chinese);
			}
		}
		for (int l = 0; l < this.SpriteDatas.Count; l++)
		{
			GameMultiLang.SpriteFields.Add(this.SpriteDatas[l].Key, this.SpriteDatas[l].Chinese);
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00003D5B File Offset: 0x00001F5B
	public static string GetTraduction(string key)
	{
		if (!GameMultiLang.Fields.ContainsKey(key))
		{
			Debug.LogError("There is no key with name: [" + key + "] in your text files");
			return null;
		}
		return GameMultiLang.Fields[key];
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003D8C File Offset: 0x00001F8C
	public static Sprite GetSprite(string key)
	{
		if (!GameMultiLang.SpriteFields.ContainsKey(key))
		{
			Debug.LogError("There is no key with name: [" + key + "] in your sprite files");
			return null;
		}
		return GameMultiLang.SpriteFields[key];
	}

	// Token: 0x0400002F RID: 47
	public static GameMultiLang Instance;

	// Token: 0x04000030 RID: 48
	public static Dictionary<string, string> Fields;

	// Token: 0x04000031 RID: 49
	public static Dictionary<string, Sprite> SpriteFields;

	// Token: 0x04000032 RID: 50
	[SerializeField]
	private string defaultLang;

	// Token: 0x04000033 RID: 51
	[SerializeField]
	private LanguageManager languageData;

	// Token: 0x04000034 RID: 52
	[SerializeField]
	private List<SpriteData> SpriteDatas;
}
