using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class ClientPhysicalAttachment : ClientSynchroniserBase, IClientAttachment
{
	// Token: 0x0600278D RID: 10125 RVA: 0x000B9FBC File Offset: 0x000B83BC
	public IClientSidePredicted GetClientSidePrediction()
	{
		return this.m_Prediction;
	}

	// Token: 0x0600278E RID: 10126 RVA: 0x000B9FC4 File Offset: 0x000B83C4
	public void SetClientSidePrediction(CreateClientSidePredictionCallback prediction)
	{
		this.m_Prediction = prediction();
	}

	// Token: 0x0600278F RID: 10127 RVA: 0x000B9FD2 File Offset: 0x000B83D2
	public override EntityType GetEntityType()
	{
		return EntityType.PhysicalAttach;
	}

	// Token: 0x06002790 RID: 10128 RVA: 0x000B9FD8 File Offset: 0x000B83D8
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_isHeld = false;
		this.m_physicalAttachment = (PhysicalAttachment)synchronisedObject;
		if (base.GetComponent<ServerPhysicalAttachment>() == null)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = base.name + " MeshLerper";
			this.m_physicalAttachment.m_meshLerper = gameObject.AddComponent<MeshLerper>();
			this.m_physicalAttachment.m_meshLerper.SetLerpActive(this.m_physicalAttachment.GetFakeMeshActive());
			Transform transform = gameObject.transform;
			transform.position = base.transform.position;
			transform.rotation = base.transform.rotation;
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				base.transform.GetChild(i).SetParent(transform);
			}
			transform.SetParent(base.transform);
			transform.position = base.transform.position;
			this.m_physicalAttachment.m_meshLerper.SetPosition(base.transform.position);
		}
		this.m_worldObjSync = base.gameObject.RequireComponent<ClientWorldObjectSynchroniser>();
		this.m_worldObjSync.RegisterOnParentChanged(new GenericVoid(this.OnParentChanged));
	}

	// Token: 0x06002791 RID: 10129 RVA: 0x000BA10C File Offset: 0x000B850C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		PhysicalAttachMessage physicalAttachMessage = (PhysicalAttachMessage)serialisable;
		this.m_isHeld = (physicalAttachMessage.m_parentable != null);
		if (physicalAttachMessage.m_parentable != null)
		{
			this.m_bClientSidePredicted = (this.m_isHeld && physicalAttachMessage.m_parentable.HasClientSidePrediction());
			this.m_Parent = physicalAttachMessage.m_parentable;
			if (!this.m_bClientSidePredicted && this.m_Prediction != null)
			{
				this.m_Prediction.Clear();
				base.transform.localPosition = Vector3.zero;
				base.transform.localRotation = Quaternion.identity;
			}
		}
		if (this.m_physicalAttachment.m_groundCast != null)
		{
			this.m_physicalAttachment.m_groundCast.enabled = !this.m_isHeld;
		}
		this.m_attachChangedCallback(physicalAttachMessage.m_parentable);
	}

	// Token: 0x06002792 RID: 10130 RVA: 0x000BA1E9 File Offset: 0x000B85E9
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_Prediction != null && this.m_bClientSidePredicted)
		{
			this.m_Prediction.Update();
		}
	}

	// Token: 0x06002793 RID: 10131 RVA: 0x000BA212 File Offset: 0x000B8612
	public bool IsAttached()
	{
		return this.m_isHeld;
	}

	// Token: 0x06002794 RID: 10132 RVA: 0x000BA21A File Offset: 0x000B861A
	public GameObject AccessGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06002795 RID: 10133 RVA: 0x000BA222 File Offset: 0x000B8622
	public Rigidbody AccessRigidbody()
	{
		return this.m_physicalAttachment.m_container;
	}

	// Token: 0x06002796 RID: 10134 RVA: 0x000BA22F File Offset: 0x000B862F
	private void OnParentChanged()
	{
		this.m_worldObjSync.CorrectScale();
	}

	// Token: 0x06002797 RID: 10135 RVA: 0x000BA23C File Offset: 0x000B863C
	public void RegisterAttachChangedCallback(AttachChangedCallback _callback)
	{
		this.m_attachChangedCallback = (AttachChangedCallback)Delegate.Combine(this.m_attachChangedCallback, _callback);
	}

	// Token: 0x06002798 RID: 10136 RVA: 0x000BA255 File Offset: 0x000B8655
	public void UnregisterAttachChangedCallback(AttachChangedCallback _callback)
	{
		this.m_attachChangedCallback = (AttachChangedCallback)Delegate.Remove(this.m_attachChangedCallback, _callback);
	}

	// Token: 0x06002799 RID: 10137 RVA: 0x000BA26E File Offset: 0x000B866E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_worldObjSync != null)
		{
			this.m_worldObjSync.UnregisterOnParentChanged(new GenericVoid(this.OnParentChanged));
		}
	}

	// Token: 0x04001F1A RID: 7962
	private bool m_isHeld;

	// Token: 0x04001F1B RID: 7963
	private AttachChangedCallback m_attachChangedCallback = delegate(IParentable _parentable)
	{
	};

	// Token: 0x04001F1C RID: 7964
	private IParentable m_Parent;

	// Token: 0x04001F1D RID: 7965
	private PhysicalAttachment m_physicalAttachment;

	// Token: 0x04001F1E RID: 7966
	private ClientWorldObjectSynchroniser m_worldObjSync;

	// Token: 0x04001F1F RID: 7967
	private bool m_bClientSidePredicted;

	// Token: 0x04001F20 RID: 7968
	public IClientSidePredicted m_Prediction;
}
