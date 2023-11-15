using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x020000B9 RID: 185
public class MonsterSpawner : MonoBehaviour
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x000102AF File Offset: 0x0000E4AF
	public int SpawnNodeIndex
	{
		get
		{
			return this.spawnNodeIndex;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600041B RID: 1051 RVA: 0x000102B7 File Offset: 0x0000E4B7
	public Vector3 SpawnPosition
	{
		get
		{
			return this.node_SpawnPosition.transform.position;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x0600041C RID: 1052 RVA: 0x000102C9 File Offset: 0x0000E4C9
	public Vector3 EndPosition
	{
		get
		{
			return this.endPosition;
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x000102D4 File Offset: 0x0000E4D4
	private void Awake()
	{
		EventMgr.Register<int>(eGameEvents.OnFloodPathUpdate, new Action<int>(this.OnFloodPathUpdated));
		EventMgr.Register<MonsterSpawnRequest>(eGameEvents.RequestSpawnMonster, new Action<MonsterSpawnRequest>(this.OnRequestSpawnMonster));
		EventMgr.Register(eGameEvents.OnGraphUpdated, new Action(this.OnGraphUpdated));
		EventMgr.Register<int, List<int>>(eGameEvents.SetNextWaveSpawnIndex, new Action<int, List<int>>(this.OnSetNextWaveSpawnIndex));
		this.lineRenderer.material = this.mat_PathLine_Unused;
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x00010354 File Offset: 0x0000E554
	private void OnDestroy()
	{
		EventMgr.Remove<int>(eGameEvents.OnFloodPathUpdate, new Action<int>(this.OnFloodPathUpdated));
		EventMgr.Remove<MonsterSpawnRequest>(eGameEvents.RequestSpawnMonster, new Action<MonsterSpawnRequest>(this.OnRequestSpawnMonster));
		EventMgr.Remove(eGameEvents.OnGraphUpdated, new Action(this.OnGraphUpdated));
		EventMgr.Remove<int, List<int>>(eGameEvents.SetNextWaveSpawnIndex, new Action<int, List<int>>(this.OnSetNextWaveSpawnIndex));
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x000103C4 File Offset: 0x0000E5C4
	private void OnSetNextWaveSpawnIndex(int round, List<int> list)
	{
		bool flag = list.Contains(this.spawnNodeIndex);
		if (round != 0 && flag != this.isSpawnInThisWave)
		{
			string @string = LocalizationManager.Instance.GetString("UI", "NOTIFICATION_ENEMY_ATTACKING_FROM_NEW_PATH", Array.Empty<object>());
			EventMgr.SendEvent<string>(eGameEvents.TriggerNotification, @string);
		}
		if (flag)
		{
			this.lineRenderer.material = this.mat_PathLine_Incoming;
			this.lineRenderer.sortingOrder = 0;
		}
		else
		{
			this.lineRenderer.material = this.mat_PathLine_Unused;
			this.lineRenderer.sortingOrder = -1;
		}
		this.isSpawnInThisWave = flag;
		this.lineRenderer.enabled = true;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00010468 File Offset: 0x0000E668
	private void OnRequestSpawnMonster(MonsterSpawnRequest data)
	{
		if (data.spawnNodeIndex != this.spawnNodeIndex)
		{
			return;
		}
		for (int i = 0; i < data.count; i++)
		{
			this.Spawn(data.type, data.isCorrupted);
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x000104A7 File Offset: 0x0000E6A7
	private void Start()
	{
		this.endPosition = Singleton<MapManager>.Instance.GetClosestPlayerOrigin(this.SpawnPosition).GetPosition();
		this.isInitialized = true;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x000104CB File Offset: 0x0000E6CB
	private void OnFloodPathUpdated(int spawnIndex)
	{
		if (spawnIndex != this.spawnNodeIndex)
		{
			return;
		}
		this.RecalculatePath();
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000104DD File Offset: 0x0000E6DD
	public void RecalculatePath()
	{
		this.path = FloodPathTracer.Construct(this.SpawnPosition, this.floodPath, new OnPathDelegate(this.OnPathComplete));
		AstarPath.StartPath(this.path, false);
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001050E File Offset: 0x0000E70E
	public void SetSpawnPoint(Vector3 newPosition)
	{
		this.node_SpawnPosition.transform.position = newPosition.WithY(0f);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001052C File Offset: 0x0000E72C
	private void OnPathComplete(Path path)
	{
		path.callback = (OnPathDelegate)Delegate.Remove(path.callback, new OnPathDelegate(this.OnPathComplete));
		if (path.error)
		{
			DebugManager.LogError(eDebugKey.PATHFINDING, "MonsterSpawner路徑計算出錯", base.gameObject);
			return;
		}
		this.SetupPathLine(path);
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x00010580 File Offset: 0x0000E780
	private void SetupPathLine(Path p)
	{
		this.lineRenderer.positionCount = p.vectorPath.Count;
		for (int i = 0; i < p.vectorPath.Count; i++)
		{
			this.lineRenderer.SetPosition(i, p.vectorPath[i] + Vector3.up * 0.1f);
		}
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x000105E8 File Offset: 0x0000E7E8
	private void SetupPlacementLine(Path p)
	{
		this.lineRenderer_Placement.positionCount = p.vectorPath.Count;
		for (int i = 0; i < p.vectorPath.Count; i++)
		{
			this.lineRenderer_Placement.SetPosition(i, p.vectorPath[i] + Vector3.up * 0.1f);
		}
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0001064D File Offset: 0x0000E84D
	private IEnumerator CR_SetupPlacementLine(Path p)
	{
		while (p == null)
		{
			Debug.Log("wait...");
			yield return null;
		}
		this.lineRenderer_Placement.positionCount = p.vectorPath.Count;
		for (int i = 0; i < p.vectorPath.Count; i++)
		{
			this.lineRenderer_Placement.SetPosition(i, p.vectorPath[i] + Vector3.up * 0.1f);
		}
		yield break;
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00010663 File Offset: 0x0000E863
	private void OnGraphUpdated()
	{
		this.endPosition = Singleton<MapManager>.Instance.GetClosestPlayerOrigin(this.SpawnPosition).GetPosition();
		this.CalculateFloodPath(this.endPosition);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001068C File Offset: 0x0000E88C
	private void CalculateFloodPath(Vector3 targetPos)
	{
		this.floodPath = FloodPath.Construct(targetPos, new OnPathDelegate(this.OnFloodPathReady));
		AstarPath.StartPath(this.floodPath, true);
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x000106B2 File Offset: 0x0000E8B2
	private void OnFloodPathReady(Path path)
	{
		path.callback = (OnPathDelegate)Delegate.Remove(path.callback, new OnPathDelegate(this.OnFloodPathReady));
		base.StartCoroutine(this.CR_WaitFloodPathPipelineState());
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x000106E3 File Offset: 0x0000E8E3
	private IEnumerator CR_WaitFloodPathPipelineState()
	{
		while (!this.IsFloodPathReady())
		{
			yield return null;
		}
		EventMgr.SendEvent<int>(eGameEvents.OnFloodPathUpdate, this.spawnNodeIndex);
		yield break;
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x000106F2 File Offset: 0x0000E8F2
	public bool IsFloodPathReady()
	{
		return this.floodPath != null && this.floodPath.IsDone() && this.floodPath.PipelineState >= PathState.Returned;
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001071C File Offset: 0x0000E91C
	public FloodPathTracer GetFloodPathTracer(Vector3 startPos, OnPathDelegate callback)
	{
		if (this.floodPath == null)
		{
			Debug.LogError("floodPath is null");
		}
		if (!this.floodPath.IsDone())
		{
			Debug.LogError("floodPath not done");
		}
		return FloodPathTracer.Construct(startPos, this.floodPath, callback);
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00010754 File Offset: 0x0000E954
	public bool CheckPathBlockedByObject(GameObject obj, Vector3 startPos, Vector3 endPos, bool alwaysRevert = false)
	{
		GraphUpdateObject guo = new GraphUpdateObject(obj.GetComponent<Collider>().bounds);
		GraphNode node = AstarPath.active.GetNearest(startPos).node;
		GraphNode node2 = AstarPath.active.GetNearest(endPos).node;
		return !GraphUpdateUtilities.UpdateGraphsNoBlock(guo, node, node2, alwaysRevert);
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x000107A1 File Offset: 0x0000E9A1
	public bool CheckPathBlockedByObject(List<Collider> list_Colliders, bool alwaysRevert = false)
	{
		return this.CheckPathBlockedByObject(list_Colliders, this.SpawnPosition, this.endPosition, alwaysRevert);
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x000107B8 File Offset: 0x0000E9B8
	public bool CheckPathBlockedByObject(List<Collider> list_Colliders, Vector3 startPos, Vector3 endPos, bool alwaysRevert = false)
	{
		GraphUpdateObject graphUpdateObject;
		if (list_Colliders != null)
		{
			Bounds bounds = list_Colliders[0].bounds;
			foreach (Collider collider in list_Colliders)
			{
				bounds.Encapsulate(collider.bounds);
			}
			graphUpdateObject = new GraphUpdateObject(bounds);
		}
		else
		{
			graphUpdateObject = new GraphUpdateObject();
		}
		GraphNode node = AstarPath.active.GetNearest(startPos).node;
		GraphNode node2 = AstarPath.active.GetNearest(endPos).node;
		graphUpdateObject.trackChangedNodes = true;
		return !GraphUpdateUtilities.UpdateGraphsNoBlock(graphUpdateObject, node, node2, alwaysRevert);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00010868 File Offset: 0x0000EA68
	private IEnumerator CR_WaitFloodPathDone(Vector3 startPos, Vector3 endPos, FloodPath floodPath, GraphUpdateObject guo)
	{
		while (!this.IsFloodPathReady())
		{
			Debug.Log(string.Format("Waiting for FloodPath... ({0})", floodPath.PipelineState));
			yield return null;
		}
		this.CalculateFloodPathTracer(startPos, endPos, floodPath);
		yield break;
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0001088C File Offset: 0x0000EA8C
	public void CalculateFloodPath(Vector3 startPos, Vector3 endPos, Action<FloodPath> OnFinsh)
	{
		Debug.Log("CalculateFloodPath (1)");
		AstarPath.StartPath(FloodPath.Construct(endPos, delegate(Path floodPath)
		{
			Debug.Log("FloodPath DONE");
			Action<FloodPath> onFinsh = OnFinsh;
			if (onFinsh == null)
			{
				return;
			}
			onFinsh(floodPath as FloodPath);
		}), false);
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x000108C8 File Offset: 0x0000EAC8
	public void CalculateFloodPathTracer(Vector3 startPos, Vector3 endPos, FloodPath floodPath)
	{
		Debug.Log("CalculateFloodPathTracer (2)");
		AstarPath.StartPath(FloodPathTracer.Construct(startPos, floodPath, delegate(Path floodPathTracer)
		{
			Debug.Log("FloodPathTracer DONE");
			this.SetupPlacementLine(floodPathTracer);
		}), false);
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x000108F0 File Offset: 0x0000EAF0
	public void Spawn(eMonsterType type, bool isCorrupted)
	{
		Vector3 b = Random.insideUnitSphere * 2f;
		b.y = 0f;
		GameObject prefab = Singleton<ResourceManager>.Instance.GetMonsterDataByType(type).GetPrefab();
		Singleton<PrefabManager>.Instance.InstantiatePrefab(prefab, this.SpawnPosition + b, Quaternion.LookRotation(-1f * this.SpawnPosition, Vector3.up), base.transform).GetComponent<AMonsterBase>().Spawn(this, isCorrupted);
	}

	// Token: 0x0400041F RID: 1055
	[SerializeField]
	private int spawnNodeIndex;

	// Token: 0x04000420 RID: 1056
	[SerializeField]
	private LineRenderer lineRenderer;

	// Token: 0x04000421 RID: 1057
	[SerializeField]
	private LineRenderer lineRenderer_Placement;

	// Token: 0x04000422 RID: 1058
	[SerializeField]
	private GameObject node_SpawnPosition;

	// Token: 0x04000423 RID: 1059
	[SerializeField]
	private Material mat_PathLine_Incoming;

	// Token: 0x04000424 RID: 1060
	[SerializeField]
	private Material mat_PathLine_Unused;

	// Token: 0x04000425 RID: 1061
	[SerializeField]
	private float spawnInterval = 1f;

	// Token: 0x04000426 RID: 1062
	private FloodPath floodPath;

	// Token: 0x04000427 RID: 1063
	private FloodPath placementFloodPath;

	// Token: 0x04000428 RID: 1064
	private FloodPathTracer path;

	// Token: 0x04000429 RID: 1065
	private FloodPathTracer placementPath;

	// Token: 0x0400042A RID: 1066
	private float timer;

	// Token: 0x0400042B RID: 1067
	private Vector3 endPosition;

	// Token: 0x0400042C RID: 1068
	private bool isSpawnInThisWave;

	// Token: 0x0400042D RID: 1069
	private bool isInitialized;

	// Token: 0x0400042E RID: 1070
	private Coroutine coroutine_SetupPlacementLine;

	// Token: 0x0400042F RID: 1071
	private Coroutine coroutine_WaitFloodPathDone;
}
