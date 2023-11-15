using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000687 RID: 1671
public class SurvivalModeOutroFlowroutine : OutroFlowroutineBase
{
	// Token: 0x17000283 RID: 643
	// (set) Token: 0x06002010 RID: 8208 RVA: 0x0009C1F5 File Offset: 0x0009A5F5
	public GenericVoid OnRestartRequest
	{
		set
		{
			this.m_onRestartRequest = value;
		}
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x0009C200 File Offset: 0x0009A600
	protected override void Setup(FlowroutineData flowroutineData)
	{
		this.m_flowroutineData = (SurvivalModeOutroFlowroutineData)flowroutineData;
		this.m_pauseManager = GameUtils.RequireManager<PauseMenuManager>();
		this.m_campaignAudioManager = GameUtils.RequireManager<CampaignAudioManager>();
		this.m_selectButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
		this.m_restartButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIRestartLevel, PlayerInputLookup.Player.One);
		if (this.m_flowroutineData.m_timesUpUIPrefab != null)
		{
			this.m_timesUpUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_flowroutineData.m_timesUpUIPrefab);
			this.m_timesUpUIInstance.SetActive(false);
		}
		GameObject obj = GameUtils.InstantiateUIController(this.m_flowroutineData.m_survivalModeRatingUIController.gameObject, "UICanvas");
		this.m_survivalModeRatingController = obj.RequireComponent<SurvivalModeRatingUIController>();
		this.m_survivalModeRatingController.gameObject.SetActive(false);
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x0009C2BB File Offset: 0x0009A6BB
	protected override void Shutdown()
	{
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x0009C2C0 File Offset: 0x0009A6C0
	protected override IEnumerator Run()
	{
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.SetInRoundResults(true);
			Mailbox.Client.RegisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLevelLoad));
			Mailbox.Client.RegisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		}
		PlayerInputLookup.ResetToDefaultInputConfig();
		this.m_campaignAudioManager.SetAudioState(CampaignAudioManager.AudioState.SummaryScreen);
		this.m_pauseManager.enabled = false;
		TimeManager timeManger = GameUtils.RequireManager<TimeManager>();
		timeManger.SetPaused(TimeManager.PauseLayer.Main, true, this);
		int layer = LayerMask.NameToLayer("Administration");
		if (this.m_timesUpUIInstance != null)
		{
			this.m_timesUpUIInstance.SetActive(true);
			GameUtils.TriggerAudio(GameOneShotAudioTag.TimesUp, layer);
			IEnumerator timeUpDelay = CoroutineUtils.TimerRoutine(this.m_flowroutineData.m_minTimesUpDuration, layer);
			while (timeUpDelay.MoveNext())
			{
				yield return null;
			}
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.LevelEnd, layer);
		this.m_survivalModeRatingController.gameObject.SetActive(true);
		this.m_survivalModeRatingController.SetScoreData(this.m_flowroutineData.m_scoreData);
		IEnumerator minTimeDelay = CoroutineUtils.TimerRoutine(this.m_flowroutineData.m_minRatingDuration, layer);
		while (minTimeDelay.MoveNext())
		{
			yield return null;
		}
		IEnumerator timeoutRoutine = CoroutineUtils.TimerRoutine(this.m_flowroutineData.m_minOutroDuration, layer);
		while (timeoutRoutine.MoveNext())
		{
			if (this.m_survivalModeRatingController.AllowedToSkip())
			{
				if (this.m_selectButton.JustPressed())
				{
					break;
				}
				if (this.m_survivalModeRatingController.AllowedToRestart() && this.m_restartButton.JustPressed())
				{
					this.m_restartButton.ClaimPressEvent();
					this.m_restartButton.ClaimReleaseEvent();
					this.m_onRestartRequest();
					break;
				}
			}
			yield return null;
		}
		if (ConnectionStatus.IsInSession())
		{
			this.m_survivalModeRatingController.m_waitingForPlayers.SetActive(true);
		}
		yield break;
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x0009C2DC File Offset: 0x0009A6DC
	private void OnLevelLoad(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.SetInRoundResults(false);
		}
	}

	// Token: 0x0400186D RID: 6253
	private SurvivalModeOutroFlowroutineData m_flowroutineData;

	// Token: 0x0400186E RID: 6254
	private PauseMenuManager m_pauseManager;

	// Token: 0x0400186F RID: 6255
	private CampaignAudioManager m_campaignAudioManager;

	// Token: 0x04001870 RID: 6256
	private GameObject m_timesUpUIInstance;

	// Token: 0x04001871 RID: 6257
	private SurvivalModeRatingUIController m_survivalModeRatingController;

	// Token: 0x04001872 RID: 6258
	private ILogicalButton m_selectButton;

	// Token: 0x04001873 RID: 6259
	private ILogicalButton m_restartButton;

	// Token: 0x04001874 RID: 6260
	private GenericVoid m_onRestartRequest;
}
