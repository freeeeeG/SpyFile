using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005F3 RID: 1523
public class ServerBackpackRespawnBehaviour : ServerSynchroniserBase, IRespawnBehaviour
{
	// Token: 0x06001CFC RID: 7420 RVA: 0x0008E348 File Offset: 0x0008C748
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpackRespawnBehaviour = (BackpackRespawnBehaviour)synchronisedObject;
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x0008E35D File Offset: 0x0008C75D
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnBehaviour;
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x0008E360 File Offset: 0x0008C760
	public IEnumerator RespawnCoroutine(ServerRespawnCollider _collider)
	{
		if (this.m_isRespawning)
		{
			yield break;
		}
		this.m_isRespawning = true;
		ServerPhysicalAttachment attachment = base.gameObject.RequestComponent<ServerPhysicalAttachment>();
		if (attachment.IsAttached())
		{
			attachment.Detach();
		}
		if (attachment)
		{
			attachment.ManualDisable(true);
		}
		this.m_ServerData.m_RespawnType = ((!(_collider != null)) ? RespawnCollider.RespawnType.FallDeath : _collider.Type);
		this.m_ServerData.m_Phase = RespawnMessage.Phase.Begin;
		this.SendServerEvent(this.m_ServerData);
		base.gameObject.SetActive(false);
		IEnumerator wait = CoroutineUtils.TimerRoutine(this.m_backpackRespawnBehaviour.m_respawnTime, base.gameObject.layer);
		while (wait.MoveNext())
		{
			yield return null;
		}
		if (attachment)
		{
			attachment.ManualEnable();
		}
		if (this == null || base.gameObject == null)
		{
			yield break;
		}
		Collider collider = base.gameObject.GetComponent<Collider>();
		if (collider != null)
		{
			collider.enabled = true;
		}
		this.m_ServerData.m_Phase = RespawnMessage.Phase.End;
		this.SendServerEvent(this.m_ServerData);
		this.m_isRespawning = false;
		yield break;
	}

	// Token: 0x0400168E RID: 5774
	protected BackpackRespawnBehaviour m_backpackRespawnBehaviour;

	// Token: 0x0400168F RID: 5775
	private RespawnMessage m_ServerData = new RespawnMessage();

	// Token: 0x04001690 RID: 5776
	private bool m_isRespawning;
}
