using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class ServerThrowableItem : ServerSynchroniserBase, IThrowable, IHandlePickup, IBaseHandlePickup
{
	// Token: 0x060027AA RID: 10154 RVA: 0x000BA5F8 File Offset: 0x000B89F8
	public override EntityType GetEntityType()
	{
		return EntityType.ThrowableItem;
	}

	// Token: 0x060027AB RID: 10155 RVA: 0x000BA5FC File Offset: 0x000B89FC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_throwableItem = (ThrowableItem)synchronisedObject;
		this.m_inFlight = new Generic<bool>(this.IsFlying);
		this.m_pickupReferral = base.gameObject.RequestComponent<ServerHandlePickupReferral>();
		ServerLimitedQuantityItem component = base.GetComponent<ServerLimitedQuantityItem>();
		if (null != component)
		{
			component.AddInvincibilityCondition(this.m_inFlight);
		}
	}

	// Token: 0x060027AC RID: 10156 RVA: 0x000BA65E File Offset: 0x000B8A5E
	private void SendStateMessage(bool _inFlight, GameObject _thrower)
	{
		this.m_data.Initialise(_inFlight, _thrower);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060027AD RID: 10157 RVA: 0x000BA67C File Offset: 0x000B8A7C
	protected virtual void Awake()
	{
		if (ServerThrowableItem.ms_AttachmentsLayer == 0)
		{
			ServerThrowableItem.ms_AttachmentsLayer = LayerMask.GetMask(new string[]
			{
				"Attachments"
			});
		}
		this.m_Collider = base.GetComponent<Collider>();
		this.m_Transform = base.transform;
		this.m_OnAttachChanged = new AttachChangedCallback(this.OnAttachChanged);
	}

	// Token: 0x060027AE RID: 10158 RVA: 0x000BA6D5 File Offset: 0x000B8AD5
	public void RegisterLandedCallback(GenericVoid<GameObject> _callback)
	{
		this.m_landedCallback = (GenericVoid<GameObject>)Delegate.Combine(this.m_landedCallback, _callback);
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000BA6EE File Offset: 0x000B8AEE
	public void UnregisterLandedCallback(GenericVoid<GameObject> _callback)
	{
		this.m_landedCallback = (GenericVoid<GameObject>)Delegate.Remove(this.m_landedCallback, _callback);
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000BA707 File Offset: 0x000B8B07
	public void RegisterCanThrowCallback(Generic<bool> _callback)
	{
		this.m_canThrowCallbacks.Add(_callback);
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000BA715 File Offset: 0x000B8B15
	public void UnregisterCanThrowCallback(Generic<bool> _callback)
	{
		this.m_canThrowCallbacks.Remove(_callback);
	}

	// Token: 0x060027B2 RID: 10162 RVA: 0x000BA724 File Offset: 0x000B8B24
	public bool CanHandleThrow(IThrower _thrower, Vector2 _directionXZ)
	{
		return !this.m_canThrowCallbacks.CallForResult(false);
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x000BA738 File Offset: 0x000B8B38
	public void HandleThrow(IThrower _thrower, Vector2 _directionXZ)
	{
		_thrower.ThrowItem(base.gameObject, _directionXZ);
		this.m_thrower = _thrower;
		this.m_previousThrower = _thrower;
		if (this.m_attachment == null)
		{
			this.m_attachment = base.gameObject.RequestInterface<IAttachment>();
		}
		if (this.m_attachment != null)
		{
			this.m_attachment.RegisterAttachChangedCallback(this.m_OnAttachChanged);
		}
		if (this.m_pickupReferral != null)
		{
			this.m_pickupReferral.SetHandlePickupReferree(this);
		}
		this.m_flying = true;
		this.SendStateMessage(this.m_flying, ((MonoBehaviour)_thrower).gameObject);
		this.ResumeAndClearThrowStartCollisions();
		this.m_ignoredCollidersCount = Physics.OverlapBoxNonAlloc(this.m_Transform.position, this.m_Collider.bounds.extents, this.m_ThrowStartColliders, this.m_Transform.rotation, ServerThrowableItem.ms_AttachmentsLayer);
		for (int i = 0; i < this.m_ignoredCollidersCount; i++)
		{
			Physics.IgnoreCollision(this.m_Collider, this.m_ThrowStartColliders[i], true);
		}
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x000BA844 File Offset: 0x000B8C44
	private void ResumeAndClearThrowStartCollisions()
	{
		for (int i = 0; i < this.m_ignoredCollidersCount; i++)
		{
			Collider collider = this.m_ThrowStartColliders[i];
			if (collider != null && collider.gameObject != null)
			{
				Physics.IgnoreCollision(this.m_Collider, collider, false);
				this.m_ThrowStartColliders[i] = null;
			}
		}
		this.m_ignoredCollidersCount = 0;
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x000BA8AC File Offset: 0x000B8CAC
	public override void UpdateSynchronising()
	{
		float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
		if (this.IsFlying())
		{
			this.m_flightTimer += deltaTime;
			if (this.m_flightTimer >= 0.1f)
			{
				IAttachment component = base.GetComponent<IAttachment>();
				if (component.AccessMotion().GetVelocity().sqrMagnitude < 0.0225f)
				{
					this.EndFlight();
				}
				if (this.m_flightTimer >= 5f)
				{
					this.EndFlight();
				}
				this.ResumeAndClearThrowStartCollisions();
			}
		}
		else
		{
			this.m_throwerTimer -= deltaTime;
			if (this.m_throwerTimer <= 0f)
			{
				this.m_previousThrower = null;
			}
		}
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x000BA960 File Offset: 0x000B8D60
	private void OnCollisionEnter(Collision _collision)
	{
		if (!this.IsFlying())
		{
			return;
		}
		IThrower thrower = _collision.gameObject.RequestInterface<IThrower>();
		if (thrower != null && thrower == this.m_thrower)
		{
			return;
		}
		Vector3 normal = _collision.contacts[0].normal;
		float num = Vector3.Angle(normal, Vector3.up);
		if (num <= 45f)
		{
			this.EndFlight();
			this.m_landedCallback(_collision.gameObject);
		}
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x000BA9D8 File Offset: 0x000B8DD8
	private void OnAttachChanged(IParentable _parentable)
	{
		if (_parentable != null)
		{
			this.EndFlight();
		}
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x000BA9E8 File Offset: 0x000B8DE8
	public void EndFlight()
	{
		if (this.m_pickupReferral != null && this.m_pickupReferral.GetHandlePickupReferree() == this)
		{
			this.m_pickupReferral.SetHandlePickupReferree(null);
		}
		if (this.m_attachment != null)
		{
			this.m_attachment.UnregisterAttachChangedCallback(this.m_OnAttachChanged);
		}
		this.m_previousThrower = this.m_thrower;
		this.m_throwerTimer = this.m_throwableItem.m_throwerTimeout;
		this.m_thrower = null;
		this.m_flightTimer = 0f;
		this.m_flying = false;
		this.SendStateMessage(this.m_flying, null);
		this.ResumeAndClearThrowStartCollisions();
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x000BAA88 File Offset: 0x000B8E88
	public bool IsFlying()
	{
		return this.m_flying;
	}

	// Token: 0x060027BA RID: 10170 RVA: 0x000BAA90 File Offset: 0x000B8E90
	public float GetFlightTime()
	{
		return this.m_flightTimer;
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x000BAA98 File Offset: 0x000B8E98
	public IThrower GetThrower()
	{
		return this.m_thrower;
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000BAAA0 File Offset: 0x000B8EA0
	public IThrower GetPreviousThrower()
	{
		return this.m_previousThrower;
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x000BAAA8 File Offset: 0x000B8EA8
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return !this.m_flying;
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000BAAB3 File Offset: 0x000B8EB3
	public int GetPickupPriority()
	{
		return int.MinValue;
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000BAABA File Offset: 0x000B8EBA
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
	}

	// Token: 0x04001F2D RID: 7981
	private ThrowableItem m_throwableItem;

	// Token: 0x04001F2E RID: 7982
	private static int ms_AttachmentsLayer;

	// Token: 0x04001F2F RID: 7983
	private ThrowableItemMessage m_data = new ThrowableItemMessage();

	// Token: 0x04001F30 RID: 7984
	private const float c_minFlightTime = 0.1f;

	// Token: 0x04001F31 RID: 7985
	private const float c_maxFlightTime = 5f;

	// Token: 0x04001F32 RID: 7986
	private const float c_minVelocity = 0.15f;

	// Token: 0x04001F33 RID: 7987
	private const float c_minVelocitySqr = 0.0225f;

	// Token: 0x04001F34 RID: 7988
	private const float c_horizCollisionTolerance = 45f;

	// Token: 0x04001F35 RID: 7989
	private const float c_bounceAngleMax = 80f;

	// Token: 0x04001F36 RID: 7990
	private IAttachment m_attachment;

	// Token: 0x04001F37 RID: 7991
	private IThrower m_thrower;

	// Token: 0x04001F38 RID: 7992
	private float m_flightTimer;

	// Token: 0x04001F39 RID: 7993
	private bool m_flying;

	// Token: 0x04001F3A RID: 7994
	private IThrower m_previousThrower;

	// Token: 0x04001F3B RID: 7995
	private float m_throwerTimer;

	// Token: 0x04001F3C RID: 7996
	private ServerHandlePickupReferral m_pickupReferral;

	// Token: 0x04001F3D RID: 7997
	private AttachChangedCallback m_OnAttachChanged;

	// Token: 0x04001F3E RID: 7998
	private Generic<bool> m_inFlight;

	// Token: 0x04001F3F RID: 7999
	private GenericVoid<GameObject> m_landedCallback = delegate(GameObject _object)
	{
	};

	// Token: 0x04001F40 RID: 8000
	private List<Generic<bool>> m_canThrowCallbacks = new List<Generic<bool>>();

	// Token: 0x04001F41 RID: 8001
	private Collider[] m_ThrowStartColliders = new Collider[4];

	// Token: 0x04001F42 RID: 8002
	private Collider m_Collider;

	// Token: 0x04001F43 RID: 8003
	private int m_ignoredCollidersCount;

	// Token: 0x04001F44 RID: 8004
	private Transform m_Transform;
}
