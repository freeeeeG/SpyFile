using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
public class ServerTriggerMoveSpawnPoints : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06001CBC RID: 7356 RVA: 0x0008C751 File Offset: 0x0008AB51
	public override EntityType GetEntityType()
	{
		return EntityType.TriggerMoveSpawn;
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x0008C755 File Offset: 0x0008AB55
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_changeRespawn = (TriggerMoveSpawnPoints)synchronisedObject;
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x0008C76A File Offset: 0x0008AB6A
	public void OnTrigger(string _trigger)
	{
		if (this.m_changeRespawn.m_trigger == _trigger)
		{
			this.Move();
		}
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x0008C788 File Offset: 0x0008AB88
	private void Move()
	{
		if (this.m_changeRespawn.m_spawnPoints == null || this.m_changeRespawn.m_spawnPoints.Length == 0)
		{
			return;
		}
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		KeyValuePair<GameObject, Transform>[] spawns = this.CalculateBestSpawns(players, this.m_changeRespawn.m_spawnPoints);
		this.m_data.Initialise(spawns, this.m_changeRespawn.m_spawnPoints);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x0008C7FC File Offset: 0x0008ABFC
	private KeyValuePair<GameObject, Transform>[] CalculateBestSpawns(GameObject[] _players, Transform[] _spawns)
	{
		ServerTriggerMoveSpawnPoints.<CalculateBestSpawns>c__AnonStorey0 <CalculateBestSpawns>c__AnonStorey = new ServerTriggerMoveSpawnPoints.<CalculateBestSpawns>c__AnonStorey0();
		<CalculateBestSpawns>c__AnonStorey._players = _players;
		List<KeyValuePair<GameObject, Transform>> list = new List<KeyValuePair<GameObject, Transform>>();
		int i;
		for (i = 0; i < <CalculateBestSpawns>c__AnonStorey._players.Length; i++)
		{
			list.AddRange(this.m_changeRespawn.m_spawnPoints.ConvertAll((Transform x) => new KeyValuePair<GameObject, Transform>(<CalculateBestSpawns>c__AnonStorey._players[i], x)));
		}
		list.Sort(new Comparison<KeyValuePair<GameObject, Transform>>(this.SortSpawnsByDistance));
		KeyValuePair<GameObject, Transform>[] result = new KeyValuePair<GameObject, Transform>[0];
		for (int j = 0; j < <CalculateBestSpawns>c__AnonStorey._players.Length; j++)
		{
			KeyValuePair<GameObject, Transform> spawn = list[0];
			list.RemoveAll((KeyValuePair<GameObject, Transform> x) => x.Key == spawn.Key || x.Value == spawn.Value);
			ArrayUtils.PushBack<KeyValuePair<GameObject, Transform>>(ref result, spawn);
		}
		return result;
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x0008C8E8 File Offset: 0x0008ACE8
	private int SortSpawnsByDistance(KeyValuePair<GameObject, Transform> _spawn1, KeyValuePair<GameObject, Transform> _spawn2)
	{
		bool flag = this.IsSpawnPointOccupied(_spawn1.Value);
		bool flag2 = this.IsSpawnPointOccupied(_spawn2.Value);
		if (flag != flag2)
		{
			return (!flag) ? -1 : 1;
		}
		float sqrMagnitude = (_spawn1.Value.position - _spawn1.Key.transform.position).sqrMagnitude;
		float sqrMagnitude2 = (_spawn2.Value.position - _spawn2.Key.transform.position).sqrMagnitude;
		return sqrMagnitude.CompareTo(sqrMagnitude2);
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x0008C988 File Offset: 0x0008AD88
	private bool IsSpawnPointOccupied(Transform _transform)
	{
		GridManager gridManager = GameUtils.GetGridManager(_transform);
		return gridManager != null && gridManager.GetGridOccupant(gridManager.GetGridLocationFromPos(_transform.position));
	}

	// Token: 0x0400166B RID: 5739
	private TriggerMoveSpawnPoints m_changeRespawn;

	// Token: 0x0400166C RID: 5740
	private MoveSpawnMessage m_data = new MoveSpawnMessage();
}
