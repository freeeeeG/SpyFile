using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class ClientFireHazardSpawner : ClientSynchroniserBase
{
	// Token: 0x060015FD RID: 5629 RVA: 0x0007558C File Offset: 0x0007398C
	public override EntityType GetEntityType()
	{
		return EntityType.FireHazardSpawner;
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x00075590 File Offset: 0x00073990
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_spawner = (FireHazardSpawner)synchronisedObject;
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_spawner.m_hazardPrefab, new VoidGeneric<GameObject>(this.OnHazardSpawned));
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x000755C7 File Offset: 0x000739C7
	private void OnHazardSpawned(GameObject _object)
	{
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x000755CC File Offset: 0x000739CC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			FireHazardSpawnerMessage fireHazardSpawnerMessage = (FireHazardSpawnerMessage)serialisable;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(fireHazardSpawnerMessage.m_parentEntry.m_Header.m_uEntityID);
			if (entry != null)
			{
				GameObject gameObject = entry.m_GameObject;
				if (gameObject != null)
				{
					IParentable parentable = gameObject.RequestInterface<IParentable>();
					if (parentable != null)
					{
						Transform attachPoint = parentable.GetAttachPoint(base.gameObject);
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_spawner.m_hazardPrefab, attachPoint.position, attachPoint.rotation, attachPoint);
						EntitySerialisationRegistry.RegisterObject(gameObject2, fireHazardSpawnerMessage.m_spawnedEntry.m_Header.m_uEntityID);
						ComponentCacheRegistry.UpdateObject(gameObject2);
					}
				}
			}
		}
	}

	// Token: 0x0400109E RID: 4254
	private FireHazardSpawner m_spawner;
}
