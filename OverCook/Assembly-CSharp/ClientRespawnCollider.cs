using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class ClientRespawnCollider : ClientSynchroniserBase
{
	// Token: 0x060019ED RID: 6637 RVA: 0x00082136 File Offset: 0x00080536
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnCollider;
	}

	// Token: 0x060019EE RID: 6638 RVA: 0x0008213A File Offset: 0x0008053A
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_RespawnCollider = (RespawnCollider)synchronisedObject;
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x00082150 File Offset: 0x00080550
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		RespawnColliderMessage respawnColliderMessage = (RespawnColliderMessage)serialisable;
		if (this.m_RespawnCollider.m_onDeathEffect != null)
		{
			GameObject gameObject = this.m_RespawnCollider.m_onDeathEffect.Instantiate(base.transform, base.transform.InverseTransformPoint(respawnColliderMessage.m_killPosition), Quaternion.identity);
			GameUtils.TriggerAudio(GameOneShotAudioTag.ItemSplash, base.gameObject.layer);
			if (gameObject != null && gameObject.RequestComponentRecursive<AutoDestructParticleSystem>() == null)
			{
				gameObject.AddComponent<AutoDestructParticleSystem>();
			}
		}
	}

	// Token: 0x04001497 RID: 5271
	private RespawnCollider m_RespawnCollider;
}
