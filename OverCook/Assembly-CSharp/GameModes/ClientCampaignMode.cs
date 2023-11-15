using System;
using OrderController;
using UnityEngine;

namespace GameModes
{
	// Token: 0x02000692 RID: 1682
	public class ClientCampaignMode : ClientGameModeBase
	{
		// Token: 0x06002021 RID: 8225 RVA: 0x0009C8F7 File Offset: 0x0009ACF7
		public ClientCampaignMode(Config config) : base(config)
		{
			this.m_config = (CampaignModeConfig)config;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0009C90C File Offset: 0x0009AD0C
		private ClientOrderControllerBase ClientOrderControllerBuilder(RecipeFlowGUI _recipeUI)
		{
			ClientFixedTimeOrderController clientFixedTimeOrderController = new ClientFixedTimeOrderController(_recipeUI);
			clientFixedTimeOrderController.SetRoundTimer(this.m_roundTimer);
			return clientFixedTimeOrderController;
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0009C930 File Offset: 0x0009AD30
		public override void Setup(ClientContext context, SessionConfig config, ref ClientSetupData setupData)
		{
			this.m_context = context;
			this.m_roundTimer = new ClientRoundTimer();
			this.m_scoreScreenFlowroutine = new ScoreScreenOutroFlowroutine();
			setupData.m_orderControllerBuilder = new ClientOrderControllerBuilder(this.ClientOrderControllerBuilder);
			setupData.m_roundTimer = this.m_roundTimer;
			setupData.m_onSuccessfulDelivery = new OnSuccessfulDeliveryClient(this.OnSuccessfulDelivery);
			setupData.m_onFailedDelivery = new OnFailedDeliveryClient(this.OnFailedDelivery);
			setupData.m_onOrderExpired = new OnOrderExpiredClient(this.OnOrderExpired);
			setupData.m_onOutro = new OnOutroClient(this.OnOutro);
			GameObject gameObject = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_config.m_uiPrefab);
			this.m_dataStore = GameUtils.RequireManager<DataStore>();
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0009C9DC File Offset: 0x0009ADDC
		public override void Begin()
		{
			if (ClientGameSetup.Mode == GameMode.Campaign && this.m_context.m_levelConfig.m_recipesBeforeTimerStarts > 0)
			{
				this.m_recipeDeliverySuppressor = this.m_roundTimer.Suppressor.AddSuppressor(this.m_context.m_gameObject);
			}
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0009CA2C File Offset: 0x0009AE2C
		private void OnSuccessfulDelivery(TeamID teamID, OrderID orderID, float _timePropRemainingPercentage, int tip, bool wasCombo, ClientPlateStation station)
		{
			TeamTip teamTip = new TeamTip
			{
				m_team = teamID,
				m_tip = tip,
				m_station = station
			};
			this.m_dataStore.Write(ClientCampaignMode.k_scoreTipId, teamTip);
			if (this.m_recipeDeliverySuppressor != null)
			{
				this.m_recipesDelivered++;
				if (this.m_recipesDelivered >= this.m_context.m_levelConfig.m_recipesBeforeTimerStarts)
				{
					this.m_recipeDeliverySuppressor.Release();
					this.m_recipeDeliverySuppressor = null;
				}
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0009CABC File Offset: 0x0009AEBC
		private void OnFailedDelivery(TeamID teamId, OrderID orderId)
		{
			TeamTip teamTip = new TeamTip
			{
				m_team = teamId,
				m_tip = 0
			};
			this.m_dataStore.Write(ClientCampaignMode.k_scoreTipId, teamTip);
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0009CAFC File Offset: 0x0009AEFC
		private void OnOrderExpired(TeamID teamId, OrderID orderId)
		{
			TeamTip teamTip = new TeamTip
			{
				m_team = teamId,
				m_tip = 0
			};
			this.m_dataStore.Write(ClientCampaignMode.k_scoreTipId, teamTip);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0009CB3C File Offset: 0x0009AF3C
		private IFlowroutine OnOutro(GenericVoid onRestartRequest, int starRating)
		{
			ClientTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam(TeamID.One);
			GameSession gameSession = GameUtils.GetGameSession();
			TeamMonitor.TeamScoreStats score = monitorForTeam.Score;
			int totalScore = score.GetTotalScore();
			int levelID = GameUtils.GetLevelID();
			GameProgress.GameProgressData.LevelProgress levelProgress = gameSession.Progress.SaveData.GetLevelProgress(levelID);
			bool ngpenabled = levelProgress.NGPEnabled;
			int num = (levelProgress == null) ? 0 : levelProgress.ScoreStars;
			GameProgress.UnlockData[] unlocks = new GameProgress.UnlockData[0];
			gameSession.Progress.RecordLevelProgress(levelID, starRating, ref unlocks);
			gameSession.Progress.RecordLevelScore(new GameProgress.HighScores.Score
			{
				iLevelID = levelID,
				iHighScore = totalScore,
				iSurvivalModeTime = 0
			});
			bool justUnlockedNGP = false;
			if (!ngpenabled)
			{
				justUnlockedNGP = gameSession.Progress.SaveData.GetLevelProgress(levelID).NGPEnabled;
			}
			OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
			if (overcookedAchievementManager != null)
			{
				overcookedAchievementManager.IncStat(16, 1f, ControlPadInput.PadNum.One);
				if (score.ComboMaintained && score.TotalCombo != 0)
				{
					overcookedAchievementManager.AddIDStat(13, 1, ControlPadInput.PadNum.One);
				}
				if (ClientGameSetup.Mode == GameMode.Party)
				{
					overcookedAchievementManager.IncStat(10, 1f, ControlPadInput.PadNum.One);
				}
			}
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				Analytics.LogEvent("Level Score", (long)score.GetTotalScore(), Analytics.Flags.LevelName | Analytics.Flags.PlayerCount);
			}
			CoopStarRatingUIController.ScoreData scoreData = new CoopStarRatingUIController.ScoreData
			{
				SuccessPoints = score.TotalBaseScore,
				FailDeductions = score.TotalTimeExpireDeductions,
				Tips = score.TotalTipsScore,
				Score = totalScore,
				StarRating = starRating,
				StarRatingIncreased = (starRating > num),
				TotalSuccessfulDeliveries = score.TotalSuccessfulDeliveries,
				JustUnlockedNGP = justUnlockedNGP
			};
			CutsceneOutroFlowroutineBase component = this.m_context.m_flowController.GetComponent<CutsceneOutroFlowroutineBase>();
			if (component != null)
			{
				CampaignFlowController.OutroData setupData = new CampaignFlowController.OutroData(scoreData, scoreData.Score, scoreData.StarRating, unlocks);
				return component.BuildFlowroutine(setupData);
			}
			this.m_config.m_scoreScreenData.m_scoreData = scoreData;
			this.m_config.m_scoreScreenData.m_points = scoreData.Score;
			this.m_config.m_scoreScreenData.m_starsAwarded = scoreData.StarRating;
			this.m_config.m_scoreScreenData.m_unlocks = unlocks;
			this.m_scoreScreenFlowroutine.OnRestartRequest = onRestartRequest;
			return this.m_scoreScreenFlowroutine.BuildFlowroutine(this.m_config.m_scoreScreenData);
		}

		// Token: 0x0400189E RID: 6302
		private ClientContext m_context;

		// Token: 0x0400189F RID: 6303
		private CampaignModeConfig m_config;

		// Token: 0x040018A0 RID: 6304
		private Suppressor m_recipeDeliverySuppressor;

		// Token: 0x040018A1 RID: 6305
		private int m_recipesDelivered;

		// Token: 0x040018A2 RID: 6306
		private IClientRoundTimer m_roundTimer;

		// Token: 0x040018A3 RID: 6307
		private ScoreScreenOutroFlowroutine m_scoreScreenFlowroutine;

		// Token: 0x040018A4 RID: 6308
		private DataStore m_dataStore;

		// Token: 0x040018A5 RID: 6309
		private static readonly DataStore.Id k_scoreTipId = new DataStore.Id("score.tip");
	}
}
