using System;
using System.Diagnostics;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class ClientCookingHandler : ClientSynchroniserBase, IClientCookable, IClientCookingRegionNotified, IBaseCookable
{
	// Token: 0x14000015 RID: 21
	// (add) Token: 0x0600155A RID: 5466 RVA: 0x00074098 File Offset: 0x00072498
	// (remove) Token: 0x0600155B RID: 5467 RVA: 0x000740D0 File Offset: 0x000724D0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private event CookingStateChanged m_cookingStateChangedCallback = delegate(CookingUIController.State _state)
	{
	};

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600155C RID: 5468 RVA: 0x00074106 File Offset: 0x00072506
	// (remove) Token: 0x0600155D RID: 5469 RVA: 0x00074113 File Offset: 0x00072513
	public static event VoidGeneric<ClientCookingHandler> OnBurntCookableAdded
	{
		add
		{
			ClientCookingHandler.s_burntCookables.OnObjectAdded += value;
		}
		remove
		{
			ClientCookingHandler.s_burntCookables.OnObjectAdded -= value;
		}
	}

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x0600155E RID: 5470 RVA: 0x00074120 File Offset: 0x00072520
	// (remove) Token: 0x0600155F RID: 5471 RVA: 0x0007412D File Offset: 0x0007252D
	public static event VoidGeneric<ClientCookingHandler> OnBurntCookableRemoved
	{
		add
		{
			ClientCookingHandler.s_burntCookables.OnObjectRemoved += value;
		}
		remove
		{
			ClientCookingHandler.s_burntCookables.OnObjectRemoved -= value;
		}
	}

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06001560 RID: 5472 RVA: 0x0007413A File Offset: 0x0007253A
	// (remove) Token: 0x06001561 RID: 5473 RVA: 0x00074143 File Offset: 0x00072543
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

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06001562 RID: 5474 RVA: 0x0007414C File Offset: 0x0007254C
	public float AccessCookingTime
	{
		get
		{
			return this.GetCookingHandler().m_cookingtime;
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06001563 RID: 5475 RVA: 0x00074159 File Offset: 0x00072559
	public CookingStepData AccessCookingType
	{
		get
		{
			return this.GetCookingHandler().m_cookingType;
		}
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x00074168 File Offset: 0x00072568
	public override void StartSynchronising(Component synchronisedObject)
	{
		GameObject gameObject = GameUtils.InstantiateUIController(this.GetCookingHandler().m_cookingUIPrefab.gameObject, "HoverIconCanvas");
		this.m_gui = gameObject.GetComponent<CookingUIController>();
		this.m_gui.SetFollowTransform(NetworkUtils.FindVisualRoot(base.gameObject), Vector3.zero);
		this.m_gui.SetFollowOffset(this.GetCookingHandler().m_cookingUIPrefabOffset);
		this.m_ICookingNotified = base.gameObject.RequestInterfacesRecursive<IClientCookingNotifed>();
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000741DE File Offset: 0x000725DE
	public override EntityType GetEntityType()
	{
		return EntityType.CookingState;
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x000741E4 File Offset: 0x000725E4
	public override void ApplyServerUpdate(Serialisable serialisable)
	{
		CookingHandler cookingHandler = this.GetCookingHandler();
		CookingStateMessage cookingStateMessage = (CookingStateMessage)serialisable;
		bool flag = false;
		if (this.m_cachedCookingProgress != cookingStateMessage.m_cookingProgress)
		{
			this.m_cachedCookingProgress = cookingStateMessage.m_cookingProgress;
			flag = true;
		}
		if (this.m_cachedCookingState != cookingStateMessage.m_cookingState)
		{
			if (this.m_cachedCookingState == CookingUIController.State.Ruined)
			{
				ClientCookingHandler.s_burntCookables.Remove(this);
			}
			this.m_cachedCookingState = cookingStateMessage.m_cookingState;
			flag = true;
			this.m_cookingStateChangedCallback(this.m_cachedCookingState);
			if (this.m_cachedCookingState == CookingUIController.State.Ruined)
			{
				ClientCookingHandler.s_burntCookables.Add(this);
			}
		}
		if (flag)
		{
			this.m_ICookingNotified = base.gameObject.RequestInterfacesRecursive<IClientCookingNotifed>();
			float num = cookingStateMessage.m_cookingProgress / cookingHandler.m_cookingtime;
			for (int i = 0; i < this.m_ICookingNotified.Length; i++)
			{
				IClientCookingNotifed clientCookingNotifed = this.m_ICookingNotified[i];
				clientCookingNotifed.OnCookingPropChanged(num);
			}
			if (cookingStateMessage.m_cookingProgress > 0.001f)
			{
				if (this.m_gui != null)
				{
					this.m_gui.gameObject.SetActive(base.gameObject.activeInHierarchy);
					this.m_gui.SetProgress(num);
					float b = Mathf.Clamp(2f * cookingHandler.m_cookingtime - 0.5f, cookingHandler.m_cookingtime, 2f * cookingHandler.m_cookingtime);
					this.m_gui.SetOverDoingAmount(MathUtils.ClampedRemap(cookingStateMessage.m_cookingProgress, cookingHandler.m_cookingtime, b, 0f, 1f));
				}
				if (this.m_isInCookingRegion)
				{
					this.m_hideAfterTime = 1f;
					if (this.m_gui != null)
					{
						this.m_gui.SetState(cookingStateMessage.m_cookingState);
					}
					for (int j = 0; j < this.m_ICookingNotified.Length; j++)
					{
						IClientCookingNotifed clientCookingNotifed2 = this.m_ICookingNotified[j];
						clientCookingNotifed2.OnCookingStarted();
					}
				}
				else
				{
					this.m_hideAfterTime = 0f;
					if (this.m_gui != null)
					{
						if (this.m_cachedCookingState != CookingUIController.State.Progressing)
						{
							this.m_gui.SetState(CookingUIController.State.Idle);
						}
						else
						{
							this.m_gui.SetState(cookingStateMessage.m_cookingState);
						}
					}
				}
			}
			else
			{
				if (this.m_gui != null)
				{
					this.m_gui.gameObject.SetActive(false);
				}
				this.m_hideAfterTime = float.MinValue;
				for (int k = 0; k < this.m_ICookingNotified.Length; k++)
				{
					IClientCookingNotifed clientCookingNotifed3 = this.m_ICookingNotified[k];
					clientCookingNotifed3.OnCookingFinished();
				}
			}
		}
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x00074485 File Offset: 0x00072885
	public CookedCompositeOrderNode.CookingProgress GetCookedOrderState()
	{
		return this.GetCookingHandler().GetCookedOrderState(this.m_cachedCookingProgress);
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x00074498 File Offset: 0x00072898
	public float GetCookingProgress()
	{
		return this.m_cachedCookingProgress;
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x000744A0 File Offset: 0x000728A0
	public GameLoopingAudioTag GetSizzleSoundTag()
	{
		return this.GetCookingHandler().m_cookingType.m_sizzleSound;
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x000744B2 File Offset: 0x000728B2
	private CookingHandler GetCookingHandler()
	{
		if (this.m_cookingHandler == null)
		{
			this.m_cookingHandler = base.gameObject.RequireComponent<CookingHandler>();
		}
		return this.m_cookingHandler;
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x000744DC File Offset: 0x000728DC
	public override void UpdateSynchronising()
	{
		float hideAfterTime = this.m_hideAfterTime;
		this.m_hideAfterTime -= TimeManager.GetDeltaTime(base.gameObject);
		if (hideAfterTime >= 0f && this.m_hideAfterTime < 0f)
		{
			if (this.m_cachedCookingState == CookingUIController.State.OverDoing && this.m_gui != null)
			{
				this.m_gui.SetState(CookingUIController.State.Idle);
			}
			for (int i = 0; i < this.m_ICookingNotified.Length; i++)
			{
				IClientCookingNotifed clientCookingNotifed = this.m_ICookingNotified[i];
				clientCookingNotifed.OnCookingFinished();
			}
		}
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x00074574 File Offset: 0x00072974
	public bool IsBurning()
	{
		return this.GetCookingProgress() > 2f * this.AccessCookingTime;
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x0007458A File Offset: 0x0007298A
	public CookingStationType GetRequiredStationType()
	{
		return this.GetCookingHandler().m_stationType;
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x00074597 File Offset: 0x00072997
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_gui != null)
		{
			this.m_gui.SetState(CookingUIController.State.Idle);
			this.m_gui.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x000745CD File Offset: 0x000729CD
	public void EnterCookingRegion()
	{
		this.m_isInCookingRegion = true;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x000745D6 File Offset: 0x000729D6
	public void ExitCookingRegion()
	{
		this.m_isInCookingRegion = false;
	}

	// Token: 0x04001058 RID: 4184
	private CookingHandler m_cookingHandler;

	// Token: 0x04001059 RID: 4185
	private IClientCookingNotifed[] m_ICookingNotified;

	// Token: 0x0400105A RID: 4186
	private CookingUIController m_gui;

	// Token: 0x0400105B RID: 4187
	private float m_hideAfterTime = float.MinValue;

	// Token: 0x0400105C RID: 4188
	private float m_cachedCookingProgress = -1f;

	// Token: 0x0400105D RID: 4189
	private CookingUIController.State m_cachedCookingState;

	// Token: 0x0400105F RID: 4191
	private static StaticList<ClientCookingHandler> s_burntCookables = new StaticList<ClientCookingHandler>();

	// Token: 0x04001060 RID: 4192
	private bool m_isInCookingRegion;
}
