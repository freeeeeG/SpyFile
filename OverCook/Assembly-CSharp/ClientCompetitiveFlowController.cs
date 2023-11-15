using System;
using System.Collections;
using OrderController;
using Team17.Online;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class ClientCompetitiveFlowController : ClientKitchenFlowControllerBase
{
	// Token: 0x06001EC0 RID: 7872 RVA: 0x00096848 File Offset: 0x00094C48
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_competitiveFlowController = (CompetitiveFlowController)synchronisedObject;
		this.m_teamOneMonitor.Initialise(this.m_competitiveFlowController.m_teamOneData, TeamID.One, new ClientOrderControllerBuilder(this.BuildOrderController));
		this.m_teamTwoMonitor.Initialise(this.m_competitiveFlowController.m_teamTwoData, TeamID.Two, new ClientOrderControllerBuilder(this.BuildOrderController));
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x000968BB File Offset: 0x00094CBB
	public override ClientTeamMonitor GetMonitorForTeam(TeamID _team)
	{
		if (_team == TeamID.One)
		{
			return this.m_teamOneMonitor;
		}
		if (_team != TeamID.Two)
		{
			return null;
		}
		return this.m_teamTwoMonitor;
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000968DE File Offset: 0x00094CDE
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		this.m_teamOneMonitor.Update();
		this.m_teamTwoMonitor.Update();
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x000968FC File Offset: 0x00094CFC
	protected override IEnumerator RunLevelOutro()
	{
		this.FinaliseRoundTimer();
		if (this.m_competitiveFlowController.m_outroFlowroutine != null)
		{
			OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
			if (overcookedAchievementManager != null)
			{
				overcookedAchievementManager.IncStat(16, 1f, ControlPadInput.PadNum.One);
				overcookedAchievementManager.IncStat(11, 1f, ControlPadInput.PadNum.One);
				int totalScore = this.GetMonitorForTeam(TeamID.One).Score.GetTotalScore();
				int totalScore2 = this.GetMonitorForTeam(TeamID.Two).Score.GetTotalScore();
				User user = ClientUserSystem.m_Users.Find((User x) => x.IsLocal);
				if (user != null)
				{
					if (totalScore > totalScore2 && user.Team == TeamID.One)
					{
						overcookedAchievementManager.IncStat(9, 1f, ControlPadInput.PadNum.One);
					}
					else if (totalScore < totalScore2 && user.Team == TeamID.Two)
					{
						overcookedAchievementManager.IncStat(9, 1f, ControlPadInput.PadNum.One);
					}
				}
			}
			TeamMonitor.TeamScoreStats score = this.m_teamOneMonitor.Score;
			TeamMonitor.TeamScoreStats score2 = this.m_teamTwoMonitor.Score;
			CompetitiveScoreboardUIController.ScoreData scoreData = new CompetitiveScoreboardUIController.ScoreData();
			scoreData.TeamOneData = new TeamMonitor.TeamScoreStats();
			scoreData.TeamTwoData = new TeamMonitor.TeamScoreStats();
			scoreData.TeamOneData.Copy(score);
			scoreData.TeamTwoData.Copy(score2);
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				Analytics.LogEvent("Level Score", (long)Mathf.Max(score.GetTotalScore(), score2.GetTotalScore()), Analytics.Flags.LevelName | Analytics.Flags.PlayerCount);
			}
			CompetitiveFlowController.OutroData setupData = new CompetitiveFlowController.OutroData(scoreData);
			return this.m_competitiveFlowController.m_outroFlowroutine.BuildFlowroutine(setupData);
		}
		return null;
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x00096A98 File Offset: 0x00094E98
	protected override void OnSuccessfulDelivery(TeamID _teamID, OrderID _orderID, float _timePropRemainingPercentage, int _tip, bool _wasCombo, ClientPlateStation _station)
	{
		base.OnSuccessfulDelivery(_teamID, _orderID, _timePropRemainingPercentage, _tip, _wasCombo, _station);
		TeamTip teamTip = new TeamTip
		{
			m_team = _teamID,
			m_tip = _tip,
			m_station = _station
		};
		this.m_dataStore.Write(ClientCompetitiveFlowController.k_scoreTipId, teamTip);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x00096AF0 File Offset: 0x00094EF0
	protected override void OnFailedDelivery(TeamID _teamID, OrderID _orderID)
	{
		base.OnFailedDelivery(_teamID, _orderID);
		TeamTip teamTip = new TeamTip
		{
			m_team = _teamID,
			m_tip = 0
		};
		this.m_dataStore.Write(ClientCompetitiveFlowController.k_scoreTipId, teamTip);
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x00096B38 File Offset: 0x00094F38
	protected override void OnOrderExpired(TeamID _teamID, OrderID _orderID)
	{
		base.OnOrderExpired(_teamID, _orderID);
		TeamTip teamTip = new TeamTip
		{
			m_team = _teamID,
			m_tip = 0
		};
		this.m_dataStore.Write(ClientCompetitiveFlowController.k_scoreTipId, teamTip);
	}

	// Token: 0x0400178D RID: 6029
	private CompetitiveFlowController m_competitiveFlowController;

	// Token: 0x0400178E RID: 6030
	private DataStore m_dataStore;

	// Token: 0x0400178F RID: 6031
	private static readonly DataStore.Id k_scoreTipId = new DataStore.Id("score.tip");

	// Token: 0x04001790 RID: 6032
	private ClientTeamMonitor m_teamOneMonitor = new ClientTeamMonitor();

	// Token: 0x04001791 RID: 6033
	private ClientTeamMonitor m_teamTwoMonitor = new ClientTeamMonitor();

	// Token: 0x04001792 RID: 6034
	private ILogicalButton m_cancelButton;
}
