using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200067C RID: 1660
public class CompetitiveScoreScreenFlowroutine : CompetitiveFlowController.OutroFlowroutine
{
	// Token: 0x06001FDC RID: 8156 RVA: 0x0009A69A File Offset: 0x00098A9A
	private void Awake()
	{
		this.m_pauseManager = GameUtils.RequireManager<PauseMenuManager>();
		this.m_campaignAudioManager = GameUtils.RequireManager<CampaignAudioManager>();
		this.m_selectButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
	}

	// Token: 0x06001FDD RID: 8157 RVA: 0x0009A6C0 File Offset: 0x00098AC0
	protected override void Setup(CompetitiveFlowController.OutroData _setupData)
	{
		GameObject obj = GameUtils.InstantiateUIController(this.m_scoreboardPrefab.gameObject, "UICanvas");
		this.m_scoreboardController = obj.RequireComponent<CompetitiveScoreboardUIController>();
		this.m_scoreboardController.gameObject.SetActive(false);
		if (this.m_data.TimesUpUIPrefab != null)
		{
			this.m_timesUpUIInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_data.TimesUpUIPrefab);
			this.m_timesUpUIInstance.transform.SetSiblingIndex(0);
			this.m_timesUpUIInstance.SetActive(false);
		}
		this.m_scoreData = _setupData.ScoreData;
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x0009A758 File Offset: 0x00098B58
	protected override IEnumerator Run()
	{
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.SetInRoundResults(true);
			Mailbox.Client.RegisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLevelLoad));
			Mailbox.Client.RegisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		}
		PlayerInputLookup.ResetToDefaultInputConfig();
		float startTime = Time.realtimeSinceStartup;
		this.m_campaignAudioManager.SetAudioState(CampaignAudioManager.AudioState.SummaryScreen);
		this.m_pauseManager.enabled = false;
		TimeManager timeManger = GameUtils.RequireManager<TimeManager>();
		timeManger.SetPaused(TimeManager.PauseLayer.Main, true, this);
		int layer = LayerMask.NameToLayer("Administration");
		if (this.m_timesUpUIInstance != null)
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.TimesUp, layer);
			this.m_timesUpUIInstance.SetActive(true);
			IEnumerator timeUpDelay = CoroutineUtils.TimerRoutine(this.m_data.TimesUpUILifetime, layer);
			while (timeUpDelay.MoveNext())
			{
				yield return null;
			}
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.LevelEnd, layer);
		this.m_scoreboardController.gameObject.SetActive(true);
		this.m_scoreboardController.SetScoreData(this.m_scoreData);
		while (!this.m_scoreboardController.IsButtonActive())
		{
			yield return null;
		}
		IEnumerator timeoutRoutine = CoroutineUtils.TimerRoutine(this.m_fTimeout, layer);
		while ((!this.m_scoreboardController.AllowedToSkip() || !this.m_selectButton.JustPressed()) && timeoutRoutine.MoveNext())
		{
			yield return null;
		}
		this.m_scoreboardController.m_waitingForPlayers.SetActive(true);
		yield break;
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x0009A774 File Offset: 0x00098B74
	private void OnLevelLoad(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLevelLoad));
		if (T17InGameFlow.Instance != null)
		{
			T17InGameFlow.Instance.SetInRoundResults(false);
		}
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x0009A7CA File Offset: 0x00098BCA
	protected override void Shutdown()
	{
	}

	// Token: 0x04001839 RID: 6201
	[SerializeField]
	private LevelOutroFlowroutineData m_data;

	// Token: 0x0400183A RID: 6202
	[SerializeField]
	private CompetitiveScoreboardUIController m_scoreboardPrefab;

	// Token: 0x0400183B RID: 6203
	[SerializeField]
	private float m_fTimeout = 20f;

	// Token: 0x0400183C RID: 6204
	private CampaignAudioManager m_campaignAudioManager;

	// Token: 0x0400183D RID: 6205
	private PauseMenuManager m_pauseManager;

	// Token: 0x0400183E RID: 6206
	private ILogicalButton m_selectButton;

	// Token: 0x0400183F RID: 6207
	private GameObject m_timesUpUIInstance;

	// Token: 0x04001840 RID: 6208
	private CompetitiveScoreboardUIController m_scoreboardController;

	// Token: 0x04001841 RID: 6209
	private object m_scoreData;
}
