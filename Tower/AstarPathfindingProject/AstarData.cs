using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Pathfinding.WindowsStore;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pathfinding
{
	// Token: 0x0200002E RID: 46
	[Serializable]
	public class AstarData
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00008FFC File Offset: 0x000071FC
		public static AstarPath active
		{
			get
			{
				return AstarPath.active;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00009003 File Offset: 0x00007203
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000900B File Offset: 0x0000720B
		public NavMeshGraph navmesh { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00009014 File Offset: 0x00007214
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000901C File Offset: 0x0000721C
		public GridGraph gridGraph { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00009025 File Offset: 0x00007225
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000902D File Offset: 0x0000722D
		public LayerGridGraph layerGridGraph { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00009036 File Offset: 0x00007236
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000903E File Offset: 0x0000723E
		public PointGraph pointGraph { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00009047 File Offset: 0x00007247
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0000904F File Offset: 0x0000724F
		public RecastGraph recastGraph { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00009058 File Offset: 0x00007258
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00009060 File Offset: 0x00007260
		public Type[] graphTypes { get; private set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00009069 File Offset: 0x00007269
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x000090A4 File Offset: 0x000072A4
		private byte[] data
		{
			get
			{
				if (this.upgradeData != null && this.upgradeData.Length != 0)
				{
					this.data = this.upgradeData;
					this.upgradeData = null;
				}
				if (this.dataString == null)
				{
					return null;
				}
				return Convert.FromBase64String(this.dataString);
			}
			set
			{
				this.dataString = ((value != null) ? Convert.ToBase64String(value) : null);
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000090B8 File Offset: 0x000072B8
		public byte[] GetData()
		{
			return this.data;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000090C0 File Offset: 0x000072C0
		public void SetData(byte[] data)
		{
			this.data = data;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x000090C9 File Offset: 0x000072C9
		public void Awake()
		{
			this.graphs = new NavGraph[0];
			if (this.cacheStartup && this.file_cachedStartup != null)
			{
				this.LoadFromCache();
				return;
			}
			this.DeserializeGraphs();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000090FA File Offset: 0x000072FA
		internal void LockGraphStructure(bool allowAddingGraphs = false)
		{
			this.graphStructureLocked.Add(allowAddingGraphs);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00009108 File Offset: 0x00007308
		internal void UnlockGraphStructure()
		{
			if (this.graphStructureLocked.Count == 0)
			{
				throw new InvalidOperationException();
			}
			this.graphStructureLocked.RemoveAt(this.graphStructureLocked.Count - 1);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00009138 File Offset: 0x00007338
		private PathProcessor.GraphUpdateLock AssertSafe(bool onlyAddingGraph = false)
		{
			if (this.graphStructureLocked.Count > 0)
			{
				bool flag = true;
				for (int i = 0; i < this.graphStructureLocked.Count; i++)
				{
					flag &= this.graphStructureLocked[i];
				}
				if (!onlyAddingGraph || !flag)
				{
					throw new InvalidOperationException("Graphs cannot be added, removed or serialized while the graph structure is locked. This is the case when a graph is currently being scanned and when executing graph updates and work items.\nHowever as a special case, graphs can be added inside work items.");
				}
			}
			PathProcessor.GraphUpdateLock result = AstarData.active.PausePathfinding();
			if (!AstarData.active.IsInsideWorkItem)
			{
				AstarData.active.FlushWorkItems();
				AstarData.active.pathReturnQueue.ReturnPaths(false);
			}
			return result;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000091BC File Offset: 0x000073BC
		public void GetNodes(Action<GraphNode> callback)
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(callback);
				}
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000091F4 File Offset: 0x000073F4
		public void UpdateShortcuts()
		{
			this.navmesh = (NavMeshGraph)this.FindGraphOfType(typeof(NavMeshGraph));
			this.gridGraph = (GridGraph)this.FindGraphOfType(typeof(GridGraph));
			this.layerGridGraph = (LayerGridGraph)this.FindGraphOfType(typeof(LayerGridGraph));
			this.pointGraph = (PointGraph)this.FindGraphOfType(typeof(PointGraph));
			this.recastGraph = (RecastGraph)this.FindGraphOfType(typeof(RecastGraph));
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00009288 File Offset: 0x00007488
		public void LoadFromCache()
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.AssertSafe(false);
			if (this.file_cachedStartup != null)
			{
				byte[] bytes = this.file_cachedStartup.bytes;
				this.DeserializeGraphs(bytes);
				GraphModifier.TriggerEvent(GraphModifier.EventType.PostCacheLoad);
			}
			else
			{
				Debug.LogError("Can't load from cache since the cache is empty");
			}
			graphUpdateLock.Release();
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000092D8 File Offset: 0x000074D8
		public byte[] SerializeGraphs()
		{
			return this.SerializeGraphs(SerializeSettings.Settings);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000092E8 File Offset: 0x000074E8
		public byte[] SerializeGraphs(SerializeSettings settings)
		{
			uint num;
			return this.SerializeGraphs(settings, out num);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00009300 File Offset: 0x00007500
		public byte[] SerializeGraphs(SerializeSettings settings, out uint checksum)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.AssertSafe(false);
			AstarSerializer astarSerializer = new AstarSerializer(this, settings, AstarData.active.gameObject);
			astarSerializer.OpenSerialize();
			astarSerializer.SerializeGraphs(this.graphs);
			astarSerializer.SerializeExtraInfo();
			byte[] result = astarSerializer.CloseSerialize();
			checksum = astarSerializer.GetChecksum();
			graphUpdateLock.Release();
			return result;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00009354 File Offset: 0x00007554
		public void DeserializeGraphs()
		{
			if (this.data != null)
			{
				this.DeserializeGraphs(this.data);
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000936C File Offset: 0x0000756C
		private void ClearGraphs()
		{
			if (this.graphs == null)
			{
				return;
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					((IGraphInternals)this.graphs[i]).OnDestroy();
					this.graphs[i].active = null;
				}
			}
			this.graphs = new NavGraph[0];
			this.UpdateShortcuts();
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000093CC File Offset: 0x000075CC
		public void OnDestroy()
		{
			this.ClearGraphs();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000093D4 File Offset: 0x000075D4
		public void DeserializeGraphs(byte[] bytes)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.AssertSafe(false);
			this.ClearGraphs();
			this.DeserializeGraphsAdditive(bytes);
			graphUpdateLock.Release();
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00009400 File Offset: 0x00007600
		public void DeserializeGraphsAdditive(byte[] bytes)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.AssertSafe(false);
			try
			{
				if (bytes == null)
				{
					throw new ArgumentNullException("bytes");
				}
				AstarSerializer astarSerializer = new AstarSerializer(this, AstarData.active.gameObject);
				if (astarSerializer.OpenDeserialize(bytes))
				{
					this.DeserializeGraphsPartAdditive(astarSerializer);
					astarSerializer.CloseDeserialize();
				}
				else
				{
					Debug.Log("Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
				}
				AstarData.active.VerifyIntegrity();
			}
			catch (Exception ex)
			{
				string str = "Caught exception while deserializing data.\n";
				Exception ex2 = ex;
				Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
				this.graphs = new NavGraph[0];
			}
			this.UpdateShortcuts();
			graphUpdateLock.Release();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x000094AC File Offset: 0x000076AC
		private void DeserializeGraphsPartAdditive(AstarSerializer sr)
		{
			if (this.graphs == null)
			{
				this.graphs = new NavGraph[0];
			}
			List<NavGraph> list = new List<NavGraph>(this.graphs);
			sr.SetGraphIndexOffset(list.Count);
			if (this.graphTypes == null)
			{
				this.FindGraphTypes();
			}
			list.AddRange(sr.DeserializeGraphs(this.graphTypes));
			this.graphs = list.ToArray();
			sr.DeserializeEditorSettingsCompatibility();
			sr.DeserializeExtraInfo();
			int i;
			int l;
			for (i = 0; i < this.graphs.Length; i = l + 1)
			{
				if (this.graphs[i] != null)
				{
					this.graphs[i].GetNodes(delegate(GraphNode node)
					{
						node.GraphIndex = (uint)i;
					});
				}
				l = i;
			}
			for (int j = 0; j < this.graphs.Length; j++)
			{
				for (int k = j + 1; k < this.graphs.Length; k++)
				{
					if (this.graphs[j] != null && this.graphs[k] != null && this.graphs[j].guid == this.graphs[k].guid)
					{
						Debug.LogWarning("Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
						this.graphs[j].guid = Guid.NewGuid();
						break;
					}
				}
			}
			sr.PostDeserialization();
			AstarData.active.hierarchicalGraph.RecalculateIfNecessary();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009610 File Offset: 0x00007810
		public void FindGraphTypes()
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			int i = 0;
			while (i < assemblies.Length)
			{
				Assembly assembly = assemblies[i];
				Type[] array = null;
				try
				{
					array = assembly.GetTypes();
				}
				catch
				{
					goto IL_82;
				}
				goto IL_29;
				IL_82:
				i++;
				continue;
				IL_29:
				foreach (Type type in array)
				{
					Type baseType = type.BaseType;
					while (baseType != null)
					{
						if (object.Equals(baseType, typeof(NavGraph)))
						{
							list.Add(type);
							break;
						}
						baseType = baseType.BaseType;
					}
				}
				goto IL_82;
			}
			this.graphTypes = list.ToArray();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x000096C8 File Offset: 0x000078C8
		[Obsolete("If really necessary. Use System.Type.GetType instead.")]
		public Type GetGraphType(string type)
		{
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (this.graphTypes[i].Name == type)
				{
					return this.graphTypes[i];
				}
			}
			return null;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009708 File Offset: 0x00007908
		[Obsolete("Use CreateGraph(System.Type) instead")]
		public NavGraph CreateGraph(string type)
		{
			Debug.Log("Creating Graph of type '" + type + "'");
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (this.graphTypes[i].Name == type)
				{
					return this.CreateGraph(this.graphTypes[i]);
				}
			}
			Debug.LogError("Graph type (" + type + ") wasn't found");
			return null;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009777 File Offset: 0x00007977
		internal NavGraph CreateGraph(Type type)
		{
			NavGraph navGraph = Activator.CreateInstance(type) as NavGraph;
			navGraph.active = AstarData.active;
			return navGraph;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009790 File Offset: 0x00007990
		[Obsolete("Use AddGraph(System.Type) instead")]
		public NavGraph AddGraph(string type)
		{
			NavGraph navGraph = null;
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (this.graphTypes[i].Name == type)
				{
					navGraph = this.CreateGraph(this.graphTypes[i]);
				}
			}
			if (navGraph == null)
			{
				Debug.LogError("No NavGraph of type '" + type + "' could be found");
				return null;
			}
			this.AddGraph(navGraph);
			return navGraph;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000097F8 File Offset: 0x000079F8
		public NavGraph AddGraph(Type type)
		{
			NavGraph navGraph = null;
			for (int i = 0; i < this.graphTypes.Length; i++)
			{
				if (object.Equals(this.graphTypes[i], type))
				{
					navGraph = this.CreateGraph(this.graphTypes[i]);
				}
			}
			if (navGraph == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"No NavGraph of type '",
					(type != null) ? type.ToString() : null,
					"' could be found, ",
					this.graphTypes.Length.ToString(),
					" graph types are avaliable"
				}));
				return null;
			}
			this.AddGraph(navGraph);
			return navGraph;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00009894 File Offset: 0x00007A94
		private void AddGraph(NavGraph graph)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.AssertSafe(true);
			bool flag = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] == null)
				{
					this.graphs[i] = graph;
					graph.graphIndex = (uint)i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (this.graphs != null && (long)this.graphs.Length >= 255L)
				{
					throw new Exception("Graph Count Limit Reached. You cannot have more than " + 255U.ToString() + " graphs.");
				}
				this.graphs = new List<NavGraph>(this.graphs ?? new NavGraph[0])
				{
					graph
				}.ToArray();
				graph.graphIndex = (uint)(this.graphs.Length - 1);
			}
			this.UpdateShortcuts();
			graph.active = AstarData.active;
			graphUpdateLock.Release();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000996C File Offset: 0x00007B6C
		public bool RemoveGraph(NavGraph graph)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = this.AssertSafe(false);
			((IGraphInternals)graph).OnDestroy();
			graph.active = null;
			int num = Array.IndexOf<NavGraph>(this.graphs, graph);
			if (num != -1)
			{
				this.graphs[num] = null;
			}
			this.UpdateShortcuts();
			graphUpdateLock.Release();
			return num != -1;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x000099BC File Offset: 0x00007BBC
		public static NavGraph GetGraph(GraphNode node)
		{
			if (node == null)
			{
				return null;
			}
			AstarPath active = AstarPath.active;
			if (active == null)
			{
				return null;
			}
			AstarData data = active.data;
			if (data == null || data.graphs == null)
			{
				return null;
			}
			uint graphIndex = node.GraphIndex;
			if ((ulong)graphIndex >= (ulong)((long)data.graphs.Length))
			{
				return null;
			}
			return data.graphs[(int)graphIndex];
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009A14 File Offset: 0x00007C14
		public NavGraph FindGraph(Func<NavGraph, bool> predicate)
		{
			if (this.graphs != null)
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					if (this.graphs[i] != null && predicate(this.graphs[i]))
					{
						return this.graphs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00009A60 File Offset: 0x00007C60
		public NavGraph FindGraphOfType(Type type)
		{
			return this.FindGraph((NavGraph graph) => object.Equals(graph.GetType(), type));
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009A8C File Offset: 0x00007C8C
		public NavGraph FindGraphWhichInheritsFrom(Type type)
		{
			return this.FindGraph((NavGraph graph) => WindowsStoreCompatibility.GetTypeInfo(type).IsAssignableFrom(WindowsStoreCompatibility.GetTypeInfo(graph.GetType())));
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00009AB8 File Offset: 0x00007CB8
		public IEnumerable FindGraphsOfType(Type type)
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] != null && object.Equals(this.graphs[i].GetType(), type))
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009ACF File Offset: 0x00007CCF
		public IEnumerable GetUpdateableGraphs()
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] is IUpdatableGraph)
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009ADF File Offset: 0x00007CDF
		[Obsolete("Obsolete because it is not used by the package internally and the use cases are few. Iterate through the graphs array instead.")]
		public IEnumerable GetRaycastableGraphs()
		{
			if (this.graphs == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.graphs.Length; i = num + 1)
			{
				if (this.graphs[i] is IRaycastableGraph)
				{
					yield return this.graphs[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009AF0 File Offset: 0x00007CF0
		public int GetGraphIndex(NavGraph graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			int num = -1;
			if (this.graphs != null)
			{
				num = Array.IndexOf<NavGraph>(this.graphs, graph);
				if (num == -1)
				{
					Debug.LogError("Graph doesn't exist");
				}
			}
			return num;
		}

		// Token: 0x04000160 RID: 352
		[NonSerialized]
		public NavGraph[] graphs = new NavGraph[0];

		// Token: 0x04000161 RID: 353
		[SerializeField]
		private string dataString;

		// Token: 0x04000162 RID: 354
		[SerializeField]
		[FormerlySerializedAs("data")]
		private byte[] upgradeData;

		// Token: 0x04000163 RID: 355
		public TextAsset file_cachedStartup;

		// Token: 0x04000164 RID: 356
		public byte[] data_cachedStartup;

		// Token: 0x04000165 RID: 357
		[SerializeField]
		public bool cacheStartup;

		// Token: 0x04000166 RID: 358
		private List<bool> graphStructureLocked = new List<bool>();
	}
}
