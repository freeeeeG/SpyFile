using System;
using OrderController;
using UnityEngine;

namespace GameModes
{
	// Token: 0x020006BB RID: 1723
	public class ClientSurvivalMode : ClientGameModeBase
	{
		// Token: 0x06002098 RID: 8344 RVA: 0x0009D756 File Offset: 0x0009BB56
		public ClientSurvivalMode(Config config) : base(config)
		{
			this.m_config = (SurvivalModeConfig)config;
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0009D76C File Offset: 0x0009BB6C
		private ClientOrderControllerBase ClientOrderControllerBuilder(RecipeFlowGUI _recipeUI)
		{
			ClientFixedTimeOrderController clientFixedTimeOrderController = new ClientFixedTimeOrderController(_recipeUI);
			clientFixedTimeOrderController.SetRoundTimer(this.m_roundTimer);
			return clientFixedTimeOrderController;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x0009D790 File Offset: 0x0009BB90
		public override void Setup(ClientContext context, SessionConfig config, ref ClientSetupData setupData)
		{
			this.m_context = context;
			this.m_roundTimer = new ClientModifiableRoundTimer();
			setupData.m_orderControllerBuilder = new ClientOrderControllerBuilder(this.ClientOrderControllerBuilder);
			setupData.m_roundTimer = this.m_roundTimer;
			setupData.m_onSuccessfulDelivery = new OnSuccessfulDeliveryClient(this.OnSuccessfulDelivery);
			setupData.m_onOrderExpired = new OnOrderExpiredClient(this.OnOrderExpired);
			setupData.m_onOutro = new OnOutroClient(this.OnOutro);
			GameObject source = (context.m_teamCount != 1) ? this.m_config.m_competitiveUIPrefab : this.m_config.m_uiPrefab;
			GameUtils.InstantiateUIControllerOnScalingHUDCanvas(source);
			this.m_outroFlowroutine = new SurvivalModeOutroFlowroutine();
			this.m_dataStore = GameUtils.RequireManager<DataStore>();
			this.m_defaultLayer = LayerMask.NameToLayer("Default");
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0009D858 File Offset: 0x0009BC58
		public override void Update()
		{
			if (!TimeManager.IsPaused(TimeManager.PauseLayer.Main))
			{
				this.m_timeSurvived += TimeManager.GetDeltaTime(this.m_defaultLayer);
				this.m_dataStore.Write(ClientSurvivalMode.k_timeSurvivedId, this.m_timeSurvived);
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x0009D898 File Offset: 0x0009BC98
		private void OnSuccessfulDelivery(TeamID teamID, OrderID orderID, float timePropRemainingPercentage, int tip, bool wasCombo, ClientPlateStation station)
		{
			ClientTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam(teamID);
			RecipeList.Entry recipe = monitorForTeam.OrdersController.GetRecipe(orderID);
			float num = (float)this.m_config.m_recipeTimes.Get(recipe.m_order);
			int num2 = SurvivalModeUtil.CalculateDeliveryBonus(this.m_config, timePropRemainingPercentage);
			this.m_roundTimer.AddTime((int)(this.m_config.m_timeMultiplier * num) + num2);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x0009D904 File Offset: 0x0009BD04
		private void OnOrderExpired(TeamID teamID, OrderID orderID)
		{
			this.m_roundTimer.AddTime(-Math.Abs(this.m_config.m_recipeTimes.RecipeFailedPenalty));
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0009D928 File Offset: 0x0009BD28
		private IFlowroutine OnOutro(GenericVoid onRestartRequest, int starRating)
		{
			ClientTeamMonitor monitorForTeam = this.m_context.m_flowController.GetMonitorForTeam(TeamID.One);
			GameSession gameSession = GameUtils.GetGameSession();
			TeamMonitor.TeamScoreStats score = monitorForTeam.Score;
			int levelID = GameUtils.GetLevelID();
			GameProgress.GameProgressData.LevelProgress levelProgress = gameSession.Progress.SaveData.GetLevelProgress(levelID);
			gameSession.Progress.RecordLevelScore(new GameProgress.HighScores.Score
			{
				iLevelID = levelID,
				iHighScore = int.MinValue,
				iSurvivalModeTime = Mathf.RoundToInt(this.m_timeSurvived)
			});
			SurvivalModeRatingUIController.ScoreData scoreData = new SurvivalModeRatingUIController.ScoreData
			{
				m_timeSurvived = Mathf.RoundToInt(this.m_timeSurvived),
				m_successPoints = score.TotalBaseScore,
				m_failDeductions = score.TotalTimeExpireDeductions,
				m_tips = score.TotalTipsScore,
				m_score = score.GetTotalScore(),
				m_totalSuccessfulDeliveries = score.TotalSuccessfulDeliveries
			};
			this.m_config.m_outroFlowroutineData.m_scoreData = scoreData;
			this.m_outroFlowroutine.OnRestartRequest = onRestartRequest;
			return this.m_outroFlowroutine.BuildFlowroutine(this.m_config.m_outroFlowroutineData);
		}

		// Token: 0x040018FF RID: 6399
		private ClientContext m_context;

		// Token: 0x04001900 RID: 6400
		private SurvivalModeConfig m_config;

		// Token: 0x04001901 RID: 6401
		private ClientModifiableRoundTimer m_roundTimer;

		// Token: 0x04001902 RID: 6402
		private SurvivalModeOutroFlowroutine m_outroFlowroutine;

		// Token: 0x04001903 RID: 6403
		private float m_timeSurvived;

		// Token: 0x04001904 RID: 6404
		private DataStore m_dataStore;

		// Token: 0x04001905 RID: 6405
		private static readonly DataStore.Id k_timeSurvivedId = new DataStore.Id("time.survived");

		// Token: 0x04001906 RID: 6406
		private int m_defaultLayer;
	}
}
