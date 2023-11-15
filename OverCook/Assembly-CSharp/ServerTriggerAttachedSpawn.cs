using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005B9 RID: 1465
public class ServerTriggerAttachedSpawn : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06001BD3 RID: 7123 RVA: 0x000882E0 File Offset: 0x000866E0
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_triggerAttachedSpawn = (TriggerAttachedSpawn)synchronisedObject;
		this.m_attachStation = this.FindAttachStation();
		if (this.m_attachStation != null)
		{
			this.m_conveyenceReceiver = this.m_attachStation.gameObject.RequestInterface<IConveyenceReceiver>();
		}
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x0008832C File Offset: 0x0008672C
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.m_attachStation == null && this.m_triggerAttachedSpawn != null)
		{
			this.m_attachStation = this.FindAttachStation();
			if (this.m_attachStation != null)
			{
				this.m_conveyenceReceiver = this.m_attachStation.gameObject.RequestInterface<IConveyenceReceiver>();
			}
		}
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x00088394 File Offset: 0x00086794
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerAttachedSpawn != null && this.m_triggerAttachedSpawn.m_trigger == _trigger)
		{
			if (this.m_attachStation == null || this.m_attachStation.HasItem() || (this.m_conveyenceReceiver != null && this.m_conveyenceReceiver.IsReceiving()))
			{
				return;
			}
			this.m_spawned.RemoveAll((GameObject obj) => obj == null);
			if (this.m_spawned.Count < this.m_triggerAttachedSpawn.m_maxNumber)
			{
				TriggerAttachedSpawn.WeightedPrefab weightedPrefab;
				if (this.m_triggerAttachedSpawn.m_spawnInOrder)
				{
					int num = MathUtils.Wrap(this.m_lastSpawned + 1, 0, this.m_triggerAttachedSpawn.m_attachmentPrefabs.Length);
					weightedPrefab = this.m_triggerAttachedSpawn.m_attachmentPrefabs[num];
					this.m_lastSpawned = num;
				}
				else
				{
					weightedPrefab = this.m_triggerAttachedSpawn.m_attachmentPrefabs.GetWeightedRandomElement<TriggerAttachedSpawn.WeightedPrefab>().Value;
				}
				if (weightedPrefab != null)
				{
					GameObject gameObject = NetworkUtils.ServerSpawnPrefab(base.gameObject, weightedPrefab.AttachmentPrefab, this.m_triggerAttachedSpawn.m_gridPointSelector.position, this.m_triggerAttachedSpawn.m_gridPointSelector.rotation);
					gameObject.name = weightedPrefab.AttachmentPrefab.name;
					this.m_attachStation.AddItem(gameObject, this.m_triggerAttachedSpawn.m_gridPointSelector.forward.XZ(), default(PlacementContext));
					this.m_spawned.Add(gameObject);
				}
			}
		}
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x00088528 File Offset: 0x00086928
	private ServerAttachStation FindAttachStation()
	{
		GridManager gridManager = GameUtils.GetGridManager(base.transform);
		GridIndex gridLocationFromPos = gridManager.GetGridLocationFromPos(this.m_triggerAttachedSpawn.m_gridPointSelector.position);
		GameObject gridOccupant = gridManager.GetGridOccupant(gridLocationFromPos);
		if (gridOccupant != null)
		{
			return gridOccupant.RequireComponent<ServerAttachStation>();
		}
		return null;
	}

	// Token: 0x040015D8 RID: 5592
	private TriggerAttachedSpawn m_triggerAttachedSpawn;

	// Token: 0x040015D9 RID: 5593
	private List<GameObject> m_spawned = new List<GameObject>();

	// Token: 0x040015DA RID: 5594
	private ServerAttachStation m_attachStation;

	// Token: 0x040015DB RID: 5595
	private IConveyenceReceiver m_conveyenceReceiver;

	// Token: 0x040015DC RID: 5596
	private int m_lastSpawned = -1;
}
