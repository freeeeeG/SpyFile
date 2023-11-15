using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public class MinionSelectScreen : CharacterSelectionController
{
	// Token: 0x06005115 RID: 20757 RVA: 0x001CCE74 File Offset: 0x001CB074
	protected override void OnPrefabInit()
	{
		base.IsStarterMinion = true;
		base.OnPrefabInit();
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f, true);
		}
		GameObject parent = GameObject.Find("ScreenSpaceOverlayCanvas");
		GameObject gameObject = global::Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, parent, false);
		gameObject.name = "WattsonMessage";
		gameObject.SetActive(false);
		Game.Instance.Subscribe(-1992507039, new Action<object>(this.OnBaseAlreadyCreated));
		this.backButton.onClick += delegate()
		{
			LoadScreen.ForceStopGame();
			App.LoadScene("frontend");
		};
		this.InitializeContainers();
	}

	// Token: 0x06005116 RID: 20758 RVA: 0x001CCF34 File Offset: 0x001CB134
	public void SetProceedButtonActive(bool state, string tooltip = null)
	{
		if (state)
		{
			base.EnableProceedButton();
		}
		else
		{
			base.DisableProceedButton();
		}
		ToolTip component = this.proceedButton.GetComponent<ToolTip>();
		if (component != null)
		{
			if (tooltip != null)
			{
				component.toolTip = tooltip;
				return;
			}
			component.ClearMultiStringTooltip();
		}
	}

	// Token: 0x06005117 RID: 20759 RVA: 0x001CCF78 File Offset: 0x001CB178
	protected override void OnSpawn()
	{
		this.OnDeliverableAdded();
		base.EnableProceedButton();
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.EMBARK;
		this.containers.ForEach(delegate(ITelepadDeliverableContainer container)
		{
			CharacterContainer characterContainer = container as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.DisableSelectButton();
			}
		});
	}

	// Token: 0x06005118 RID: 20760 RVA: 0x001CCFD8 File Offset: 0x001CB1D8
	protected override void OnProceed()
	{
		global::Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas, false);
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot, STOP_MODE.ALLOWFADEOUT);
		this.selectedDeliverables.Clear();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = (CharacterContainer)telepadDeliverableContainer;
			this.selectedDeliverables.Add(characterContainer.Stats);
		}
		NewBaseScreen.Instance.Init(SaveLoader.Instance.ClusterLayout, this.selectedDeliverables.ToArray());
		if (this.OnProceedEvent != null)
		{
			this.OnProceedEvent();
		}
		Game.Instance.Trigger(-838649377, null);
		BuildWatermark.Instance.gameObject.SetActive(false);
		this.Deactivate();
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x001CD0F8 File Offset: 0x001CB2F8
	private void OnBaseAlreadyCreated(object data)
	{
		Game.Instance.StopFE();
		Game.Instance.StartBE();
		Game.Instance.SetGameStarted();
		this.Deactivate();
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x001CD11E File Offset: 0x001CB31E
	private void ReshuffleAll()
	{
		if (this.OnReshuffleEvent != null)
		{
			this.OnReshuffleEvent(base.IsStarterMinion);
		}
	}

	// Token: 0x0600511B RID: 20763 RVA: 0x001CD13C File Offset: 0x001CB33C
	public override void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
	}

	// Token: 0x0400351D RID: 13597
	[SerializeField]
	private NewBaseScreen newBasePrefab;

	// Token: 0x0400351E RID: 13598
	[SerializeField]
	private WattsonMessage wattsonMessagePrefab;

	// Token: 0x0400351F RID: 13599
	public const string WattsonGameObjName = "WattsonMessage";

	// Token: 0x04003520 RID: 13600
	public KButton backButton;
}
