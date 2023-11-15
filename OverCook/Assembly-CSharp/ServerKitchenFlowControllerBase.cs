using System;
using OrderController;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public abstract class ServerKitchenFlowControllerBase : ServerFlowControllerBase, IKitchenOrderHandler
{
	// Token: 0x060020C0 RID: 8384 RVA: 0x000938EA File Offset: 0x00091CEA
	public override EntityType GetEntityType()
	{
		return EntityType.FlowController;
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x000938F0 File Offset: 0x00091CF0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_kitchenFlowController = (KitchenFlowControllerBase)synchronisedObject;
		this.m_roundTimer = new ServerRoundTimer();
		this.m_roundTimer.Initialise();
		PlateReturnController.PlateReturnControllerConfig plateReturnControllerConfig;
		plateReturnControllerConfig.m_plateReturnTime = (base.LevelConfig as KitchenLevelConfigBase).m_plateReturnTime;
		this.m_plateReturnController = new PlateReturnController(ref plateReturnControllerConfig);
		this.m_plateReturnController.Init();
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x00093955 File Offset: 0x00091D55
	private void SendOrderAdded(TeamID _teamID, Serialisable _orderData)
	{
		this.m_data.Initialise_OrderAdded(_teamID, _orderData);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x00093970 File Offset: 0x00091D70
	private void SendOrderExpired(TeamID _teamID, OrderID _orderID)
	{
		this.m_data.Initialise_OrderExpired(_teamID, _orderID);
		this.m_data.SetScoreData(this.GetMonitorForTeam(_teamID).Score);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x000939A2 File Offset: 0x00091DA2
	private void SendDeliverySuccess(TeamID _teamID, ServerPlateStation _plateStation, OrderID _orderID, float _timePropRemainingPercentage, int _tip, bool _wasCombo)
	{
		this.m_data.Initialise_DeliverySuccess(_teamID, _plateStation.gameObject, _orderID, _timePropRemainingPercentage, _tip, _wasCombo);
		this.m_data.SetScoreData(this.GetMonitorForTeam(_teamID).Score);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x000939E0 File Offset: 0x00091DE0
	private void SendDeliveryFailed(TeamID _teamID, ServerPlateStation _plateStation)
	{
		this.m_data.Initialise_DeliveryFailed(_teamID, _plateStation.gameObject);
		this.m_data.SetScoreData(this.GetMonitorForTeam(_teamID).Score);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x00093A17 File Offset: 0x00091E17
	protected void SendScore(TeamID _teamID)
	{
		this.m_data.Initialise_ScoreOnly(_teamID);
		this.m_data.SetScoreData(this.GetMonitorForTeam(_teamID).Score);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x00093A48 File Offset: 0x00091E48
	protected virtual ServerOrderControllerBase BuildOrderController(VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
	{
		ServerFixedTimeOrderController.OrdersConfig ordersConfig = this.BuildOrderConfig();
		ServerOrderControllerBase serverOrderControllerBase = new ServerFixedTimeOrderController(ref ordersConfig, _addedCallback, _timeoutCallback);
		serverOrderControllerBase.SetRoundTimer(this.m_roundTimer);
		return serverOrderControllerBase;
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x00093A74 File Offset: 0x00091E74
	protected ServerFixedTimeOrderController.OrdersConfig BuildOrderConfig()
	{
		KitchenLevelConfigBase kitchenLevelConfigBase = base.LevelConfig as KitchenLevelConfigBase;
		ServerFixedTimeOrderController.OrdersConfig result;
		result.m_maxActiveOrders = this.m_kitchenFlowController.m_maxOrdersAllowed;
		result.m_roundData = kitchenLevelConfigBase.GetRoundData();
		result.m_orderLifetime = kitchenLevelConfigBase.m_orderLifetime;
		result.m_timeBetweenOrders = kitchenLevelConfigBase.m_timeBetweenOrders;
		return result;
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x00093AC7 File Offset: 0x00091EC7
	protected override void OnUpdateInRound()
	{
		base.OnUpdateInRound();
		if (this.m_roundTimer != null)
		{
			this.m_roundTimer.Update();
		}
		if (this.m_plateReturnController != null)
		{
			this.m_plateReturnController.Update();
		}
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x00093AFB File Offset: 0x00091EFB
	protected override bool HasFinished()
	{
		return this.m_roundTimer.TimeExpired() || base.HasFinished();
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x00093B18 File Offset: 0x00091F18
	protected virtual void OnOrderAdded(TeamID _teamID, OrderID _orderID)
	{
		ServerTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		Serialisable serialisedOrderData = monitorForTeam.OrdersController.GetSerialisedOrderData(_orderID);
		this.SendOrderAdded(_teamID, serialisedOrderData);
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x00093B44 File Offset: 0x00091F44
	protected virtual void OnOrderExpired(TeamID _teamID, OrderID _orderID)
	{
		ServerTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		int recipeTimeOutPointLoss = GameUtils.GetGameConfig().RecipeTimeOutPointLoss;
		monitorForTeam.Score.TotalTimeExpireDeductions += recipeTimeOutPointLoss;
		monitorForTeam.Score.TotalMultiplier = 0;
		monitorForTeam.Score.TotalCombo = 0;
		monitorForTeam.Score.ComboMaintained = false;
		monitorForTeam.OrdersController.ResetOrderLifetime(_orderID);
		this.SendOrderExpired(_teamID, _orderID);
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x00093BAF File Offset: 0x00091FAF
	public void FoodDelivered(AssembledDefinitionNode _definition, PlatingStepData _plateType, ServerPlateStation _station)
	{
		this.OnFoodDelivered(_definition, _plateType, _station);
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x00093BBC File Offset: 0x00091FBC
	protected virtual void OnFoodDelivered(AssembledDefinitionNode _definition, PlatingStepData _plateType, ServerPlateStation _station)
	{
		this.m_plateReturnController.FoodDelivered(_definition, _plateType, _station);
		ServerTeamMonitor monitorForTeam = this.GetMonitorForTeam(_station.GetTeamID());
		OrderID orderID;
		RecipeList.Entry entry;
		float timePropRemainingPercentage;
		bool wasCombo;
		bool flag = monitorForTeam.OnFoodDelivered(_definition, _plateType, out orderID, out entry, out timePropRemainingPercentage, out wasCombo);
		if (flag)
		{
			this.OnSuccessfulDelivery(orderID, entry, timePropRemainingPercentage, wasCombo, _station);
		}
		else
		{
			this.OnFailedDelivery(_station);
		}
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x00093C18 File Offset: 0x00092018
	protected virtual void OnSuccessfulDelivery(OrderID _orderID, RecipeList.Entry _entry, float _timePropRemainingPercentage, bool _wasCombo, ServerPlateStation _plateStation)
	{
		int num = this.m_kitchenFlowController.CalculateTip(_timePropRemainingPercentage);
		int num2 = this.m_kitchenFlowController.CalculateBaseScore(_entry);
		ServerTeamMonitor monitorForTeam = this.GetMonitorForTeam(_plateStation.GetTeamID());
		int num3 = num * Mathf.Max(monitorForTeam.Score.TotalMultiplier, 1);
		monitorForTeam.Score.TotalBaseScore += num2;
		monitorForTeam.Score.TotalTipsScore += num3;
		monitorForTeam.Score.TotalSuccessfulDeliveries++;
		if (_wasCombo)
		{
			monitorForTeam.Score.TotalCombo++;
			if (monitorForTeam.Score.TotalMultiplier < 4)
			{
				monitorForTeam.Score.TotalMultiplier++;
			}
		}
		else
		{
			monitorForTeam.Score.TotalMultiplier = 0;
			monitorForTeam.Score.TotalCombo = 0;
			monitorForTeam.Score.ComboMaintained = false;
		}
		this.SendDeliverySuccess(_plateStation.GetTeamID(), _plateStation, _orderID, _timePropRemainingPercentage, num3, _wasCombo);
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x00093D18 File Offset: 0x00092118
	protected virtual void OnFailedDelivery(ServerPlateStation _plateStation)
	{
		ServerTeamMonitor monitorForTeam = this.GetMonitorForTeam(_plateStation.GetTeamID());
		monitorForTeam.Score.TotalMultiplier = 0;
		monitorForTeam.Score.TotalCombo = 0;
		monitorForTeam.Score.ComboMaintained = false;
		this.SendDeliveryFailed(_plateStation.GetTeamID(), _plateStation);
	}

	// Token: 0x060020D1 RID: 8401
	public abstract ServerTeamMonitor GetMonitorForTeam(TeamID _team);

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00093D63 File Offset: 0x00092163
	public IServerRoundTimer RoundTimer
	{
		get
		{
			return this.m_roundTimer;
		}
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x00093D6C File Offset: 0x0009216C
	public int GetPoints(TeamID _teamID)
	{
		ServerTeamMonitor monitorForTeam = this.GetMonitorForTeam(_teamID);
		return monitorForTeam.Score.GetTotalScore();
	}

	// Token: 0x04001911 RID: 6417
	private KitchenFlowControllerBase m_kitchenFlowController;

	// Token: 0x04001912 RID: 6418
	private KitchenFlowMessage m_data = new KitchenFlowMessage();

	// Token: 0x04001913 RID: 6419
	private const int c_scoreMultiplierMax = 4;

	// Token: 0x04001914 RID: 6420
	private const int c_scoreMultiplierMultiplier = 2;

	// Token: 0x04001915 RID: 6421
	protected IServerRoundTimer m_roundTimer;

	// Token: 0x04001916 RID: 6422
	private PlateReturnController m_plateReturnController;
}
