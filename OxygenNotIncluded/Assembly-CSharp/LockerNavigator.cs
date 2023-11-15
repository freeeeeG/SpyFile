using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B51 RID: 2897
public class LockerNavigator : KModalScreen
{
	// Token: 0x17000676 RID: 1654
	// (get) Token: 0x06005979 RID: 22905 RVA: 0x0020B922 File Offset: 0x00209B22
	public GameObject ContentSlot
	{
		get
		{
			return this.slot.gameObject;
		}
	}

	// Token: 0x0600597A RID: 22906 RVA: 0x0020B92F File Offset: 0x00209B2F
	protected override void OnActivate()
	{
		LockerNavigator.Instance = this;
		this.Show(false);
		this.backButton.onClick += this.OnClickBack;
	}

	// Token: 0x0600597B RID: 22907 RVA: 0x0020B955 File Offset: 0x00209B55
	public override float GetSortKey()
	{
		return 41f;
	}

	// Token: 0x0600597C RID: 22908 RVA: 0x0020B95C File Offset: 0x00209B5C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.PopScreen();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600597D RID: 22909 RVA: 0x0020B97E File Offset: 0x00209B7E
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show)
		{
			this.PopAllScreens();
		}
		StreamedTextures.SetBundlesLoaded(show);
	}

	// Token: 0x0600597E RID: 22910 RVA: 0x0020B996 File Offset: 0x00209B96
	private void OnClickBack()
	{
		this.PopScreen();
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x0020B9A0 File Offset: 0x00209BA0
	public void PushScreen(GameObject screen, System.Action onClose = null)
	{
		if (screen == null)
		{
			return;
		}
		if (this.navigationHistory.Count == 0)
		{
			this.Show(true);
			if (!LockerNavigator.didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
			{
				LockerNavigator.MakeDataCollectionWarningPopup(base.gameObject.transform.parent.gameObject);
				LockerNavigator.didDisplayDataCollectionWarningPopupOnce = true;
			}
		}
		if (this.navigationHistory.Count > 0 && screen == this.navigationHistory[this.navigationHistory.Count - 1].screen)
		{
			return;
		}
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(false);
		}
		this.navigationHistory.Add(new LockerNavigator.HistoryEntry(screen, onClose));
		this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.RefreshButtons();
	}

	// Token: 0x06005980 RID: 22912 RVA: 0x0020BAB8 File Offset: 0x00209CB8
	public bool PopScreen()
	{
		while (this.preventScreenPop.Count > 0)
		{
			int index = this.preventScreenPop.Count - 1;
			Func<bool> func = this.preventScreenPop[index];
			this.preventScreenPop.RemoveAt(index);
			if (func())
			{
				return true;
			}
		}
		int index2 = this.navigationHistory.Count - 1;
		LockerNavigator.HistoryEntry historyEntry = this.navigationHistory[index2];
		historyEntry.screen.SetActive(false);
		if (historyEntry.onClose.IsSome())
		{
			historyEntry.onClose.Unwrap()();
		}
		this.navigationHistory.RemoveAt(index2);
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
			this.RefreshButtons();
			return true;
		}
		this.Show(false);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial", true);
		return false;
	}

	// Token: 0x06005981 RID: 22913 RVA: 0x0020BBB4 File Offset: 0x00209DB4
	public void PopAllScreens()
	{
		if (this.navigationHistory.Count == 0 && this.preventScreenPop.Count == 0)
		{
			return;
		}
		int num = 0;
		while (this.PopScreen())
		{
			if (num > 100)
			{
				DebugUtil.DevAssert(false, string.Format("Can't close all LockerNavigator screens, hit limit of trying to close {0} screens", 100), null);
				return;
			}
			num++;
		}
	}

	// Token: 0x06005982 RID: 22914 RVA: 0x0020BC0A File Offset: 0x00209E0A
	private void RefreshButtons()
	{
		this.backButton.isInteractable = true;
	}

	// Token: 0x06005983 RID: 22915 RVA: 0x0020BC18 File Offset: 0x00209E18
	public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
	{
		InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.ContentSlot, false);
		configureDialogFn(dialog);
		dialog.Activate();
		dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
		dialog.gameObject.AddOrGet<RectTransform>().Fill();
		Func<bool> preventScreenPopFn = delegate()
		{
			dialog.Deactivate();
			return true;
		};
		this.preventScreenPop.Add(preventScreenPopFn);
		InfoDialogScreen dialog2 = dialog;
		dialog2.onDeactivateFn = (System.Action)Delegate.Combine(dialog2.onDeactivateFn, new System.Action(delegate()
		{
			this.preventScreenPop.Remove(preventScreenPopFn);
		}));
	}

	// Token: 0x06005984 RID: 22916 RVA: 0x0020BCE0 File Offset: 0x00209EE0
	public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
	{
		Action<InfoDialogScreen> <>9__2;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, true);
			string text = UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS;
			Action<InfoDialogScreen> action;
			if ((action = <>9__2) == null)
			{
				action = (<>9__2 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					LockerNavigator.Instance.PopAllScreens();
					LockerMenuScreen.Instance.Show(false);
					Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, true).ShowMetricsScreen();
				});
			}
			infoDialogScreen.AddOption(text, action, false);
		});
	}

	// Token: 0x04003C80 RID: 15488
	public static LockerNavigator Instance;

	// Token: 0x04003C81 RID: 15489
	[SerializeField]
	private RectTransform slot;

	// Token: 0x04003C82 RID: 15490
	[SerializeField]
	private KButton backButton;

	// Token: 0x04003C83 RID: 15491
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003C84 RID: 15492
	[SerializeField]
	public GameObject kleiInventoryScreen;

	// Token: 0x04003C85 RID: 15493
	[SerializeField]
	public GameObject duplicantCatalogueScreen;

	// Token: 0x04003C86 RID: 15494
	[SerializeField]
	public GameObject outfitDesignerScreen;

	// Token: 0x04003C87 RID: 15495
	[SerializeField]
	public GameObject outfitBrowserScreen;

	// Token: 0x04003C88 RID: 15496
	[SerializeField]
	public GameObject joyResponseDesignerScreen;

	// Token: 0x04003C89 RID: 15497
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04003C8A RID: 15498
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04003C8B RID: 15499
	private List<LockerNavigator.HistoryEntry> navigationHistory = new List<LockerNavigator.HistoryEntry>();

	// Token: 0x04003C8C RID: 15500
	private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

	// Token: 0x04003C8D RID: 15501
	private static bool didDisplayDataCollectionWarningPopupOnce;

	// Token: 0x04003C8E RID: 15502
	public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

	// Token: 0x02001A60 RID: 6752
	public readonly struct HistoryEntry
	{
		// Token: 0x060096E9 RID: 38633 RVA: 0x0033E1FE File Offset: 0x0033C3FE
		public HistoryEntry(GameObject screen, System.Action onClose = null)
		{
			this.screen = screen;
			this.onClose = onClose;
		}

		// Token: 0x0400795D RID: 31069
		public readonly GameObject screen;

		// Token: 0x0400795E RID: 31070
		public readonly Option<System.Action> onClose;
	}
}
