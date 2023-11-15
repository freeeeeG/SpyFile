using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x020000B5 RID: 181
public class MapManager : Singleton<MapManager>
{
	// Token: 0x060003E5 RID: 997 RVA: 0x0000F5DE File Offset: 0x0000D7DE
	private new void Awake()
	{
		base.Awake();
		if (this.list_PlayerOrigins == null)
		{
			this.list_PlayerOrigins = new List<IPlayerStartPoint>();
		}
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0000F5F9 File Offset: 0x0000D7F9
	private void Start()
	{
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0000F5FB File Offset: 0x0000D7FB
	private void OnEnable()
	{
		EventMgr.Register<GameObject>(eGameEvents.OnGridObjectChanged, new Action<GameObject>(this.OnGridObjectChanged));
		EventMgr.Register<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, new Action<GameSceneReferenceHandler>(this.OnUpdateEnvSceneBindings));
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0000F62C File Offset: 0x0000D82C
	private void OnDisable()
	{
		EventMgr.Remove<GameObject>(eGameEvents.OnGridObjectChanged, new Action<GameObject>(this.OnGridObjectChanged));
		EventMgr.Remove<GameSceneReferenceHandler>(eGameEvents.UpdateEnvSceneBindings, new Action<GameSceneReferenceHandler>(this.OnUpdateEnvSceneBindings));
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0000F65D File Offset: 0x0000D85D
	private void OnUpdateEnvSceneBindings(GameSceneReferenceHandler refHandler)
	{
		this.list_Spawners = refHandler.List_MonsterSpawners;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0000F66C File Offset: 0x0000D86C
	public bool IsPathBlocked()
	{
		List<Collider> list_Colliders = null;
		using (List<MonsterSpawner>.Enumerator enumerator = this.list_Spawners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.CheckPathBlockedByObject(list_Colliders, true))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0000F6CC File Offset: 0x0000D8CC
	public void RegisterPlayerOrigin(IPlayerStartPoint origin)
	{
		if (this.list_PlayerOrigins == null)
		{
			this.list_PlayerOrigins = new List<IPlayerStartPoint>();
		}
		if (!this.list_PlayerOrigins.Contains(origin))
		{
			this.list_PlayerOrigins.Add(origin);
			DebugManager.Log(eDebugKey.PATHFINDING, "註冊玩家起點: " + origin.GetGameObject().name, null);
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0000F724 File Offset: 0x0000D924
	public void UnregisterPlayerOrigin(IPlayerStartPoint origin)
	{
		if (this.list_PlayerOrigins == null)
		{
			this.list_PlayerOrigins = new List<IPlayerStartPoint>();
		}
		if (this.list_PlayerOrigins.Contains(origin))
		{
			this.list_PlayerOrigins.Remove(origin);
			DebugManager.Log(eDebugKey.PATHFINDING, "解除註冊玩家起點: " + origin.GetGameObject().name, null);
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0000F77C File Offset: 0x0000D97C
	public bool IsAnyPlayerOriginRegistered()
	{
		return this.list_PlayerOrigins != null && this.list_PlayerOrigins.Count > 0;
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0000F798 File Offset: 0x0000D998
	public IPlayerStartPoint GetClosestPlayerOrigin(Vector3 from)
	{
		float num = float.PositiveInfinity;
		int index = 0;
		if (this.list_PlayerOrigins == null || this.list_PlayerOrigins.Count == 0)
		{
			Debug.LogError("還沒有註冊玩家起點就取最近位置了");
			return null;
		}
		for (int i = 0; i < this.list_PlayerOrigins.Count; i++)
		{
			float num2 = Vector3.SqrMagnitude(this.list_PlayerOrigins[i].GetPosition() - from);
			if (num2 < num)
			{
				num = num2;
				index = i;
			}
		}
		return this.list_PlayerOrigins[index];
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0000F818 File Offset: 0x0000DA18
	private void OnGridObjectChanged(GameObject obj)
	{
		List<Collider> placementColliders = obj.GetComponent<IPlaceable>().GetPlacementColliders();
		Bounds bounds = placementColliders[0].bounds;
		foreach (Collider collider in placementColliders)
		{
			bounds.Encapsulate(collider.bounds);
		}
		GraphUpdateObject graphUpdateObject = new GraphUpdateObject(bounds);
		graphUpdateObject.updatePhysics = true;
		AstarPath.OnGraphsUpdated = (OnScanDelegate)Delegate.Combine(AstarPath.OnGraphsUpdated, new OnScanDelegate(this.OnGraphUpdated));
		AstarPath.active.UpdateGraphs(graphUpdateObject);
		AstarPath.active.FlushGraphUpdates();
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0000F8C8 File Offset: 0x0000DAC8
	private void OnGraphUpdated(AstarPath script)
	{
		AstarPath.OnGraphsUpdated = (OnScanDelegate)Delegate.Remove(AstarPath.OnGraphsUpdated, new OnScanDelegate(this.OnGraphUpdated));
		EventMgr.SendEvent(eGameEvents.OnGraphUpdated);
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
	public bool CheckPathBlockedByObject(List<Collider> list_Colliders, bool alwaysRevert = false)
	{
		using (List<MonsterSpawner>.Enumerator enumerator = this.list_Spawners.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.CheckPathBlockedByObject(list_Colliders, alwaysRevert))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0000F954 File Offset: 0x0000DB54
	public void ChangeSpawnPointPosition(int index, Vector3 newPosition)
	{
		if (index >= this.list_Spawners.Count)
		{
			Debug.LogError("ChangeSpawnPointPosition: index超出範圍");
			return;
		}
		this.list_Spawners[index].SetSpawnPoint(newPosition);
		foreach (MonsterSpawner monsterSpawner in this.list_Spawners)
		{
			monsterSpawner.RecalculatePath();
		}
	}

	// Token: 0x04000406 RID: 1030
	[SerializeField]
	[Header("玩家的基地位置")]
	private List<IPlayerStartPoint> list_PlayerOrigins;

	// Token: 0x04000407 RID: 1031
	[SerializeField]
	private List<MonsterSpawner> list_Spawners;
}
