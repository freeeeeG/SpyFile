using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class Localization : MonoBehaviour
{
	// Token: 0x06001064 RID: 4196 RVA: 0x0005E240 File Offset: 0x0005C640
	private void Awake()
	{
		TextAsset[] array = new TextAsset[0];
		LocalizationData[] array2 = this.m_LocalizationData.AllData;
		array2 = array2.AllRemoved_Predicate((LocalizationData x) => x == null);
		for (int i = 0; i < array2.Length; i++)
		{
			array = array.Union(array2[i].m_Localizations);
		}
		Localization.LoadDictionarys(false, array);
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x0005E2B0 File Offset: 0x0005C6B0
	public static bool LoadDictionarys(bool reload, TextAsset[] localisationFiles = null)
	{
		if (reload)
		{
			Localization.m_bIsLoaded = false;
			Localization.m_bIsLoading = false;
			Localization.m_Languages = null;
			Localization.m_stringTable.Clear();
		}
		if (Localization.m_bIsLoaded || Localization.m_bIsLoading)
		{
			return true;
		}
		Localization.m_bIsLoading = true;
		for (int i = 0; i < Localization.c_localisationDataPaths.Length; i++)
		{
			string format = Localization.c_localisationDataPaths[i];
			Localization.AddLanguageFile(string.Format(format, "Shared"), localisationFiles);
			Localization.AddLanguageFile(string.Format(format, "PC"), localisationFiles);
		}
		Localization.m_bIsLoading = false;
		Localization.m_bIsLoaded = true;
		return true;
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x0005E34C File Offset: 0x0005C74C
	private static bool AddLanguageFile(string path, TextAsset[] localisationFiles = null)
	{
		if (Localization.m_LanguageFiles.Contains(path))
		{
			return false;
		}
		Localization.m_LanguageFiles.Add(path);
		TextAsset text = null;
		if (localisationFiles != null)
		{
			int num = localisationFiles.FindIndex_Predicate((TextAsset x) => path.EndsWith(x.name));
			if (num != -1)
			{
				text = localisationFiles[num];
			}
		}
		else
		{
			text = (Resources.Load(path, typeof(TextAsset)) as TextAsset);
		}
		Localization.LoadTSV(text);
		return true;
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0005E3DC File Offset: 0x0005C7DC
	private static bool LoadTSV(TextAsset text)
	{
		TVSReader tvsreader = new TVSReader(text.bytes);
		string[] array = tvsreader.ReadRow();
		if (array.Length < 2)
		{
			return false;
		}
		Localization.m_Languages = new string[array.Length - 1];
		for (int i = 0; i < Localization.m_Languages.Length; i++)
		{
			Localization.m_Languages[i] = array[i + 1];
		}
		string[] array2 = null;
		do
		{
			array2 = tvsreader.ReadRow();
			if (array2 != null && array2.Length > 0 && array2[0] != string.Empty)
			{
				if (array2[0].ToLowerInvariant() != "end")
				{
					for (int j = 1; j < array2.Length; j++)
					{
						if (array2[j] != null && array2[j].Length > 2)
						{
							array2[j] = array2[j].Replace("\"", "\b");
							array2[j] = array2[j].Replace("\b\b", "\"");
							array2[j] = array2[j].Replace("\b", string.Empty);
						}
					}
					if (!Localization.m_stringTable.ContainsKey(array2[0]))
					{
						Localization.CheckForMarkup(ref array2);
						Localization.m_stringTable.Add(array2[0], array2);
					}
				}
				else
				{
					array2 = null;
				}
			}
		}
		while (array2 != null);
		return true;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0005E52C File Offset: 0x0005C92C
	public static bool Get(string tag, out string localised, params LocToken[] findReplace)
	{
		if (tag.StartsWith("\"") && tag.EndsWith("\"") && tag.Length >= 2)
		{
			localised = tag.Substring(1, tag.Length - 2);
			return true;
		}
		if (Localization.m_stringTable.ContainsKey(tag))
		{
			SupportedLanguages language = Localization.GetLanguage();
			int num = (int)(language + 1);
			string[] array = Localization.m_stringTable[tag] as string[];
			string text = array[num];
			if (text.Length > 0)
			{
				if (findReplace.Length > 0)
				{
					StringBuilder stringBuilder = new StringBuilder(text);
					for (int i = 0; i < findReplace.Length; i++)
					{
						int num2;
						do
						{
							num2 = stringBuilder.ToString().IndexOf(findReplace[i].m_token, StringComparison.OrdinalIgnoreCase);
							if (num2 > -1)
							{
								stringBuilder.Remove(num2, findReplace[i].m_token.Length);
								stringBuilder.Insert(num2, findReplace[i].m_value);
							}
						}
						while (num2 != -1);
					}
					text = stringBuilder.ToString();
				}
				localised = text;
				return true;
			}
		}
		localised = "MT[" + tag + "]";
		return false;
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0005E660 File Offset: 0x0005CA60
	public static string Get(string tag, params LocToken[] findReplace)
	{
		string result = null;
		Localization.Get(tag, out result, findReplace);
		return result;
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x0005E67C File Offset: 0x0005CA7C
	public static SupportedLanguages GetLanguage()
	{
		int num = -1;
		if (Application.isPlaying && SteamPlayerManager.Initialized)
		{
			num = SteamApps.GetCurrentGameLanguage().GetHashCode();
		}
		if (num == Localization.m_FrenchHashCode)
		{
			return SupportedLanguages.French;
		}
		if (num == Localization.m_ItalianHashCode)
		{
			return SupportedLanguages.Italian;
		}
		if (num == Localization.m_GermanHashCode)
		{
			return SupportedLanguages.German;
		}
		if (num == Localization.m_SpanishHashCode)
		{
			return SupportedLanguages.Spanish;
		}
		if (num == Localization.m_RussianHashCode)
		{
			return SupportedLanguages.Russian;
		}
		if (num == Localization.m_SChineseHashCode)
		{
			return SupportedLanguages.Chinese;
		}
		if (num == Localization.m_TChineseHashCode)
		{
			return SupportedLanguages.ChineseTraditional;
		}
		if (num == Localization.m_JapaneseHashCode)
		{
			return SupportedLanguages.Japanese;
		}
		if (num == Localization.m_KoreanHashCode)
		{
			return SupportedLanguages.Korean;
		}
		if (num == Localization.m_PolishHashCode)
		{
			return SupportedLanguages.Polish;
		}
		if (num == Localization.m_BrazilianHashCode)
		{
			return SupportedLanguages.Brazilian;
		}
		if (num != -1)
		{
			return SupportedLanguages.English;
		}
		SystemLanguage systemLanguage = Application.systemLanguage;
		switch (systemLanguage)
		{
		case SystemLanguage.Italian:
			return SupportedLanguages.Italian;
		case SystemLanguage.Japanese:
			return SupportedLanguages.Japanese;
		case SystemLanguage.Korean:
			return SupportedLanguages.Korean;
		default:
			if (systemLanguage == SystemLanguage.French)
			{
				return SupportedLanguages.French;
			}
			if (systemLanguage == SystemLanguage.German)
			{
				return SupportedLanguages.German;
			}
			if (systemLanguage == SystemLanguage.ChineseSimplified)
			{
				return SupportedLanguages.Chinese;
			}
			if (systemLanguage != SystemLanguage.ChineseTraditional)
			{
				return SupportedLanguages.English;
			}
			return SupportedLanguages.ChineseTraditional;
		case SystemLanguage.Polish:
			return SupportedLanguages.Polish;
		case SystemLanguage.Portuguese:
			return SupportedLanguages.Brazilian;
		case SystemLanguage.Russian:
			return SupportedLanguages.Russian;
		case SystemLanguage.Spanish:
			return SupportedLanguages.Spanish;
		}
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0005E7CB File Offset: 0x0005CBCB
	public static bool AreLanguagesLoaded()
	{
		return Localization.m_bIsLoaded;
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0005E7D4 File Offset: 0x0005CBD4
	private static void CheckForMarkup(ref string[] trans)
	{
		for (int i = 1; i < trans.Length; i++)
		{
			string text = string.Empty;
			int j = 0;
			bool flag = false;
			while (j < trans[i].Length)
			{
				int num = trans[i].IndexOf('[', j);
				if (num <= -1)
				{
					if (flag)
					{
						text += trans[i].Substring(j, trans[i].Length - j);
					}
					break;
				}
				text += trans[i].Substring(j, num - j);
				j = num + 1;
				int num2 = trans[i].IndexOf(']', j);
				if (num2 > -1)
				{
					string text2 = trans[i].Substring(j, num2 - j);
					if (text2 == "A")
					{
						flag = true;
						text2 = "<color=red>\u0080</color>";
						text += text2;
					}
					else if (text2 == "B")
					{
						flag = true;
						text2 = "<color=red>\u0081</color>";
						text += text2;
					}
					else if (text2 == "X")
					{
						flag = true;
						text2 = "<color=red>\u0082</color>";
						text += text2;
					}
					else if (text2 == "Y")
					{
						flag = true;
						text2 = "<color=red>\u0082</color>";
						text += text2;
					}
					j = num2 + 1;
				}
			}
			if (flag)
			{
				trans[i] = text;
			}
		}
	}

	// Token: 0x04000C69 RID: 3177
	private const string PRECODE = "<color=red>";

	// Token: 0x04000C6A RID: 3178
	private const string POSTCODE = "</color>";

	// Token: 0x04000C6B RID: 3179
	private const string A_BUTTON = "\u0080";

	// Token: 0x04000C6C RID: 3180
	private const string B_BUTTON = "\u0081";

	// Token: 0x04000C6D RID: 3181
	private const string X_BUTTON = "\u0082";

	// Token: 0x04000C6E RID: 3182
	private const string Y_BUTTON = "\u0082";

	// Token: 0x04000C6F RID: 3183
	private static int m_SChineseHashCode = "schinese".GetHashCode();

	// Token: 0x04000C70 RID: 3184
	private static int m_TChineseHashCode = "tchinese".GetHashCode();

	// Token: 0x04000C71 RID: 3185
	private static int m_KoreanHashCode = "koreana".GetHashCode();

	// Token: 0x04000C72 RID: 3186
	private static int m_BrazilianHashCode = "brazilian".GetHashCode();

	// Token: 0x04000C73 RID: 3187
	private static int m_EnglishHashCode = "english".GetHashCode();

	// Token: 0x04000C74 RID: 3188
	private static int m_FrenchHashCode = "french".GetHashCode();

	// Token: 0x04000C75 RID: 3189
	private static int m_ItalianHashCode = "italian".GetHashCode();

	// Token: 0x04000C76 RID: 3190
	private static int m_GermanHashCode = "german".GetHashCode();

	// Token: 0x04000C77 RID: 3191
	private static int m_SpanishHashCode = "spanish".GetHashCode();

	// Token: 0x04000C78 RID: 3192
	private static int m_JapaneseHashCode = "japanese".GetHashCode();

	// Token: 0x04000C79 RID: 3193
	private static int m_PolishHashCode = "polish".GetHashCode();

	// Token: 0x04000C7A RID: 3194
	private static int m_RussianHashCode = "russian".GetHashCode();

	// Token: 0x04000C7B RID: 3195
	private static string[] m_Languages = null;

	// Token: 0x04000C7C RID: 3196
	private static List<string> m_LanguageFiles = new List<string>();

	// Token: 0x04000C7D RID: 3197
	private static Hashtable m_stringTable = new Hashtable();

	// Token: 0x04000C7E RID: 3198
	private static bool m_bIsLoaded = false;

	// Token: 0x04000C7F RID: 3199
	private static bool m_bIsLoading = false;

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	private Localization.DLCSerializedLocalizationData m_LocalizationData = new Localization.DLCSerializedLocalizationData();

	// Token: 0x04000C81 RID: 3201
	private static readonly string[] c_localisationDataPaths = new string[]
	{
		"Assets/Data/Localization/Localization - {0}",
		"Assets/DownloadableContent/DLC02/DLC_Assets/Data/Localization/Localization_DLC02 - {0}",
		"Assets/DownloadableContent/DLC03/DLC_Assets/Data/Localization/Localization_DLC03 - {0}",
		"Assets/DownloadableContent/DLC04/DLC_Assets/Data/Localization/Localization_DLC04 - {0}",
		"Assets/DownloadableContent/DLC05/DLC_Assets/Data/Localization/Localization_DLC05 - {0}",
		"Assets/DownloadableContent/DLC06/DLC_Assets/Data/Localization/Localization_DLC06 - {0}",
		"Assets/DownloadableContent/DLC07/DLC_Assets/Data/Localization/Localization_DLC07 - {0}",
		"Assets/DownloadableContent/DLC08/DLC_Assets/Data/Localization/Localization_DLC08 - {0}",
		"Assets/DownloadableContent/DLC09/DLC_Assets/Data/Localization/Localization_DLC09 - {0}",
		"Assets/DownloadableContent/DLC10/DLC_Assets/Data/Localization/Localization_DLC10 - {0}",
		"Assets/DownloadableContent/DLC11/DLC_Assets/Data/Localization/Localization_DLC11 - {0}",
		"Assets/DownloadableContent/DLC13/Assets/Data/Localization/Localization_DLC13 - {0}"
	};

	// Token: 0x0200035B RID: 859
	[Serializable]
	private class DLCSerializedLocalizationData : DLCSerializedData<LocalizationData>
	{
	}
}
