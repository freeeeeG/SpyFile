using System;
using System.Collections;
using GameModes;
using OrderController;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000648 RID: 1608
public class ClientCampaignFlowController : ClientKitchenFlowControllerBase
{
	// Token: 0x06001EA3 RID: 7843 RVA: 0x00095368 File Offset: 0x00093768
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_campaignFlowController = (CampaignFlowController)synchronisedObject;
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameMode = gameSession.GetGameModeClient(base.LevelConfig as KitchenLevelConfigBase);
		this.m_gameModeContext = new ClientContext
		{
			m_gameObject = base.gameObject,
			m_levelConfig = (base.LevelConfig as KitchenLevelConfigBase),
			m_teamCount = 1,
			m_flowController = this
		};
		this.m_gameMode.Setup(this.m_gameModeContext, gameSession.GameModeSessionConfig, ref this.m_gameModeSetupData);
		this.m_roundTimer = this.m_gameModeSetupData.m_roundTimer;
		this.m_roundTimer.Initialise();
		if (this.m_gameModeSetupData.m_onSessionConfigChangedCallback != null)
		{
			GameSession gameSession2 = gameSession;
			gameSession2.OnGameModeSessionConfigChanged = (OnSessionConfigChanged)Delegate.Combine(gameSession2.OnGameModeSessionConfigChanged, this.m_gameModeSetupData.m_onSessionConfigChangedCallback);
		}
		this.m_teamMonitor.Initialise(this.m_campaignFlowController.m_teamMonitor, TeamID.One, new ClientOrderControllerBuilder(this.BuildOrderController));
		this.m_gameMode.Begin();
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x00095476 File Offset: 0x00093876
	protected override ClientOrderControllerBase BuildOrderController(RecipeFlowGUI _recipeUI)
	{
		return this.m_gameModeSetupData.m_orderControllerBuilder(_recipeUI);
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x0009548C File Offset: 0x0009388C
	public override void StopSynchronising()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (this.m_gameModeSetupData.m_onSessionConfigChangedCallback != null)
		{
			GameSession gameSession2 = gameSession;
			gameSession2.OnGameModeSessionConfigChanged = (OnSessionConfigChanged)Delegate.Remove(gameSession2.OnGameModeSessionConfigChanged, this.m_gameModeSetupData.m_onSessionConfigChangedCallback);
		}
		if (this.m_gameMode != null)
		{
			this.m_gameMode.End();
		}
		base.StopSynchronising();
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x000954EC File Offset: 0x000938EC
	public void RegisterOnSuccessfulDeliveryCallback(VoidGeneric<RecipeList.Entry> _callback)
	{
		this.m_successCallback = (VoidGeneric<RecipeList.Entry>)Delegate.Combine(this.m_successCallback, _callback);
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x00095505 File Offset: 0x00093905
	public void UnregisterOnSuccessfulDeliveryCallback(VoidGeneric<RecipeList.Entry> _callback)
	{
		this.m_successCallback = (VoidGeneric<RecipeList.Entry>)Delegate.Remove(this.m_successCallback, _callback);
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x0009551E File Offset: 0x0009391E
	public override ClientTeamMonitor GetMonitorForTeam(TeamID _team)
	{
		return this.m_teamMonitor;
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x00095526 File Offset: 0x00093926
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		this.m_teamMonitor.Update();
		this.m_gameMode.Update();
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x00095544 File Offset: 0x00093944
	protected override void OnSuccessfulDelivery(TeamID _teamID, OrderID _orderID, float _timePropRemainingPercentage, int _tip, bool _wasCombo, ClientPlateStation _station)
	{
		RecipeList.Entry recipe = this.m_teamMonitor.OrdersController.GetRecipe(_orderID);
		if (this.m_gameModeSetupData.m_onSuccessfulDelivery != null)
		{
			this.m_gameModeSetupData.m_onSuccessfulDelivery(_teamID, _orderID, _timePropRemainingPercentage, _tip, _wasCombo, _station);
		}
		this.m_successCallback(recipe);
		base.OnSuccessfulDelivery(_teamID, _orderID, _timePropRemainingPercentage, _tip, _wasCombo, _station);
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x000955A7 File Offset: 0x000939A7
	protected override void OnFailedDelivery(TeamID _teamID, OrderID _orderID)
	{
		if (this.m_gameModeSetupData.m_onFailedDelivery != null)
		{
			this.m_gameModeSetupData.m_onFailedDelivery(_teamID, _orderID);
		}
		base.OnFailedDelivery(_teamID, _orderID);
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x000955D3 File Offset: 0x000939D3
	protected override void OnOrderAdded(TeamID _teamID, Serialisable _orderData)
	{
		if (this.m_gameModeSetupData.m_onOrderAdded != null)
		{
			this.m_gameModeSetupData.m_onOrderAdded(_teamID, _orderData);
		}
		base.OnOrderAdded(_teamID, _orderData);
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x000955FF File Offset: 0x000939FF
	protected override void OnOrderExpired(TeamID _teamID, OrderID _orderID)
	{
		if (this.m_gameModeSetupData.m_onOrderExpired != null)
		{
			this.m_gameModeSetupData.m_onOrderExpired(_teamID, _orderID);
		}
		base.OnOrderExpired(_teamID, _orderID);
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x0009562C File Offset: 0x00093A2C
	protected override IEnumerator RunLevelOutro()
	{
		this.FinaliseRoundTimer();
		TeamMonitor.TeamScoreStats score = this.m_teamMonitor.Score;
		if (this.m_gameModeSetupData.m_onOutro != null)
		{
			return this.m_gameModeSetupData.m_onOutro(new GenericVoid(this.OnLevelRestartRequested), this.GetStarRating(score.GetTotalScore()));
		}
		return null;
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x00095688 File Offset: 0x00093A88
	private void OnLevelRestartRequested()
	{
		bool flag = !ConnectionStatus.IsHost() && ConnectionStatus.IsInSession();
		bool flag2 = ClientGameSetup.Mode != GameMode.Campaign;
		if (flag || flag2)
		{
			return;
		}
		base.gameObject.RequireComponent<ServerCampaignFlowController>().OnLevelRestartRequested();
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x000956D4 File Offset: 0x00093AD4
	protected virtual int GetStarRating(int _points)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		GameSession.GameLevelSettings levelSettings = gameSession.LevelSettings;
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = levelSettings.SceneDirectoryVarientEntry;
		bool inNGPlus = gameSession.Progress.SaveData.IsNGPEnabledForLevel(GameUtils.GetLevelID()) && gameSession.Progress.SaveData.NewGamePlusDialogShown;
		return sceneDirectoryVarientEntry.GetStarForPoints(_points, inNGPlus);
	}

	// Token: 0x0400177D RID: 6013
	private CampaignFlowController m_campaignFlowController;

	// Token: 0x0400177E RID: 6014
	private IClientMode m_gameMode;

	// Token: 0x0400177F RID: 6015
	private ClientContext m_gameModeContext;

	// Token: 0x04001780 RID: 6016
	private ClientSetupData m_gameModeSetupData = default(ClientSetupData);

	// Token: 0x04001781 RID: 6017
	private ClientTeamMonitor m_teamMonitor = new ClientTeamMonitor();

	// Token: 0x04001782 RID: 6018
	private VoidGeneric<RecipeList.Entry> m_successCallback = delegate(RecipeList.Entry _node)
	{
	};
}
