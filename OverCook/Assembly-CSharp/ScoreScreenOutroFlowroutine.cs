using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000686 RID: 1670
public class ScoreScreenOutroFlowroutine : OutroFlowroutineBase
{
	// Token: 0x17000282 RID: 642
	// (set) Token: 0x0600200A RID: 8202 RVA: 0x0009BB06 File Offset: 0x00099F06
	public GenericVoid OnRestartRequest
	{
		set
		{
			this.m_onRestartRequest = value;
		}
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x0009BB10 File Offset: 0x00099F10
	protected override void Setup(FlowroutineData _setupData)
	{
		this.m_flowroutineData = (ScoreScreenFlowroutineData)_setupData;
		this.m_pauseManager = GameUtils.RequireManager<PauseMenuManager>();
		this.m_campaignAudioManager = GameUtils.RequireManager<CampaignAudioManager>();
		this.m_selectButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
		this.m_restartButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIRestartLevel, PlayerInputLookup.Player.One);
		GameObject obj = GameUtils.InstantiateUIController(this.m_flowroutineData.m_starRatingUIController.gameObject, "UICanvas");
		this.m_starRatingController = obj.RequireComponent<StarRatingUIController>();
		this.m_starRatingController.gameObject.SetActive(false);
		GameObject obj2 = GameUtils.InstantiateUIController(this.m_flowroutineData.m_awardAvatarUIController.gameObject, "UICanvas");
		this.m_awardAvatarController = obj2.RequireComponent<AwardAvatarUIController>();
		this.m_awardAvatarController.gameObject.SetActive(false);
		if (this.m_flowroutineData.TimesUpUIPrefab != null)
		{
			this.m_timesUpUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_flowroutineData.TimesUpUIPrefab);
			this.m_timesUpUIInstance.SetActive(false);
		}
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x0009BC04 File Offset: 0x0009A004
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
			IEnumerator timeUpDelay = CoroutineUtils.TimerRoutine(this.m_flowroutineData.TimesUpUILifetime, layer);
			while (timeUpDelay.MoveNext())
			{
				yield return null;
			}
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.LevelEnd, layer);
		this.m_starRatingController.gameObject.SetActive(true);
		this.m_starRatingController.SetScoreData(this.m_flowroutineData.m_scoreData);
		IEnumerator minTimeDelay = CoroutineUtils.TimerRoutine(1f, layer);
		while (minTimeDelay.MoveNext())
		{
			yield return null;
		}
		while (!this.m_starRatingController.HasAnimationSettled())
		{
			yield return null;
		}
		this.m_starRatingController.SetButtonActive();
		IEnumerator timeoutRoutine = CoroutineUtils.TimerRoutine(this.m_flowroutineData.m_fTimeout, layer);
		while (timeoutRoutine.MoveNext())
		{
			if (this.m_starRatingController.AllowedToSkip())
			{
				bool flag = this.m_starRatingController.AllowedToRestart() && this.m_restartButton.JustPressed();
				bool flag2 = flag || this.m_selectButton.JustPressed();
				if (flag && this.m_onRestartRequest != null)
				{
					this.m_restartButton.ClaimPressEvent();
					this.m_restartButton.ClaimReleaseEvent();
					this.m_onRestartRequest();
				}
				if (flag2)
				{
					break;
				}
			}
			yield return null;
		}
		if (ConnectionStatus.IsInSession())
		{
			CoopStarRatingUIController coopStarRatingUIController = (CoopStarRatingUIController)this.m_starRatingController;
			if (coopStarRatingUIController != null)
			{
				coopStarRatingUIController.m_waitingForPlayers.SetActive(true);
			}
		}
		if (this.m_flowroutineData.m_unlocks != null && this.m_flowroutineData.m_unlocks.Length > 0)
		{
			this.m_starRatingController.gameObject.SetActive(false);
			this.m_awardAvatarController.gameObject.SetActive(true);
			this.m_awardAvatarController.EnableButton();
			GameSession gameSession = GameUtils.GetGameSession();
			for (int i = 0; i < this.m_flowroutineData.m_unlocks.Length; i++)
			{
				GameProgress.UnlockData unlock = this.m_flowroutineData.m_unlocks[i];
				GameProgress.UnlockData.UnlockType type = unlock.Type;
				if (type == GameProgress.UnlockData.UnlockType.Avatar)
				{
					AvatarDirectoryData avatarDirectory = gameSession.Progress.GetAvatarDirectory();
					this.m_awardAvatarController.SetData(unlock.AvatarData.GetAvatarData(avatarDirectory));
					timeoutRoutine = CoroutineUtils.TimerRoutine(this.m_awardAvatarController.m_awardTimeout, layer);
					while (timeoutRoutine.MoveNext())
					{
						if (this.m_selectButton.JustPressed() || this.m_restartButton.JustPressed())
						{
							break;
						}
						yield return null;
					}
				}
			}
			if (ConnectionStatus.IsInSession())
			{
				this.m_awardAvatarController.m_WaitingForPlayersText.gameObject.SetActive(true);
			}
		}
		yield break;
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x0009BC20 File Offset: 0x0009A020
	private void OnLevelLoad(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.SetInRoundResults(false);
		}
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x0009BC76 File Offset: 0x0009A076
	protected override void Shutdown()
	{
	}

	// Token: 0x04001863 RID: 6243
	private const float c_minScreenDuration = 1f;

	// Token: 0x04001864 RID: 6244
	private CampaignAudioManager m_campaignAudioManager;

	// Token: 0x04001865 RID: 6245
	private PauseMenuManager m_pauseManager;

	// Token: 0x04001866 RID: 6246
	private ILogicalButton m_selectButton;

	// Token: 0x04001867 RID: 6247
	private ILogicalButton m_restartButton;

	// Token: 0x04001868 RID: 6248
	private GameObject m_timesUpUIInstance;

	// Token: 0x04001869 RID: 6249
	private StarRatingUIController m_starRatingController;

	// Token: 0x0400186A RID: 6250
	private AwardAvatarUIController m_awardAvatarController;

	// Token: 0x0400186B RID: 6251
	private ScoreScreenFlowroutineData m_flowroutineData;

	// Token: 0x0400186C RID: 6252
	private GenericVoid m_onRestartRequest;
}
