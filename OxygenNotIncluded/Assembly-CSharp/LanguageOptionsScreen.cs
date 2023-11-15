using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B4D RID: 2893
public class LanguageOptionsScreen : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x06005934 RID: 22836 RVA: 0x0020A280 File Offset: 0x00208480
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += this.Deactivate;
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += this.Deactivate;
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.uninstallButton.onClick += delegate()
		{
			this.OnClickUninstall();
		};
		this.uninstallButton.gameObject.SetActive(false);
		this.RebuildScreen();
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x0020A32C File Offset: 0x0020852C
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.uninstallButton.isInteractable = (KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()) != Localization.SelectedLanguageType.None.ToString());
		this.RebuildPreinstalledButtons();
		this.RebuildUGCButtons();
	}

	// Token: 0x06005936 RID: 22838 RVA: 0x0020A3CC File Offset: 0x002085CC
	private void RebuildPreinstalledButtons()
	{
		using (List<string>.Enumerator enumerator = Localization.PreinstalledLanguages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string code = enumerator.Current;
				if (!(code != Localization.DEFAULT_LANGUAGE_CODE) || File.Exists(Localization.GetPreinstalledLocalizationFilePath(code)))
				{
					GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.preinstalledLanguagesContainer, false);
					gameObject.name = code + "_button";
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					LocText reference = component.GetReference<LocText>("Title");
					reference.text = Localization.GetPreinstalledLocalizationTitle(code);
					reference.enabled = false;
					reference.enabled = true;
					Texture2D preinstalledLocalizationImage = Localization.GetPreinstalledLocalizationImage(code);
					if (preinstalledLocalizationImage != null)
					{
						component.GetReference<Image>("Image").sprite = Sprite.Create(preinstalledLocalizationImage, new Rect(Vector2.zero, new Vector2((float)preinstalledLocalizationImage.width, (float)preinstalledLocalizationImage.height)), Vector2.one * 0.5f);
					}
					gameObject.GetComponent<KButton>().onClick += delegate()
					{
						this.ConfirmLanguagePreinstalledOrMod((code != Localization.DEFAULT_LANGUAGE_CODE) ? code : string.Empty, null);
					};
					this.buttons.Add(gameObject);
				}
			}
		}
	}

	// Token: 0x06005937 RID: 22839 RVA: 0x0020A53C File Offset: 0x0020873C
	protected override void OnActivate()
	{
		base.OnActivate();
		Global.Instance.modManager.Sanitize(base.gameObject);
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.AddClient(this);
		}
	}

	// Token: 0x06005938 RID: 22840 RVA: 0x0020A571 File Offset: 0x00208771
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.RemoveClient(this);
		}
	}

	// Token: 0x06005939 RID: 22841 RVA: 0x0020A594 File Offset: 0x00208794
	private void ConfirmLanguageChoiceDialog(string[] lines, bool is_template, System.Action install_language)
	{
		Localization.Locale locale = Localization.GetLocale(lines);
		Dictionary<string, string> translated_strings = Localization.ExtractTranslatedStrings(lines, is_template);
		TMP_FontAsset font = Localization.GetFont(locale.FontName);
		ConfirmDialogScreen screen = this.GetConfirmDialog();
		HashSet<MemberInfo> excluded_members = new HashSet<MemberInfo>(typeof(ConfirmDialogScreen).GetMember("cancelButton", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
		Localization.SetFont<ConfirmDialogScreen>(screen, font, locale.IsRightToLeft, excluded_members);
		Func<LocString, string> func = delegate(LocString loc_string)
		{
			string result;
			if (!translated_strings.TryGetValue(loc_string.key.String, out result))
			{
				return loc_string;
			}
			return result;
		};
		ConfirmDialogScreen screen2 = screen;
		string title_text = func(UI.CONFIRMDIALOG.DIALOG_HEADER);
		screen2.PopupConfirmDialog(func(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT), delegate
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			install_language();
			App.instance.Restart();
		}, delegate
		{
			Localization.SetFont<ConfirmDialogScreen>(screen, Localization.FontAsset, Localization.IsRightToLeft, excluded_members);
		}, null, null, title_text, func(UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART), UI.FRONTEND.TRANSLATIONS_SCREEN.CANCEL, null);
	}

	// Token: 0x0600593A RID: 22842 RVA: 0x0020A676 File Offset: 0x00208876
	private void ConfirmPreinstalledLanguage(string selected_preinstalled_translation)
	{
		Localization.GetSelectedLanguageType();
	}

	// Token: 0x0600593B RID: 22843 RVA: 0x0020A680 File Offset: 0x00208880
	private void ConfirmLanguagePreinstalledOrMod(string selected_preinstalled_translation, string mod_id)
	{
		Localization.SelectedLanguageType selectedLanguageType = Localization.GetSelectedLanguageType();
		if (mod_id != null)
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.UGC && mod_id == this.currentLanguageModId)
			{
				this.Deactivate();
				return;
			}
			string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(mod_id);
			this.ConfirmLanguageChoiceDialog(languageLinesForMod, false, delegate
			{
				LanguageOptionsScreen.SetCurrentLanguage(mod_id);
			});
			return;
		}
		else if (!string.IsNullOrEmpty(selected_preinstalled_translation))
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (selectedLanguageType == Localization.SelectedLanguageType.Preinstalled && currentLanguageCode == selected_preinstalled_translation)
			{
				this.Deactivate();
				return;
			}
			string[] lines = File.ReadAllLines(Localization.GetPreinstalledLocalizationFilePath(selected_preinstalled_translation), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines, false, delegate
			{
				Localization.LoadPreinstalledTranslation(selected_preinstalled_translation);
			});
			return;
		}
		else
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.None)
			{
				this.Deactivate();
				return;
			}
			string[] lines2 = File.ReadAllLines(Localization.GetDefaultLocalizationFilePath(), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines2, true, delegate
			{
				Localization.ClearLanguage();
			});
			return;
		}
	}

	// Token: 0x0600593C RID: 22844 RVA: 0x0020A78A File Offset: 0x0020898A
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x0600593D RID: 22845 RVA: 0x0020A7C0 File Offset: 0x002089C0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600593E RID: 22846 RVA: 0x0020A7E4 File Offset: 0x002089E4
	private void RebuildUGCButtons()
	{
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if ((mod.available_content & Content.Translation) != (Content)0 && mod.status == Mod.Status.Installed)
			{
				GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.ugcLanguagesContainer, false);
				gameObject.name = mod.title + "_button";
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				TMP_FontAsset font = Localization.GetFont(Localization.GetFontName(LanguageOptionsScreen.GetLanguageLinesForMod(mod)));
				LocText reference = component.GetReference<LocText>("Title");
				reference.SetText(string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.UGC_MOD_TITLE_FORMAT, mod.title));
				reference.font = font;
				Texture2D previewImage = mod.GetPreviewImage();
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				string mod_id = mod.label.id;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.ConfirmLanguagePreinstalledOrMod(string.Empty, mod_id);
				};
				this.buttons.Add(gameObject);
			}
		}
	}

	// Token: 0x0600593F RID: 22847 RVA: 0x0020A970 File Offset: 0x00208B70
	private void Uninstall()
	{
		this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.ARE_YOU_SURE, delegate
		{
			Localization.ClearLanguage();
			this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, new System.Action(App.instance.Restart), new System.Action(this.Deactivate), null, null, null, null, null, null);
		}, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x06005940 RID: 22848 RVA: 0x0020A9C3 File Offset: 0x00208BC3
	private void OnClickUninstall()
	{
		this.Uninstall();
	}

	// Token: 0x06005941 RID: 22849 RVA: 0x0020A9CB File Offset: 0x00208BCB
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=language");
	}

	// Token: 0x06005942 RID: 22850 RVA: 0x0020A9D8 File Offset: 0x00208BD8
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
		ulong value;
		if (ulong.TryParse(savedLanguageMod, out value))
		{
			PublishedFileId_t value2 = (PublishedFileId_t)value;
			if (removed.Contains(value2))
			{
				global::Debug.Log("Unsubscribe detected for currently installed language mod [" + savedLanguageMod + "]");
				this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, delegate
				{
					Localization.ClearLanguage();
					App.instance.Restart();
				}, null, null, null, null, UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART, null, null);
			}
			if (updated.Contains(value2))
			{
				global::Debug.Log("Download complete for currently installed language [" + savedLanguageMod + "] updating in background. Changes will happen next restart.");
			}
		}
		this.RebuildScreen();
	}

	// Token: 0x06005943 RID: 22851 RVA: 0x0020AA85 File Offset: 0x00208C85
	public static string GetSavedLanguageMod()
	{
		return KPlayerPrefs.GetString("InstalledLanguage");
	}

	// Token: 0x06005944 RID: 22852 RVA: 0x0020AA91 File Offset: 0x00208C91
	public static void SetSavedLanguageMod(string mod_id)
	{
		KPlayerPrefs.SetString("InstalledLanguage", mod_id);
	}

	// Token: 0x06005945 RID: 22853 RVA: 0x0020AA9E File Offset: 0x00208C9E
	public static void CleanUpSavedLanguageMod()
	{
		KPlayerPrefs.SetString("InstalledLanguage", null);
	}

	// Token: 0x17000673 RID: 1651
	// (get) Token: 0x06005946 RID: 22854 RVA: 0x0020AAAB File Offset: 0x00208CAB
	// (set) Token: 0x06005947 RID: 22855 RVA: 0x0020AAB3 File Offset: 0x00208CB3
	public string currentLanguageModId
	{
		get
		{
			return this._currentLanguageModId;
		}
		private set
		{
			this._currentLanguageModId = value;
			LanguageOptionsScreen.SetSavedLanguageMod(this._currentLanguageModId);
		}
	}

	// Token: 0x06005948 RID: 22856 RVA: 0x0020AAC7 File Offset: 0x00208CC7
	public static bool SetCurrentLanguage(string mod_id)
	{
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
		if (LanguageOptionsScreen.LoadTranslation(mod_id))
		{
			LanguageOptionsScreen.SetSavedLanguageMod(mod_id);
			return true;
		}
		return false;
	}

	// Token: 0x06005949 RID: 22857 RVA: 0x0020AAE0 File Offset: 0x00208CE0
	public static bool HasInstalledLanguage()
	{
		string currentModId = LanguageOptionsScreen.GetSavedLanguageMod();
		if (currentModId == null)
		{
			return false;
		}
		if (Global.Instance.modManager.mods.Find((Mod m) => m.label.id == currentModId) == null)
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			return false;
		}
		return true;
	}

	// Token: 0x0600594A RID: 22858 RVA: 0x0020AB34 File Offset: 0x00208D34
	public static string GetInstalledLanguageCode()
	{
		string result = "";
		string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(LanguageOptionsScreen.GetSavedLanguageMod());
		if (languageLinesForMod != null)
		{
			Localization.Locale locale = Localization.GetLocale(languageLinesForMod);
			if (locale != null)
			{
				result = locale.Code;
			}
		}
		return result;
	}

	// Token: 0x0600594B RID: 22859 RVA: 0x0020AB68 File Offset: 0x00208D68
	private static bool LoadTranslation(string mod_id)
	{
		Mod mod = Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id);
		if (mod == null)
		{
			global::Debug.LogWarning("Tried loading a translation from a non-existent mod id: " + mod_id);
			return false;
		}
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		return languageFilename != null && Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.UGC, languageFilename);
	}

	// Token: 0x0600594C RID: 22860 RVA: 0x0020ABD0 File Offset: 0x00208DD0
	private static string GetLanguageFilename(Mod mod)
	{
		global::Debug.Assert(mod.content_source.GetType() == typeof(KMod.Directory), "Can only load translations from extracted mods.");
		string text = Path.Combine(mod.ContentPath, "strings.po");
		if (!File.Exists(text))
		{
			global::Debug.LogWarning("GetLanguagFile: " + text + " missing for mod " + mod.label.title);
			return null;
		}
		return text;
	}

	// Token: 0x0600594D RID: 22861 RVA: 0x0020AC40 File Offset: 0x00208E40
	private static string[] GetLanguageLinesForMod(string mod_id)
	{
		return LanguageOptionsScreen.GetLanguageLinesForMod(Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id));
	}

	// Token: 0x0600594E RID: 22862 RVA: 0x0020AC80 File Offset: 0x00208E80
	private static string[] GetLanguageLinesForMod(Mod mod)
	{
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		if (languageFilename == null)
		{
			return null;
		}
		string[] array = File.ReadAllLines(languageFilename, Encoding.UTF8);
		if (array == null || array.Length == 0)
		{
			global::Debug.LogWarning("Couldn't find any strings in the translation mod " + mod.label.title);
			return null;
		}
		return array;
	}

	// Token: 0x04003C4E RID: 15438
	private static readonly string[] poFile = new string[]
	{
		"strings.po"
	};

	// Token: 0x04003C4F RID: 15439
	public const string KPLAYER_PREFS_LANGUAGE_KEY = "InstalledLanguage";

	// Token: 0x04003C50 RID: 15440
	public const string TAG_LANGUAGE = "language";

	// Token: 0x04003C51 RID: 15441
	public KButton textButton;

	// Token: 0x04003C52 RID: 15442
	public KButton dismissButton;

	// Token: 0x04003C53 RID: 15443
	public KButton closeButton;

	// Token: 0x04003C54 RID: 15444
	public KButton workshopButton;

	// Token: 0x04003C55 RID: 15445
	public KButton uninstallButton;

	// Token: 0x04003C56 RID: 15446
	[Space]
	public GameObject languageButtonPrefab;

	// Token: 0x04003C57 RID: 15447
	public GameObject preinstalledLanguagesTitle;

	// Token: 0x04003C58 RID: 15448
	public GameObject preinstalledLanguagesContainer;

	// Token: 0x04003C59 RID: 15449
	public GameObject ugcLanguagesTitle;

	// Token: 0x04003C5A RID: 15450
	public GameObject ugcLanguagesContainer;

	// Token: 0x04003C5B RID: 15451
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04003C5C RID: 15452
	private string _currentLanguageModId;

	// Token: 0x04003C5D RID: 15453
	private System.DateTime currentLastModified;
}
