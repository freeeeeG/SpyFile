using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

// Token: 0x0200009F RID: 159
public class LocalizationManager : MonoBehaviour
{
	// Token: 0x06000342 RID: 834 RVA: 0x0000C627 File Offset: 0x0000A827
	private void Awake()
	{
		if (LocalizationManager.Instance == null)
		{
			LocalizationManager.Instance = this;
			base.transform.SetParent(null);
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000343 RID: 835 RVA: 0x0000C660 File Offset: 0x0000A860
	private void Start()
	{
		string currentGameLanguage = SteamApps.GetCurrentGameLanguage();
		Debug.Log("Steam預設語言: " + currentGameLanguage);
		if (!PlayerPrefs.HasKey("GAME_LANGUAGE"))
		{
			if (!(currentGameLanguage == "english"))
			{
				if (currentGameLanguage == "tchinese")
				{
					this.SetLanguage(SystemLanguage.ChineseTraditional);
					PlayerPrefs.SetInt("GAME_LANGUAGE", 1);
					goto IL_8C;
				}
				if (currentGameLanguage == "schinese")
				{
					this.SetLanguage(SystemLanguage.ChineseSimplified);
					PlayerPrefs.SetInt("GAME_LANGUAGE", 2);
					goto IL_8C;
				}
			}
			this.SetLanguage(SystemLanguage.English);
			PlayerPrefs.SetInt("GAME_LANGUAGE", 0);
			IL_8C:
			Debug.Log("設定遊戲語言為Steam設定語言: " + currentGameLanguage);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x0000C70C File Offset: 0x0000A90C
	public string GetString(string tableName, string key, params object[] arg)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < arg.Length; i++)
		{
			dictionary[i.ToString()] = arg[i].ToString();
		}
		object[] arguments = new object[]
		{
			dictionary
		};
		return LocalizationSettings.StringDatabase.GetLocalizedString(tableName, key, null, FallbackBehavior.UseProjectSettings, arguments);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0000C768 File Offset: 0x0000A968
	public bool HasString(string tableName, string key)
	{
		StringTable table = LocalizationSettings.StringDatabase.GetTable(tableName, null);
		return !(table == null) && table.GetEntry(key) != null;
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0000C79C File Offset: 0x0000A99C
	public void SetLanguage(SystemLanguage language)
	{
		int index = 0;
		if (language != SystemLanguage.English)
		{
			if (language != SystemLanguage.ChineseSimplified)
			{
				if (language == SystemLanguage.ChineseTraditional)
				{
					index = 1;
				}
			}
			else
			{
				index = 2;
			}
		}
		else
		{
			index = 0;
		}
		this.currentLanguage = language;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
		EventMgr.SendEvent(eGameEvents.OnLanguageChanged);
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0000C7EE File Offset: 0x0000A9EE
	public SystemLanguage GetCurrentLanguage()
	{
		return this.currentLanguage;
	}

	// Token: 0x0400036B RID: 875
	public static LocalizationManager Instance;

	// Token: 0x0400036C RID: 876
	private LocalizationSettings localizationSettings;

	// Token: 0x0400036D RID: 877
	[SerializeField]
	private LocalizedStringTable _localizedStringTable;

	// Token: 0x0400036E RID: 878
	private StringTable _currentStringTable;

	// Token: 0x0400036F RID: 879
	private SystemLanguage currentLanguage;
}
