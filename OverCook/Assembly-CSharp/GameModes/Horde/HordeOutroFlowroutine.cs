using System;
using System.Collections;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007D5 RID: 2005
	public class HordeOutroFlowroutine : OutroFlowroutineBase
	{
		// Token: 0x17000301 RID: 769
		// (set) Token: 0x0600268B RID: 9867 RVA: 0x000B782A File Offset: 0x000B5C2A
		public GenericVoid OnRestartRequest
		{
			set
			{
				this.m_onRestartRequest = value;
			}
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000B7834 File Offset: 0x000B5C34
		protected override void Setup(FlowroutineData flowroutineData)
		{
			this.m_data = (HordeOutroFlowroutineData)flowroutineData;
			this.m_pauseManager = GameUtils.RequireManager<PauseMenuManager>();
			this.m_campaignAudioManager = GameUtils.RequireManager<CampaignAudioManager>();
			this.m_selectButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One);
			this.m_restartButton = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIRestartLevel, PlayerInputLookup.Player.One);
			this.m_hoverIconCanvas = GameUtils.GetNamedCanvas("HoverIconCanvas").RequireComponent<Canvas>();
			if (!this.m_data.m_success && this.m_data.m_failureOutroPrefab != null)
			{
				Camera main = Camera.main;
				this.m_outroInstance = this.m_data.m_failureOutroPrefab.InstantiateOnParent(main.transform, false);
				this.m_outroInstance.SetActive(false);
			}
			if (this.m_data.m_success)
			{
				this.m_successFailInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_data.m_successUIPrefab);
			}
			else
			{
				this.m_successFailInstance = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_data.m_failedUIPrefab);
			}
			this.m_successFailInstance.SetActive(false);
			GameObject obj = GameUtils.InstantiateUIController(this.m_data.m_hordeRatingUIController.gameObject, "UICanvas");
			this.m_ratingController = obj.RequireComponent<HordeRatingUIController>();
			this.m_ratingController.gameObject.SetActive(false);
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000B796E File Offset: 0x000B5D6E
		protected override void Shutdown()
		{
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000B7970 File Offset: 0x000B5D70
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
			if (this.m_outroInstance != null)
			{
				this.m_hoverIconCanvas.enabled = false;
				this.m_outroInstance.SetActive(true);
				GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Mist, layer);
				IEnumerator timer = CoroutineUtils.TimerRoutine(this.m_data.m_failureOutroDelaySeconds, layer);
				while (timer != null && timer.MoveNext())
				{
					yield return null;
				}
				if (this.m_successFailInstance != null)
				{
					this.m_successFailInstance.SetActive(true);
					GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Failed, layer);
					timer = CoroutineUtils.TimerRoutine(this.m_data.m_successFailPrefabDelaySeconds, layer);
					while (timer != null && timer.MoveNext())
					{
						yield return null;
					}
				}
				this.m_hoverIconCanvas.enabled = true;
			}
			else
			{
				if (this.m_successFailInstance != null)
				{
					this.m_successFailInstance.SetActive(true);
					GameUtils.TriggerAudio(GameOneShotAudioTag.DLC_07_Success, layer);
					IEnumerator timer2 = CoroutineUtils.TimerRoutine(this.m_data.m_successFailPrefabDelaySeconds, layer);
					while (timer2 != null && timer2.MoveNext())
					{
						yield return null;
					}
				}
				GameUtils.TriggerAudio(GameOneShotAudioTag.LevelEnd, layer);
			}
			this.m_ratingController.gameObject.SetActive(true);
			this.m_ratingController.SetScoreData(this.m_data.m_scoreData);
			IEnumerator minTimeDelay = CoroutineUtils.TimerRoutine(1f, layer);
			while (minTimeDelay.MoveNext())
			{
				yield return null;
			}
			while (!this.m_ratingController.HasAnimationSettled())
			{
				yield return null;
			}
			IEnumerator timeoutRoutine = CoroutineUtils.TimerRoutine(this.m_data.m_minOutroDelaySeconds, layer);
			while (timeoutRoutine.MoveNext())
			{
				if (this.m_ratingController.AllowedToSkip())
				{
					if (this.m_selectButton.JustPressed())
					{
						break;
					}
					if (this.m_ratingController.AllowedToRestart() && this.m_restartButton.JustPressed())
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
				this.m_ratingController.m_waitingForPlayers.SetActive(true);
			}
			yield break;
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000B798C File Offset: 0x000B5D8C
		private void OnLevelLoad(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByIndex, new OrderedMessageReceivedCallback(this.OnLevelLoad));
			Mailbox.Client.UnregisterForMessageType(MessageType.LevelLoadByName, new OrderedMessageReceivedCallback(this.OnLevelLoad));
			if (T17InGameFlow.Instance != null)
			{
				T17InGameFlow.Instance.SetInRoundResults(false);
			}
		}

		// Token: 0x04001E99 RID: 7833
		private HordeOutroFlowroutineData m_data;

		// Token: 0x04001E9A RID: 7834
		private PauseMenuManager m_pauseManager;

		// Token: 0x04001E9B RID: 7835
		private CampaignAudioManager m_campaignAudioManager;

		// Token: 0x04001E9C RID: 7836
		private Canvas m_hoverIconCanvas;

		// Token: 0x04001E9D RID: 7837
		private ILogicalButton m_selectButton;

		// Token: 0x04001E9E RID: 7838
		private ILogicalButton m_restartButton;

		// Token: 0x04001E9F RID: 7839
		private GameObject m_outroInstance;

		// Token: 0x04001EA0 RID: 7840
		private GameObject m_successFailInstance;

		// Token: 0x04001EA1 RID: 7841
		private HordeRatingUIController m_ratingController;

		// Token: 0x04001EA2 RID: 7842
		private GenericVoid m_onRestartRequest;
	}
}
