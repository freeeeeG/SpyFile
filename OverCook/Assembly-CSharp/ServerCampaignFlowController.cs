using System;
using GameModes;
using OrderController;
using UnityEngine;

// Token: 0x02000647 RID: 1607
public class ServerCampaignFlowController : ServerKitchenFlowControllerBase
{
	// Token: 0x06001E8E RID: 7822 RVA: 0x00093DDC File Offset: 0x000921DC
	public void OnLevelRestartRequested()
	{
		this.m_LevelRestartRequested = true;
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x00093DE8 File Offset: 0x000921E8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_campaignFlowController = (CampaignFlowController)synchronisedObject;
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameMode = gameSession.GetGameModeServer(base.LevelConfig as KitchenLevelConfigBase);
		this.m_gameModeContext = new ServerContext
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
		this.m_teamMonitor.Initialise(this.m_campaignFlowController.m_teamMonitor, new ServerOrderControllerBuilder(this.BuildOrderController), delegate(OrderID _order)
		{
			this.OnOrderAdded(TeamID.One, _order);
		}, delegate(OrderID _order)
		{
			this.OnOrderExpired(TeamID.One, _order);
		});
		this.m_gameMode.Begin();
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x00093F0D File Offset: 0x0009230D
	protected override ServerOrderControllerBase BuildOrderController(VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
	{
		return this.m_gameModeSetupData.m_orderControllerBuilder(_addedCallback, _timeoutCallback);
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x00093F24 File Offset: 0x00092324
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

	// Token: 0x06001E92 RID: 7826 RVA: 0x00093F84 File Offset: 0x00092384
	public void RegisterOnSuccessfulDeliveryCallback(VoidGeneric<RecipeList.Entry> _callback)
	{
		this.m_successCallback = (VoidGeneric<RecipeList.Entry>)Delegate.Combine(this.m_successCallback, _callback);
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x00093F9D File Offset: 0x0009239D
	public void UnregisterOnSuccessfulDeliveryCallback(VoidGeneric<RecipeList.Entry> _callback)
	{
		this.m_successCallback = (VoidGeneric<RecipeList.Entry>)Delegate.Remove(this.m_successCallback, _callback);
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x00093FB6 File Offset: 0x000923B6
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		this.m_teamMonitor.Update();
		this.m_gameMode.Update();
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x00093FD4 File Offset: 0x000923D4
	public override ServerTeamMonitor GetMonitorForTeam(TeamID _team)
	{
		return this.m_teamMonitor;
	}

	// Token: 0x06001E96 RID: 7830 RVA: 0x00093FDC File Offset: 0x000923DC
	protected override void OnSuccessfulDelivery(OrderID _orderID, RecipeList.Entry _entry, float _timePropRemainingPercentage, bool _wasCombo, ServerPlateStation _plateStation)
	{
		if (this.m_gameModeSetupData.m_onSuccessfulDelivery != null)
		{
			this.m_gameModeSetupData.m_onSuccessfulDelivery(_orderID, _entry, _timePropRemainingPercentage, _wasCombo, _plateStation);
		}
		this.m_successCallback(_entry);
		base.OnSuccessfulDelivery(_orderID, _entry, _timePropRemainingPercentage, _wasCombo, _plateStation);
	}

	// Token: 0x06001E97 RID: 7831 RVA: 0x00094029 File Offset: 0x00092429
	protected override void OnFailedDelivery(ServerPlateStation _plateStation)
	{
		if (this.m_gameModeSetupData.m_onFailedDelivery != null)
		{
			this.m_gameModeSetupData.m_onFailedDelivery(_plateStation);
		}
		base.OnFailedDelivery(_plateStation);
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x00094053 File Offset: 0x00092453
	protected override void OnOrderAdded(TeamID _teamID, OrderID _orderID)
	{
		if (this.m_gameModeSetupData.m_onOrderAdded != null)
		{
			this.m_gameModeSetupData.m_onOrderAdded(_teamID, _orderID);
		}
		base.OnOrderAdded(_teamID, _orderID);
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x0009407F File Offset: 0x0009247F
	protected override void OnOrderExpired(TeamID _teamID, OrderID _orderID)
	{
		if (this.m_gameModeSetupData.m_onOrderExpired != null)
		{
			this.m_gameModeSetupData.m_onOrderExpired(_teamID, _orderID);
		}
		base.OnOrderExpired(_teamID, _orderID);
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x000940AC File Offset: 0x000924AC
	public virtual void SkipLevel(int _stars)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		GameSession.GameLevelSettings levelSettings = gameSession.LevelSettings;
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneDirectoryVarientEntry = levelSettings.SceneDirectoryVarientEntry;
		int pointsForStar = sceneDirectoryVarientEntry.GetPointsForStar(_stars);
		this.m_teamMonitor.Score.TotalBaseScore = pointsForStar;
		this.m_teamMonitor.Score.TotalTipsScore = 0;
		this.m_teamMonitor.Score.TotalTimeExpireDeductions = 0;
		this.SkipToEnd();
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x0009410E File Offset: 0x0009250E
	public override void SkipToEnd()
	{
		base.SendScore(TeamID.One);
		base.SkipToEnd();
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00094120 File Offset: 0x00092520
	protected override string GetNextScene(out GameState o_loadState, out GameState o_loadEndState, out bool o_useLoadingScreen)
	{
		o_loadState = GameState.NotSet;
		o_loadEndState = GameState.NotSet;
		o_useLoadingScreen = true;
		if (this.m_LevelRestartRequested)
		{
			o_loadState = GameState.LoadKitchen;
			o_loadEndState = GameState.RunLevelIntro;
			o_useLoadingScreen = true;
			return GameUtils.GetGameSession().LevelSettings.SceneDirectoryVarientEntry.SceneName;
		}
		if (ServerGameSetup.Mode == GameMode.Campaign)
		{
			CampaignFlowController.IOutroFlowSceneProvider outroFlowSceneProvider = (this.m_gameModeSetupData.m_onOutroScene == null) ? base.gameObject.RequestInterface<CampaignFlowController.IOutroFlowSceneProvider>() : this.m_gameModeSetupData.m_onOutroScene();
			if (outroFlowSceneProvider != null)
			{
				return outroFlowSceneProvider.GetNextScene(out o_loadState, out o_loadEndState, out o_useLoadingScreen);
			}
			o_loadState = GameState.CampaignMap;
			o_loadEndState = GameState.RunMapUnfoldRoutine;
			return GameUtils.GetGameSession().TypeSettings.WorldMapScene;
		}
		else
		{
			if (ServerGameSetup.Mode == GameMode.Party)
			{
				o_loadState = GameState.PartyLobby;
				o_loadEndState = GameState.NotSet;
				return GameUtils.GetGameSession().TypeSettings.WorldMapScene;
			}
			return string.Empty;
		}
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000941EB File Offset: 0x000925EB
	public void SetOrdersAutoProgress(bool _autoProgress)
	{
		this.m_teamMonitor.OrdersController.SetAutoProgress(_autoProgress);
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x000941FE File Offset: 0x000925FE
	public void AddNextOrder()
	{
		this.m_teamMonitor.OrdersController.AddNewOrder();
	}

	// Token: 0x04001775 RID: 6005
	private CampaignFlowController m_campaignFlowController;

	// Token: 0x04001776 RID: 6006
	private IServerMode m_gameMode;

	// Token: 0x04001777 RID: 6007
	private ServerContext m_gameModeContext;

	// Token: 0x04001778 RID: 6008
	private ServerSetupData m_gameModeSetupData = default(ServerSetupData);

	// Token: 0x04001779 RID: 6009
	private bool m_LevelRestartRequested;

	// Token: 0x0400177A RID: 6010
	private ServerTeamMonitor m_teamMonitor = new ServerTeamMonitor();

	// Token: 0x0400177B RID: 6011
	private VoidGeneric<RecipeList.Entry> m_successCallback = delegate(RecipeList.Entry _node)
	{
	};
}
