using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000819 RID: 2073
public class ClientThrowableItem : ClientSynchroniserBase, IClientThrowable, IClientHandlePickup, IBaseHandlePickup
{
	// Token: 0x060027C3 RID: 10179 RVA: 0x000BAAD3 File Offset: 0x000B8ED3
	public override EntityType GetEntityType()
	{
		return EntityType.ThrowableItem;
	}

	// Token: 0x060027C4 RID: 10180 RVA: 0x000BAAD7 File Offset: 0x000B8ED7
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_throwableItem = (ThrowableItem)synchronisedObject;
		this.m_pickupReferral = base.gameObject.RequestComponent<ClientHandlePickupReferral>();
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x000BAB00 File Offset: 0x000B8F00
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		ThrowableItemMessage throwableItemMessage = (ThrowableItemMessage)serialisable;
		if (throwableItemMessage.m_inFlight)
		{
			IClientThrower thrower = throwableItemMessage.m_thrower.RequestInterface<IClientThrower>();
			this.StartFlight(thrower);
		}
		else
		{
			this.EndFlight();
		}
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x000BAB3D File Offset: 0x000B8F3D
	public void RegisterCanThrowCallback(Generic<bool> _callback)
	{
		this.m_canThrowCallbacks.Add(_callback);
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x000BAB4B File Offset: 0x000B8F4B
	public void UnregisterCanThrowCallback(Generic<bool> _callback)
	{
		this.m_canThrowCallbacks.Remove(_callback);
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000BAB5A File Offset: 0x000B8F5A
	public bool CanHandleThrow(IClientThrower _thrower, Vector2 _directionXZ)
	{
		return !this.m_canThrowCallbacks.CallForResult(false);
	}

	// Token: 0x060027C9 RID: 10185 RVA: 0x000BAB6B File Offset: 0x000B8F6B
	public bool IsFlying()
	{
		return this.m_isFlying;
	}

	// Token: 0x060027CA RID: 10186 RVA: 0x000BAB73 File Offset: 0x000B8F73
	public IClientThrower GetThrower()
	{
		return this.m_thrower;
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000BAB7C File Offset: 0x000B8F7C
	private void StartFlight(IClientThrower _thrower)
	{
		this.m_isFlying = true;
		this.m_thrower = _thrower;
		if (this.m_pickupReferral != null)
		{
			this.m_pickupReferral.SetHandlePickupReferree(this);
		}
		if (this.m_throwableItem.m_throwParticle != null)
		{
			Transform parent = NetworkUtils.FindVisualRoot(base.gameObject);
			GameObject gameObject = this.m_throwableItem.m_throwParticle.InstantiateOnParent(parent, true);
			this.m_pfx = gameObject.GetComponent<ParticleSystem>();
		}
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000BABF8 File Offset: 0x000B8FF8
	private void EndFlight()
	{
		this.m_isFlying = false;
		this.m_thrower = null;
		if (this.m_pickupReferral != null && this.m_pickupReferral.GetHandlePickupReferree() == this)
		{
			this.m_pickupReferral.SetHandlePickupReferree(null);
		}
		if (this.m_pfx != null)
		{
			this.m_pfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		}
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000BAC5F File Offset: 0x000B905F
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return !this.m_isFlying;
	}

	// Token: 0x060027CE RID: 10190 RVA: 0x000BAC6A File Offset: 0x000B906A
	public int GetPickupPriority()
	{
		return int.MinValue;
	}

	// Token: 0x04001F46 RID: 8006
	private ThrowableItem m_throwableItem;

	// Token: 0x04001F47 RID: 8007
	private IClientThrower m_thrower;

	// Token: 0x04001F48 RID: 8008
	private bool m_isFlying;

	// Token: 0x04001F49 RID: 8009
	private ParticleSystem m_pfx;

	// Token: 0x04001F4A RID: 8010
	private ClientHandlePickupReferral m_pickupReferral;

	// Token: 0x04001F4B RID: 8011
	private List<Generic<bool>> m_canThrowCallbacks = new List<Generic<bool>>();
}
