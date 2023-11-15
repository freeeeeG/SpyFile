using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public class ServerRespawnCollider : ServerSynchroniserBase
{
	// Token: 0x060019E5 RID: 6629 RVA: 0x0008203F File Offset: 0x0008043F
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnCollider;
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x00082043 File Offset: 0x00080443
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_RespawnCollider = (RespawnCollider)synchronisedObject;
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x00082051 File Offset: 0x00080451
	private void SendRespawnMessage(GameObject _gameObject)
	{
		this.m_data.Initialise(_gameObject);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x0008206C File Offset: 0x0008046C
	public void ObjectAdded(GameObject _gameObject)
	{
		if (this.m_RespawnCollider == null)
		{
			return;
		}
		if ((this.m_RespawnCollider.m_respawnFilter.value & 1 << _gameObject.layer) != 0 && (!this.m_RespawnCollider.m_onlyRespawnables || _gameObject.RequestInterfaceRecursive<IRespawnBehaviour>() != null))
		{
			this.SendRespawnMessage(_gameObject);
			ServerPlayerRespawnManager.KillOrRespawn(_gameObject, this);
			if (this.m_RespawnCollider.m_onRespawnTrigger != string.Empty)
			{
				base.gameObject.SendTrigger(this.m_RespawnCollider.m_onRespawnTrigger);
			}
		}
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x00082105 File Offset: 0x00080505
	private void OnCollisionEnter(Collision collision)
	{
		this.ObjectAdded(collision.gameObject);
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x00082113 File Offset: 0x00080513
	private void OnTriggerEnter(Collider collider)
	{
		this.ObjectAdded(collider.gameObject);
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x060019EB RID: 6635 RVA: 0x00082121 File Offset: 0x00080521
	public RespawnCollider.RespawnType Type
	{
		get
		{
			return this.m_RespawnCollider.m_respawnType;
		}
	}

	// Token: 0x04001495 RID: 5269
	private RespawnCollider m_RespawnCollider;

	// Token: 0x04001496 RID: 5270
	private RespawnColliderMessage m_data = new RespawnColliderMessage();
}
