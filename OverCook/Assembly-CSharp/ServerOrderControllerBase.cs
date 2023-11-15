using System;
using System.Collections.Generic;
using OrderController;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public abstract class ServerOrderControllerBase
{
	// Token: 0x06002423 RID: 9251 RVA: 0x00095C14 File Offset: 0x00094014
	public ServerOrderControllerBase(RoundDataBase _data, int _maxOrders, VoidGeneric<OrderID> _addedCallback, VoidGeneric<OrderID> _timeoutCallback)
	{
		this.m_roundData = _data;
		this.m_roundInstanceData = _data.InitialiseRound();
		this.m_maxOrdersAllowed = _maxOrders;
		this.m_layer = LayerMask.NameToLayer("Default");
		this.m_orderAddedCallback = _addedCallback;
		this.m_orderTimeoutCallback = _timeoutCallback;
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06002424 RID: 9252 RVA: 0x00095CC6 File Offset: 0x000940C6
	public List<RecipeList.Entry> ActiveRecipes
	{
		get
		{
			return this.m_activeOrders.ConvertAll<RecipeList.Entry>((ServerOrderData x) => x.RecipeListEntry);
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06002425 RID: 9253 RVA: 0x00095CF0 File Offset: 0x000940F0
	// (set) Token: 0x06002426 RID: 9254 RVA: 0x00095CF8 File Offset: 0x000940F8
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

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06002427 RID: 9255 RVA: 0x00095D01 File Offset: 0x00094101
	protected RoundInstanceDataBase AccessRoundInstanceData
	{
		get
		{
			return this.m_roundInstanceData;
		}
	}

	// Token: 0x06002428 RID: 9256 RVA: 0x00095D0C File Offset: 0x0009410C
	public virtual void Update()
	{
		float num = (this.m_roundTimer == null || this.m_roundTimer.IsSuppressed || !this.m_enableOrderExpiration) ? 0f : TimeManager.GetDeltaTime(this.m_layer);
		for (int i = 0; i < this.m_activeOrders.Count; i++)
		{
			ServerOrderData serverOrderData = this.m_activeOrders[i];
			if (serverOrderData.Remaining > 0f)
			{
				serverOrderData.Remaining -= num;
				if (serverOrderData.Remaining <= 0f)
				{
					this.m_orderTimeoutCallback(serverOrderData.ID);
				}
			}
		}
		this.m_timerUntilOrder -= num;
		if (this.m_autoProgress && !this.IsFull() && (this.m_timerUntilOrder < 0f || this.m_activeOrders.Count < 2))
		{
			this.AddNewOrder();
			this.m_timerUntilOrder = this.GetNextTimeBetweenOrders();
		}
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x00095E14 File Offset: 0x00094214
	protected void ResetOrderTimer()
	{
		this.m_timerUntilOrder = 0f;
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x00095E21 File Offset: 0x00094221
	protected bool IsEmpty()
	{
		return this.m_activeOrders.Count == 0;
	}

	// Token: 0x0600242B RID: 9259 RVA: 0x00095E31 File Offset: 0x00094231
	protected bool IsFull()
	{
		return this.m_activeOrders.Count >= this.m_maxOrdersAllowed;
	}

	// Token: 0x0600242C RID: 9260 RVA: 0x00095E49 File Offset: 0x00094249
	public void SetAutoProgress(bool _autoProgress)
	{
		this.m_autoProgress = _autoProgress;
	}

	// Token: 0x0600242D RID: 9261 RVA: 0x00095E52 File Offset: 0x00094252
	public void SetRoundTimer(IServerRoundTimer _timer)
	{
		this.m_roundTimer = _timer;
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x00095E5C File Offset: 0x0009425C
	protected virtual ServerOrderData AddNewOrder(RecipeList.Entry _entry)
	{
		ServerOrderData serverOrderData = new ServerOrderData(new OrderID(this.m_nextOrderID++), _entry, this.GetNextOrderLifetime());
		this.m_activeOrders.Add(serverOrderData);
		this.m_orderAddedCallback(serverOrderData.ID);
		return serverOrderData;
	}

	// Token: 0x0600242F RID: 9263 RVA: 0x00095EAC File Offset: 0x000942AC
	public void AddNewOrder()
	{
		RecipeList.Entry[] nextRecipe = this.m_roundData.GetNextRecipe(this.m_roundInstanceData);
		for (int i = 0; i < nextRecipe.Length; i++)
		{
			this.AddNewOrder(nextRecipe[i]);
		}
	}

	// Token: 0x06002430 RID: 9264 RVA: 0x00095EEC File Offset: 0x000942EC
	public void RemoveOrder(OrderID _orderID)
	{
		int index = this.m_activeOrders.FindIndex((ServerOrderData x) => x.ID == _orderID);
		this.m_activeOrders.RemoveAt(index);
	}

	// Token: 0x06002431 RID: 9265 RVA: 0x00095F2C File Offset: 0x0009432C
	public void ResetOrderLifetime(OrderID _orderID)
	{
		ServerOrderData serverOrderData = this.m_activeOrders.Find((ServerOrderData x) => x.ID == _orderID);
		serverOrderData.Remaining = serverOrderData.Lifetime;
	}

	// Token: 0x06002432 RID: 9266
	protected abstract float GetNextOrderLifetime();

	// Token: 0x06002433 RID: 9267
	protected abstract float GetNextTimeBetweenOrders();

	// Token: 0x06002434 RID: 9268 RVA: 0x00095F6A File Offset: 0x0009436A
	protected bool Matches(OrderDefinitionNode _required, AssembledDefinitionNode _provided, PlatingStepData _plateType)
	{
		if (_required.m_platingStep != _plateType)
		{
			return false;
		}
		if (_required.GetType() == typeof(WildcardOrderNode))
		{
			return AssembledDefinitionNode.Matching(_required, _provided);
		}
		return AssembledDefinitionNode.Matching(_provided, _required);
	}

	// Token: 0x06002435 RID: 9269 RVA: 0x00095FA4 File Offset: 0x000943A4
	public bool FindBestOrderForRecipe(AssembledDefinitionNode _order, PlatingStepData _plateType, out OrderID o_orderID, out float _timePropRemainingPercentage)
	{
		o_orderID = new OrderID(0U);
		_timePropRemainingPercentage = 0f;
		List<ServerOrderData> list = this.m_activeOrders.FindAll((ServerOrderData x) => this.Matches(x.RecipeListEntry.m_order, _order, _plateType));
		ServerOrderData value = list.ToArray().FindLowestScoring((ServerOrderData x) => x.Remaining).Value;
		if (value != null)
		{
			o_orderID = value.ID;
			_timePropRemainingPercentage = Mathf.Clamp01(value.Remaining / value.Lifetime);
			return true;
		}
		return false;
	}

	// Token: 0x06002436 RID: 9270 RVA: 0x00096050 File Offset: 0x00094450
	public bool IsComboOrder(OrderID _orderID, bool _restart)
	{
		if (_restart)
		{
			ServerOrderData serverOrderData = this.m_activeOrders.Find((ServerOrderData x) => x.ID == _orderID);
			if (serverOrderData != null)
			{
				this.m_comboIndex = this.m_activeOrders.IndexOf(serverOrderData);
				return this.m_comboIndex == 0;
			}
			return false;
		}
		else
		{
			if (this.m_comboIndex >= 0 && this.m_comboIndex < this.m_activeOrders.Count)
			{
				ServerOrderData serverOrderData2 = this.m_activeOrders[this.m_comboIndex];
				return serverOrderData2.ID == _orderID;
			}
			return false;
		}
	}

	// Token: 0x06002437 RID: 9271 RVA: 0x000960FC File Offset: 0x000944FC
	public RecipeList.Entry GetRecipe(OrderID _orderID)
	{
		ServerOrderData serverOrderData = this.m_activeOrders.Find((ServerOrderData x) => x.ID == _orderID);
		return serverOrderData.RecipeListEntry;
	}

	// Token: 0x06002438 RID: 9272 RVA: 0x00096134 File Offset: 0x00094534
	public Serialisable GetSerialisedOrderData(OrderID _orderID)
	{
		ServerOrderData result = null;
		for (int i = 0; i < this.m_activeOrders.Count; i++)
		{
			ServerOrderData serverOrderData = this.m_activeOrders[i];
			if (serverOrderData.ID == _orderID)
			{
				result = serverOrderData;
				break;
			}
		}
		return result;
	}

	// Token: 0x04001B9A RID: 7066
	private uint m_nextOrderID = 1U;

	// Token: 0x04001B9B RID: 7067
	protected List<ServerOrderData> m_activeOrders = new List<ServerOrderData>();

	// Token: 0x04001B9C RID: 7068
	protected IServerRoundTimer m_roundTimer;

	// Token: 0x04001B9D RID: 7069
	private RoundDataBase m_roundData;

	// Token: 0x04001B9E RID: 7070
	private RoundInstanceDataBase m_roundInstanceData;

	// Token: 0x04001B9F RID: 7071
	private int m_maxOrdersAllowed;

	// Token: 0x04001BA0 RID: 7072
	protected int m_layer;

	// Token: 0x04001BA1 RID: 7073
	protected float m_timerUntilOrder;

	// Token: 0x04001BA2 RID: 7074
	protected bool m_autoProgress = true;

	// Token: 0x04001BA3 RID: 7075
	private int m_comboIndex;

	// Token: 0x04001BA4 RID: 7076
	private bool m_enableOrderExpiration = true;

	// Token: 0x04001BA5 RID: 7077
	private VoidGeneric<OrderID> m_orderAddedCallback = delegate(OrderID _orderID)
	{
	};

	// Token: 0x04001BA6 RID: 7078
	protected VoidGeneric<OrderID> m_orderTimeoutCallback = delegate(OrderID _orderID)
	{
	};
}
