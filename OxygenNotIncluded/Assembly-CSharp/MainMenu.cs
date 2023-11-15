using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B55 RID: 2901
public class MainMenu : KScreen
{
	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06005991 RID: 22929 RVA: 0x0020C0B2 File Offset: 0x0020A2B2
	public static MainMenu Instance
	{
		get
		{
			return MainMenu._instance;
		}
	}

	// Token: 0x06005992 RID: 22930 RVA: 0x0020C0BC File Offset: 0x0020A2BC
	private KButton MakeButton(MainMenu.ButtonInfo info)
	{
		KButton kbutton = global::Util.KInstantiateUI<KButton>(this.buttonPrefab.gameObject, this.buttonParent, true);
		kbutton.onClick += info.action;
		KImage component = kbutton.GetComponent<KImage>();
		component.colorStyleSetting = info.style;
		component.ApplyColorStyleSetting();
		LocText componentInChildren = kbutton.GetComponentInChildren<LocText>();
		componentInChildren.text = info.text;
		componentInChildren.fontSize = (float)info.fontSize;
		return kbutton;
	}

	// Token: 0x06005993 RID: 22931 RVA: 0x0020C128 File Offset: 0x0020A328
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MainMenu._instance = this;
		this.Button_NewGame = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.NEWGAME, new System.Action(this.NewGame), 22, this.topButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOADGAME, new System.Action(this.LoadGame), 22, this.normalButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.RETIREDCOLONIES, delegate()
		{
			MainMenu.ActivateRetiredColoniesScreen(this.transform.gameObject, "");
		}, 14, this.normalButtonStyle));
		this.lockerButton = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOCKERMENU, delegate()
		{
			MainMenu.ActivateLockerMenu();
		}, 14, this.normalButtonStyle));
		if (DistributionPlatform.Initialized)
		{
			this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.TRANSLATIONS, new System.Action(this.Translations), 14, this.normalButtonStyle));
			this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MODS.TITLE, new System.Action(this.Mods), 14, this.normalButtonStyle));
		}
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.OPTIONS, new System.Action(this.Options), 14, this.normalButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.QUITTODESKTOP, new System.Action(this.QuitGame), 14, this.normalButtonStyle));
		this.RefreshResumeButton(false);
		this.Button_ResumeGame.onClick += this.ResumeGame;
		this.SpawnVideoScreen();
		this.StartFEAudio();
		this.CheckPlayerPrefsCorruption();
		if (PatchNotesScreen.ShouldShowScreen())
		{
			global::Util.KInstantiateUI(this.patchNotesScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
		}
		this.CheckDoubleBoundKeys();
		this.topLeftAlphaMessage.gameObject.SetActive(false);
		this.MOTDContainer.SetActive(false);
		this.buttonContainer.SetActive(false);
		this.nextUpdateTimer.gameObject.SetActive(true);
		bool flag = DistributionPlatform.Inst.IsDLCPurchased("EXPANSION1_ID");
		this.expansion1Toggle.gameObject.SetActive(flag);
		if (this.expansion1Ad != null)
		{
			this.expansion1Ad.gameObject.SetActive(!flag);
		}
		this.m_motdServerClient = new MotdServerClient();
		this.m_motdServerClient.GetMotd(delegate(MotdServerClient.MotdResponse response, string error)
		{
			if (error == null)
			{
				if (DlcManager.IsExpansion1Active())
				{
					this.nextUpdateTimer.UpdateReleaseTimes(response.expansion1_update_data.last_update_time, response.expansion1_update_data.next_update_time, response.expansion1_update_data.update_text_override);
				}
				else
				{
					this.nextUpdateTimer.UpdateReleaseTimes(response.vanilla_update_data.last_update_time, response.vanilla_update_data.next_update_time, response.vanilla_update_data.update_text_override);
				}
				this.topLeftAlphaMessage.gameObject.SetActive(true);
				this.MOTDContainer.SetActive(true);
				this.buttonContainer.SetActive(true);
				this.motdImageHeader.text = response.image_header_text;
				this.motdNewsHeader.text = response.news_header_text;
				this.motdNewsBody.text = response.news_body_text;
				PatchNotesScreen.UpdatePatchNotes(response.patch_notes_summary, response.patch_notes_link_url);
				if (response.image_texture != null)
				{
					this.motdImage.sprite = Sprite.Create(response.image_texture, new Rect(0f, 0f, (float)response.image_texture.width, (float)response.image_texture.height), Vector2.zero);
				}
				else
				{
					global::Debug.LogWarning("GetMotd failed to return an image texture");
				}
				if (this.motdImage.sprite != null && this.motdImage.sprite.rect.height != 0f)
				{
					AspectRatioFitter component = this.motdImage.gameObject.GetComponent<AspectRatioFitter>();
					if (component != null)
					{
						float aspectRatio = this.motdImage.sprite.rect.width / this.motdImage.sprite.rect.height;
						component.aspectRatio = aspectRatio;
					}
					else
					{
						global::Debug.LogWarning("Missing AspectRatioFitter on MainMenu motd image.");
					}
				}
				else
				{
					global::Debug.LogWarning("Cannot resize motd image, missing sprite");
				}
				this.motdImageButton.ClearOnClick();
				this.motdImageButton.onClick += delegate()
				{
					App.OpenWebURL(response.image_link_url);
				};
				return;
			}
			global::Debug.LogWarning("Motd Request error: " + error);
		});
		if (DistributionPlatform.Initialized && DistributionPlatform.Inst.IsPreviousVersionBranch)
		{
			UnityEngine.Object.Instantiate<GameObject>(ScreenPrefabs.Instance.OldVersionWarningScreen, this.uiCanvas.transform);
		}
		string targetExpansion1AdURL = "";
		Sprite sprite = Assets.GetSprite("expansionPromo_en");
		if (DistributionPlatform.Initialized && this.expansion1Ad != null)
		{
			string name = DistributionPlatform.Inst.Name;
			if (name != null)
			{
				if (!(name == "Steam"))
				{
					if (!(name == "Epic"))
					{
						if (name == "Rail")
						{
							targetExpansion1AdURL = "https://www.wegame.com.cn/store/2001539/";
							sprite = Assets.GetSprite("expansionPromo_cn");
						}
					}
					else
					{
						targetExpansion1AdURL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
					}
				}
				else
				{
					targetExpansion1AdURL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
				}
			}
			this.expansion1Ad.GetComponentInChildren<KButton>().onClick += delegate()
			{
				App.OpenWebURL(targetExpansion1AdURL);
			};
			this.expansion1Ad.GetComponent<HierarchyReferences>().GetReference<Image>("Image").sprite = sprite;
		}
		this.activateOnSpawn = true;
	}

	// Token: 0x06005994 RID: 22932 RVA: 0x0020C4A0 File Offset: 0x0020A6A0
	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			this.RefreshResumeButton(false);
		}
	}

	// Token: 0x06005995 RID: 22933 RVA: 0x0020C4AC File Offset: 0x0020A6AC
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.DebugToggleUI))
		{
			this.m_screenshotMode = !this.m_screenshotMode;
			this.uiCanvas.alpha = (this.m_screenshotMode ? 0f : 1f);
		}
		KKeyCode key_code;
		switch (this.m_cheatInputCounter)
		{
		case 0:
			key_code = KKeyCode.K;
			break;
		case 1:
			key_code = KKeyCode.L;
			break;
		case 2:
			key_code = KKeyCode.E;
			break;
		case 3:
			key_code = KKeyCode.I;
			break;
		case 4:
			key_code = KKeyCode.P;
			break;
		case 5:
			key_code = KKeyCode.L;
			break;
		case 6:
			key_code = KKeyCode.A;
			break;
		default:
			key_code = KKeyCode.Y;
			break;
		}
		if (e.Controller.GetKeyDown(key_code))
		{
			e.Consumed = true;
			this.m_cheatInputCounter++;
			if (this.m_cheatInputCounter >= 8)
			{
				global::Debug.Log("Cheat Detected - enabling Debug Mode");
				DebugHandler.SetDebugEnabled(true);
				this.buildWatermark.RefreshText();
				this.m_cheatInputCounter = 0;
				return;
			}
		}
		else
		{
			this.m_cheatInputCounter = 0;
		}
	}

	// Token: 0x06005996 RID: 22934 RVA: 0x0020C5AB File Offset: 0x0020A7AB
	private void PlayMouseOverSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06005997 RID: 22935 RVA: 0x0020C5BD File Offset: 0x0020A7BD
	private void PlayMouseClickSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
	}

	// Token: 0x06005998 RID: 22936 RVA: 0x0020C5D0 File Offset: 0x0020A7D0
	protected override void OnSpawn()
	{
		global::Debug.Log("-- MAIN MENU -- ");
		base.OnSpawn();
		this.m_cheatInputCounter = 0;
		Canvas.ForceUpdateCanvases();
		this.ShowLanguageConfirmation();
		this.InitLoadScreen();
		LoadScreen.Instance.ShowMigrationIfNecessary(true);
		string savePrefix = SaveLoader.GetSavePrefix();
		try
		{
			string path = Path.Combine(savePrefix, "__SPCCHK");
			using (FileStream fileStream = File.OpenWrite(path))
			{
				byte[] array = new byte[1024];
				for (int i = 0; i < 15360; i++)
				{
					fileStream.Write(array, 0, array.Length);
				}
			}
			File.Delete(path);
		}
		catch (Exception ex)
		{
			string format;
			if (ex is IOException)
			{
				format = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, savePrefix);
			}
			else
			{
				format = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, savePrefix);
			}
			string text = string.Format(format, savePrefix);
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
		}
		Global.Instance.modManager.Report(base.gameObject);
		if ((GenericGameSettings.instance.autoResumeGame && !MainMenu.HasAutoresumedOnce && !KCrashReporter.hasCrash) || !string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame) || KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			MainMenu.HasAutoresumedOnce = true;
			this.ResumeGame();
		}
		if (GenericGameSettings.instance.devAutoWorldGen && !KCrashReporter.hasCrash)
		{
			GenericGameSettings.instance.devAutoWorldGen = false;
			GenericGameSettings.instance.devAutoWorldGenActive = true;
			GenericGameSettings.instance.SaveSettings();
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.WorldGenScreen.gameObject, base.gameObject, true);
		}
		this.RefreshInventoryNotification();
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x0020C798 File Offset: 0x0020A998
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x0600599A RID: 22938 RVA: 0x0020C7A0 File Offset: 0x0020A9A0
	private void RefreshInventoryNotification()
	{
		bool active = PermitItems.HasUnopenedItem();
		this.lockerButton.GetComponent<HierarchyReferences>().GetReference<RectTransform>("AttentionIcon").gameObject.SetActive(active);
	}

	// Token: 0x0600599B RID: 22939 RVA: 0x0020C7D3 File Offset: 0x0020A9D3
	private void UnregisterMotdRequest()
	{
		if (this.m_motdServerClient != null)
		{
			this.m_motdServerClient.UnregisterCallback();
			this.m_motdServerClient = null;
		}
	}

	// Token: 0x0600599C RID: 22940 RVA: 0x0020C7EF File Offset: 0x0020A9EF
	protected override void OnActivate()
	{
		if (!this.ambientLoopEventName.IsNullOrWhiteSpace())
		{
			this.ambientLoop = KFMOD.CreateInstance(GlobalAssets.GetSound(this.ambientLoopEventName, false));
			if (this.ambientLoop.isValid())
			{
				this.ambientLoop.start();
			}
		}
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x0020C82E File Offset: 0x0020AA2E
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.UnregisterMotdRequest();
	}

	// Token: 0x0600599E RID: 22942 RVA: 0x0020C83C File Offset: 0x0020AA3C
	public override void ScreenUpdate(bool topLevel)
	{
		this.refreshResumeButton = topLevel;
		if (KleiItemDropScreen.Instance != null && KleiItemDropScreen.Instance.gameObject.activeInHierarchy != this.itemDropOpenFlag)
		{
			this.RefreshInventoryNotification();
			this.itemDropOpenFlag = KleiItemDropScreen.Instance.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x0600599F RID: 22943 RVA: 0x0020C88F File Offset: 0x0020AA8F
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		this.StopAmbience();
		this.UnregisterMotdRequest();
	}

	// Token: 0x060059A0 RID: 22944 RVA: 0x0020C8A4 File Offset: 0x0020AAA4
	private void ShowLanguageConfirmation()
	{
		if (SteamManager.Initialized)
		{
			if (SteamUtils.GetSteamUILanguage() != "schinese")
			{
				return;
			}
			if (KPlayerPrefs.GetInt("LanguageConfirmationVersion") >= MainMenu.LANGUAGE_CONFIRMATION_VERSION)
			{
				return;
			}
			KPlayerPrefs.SetInt("LanguageConfirmationVersion", MainMenu.LANGUAGE_CONFIRMATION_VERSION);
			this.Translations();
		}
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x0020C8F4 File Offset: 0x0020AAF4
	private void ResumeGame()
	{
		string text;
		if (KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			text = KPlayerPrefs.GetString("AutoResumeSaveFile");
			KPlayerPrefs.DeleteKey("AutoResumeSaveFile");
		}
		else if (!string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame))
		{
			text = GenericGameSettings.instance.performanceCapture.saveGame;
		}
		else
		{
			text = SaveLoader.GetLatestSaveForCurrentDLC();
		}
		if (!string.IsNullOrEmpty(text))
		{
			KCrashReporter.MOST_RECENT_SAVEFILE = text;
			SaveLoader.SetActiveSaveFilePath(text);
			LoadingOverlay.Load(delegate
			{
				App.LoadScene("backend");
			});
		}
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x0020C98A File Offset: 0x0020AB8A
	private void NewGame()
	{
		base.GetComponent<NewGameFlow>().BeginFlow();
	}

	// Token: 0x060059A3 RID: 22947 RVA: 0x0020C997 File Offset: 0x0020AB97
	private void InitLoadScreen()
	{
		if (LoadScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.LoadScreen.gameObject, base.gameObject, true).GetComponent<LoadScreen>();
		}
	}

	// Token: 0x060059A4 RID: 22948 RVA: 0x0020C9C7 File Offset: 0x0020ABC7
	private void LoadGame()
	{
		this.InitLoadScreen();
		LoadScreen.Instance.Activate();
	}

	// Token: 0x060059A5 RID: 22949 RVA: 0x0020C9DC File Offset: 0x0020ABDC
	public static void ActivateRetiredColoniesScreen(GameObject parent, string colonyID = "")
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
		}
		RetiredColonyInfoScreen.Instance.Show(true);
		if (!string.IsNullOrEmpty(colonyID))
		{
			if (SaveGame.Instance != null)
			{
				RetireColonyUtility.SaveColonySummaryData();
			}
			RetiredColonyInfoScreen.Instance.LoadColony(RetiredColonyInfoScreen.Instance.GetColonyDataByBaseName(colonyID));
		}
	}

	// Token: 0x060059A6 RID: 22950 RVA: 0x0020CA48 File Offset: 0x0020AC48
	public static void ActivateRetiredColoniesScreenFromData(GameObject parent, RetiredColonyData data)
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
		}
		RetiredColonyInfoScreen.Instance.Show(true);
		RetiredColonyInfoScreen.Instance.LoadColony(data);
	}

	// Token: 0x060059A7 RID: 22951 RVA: 0x0020CA84 File Offset: 0x0020AC84
	public static void ActivateInventoyScreen()
	{
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
	}

	// Token: 0x060059A8 RID: 22952 RVA: 0x0020CA9B File Offset: 0x0020AC9B
	public static void ActivateLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x060059A9 RID: 22953 RVA: 0x0020CAA8 File Offset: 0x0020ACA8
	private void SpawnVideoScreen()
	{
		VideoScreen.Instance = global::Util.KInstantiateUI(ScreenPrefabs.Instance.VideoScreen.gameObject, base.gameObject, false).GetComponent<VideoScreen>();
	}

	// Token: 0x060059AA RID: 22954 RVA: 0x0020CACF File Offset: 0x0020ACCF
	private void Update()
	{
	}

	// Token: 0x060059AB RID: 22955 RVA: 0x0020CAD4 File Offset: 0x0020ACD4
	public void RefreshResumeButton(bool simpleCheck = false)
	{
		string latestSaveForCurrentDLC = SaveLoader.GetLatestSaveForCurrentDLC();
		bool flag = !string.IsNullOrEmpty(latestSaveForCurrentDLC) && File.Exists(latestSaveForCurrentDLC);
		if (flag)
		{
			try
			{
				if (GenericGameSettings.instance.demoMode)
				{
					flag = false;
				}
				System.DateTime lastWriteTime = File.GetLastWriteTime(latestSaveForCurrentDLC);
				MainMenu.SaveFileEntry saveFileEntry = default(MainMenu.SaveFileEntry);
				SaveGame.Header header = default(SaveGame.Header);
				SaveGame.GameInfo gameInfo = default(SaveGame.GameInfo);
				if (!this.saveFileEntries.TryGetValue(latestSaveForCurrentDLC, out saveFileEntry) || saveFileEntry.timeStamp != lastWriteTime)
				{
					gameInfo = SaveLoader.LoadHeader(latestSaveForCurrentDLC, out header);
					saveFileEntry = new MainMenu.SaveFileEntry
					{
						timeStamp = lastWriteTime,
						header = header,
						headerData = gameInfo
					};
					this.saveFileEntries[latestSaveForCurrentDLC] = saveFileEntry;
				}
				else
				{
					header = saveFileEntry.header;
					gameInfo = saveFileEntry.headerData;
				}
				if (header.buildVersion > 577063U || gameInfo.saveMajorVersion != 7 || gameInfo.saveMinorVersion > 32)
				{
					flag = false;
				}
				if (!DlcManager.IsContentActive(gameInfo.dlcId))
				{
					flag = false;
				}
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestSaveForCurrentDLC);
				if (!string.IsNullOrEmpty(gameInfo.baseName))
				{
					this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = string.Format(UI.FRONTEND.MAINMENU.RESUMEBUTTON_BASENAME, gameInfo.baseName, gameInfo.numberOfCycles + 1);
				}
				else
				{
					this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = fileNameWithoutExtension;
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
				flag = false;
			}
		}
		if (this.Button_ResumeGame != null && this.Button_ResumeGame.gameObject != null)
		{
			this.Button_ResumeGame.gameObject.SetActive(flag);
			KImage component = this.Button_NewGame.GetComponent<KImage>();
			component.colorStyleSetting = (flag ? this.normalButtonStyle : this.topButtonStyle);
			component.ApplyColorStyleSetting();
			return;
		}
		global::Debug.LogWarning("Why is the resume game button null?");
	}

	// Token: 0x060059AC RID: 22956 RVA: 0x0020CCBC File Offset: 0x0020AEBC
	private void Translations()
	{
		global::Util.KInstantiateUI<LanguageOptionsScreen>(ScreenPrefabs.Instance.languageOptionsScreen.gameObject, base.transform.parent.gameObject, false);
	}

	// Token: 0x060059AD RID: 22957 RVA: 0x0020CCE4 File Offset: 0x0020AEE4
	private void Mods()
	{
		global::Util.KInstantiateUI<ModsScreen>(ScreenPrefabs.Instance.modsMenu.gameObject, base.transform.parent.gameObject, false);
	}

	// Token: 0x060059AE RID: 22958 RVA: 0x0020CD0C File Offset: 0x0020AF0C
	private void Options()
	{
		global::Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, base.gameObject, true);
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x0020CD2A File Offset: 0x0020AF2A
	private void QuitGame()
	{
		App.Quit();
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x0020CD34 File Offset: 0x0020AF34
	public void StartFEAudio()
	{
		AudioMixer.instance.Reset();
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSnapshot);
		if (!AudioMixer.instance.SnapshotIsActive(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot))
		{
			AudioMixer.instance.StartUserVolumesSnapshot();
		}
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying(this.menuMusicEventName))
		{
			MusicManager.instance.PlaySong(this.menuMusicEventName, false);
		}
		this.CheckForAudioDriverIssue();
	}

	// Token: 0x060059B1 RID: 22961 RVA: 0x0020CDC0 File Offset: 0x0020AFC0
	public void StopAmbience()
	{
		if (this.ambientLoop.isValid())
		{
			this.ambientLoop.stop(STOP_MODE.ALLOWFADEOUT);
			this.ambientLoop.release();
			this.ambientLoop.clearHandle();
		}
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x0020CDF3 File Offset: 0x0020AFF3
	public void StopMainMenuMusic()
	{
		if (MusicManager.instance.SongIsPlaying(this.menuMusicEventName))
		{
			MusicManager.instance.StopSong(this.menuMusicEventName, true, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060059B3 RID: 22963 RVA: 0x0020CE30 File Offset: 0x0020B030
	private void CheckForAudioDriverIssue()
	{
		if (!KFMOD.didFmodInitializeSuccessfully)
		{
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS, null, null, UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS_MORE_INFO, delegate
			{
				App.OpenWebURL("http://support.kleientertainment.com/customer/en/portal/articles/2947881-no-audio-when-playing-oxygen-not-included");
			}, null, null, null, GlobalResources.Instance().sadDupeAudio);
		}
	}

	// Token: 0x060059B4 RID: 22964 RVA: 0x0020CEA8 File Offset: 0x0020B0A8
	private void CheckPlayerPrefsCorruption()
	{
		if (KPlayerPrefs.HasCorruptedFlag())
		{
			KPlayerPrefs.ResetCorruptedFlag();
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.PLAYER_PREFS_CORRUPTED, null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	// Token: 0x060059B5 RID: 22965 RVA: 0x0020CEFC File Offset: 0x0020B0FC
	private void CheckDoubleBoundKeys()
	{
		string text = "";
		HashSet<BindingEntry> hashSet = new HashSet<BindingEntry>();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			if (GameInputMapping.KeyBindings[i].mKeyCode != KKeyCode.Mouse1)
			{
				for (int j = 0; j < GameInputMapping.KeyBindings.Length; j++)
				{
					if (i != j)
					{
						BindingEntry bindingEntry = GameInputMapping.KeyBindings[j];
						if (!hashSet.Contains(bindingEntry))
						{
							BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
							if (bindingEntry2.mKeyCode != KKeyCode.None && bindingEntry2.mKeyCode == bindingEntry.mKeyCode && bindingEntry2.mModifier == bindingEntry.mModifier && bindingEntry2.mRebindable && bindingEntry.mRebindable)
							{
								string mGroup = GameInputMapping.KeyBindings[i].mGroup;
								string mGroup2 = GameInputMapping.KeyBindings[j].mGroup;
								if ((mGroup == "Root" || mGroup2 == "Root" || mGroup == mGroup2) && (!(mGroup == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(mGroup2 == "Root") || !bindingEntry2.mIgnoreRootConflics))
								{
									text = string.Concat(new string[]
									{
										text,
										"\n\n",
										bindingEntry2.mAction.ToString(),
										": <b>",
										bindingEntry2.mKeyCode.ToString(),
										"</b>\n",
										bindingEntry.mAction.ToString(),
										": <b>",
										bindingEntry.mKeyCode.ToString(),
										"</b>"
									});
									BindingEntry bindingEntry3 = bindingEntry2;
									bindingEntry3.mKeyCode = KKeyCode.None;
									bindingEntry3.mModifier = Modifier.None;
									GameInputMapping.KeyBindings[i] = bindingEntry3;
									bindingEntry3 = bindingEntry;
									bindingEntry3.mKeyCode = KKeyCode.None;
									bindingEntry3.mModifier = Modifier.None;
									GameInputMapping.KeyBindings[j] = bindingEntry3;
								}
							}
						}
					}
				}
				hashSet.Add(GameInputMapping.KeyBindings[i]);
			}
		}
		if (text != "")
		{
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(string.Format(UI.FRONTEND.SUPPORTWARNINGS.DUPLICATE_KEY_BINDINGS, text), null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	// Token: 0x060059B6 RID: 22966 RVA: 0x0020D189 File Offset: 0x0020B389
	private void RestartGame()
	{
		App.instance.Restart();
	}

	// Token: 0x04003CA0 RID: 15520
	private static MainMenu _instance;

	// Token: 0x04003CA1 RID: 15521
	public KButton Button_ResumeGame;

	// Token: 0x04003CA2 RID: 15522
	private KButton Button_NewGame;

	// Token: 0x04003CA3 RID: 15523
	public GameObject topLeftAlphaMessage;

	// Token: 0x04003CA4 RID: 15524
	private MotdServerClient m_motdServerClient;

	// Token: 0x04003CA5 RID: 15525
	private GameObject GameSettingsScreen;

	// Token: 0x04003CA6 RID: 15526
	private bool m_screenshotMode;

	// Token: 0x04003CA7 RID: 15527
	[SerializeField]
	private CanvasGroup uiCanvas;

	// Token: 0x04003CA8 RID: 15528
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04003CA9 RID: 15529
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x04003CAA RID: 15530
	[SerializeField]
	private ColorStyleSetting topButtonStyle;

	// Token: 0x04003CAB RID: 15531
	[SerializeField]
	private ColorStyleSetting normalButtonStyle;

	// Token: 0x04003CAC RID: 15532
	[SerializeField]
	private string menuMusicEventName;

	// Token: 0x04003CAD RID: 15533
	[SerializeField]
	private string ambientLoopEventName;

	// Token: 0x04003CAE RID: 15534
	private EventInstance ambientLoop;

	// Token: 0x04003CAF RID: 15535
	[SerializeField]
	private GameObject MOTDContainer;

	// Token: 0x04003CB0 RID: 15536
	[SerializeField]
	private GameObject buttonContainer;

	// Token: 0x04003CB1 RID: 15537
	[SerializeField]
	private LocText motdImageHeader;

	// Token: 0x04003CB2 RID: 15538
	[SerializeField]
	private KButton motdImageButton;

	// Token: 0x04003CB3 RID: 15539
	[SerializeField]
	private Image motdImage;

	// Token: 0x04003CB4 RID: 15540
	[SerializeField]
	private LocText motdNewsHeader;

	// Token: 0x04003CB5 RID: 15541
	[SerializeField]
	private LocText motdNewsBody;

	// Token: 0x04003CB6 RID: 15542
	[SerializeField]
	private PatchNotesScreen patchNotesScreenPrefab;

	// Token: 0x04003CB7 RID: 15543
	[SerializeField]
	private NextUpdateTimer nextUpdateTimer;

	// Token: 0x04003CB8 RID: 15544
	[SerializeField]
	private DLCToggle expansion1Toggle;

	// Token: 0x04003CB9 RID: 15545
	[SerializeField]
	private GameObject expansion1Ad;

	// Token: 0x04003CBA RID: 15546
	[SerializeField]
	private BuildWatermark buildWatermark;

	// Token: 0x04003CBB RID: 15547
	[SerializeField]
	public string IntroShortName;

	// Token: 0x04003CBC RID: 15548
	private KButton lockerButton;

	// Token: 0x04003CBD RID: 15549
	private bool itemDropOpenFlag;

	// Token: 0x04003CBE RID: 15550
	private static bool HasAutoresumedOnce = false;

	// Token: 0x04003CBF RID: 15551
	private bool refreshResumeButton = true;

	// Token: 0x04003CC0 RID: 15552
	private int m_cheatInputCounter;

	// Token: 0x04003CC1 RID: 15553
	public const string AutoResumeSaveFileKey = "AutoResumeSaveFile";

	// Token: 0x04003CC2 RID: 15554
	public const string PLAY_SHORT_ON_LAUNCH = "PlayShortOnLaunch";

	// Token: 0x04003CC3 RID: 15555
	private static int LANGUAGE_CONFIRMATION_VERSION = 2;

	// Token: 0x04003CC4 RID: 15556
	private Dictionary<string, MainMenu.SaveFileEntry> saveFileEntries = new Dictionary<string, MainMenu.SaveFileEntry>();

	// Token: 0x02001A66 RID: 6758
	private struct ButtonInfo
	{
		// Token: 0x060096FD RID: 38653 RVA: 0x0033E4F6 File Offset: 0x0033C6F6
		public ButtonInfo(LocString text, System.Action action, int font_size, ColorStyleSetting style)
		{
			this.text = text;
			this.action = action;
			this.fontSize = font_size;
			this.style = style;
		}

		// Token: 0x0400796D RID: 31085
		public LocString text;

		// Token: 0x0400796E RID: 31086
		public System.Action action;

		// Token: 0x0400796F RID: 31087
		public int fontSize;

		// Token: 0x04007970 RID: 31088
		public ColorStyleSetting style;
	}

	// Token: 0x02001A67 RID: 6759
	private struct SaveFileEntry
	{
		// Token: 0x04007971 RID: 31089
		public System.DateTime timeStamp;

		// Token: 0x04007972 RID: 31090
		public SaveGame.Header header;

		// Token: 0x04007973 RID: 31091
		public SaveGame.GameInfo headerData;
	}
}
