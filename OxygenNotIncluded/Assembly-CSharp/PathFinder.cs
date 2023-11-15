using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x020003F6 RID: 1014
public class PathFinder
{
	// Token: 0x06001569 RID: 5481 RVA: 0x000712B8 File Offset: 0x0006F4B8
	public static void Initialize()
	{
		NavType[] array = new NavType[11];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NavType)i;
		}
		PathFinder.PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, array);
		for (int j = 0; j < Grid.CellCount; j++)
		{
			if (Grid.Visible[j] > 0 || Grid.Spawnable[j] > 0)
			{
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				GameUtil.FloodFillConditional(j, PathFinder.allowPathfindingFloodFillCb, pooledList, null);
				Grid.AllowPathfinding[j] = true;
				pooledList.Recycle();
			}
		}
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(PathFinder.OnReveal));
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x0007135F File Offset: 0x0006F55F
	private static void OnReveal(int cell)
	{
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x00071361 File Offset: 0x0006F561
	public static void UpdatePath(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query, ref path);
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x00071370 File Offset: 0x0006F570
	public static bool ValidatePath(NavGrid nav_grid, PathFinderAbilities abilities, ref PathFinder.Path path)
	{
		if (!path.IsValid())
		{
			return false;
		}
		for (int i = 0; i < path.nodes.Count; i++)
		{
			PathFinder.Path.Node node = path.nodes[i];
			if (i < path.nodes.Count - 1)
			{
				PathFinder.Path.Node node2 = path.nodes[i + 1];
				int num = node.cell * nav_grid.maxLinksPerCell;
				bool flag = false;
				NavGrid.Link link = nav_grid.Links[num];
				while (link.link != PathFinder.InvalidHandle)
				{
					if (link.link == node2.cell && node2.navType == link.endNavType && node.navType == link.startNavType)
					{
						PathFinder.PotentialPath potentialPath = new PathFinder.PotentialPath(node.cell, node.navType, PathFinder.PotentialPath.Flags.None);
						flag = abilities.TraversePath(ref potentialPath, node.cell, node.navType, 0, (int)link.transitionId, false);
						if (flag)
						{
							break;
						}
					}
					num++;
					link = nav_grid.Links[num];
				}
				if (!flag)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x00071484 File Offset: 0x0006F684
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query)
	{
		int invalidCell = PathFinder.InvalidCell;
		NavType nav_type = NavType.NumNavTypes;
		query.ClearResult();
		if (!Grid.IsValidCell(potential_path.cell))
		{
			return;
		}
		PathFinder.FindPaths(nav_grid, ref abilities, potential_path, query, PathFinder.Temp.Potentials, ref invalidCell, ref nav_type);
		if (invalidCell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(invalidCell, nav_type, out flag);
			query.SetResult(invalidCell, cell.cost, nav_type);
		}
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x000714E8 File Offset: 0x0006F6E8
	public static void Run(NavGrid nav_grid, PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, ref PathFinder.Path path)
	{
		PathFinder.Run(nav_grid, abilities, potential_path, query);
		if (query.GetResultCell() != PathFinder.InvalidCell)
		{
			PathFinder.BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
			return;
		}
		path.Clear();
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0007151C File Offset: 0x0006F71C
	private static void BuildResultPath(int path_cell, NavType path_nav_type, ref PathFinder.Path path)
	{
		if (path_cell != PathFinder.InvalidCell)
		{
			bool flag = false;
			PathFinder.Cell cell = PathFinder.PathGrid.GetCell(path_cell, path_nav_type, out flag);
			path.Clear();
			path.cost = cell.cost;
			while (path_cell != PathFinder.InvalidCell)
			{
				path.AddNode(new PathFinder.Path.Node
				{
					cell = path_cell,
					navType = cell.navType,
					transitionId = cell.transitionId
				});
				path_cell = cell.parent;
				if (path_cell != PathFinder.InvalidCell)
				{
					cell = PathFinder.PathGrid.GetCell(path_cell, cell.parentNavType, out flag);
				}
			}
			if (path.nodes != null)
			{
				for (int i = 0; i < path.nodes.Count / 2; i++)
				{
					PathFinder.Path.Node value = path.nodes[i];
					path.nodes[i] = path.nodes[path.nodes.Count - i - 1];
					path.nodes[path.nodes.Count - i - 1] = value;
				}
			}
		}
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x00071628 File Offset: 0x0006F828
	private static void FindPaths(NavGrid nav_grid, ref PathFinderAbilities abilities, PathFinder.PotentialPath potential_path, PathFinderQuery query, PathFinder.PotentialList potentials, ref int result_cell, ref NavType result_nav_type)
	{
		potentials.Clear();
		PathFinder.PathGrid.ResetUpdate();
		PathFinder.PathGrid.BeginUpdate(potential_path.cell, false);
		bool flag;
		PathFinder.Cell cell = PathFinder.PathGrid.GetCell(potential_path, out flag);
		PathFinder.AddPotential(potential_path, Grid.InvalidCell, NavType.NumNavTypes, 0, 0, potentials, PathFinder.PathGrid, ref cell);
		int num = int.MaxValue;
		while (potentials.Count > 0)
		{
			KeyValuePair<int, PathFinder.PotentialPath> keyValuePair = potentials.Next();
			cell = PathFinder.PathGrid.GetCell(keyValuePair.Value, out flag);
			if (cell.cost == keyValuePair.Key)
			{
				if (cell.navType != NavType.Tube && query.IsMatch(keyValuePair.Value.cell, cell.parent, cell.cost) && cell.cost < num)
				{
					result_cell = keyValuePair.Value.cell;
					num = cell.cost;
					result_nav_type = cell.navType;
					break;
				}
				PathFinder.AddPotentials(nav_grid.potentialScratchPad, keyValuePair.Value, cell.cost, ref abilities, query, nav_grid.maxLinksPerCell, nav_grid.Links, potentials, PathFinder.PathGrid, cell.parent, cell.parentNavType);
			}
		}
		PathFinder.PathGrid.EndUpdate(true);
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x00071762 File Offset: 0x0006F962
	public static void AddPotential(PathFinder.PotentialPath potential_path, int parent_cell, NavType parent_nav_type, int cost, byte transition_id, PathFinder.PotentialList potentials, PathGrid path_grid, ref PathFinder.Cell cell_data)
	{
		cell_data.cost = cost;
		cell_data.parent = parent_cell;
		cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
		cell_data.transitionId = transition_id;
		potentials.Add(cost, potential_path);
		path_grid.SetCell(potential_path, ref cell_data);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x0007179E File Offset: 0x0006F99E
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void BeginDetailSample(string region_name)
	{
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000717A0 File Offset: 0x0006F9A0
	[Conditional("ENABLE_PATH_DETAILS")]
	private static void EndDetailSample(string region_name)
	{
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x000717A4 File Offset: 0x0006F9A4
	public static bool IsSubmerged(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num = Grid.CellAbove(cell);
		return (Grid.IsValidCell(num) && Grid.Element[num].IsLiquid) || (Grid.Element[cell].IsLiquid && Grid.IsValidCell(num) && Grid.Element[num].IsSolid);
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x00071804 File Offset: 0x0006FA04
	public static void AddPotentials(PathFinder.PotentialScratchPad potential_scratch_pad, PathFinder.PotentialPath potential, int cost, ref PathFinderAbilities abilities, PathFinderQuery query, int max_links_per_cell, NavGrid.Link[] links, PathFinder.PotentialList potentials, PathGrid path_grid, int parent_cell, NavType parent_nav_type)
	{
		if (!Grid.IsValidCell(potential.cell))
		{
			return;
		}
		int num = 0;
		NavGrid.Link[] linksWithCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
		int num2 = potential.cell * max_links_per_cell;
		NavGrid.Link link = links[num2];
		for (int link2 = link.link; link2 != PathFinder.InvalidHandle; link2 = link.link)
		{
			if (link.startNavType == potential.navType && (parent_cell != link2 || parent_nav_type != link.startNavType))
			{
				linksWithCorrectNavType[num++] = link;
			}
			num2++;
			link = links[num2];
		}
		int num3 = 0;
		PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange = potential_scratch_pad.linksInCellRange;
		for (int i = 0; i < num; i++)
		{
			NavGrid.Link link3 = linksWithCorrectNavType[i];
			int link4 = link3.link;
			bool flag = false;
			PathFinder.Cell cell = path_grid.GetCell(link4, link3.endNavType, out flag);
			if (flag)
			{
				int num4 = cost + (int)link3.cost;
				bool flag2 = cell.cost == -1;
				bool flag3 = num4 < cell.cost;
				if (flag2 || flag3)
				{
					linksInCellRange[num3++] = new PathFinder.PotentialScratchPad.PathGridCellData
					{
						pathGridCell = cell,
						link = link3
					};
				}
			}
		}
		for (int j = 0; j < num3; j++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData = linksInCellRange[j];
			int link5 = pathGridCellData.link.link;
			pathGridCellData.isSubmerged = PathFinder.IsSubmerged(link5);
			linksInCellRange[j] = pathGridCellData;
		}
		for (int k = 0; k < num3; k++)
		{
			PathFinder.PotentialScratchPad.PathGridCellData pathGridCellData2 = linksInCellRange[k];
			NavGrid.Link link6 = pathGridCellData2.link;
			int link7 = link6.link;
			PathFinder.Cell pathGridCell = pathGridCellData2.pathGridCell;
			int num5 = cost + (int)link6.cost;
			PathFinder.PotentialPath potentialPath = potential;
			potentialPath.cell = link7;
			potentialPath.navType = link6.endNavType;
			if (pathGridCellData2.isSubmerged)
			{
				int submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potentialPath, link6);
				num5 += submergedPathCostPenalty;
			}
			PathFinder.PotentialPath.Flags flags = potentialPath.flags;
			bool flag4 = abilities.TraversePath(ref potentialPath, potential.cell, potential.navType, num5, (int)link6.transitionId, pathGridCellData2.isSubmerged);
			PathFinder.PotentialPath.Flags flags2 = potentialPath.flags;
			if (flag4)
			{
				PathFinder.AddPotential(potentialPath, potential.cell, potential.navType, num5, link6.transitionId, potentials, path_grid, ref pathGridCell);
			}
		}
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x00071A4F File Offset: 0x0006FC4F
	public static void DestroyStatics()
	{
		PathFinder.PathGrid.OnCleanUp();
		PathFinder.PathGrid = null;
		PathFinder.Temp.Potentials.Clear();
	}

	// Token: 0x04000BB7 RID: 2999
	public static int InvalidHandle = -1;

	// Token: 0x04000BB8 RID: 3000
	public static int InvalidIdx = -1;

	// Token: 0x04000BB9 RID: 3001
	public static int InvalidCell = -1;

	// Token: 0x04000BBA RID: 3002
	public static PathGrid PathGrid;

	// Token: 0x04000BBB RID: 3003
	private static readonly Func<int, bool> allowPathfindingFloodFillCb = delegate(int cell)
	{
		if (Grid.Solid[cell])
		{
			return false;
		}
		if (Grid.AllowPathfinding[cell])
		{
			return false;
		}
		Grid.AllowPathfinding[cell] = true;
		return true;
	};

	// Token: 0x02001083 RID: 4227
	public struct Cell
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060075DC RID: 30172 RVA: 0x002CE1DC File Offset: 0x002CC3DC
		public NavType navType
		{
			get
			{
				return (NavType)(this.navTypes & 15);
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060075DD RID: 30173 RVA: 0x002CE1E8 File Offset: 0x002CC3E8
		public NavType parentNavType
		{
			get
			{
				return (NavType)(this.navTypes >> 4);
			}
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x002CE1F4 File Offset: 0x002CC3F4
		public void SetNavTypes(NavType type, NavType parent_type)
		{
			this.navTypes = (byte)(type | parent_type << 4);
		}

		// Token: 0x0400595F RID: 22879
		public int cost;

		// Token: 0x04005960 RID: 22880
		public int parent;

		// Token: 0x04005961 RID: 22881
		public short queryId;

		// Token: 0x04005962 RID: 22882
		private byte navTypes;

		// Token: 0x04005963 RID: 22883
		public byte transitionId;
	}

	// Token: 0x02001084 RID: 4228
	public struct PotentialPath
	{
		// Token: 0x060075DF RID: 30175 RVA: 0x002CE211 File Offset: 0x002CC411
		public PotentialPath(int cell, NavType nav_type, PathFinder.PotentialPath.Flags flags)
		{
			this.cell = cell;
			this.navType = nav_type;
			this.flags = flags;
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x002CE228 File Offset: 0x002CC428
		public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags |= new_flags;
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x002CE238 File Offset: 0x002CC438
		public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
		{
			this.flags &= ~new_flags;
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x002CE24A File Offset: 0x002CC44A
		public bool HasFlag(PathFinder.PotentialPath.Flags flag)
		{
			return this.HasAnyFlag(flag);
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x002CE253 File Offset: 0x002CC453
		public bool HasAnyFlag(PathFinder.PotentialPath.Flags mask)
		{
			return (this.flags & mask) > PathFinder.PotentialPath.Flags.None;
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060075E4 RID: 30180 RVA: 0x002CE260 File Offset: 0x002CC460
		// (set) Token: 0x060075E5 RID: 30181 RVA: 0x002CE268 File Offset: 0x002CC468
		public PathFinder.PotentialPath.Flags flags { readonly get; private set; }

		// Token: 0x04005964 RID: 22884
		public int cell;

		// Token: 0x04005965 RID: 22885
		public NavType navType;

		// Token: 0x02002004 RID: 8196
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x04009029 RID: 36905
			None = 0,
			// Token: 0x0400902A RID: 36906
			HasAtmoSuit = 1,
			// Token: 0x0400902B RID: 36907
			HasJetPack = 2,
			// Token: 0x0400902C RID: 36908
			HasOxygenMask = 4,
			// Token: 0x0400902D RID: 36909
			PerformSuitChecks = 8,
			// Token: 0x0400902E RID: 36910
			HasLeadSuit = 16
		}
	}

	// Token: 0x02001085 RID: 4229
	public struct Path
	{
		// Token: 0x060075E6 RID: 30182 RVA: 0x002CE271 File Offset: 0x002CC471
		public void AddNode(PathFinder.Path.Node node)
		{
			if (this.nodes == null)
			{
				this.nodes = new List<PathFinder.Path.Node>();
			}
			this.nodes.Add(node);
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x002CE292 File Offset: 0x002CC492
		public bool IsValid()
		{
			return this.nodes != null && this.nodes.Count > 1;
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x002CE2AC File Offset: 0x002CC4AC
		public bool HasArrived()
		{
			return this.nodes != null && this.nodes.Count > 0;
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x002CE2C6 File Offset: 0x002CC4C6
		public void Clear()
		{
			this.cost = 0;
			if (this.nodes != null)
			{
				this.nodes.Clear();
			}
		}

		// Token: 0x04005967 RID: 22887
		public int cost;

		// Token: 0x04005968 RID: 22888
		public List<PathFinder.Path.Node> nodes;

		// Token: 0x02002005 RID: 8197
		public struct Node
		{
			// Token: 0x0400902F RID: 36911
			public int cell;

			// Token: 0x04009030 RID: 36912
			public NavType navType;

			// Token: 0x04009031 RID: 36913
			public byte transitionId;
		}
	}

	// Token: 0x02001086 RID: 4230
	public class PotentialList
	{
		// Token: 0x060075EA RID: 30186 RVA: 0x002CE2E2 File Offset: 0x002CC4E2
		public KeyValuePair<int, PathFinder.PotentialPath> Next()
		{
			return this.queue.Dequeue();
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060075EB RID: 30187 RVA: 0x002CE2EF File Offset: 0x002CC4EF
		public int Count
		{
			get
			{
				return this.queue.Count;
			}
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x002CE2FC File Offset: 0x002CC4FC
		public void Add(int cost, PathFinder.PotentialPath path)
		{
			this.queue.Enqueue(cost, path);
		}

		// Token: 0x060075ED RID: 30189 RVA: 0x002CE30B File Offset: 0x002CC50B
		public void Clear()
		{
			this.queue.Clear();
		}

		// Token: 0x04005969 RID: 22889
		private PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath> queue = new PathFinder.PotentialList.HOTQueue<PathFinder.PotentialPath>();

		// Token: 0x02002006 RID: 8198
		public class PriorityQueue<TValue>
		{
			// Token: 0x0600A46E RID: 42094 RVA: 0x00369CA9 File Offset: 0x00367EA9
			public PriorityQueue()
			{
				this._baseHeap = new List<KeyValuePair<int, TValue>>();
			}

			// Token: 0x0600A46F RID: 42095 RVA: 0x00369CBC File Offset: 0x00367EBC
			public void Enqueue(int priority, TValue value)
			{
				this.Insert(priority, value);
			}

			// Token: 0x0600A470 RID: 42096 RVA: 0x00369CC6 File Offset: 0x00367EC6
			public KeyValuePair<int, TValue> Dequeue()
			{
				KeyValuePair<int, TValue> result = this._baseHeap[0];
				this.DeleteRoot();
				return result;
			}

			// Token: 0x0600A471 RID: 42097 RVA: 0x00369CDA File Offset: 0x00367EDA
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.Count > 0)
				{
					return this._baseHeap[0];
				}
				throw new InvalidOperationException("Priority queue is empty");
			}

			// Token: 0x0600A472 RID: 42098 RVA: 0x00369CFC File Offset: 0x00367EFC
			private void ExchangeElements(int pos1, int pos2)
			{
				KeyValuePair<int, TValue> value = this._baseHeap[pos1];
				this._baseHeap[pos1] = this._baseHeap[pos2];
				this._baseHeap[pos2] = value;
			}

			// Token: 0x0600A473 RID: 42099 RVA: 0x00369D3C File Offset: 0x00367F3C
			private void Insert(int priority, TValue value)
			{
				KeyValuePair<int, TValue> item = new KeyValuePair<int, TValue>(priority, value);
				this._baseHeap.Add(item);
				this.HeapifyFromEndToBeginning(this._baseHeap.Count - 1);
			}

			// Token: 0x0600A474 RID: 42100 RVA: 0x00369D74 File Offset: 0x00367F74
			private int HeapifyFromEndToBeginning(int pos)
			{
				if (pos >= this._baseHeap.Count)
				{
					return -1;
				}
				while (pos > 0)
				{
					int num = (pos - 1) / 2;
					if (this._baseHeap[num].Key - this._baseHeap[pos].Key <= 0)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
				return pos;
			}

			// Token: 0x0600A475 RID: 42101 RVA: 0x00369DD4 File Offset: 0x00367FD4
			private void DeleteRoot()
			{
				if (this._baseHeap.Count <= 1)
				{
					this._baseHeap.Clear();
					return;
				}
				this._baseHeap[0] = this._baseHeap[this._baseHeap.Count - 1];
				this._baseHeap.RemoveAt(this._baseHeap.Count - 1);
				this.HeapifyFromBeginningToEnd(0);
			}

			// Token: 0x0600A476 RID: 42102 RVA: 0x00369E40 File Offset: 0x00368040
			private void HeapifyFromBeginningToEnd(int pos)
			{
				int count = this._baseHeap.Count;
				if (pos >= count)
				{
					return;
				}
				for (;;)
				{
					int num = pos;
					int num2 = 2 * pos + 1;
					int num3 = 2 * pos + 2;
					if (num2 < count && this._baseHeap[num].Key - this._baseHeap[num2].Key > 0)
					{
						num = num2;
					}
					if (num3 < count && this._baseHeap[num].Key - this._baseHeap[num3].Key > 0)
					{
						num = num3;
					}
					if (num == pos)
					{
						break;
					}
					this.ExchangeElements(num, pos);
					pos = num;
				}
			}

			// Token: 0x0600A477 RID: 42103 RVA: 0x00369EE8 File Offset: 0x003680E8
			public void Clear()
			{
				this._baseHeap.Clear();
			}

			// Token: 0x17000A6E RID: 2670
			// (get) Token: 0x0600A478 RID: 42104 RVA: 0x00369EF5 File Offset: 0x003680F5
			public int Count
			{
				get
				{
					return this._baseHeap.Count;
				}
			}

			// Token: 0x04009032 RID: 36914
			private List<KeyValuePair<int, TValue>> _baseHeap;
		}

		// Token: 0x02002007 RID: 8199
		private class HOTQueue<TValue>
		{
			// Token: 0x0600A479 RID: 42105 RVA: 0x00369F04 File Offset: 0x00368104
			public KeyValuePair<int, TValue> Dequeue()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				this.count--;
				return this.hotQueue.Dequeue();
			}

			// Token: 0x0600A47A RID: 42106 RVA: 0x00369F60 File Offset: 0x00368160
			public void Enqueue(int priority, TValue value)
			{
				if (priority <= this.hotThreshold)
				{
					this.hotQueue.Enqueue(priority, value);
				}
				else
				{
					this.coldQueue.Enqueue(priority, value);
					this.coldThreshold = Math.Max(this.coldThreshold, priority);
				}
				this.count++;
			}

			// Token: 0x0600A47B RID: 42107 RVA: 0x00369FB4 File Offset: 0x003681B4
			public KeyValuePair<int, TValue> Peek()
			{
				if (this.hotQueue.Count == 0)
				{
					PathFinder.PotentialList.PriorityQueue<TValue> priorityQueue = this.hotQueue;
					this.hotQueue = this.coldQueue;
					this.coldQueue = priorityQueue;
					this.hotThreshold = this.coldThreshold;
				}
				return this.hotQueue.Peek();
			}

			// Token: 0x0600A47C RID: 42108 RVA: 0x00369FFF File Offset: 0x003681FF
			public void Clear()
			{
				this.count = 0;
				this.hotThreshold = int.MinValue;
				this.hotQueue.Clear();
				this.coldThreshold = int.MinValue;
				this.coldQueue.Clear();
			}

			// Token: 0x17000A6F RID: 2671
			// (get) Token: 0x0600A47D RID: 42109 RVA: 0x0036A034 File Offset: 0x00368234
			public int Count
			{
				get
				{
					return this.count;
				}
			}

			// Token: 0x04009033 RID: 36915
			private PathFinder.PotentialList.PriorityQueue<TValue> hotQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x04009034 RID: 36916
			private PathFinder.PotentialList.PriorityQueue<TValue> coldQueue = new PathFinder.PotentialList.PriorityQueue<TValue>();

			// Token: 0x04009035 RID: 36917
			private int hotThreshold = int.MinValue;

			// Token: 0x04009036 RID: 36918
			private int coldThreshold = int.MinValue;

			// Token: 0x04009037 RID: 36919
			private int count;
		}
	}

	// Token: 0x02001087 RID: 4231
	private class Temp
	{
		// Token: 0x0400596A RID: 22890
		public static PathFinder.PotentialList Potentials = new PathFinder.PotentialList();
	}

	// Token: 0x02001088 RID: 4232
	public class PotentialScratchPad
	{
		// Token: 0x060075F1 RID: 30193 RVA: 0x002CE33F File Offset: 0x002CC53F
		public PotentialScratchPad(int max_links_per_cell)
		{
			this.linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
			this.linksInCellRange = new PathFinder.PotentialScratchPad.PathGridCellData[max_links_per_cell];
		}

		// Token: 0x0400596B RID: 22891
		public NavGrid.Link[] linksWithCorrectNavType;

		// Token: 0x0400596C RID: 22892
		public PathFinder.PotentialScratchPad.PathGridCellData[] linksInCellRange;

		// Token: 0x02002008 RID: 8200
		public struct PathGridCellData
		{
			// Token: 0x04009038 RID: 36920
			public PathFinder.Cell pathGridCell;

			// Token: 0x04009039 RID: 36921
			public NavGrid.Link link;

			// Token: 0x0400903A RID: 36922
			public bool isSubmerged;
		}
	}
}
