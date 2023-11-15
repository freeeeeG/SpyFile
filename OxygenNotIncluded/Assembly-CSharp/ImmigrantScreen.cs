using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000A68 RID: 2664
public class ImmigrantScreen : CharacterSelectionController
{
	// Token: 0x06005086 RID: 20614 RVA: 0x001C8994 File Offset: 0x001C6B94
	public static void DestroyInstance()
	{
		ImmigrantScreen.instance = null;
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x06005087 RID: 20615 RVA: 0x001C899C File Offset: 0x001C6B9C
	public Telepad Telepad
	{
		get
		{
			return this.telepad;
		}
	}

	// Token: 0x06005088 RID: 20616 RVA: 0x001C89A4 File Offset: 0x001C6BA4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005089 RID: 20617 RVA: 0x001C89AC File Offset: 0x001C6BAC
	protected override void OnSpawn()
	{
		this.activateOnSpawn = false;
		base.ConsumeMouseScroll = false;
		base.OnSpawn();
		base.IsStarterMinion = false;
		this.rejectButton.onClick += this.OnRejectAll;
		this.confirmRejectionBtn.onClick += this.OnRejectionConfirmed;
		this.cancelRejectionBtn.onClick += this.OnRejectionCancelled;
		ImmigrantScreen.instance = this;
		this.title.text = UI.IMMIGRANTSCREEN.IMMIGRANTSCREENTITLE;
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.PROCEEDBUTTON;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.Show(false);
	}

	// Token: 0x0600508A RID: 20618 RVA: 0x001C8A6C File Offset: 0x001C6C6C
	protected override void OnShow(bool show)
	{
		if (show)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("Dialog_Popup", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
			MusicManager.instance.PlaySong("Music_SelectDuplicant", false);
			this.hasShown = true;
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.StopSong("Music_SelectDuplicant", true, STOP_MODE.ALLOWFADEOUT);
			}
			if (Immigration.Instance.ImmigrantsAvailable && this.hasShown)
			{
				AudioMixer.instance.Start(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
			}
		}
		base.OnShow(show);
	}

	// Token: 0x0600508B RID: 20619 RVA: 0x001C8B22 File Offset: 0x001C6D22
	public void DebugShuffleOptions()
	{
		this.OnRejectionConfirmed();
		Immigration.Instance.timeBeforeSpawn = 0f;
	}

	// Token: 0x0600508C RID: 20620 RVA: 0x001C8B39 File Offset: 0x001C6D39
	public override void OnPressBack()
	{
		if (this.rejectConfirmationScreen.activeSelf)
		{
			this.OnRejectionCancelled();
			return;
		}
		base.OnPressBack();
	}

	// Token: 0x0600508D RID: 20621 RVA: 0x001C8B55 File Offset: 0x001C6D55
	public override void Deactivate()
	{
		this.Show(false);
	}

	// Token: 0x0600508E RID: 20622 RVA: 0x001C8B5E File Offset: 0x001C6D5E
	public static void InitializeImmigrantScreen(Telepad telepad)
	{
		ImmigrantScreen.instance.Initialize(telepad);
		ImmigrantScreen.instance.Show(true);
	}

	// Token: 0x0600508F RID: 20623 RVA: 0x001C8B78 File Offset: 0x001C6D78
	private void Initialize(Telepad telepad)
	{
		this.InitializeContainers();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.SetReshufflingState(false);
			}
		}
		this.telepad = telepad;
	}

	// Token: 0x06005090 RID: 20624 RVA: 0x001C8BE8 File Offset: 0x001C6DE8
	protected override void OnProceed()
	{
		this.telepad.OnAcceptDelivery(this.selectedDeliverables[0]);
		this.Show(false);
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.PlaySong("Stinger_NewDuplicant", false);
	}

	// Token: 0x06005091 RID: 20625 RVA: 0x001C8C84 File Offset: 0x001C6E84
	private void OnRejectAll()
	{
		this.rejectConfirmationScreen.transform.SetAsLastSibling();
		this.rejectConfirmationScreen.SetActive(true);
	}

	// Token: 0x06005092 RID: 20626 RVA: 0x001C8CA2 File Offset: 0x001C6EA2
	private void OnRejectionCancelled()
	{
		this.rejectConfirmationScreen.SetActive(false);
	}

	// Token: 0x06005093 RID: 20627 RVA: 0x001C8CB0 File Offset: 0x001C6EB0
	private void OnRejectionConfirmed()
	{
		this.telepad.RejectAll();
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		this.rejectConfirmationScreen.SetActive(false);
		this.Show(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x040034C1 RID: 13505
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040034C2 RID: 13506
	[SerializeField]
	private KButton rejectButton;

	// Token: 0x040034C3 RID: 13507
	[SerializeField]
	private LocText title;

	// Token: 0x040034C4 RID: 13508
	[SerializeField]
	private GameObject rejectConfirmationScreen;

	// Token: 0x040034C5 RID: 13509
	[SerializeField]
	private KButton confirmRejectionBtn;

	// Token: 0x040034C6 RID: 13510
	[SerializeField]
	private KButton cancelRejectionBtn;

	// Token: 0x040034C7 RID: 13511
	public static ImmigrantScreen instance;

	// Token: 0x040034C8 RID: 13512
	private Telepad telepad;

	// Token: 0x040034C9 RID: 13513
	private bool hasShown;
}
