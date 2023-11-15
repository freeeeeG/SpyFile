using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009CE RID: 2510
public class ServerAttachmentThrower : ServerSynchroniserBase, IThrower
{
	// Token: 0x0600312D RID: 12589 RVA: 0x000E6A3F File Offset: 0x000E4E3F
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_thrower = (AttachmentThrower)synchronisedObject;
	}

	// Token: 0x0600312E RID: 12590 RVA: 0x000E6A54 File Offset: 0x000E4E54
	public void RegisterThrowCallback(GenericVoid<GameObject> _callback)
	{
		this.m_throwCallback = (GenericVoid<GameObject>)Delegate.Combine(this.m_throwCallback, _callback);
	}

	// Token: 0x0600312F RID: 12591 RVA: 0x000E6A6D File Offset: 0x000E4E6D
	public void UnregisterThrowCallback(GenericVoid<GameObject> _callback)
	{
		this.m_throwCallback = (GenericVoid<GameObject>)Delegate.Remove(this.m_throwCallback, _callback);
	}

	// Token: 0x06003130 RID: 12592 RVA: 0x000E6A86 File Offset: 0x000E4E86
	private void Awake()
	{
		ServerAttachmentThrower.m_playersLayerMask = LayerMask.GetMask(new string[]
		{
			"Players"
		});
	}

	// Token: 0x06003131 RID: 12593 RVA: 0x000E6AA0 File Offset: 0x000E4EA0
	public void ThrowItem(GameObject _object, Vector2 _directionXZ)
	{
		IAttachment attachment = _object.RequestInterface<IAttachment>();
		if (attachment != null)
		{
			Vector3 velocity = this.CalculateThrowVelocity(_directionXZ);
			attachment.AccessMotion().SetVelocity(velocity);
			ICatchable catchable = _object.RequestInterface<ICatchable>();
			if (catchable != null)
			{
				this.AlertPotentialCatchers(catchable, attachment.AccessGameObject().transform.position, _directionXZ);
			}
		}
		this.m_throwCallback(_object);
	}

	// Token: 0x06003132 RID: 12594 RVA: 0x000E6B00 File Offset: 0x000E4F00
	private Vector3 CalculateThrowVelocity(Vector2 _directionXZ)
	{
		Vector3 zero = Vector3.zero;
		zero.x = _directionXZ.x * this.m_thrower.m_throwForce;
		zero.z = _directionXZ.y * this.m_thrower.m_throwForce;
		zero.y = Mathf.Tan(0.017453292f * this.m_thrower.m_throwInclination) * this.m_thrower.m_throwForce;
		return zero;
	}

	// Token: 0x06003133 RID: 12595 RVA: 0x000E6B74 File Offset: 0x000E4F74
	private void AlertPotentialCatchers(ICatchable _object, Vector3 _position, Vector2 _directionXZ)
	{
		Vector3 direction = VectorUtils.FromXZ(_directionXZ, 0f);
		int num = Physics.SphereCastNonAlloc(_position, 1f, direction, ServerAttachmentThrower.ms_raycastHits, 10f, ServerAttachmentThrower.m_playersLayerMask);
		for (int i = 0; i < num; i++)
		{
			RaycastHit raycastHit = ServerAttachmentThrower.ms_raycastHits[i];
			GameObject gameObject = raycastHit.collider.gameObject;
			if (!(gameObject == base.gameObject))
			{
				IHandleCatch handleCatch = gameObject.RequestInterface<IHandleCatch>();
				if (handleCatch != null)
				{
					handleCatch.AlertToThrownItem(_object, this, _directionXZ);
				}
			}
		}
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x000E6C0B File Offset: 0x000E500B
	public void OnFailedToThrowItem(GameObject _object)
	{
	}

	// Token: 0x04002767 RID: 10087
	private AttachmentThrower m_thrower;

	// Token: 0x04002768 RID: 10088
	private const float c_alertRaycastRadius = 1f;

	// Token: 0x04002769 RID: 10089
	private const float c_alertRaycastDistance = 10f;

	// Token: 0x0400276A RID: 10090
	private const int kRaycastHitsMax = 10;

	// Token: 0x0400276B RID: 10091
	private static RaycastHit[] ms_raycastHits = new RaycastHit[10];

	// Token: 0x0400276C RID: 10092
	private static int m_playersLayerMask = 0;

	// Token: 0x0400276D RID: 10093
	private GenericVoid<GameObject> m_throwCallback = delegate(GameObject _object)
	{
	};
}
