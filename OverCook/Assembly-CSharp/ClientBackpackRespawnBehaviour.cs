using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class ClientBackpackRespawnBehaviour : ClientSynchroniserBase
{
	// Token: 0x06001D00 RID: 7424 RVA: 0x0008E5D4 File Offset: 0x0008C9D4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_backpackRespawnBehaviour = (BackpackRespawnBehaviour)synchronisedObject;
		this.m_waitForPfxDelay = new WaitForSeconds(this.m_backpackRespawnBehaviour.m_particleTime);
		this.m_physicalAttachment = base.gameObject.RequireComponent<ClientPhysicalAttachment>();
		this.m_worldObjectSynchroniser = base.gameObject.RequireComponent<ClientWorldObjectSynchroniser>();
		this.m_physicsObjectSynchroniser = this.m_physicalAttachment.AccessRigidbody().gameObject.RequireComponent<ClientWorldObjectSynchroniser>();
		this.m_startParent = base.transform.parent;
		this.m_startLocation = base.transform.localPosition;
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x0008E669 File Offset: 0x0008CA69
	public override EntityType GetEntityType()
	{
		return EntityType.RespawnBehaviour;
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x0008E66C File Offset: 0x0008CA6C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		RespawnMessage respawnMessage = (RespawnMessage)serialisable;
		if (respawnMessage.m_Phase == RespawnMessage.Phase.Begin)
		{
			base.gameObject.SetActive(false);
		}
		else if (respawnMessage.m_Phase == RespawnMessage.Phase.End)
		{
			this.m_physicsObjectSynchroniser.Pause();
			this.m_worldObjectSynchroniser.Pause();
			this.m_physicalAttachment.AccessRigidbody().transform.SetParent(this.m_startParent, false);
			this.m_physicalAttachment.AccessRigidbody().transform.localPosition = this.m_startLocation;
			this.m_physicalAttachment.AccessRigidbody().transform.localScale = Vector3.one;
			this.m_physicalAttachment.AccessRigidbody().transform.localRotation = Quaternion.identity;
			base.transform.SetParent(this.m_physicalAttachment.AccessRigidbody().transform, false);
			base.transform.localPosition = Vector3.zero;
			base.transform.localScale = Vector3.one;
			base.transform.localRotation = Quaternion.identity;
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				ServerWorldObjectSynchroniser serverWorldObjectSynchroniser = base.gameObject.RequireComponent<ServerWorldObjectSynchroniser>();
				if (serverWorldObjectSynchroniser != null)
				{
					serverWorldObjectSynchroniser.ResumeAllClients(true);
				}
				ServerPhysicsObjectSynchroniser serverPhysicsObjectSynchroniser = base.gameObject.RequestComponentUpwardsRecursive<ServerPhysicsObjectSynchroniser>();
				if (serverPhysicsObjectSynchroniser != null)
				{
					serverPhysicsObjectSynchroniser.ResumeAllClients(true);
				}
			}
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.EndRespawn());
		}
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x0008E7E0 File Offset: 0x0008CBE0
	private IEnumerator EndRespawn()
	{
		if (this.m_worldObjectSynchroniser != null)
		{
			while (!this.m_worldObjectSynchroniser.IsReadyToResume())
			{
				yield return null;
			}
			this.m_worldObjectSynchroniser.Resume();
		}
		if (this.m_physicsObjectSynchroniser != null)
		{
			while (!this.m_physicsObjectSynchroniser.IsReadyToResume())
			{
				yield return null;
			}
			this.m_physicsObjectSynchroniser.Resume();
		}
		Collider collider = base.gameObject.RequestComponent<Collider>();
		if (collider != null)
		{
			collider.enabled = true;
		}
		this.m_backpackRespawnBehaviour.m_spawnEffect.InstantiateOnParent(base.transform, true);
		GameUtils.TriggerAudio(GameOneShotAudioTag.PlayerSpawn, base.gameObject.layer);
		yield return this.m_waitForPfxDelay;
		yield break;
	}

	// Token: 0x04001691 RID: 5777
	private BackpackRespawnBehaviour m_backpackRespawnBehaviour;

	// Token: 0x04001692 RID: 5778
	private WaitForSeconds m_waitForPfxDelay;

	// Token: 0x04001693 RID: 5779
	private Transform m_startParent;

	// Token: 0x04001694 RID: 5780
	private Vector3 m_startLocation;

	// Token: 0x04001695 RID: 5781
	private ClientWorldObjectSynchroniser m_worldObjectSynchroniser;

	// Token: 0x04001696 RID: 5782
	private ClientWorldObjectSynchroniser m_physicsObjectSynchroniser;

	// Token: 0x04001697 RID: 5783
	private ClientPhysicalAttachment m_physicalAttachment;
}
