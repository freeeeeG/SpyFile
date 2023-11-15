using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000496 RID: 1174
public class ServerFireHazardSpawner : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060015F7 RID: 5623 RVA: 0x00075394 File Offset: 0x00073794
	public override EntityType GetEntityType()
	{
		return EntityType.FireHazardSpawner;
	}

	// Token: 0x060015F8 RID: 5624 RVA: 0x00075398 File Offset: 0x00073798
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_spawner = (FireHazardSpawner)synchronisedObject;
		this.m_gridManager = GameUtils.GetGridManager(base.transform);
		this.m_Rigidbody = base.gameObject.RequestComponentUpwardsRecursive<Rigidbody>();
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_spawner.m_hazardPrefab);
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x000753F0 File Offset: 0x000737F0
	private void Burn()
	{
		GridManager gridManager = GameUtils.GetGridManager(base.transform);
		GridIndex gridLocationFromPos = gridManager.GetGridLocationFromPos(this.m_Rigidbody.position);
		GameObject gridOccupant = gridManager.GetGridOccupant(gridLocationFromPos);
		if (gridOccupant != null && gridOccupant.RequestComponent<FireHazard>())
		{
			ServerFireHazard serverFireHazard = gridOccupant.RequestComponent<ServerFireHazard>();
			serverFireHazard.Conflagrate();
		}
		else if (gridOccupant == null || gridOccupant.RequestInterface<HazardBase>() != null)
		{
			if (this.m_targetTransform)
			{
				IParentable parentable = this.m_targetTransform.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
				Transform attachPoint = parentable.GetAttachPoint(base.gameObject);
				if (attachPoint)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_spawner.m_hazardPrefab, attachPoint.position, attachPoint.rotation, attachPoint);
					EntitySerialisationRegistry.ServerRegisterObject(gameObject);
					ComponentCacheRegistry.UpdateObject(gameObject);
					EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(((MonoBehaviour)parentable).gameObject);
					EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(gameObject);
					this.m_data.m_parentEntry = entry;
					this.m_data.m_spawnedEntry = entry2;
					this.SendServerEvent(this.m_data);
				}
			}
			else
			{
				GameObject gameObject2 = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_spawner.m_hazardPrefab);
				if (this.m_spawner.m_alignToGrid)
				{
					gameObject2.transform.position = gridManager.GetPosFromGridLocation(gridLocationFromPos);
				}
			}
		}
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x0007555D File Offset: 0x0007395D
	public void SetTargetTransformToAttach(Transform transform)
	{
		this.m_targetTransform = transform;
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x00075566 File Offset: 0x00073966
	public void OnTrigger(string _message)
	{
		if (this.m_spawner.m_spawnTrigger == _message)
		{
			this.Burn();
		}
	}

	// Token: 0x04001099 RID: 4249
	private FireHazardSpawner m_spawner;

	// Token: 0x0400109A RID: 4250
	private GridManager m_gridManager;

	// Token: 0x0400109B RID: 4251
	private Rigidbody m_Rigidbody;

	// Token: 0x0400109C RID: 4252
	private Transform m_targetTransform;

	// Token: 0x0400109D RID: 4253
	private FireHazardSpawnerMessage m_data = new FireHazardSpawnerMessage();
}
