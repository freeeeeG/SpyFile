using System;
using System.Collections.Generic;
using OrderController;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000759 RID: 1881
public abstract class ClientOrderControllerBase
{
	// Token: 0x0600243D RID: 9277 RVA: 0x00096374 File Offset: 0x00094774
	public ClientOrderControllerBase(RecipeFlowGUI _flowGUI)
	{
		this.m_gui = _flowGUI;
		this.m_gui.gameObject.layer = LayerMask.NameToLayer("Default");
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x0600243E RID: 9278 RVA: 0x000963DD File Offset: 0x000947DD
	// (set) Token: 0x0600243F RID: 9279 RVA: 0x000963E5 File Offset: 0x000947E5
	public bool EnableOrderExpiration
	{
		get
		{
			return this.m_enableOrderExpiration;
		}
		set
		{
			this.m_enableOrderExpiration = value;
		}
	}

	// Token: 0x06002440 RID: 9280 RVA: 0x000963F0 File Offset: 0x000947F0
	public virtual void Update()
	{
		float dt = (this.m_roundTimer == null || this.m_roundTimer.IsSuppressed || !this.m_enableOrderExpiration) ? 0f : TimeManager.GetDeltaTime(this.m_gui.gameObject.layer);
		this.m_gui.UpdateTimers(dt);
	}

	// Token: 0x06002441 RID: 9281 RVA: 0x0009644F File Offset: 0x0009484F
	public void SetRoundTimer(IClientRoundTimer _timer)
	{
		this.m_roundTimer = _timer;
	}

	// Token: 0x06002442 RID: 9282 RVA: 0x00096458 File Offset: 0x00094858
	public virtual void AddNewOrder(Serialisable _data)
	{
		ServerOrderData data = (ServerOrderData)_data;
		RecipeList.Entry entry = new RecipeList.Entry();
		entry.Copy(data.RecipeListEntry);
		RecipeFlowGUI.ElementToken token = this.m_gui.AddElement(entry.m_order, data.Lifetime, this.m_expiredDoNothingCallback);
		ClientOrderControllerBase.ActiveOrder item = new ClientOrderControllerBase.ActiveOrder(data.ID, entry, token);
		this.m_activeOrders.Add(item);
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x000964D0 File Offset: 0x000948D0
	public virtual void OnFoodDelivered(bool _success, OrderID _orderID)
	{
		if (_success)
		{
			ClientOrderControllerBase.ActiveOrder activeOrder = this.m_activeOrders.Find((ClientOrderControllerBase.ActiveOrder x) => x.ID == _orderID);
			if (activeOrder != null)
			{
				this.m_gui.RemoveElement(activeOrder.UIToken, new RecipeSuccessAnimation());
			}
			this.m_activeOrders.RemoveAll((ClientOrderControllerBase.ActiveOrder x) => x.ID == _orderID);
		}
		else
		{
			for (int i = 0; i < this.m_activeOrders.Count; i++)
			{
				this.m_gui.PlayAnimationOnElement(this.m_activeOrders[i].UIToken, new RecipeFailureAnimation());
			}
		}
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x00096580 File Offset: 0x00094980
	public virtual void OnOrderExpired(OrderID _orderID)
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.RecipeTimeOut, this.m_gui.gameObject.layer);
		ClientOrderControllerBase.ActiveOrder activeOrder = this.m_activeOrders.Find((ClientOrderControllerBase.ActiveOrder x) => x.ID == _orderID);
		if (activeOrder != null)
		{
			this.m_gui.PlayAnimationOnElement(activeOrder.UIToken, new RecipeFailureAnimation());
			this.m_gui.ResetElementTimer(activeOrder.UIToken);
		}
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x000965F8 File Offset: 0x000949F8
	public RecipeList.Entry GetRecipe(OrderID _orderID)
	{
		ClientOrderControllerBase.ActiveOrder activeOrder = this.m_activeOrders.Find((ClientOrderControllerBase.ActiveOrder x) => x.ID == _orderID);
		return activeOrder.RecipeListEntry;
	}

	// Token: 0x04001BAB RID: 7083
	protected List<ClientOrderControllerBase.ActiveOrder> m_activeOrders = new List<ClientOrderControllerBase.ActiveOrder>();

	// Token: 0x04001BAC RID: 7084
	private VoidGeneric<RecipeFlowGUI.ElementToken> m_expiredDoNothingCallback = delegate(RecipeFlowGUI.ElementToken _token)
	{
	};

	// Token: 0x04001BAD RID: 7085
	protected IClientRoundTimer m_roundTimer;

	// Token: 0x04001BAE RID: 7086
	protected RecipeFlowGUI m_gui;

	// Token: 0x04001BAF RID: 7087
	private bool m_enableOrderExpiration = true;

	// Token: 0x0200075A RID: 1882
	protected class ActiveOrder
	{
		// Token: 0x06002447 RID: 9287 RVA: 0x00096632 File Offset: 0x00094A32
		public ActiveOrder(OrderID _id, RecipeList.Entry _entry, RecipeFlowGUI.ElementToken _token)
		{
			this.ID = _id;
			this.UIToken = _token;
			this.RecipeListEntry = _entry;
		}

		// Token: 0x04001BB1 RID: 7089
		public OrderID ID;

		// Token: 0x04001BB2 RID: 7090
		public RecipeList.Entry RecipeListEntry;

		// Token: 0x04001BB3 RID: 7091
		public RecipeFlowGUI.ElementToken UIToken;
	}
}
