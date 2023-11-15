using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A1C RID: 2588
public class ServerPlayerRespawnBehaviour : ServerSynchroniserBase, IRespawnBehaviour
{
	// Token: 0x06003346 RID: 13126 RVA: 0x000F07DC File Offset: 0x000EEBDC
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_PlayerRespawnBehaviour = (PlayerRespawnBehaviour)synchronisedObject;
	}

	// Token: 0x06003347 RID: 13127 RVA: 0x000F07EA File Offset: 0x000EEBEA
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnBehaviour;
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x000F07F0 File Offset: 0x000EEBF0
	public IEnumerator RespawnCoroutine(ServerRespawnCollider _collider)
	{
		this.DropOrDestroyHeldItems(_collider);
		this.m_ServerData.m_RespawnType = _collider.Type;
		this.m_ServerData.m_Phase = RespawnMessage.Phase.Begin;
		this.SendServerEvent(this.m_ServerData);
		yield break;
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x000F0814 File Offset: 0x000EEC14
	private void DropOrDestroyHeldItems(ServerRespawnCollider _collider)
	{
		IPlayerCarrier playerCarrier = base.gameObject.RequireInterface<IPlayerCarrier>();
		for (int i = 0; i < 2; i++)
		{
			if (playerCarrier.InspectCarriedItem((PlayerAttachTarget)i) != null)
			{
				if (_collider.Type == RespawnCollider.RespawnType.FallDeath || _collider.Type == RespawnCollider.RespawnType.Drowning)
				{
					GameObject gameObject = playerCarrier.TakeItem((PlayerAttachTarget)i);
					_collider.ObjectAdded(gameObject);
				}
				else
				{
					PlayerControlsHelper.DropHeldItem(this.m_PlayerRespawnBehaviour.m_playerControls, base.gameObject.transform.forward.XZ());
				}
			}
		}
	}

	// Token: 0x04002942 RID: 10562
	private PlayerRespawnBehaviour m_PlayerRespawnBehaviour;

	// Token: 0x04002943 RID: 10563
	private RespawnMessage m_ServerData = new RespawnMessage();
}
