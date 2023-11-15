using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000812 RID: 2066
public class ServerPhysicalAttachment : ServerSynchroniserBase, IAttachment
{
	// Token: 0x0600277A RID: 10106 RVA: 0x000B9B10 File Offset: 0x000B7F10
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_physicalAttachment = (PhysicalAttachment)synchronisedObject;
		this.m_collider = base.GetComponent<Collider>();
		this.m_isHeld = false;
		this.m_LimitedQuantityItem = base.GetComponent<ServerLimitedQuantityItem>();
		this.m_rigidbodyTransform = this.AccessRigidbody().transform;
		LimitedQuantityItemManager limitedMan = GameUtils.RequireManager<LimitedQuantityItemManager>();
		this.m_AttachedDestructionScoreModifier = (() => limitedMan.m_AttachedDeletionScoreModifier);
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x000B9B7C File Offset: 0x000B7F7C
	public virtual void Awake()
	{
		this.m_transform = base.transform;
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000B9B8A File Offset: 0x000B7F8A
	public override EntityType GetEntityType()
	{
		return EntityType.PhysicalAttach;
	}

	// Token: 0x0600277D RID: 10109 RVA: 0x000B9B8E File Offset: 0x000B7F8E
	public void RegisterAttachChangedCallback(AttachChangedCallback _callback)
	{
		this.m_attachChangedCallback = (AttachChangedCallback)Delegate.Combine(this.m_attachChangedCallback, _callback);
	}

	// Token: 0x0600277E RID: 10110 RVA: 0x000B9BA7 File Offset: 0x000B7FA7
	public void UnregisterAttachChangedCallback(AttachChangedCallback _callback)
	{
		this.m_attachChangedCallback = (AttachChangedCallback)Delegate.Remove(this.m_attachChangedCallback, _callback);
	}

	// Token: 0x0600277F RID: 10111 RVA: 0x000B9BC0 File Offset: 0x000B7FC0
	public void Attach(IParentable _parentable)
	{
		this.DetachFromRigidBodyContainer();
		this.m_transform.SetParent(_parentable.GetAttachPoint(base.gameObject));
		Vector3 lossyScale = this.m_transform.lossyScale;
		Vector3 b = new Vector3(1f / lossyScale.x, 1f / lossyScale.y, 1f / lossyScale.z);
		this.m_transform.localScale = this.m_transform.localScale.MultipliedBy(b);
		bool flag = _parentable.HasClientSidePrediction();
		if (!this.m_bIsClientSidePredicted || !flag)
		{
			this.m_transform.localPosition = Vector3.zero;
			this.m_transform.localRotation = Quaternion.identity;
			ServerWorldObjectSynchroniser component = base.GetComponent<ServerWorldObjectSynchroniser>();
			component.ResumePositions();
			ServerPhysicsObjectSynchroniser component2 = this.AccessRigidbody().GetComponent<ServerPhysicsObjectSynchroniser>();
			if (null != component2)
			{
				component2.Serialising = true;
			}
		}
		else
		{
			ServerWorldObjectSynchroniser component3 = base.GetComponent<ServerWorldObjectSynchroniser>();
			component3.PausePositions();
			ServerPhysicsObjectSynchroniser component4 = this.AccessRigidbody().GetComponent<ServerPhysicsObjectSynchroniser>();
			if (null != component4)
			{
				component4.Serialising = false;
			}
		}
		this.m_bIsClientSidePredicted = flag;
		this.m_isHeld = true;
		if (null != this.m_LimitedQuantityItem)
		{
			this.m_LimitedQuantityItem.AddDestructionScoreModifier(this.m_AttachedDestructionScoreModifier);
		}
		this.OnAttachChanged(_parentable);
	}

	// Token: 0x06002780 RID: 10112 RVA: 0x000B9D14 File Offset: 0x000B8114
	public void Detach()
	{
		this.m_collider.transform.SetParent(null);
		this.AttachToRigidBodyContainer();
		this.m_isHeld = false;
		if (null != this.m_LimitedQuantityItem)
		{
			this.m_LimitedQuantityItem.RemoveDestructionScoreModifier(this.m_AttachedDestructionScoreModifier);
		}
		this.OnAttachChanged(null);
		this.m_transform.localScale = Vector3.one;
		this.m_physicalAttachment.m_groundCast.ClearGround();
	}

	// Token: 0x06002781 RID: 10113 RVA: 0x000B9D88 File Offset: 0x000B8188
	public bool IsAttached()
	{
		return this.m_isHeld;
	}

	// Token: 0x06002782 RID: 10114 RVA: 0x000B9D90 File Offset: 0x000B8190
	public GameObject AccessGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x000B9D98 File Offset: 0x000B8198
	public Rigidbody AccessRigidbody()
	{
		return this.m_physicalAttachment.m_container;
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x000B9DA5 File Offset: 0x000B81A5
	public RigidbodyMotion AccessMotion()
	{
		return this.m_physicalAttachment.m_motion;
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x000B9DB4 File Offset: 0x000B81B4
	public override void UpdateSynchronising()
	{
		if (this.m_physicalAttachment != null)
		{
			if (this.m_isHeld)
			{
				this.m_rigidbodyTransform.position = this.m_transform.position;
			}
			else
			{
				Vector3 velocity = this.m_physicalAttachment.m_surfaceMovable.GetVelocity();
				if (velocity.sqrMagnitude >= 0.05f)
				{
					this.AccessMotion().Movement(velocity);
				}
			}
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x000B9E26 File Offset: 0x000B8226
	public void ManualEnable()
	{
		if (!this.m_isHeld)
		{
			this.AttachToRigidBodyContainer();
		}
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x000B9E39 File Offset: 0x000B8239
	public void ManualDisable(bool _clearPredicted = false)
	{
		if (!this.m_isHeld)
		{
			if (_clearPredicted)
			{
				this.m_bIsClientSidePredicted = false;
			}
			this.DetachFromRigidBodyContainer();
		}
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x000B9E5C File Offset: 0x000B825C
	private void AttachToRigidBodyContainer()
	{
		Rigidbody rigidbody = this.AccessRigidbody();
		rigidbody.transform.position = this.m_transform.position;
		rigidbody.transform.rotation = this.m_transform.rotation;
		rigidbody.transform.SetParent(this.m_transform.parent);
		this.m_transform.SetParent(rigidbody.transform);
		this.AccessMotion().SetKinematic(false);
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x000B9ED0 File Offset: 0x000B82D0
	private void DetachFromRigidBodyContainer()
	{
		Rigidbody rigidbody = this.AccessRigidbody();
		this.m_collider.transform.SetParent(null, false);
		this.m_collider.transform.SetParent(rigidbody.transform.parent, false);
		this.m_collider.transform.localPosition = rigidbody.transform.localPosition;
		this.m_collider.transform.localRotation = rigidbody.transform.localRotation;
		this.AccessMotion().SetKinematic(true);
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x000B9F54 File Offset: 0x000B8354
	private void OnAttachChanged(IParentable _parentable)
	{
		this.m_attachChangedCallback(_parentable);
		this.m_ServerData.m_parentable = _parentable;
		this.SendServerEvent(this.m_ServerData);
	}

	// Token: 0x04001F0E RID: 7950
	private const float c_MinSurfaceMoveVelocity = 0.05f;

	// Token: 0x04001F0F RID: 7951
	private PhysicalAttachment m_physicalAttachment;

	// Token: 0x04001F10 RID: 7952
	private Collider m_collider;

	// Token: 0x04001F11 RID: 7953
	private bool m_isHeld;

	// Token: 0x04001F12 RID: 7954
	private AttachChangedCallback m_attachChangedCallback = delegate(IParentable _parentable)
	{
	};

	// Token: 0x04001F13 RID: 7955
	private PhysicalAttachMessage m_ServerData = new PhysicalAttachMessage();

	// Token: 0x04001F14 RID: 7956
	private ServerLimitedQuantityItem m_LimitedQuantityItem;

	// Token: 0x04001F15 RID: 7957
	private Generic<float> m_AttachedDestructionScoreModifier;

	// Token: 0x04001F16 RID: 7958
	private bool m_bIsClientSidePredicted;

	// Token: 0x04001F17 RID: 7959
	private Transform m_transform;

	// Token: 0x04001F18 RID: 7960
	private Transform m_rigidbodyTransform;
}
