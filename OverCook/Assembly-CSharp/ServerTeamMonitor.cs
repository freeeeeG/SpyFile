using System;
using OrderController;

// Token: 0x02000778 RID: 1912
public class ServerTeamMonitor
{
	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060024ED RID: 9453 RVA: 0x000AEAF0 File Offset: 0x000ACEF0
	public TeamMonitor.TeamScoreStats Score
	{
		get
		{
			return this.m_score;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060024EE RID: 9454 RVA: 0x000AEAF8 File Offset: 0x000ACEF8
	public ServerOrderControllerBase OrdersController
	{
		get
		{
			return this.m_ordersController;
		}
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000AEB00 File Offset: 0x000ACF00
	public virtual void Update()
	{
		this.m_ordersController.Update();
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x000AEB0D File Offset: 0x000ACF0D
	public virtual void Initialise(TeamMonitor _monitor, ServerOrderControllerBuilder _controllerBuilder, VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
	{
		this.m_monitor = _monitor;
		this.m_ordersController = _controllerBuilder(_addedCallback, _timeoutCallback);
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x000AEB28 File Offset: 0x000ACF28
	public virtual bool OnFoodDelivered(AssembledDefinitionNode _definition, PlatingStepData _plateType, out OrderID o_orderID, out RecipeList.Entry o_entry, out float o_timePropRemainingPercentage, out bool o_wasCombo)
	{
		o_orderID = default(OrderID);
		o_entry = null;
		o_timePropRemainingPercentage = 0f;
		o_wasCombo = false;
		bool flag = this.m_ordersController.FindBestOrderForRecipe(_definition, _plateType, out o_orderID, out o_timePropRemainingPercentage);
		if (flag)
		{
			o_entry = this.m_ordersController.GetRecipe(o_orderID);
			bool restart = this.m_score.TotalCombo == 0;
			o_wasCombo = this.m_ordersController.IsComboOrder(o_orderID, restart);
			this.m_ordersController.RemoveOrder(o_orderID);
			return true;
		}
		return false;
	}

	// Token: 0x04001C7E RID: 7294
	private TeamMonitor m_monitor;

	// Token: 0x04001C7F RID: 7295
	private ServerOrderControllerBase m_ordersController;

	// Token: 0x04001C80 RID: 7296
	private TeamMonitor.TeamScoreStats m_score = new TeamMonitor.TeamScoreStats();
}
