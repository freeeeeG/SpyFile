using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000568 RID: 1384
public class ServerSlipCollider : ServerSynchroniserBase
{
	// Token: 0x06001A0E RID: 6670 RVA: 0x00082A70 File Offset: 0x00080E70
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_slipCollider = (SlipCollider)synchronisedObject;
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x00082A88 File Offset: 0x00080E88
	private void Slip(GameObject _object)
	{
		ServerPlayerSlipBehaviour serverPlayerSlipBehaviour = _object.RequestComponent<ServerPlayerSlipBehaviour>();
		if (serverPlayerSlipBehaviour != null)
		{
			serverPlayerSlipBehaviour.Slip(this);
		}
		if (this.m_slipCollider.m_deleteOnSlip)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x00082ACC File Offset: 0x00080ECC
	public void ObjectAdded(GameObject _object)
	{
		if ((this.m_slipCollider.m_slipFilter.value & 1 << _object.layer) != 0 && !this.m_slipped.Contains(_object))
		{
			this.Slip(_object);
			this.m_slipped.Add(_object);
		}
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x00082B1E File Offset: 0x00080F1E
	public void ObjectRemoved(GameObject _object)
	{
		if ((this.m_slipCollider.m_slipFilter.value & 1 << _object.layer) != 0)
		{
			this.m_slipped.Remove(_object);
		}
	}

	// Token: 0x06001A12 RID: 6674 RVA: 0x00082B4E File Offset: 0x00080F4E
	private void OnCollisionEnter(Collision collision)
	{
		this.ObjectAdded(collision.gameObject);
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x00082B5C File Offset: 0x00080F5C
	private void OnTriggerEnter(Collider collider)
	{
		this.ObjectAdded(collider.gameObject);
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x00082B6A File Offset: 0x00080F6A
	private void OnCollisionExit(Collision collision)
	{
		this.ObjectRemoved(collision.gameObject);
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x00082B78 File Offset: 0x00080F78
	private void OnTriggerExit(Collider collider)
	{
		this.ObjectRemoved(collider.gameObject);
	}

	// Token: 0x040014B8 RID: 5304
	private SlipCollider m_slipCollider;

	// Token: 0x040014B9 RID: 5305
	private List<GameObject> m_slipped = new List<GameObject>();
}
