using System;
using OrderController;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class ServerCompetitiveFlowController : ServerKitchenFlowControllerBase
{
	// Token: 0x06001EB7 RID: 7863 RVA: 0x000966F4 File Offset: 0x00094AF4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_competitiveFlowController = (CompetitiveFlowController)synchronisedObject;
		this.m_teamOneMonitor.Initialise(this.m_competitiveFlowController.m_teamOneData, new ServerOrderControllerBuilder(this.BuildOrderController), delegate(OrderID _order)
		{
			this.OnOrderAdded(TeamID.One, _order);
		}, delegate(OrderID _order)
		{
			this.OnOrderExpired(TeamID.One, _order);
		});
		this.m_teamTwoMonitor.Initialise(this.m_competitiveFlowController.m_teamTwoData, new ServerOrderControllerBuilder(this.BuildOrderController), delegate(OrderID _order)
		{
			this.OnOrderAdded(TeamID.Two, _order);
		}, delegate(OrderID _order)
		{
			this.OnOrderExpired(TeamID.Two, _order);
		});
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x0009678A File Offset: 0x00094B8A
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		this.m_teamOneMonitor.Update();
		this.m_teamTwoMonitor.Update();
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x000967A8 File Offset: 0x00094BA8
	public override ServerTeamMonitor GetMonitorForTeam(TeamID _team)
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

	// Token: 0x06001EBA RID: 7866 RVA: 0x000967CB File Offset: 0x00094BCB
	protected override string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen)
	{
		o_loadState = GameState.NotSet;
		o_loadEndState = GameState.NotSet;
		o_useLoadingScreen = false;
		if (ServerGameSetup.Mode == GameMode.Versus)
		{
			o_loadState = GameState.VSLobby;
			o_loadEndState = GameState.NotSet;
			o_useLoadingScreen = true;
			return GameUtils.GetGameSession().TypeSettings.WorldMapScene;
		}
		return string.Empty;
	}

	// Token: 0x04001789 RID: 6025
	private CompetitiveFlowController m_competitiveFlowController;

	// Token: 0x0400178A RID: 6026
	private ServerTeamMonitor m_teamOneMonitor = new ServerTeamMonitor();

	// Token: 0x0400178B RID: 6027
	private ServerTeamMonitor m_teamTwoMonitor = new ServerTeamMonitor();

	// Token: 0x0400178C RID: 6028
	private ILogicalButton m_cancelButton;
}
