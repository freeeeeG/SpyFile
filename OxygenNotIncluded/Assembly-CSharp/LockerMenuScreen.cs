using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B50 RID: 2896
public class LockerMenuScreen : KModalScreen
{
	// Token: 0x0600596C RID: 22892 RVA: 0x0020B4D3 File Offset: 0x002096D3
	protected override void OnActivate()
	{
		LockerMenuScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x0600596D RID: 22893 RVA: 0x0020B4E2 File Offset: 0x002096E2
	public override float GetSortKey()
	{
		return 40f;
	}

	// Token: 0x0600596E RID: 22894 RVA: 0x0020B4EC File Offset: 0x002096EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.buttonInventory;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "inventory", true);
		}));
		MultiToggle multiToggle2 = this.buttonDuplicants;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			MinionBrowserScreenConfig.Personalities(default(Option<Personality>)).ApplyAndOpenScreen(null);
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "dupe", true);
		}));
		MultiToggle multiToggle3 = this.buttonOutfitBroswer;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			OutfitBrowserScreenConfig.Mannequin().ApplyAndOpenScreen();
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "wardrobe", true);
		}));
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.ConfigureHoverForButton(this.buttonInventory, UI.LOCKER_MENU.BUTTON_INVENTORY_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonDuplicants, UI.LOCKER_MENU.BUTTON_DUPLICANTS_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonOutfitBroswer, UI.LOCKER_MENU.BUTTON_OUTFITS_DESCRIPTION, true);
		this.descriptionArea.text = UI.LOCKER_MENU.DEFAULT_DESCRIPTION;
	}

	// Token: 0x0600596F RID: 22895 RVA: 0x0020B620 File Offset: 0x00209820
	private void ConfigureHoverForButton(MultiToggle toggle, string desc, bool useHoverColor = true)
	{
		LockerMenuScreen.<>c__DisplayClass16_0 CS$<>8__locals1 = new LockerMenuScreen.<>c__DisplayClass16_0();
		CS$<>8__locals1.useHoverColor = useHoverColor;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.defaultColor = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		CS$<>8__locals1.hoverColor = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		toggle.onEnter = null;
		toggle.onExit = null;
		toggle.onEnter = (System.Action)Delegate.Combine(toggle.onEnter, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverEnterFn|0(toggle, desc));
		toggle.onExit = (System.Action)Delegate.Combine(toggle.onExit, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverExitFn|1(toggle));
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x0020B6C8 File Offset: 0x002098C8
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(true, 0.5f);
			MusicManager.instance.PlaySong("Music_SupplyCloset", false);
			ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(new KleiAccount.GetUserIDdelegate(this.TriggerShouldRefreshClaimItems), false);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		this.RefreshClaimItemsButton();
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x0020B77B File Offset: 0x0020997B
	private void TriggerShouldRefreshClaimItems()
	{
		this.refreshRequested = true;
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x0020B784 File Offset: 0x00209984
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06005973 RID: 22899 RVA: 0x0020B78C File Offset: 0x0020998C
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x06005974 RID: 22900 RVA: 0x0020B794 File Offset: 0x00209994
	private void RefreshClaimItemsButton()
	{
		this.noConnectionIcon.SetActive(!ThreadedHttps<KleiAccount>.Instance.HasValidTicket());
		this.refreshRequested = false;
		bool hasClaimable = PermitItems.HasUnopenedItem();
		this.dropsAvailableNotification.SetActive(hasClaimable);
		this.buttonClaimItems.ChangeState(hasClaimable ? 0 : 1);
		this.buttonClaimItems.GetComponent<HierarchyReferences>().GetReference<Image>("FGIcon").material = (hasClaimable ? null : this.desatUIMaterial);
		this.buttonClaimItems.onClick = null;
		MultiToggle multiToggle = this.buttonClaimItems;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (!hasClaimable)
			{
				return;
			}
			UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
			this.Show(false);
		}));
		this.ConfigureHoverForButton(this.buttonClaimItems, hasClaimable ? UI.LOCKER_MENU.BUTTON_CLAIM_DESCRIPTION : UI.LOCKER_MENU.BUTTON_CLAIM_NONE_DESCRIPTION, hasClaimable);
	}

	// Token: 0x06005975 RID: 22901 RVA: 0x0020B88C File Offset: 0x00209A8C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06005976 RID: 22902 RVA: 0x0020B901 File Offset: 0x00209B01
	private void Update()
	{
		if (this.refreshRequested)
		{
			this.RefreshClaimItemsButton();
		}
	}

	// Token: 0x04003C73 RID: 15475
	public static LockerMenuScreen Instance;

	// Token: 0x04003C74 RID: 15476
	[SerializeField]
	private MultiToggle buttonInventory;

	// Token: 0x04003C75 RID: 15477
	[SerializeField]
	private MultiToggle buttonDuplicants;

	// Token: 0x04003C76 RID: 15478
	[SerializeField]
	private MultiToggle buttonOutfitBroswer;

	// Token: 0x04003C77 RID: 15479
	[SerializeField]
	private MultiToggle buttonClaimItems;

	// Token: 0x04003C78 RID: 15480
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04003C79 RID: 15481
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003C7A RID: 15482
	[SerializeField]
	private GameObject dropsAvailableNotification;

	// Token: 0x04003C7B RID: 15483
	[SerializeField]
	private GameObject noConnectionIcon;

	// Token: 0x04003C7C RID: 15484
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04003C7D RID: 15485
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04003C7E RID: 15486
	[SerializeField]
	private Material desatUIMaterial;

	// Token: 0x04003C7F RID: 15487
	private bool refreshRequested;
}
