using System;
using System.Diagnostics;
using OrderController;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020006C5 RID: 1733
public abstract class ClientKitchenFlowControllerBase : ClientFlowControllerBase, IRecipeListCache
{
	// Token: 0x060020D5 RID: 8405 RVA: 0x00094D92 File Offset: 0x00093192
	public override EntityType GetEntityType()
	{
		return EntityType.FlowController;
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x00094D96 File Offset: 0x00093196
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_kitchenFlowController = (KitchenFlowControllerBase)synchronisedObject;
		this.m_roundTimer = new ClientRoundTimer();
		this.m_roundTimer.Initialise();
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
		this.CacheRecipeListData();
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x00094DD4 File Offset: 0x000931D4
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		KitchenFlowMessage kitchenFlowMessage = (KitchenFlowMessage)serialisable;
		switch (kitchenFlowMessage.m_msgType)
		{
		case KitchenFlowMessage.MsgType.Delivery:
		{
			ClientTeamMonitor monitorForTeam = this.GetMonitorForTeam(kitchenFlowMessage.m_teamID);
			monitorForTeam.Score.Copy(kitchenFlowMessage.m_teamScore);
			if (kitchenFlowMessage.m_success)
			{
				ClientPlateStation station = kitchenFlowMessage.m_plateStation.RequireComponent<ClientPlateStation>();
				this.OnSuccessfulDelivery(kitchenFlowMessage.m_teamID, kitchenFlowMessage.m_orderID, kitchenFlowMessage.m_timePropRemainingPercentage, kitchenFlowMessage.m_tip, kitchenFlowMessage.m_wasCombo, station);
			}
			else
			{
				this.OnFailedDelivery(kitchenFlowMessage.m_teamID, kitchenFlowMessage.m_orderID);
			}
			break;
		}
		case KitchenFlowMessage.MsgType.OrderAdded:
			this.OnOrderAdded(kitchenFlowMessage.m_teamID, kitchenFlowMessage.m_orderData);
			break;
		case KitchenFlowMessage.MsgType.OrderExpired:
		{
			ClientTeamMonitor monitorForTeam2 = this.GetMonitorForTeam(kitchenFlowMessage.m_teamID);
			monitorForTeam2.Score.Copy(kitchenFlowMessage.m_teamScore);
			this.OnOrderExpired(kitchenFlowMessage.m_teamID, kitchenFlowMessage.m_orderID);
			break;
		}
		case KitchenFlowMessage.MsgType.ScoreOnly:
		{
			ClientTeamMonitor monitorForTeam3 = this.GetMonitorForTeam(kitchenFlowMessage.m_teamID);
			monitorForTeam3.Score.Copy(kitchenFlowMessage.m_teamScore);
			break;
		}
		}
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x00094EF6 File Offset: 0x000932F6
	public OrderDefinitionNode[] GetCachedRecipeList()
	{
		return this.m_cachedRecipeList;
	}

	// Token: 0x060020D9 RID: 8409 RVA: 0x00094EFE File Offset: 0x000932FE
	public AssembledDefinitionNode[] GetCachedAssembledRecipes()
	{
		return this.m_cachedAssembledRecipes;
	}

	// Token: 0x060020DA RID: 8410 RVA: 0x00094F06 File Offset: 0x00093306
	public CookingStepData[] GetCachedCookingSteps()
	{
		return this.m_cachedCookingStepList;
	}

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x060020DB RID: 8411 RVA: 0x00094F10 File Offset: 0x00093310
	// (remove) Token: 0x060020DC RID: 8412 RVA: 0x00094F48 File Offset: 0x00093348
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClientKitchenFlowControllerBase.MealDeliveredCallback m_onMealDelivered;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x060020DD RID: 8413 RVA: 0x00094F80 File Offset: 0x00093380
	// (remove) Token: 0x060020DE RID: 8414 RVA: 0x00094FB8 File Offset: 0x000933B8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event ClientKitchenFlowControllerBase.FailedDeliveryCallback m_onFailedDelivery;

	// Token: 0x060020DF RID: 8415 RVA: 0x00094FF0 File Offset: 0x000933F0
	private void CacheRecipeListData()
	{
		LevelConfigBase levelConfig = base.GetLevelConfig();
		if (levelConfig != null && levelConfig.m_recipeMatchingList != null)
		{
			this.m_cachedRecipeList = new OrderDefinitionNode[levelConfig.m_recipeMatchingList.m_recipes.Length];
			this.m_cachedCookingStepList = new CookingStepData[levelConfig.m_recipeMatchingList.m_cookingSteps.Length];
			levelConfig.m_recipeMatchingList.m_recipes.CopyTo(this.m_cachedRecipeList, 0);
			levelConfig.m_recipeMatchingList.m_cookingSteps.CopyTo(this.m_cachedCookingStepList, 0);
			for (int i = 0; i < levelConfig.m_recipeMatchingList.m_includeLists.Length; i++)
			{
				this.m_cachedRecipeList = this.m_cachedRecipeList.Union(levelConfig.m_recipeMatchingList.m_includeLists[i].m_recipes);
				this.m_cachedCookingStepList = this.m_cachedCookingStepList.Union(levelConfig.m_recipeMatchingList.m_includeLists[i].m_cookingSteps);
			}
			this.m_cachedAssembledRecipes = new AssembledDefinitionNode[this.m_cachedRecipeList.Length];
			for (int j = 0; j < this.m_cachedRecipeList.Length; j++)
			{
				this.m_cachedAssembledRecipes[j] = this.m_cachedRecipeList[j].Convert().Simpilfy();
			}
		}
	}

	// Token: 0x060020E0 RID: 8416 RVA: 0x00095129 File Offset: 0x00093529
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		this.m_roundTimer.Update();
	}

	// Token: 0x060020E1 RID: 8417 RVA: 0x0009513C File Offset: 0x0009353C
	protected virtual ClientOrderControllerBase BuildOrderController(RecipeFlowGUI _recipeUI)
	{
		ClientFixedTimeOrderController clientFixedTimeOrderController = new ClientFixedTimeOrderController(_recipeUI);
		clientFixedTimeOrderController.SetRoundTimer(this.m_roundTimer);
		return clientFixedTimeOrderController;
	}

	// Token: 0x060020E2 RID: 8418 RVA: 0x00095160 File Offset: 0x00093560
	protected virtual void OnSuccessfulDelivery(TeamID _teamID, OrderID _orderID, float _timePropRemainingPercentage, int _tip, bool _wasCombo, ClientPlateStation _station)
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.SuccessfulDelivery, base.gameObject.layer);
		ClientTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		int uID = monitorForTeam.OrdersController.GetRecipe(_orderID).m_order.m_uID;
		monitorForTeam.OrdersController.OnFoodDelivered(true, _orderID);
		this.UpdateScoreUI(_teamID);
		if (this.m_onMealDelivered != null)
		{
			this.m_onMealDelivered(uID, _wasCombo);
		}
		OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
		if (overcookedAchievementManager != null)
		{
			overcookedAchievementManager.IncStat(1, 1f, ControlPadInput.PadNum.One);
			overcookedAchievementManager.AddIDStat(22, uID, ControlPadInput.PadNum.One);
			overcookedAchievementManager.AddIDStat(100, uID, ControlPadInput.PadNum.One);
			overcookedAchievementManager.AddIDStat(500, uID, ControlPadInput.PadNum.One);
			overcookedAchievementManager.AddIDStat(801, uID, ControlPadInput.PadNum.One);
		}
	}

	// Token: 0x060020E3 RID: 8419 RVA: 0x00095218 File Offset: 0x00093618
	protected virtual void OnFailedDelivery(TeamID _teamID, OrderID _orderID)
	{
		ClientTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		monitorForTeam.OrdersController.OnFoodDelivered(false, _orderID);
		this.UpdateScoreUI(_teamID);
		if (this.m_onFailedDelivery != null)
		{
			this.m_onFailedDelivery();
		}
	}

	// Token: 0x060020E4 RID: 8420 RVA: 0x00095258 File Offset: 0x00093658
	protected virtual void OnOrderAdded(TeamID _teamID, Serialisable _orderData)
	{
		ClientTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		monitorForTeam.OrdersController.AddNewOrder(_orderData);
		this.UpdateScoreUI(_teamID);
	}

	// Token: 0x060020E5 RID: 8421 RVA: 0x00095280 File Offset: 0x00093680
	protected virtual void OnOrderExpired(TeamID _teamID, OrderID _orderID)
	{
		ClientTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		monitorForTeam.OrdersController.OnOrderExpired(_orderID);
		this.UpdateScoreUI(_teamID);
	}

	// Token: 0x060020E6 RID: 8422
	public abstract ClientTeamMonitor GetMonitorForTeam(TeamID _team);

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060020E7 RID: 8423 RVA: 0x000952A8 File Offset: 0x000936A8
	public IClientRoundTimer RoundTimer
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x000952B0 File Offset: 0x000936B0
	protected void UpdateScoreUI(TeamID team)
	{
		ClientTeamMonitor monitorForTeam = this.GetMonitorForTeam(team);
		TeamScore teamScore = new TeamScore
		{
			m_team = team,
			m_score = monitorForTeam.Score
		};
		this.m_dataStore.Write(ClientKitchenFlowControllerBase.k_scoreTeamId, teamScore);
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000952FA File Offset: 0x000936FA
	protected virtual void FinaliseRoundTimer()
	{
		this.RoundTimer.Zero();
	}

	// Token: 0x04001917 RID: 6423
	private KitchenFlowControllerBase m_kitchenFlowController;

	// Token: 0x04001918 RID: 6424
	private DataStore m_dataStore;

	// Token: 0x04001919 RID: 6425
	private static readonly DataStore.Id k_scoreTeamId = new DataStore.Id("score.team");

	// Token: 0x0400191A RID: 6426
	protected IClientRoundTimer m_roundTimer;

	// Token: 0x0400191B RID: 6427
	private OrderDefinitionNode[] m_cachedRecipeList = new OrderDefinitionNode[0];

	// Token: 0x0400191C RID: 6428
	private AssembledDefinitionNode[] m_cachedAssembledRecipes = new AssembledDefinitionNode[0];

	// Token: 0x0400191D RID: 6429
	private CookingStepData[] m_cachedCookingStepList = new CookingStepData[0];

	// Token: 0x020006C6 RID: 1734
	// (Invoke) Token: 0x060020EC RID: 8428
	public delegate void MealDeliveredCallback(int mealId, bool bWasCombo);

	// Token: 0x020006C7 RID: 1735
	// (Invoke) Token: 0x060020F0 RID: 8432
	public delegate void FailedDeliveryCallback();
}
