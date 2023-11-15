using System;
using System.Collections.Generic;
using System.Diagnostics;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200047C RID: 1148
public class ServerCookingHandler : ServerSynchroniserBase, ICookable, IBaseCookable
{
	// Token: 0x06001543 RID: 5443 RVA: 0x00073DD8 File Offset: 0x000721D8
	public static IEnumerable<ServerCookingHandler> GetCookingHandlers()
	{
		return ServerCookingHandler.s_cookingHandlers;
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06001544 RID: 5444 RVA: 0x00073DE0 File Offset: 0x000721E0
	// (remove) Token: 0x06001545 RID: 5445 RVA: 0x00073E18 File Offset: 0x00072218
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event CookingStateChanged m_cookingStateChangedCallback = delegate(CookingUIController.State _state)
	{
	};

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06001546 RID: 5446 RVA: 0x00073E4E File Offset: 0x0007224E
	// (remove) Token: 0x06001547 RID: 5447 RVA: 0x00073E57 File Offset: 0x00072257
	public event CookingStateChanged CookingStateChangedCallback
	{
		add
		{
			this.m_cookingStateChangedCallback += value;
		}
		remove
		{
			this.m_cookingStateChangedCallback -= value;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06001548 RID: 5448 RVA: 0x00073E60 File Offset: 0x00072260
	public float AccessCookingTime
	{
		get
		{
			return this.GetCookingHandler().m_cookingtime;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06001549 RID: 5449 RVA: 0x00073E6D File Offset: 0x0007226D
	public CookingStepData AccessCookingType
	{
		get
		{
			return this.GetCookingHandler().m_cookingType;
		}
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00073E7A File Offset: 0x0007227A
	public override void StartSynchronising(Component synchronisedObject)
	{
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00073E7C File Offset: 0x0007227C
	public override EntityType GetEntityType()
	{
		return EntityType.CookingState;
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x00073E80 File Offset: 0x00072280
	public override Serialisable GetServerUpdate()
	{
		return this.m_ServerData;
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x00073E88 File Offset: 0x00072288
	public CookedCompositeOrderNode.CookingProgress GetCookedOrderState()
	{
		return this.GetCookingHandler().GetCookedOrderState(this.m_ServerData.m_cookingProgress);
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x00073EA0 File Offset: 0x000722A0
	public CookingStationType GetRequiredStationType()
	{
		return this.GetCookingHandler().m_stationType;
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00073EAD File Offset: 0x000722AD
	public bool IsCooked()
	{
		return this.m_ServerData.m_cookingProgress >= this.AccessCookingTime;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x00073EC5 File Offset: 0x000722C5
	public bool IsBurning()
	{
		return this.m_ServerData.m_cookingProgress > 2f * this.AccessCookingTime;
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x00073EE0 File Offset: 0x000722E0
	public float GetCookingProgress()
	{
		return this.m_ServerData.m_cookingProgress;
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x00073EF0 File Offset: 0x000722F0
	public void SetCookingProgress(float _cookingProgress)
	{
		CookingHandler cookingHandler = this.GetCookingHandler();
		this.m_ServerData.m_cookingProgress = _cookingProgress;
		CookingUIController.State cookingState = this.m_ServerData.m_cookingState;
		if (this.IsBurning())
		{
			this.m_ServerData.m_cookingState = CookingUIController.State.Ruined;
		}
		else if (_cookingProgress >= cookingHandler.m_cookingtime)
		{
			if (this.m_ServerData.m_cookingState == CookingUIController.State.Progressing)
			{
				this.m_ServerData.m_cookingState = CookingUIController.State.Completed;
			}
			if (_cookingProgress > 1.3f * cookingHandler.m_cookingtime)
			{
				this.m_ServerData.m_cookingState = CookingUIController.State.OverDoing;
			}
		}
		else
		{
			this.m_ServerData.m_cookingState = CookingUIController.State.Progressing;
		}
		if (this.m_ServerData.m_cookingState != cookingState)
		{
			this.m_cookingStateChangedCallback(this.m_ServerData.m_cookingState);
		}
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x00073FB7 File Offset: 0x000723B7
	public bool Cook(float _cookingDeltatTime)
	{
		if (!this.IsBurning())
		{
			this.SetCookingProgress(this.m_ServerData.m_cookingProgress + _cookingDeltatTime);
			return true;
		}
		return false;
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x00073FDA File Offset: 0x000723DA
	public CookingHandler GetCookingHandler()
	{
		if (this.m_cookingHandler == null)
		{
			this.m_cookingHandler = base.gameObject.RequireComponent<CookingHandler>();
		}
		return this.m_cookingHandler;
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x00074004 File Offset: 0x00072404
	protected override void OnEnable()
	{
		base.OnEnable();
		ServerCookingHandler.s_cookingHandlers.Add(this);
		this.SetCookingProgress(this.m_ServerData.m_cookingProgress);
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00074028 File Offset: 0x00072428
	protected override void OnDisable()
	{
		base.OnDisable();
		ServerCookingHandler.s_cookingHandlers.Remove(this);
	}

	// Token: 0x04001053 RID: 4179
	private static List<ServerCookingHandler> s_cookingHandlers = new List<ServerCookingHandler>();

	// Token: 0x04001054 RID: 4180
	private CookingHandler m_cookingHandler;

	// Token: 0x04001055 RID: 4181
	private CookingStateMessage m_ServerData = new CookingStateMessage();
}
