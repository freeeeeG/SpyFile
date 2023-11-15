using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000002 RID: 2
[ExecuteInEditMode]
[AddComponentMenu("Pathfinding/Pathfinder")]
[HelpURL("http://arongranberg.com/astar/documentation/stable/class_astar_path.php")]
public class AstarPath : VersionedMonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	[Obsolete]
	public Type[] graphTypes
	{
		get
		{
			return this.data.graphTypes;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x0000205D File Offset: 0x0000025D
	[Obsolete("The 'astarData' field has been renamed to 'data'")]
	public AstarData astarData
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002065 File Offset: 0x00000265
	public NavGraph[] graphs
	{
		get
		{
			if (this.data == null)
			{
				this.data = new AstarData();
			}
			return this.data.graphs;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000004 RID: 4 RVA: 0x00002085 File Offset: 0x00000285
	public float maxNearestNodeDistanceSqr
	{
		get
		{
			return this.maxNearestNodeDistance * this.maxNearestNodeDistance;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000005 RID: 5 RVA: 0x00002094 File Offset: 0x00000294
	// (set) Token: 0x06000006 RID: 6 RVA: 0x0000209C File Offset: 0x0000029C
	[Obsolete("This field has been renamed to 'batchGraphUpdates'")]
	public bool limitGraphUpdates
	{
		get
		{
			return this.batchGraphUpdates;
		}
		set
		{
			this.batchGraphUpdates = value;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000007 RID: 7 RVA: 0x000020A5 File Offset: 0x000002A5
	// (set) Token: 0x06000008 RID: 8 RVA: 0x000020AD File Offset: 0x000002AD
	[Obsolete("This field has been renamed to 'graphUpdateBatchingInterval'")]
	public float maxGraphUpdateFreq
	{
		get
		{
			return this.graphUpdateBatchingInterval;
		}
		set
		{
			this.graphUpdateBatchingInterval = value;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000009 RID: 9 RVA: 0x000020B6 File Offset: 0x000002B6
	// (set) Token: 0x0600000A RID: 10 RVA: 0x000020BE File Offset: 0x000002BE
	public float lastScanTime { get; private set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600000B RID: 11 RVA: 0x000020C7 File Offset: 0x000002C7
	// (set) Token: 0x0600000C RID: 12 RVA: 0x000020CF File Offset: 0x000002CF
	public bool isScanning
	{
		get
		{
			return this.isScanningBacking;
		}
		private set
		{
			this.isScanningBacking = value;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600000D RID: 13 RVA: 0x000020D8 File Offset: 0x000002D8
	public int NumParallelThreads
	{
		get
		{
			return this.pathProcessor.NumThreads;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600000E RID: 14 RVA: 0x000020E5 File Offset: 0x000002E5
	public bool IsUsingMultithreading
	{
		get
		{
			return this.pathProcessor.IsUsingMultithreading;
		}
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600000F RID: 15 RVA: 0x000020F2 File Offset: 0x000002F2
	[Obsolete("Fixed grammar, use IsAnyGraphUpdateQueued instead")]
	public bool IsAnyGraphUpdatesQueued
	{
		get
		{
			return this.IsAnyGraphUpdateQueued;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000010 RID: 16 RVA: 0x000020FA File Offset: 0x000002FA
	public bool IsAnyGraphUpdateQueued
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateQueued;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000011 RID: 17 RVA: 0x00002107 File Offset: 0x00000307
	public bool IsAnyGraphUpdateInProgress
	{
		get
		{
			return this.graphUpdates.IsAnyGraphUpdateInProgress;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000012 RID: 18 RVA: 0x00002114 File Offset: 0x00000314
	public bool IsAnyWorkItemInProgress
	{
		get
		{
			return this.workItems.workItemsInProgress;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000013 RID: 19 RVA: 0x00002121 File Offset: 0x00000321
	internal bool IsInsideWorkItem
	{
		get
		{
			return this.workItems.workItemsInProgressRightNow;
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002130 File Offset: 0x00000330
	private AstarPath()
	{
		this.pathReturnQueue = new PathReturnQueue(this);
		this.pathProcessor = new PathProcessor(this, this.pathReturnQueue, 1, false);
		this.workItems = new WorkItemProcessor(this);
		this.graphUpdates = new GraphUpdateProcessor(this);
		this.graphUpdates.OnGraphsUpdated += delegate()
		{
			if (AstarPath.OnGraphsUpdated != null)
			{
				AstarPath.OnGraphsUpdated(this);
			}
		};
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002248 File Offset: 0x00000448
	public string[] GetTagNames()
	{
		if (this.tagNames == null || this.tagNames.Length != 32)
		{
			this.tagNames = new string[32];
			for (int i = 0; i < this.tagNames.Length; i++)
			{
				this.tagNames[i] = (i.ToString() ?? "");
			}
			this.tagNames[0] = "Basic Ground";
		}
		return this.tagNames;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000022B4 File Offset: 0x000004B4
	public static void FindAstarPath()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (AstarPath.active == null)
		{
			AstarPath.active = Object.FindObjectOfType<AstarPath>();
		}
		if (AstarPath.active != null && (AstarPath.active.data.graphs == null || AstarPath.active.data.graphs.Length == 0))
		{
			AstarPath.active.data.DeserializeGraphs();
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000231F File Offset: 0x0000051F
	public static string[] FindTagNames()
	{
		AstarPath.FindAstarPath();
		if (!(AstarPath.active != null))
		{
			return new string[]
			{
				"There is no AstarPath component in the scene"
			};
		}
		return AstarPath.active.GetTagNames();
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000234C File Offset: 0x0000054C
	internal ushort GetNextPathID()
	{
		if (this.nextFreePathID == 0)
		{
			this.nextFreePathID += 1;
			if (AstarPath.On65KOverflow != null)
			{
				Action on65KOverflow = AstarPath.On65KOverflow;
				AstarPath.On65KOverflow = null;
				on65KOverflow();
			}
		}
		ushort num = this.nextFreePathID;
		this.nextFreePathID = num + 1;
		return num;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0000239C File Offset: 0x0000059C
	private void RecalculateDebugLimits()
	{
		this.debugFloor = float.PositiveInfinity;
		this.debugRoof = float.NegativeInfinity;
		bool ignoreSearchTree = !this.showSearchTree || this.debugPathData == null;
		Action<GraphNode> <>9__0;
		for (int i = 0; i < this.graphs.Length; i++)
		{
			if (this.graphs[i] != null && this.graphs[i].drawGizmos)
			{
				NavGraph navGraph = this.graphs[i];
				Action<GraphNode> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(GraphNode node)
					{
						if (node.Walkable && (ignoreSearchTree || GraphGizmoHelper.InSearchTree(node, this.debugPathData, this.debugPathID)))
						{
							if (this.debugMode == GraphDebugMode.Penalty)
							{
								this.debugFloor = Mathf.Min(this.debugFloor, node.Penalty);
								this.debugRoof = Mathf.Max(this.debugRoof, node.Penalty);
								return;
							}
							if (this.debugPathData != null)
							{
								PathNode pathNode = this.debugPathData.GetPathNode(node);
								switch (this.debugMode)
								{
								case GraphDebugMode.G:
									this.debugFloor = Mathf.Min(this.debugFloor, pathNode.G);
									this.debugRoof = Mathf.Max(this.debugRoof, pathNode.G);
									return;
								case GraphDebugMode.H:
									this.debugFloor = Mathf.Min(this.debugFloor, pathNode.H);
									this.debugRoof = Mathf.Max(this.debugRoof, pathNode.H);
									break;
								case GraphDebugMode.F:
									this.debugFloor = Mathf.Min(this.debugFloor, pathNode.F);
									this.debugRoof = Mathf.Max(this.debugRoof, pathNode.F);
									return;
								default:
									return;
								}
							}
						}
					});
				}
				navGraph.GetNodes(action);
			}
		}
		if (float.IsInfinity(this.debugFloor))
		{
			this.debugFloor = 0f;
			this.debugRoof = 1f;
		}
		if (this.debugRoof - this.debugFloor < 1f)
		{
			this.debugRoof += 1f;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002488 File Offset: 0x00000688
	private void OnDrawGizmos()
	{
		if (AstarPath.active == null)
		{
			AstarPath.active = this;
		}
		if (AstarPath.active != this || this.graphs == null)
		{
			return;
		}
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		this.colorSettings.PushToStatic(this);
		if (this.workItems.workItemsInProgress || this.isScanning)
		{
			this.gizmos.DrawExisting();
		}
		else
		{
			if (this.showNavGraphs && !this.manualDebugFloorRoof)
			{
				this.RecalculateDebugLimits();
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null && this.graphs[i].drawGizmos)
				{
					this.graphs[i].OnDrawGizmos(this.gizmos, this.showNavGraphs);
				}
			}
			if (this.showNavGraphs)
			{
				this.euclideanEmbedding.OnDrawGizmos();
				if (this.debugMode == GraphDebugMode.HierarchicalNode)
				{
					this.hierarchicalGraph.OnDrawGizmos(this.gizmos);
				}
			}
		}
		this.gizmos.FinalizeDraw();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002590 File Offset: 0x00000790
	private void OnGUI()
	{
		if (this.logPathResults == PathLog.InGame && this.inGameDebugPath != "")
		{
			GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000025DC File Offset: 0x000007DC
	private void LogPathResults(Path path)
	{
		if (this.logPathResults != PathLog.None && (path.error || this.logPathResults != PathLog.OnlyErrors))
		{
			string message = ((IPathInternals)path).DebugString(this.logPathResults);
			if (this.logPathResults == PathLog.InGame)
			{
				this.inGameDebugPath = message;
				return;
			}
			if (path.error)
			{
				Debug.LogWarning(message);
				return;
			}
			Debug.Log(message);
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002635 File Offset: 0x00000835
	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.navmeshUpdates.Update();
		if (!this.isScanning)
		{
			this.PerformBlockingActions(false);
		}
		this.pathProcessor.TickNonMultithreaded();
		this.pathReturnQueue.ReturnPaths(true);
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002670 File Offset: 0x00000870
	private void PerformBlockingActions(bool force = false)
	{
		if (this.workItemLock.Held && this.pathProcessor.queue.AllReceiversBlocked)
		{
			this.pathReturnQueue.ReturnPaths(false);
			if (this.workItems.ProcessWorkItems(force))
			{
				this.workItemLock.Release();
			}
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000026C1 File Offset: 0x000008C1
	[Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
	public void QueueWorkItemFloodFill()
	{
		throw new Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000026CD File Offset: 0x000008CD
	[Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
	public void EnsureValidFloodFill()
	{
		throw new Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000026D9 File Offset: 0x000008D9
	public void AddWorkItem(Action callback)
	{
		this.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000026E8 File Offset: 0x000008E8
	public void AddWorkItem(Action<IWorkItemContext> callback)
	{
		this.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000026F7 File Offset: 0x000008F7
	public void AddWorkItem(AstarWorkItem item)
	{
		this.workItems.AddWorkItem(item);
		if (!this.workItemLock.Held)
		{
			this.workItemLock = this.PausePathfindingSoon();
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002720 File Offset: 0x00000920
	public void QueueGraphUpdates()
	{
		if (!this.graphUpdatesWorkItemAdded)
		{
			this.graphUpdatesWorkItemAdded = true;
			AstarWorkItem workItem = this.graphUpdates.GetWorkItem();
			this.AddWorkItem(new AstarWorkItem(delegate()
			{
				this.graphUpdatesWorkItemAdded = false;
				this.lastGraphUpdate = Time.realtimeSinceStartup;
				workItem.init();
			}, workItem.update));
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0000277C File Offset: 0x0000097C
	private IEnumerator DelayedGraphUpdate()
	{
		this.graphUpdateRoutineRunning = true;
		yield return new WaitForSeconds(this.graphUpdateBatchingInterval - (Time.realtimeSinceStartup - this.lastGraphUpdate));
		this.QueueGraphUpdates();
		this.graphUpdateRoutineRunning = false;
		yield break;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x0000278B File Offset: 0x0000098B
	public void UpdateGraphs(Bounds bounds, float delay)
	{
		this.UpdateGraphs(new GraphUpdateObject(bounds), delay);
	}

	// Token: 0x06000027 RID: 39 RVA: 0x0000279A File Offset: 0x0000099A
	public void UpdateGraphs(GraphUpdateObject ob, float delay)
	{
		base.StartCoroutine(this.UpdateGraphsInternal(ob, delay));
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000027AB File Offset: 0x000009AB
	private IEnumerator UpdateGraphsInternal(GraphUpdateObject ob, float delay)
	{
		yield return new WaitForSeconds(delay);
		this.UpdateGraphs(ob);
		yield break;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000027C8 File Offset: 0x000009C8
	public void UpdateGraphs(Bounds bounds)
	{
		this.UpdateGraphs(new GraphUpdateObject(bounds));
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000027D8 File Offset: 0x000009D8
	public void UpdateGraphs(GraphUpdateObject ob)
	{
		if (ob.internalStage != -1)
		{
			throw new Exception("You are trying to update graphs using the same graph update object twice. Please create a new GraphUpdateObject instead.");
		}
		ob.internalStage = -2;
		this.graphUpdates.AddToQueue(ob);
		if (this.batchGraphUpdates && Time.realtimeSinceStartup - this.lastGraphUpdate < this.graphUpdateBatchingInterval)
		{
			if (!this.graphUpdateRoutineRunning)
			{
				base.StartCoroutine(this.DelayedGraphUpdate());
				return;
			}
		}
		else
		{
			this.QueueGraphUpdates();
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00002845 File Offset: 0x00000A45
	public void FlushGraphUpdates()
	{
		if (this.IsAnyGraphUpdateQueued)
		{
			this.QueueGraphUpdates();
			this.FlushWorkItems();
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x0000285C File Offset: 0x00000A5C
	public void FlushWorkItems()
	{
		if (this.workItems.anyQueued)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
			this.PerformBlockingActions(true);
			graphUpdateLock.Release();
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0000288C File Offset: 0x00000A8C
	[Obsolete("Use FlushWorkItems() instead")]
	public void FlushWorkItems(bool unblockOnComplete, bool block)
	{
		PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
		this.PerformBlockingActions(block);
		graphUpdateLock.Release();
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000028AE File Offset: 0x00000AAE
	[Obsolete("Use FlushWorkItems instead")]
	public void FlushThreadSafeCallbacks()
	{
		this.FlushWorkItems();
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000028B8 File Offset: 0x00000AB8
	public static int CalculateThreadCount(ThreadCount count)
	{
		if (count != ThreadCount.AutomaticLowLoad && count != ThreadCount.AutomaticHighLoad)
		{
			return (int)count;
		}
		int num = Mathf.Max(1, SystemInfo.processorCount);
		int num2 = SystemInfo.systemMemorySize;
		if (num2 <= 0)
		{
			Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
			num2 = 1024;
		}
		if (num <= 1)
		{
			return 0;
		}
		if (num2 <= 512)
		{
			return 0;
		}
		if (count == ThreadCount.AutomaticHighLoad)
		{
			if (num2 <= 1024)
			{
				num = Math.Min(num, 2);
			}
		}
		else
		{
			num /= 2;
			num = Mathf.Max(1, num);
			if (num2 <= 1024)
			{
				num = Math.Min(num, 2);
			}
			num = Math.Min(num, 6);
		}
		return num;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002944 File Offset: 0x00000B44
	protected override void Awake()
	{
		base.Awake();
		if (AstarPath.active != null && AstarPath.active != this && Application.isPlaying)
		{
			if (base.enabled)
			{
				Debug.LogWarning("Another A* component is already in the scene. More than one A* component cannot be active at the same time. Disabling this one.", this);
			}
			base.enabled = false;
			return;
		}
		AstarPath.active = this;
		if (Object.FindObjectsOfType(typeof(AstarPath)).Length > 1)
		{
			Debug.LogError("You should NOT have more than one AstarPath component in the scene at any time.\nThis can cause serious errors since the AstarPath component builds around a singleton pattern.", this);
		}
		base.useGUILayout = false;
		if (!Application.isPlaying)
		{
			return;
		}
		if (AstarPath.OnAwakeSettings != null)
		{
			AstarPath.OnAwakeSettings();
		}
		GraphModifier.FindAllModifiers();
		RelevantGraphSurface.FindAllGraphSurfaces();
		this.InitializePathProcessor();
		this.InitializeProfiler();
		this.ConfigureReferencesInternal();
		this.InitializeAstarData();
		this.FlushWorkItems();
		this.euclideanEmbedding.dirty = true;
		this.navmeshUpdates.OnEnable();
		if (this.scanOnStartup && (!this.data.cacheStartup || this.data.file_cachedStartup == null))
		{
			this.Scan(null);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002A48 File Offset: 0x00000C48
	private void InitializePathProcessor()
	{
		int num = AstarPath.CalculateThreadCount(this.threadCount);
		if (!Application.isPlaying)
		{
			num = 0;
		}
		int processors = Mathf.Max(num, 1);
		bool flag = num > 0;
		this.pathProcessor = new PathProcessor(this, this.pathReturnQueue, processors, flag);
		this.pathProcessor.OnPathPreSearch += delegate(Path path)
		{
			OnPathDelegate onPathPreSearch = AstarPath.OnPathPreSearch;
			if (onPathPreSearch != null)
			{
				onPathPreSearch(path);
			}
		};
		this.pathProcessor.OnPathPostSearch += delegate(Path path)
		{
			this.LogPathResults(path);
			OnPathDelegate onPathPostSearch = AstarPath.OnPathPostSearch;
			if (onPathPostSearch != null)
			{
				onPathPostSearch(path);
			}
		};
		this.pathProcessor.OnQueueUnblocked += delegate()
		{
			if (this.euclideanEmbedding.dirty)
			{
				this.euclideanEmbedding.RecalculateCosts();
			}
		};
		if (flag)
		{
			this.graphUpdates.EnableMultithreading();
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002AF4 File Offset: 0x00000CF4
	internal void VerifyIntegrity()
	{
		if (AstarPath.active != this)
		{
			throw new Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
		}
		if (this.data == null)
		{
			throw new NullReferenceException("data is null... A* not set up correctly?");
		}
		if (this.data.graphs == null)
		{
			this.data.graphs = new NavGraph[0];
			this.data.UpdateShortcuts();
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002B55 File Offset: 0x00000D55
	public void ConfigureReferencesInternal()
	{
		AstarPath.active = this;
		this.data = (this.data ?? new AstarData());
		this.colorSettings = (this.colorSettings ?? new AstarColor());
		this.colorSettings.PushToStatic(this);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002B93 File Offset: 0x00000D93
	private void InitializeProfiler()
	{
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002B95 File Offset: 0x00000D95
	private void InitializeAstarData()
	{
		this.data.FindGraphTypes();
		this.data.Awake();
		this.data.UpdateShortcuts();
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002BB8 File Offset: 0x00000DB8
	private void OnDisable()
	{
		this.gizmos.ClearCache();
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002BC8 File Offset: 0x00000DC8
	private void OnDestroy()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
		}
		if (AstarPath.active != this)
		{
			return;
		}
		this.PausePathfinding();
		this.navmeshUpdates.OnDisable();
		this.euclideanEmbedding.dirty = false;
		this.FlushWorkItems();
		this.pathProcessor.queue.TerminateReceivers();
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Processing Possible Work Items");
		}
		this.graphUpdates.DisableMultithreading();
		this.pathProcessor.JoinThreads();
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Returning Paths");
		}
		this.pathReturnQueue.ReturnPaths(false);
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Destroying Graphs");
		}
		if (this.data != null)
		{
			this.data.OnDestroy();
		}
		if (this.logPathResults == PathLog.Heavy)
		{
			Debug.Log("Cleaning up variables");
		}
		AstarPath.OnAwakeSettings = null;
		AstarPath.OnGraphPreScan = null;
		AstarPath.OnGraphPostScan = null;
		AstarPath.OnPathPreSearch = null;
		AstarPath.OnPathPostSearch = null;
		AstarPath.OnPreScan = null;
		AstarPath.OnPostScan = null;
		AstarPath.OnLatePostScan = null;
		AstarPath.On65KOverflow = null;
		AstarPath.OnGraphsUpdated = null;
		AstarPath.active = null;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002CF5 File Offset: 0x00000EF5
	[Obsolete("Not meaningful anymore. The HierarchicalGraph takes care of things automatically behind the scenes")]
	public void FloodFill(GraphNode seed)
	{
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002CF7 File Offset: 0x00000EF7
	[Obsolete("Not meaningful anymore. The HierarchicalGraph takes care of things automatically behind the scenes")]
	public void FloodFill(GraphNode seed, uint area)
	{
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002CF9 File Offset: 0x00000EF9
	[ContextMenu("Flood Fill Graphs")]
	[Obsolete("Avoid using. This will force a full recalculation of the connected components. In most cases the HierarchicalGraph class takes care of things automatically behind the scenes now.")]
	public void FloodFill()
	{
		this.hierarchicalGraph.RecalculateAll();
		this.workItems.OnFloodFill();
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002D11 File Offset: 0x00000F11
	internal int GetNewNodeIndex()
	{
		return this.pathProcessor.GetNewNodeIndex();
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002D1E File Offset: 0x00000F1E
	internal void InitializeNode(GraphNode node)
	{
		this.pathProcessor.InitializeNode(node);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002D2C File Offset: 0x00000F2C
	internal void DestroyNode(GraphNode node)
	{
		this.pathProcessor.DestroyNode(node);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002D3A File Offset: 0x00000F3A
	[Obsolete("Use PausePathfinding instead. Make sure to call Release on the returned lock.", true)]
	public void BlockUntilPathQueueBlocked()
	{
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002D3C File Offset: 0x00000F3C
	public PathProcessor.GraphUpdateLock PausePathfinding()
	{
		return this.pathProcessor.PausePathfinding(true);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002D4A File Offset: 0x00000F4A
	private PathProcessor.GraphUpdateLock PausePathfindingSoon()
	{
		return this.pathProcessor.PausePathfinding(false);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002D58 File Offset: 0x00000F58
	public void Scan(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		this.Scan(new NavGraph[]
		{
			graphToScan
		});
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00002D74 File Offset: 0x00000F74
	public void Scan(NavGraph[] graphsToScan = null)
	{
		Progress progress = default(Progress);
		foreach (Progress progress2 in this.ScanAsync(graphsToScan))
		{
			progress.description != progress2.description;
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002DD8 File Offset: 0x00000FD8
	public IEnumerable<Progress> ScanAsync(NavGraph graphToScan)
	{
		if (graphToScan == null)
		{
			throw new ArgumentNullException();
		}
		return this.ScanAsync(new NavGraph[]
		{
			graphToScan
		});
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002DF3 File Offset: 0x00000FF3
	public IEnumerable<Progress> ScanAsync(NavGraph[] graphsToScan = null)
	{
		if (graphsToScan == null)
		{
			graphsToScan = this.graphs;
		}
		if (graphsToScan == null)
		{
			yield break;
		}
		if (this.isScanning)
		{
			throw new InvalidOperationException("Another async scan is already running");
		}
		this.isScanning = true;
		this.VerifyIntegrity();
		PathProcessor.GraphUpdateLock graphUpdateLock = this.PausePathfinding();
		this.pathReturnQueue.ReturnPaths(false);
		if (!Application.isPlaying)
		{
			this.data.FindGraphTypes();
			GraphModifier.FindAllModifiers();
		}
		yield return new Progress(0.05f, "Pre processing graphs");
		if (AstarPath.OnPreScan != null)
		{
			AstarPath.OnPreScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
		this.data.LockGraphStructure(false);
		Physics2D.SyncTransforms();
		Stopwatch watch = Stopwatch.StartNew();
		for (int j = 0; j < graphsToScan.Length; j++)
		{
			if (graphsToScan[j] != null)
			{
				((IGraphInternals)graphsToScan[j]).DestroyAllNodes();
			}
		}
		int num;
		for (int i = 0; i < graphsToScan.Length; i = num + 1)
		{
			if (graphsToScan[i] != null)
			{
				float minp = Mathf.Lerp(0.1f, 0.8f, (float)i / (float)graphsToScan.Length);
				float maxp = Mathf.Lerp(0.1f, 0.8f, ((float)i + 0.95f) / (float)graphsToScan.Length);
				string progressDescriptionPrefix = string.Concat(new string[]
				{
					"Scanning graph ",
					(i + 1).ToString(),
					" of ",
					graphsToScan.Length.ToString(),
					" - "
				});
				IEnumerator<Progress> coroutine = this.ScanGraph(graphsToScan[i]).GetEnumerator();
				for (;;)
				{
					try
					{
						if (!coroutine.MoveNext())
						{
							break;
						}
					}
					catch
					{
						this.isScanning = false;
						this.data.UnlockGraphStructure();
						graphUpdateLock.Release();
						throw;
					}
					Progress progress = coroutine.Current;
					yield return progress.MapTo(minp, maxp, progressDescriptionPrefix);
				}
				progressDescriptionPrefix = null;
				coroutine = null;
			}
			num = i;
		}
		this.data.UnlockGraphStructure();
		yield return new Progress(0.8f, "Post processing graphs");
		if (AstarPath.OnPostScan != null)
		{
			AstarPath.OnPostScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
		this.FlushWorkItems();
		yield return new Progress(0.9f, "Computing areas");
		this.hierarchicalGraph.RecalculateIfNecessary();
		yield return new Progress(0.95f, "Late post processing");
		this.isScanning = false;
		if (AstarPath.OnLatePostScan != null)
		{
			AstarPath.OnLatePostScan(this);
		}
		GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
		this.euclideanEmbedding.dirty = true;
		this.euclideanEmbedding.RecalculatePivots();
		this.FlushWorkItems();
		graphUpdateLock.Release();
		watch.Stop();
		this.lastScanTime = (float)watch.Elapsed.TotalSeconds;
		if (this.logPathResults != PathLog.None && this.logPathResults != PathLog.OnlyErrors)
		{
			Debug.Log("Scanning - Process took " + (this.lastScanTime * 1000f).ToString("0") + " ms to complete");
		}
		yield break;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00002E0A File Offset: 0x0000100A
	private IEnumerable<Progress> ScanGraph(NavGraph graph)
	{
		if (AstarPath.OnGraphPreScan != null)
		{
			yield return new Progress(0f, "Pre processing");
			AstarPath.OnGraphPreScan(graph);
		}
		yield return new Progress(0f, "");
		foreach (Progress progress in ((IGraphInternals)graph).ScanInternal())
		{
			yield return progress.MapTo(0f, 0.95f, null);
		}
		IEnumerator<Progress> enumerator = null;
		yield return new Progress(0.95f, "Assigning graph indices");
		graph.GetNodes(delegate(GraphNode node)
		{
			node.GraphIndex = graph.graphIndex;
		});
		if (AstarPath.OnGraphPostScan != null)
		{
			yield return new Progress(0.99f, "Post processing");
			AstarPath.OnGraphPostScan(graph);
		}
		yield break;
		yield break;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002E1A File Offset: 0x0000101A
	[Obsolete("This method has been renamed to BlockUntilCalculated")]
	public static void WaitForPath(Path path)
	{
		AstarPath.BlockUntilCalculated(path);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00002E24 File Offset: 0x00001024
	public static void BlockUntilCalculated(Path path)
	{
		if (AstarPath.active == null)
		{
			throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
		}
		if (path == null)
		{
			throw new ArgumentNullException("Path must not be null");
		}
		if (AstarPath.active.pathProcessor.queue.IsTerminating)
		{
			return;
		}
		if (path.PipelineState == PathState.Created)
		{
			throw new Exception("The specified path has not been started yet.");
		}
		AstarPath.waitForPathDepth++;
		if (AstarPath.waitForPathDepth == 5)
		{
			Debug.LogError("You are calling the BlockUntilCalculated function recursively (maybe from a path callback). Please don't do this.");
		}
		if (path.PipelineState < PathState.ReturnQueue)
		{
			if (AstarPath.active.IsUsingMultithreading)
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (AstarPath.active.pathProcessor.queue.IsTerminating)
					{
						AstarPath.waitForPathDepth--;
						throw new Exception("Pathfinding Threads seem to have crashed.");
					}
					Thread.Sleep(1);
					AstarPath.active.PerformBlockingActions(true);
				}
			}
			else
			{
				while (path.PipelineState < PathState.ReturnQueue)
				{
					if (AstarPath.active.pathProcessor.queue.IsEmpty && path.PipelineState != PathState.Processing)
					{
						AstarPath.waitForPathDepth--;
						throw new Exception("Critical error. Path Queue is empty but the path state is '" + path.PipelineState.ToString() + "'");
					}
					AstarPath.active.pathProcessor.TickNonMultithreaded();
					AstarPath.active.PerformBlockingActions(true);
				}
			}
		}
		AstarPath.active.pathReturnQueue.ReturnPaths(false);
		AstarPath.waitForPathDepth--;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00002F98 File Offset: 0x00001198
	[Obsolete("Use AddWorkItem(System.Action) instead. Note the slight change in behavior (mentioned in the documentation).")]
	public static void RegisterSafeUpdate(Action callback)
	{
		AstarPath.active.AddWorkItem(new AstarWorkItem(callback, null));
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00002FAC File Offset: 0x000011AC
	public static void StartPath(Path path, bool pushToFront = false)
	{
		AstarPath astarPath = AstarPath.active;
		if (astarPath == null)
		{
			Debug.LogError("There is no AstarPath object in the scene or it has not been initialized yet");
			return;
		}
		if (path.PipelineState != PathState.Created)
		{
			throw new Exception(string.Concat(new string[]
			{
				"The path has an invalid state. Expected ",
				PathState.Created.ToString(),
				" found ",
				path.PipelineState.ToString(),
				"\nMake sure you are not requesting the same path twice"
			}));
		}
		if (astarPath.pathProcessor.queue.IsTerminating)
		{
			path.FailWithError("No new paths are accepted");
			return;
		}
		if (astarPath.graphs == null || astarPath.graphs.Length == 0)
		{
			Debug.LogError("There are no graphs in the scene");
			path.FailWithError("There are no graphs in the scene");
			Debug.LogError(path.errorLog);
			return;
		}
		path.Claim(astarPath);
		((IPathInternals)path).AdvanceState(PathState.PathQueue);
		if (pushToFront)
		{
			astarPath.pathProcessor.queue.PushFront(path);
		}
		else
		{
			astarPath.pathProcessor.queue.Push(path);
		}
		if (!Application.isPlaying)
		{
			AstarPath.BlockUntilCalculated(path);
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000030B9 File Offset: 0x000012B9
	public NNInfo GetNearest(Vector3 position)
	{
		return this.GetNearest(position, AstarPath.NNConstraintNone);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000030C7 File Offset: 0x000012C7
	public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
	{
		return this.GetNearest(position, constraint, null);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000030D4 File Offset: 0x000012D4
	public NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
	{
		NavGraph[] graphs = this.graphs;
		float num = float.PositiveInfinity;
		NNInfoInternal nninfoInternal = default(NNInfoInternal);
		int num2 = -1;
		if (graphs != null)
		{
			for (int i = 0; i < graphs.Length; i++)
			{
				NavGraph navGraph = graphs[i];
				if (navGraph != null && constraint.SuitableGraph(i, navGraph))
				{
					NNInfoInternal nninfoInternal2;
					if (this.fullGetNearestSearch)
					{
						nninfoInternal2 = navGraph.GetNearestForce(position, constraint);
					}
					else
					{
						nninfoInternal2 = navGraph.GetNearest(position, constraint);
					}
					if (nninfoInternal2.node != null)
					{
						float magnitude = (nninfoInternal2.clampedPosition - position).magnitude;
						if (this.prioritizeGraphs && magnitude < this.prioritizeGraphsLimit)
						{
							nninfoInternal = nninfoInternal2;
							num2 = i;
							break;
						}
						if (magnitude < num)
						{
							num = magnitude;
							nninfoInternal = nninfoInternal2;
							num2 = i;
						}
					}
				}
			}
		}
		if (num2 == -1)
		{
			return default(NNInfo);
		}
		if (nninfoInternal.constrainedNode != null)
		{
			nninfoInternal.node = nninfoInternal.constrainedNode;
			nninfoInternal.clampedPosition = nninfoInternal.constClampedPosition;
		}
		if (!this.fullGetNearestSearch && nninfoInternal.node != null && !constraint.Suitable(nninfoInternal.node))
		{
			NNInfoInternal nearestForce = graphs[num2].GetNearestForce(position, constraint);
			if (nearestForce.node != null)
			{
				nninfoInternal = nearestForce;
			}
		}
		if (!constraint.Suitable(nninfoInternal.node) || (constraint.constrainDistance && (nninfoInternal.clampedPosition - position).sqrMagnitude > this.maxNearestNodeDistanceSqr))
		{
			return default(NNInfo);
		}
		return new NNInfo(nninfoInternal);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003244 File Offset: 0x00001444
	public GraphNode GetNearest(Ray ray)
	{
		if (this.graphs == null)
		{
			return null;
		}
		float minDist = float.PositiveInfinity;
		GraphNode nearestNode = null;
		Vector3 lineDirection = ray.direction;
		Vector3 lineOrigin = ray.origin;
		Action<GraphNode> <>9__0;
		for (int i = 0; i < this.graphs.Length; i++)
		{
			NavGraph navGraph = this.graphs[i];
			Action<GraphNode> action;
			if ((action = <>9__0) == null)
			{
				action = (<>9__0 = delegate(GraphNode node)
				{
					Vector3 vector = (Vector3)node.position;
					Vector3 vector2 = lineOrigin + Vector3.Dot(vector - lineOrigin, lineDirection) * lineDirection;
					float num = Mathf.Abs(vector2.x - vector.x);
					if (num * num > minDist)
					{
						return;
					}
					float num2 = Mathf.Abs(vector2.z - vector.z);
					if (num2 * num2 > minDist)
					{
						return;
					}
					float sqrMagnitude = (vector2 - vector).sqrMagnitude;
					if (sqrMagnitude < minDist)
					{
						minDist = sqrMagnitude;
						nearestNode = node;
					}
				});
			}
			navGraph.GetNodes(action);
		}
		return nearestNode;
	}

	// Token: 0x04000001 RID: 1
	public static readonly Version Version = new Version(4, 2, 18);

	// Token: 0x04000002 RID: 2
	public static readonly AstarPath.AstarDistribution Distribution = AstarPath.AstarDistribution.WebsiteDownload;

	// Token: 0x04000003 RID: 3
	public static readonly string Branch = "master";

	// Token: 0x04000004 RID: 4
	[FormerlySerializedAs("astarData")]
	public AstarData data;

	// Token: 0x04000005 RID: 5
	public static AstarPath active;

	// Token: 0x04000006 RID: 6
	public bool showNavGraphs = true;

	// Token: 0x04000007 RID: 7
	public bool showUnwalkableNodes = true;

	// Token: 0x04000008 RID: 8
	public GraphDebugMode debugMode;

	// Token: 0x04000009 RID: 9
	public float debugFloor;

	// Token: 0x0400000A RID: 10
	public float debugRoof = 20000f;

	// Token: 0x0400000B RID: 11
	public bool manualDebugFloorRoof;

	// Token: 0x0400000C RID: 12
	public bool showSearchTree;

	// Token: 0x0400000D RID: 13
	public float unwalkableNodeDebugSize = 0.3f;

	// Token: 0x0400000E RID: 14
	public PathLog logPathResults = PathLog.Normal;

	// Token: 0x0400000F RID: 15
	public float maxNearestNodeDistance = 100f;

	// Token: 0x04000010 RID: 16
	public bool scanOnStartup = true;

	// Token: 0x04000011 RID: 17
	public bool fullGetNearestSearch;

	// Token: 0x04000012 RID: 18
	[Obsolete("This setting is discouraged, and it will be removed in a future update")]
	public bool prioritizeGraphs;

	// Token: 0x04000013 RID: 19
	[Obsolete("This setting is discouraged, and it will be removed in a future update")]
	public float prioritizeGraphsLimit = 1f;

	// Token: 0x04000014 RID: 20
	public AstarColor colorSettings;

	// Token: 0x04000015 RID: 21
	[SerializeField]
	protected string[] tagNames;

	// Token: 0x04000016 RID: 22
	public Heuristic heuristic = Heuristic.Euclidean;

	// Token: 0x04000017 RID: 23
	public float heuristicScale = 1f;

	// Token: 0x04000018 RID: 24
	public ThreadCount threadCount = ThreadCount.One;

	// Token: 0x04000019 RID: 25
	public float maxFrameTime = 1f;

	// Token: 0x0400001A RID: 26
	public bool batchGraphUpdates;

	// Token: 0x0400001B RID: 27
	public float graphUpdateBatchingInterval = 0.2f;

	// Token: 0x0400001D RID: 29
	[NonSerialized]
	public PathHandler debugPathData;

	// Token: 0x0400001E RID: 30
	[NonSerialized]
	public ushort debugPathID;

	// Token: 0x0400001F RID: 31
	private string inGameDebugPath;

	// Token: 0x04000020 RID: 32
	[NonSerialized]
	private bool isScanningBacking;

	// Token: 0x04000021 RID: 33
	public static Action OnAwakeSettings;

	// Token: 0x04000022 RID: 34
	public static OnGraphDelegate OnGraphPreScan;

	// Token: 0x04000023 RID: 35
	public static OnGraphDelegate OnGraphPostScan;

	// Token: 0x04000024 RID: 36
	public static OnPathDelegate OnPathPreSearch;

	// Token: 0x04000025 RID: 37
	public static OnPathDelegate OnPathPostSearch;

	// Token: 0x04000026 RID: 38
	public static OnScanDelegate OnPreScan;

	// Token: 0x04000027 RID: 39
	public static OnScanDelegate OnPostScan;

	// Token: 0x04000028 RID: 40
	public static OnScanDelegate OnLatePostScan;

	// Token: 0x04000029 RID: 41
	public static OnScanDelegate OnGraphsUpdated;

	// Token: 0x0400002A RID: 42
	public static Action On65KOverflow;

	// Token: 0x0400002B RID: 43
	[Obsolete]
	public Action OnGraphsWillBeUpdated;

	// Token: 0x0400002C RID: 44
	[Obsolete]
	public Action OnGraphsWillBeUpdated2;

	// Token: 0x0400002D RID: 45
	private readonly GraphUpdateProcessor graphUpdates;

	// Token: 0x0400002E RID: 46
	internal readonly HierarchicalGraph hierarchicalGraph = new HierarchicalGraph();

	// Token: 0x0400002F RID: 47
	public readonly NavmeshUpdates navmeshUpdates = new NavmeshUpdates();

	// Token: 0x04000030 RID: 48
	private readonly WorkItemProcessor workItems;

	// Token: 0x04000031 RID: 49
	private PathProcessor pathProcessor;

	// Token: 0x04000032 RID: 50
	private bool graphUpdateRoutineRunning;

	// Token: 0x04000033 RID: 51
	private bool graphUpdatesWorkItemAdded;

	// Token: 0x04000034 RID: 52
	private float lastGraphUpdate = -9999f;

	// Token: 0x04000035 RID: 53
	private PathProcessor.GraphUpdateLock workItemLock;

	// Token: 0x04000036 RID: 54
	internal readonly PathReturnQueue pathReturnQueue;

	// Token: 0x04000037 RID: 55
	public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();

	// Token: 0x04000038 RID: 56
	public bool showGraphs;

	// Token: 0x04000039 RID: 57
	private ushort nextFreePathID = 1;

	// Token: 0x0400003A RID: 58
	private RetainedGizmos gizmos = new RetainedGizmos();

	// Token: 0x0400003B RID: 59
	private static int waitForPathDepth = 0;

	// Token: 0x0400003C RID: 60
	private static readonly NNConstraint NNConstraintNone = NNConstraint.None;

	// Token: 0x020000F8 RID: 248
	public enum AstarDistribution
	{
		// Token: 0x0400064B RID: 1611
		WebsiteDownload,
		// Token: 0x0400064C RID: 1612
		AssetStore,
		// Token: 0x0400064D RID: 1613
		PackageManager
	}
}
