using System;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000BBC RID: 3004
public class PauseScreen : KModalButtonMenu
{
	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06005DF0 RID: 24048 RVA: 0x00226575 File Offset: 0x00224775
	public static PauseScreen Instance
	{
		get
		{
			return PauseScreen.instance;
		}
	}

	// Token: 0x06005DF1 RID: 24049 RVA: 0x0022657C File Offset: 0x0022477C
	public static void DestroyInstance()
	{
		PauseScreen.instance = null;
	}

	// Token: 0x06005DF2 RID: 24050 RVA: 0x00226584 File Offset: 0x00224784
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		if (!GenericGameSettings.instance.demoMode)
		{
			this.buttons = new KButtonMenu.ButtonInfo[]
			{
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVE, global::Action.NumActions, new UnityAction(this.OnSave), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVEAS, global::Action.NumActions, new UnityAction(this.OnSaveAs), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOAD, global::Action.NumActions, new UnityAction(this.OnLoad), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, global::Action.NumActions, new UnityAction(this.OnColonySummary), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOCKERMENU, global::Action.NumActions, new UnityAction(this.OnLockerMenu), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
			};
		}
		else
		{
			this.buttons = new KButtonMenu.ButtonInfo[]
			{
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
			};
		}
		this.closeButton.onClick += this.OnResume;
		PauseScreen.instance = this;
		this.Show(false);
	}

	// Token: 0x06005DF3 RID: 24051 RVA: 0x002267D0 File Offset: 0x002249D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.clipboard.GetText = new Func<string>(this.GetClipboardText);
		this.title.SetText(UI.FRONTEND.PAUSE_SCREEN.TITLE);
		try
		{
			string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
			string[] array = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, settingsCoordinate));
			this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, new object[]
			{
				array[1],
				array[2],
				array[3],
				array[4]
			});
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("Failed to load Coordinates on ClusterLayout {0}, please report this error on the forums", arg));
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, "0"));
		}
	}

	// Token: 0x06005DF4 RID: 24052 RVA: 0x002268F0 File Offset: 0x00224AF0
	public override float GetSortKey()
	{
		return 30f;
	}

	// Token: 0x06005DF5 RID: 24053 RVA: 0x002268F8 File Offset: 0x00224AF8
	private string GetClipboardText()
	{
		string result;
		try
		{
			result = CustomGameSettings.Instance.GetSettingsCoordinate();
		}
		catch
		{
			result = "";
		}
		return result;
	}

	// Token: 0x06005DF6 RID: 24054 RVA: 0x0022692C File Offset: 0x00224B2C
	private void OnResume()
	{
		this.Show(false);
	}

	// Token: 0x06005DF7 RID: 24055 RVA: 0x00226938 File Offset: 0x00224B38
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
			MusicManager.instance.OnEscapeMenu(true);
			MusicManager.instance.PlaySong("Music_ESC_Menu", false);
			return;
		}
		ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ESCPauseSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.OnEscapeMenu(false);
		if (MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.instance.StopSong("Music_ESC_Menu", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06005DF8 RID: 24056 RVA: 0x002269D8 File Offset: 0x00224BD8
	private void OnOptions()
	{
		base.ActivateChildScreen(this.optionsScreen.gameObject);
	}

	// Token: 0x06005DF9 RID: 24057 RVA: 0x002269EC File Offset: 0x00224BEC
	private void OnSaveAs()
	{
		base.ActivateChildScreen(this.saveScreenPrefab.gameObject);
	}

	// Token: 0x06005DFA RID: 24058 RVA: 0x00226A00 File Offset: 0x00224C00
	private void OnSave()
	{
		string filename = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
		{
			base.gameObject.SetActive(false);
			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, System.IO.Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
				this.gameObject.SetActive(true);
			}, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
			return;
		}
		this.OnSaveAs();
	}

	// Token: 0x06005DFB RID: 24059 RVA: 0x00226AC4 File Offset: 0x00224CC4
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, null, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x06005DFC RID: 24060 RVA: 0x00226B74 File Offset: 0x00224D74
	private void ConfirmDecision(string text, System.Action onConfirm)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(text, onConfirm, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
	}

	// Token: 0x06005DFD RID: 24061 RVA: 0x00226BD5 File Offset: 0x00224DD5
	private void OnLoad()
	{
		base.ActivateChildScreen(this.loadScreenPrefab.gameObject);
	}

	// Token: 0x06005DFE RID: 24062 RVA: 0x00226BEC File Offset: 0x00224DEC
	private void OnColonySummary()
	{
		RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
	}

	// Token: 0x06005DFF RID: 24063 RVA: 0x00226C19 File Offset: 0x00224E19
	private void OnLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x06005E00 RID: 24064 RVA: 0x00226C26 File Offset: 0x00224E26
	private void OnQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, new System.Action(this.OnQuitConfirm));
	}

	// Token: 0x06005E01 RID: 24065 RVA: 0x00226C44 File Offset: 0x00224E44
	private void OnDesktopQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, new System.Action(this.OnDesktopQuitConfirm));
	}

	// Token: 0x06005E02 RID: 24066 RVA: 0x00226C62 File Offset: 0x00224E62
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06005E03 RID: 24067 RVA: 0x00226C70 File Offset: 0x00224E70
	private void OnLoadConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			LoadScreen.ForceStopGame();
			this.Deactivate();
			App.LoadScene("frontend");
		});
	}

	// Token: 0x06005E04 RID: 24068 RVA: 0x00226C83 File Offset: 0x00224E83
	private void OnRetireConfirm()
	{
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x06005E05 RID: 24069 RVA: 0x00226C8B File Offset: 0x00224E8B
	private void OnQuitConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x06005E06 RID: 24070 RVA: 0x00226C9E File Offset: 0x00224E9E
	private void OnDesktopQuitConfirm()
	{
		App.Quit();
	}

	// Token: 0x06005E07 RID: 24071 RVA: 0x00226CA5 File Offset: 0x00224EA5
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005E08 RID: 24072 RVA: 0x00226CC8 File Offset: 0x00224EC8
	public static void TriggerQuitGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndGame();
		LoadScreen.ForceStopGame();
		App.LoadScene("frontend");
	}

	// Token: 0x04003F4E RID: 16206
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x04003F4F RID: 16207
	[SerializeField]
	private SaveScreen saveScreenPrefab;

	// Token: 0x04003F50 RID: 16208
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x04003F51 RID: 16209
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003F52 RID: 16210
	[SerializeField]
	private LocText title;

	// Token: 0x04003F53 RID: 16211
	[SerializeField]
	private LocText worldSeed;

	// Token: 0x04003F54 RID: 16212
	[SerializeField]
	private CopyTextFieldToClipboard clipboard;

	// Token: 0x04003F55 RID: 16213
	private float originalTimeScale;

	// Token: 0x04003F56 RID: 16214
	private static PauseScreen instance;
}
