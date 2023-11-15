using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using Klei;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000845 RID: 2117
public static class Localization
{
	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x00156C53 File Offset: 0x00154E53
	public static TMP_FontAsset FontAsset
	{
		get
		{
			return Localization.sFontAsset;
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x00156C5A File Offset: 0x00154E5A
	public static bool IsRightToLeft
	{
		get
		{
			return Localization.sLocale != null && Localization.sLocale.IsRightToLeft;
		}
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x00156C70 File Offset: 0x00154E70
	private static IEnumerable<Type> CollectLocStringTreeRoots(string locstrings_namespace, Assembly assembly)
	{
		return from type in assembly.GetTypes()
		where type.IsClass && type.Namespace == locstrings_namespace && !type.IsNested
		select type;
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x00156CA4 File Offset: 0x00154EA4
	private static Dictionary<string, object> MakeRuntimeLocStringTree(Type locstring_tree_root)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (FieldInfo fieldInfo in locstring_tree_root.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
				}
				else
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					if (locString == null)
					{
						global::Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
					}
					else
					{
						dictionary[fieldInfo.Name] = locString.text;
					}
				}
			}
		}
		foreach (Type type in locstring_tree_root.GetNestedTypes())
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		return dictionary;
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x00156D8C File Offset: 0x00154F8C
	private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtime_locstring_tree)
	{
		List<string> list = new List<string>(runtime_locstring_tree.Keys);
		list.Sort();
		foreach (string text in list)
		{
			string text2 = path + "." + text;
			object obj = runtime_locstring_tree[text];
			if (obj.GetType() != typeof(string))
			{
				Localization.WriteStringsTemplate(text2, writer, obj as Dictionary<string, object>);
			}
			else
			{
				string text3 = obj as string;
				text3 = text3.Replace("\\", "\\\\");
				text3 = text3.Replace("\"", "\\\"");
				text3 = text3.Replace("\n", "\\n");
				text3 = text3.Replace("’", "'");
				text3 = text3.Replace("“", "\\\"");
				text3 = text3.Replace("”", "\\\"");
				text3 = text3.Replace("…", "...");
				writer.WriteLine("#. " + text2);
				writer.WriteLine("msgctxt \"{0}\"", text2);
				writer.WriteLine("msgid \"" + text3 + "\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("");
			}
		}
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00156F0C File Offset: 0x0015510C
	public static void GenerateStringsTemplate(string locstrings_namespace, Assembly assembly, string output_filename, Dictionary<string, object> current_runtime_locstring_forest)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstrings_namespace, assembly))
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		if (current_runtime_locstring_forest != null)
		{
			dictionary.Concat(current_runtime_locstring_forest);
		}
		using (StreamWriter streamWriter = new StreamWriter(output_filename, false, new UTF8Encoding(false)))
		{
			streamWriter.WriteLine("msgid \"\"");
			streamWriter.WriteLine("msgstr \"\"");
			streamWriter.WriteLine("\"Application: Oxygen Not Included\"");
			streamWriter.WriteLine("\"POT Version: 2.0\"");
			streamWriter.WriteLine("");
			Localization.WriteStringsTemplate(locstrings_namespace, streamWriter, dictionary);
		}
		DebugUtil.LogArgs(new object[]
		{
			"Generated " + output_filename
		});
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00157008 File Offset: 0x00155208
	public static void GenerateStringsTemplate(Type locstring_tree_root, string output_folder)
	{
		output_folder = FileSystem.Normalize(output_folder);
		if (!FileUtil.CreateDirectory(output_folder, 5))
		{
			return;
		}
		Localization.GenerateStringsTemplate(locstring_tree_root.Namespace, Assembly.GetAssembly(locstring_tree_root), FileSystem.Normalize(Path.Combine(output_folder, string.Format("{0}_template.pot", locstring_tree_root.Namespace.ToLower()))), null);
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x0015705C File Offset: 0x0015525C
	public static void Initialize()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Localization.Initialize!"
		});
		bool flag = false;
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			Localization.sFontAsset = Localization.GetFont(Localization.GetDefaultLocale().FontName);
			break;
		case Localization.SelectedLanguageType.Preinstalled:
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (!string.IsNullOrEmpty(currentLanguageCode))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Preinstalled localization"
				});
				DebugUtil.LogArgs(new object[]
				{
					" -> ",
					currentLanguageCode
				});
				Localization.LoadPreinstalledTranslation(currentLanguageCode);
			}
			else
			{
				flag = true;
			}
			break;
		}
		case Localization.SelectedLanguageType.UGC:
			if (LanguageOptionsScreen.HasInstalledLanguage())
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Mod-based localization"
				});
				string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
				if (LanguageOptionsScreen.SetCurrentLanguage(savedLanguageMod))
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Loaded language from mod: " + savedLanguageMod
					});
				}
				else
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Failed to load language from mod: " + savedLanguageMod
					});
				}
			}
			else
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00157160 File Offset: 0x00155360
	public static void VerifyTranslationModSubscription(GameObject context)
	{
		if (Localization.GetSelectedLanguageType() != Localization.SelectedLanguageType.UGC)
		{
			return;
		}
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (LanguageOptionsScreen.HasInstalledLanguage())
		{
			return;
		}
		PublishedFileId_t publishedFileId_t = new PublishedFileId_t((ulong)KPlayerPrefs.GetInt("InstalledLanguage", (int)PublishedFileId_t.Invalid.m_PublishedFileId));
		Label rhs = new Label
		{
			distribution_platform = Label.DistributionPlatform.Steam,
			id = publishedFileId_t.ToString()
		};
		string arg = UI.FRONTEND.TRANSLATIONS_SCREEN.UNKNOWN;
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.label.Match(rhs))
			{
				arg = mod.title;
				break;
			}
		}
		Localization.ClearLanguage();
		KScreen component = KScreenManager.AddChild(context, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		ConfirmDialogScreen component2 = component.GetComponent<ConfirmDialogScreen>();
		string title_text = UI.CONFIRMDIALOG.DIALOG_HEADER;
		string text = string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.MISSING_LANGUAGE_PACK, arg);
		string confirm_text = UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART;
		component2.PopupConfirmDialog(text, new System.Action(App.instance.Restart), null, null, null, title_text, confirm_text, null, null);
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x001572A8 File Offset: 0x001554A8
	public static void LoadPreinstalledTranslation(string code)
	{
		if (!string.IsNullOrEmpty(code) && code != Localization.DEFAULT_LANGUAGE_CODE)
		{
			string preinstalledLocalizationFilePath = Localization.GetPreinstalledLocalizationFilePath(code);
			if (Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.Preinstalled, preinstalledLocalizationFilePath))
			{
				KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, code);
				return;
			}
		}
		else
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x001572EB File Offset: 0x001554EB
	public static bool LoadLocalTranslationFile(Localization.SelectedLanguageType source, string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		bool flag = Localization.LoadTranslationFromLines(File.ReadAllLines(path, Encoding.UTF8));
		if (flag)
		{
			KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, source.ToString());
			return flag;
		}
		Localization.ClearLanguage();
		return flag;
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x00157328 File Offset: 0x00155528
	private static bool LoadTranslationFromLines(string[] lines)
	{
		if (lines == null || lines.Length == 0)
		{
			return false;
		}
		Localization.sLocale = Localization.GetLocale(lines);
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
		bool flag = Localization.LoadTranslation(lines, false);
		if (flag)
		{
			Localization.currentFontName = Localization.GetFontName(lines);
			Localization.SwapToLocalizedFont(Localization.currentFontName);
		}
		return flag;
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x0015738C File Offset: 0x0015558C
	public static bool LoadTranslation(string[] lines, bool isTemplate = false)
	{
		bool result;
		try
		{
			Localization.OverloadStrings(Localization.ExtractTranslatedStrings(lines, isTemplate));
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x001573D0 File Offset: 0x001555D0
	public static Dictionary<string, string> LoadStringsFile(string path, bool isTemplate)
	{
		return Localization.ExtractTranslatedStrings(File.ReadAllLines(path, Encoding.UTF8), isTemplate);
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x001573E4 File Offset: 0x001555E4
	public static Dictionary<string, string> ExtractTranslatedStrings(string[] lines, bool isTemplate = false)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Localization.Entry entry = default(Localization.Entry);
		string key = isTemplate ? "msgid" : "msgstr";
		for (int i = 0; i < lines.Length; i++)
		{
			string text = lines[i];
			if (text == null || text.Length == 0)
			{
				entry = default(Localization.Entry);
			}
			else
			{
				string parameter = Localization.GetParameter("msgctxt", i, lines);
				if (parameter != null)
				{
					entry.msgctxt = parameter;
				}
				parameter = Localization.GetParameter(key, i, lines);
				if (parameter != null)
				{
					entry.msgstr = parameter;
				}
			}
			if (entry.IsPopulated)
			{
				dictionary[entry.msgctxt] = entry.msgstr;
				entry = default(Localization.Entry);
			}
		}
		return dictionary;
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x00157490 File Offset: 0x00155690
	private static string FixupString(string result)
	{
		result = result.Replace("\\n", "\n");
		result = result.Replace("\\\"", "\"");
		result = result.Replace("<style=“", "<style=\"");
		result = result.Replace("”>", "\">");
		result = result.Replace("<color=^p", "<color=#");
		return result;
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x001574F8 File Offset: 0x001556F8
	private static string GetParameter(string key, int idx, string[] all_lines)
	{
		if (!all_lines[idx].StartsWith(key))
		{
			return null;
		}
		List<string> list = new List<string>();
		string text = all_lines[idx];
		text = text.Substring(key.Length + 1, text.Length - key.Length - 1);
		list.Add(text);
		for (int i = idx + 1; i < all_lines.Length; i++)
		{
			string text2 = all_lines[i];
			if (!text2.StartsWith("\""))
			{
				break;
			}
			list.Add(text2);
		}
		string text3 = "";
		foreach (string text4 in list)
		{
			if (text4.EndsWith("\r"))
			{
				text4 = text4.Substring(0, text4.Length - 1);
			}
			text4 = text4.Substring(1, text4.Length - 2);
			text4 = Localization.FixupString(text4);
			text3 += text4;
		}
		return text3;
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x001575F8 File Offset: 0x001557F8
	private static void AddAssembly(string locstrings_namespace, Assembly assembly)
	{
		List<Assembly> list;
		if (!Localization.translatable_assemblies.TryGetValue(locstrings_namespace, out list))
		{
			list = new List<Assembly>();
			Localization.translatable_assemblies.Add(locstrings_namespace, list);
		}
		list.Add(assembly);
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x0015762D File Offset: 0x0015582D
	public static void AddAssembly(Assembly assembly)
	{
		Localization.AddAssembly("STRINGS", assembly);
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x0015763C File Offset: 0x0015583C
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Assembly assembly = Assembly.GetAssembly(locstring_tree_root);
		Localization.AddAssembly(locstring_tree_root.Namespace, assembly);
		string parent_path = locstring_tree_root.Namespace + ".";
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstring_tree_root.Namespace, assembly))
		{
			LocString.CreateLocStringKeys(type, parent_path);
		}
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x001576B4 File Offset: 0x001558B4
	public static void OverloadStrings(Dictionary<string, string> translated_strings)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		foreach (KeyValuePair<string, List<Assembly>> keyValuePair in Localization.translatable_assemblies)
		{
			foreach (Assembly assembly in keyValuePair.Value)
			{
				foreach (Type type in Localization.CollectLocStringTreeRoots(keyValuePair.Key, assembly))
				{
					string path = keyValuePair.Key + "." + type.Name;
					Localization.OverloadStrings(translated_strings, path, type, ref text, ref text2, ref text3);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have missing or mismatched parameters:\n" + text
			});
		}
		if (!string.IsNullOrEmpty(text2))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have mismatched <link> tags:\n" + text2
			});
		}
		if (!string.IsNullOrEmpty(text3))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following do not have the same amount of <link> tags as the english string which can cause nested link errors:\n" + text3
			});
		}
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x00157828 File Offset: 0x00155A28
	public static void OverloadStrings(Dictionary<string, string> translated_strings, string path, Type locstring_hierarchy, ref string parameter_errors, ref string link_errors, ref string link_count_errors)
	{
		foreach (FieldInfo fieldInfo in locstring_hierarchy.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				string text = path + "." + fieldInfo.Name;
				string text2 = null;
				if (translated_strings.TryGetValue(text, out text2))
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					LocString value = new LocString(text2, text);
					if (!Localization.AreParametersPreserved(locString.text, text2))
					{
						parameter_errors = parameter_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasSameOrLessLinkCountAsEnglish(locString.text, text2))
					{
						link_count_errors = link_count_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasMatchingLinkTags(text2, 0))
					{
						link_errors = link_errors + "\t" + text + "\n";
					}
					else
					{
						fieldInfo.SetValue(null, value);
					}
				}
			}
		}
		foreach (Type type in locstring_hierarchy.GetNestedTypes())
		{
			string path2 = path + "." + type.Name;
			Localization.OverloadStrings(translated_strings, path2, type, ref parameter_errors, ref link_errors, ref link_count_errors);
		}
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x00157962 File Offset: 0x00155B62
	public static string GetDefaultLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings_template.pot");
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00157973 File Offset: 0x00155B73
	public static string GetModLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings.po");
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x00157984 File Offset: 0x00155B84
	public static string GetPreinstalledLocalizationFilePath(string code)
	{
		string path = "strings/strings_preinstalled_" + code + ".po";
		return Path.Combine(Application.streamingAssetsPath, path);
	}

	// Token: 0x06003DC1 RID: 15809 RVA: 0x001579AD File Offset: 0x00155BAD
	public static string GetPreinstalledLocalizationTitle(string code)
	{
		return Strings.Get("STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PREINSTALLED_LANGUAGES." + code.ToUpper());
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x001579CC File Offset: 0x00155BCC
	public static Texture2D GetPreinstalledLocalizationImage(string code)
	{
		string path = Path.Combine(Application.streamingAssetsPath, "strings/preinstalled_icon_" + code + ".png");
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return texture2D;
		}
		return null;
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x00157A14 File Offset: 0x00155C14
	public static void SetLocale(Localization.Locale locale)
	{
		Localization.sLocale = locale;
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x00157A3C File Offset: 0x00155C3C
	public static Localization.Locale GetLocale()
	{
		return Localization.sLocale;
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x00157A44 File Offset: 0x00155C44
	private static string GetFontParam(string line)
	{
		string text = null;
		if (line.StartsWith("\"Font:"))
		{
			text = line.Substring("\"Font:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x00157A9C File Offset: 0x00155C9C
	public static string GetCurrentLanguageCode()
	{
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			return Localization.DEFAULT_LANGUAGE_CODE;
		case Localization.SelectedLanguageType.Preinstalled:
			return KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_CODE_KEY);
		case Localization.SelectedLanguageType.UGC:
			return LanguageOptionsScreen.GetInstalledLanguageCode();
		default:
			return "";
		}
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x00157AE0 File Offset: 0x00155CE0
	public static Localization.SelectedLanguageType GetSelectedLanguageType()
	{
		return (Localization.SelectedLanguageType)Enum.Parse(typeof(Localization.SelectedLanguageType), KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()), true);
	}

	// Token: 0x06003DC8 RID: 15816 RVA: 0x00157B1C File Offset: 0x00155D1C
	private static string GetLanguageCode(string line)
	{
		string text = null;
		if (line.StartsWith("\"Language:"))
		{
			text = line.Substring("\"Language:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x00157B74 File Offset: 0x00155D74
	private static Localization.Locale GetLocaleForCode(string code)
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesCode(code))
			{
				result = locale;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00157BD0 File Offset: 0x00155DD0
	public static Localization.Locale GetLocale(string[] lines)
	{
		Localization.Locale locale = null;
		string text = null;
		if (lines != null && lines.Length != 0)
		{
			foreach (string text2 in lines)
			{
				if (text2 != null && text2.Length != 0)
				{
					text = Localization.GetLanguageCode(text2);
					if (text != null)
					{
						locale = Localization.GetLocaleForCode(text);
					}
					if (text != null)
					{
						break;
					}
				}
			}
		}
		if (locale == null)
		{
			locale = Localization.GetDefaultLocale();
		}
		if (text != null && locale.Code == "")
		{
			locale.SetCode(text);
		}
		return locale;
	}

	// Token: 0x06003DCB RID: 15819 RVA: 0x00157C45 File Offset: 0x00155E45
	private static string GetFontName(string filename)
	{
		return Localization.GetFontName(File.ReadAllLines(filename, Encoding.UTF8));
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x00157C58 File Offset: 0x00155E58
	public static Localization.Locale GetDefaultLocale()
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = new Localization.Locale(locale);
				break;
			}
		}
		return result;
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00157CB8 File Offset: 0x00155EB8
	public static string GetDefaultFontName()
	{
		string result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = locale.FontName;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x00157D18 File Offset: 0x00155F18
	public static string ValidateFontName(string fontName)
	{
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesFont(fontName))
			{
				return locale.FontName;
			}
		}
		return null;
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x00157D78 File Offset: 0x00155F78
	public static string GetFontName(string[] lines)
	{
		string text = null;
		foreach (string text2 in lines)
		{
			if (text2 != null && text2.Length != 0)
			{
				string fontParam = Localization.GetFontParam(text2);
				if (fontParam != null)
				{
					text = Localization.ValidateFontName(fontParam);
				}
			}
			if (text != null)
			{
				break;
			}
		}
		if (text == null)
		{
			if (Localization.sLocale != null)
			{
				text = Localization.sLocale.FontName;
			}
			else
			{
				text = Localization.GetDefaultFontName();
			}
		}
		return text;
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x00157DDB File Offset: 0x00155FDB
	public static void SwapToLocalizedFont()
	{
		Localization.SwapToLocalizedFont(Localization.currentFontName);
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x00157DE8 File Offset: 0x00155FE8
	public static bool SwapToLocalizedFont(string fontname)
	{
		if (string.IsNullOrEmpty(fontname))
		{
			return false;
		}
		Localization.sFontAsset = Localization.GetFont(fontname);
		foreach (TextStyleSetting textStyleSetting in Resources.FindObjectsOfTypeAll<TextStyleSetting>())
		{
			if (textStyleSetting != null)
			{
				textStyleSetting.sdfFont = Localization.sFontAsset;
			}
		}
		bool isRightToLeft = Localization.IsRightToLeft;
		foreach (LocText locText in Resources.FindObjectsOfTypeAll<LocText>())
		{
			if (locText != null)
			{
				locText.SwapFont(Localization.sFontAsset, isRightToLeft);
			}
		}
		return true;
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00157E70 File Offset: 0x00156070
	private static bool SetFont(Type target_type, object target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		if (target_type == null || target == null || font == null)
		{
			return false;
		}
		foreach (FieldInfo fieldInfo in target_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!excluded_members.Contains(fieldInfo))
			{
				if (fieldInfo.FieldType == typeof(TextStyleSetting))
				{
					((TextStyleSetting)fieldInfo.GetValue(target)).sdfFont = font;
				}
				else if (fieldInfo.FieldType == typeof(LocText))
				{
					((LocText)fieldInfo.GetValue(target)).SwapFont(font, is_right_to_left);
				}
				else if (fieldInfo.FieldType == typeof(GameObject))
				{
					foreach (Component component in ((GameObject)fieldInfo.GetValue(target)).GetComponents<Component>())
					{
						Localization.SetFont(component.GetType(), component, font, is_right_to_left, excluded_members);
					}
				}
				else if (fieldInfo.MemberType == MemberTypes.Field && fieldInfo.FieldType != fieldInfo.DeclaringType)
				{
					Localization.SetFont(fieldInfo.FieldType, fieldInfo.GetValue(target), font, is_right_to_left, excluded_members);
				}
			}
		}
		return true;
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x00157FA9 File Offset: 0x001561A9
	public static bool SetFont<T>(T target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		return Localization.SetFont(typeof(T), target, font, is_right_to_left, excluded_members);
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x00157FC4 File Offset: 0x001561C4
	public static TMP_FontAsset GetFont(string fontname)
	{
		foreach (TMP_FontAsset tmp_FontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
		{
			if (tmp_FontAsset.name == fontname)
			{
				return tmp_FontAsset;
			}
		}
		return null;
	}

	// Token: 0x06003DD5 RID: 15829 RVA: 0x00157FFC File Offset: 0x001561FC
	private static bool HasSameOrLessTokenCount(string english_string, string translated_string, string token)
	{
		int num = english_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		int num2 = translated_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		return num >= num2;
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x00158038 File Offset: 0x00156238
	private static bool HasSameOrLessLinkCountAsEnglish(string english_string, string translated_string)
	{
		return Localization.HasSameOrLessTokenCount(english_string, translated_string, "<link") && Localization.HasSameOrLessTokenCount(english_string, translated_string, "</link");
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00158058 File Offset: 0x00156258
	private static bool HasMatchingLinkTags(string str, int idx = 0)
	{
		int num = str.IndexOf("<link", idx);
		int num2 = str.IndexOf("</link", idx);
		if (num == -1 && num2 == -1)
		{
			return true;
		}
		if (num == -1 && num2 != -1)
		{
			return false;
		}
		if (num != -1 && num2 == -1)
		{
			return false;
		}
		if (num2 < num)
		{
			return false;
		}
		int num3 = str.IndexOf("<link", num + 1);
		return (num < 0 || num3 == -1 || num3 >= num2) && Localization.HasMatchingLinkTags(str, num2 + 1);
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x001580CC File Offset: 0x001562CC
	private static bool AreParametersPreserved(string old_string, string new_string)
	{
		MatchCollection matchCollection = Regex.Matches(old_string, "({.[^}]*?})(?!.*\\1)");
		MatchCollection matchCollection2 = Regex.Matches(new_string, "({.[^}]*?})(?!.*\\1)");
		bool result = false;
		if (matchCollection == null && matchCollection2 == null)
		{
			result = true;
		}
		else if (matchCollection != null && matchCollection2 != null && matchCollection.Count == matchCollection2.Count)
		{
			result = true;
			foreach (object obj in matchCollection)
			{
				string a = obj.ToString();
				bool flag = false;
				foreach (object obj2 in matchCollection2)
				{
					string b = obj2.ToString();
					if (a == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003DD9 RID: 15833 RVA: 0x001581C4 File Offset: 0x001563C4
	public static bool HasDirtyWords(string str)
	{
		return Localization.FilterDirtyWords(str) != str;
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x001581D2 File Offset: 0x001563D2
	public static string FilterDirtyWords(string str)
	{
		return DistributionPlatform.Inst.ApplyWordFilter(str);
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x001581DF File Offset: 0x001563DF
	public static string GetFileDateFormat(int format_idx)
	{
		return "{" + format_idx.ToString() + ":dd / MMM / yyyy}";
	}

	// Token: 0x06003DDC RID: 15836 RVA: 0x001581F8 File Offset: 0x001563F8
	public static void ClearLanguage()
	{
		DebugUtil.LogArgs(new object[]
		{
			" -> Clearing selected language! Either it didn't load correct or returning to english by menu."
		});
		Localization.sFontAsset = null;
		Localization.sLocale = null;
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString());
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, "");
		Localization.SwapToLocalizedFont(Localization.GetDefaultLocale().FontName);
		string defaultLocalizationFilePath = Localization.GetDefaultLocalizationFilePath();
		if (File.Exists(defaultLocalizationFilePath))
		{
			Localization.LoadTranslation(File.ReadAllLines(defaultLocalizationFilePath, Encoding.UTF8), true);
		}
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x00158284 File Offset: 0x00156484
	private static string ReverseText(string source)
	{
		char[] separator = new char[]
		{
			'\n'
		};
		string[] array = source.Split(separator);
		string text = "";
		int num = 0;
		foreach (string text2 in array)
		{
			num++;
			char[] array3 = new char[text2.Length];
			for (int j = 0; j < text2.Length; j++)
			{
				array3[array3.Length - 1 - j] = text2[j];
			}
			text += new string(array3);
			if (num < array.Length)
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x00158328 File Offset: 0x00156528
	public static string Fixup(string text)
	{
		if (Localization.sLocale != null && text != null && text != "" && Localization.sLocale.Lang == Localization.Language.Arabic)
		{
			return Localization.ReverseText(ArabicFixer.Fix(text));
		}
		return text;
	}

	// Token: 0x04002826 RID: 10278
	private static TMP_FontAsset sFontAsset = null;

	// Token: 0x04002827 RID: 10279
	private static readonly List<Localization.Locale> Locales = new List<Localization.Locale>
	{
		new Localization.Locale(Localization.Language.Chinese, Localization.Direction.LeftToRight, "zh", "NotoSansCJKsc-Regular"),
		new Localization.Locale(Localization.Language.Japanese, Localization.Direction.LeftToRight, "ja", "NotoSansCJKjp-Regular"),
		new Localization.Locale(Localization.Language.Korean, Localization.Direction.LeftToRight, "ko", "NotoSansCJKkr-Regular"),
		new Localization.Locale(Localization.Language.Russian, Localization.Direction.LeftToRight, "ru", "RobotoCondensed-Regular"),
		new Localization.Locale(Localization.Language.Thai, Localization.Direction.LeftToRight, "th", "NotoSansThai-Regular"),
		new Localization.Locale(Localization.Language.Arabic, Localization.Direction.RightToLeft, "ar", "NotoNaskhArabic-Regular"),
		new Localization.Locale(Localization.Language.Hebrew, Localization.Direction.RightToLeft, "he", "NotoSansHebrew-Regular"),
		new Localization.Locale(Localization.Language.Unspecified, Localization.Direction.LeftToRight, "", "RobotoCondensed-Regular")
	};

	// Token: 0x04002828 RID: 10280
	private static Localization.Locale sLocale = null;

	// Token: 0x04002829 RID: 10281
	private static string currentFontName = null;

	// Token: 0x0400282A RID: 10282
	public static string DEFAULT_LANGUAGE_CODE = "en";

	// Token: 0x0400282B RID: 10283
	public static readonly List<string> PreinstalledLanguages = new List<string>
	{
		Localization.DEFAULT_LANGUAGE_CODE,
		"zh_klei",
		"ko_klei",
		"ru_klei"
	};

	// Token: 0x0400282C RID: 10284
	public static string SELECTED_LANGUAGE_TYPE_KEY = "SelectedLanguageType";

	// Token: 0x0400282D RID: 10285
	public static string SELECTED_LANGUAGE_CODE_KEY = "SelectedLanguageCode";

	// Token: 0x0400282E RID: 10286
	private static Dictionary<string, List<Assembly>> translatable_assemblies = new Dictionary<string, List<Assembly>>();

	// Token: 0x0400282F RID: 10287
	public const BindingFlags non_static_data_member_fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

	// Token: 0x04002830 RID: 10288
	private const string start_link_token = "<link";

	// Token: 0x04002831 RID: 10289
	private const string end_link_token = "</link";

	// Token: 0x02001619 RID: 5657
	public enum Language
	{
		// Token: 0x04006AA6 RID: 27302
		Chinese,
		// Token: 0x04006AA7 RID: 27303
		Japanese,
		// Token: 0x04006AA8 RID: 27304
		Korean,
		// Token: 0x04006AA9 RID: 27305
		Russian,
		// Token: 0x04006AAA RID: 27306
		Thai,
		// Token: 0x04006AAB RID: 27307
		Arabic,
		// Token: 0x04006AAC RID: 27308
		Hebrew,
		// Token: 0x04006AAD RID: 27309
		Unspecified
	}

	// Token: 0x0200161A RID: 5658
	public enum Direction
	{
		// Token: 0x04006AAF RID: 27311
		LeftToRight,
		// Token: 0x04006AB0 RID: 27312
		RightToLeft
	}

	// Token: 0x0200161B RID: 5659
	public class Locale
	{
		// Token: 0x060089B8 RID: 35256 RVA: 0x00311DB6 File Offset: 0x0030FFB6
		public Locale(Localization.Locale other)
		{
			this.mLanguage = other.mLanguage;
			this.mDirection = other.mDirection;
			this.mCode = other.mCode;
			this.mFontName = other.mFontName;
		}

		// Token: 0x060089B9 RID: 35257 RVA: 0x00311DEE File Offset: 0x0030FFEE
		public Locale(Localization.Language language, Localization.Direction direction, string code, string fontName)
		{
			this.mLanguage = language;
			this.mDirection = direction;
			this.mCode = code.ToLower();
			this.mFontName = fontName;
		}

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x060089BA RID: 35258 RVA: 0x00311E18 File Offset: 0x00310018
		public Localization.Language Lang
		{
			get
			{
				return this.mLanguage;
			}
		}

		// Token: 0x060089BB RID: 35259 RVA: 0x00311E20 File Offset: 0x00310020
		public void SetCode(string code)
		{
			this.mCode = code;
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x060089BC RID: 35260 RVA: 0x00311E29 File Offset: 0x00310029
		public string Code
		{
			get
			{
				return this.mCode;
			}
		}

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x060089BD RID: 35261 RVA: 0x00311E31 File Offset: 0x00310031
		public string FontName
		{
			get
			{
				return this.mFontName;
			}
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x060089BE RID: 35262 RVA: 0x00311E39 File Offset: 0x00310039
		public bool IsRightToLeft
		{
			get
			{
				return this.mDirection == Localization.Direction.RightToLeft;
			}
		}

		// Token: 0x060089BF RID: 35263 RVA: 0x00311E44 File Offset: 0x00310044
		public bool MatchesCode(string language_code)
		{
			return language_code.ToLower().Contains(this.mCode);
		}

		// Token: 0x060089C0 RID: 35264 RVA: 0x00311E57 File Offset: 0x00310057
		public bool MatchesFont(string fontname)
		{
			return fontname.ToLower() == this.mFontName.ToLower();
		}

		// Token: 0x060089C1 RID: 35265 RVA: 0x00311E70 File Offset: 0x00310070
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.mCode,
				":",
				this.mLanguage.ToString(),
				":",
				this.mDirection.ToString(),
				":",
				this.mFontName
			});
		}

		// Token: 0x04006AB1 RID: 27313
		private Localization.Language mLanguage;

		// Token: 0x04006AB2 RID: 27314
		private string mCode;

		// Token: 0x04006AB3 RID: 27315
		private string mFontName;

		// Token: 0x04006AB4 RID: 27316
		private Localization.Direction mDirection;
	}

	// Token: 0x0200161C RID: 5660
	private struct Entry
	{
		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x060089C2 RID: 35266 RVA: 0x00311EDA File Offset: 0x003100DA
		public bool IsPopulated
		{
			get
			{
				return this.msgctxt != null && this.msgstr != null && this.msgstr.Length > 0;
			}
		}

		// Token: 0x04006AB5 RID: 27317
		public string msgctxt;

		// Token: 0x04006AB6 RID: 27318
		public string msgstr;
	}

	// Token: 0x0200161D RID: 5661
	public enum SelectedLanguageType
	{
		// Token: 0x04006AB8 RID: 27320
		None,
		// Token: 0x04006AB9 RID: 27321
		Preinstalled,
		// Token: 0x04006ABA RID: 27322
		UGC
	}
}
