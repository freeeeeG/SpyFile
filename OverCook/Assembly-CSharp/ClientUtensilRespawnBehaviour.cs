using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public class ClientUtensilRespawnBehaviour : ClientSynchroniserBase
{
	// Token: 0x06001DBB RID: 7611 RVA: 0x0009061E File Offset: 0x0008EA1E
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_utensilRespawnBehaviour = (UtensilRespawnBehaviour)synchronisedObject;
		this.m_waitForPfxDelay = new WaitForSeconds(this.m_utensilRespawnBehaviour.m_particleTime);
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x00090642 File Offset: 0x0008EA42
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnBehaviour;
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x00090648 File Offset: 0x0008EA48
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		RespawnMessage respawnMessage = (RespawnMessage)serialisable;
		if (respawnMessage.m_Phase == RespawnMessage.Phase.Begin)
		{
			base.gameObject.SetActive(false);
		}
		else if (respawnMessage.m_Phase == RespawnMessage.Phase.End)
		{
			base.gameObject.SetActive(true);
			base.transform.position = respawnMessage.m_RespawnPosition;
			base.transform.localScale = Vector3.one;
			Collider component = base.gameObject.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = true;
			}
			base.StartCoroutine(this.EndRespawn());
		}
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x000906E0 File Offset: 0x0008EAE0
	private IEnumerator EndRespawn()
	{
		this.m_utensilRespawnBehaviour.m_spawnEffect.InstantiateOnParent(base.transform, true);
		GameUtils.TriggerAudio(GameOneShotAudioTag.PlayerSpawn, base.gameObject.layer);
		yield return this.m_waitForPfxDelay;
		yield break;
	}

	// Token: 0x040016F2 RID: 5874
	private UtensilRespawnBehaviour m_utensilRespawnBehaviour;

	// Token: 0x040016F3 RID: 5875
	private WaitForSeconds m_waitForPfxDelay;
}
