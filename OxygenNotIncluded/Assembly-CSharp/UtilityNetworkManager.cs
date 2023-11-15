using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A1F RID: 2591
public class UtilityNetworkManager<NetworkType, ItemType> : IUtilityNetworkMgr where NetworkType : UtilityNetwork, new() where ItemType : MonoBehaviour
{
	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x06004D81 RID: 19841 RVA: 0x001B233A File Offset: 0x001B053A
	public bool IsDirty
	{
		get
		{
			return this.dirty;
		}
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x001B2344 File Offset: 0x001B0544
	public UtilityNetworkManager(int game_width, int game_height, int tile_layer)
	{
		this.tileLayer = tile_layer;
		this.networks = new List<UtilityNetwork>();
		this.Initialize(game_width, game_height);
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x001B23D0 File Offset: 0x001B05D0
	public void Initialize(int game_width, int game_height)
	{
		this.networks.Clear();
		this.physicalGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.visualGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.stashedVisualGrid = new UtilityNetworkGridNode[game_width * game_height];
		this.physicalNodes = new HashSet<int>();
		this.visualNodes = new HashSet<int>();
		this.visitedCells = new HashSet<int>();
		this.visitedVirtualKeys = new HashSet<object>();
		this.queuedVirtualKeys = new HashSet<object>();
		for (int i = 0; i < this.visualGrid.Length; i++)
		{
			this.visualGrid[i] = new UtilityNetworkGridNode
			{
				networkIdx = -1,
				connections = (UtilityConnections)0
			};
			this.physicalGrid[i] = new UtilityNetworkGridNode
			{
				networkIdx = -1,
				connections = (UtilityConnections)0
			};
		}
	}

	// Token: 0x06004D84 RID: 19844 RVA: 0x001B24A8 File Offset: 0x001B06A8
	public void Update()
	{
		if (this.dirty)
		{
			this.dirty = false;
			for (int i = 0; i < this.networks.Count; i++)
			{
				this.networks[i].Reset(this.physicalGrid);
			}
			this.networks.Clear();
			this.virtualKeyToNetworkIdx.Clear();
			this.RebuildNetworks(this.tileLayer, false);
			this.RebuildNetworks(this.tileLayer, true);
			if (this.onNetworksRebuilt != null)
			{
				this.onNetworksRebuilt(this.networks, this.GetNodes(true));
			}
		}
	}

	// Token: 0x06004D85 RID: 19845 RVA: 0x001B2544 File Offset: 0x001B0744
	protected UtilityNetworkGridNode[] GetGrid(bool is_physical_building)
	{
		if (!is_physical_building)
		{
			return this.visualGrid;
		}
		return this.physicalGrid;
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001B2556 File Offset: 0x001B0756
	private HashSet<int> GetNodes(bool is_physical_building)
	{
		if (!is_physical_building)
		{
			return this.visualNodes;
		}
		return this.physicalNodes;
	}

	// Token: 0x06004D87 RID: 19847 RVA: 0x001B2568 File Offset: 0x001B0768
	public void ClearCell(int cell, bool is_physical_building)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
		HashSet<int> nodes = this.GetNodes(is_physical_building);
		UtilityConnections connections = grid[cell].connections;
		grid[cell].connections = (UtilityConnections)0;
		Vector2I vector2I = Grid.CellToXY(cell);
		if (vector2I.x > 1 && (connections & UtilityConnections.Left) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array = grid;
			int num = Grid.CellLeft(cell);
			array[num].connections = (array[num].connections & ~UtilityConnections.Right);
		}
		if (vector2I.x < Grid.WidthInCells - 1 && (connections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array2 = grid;
			int num2 = Grid.CellRight(cell);
			array2[num2].connections = (array2[num2].connections & ~UtilityConnections.Left);
		}
		if (vector2I.y > 1 && (connections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array3 = grid;
			int num3 = Grid.CellBelow(cell);
			array3[num3].connections = (array3[num3].connections & ~UtilityConnections.Up);
		}
		if (vector2I.y < Grid.HeightInCells - 1 && (connections & UtilityConnections.Up) != (UtilityConnections)0)
		{
			UtilityNetworkGridNode[] array4 = grid;
			int num4 = Grid.CellAbove(cell);
			array4[num4].connections = (array4[num4].connections & ~UtilityConnections.Down);
		}
		nodes.Remove(cell);
		if (is_physical_building)
		{
			this.dirty = true;
			this.ClearCell(cell, false);
		}
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001B2668 File Offset: 0x001B0868
	private void QueueCellForVisit(UtilityNetworkGridNode[] grid, int dest_cell, UtilityConnections direction)
	{
		if (!Grid.IsValidCell(dest_cell))
		{
			return;
		}
		if (this.visitedCells.Contains(dest_cell))
		{
			return;
		}
		if (direction != (UtilityConnections)0 && (grid[dest_cell].connections & direction.InverseDirection()) == (UtilityConnections)0)
		{
			return;
		}
		if (Grid.Objects[dest_cell, this.tileLayer] != null)
		{
			this.visitedCells.Add(dest_cell);
			this.queued.Enqueue(dest_cell);
		}
	}

	// Token: 0x06004D89 RID: 19849 RVA: 0x001B26D8 File Offset: 0x001B08D8
	public void ForceRebuildNetworks()
	{
		this.dirty = true;
	}

	// Token: 0x06004D8A RID: 19850 RVA: 0x001B26E4 File Offset: 0x001B08E4
	public void AddToNetworks(int cell, object item, bool is_endpoint)
	{
		if (item != null)
		{
			if (is_endpoint)
			{
				if (this.endpoints.ContainsKey(cell))
				{
					global::Debug.LogWarning(string.Format("Cell {0} already has a utility network endpoint assigned. Adding {1} will stomp previous endpoint, destroying the object that's already there.", cell, item.ToString()));
					KMonoBehaviour kmonoBehaviour = this.endpoints[cell] as KMonoBehaviour;
					if (kmonoBehaviour != null)
					{
						Util.KDestroyGameObject(kmonoBehaviour);
					}
				}
				this.endpoints[cell] = item;
			}
			else
			{
				if (this.items.ContainsKey(cell))
				{
					global::Debug.LogWarning(string.Format("Cell {0} already has a utility network connector assigned. Adding {1} will stomp previous item, destroying the object that's already there.", cell, item.ToString()));
					KMonoBehaviour kmonoBehaviour2 = this.items[cell] as KMonoBehaviour;
					if (kmonoBehaviour2 != null)
					{
						Util.KDestroyGameObject(kmonoBehaviour2);
					}
				}
				this.items[cell] = item;
			}
		}
		this.dirty = true;
	}

	// Token: 0x06004D8B RID: 19851 RVA: 0x001B27B4 File Offset: 0x001B09B4
	public void AddToVirtualNetworks(object key, object item, bool is_endpoint)
	{
		if (item != null)
		{
			if (is_endpoint)
			{
				if (!this.virtualEndpoints.ContainsKey(key))
				{
					this.virtualEndpoints[key] = new List<object>();
				}
				this.virtualEndpoints[key].Add(item);
			}
			else
			{
				if (!this.virtualItems.ContainsKey(key))
				{
					this.virtualItems[key] = new List<object>();
				}
				this.virtualItems[key].Add(item);
			}
		}
		this.dirty = true;
	}

	// Token: 0x06004D8C RID: 19852 RVA: 0x001B2834 File Offset: 0x001B0A34
	private unsafe void Reconnect(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		int* ptr3 = stackalloc int[(UIntPtr)16];
		int num = 0;
		if (vector2I.y < Grid.HeightInCells - 1)
		{
			ptr[num] = Grid.CellAbove(cell);
			ptr2[num] = 4;
			ptr3[num] = 8;
			num++;
		}
		if (vector2I.y > 1)
		{
			ptr[num] = Grid.CellBelow(cell);
			ptr2[num] = 8;
			ptr3[num] = 4;
			num++;
		}
		if (vector2I.x > 1)
		{
			ptr[num] = Grid.CellLeft(cell);
			ptr2[num] = 1;
			ptr3[num] = 2;
			num++;
		}
		if (vector2I.x < Grid.WidthInCells - 1)
		{
			ptr[num] = Grid.CellRight(cell);
			ptr2[num] = 2;
			ptr3[num] = 1;
			num++;
		}
		UtilityConnections connections = this.physicalGrid[cell].connections;
		UtilityConnections connections2 = this.visualGrid[cell].connections;
		for (int i = 0; i < num; i++)
		{
			int num2 = ptr[i];
			UtilityConnections utilityConnections = (UtilityConnections)ptr2[i];
			UtilityConnections utilityConnections2 = (UtilityConnections)ptr3[i];
			if ((connections & utilityConnections) != (UtilityConnections)0)
			{
				if (this.physicalNodes.Contains(num2))
				{
					UtilityNetworkGridNode[] array = this.physicalGrid;
					int num3 = num2;
					array[num3].connections = (array[num3].connections | utilityConnections2);
				}
				if (this.visualNodes.Contains(num2))
				{
					UtilityNetworkGridNode[] array2 = this.visualGrid;
					int num4 = num2;
					array2[num4].connections = (array2[num4].connections | utilityConnections2);
				}
			}
			else if ((connections2 & utilityConnections) != (UtilityConnections)0 && (this.physicalNodes.Contains(num2) || this.visualNodes.Contains(num2)))
			{
				UtilityNetworkGridNode[] array3 = this.visualGrid;
				int num5 = num2;
				array3[num5].connections = (array3[num5].connections | utilityConnections2);
			}
		}
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x001B2A14 File Offset: 0x001B0C14
	public void RemoveFromVirtualNetworks(object key, object item, bool is_endpoint)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.dirty = true;
		if (item != null)
		{
			if (is_endpoint)
			{
				this.virtualEndpoints[key].Remove(item);
				if (this.virtualEndpoints[key].Count == 0)
				{
					this.virtualEndpoints.Remove(key);
				}
			}
			else
			{
				this.virtualItems[key].Remove(item);
				if (this.virtualItems[key].Count == 0)
				{
					this.virtualItems.Remove(key);
				}
			}
			UtilityNetwork networkForVirtualKey = this.GetNetworkForVirtualKey(key);
			if (networkForVirtualKey != null)
			{
				networkForVirtualKey.RemoveItem(item);
			}
		}
	}

	// Token: 0x06004D8E RID: 19854 RVA: 0x001B2AB0 File Offset: 0x001B0CB0
	public void RemoveFromNetworks(int cell, object item, bool is_endpoint)
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.dirty = true;
		if (item != null)
		{
			if (is_endpoint)
			{
				this.endpoints.Remove(cell);
				int networkIdx = this.physicalGrid[cell].networkIdx;
				if (networkIdx != -1)
				{
					this.networks[networkIdx].RemoveItem(item);
					return;
				}
			}
			else
			{
				int networkIdx2 = this.physicalGrid[cell].networkIdx;
				this.physicalGrid[cell].connections = (UtilityConnections)0;
				this.physicalGrid[cell].networkIdx = -1;
				this.items.Remove(cell);
				this.Disconnect(cell);
				object item2;
				if (this.endpoints.TryGetValue(cell, out item2) && networkIdx2 != -1)
				{
					this.networks[networkIdx2].DisconnectItem(item2);
				}
			}
		}
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x001B2B80 File Offset: 0x001B0D80
	private unsafe void Disconnect(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		int num = 0;
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		if (vector2I.y < Grid.HeightInCells - 1)
		{
			ptr[num] = Grid.CellAbove(cell);
			ptr2[num] = -9;
			num++;
		}
		if (vector2I.y > 1)
		{
			ptr[num] = Grid.CellBelow(cell);
			ptr2[num] = -5;
			num++;
		}
		if (vector2I.x > 1)
		{
			ptr[num] = Grid.CellLeft(cell);
			ptr2[num] = -3;
			num++;
		}
		if (vector2I.x < Grid.WidthInCells - 1)
		{
			ptr[num] = Grid.CellRight(cell);
			ptr2[num] = -2;
			num++;
		}
		for (int i = 0; i < num; i++)
		{
			int num2 = ptr[i];
			int num3 = ptr2[i];
			int connections = (int)(this.physicalGrid[num2].connections & (UtilityConnections)num3);
			this.physicalGrid[num2].connections = (UtilityConnections)connections;
		}
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x001B2C8C File Offset: 0x001B0E8C
	private unsafe void RebuildNetworks(int layer, bool is_physical)
	{
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical);
		HashSet<int> nodes = this.GetNodes(is_physical);
		this.visitedCells.Clear();
		this.visitedVirtualKeys.Clear();
		this.queuedVirtualKeys.Clear();
		this.queued.Clear();
		int* ptr = stackalloc int[(UIntPtr)16];
		int* ptr2 = stackalloc int[(UIntPtr)16];
		foreach (int num in nodes)
		{
			UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
			if (!this.visitedCells.Contains(num))
			{
				this.queued.Enqueue(num);
				this.visitedCells.Add(num);
				NetworkType networkType = Activator.CreateInstance<NetworkType>();
				networkType.id = this.networks.Count;
				this.networks.Add(networkType);
				while (this.queued.Count > 0)
				{
					int num2 = this.queued.Dequeue();
					utilityNetworkGridNode = grid[num2];
					object obj = null;
					object obj2 = null;
					if (is_physical)
					{
						if (this.items.TryGetValue(num2, out obj))
						{
							if (obj is IDisconnectable && (obj as IDisconnectable).IsDisconnected())
							{
								continue;
							}
							if (obj != null)
							{
								networkType.AddItem(obj);
							}
						}
						if (this.endpoints.TryGetValue(num2, out obj2) && obj2 != null)
						{
							networkType.AddItem(obj2);
						}
					}
					grid[num2].networkIdx = networkType.id;
					if (obj != null && obj2 != null)
					{
						networkType.ConnectItem(obj2);
					}
					Vector2I vector2I = Grid.CellToXY(num2);
					int num3 = 0;
					if (vector2I.x >= 0)
					{
						ptr[num3] = Grid.CellLeft(num2);
						ptr2[num3] = 1;
						num3++;
					}
					if (vector2I.x < Grid.WidthInCells)
					{
						ptr[num3] = Grid.CellRight(num2);
						ptr2[num3] = 2;
						num3++;
					}
					if (vector2I.y >= 0)
					{
						ptr[num3] = Grid.CellBelow(num2);
						ptr2[num3] = 8;
						num3++;
					}
					if (vector2I.y < Grid.HeightInCells)
					{
						ptr[num3] = Grid.CellAbove(num2);
						ptr2[num3] = 4;
						num3++;
					}
					for (int i = 0; i < num3; i++)
					{
						int num4 = ptr2[i];
						if ((utilityNetworkGridNode.connections & (UtilityConnections)num4) != (UtilityConnections)0)
						{
							int dest_cell = ptr[i];
							this.QueueCellForVisit(grid, dest_cell, (UtilityConnections)num4);
						}
					}
					int dest_cell2;
					if (this.links.TryGetValue(num2, out dest_cell2))
					{
						this.QueueCellForVisit(grid, dest_cell2, (UtilityConnections)0);
					}
					object obj3;
					if (this.semiVirtualLinks.TryGetValue(num2, out obj3) && !this.visitedVirtualKeys.Contains(obj3))
					{
						this.visitedVirtualKeys.Add(obj3);
						this.virtualKeyToNetworkIdx[obj3] = networkType.id;
						if (this.virtualItems.ContainsKey(obj3))
						{
							foreach (object item in this.virtualItems[obj3])
							{
								networkType.AddItem(item);
								networkType.ConnectItem(item);
							}
						}
						if (this.virtualEndpoints.ContainsKey(obj3))
						{
							foreach (object item2 in this.virtualEndpoints[obj3])
							{
								networkType.AddItem(item2);
								networkType.ConnectItem(item2);
							}
						}
						foreach (KeyValuePair<int, object> keyValuePair in this.semiVirtualLinks)
						{
							if (keyValuePair.Value == obj3)
							{
								this.QueueCellForVisit(grid, keyValuePair.Key, (UtilityConnections)0);
							}
						}
					}
				}
			}
		}
		foreach (KeyValuePair<object, List<object>> keyValuePair2 in this.virtualItems)
		{
			if (!this.visitedVirtualKeys.Contains(keyValuePair2.Key))
			{
				NetworkType networkType2 = Activator.CreateInstance<NetworkType>();
				networkType2.id = this.networks.Count;
				this.visitedVirtualKeys.Add(keyValuePair2.Key);
				this.virtualKeyToNetworkIdx[keyValuePair2.Key] = networkType2.id;
				foreach (object item3 in keyValuePair2.Value)
				{
					networkType2.AddItem(item3);
					networkType2.ConnectItem(item3);
				}
				foreach (object item4 in this.virtualEndpoints[keyValuePair2.Key])
				{
					networkType2.AddItem(item4);
					networkType2.ConnectItem(item4);
				}
				this.networks.Add(networkType2);
			}
		}
		foreach (KeyValuePair<object, List<object>> keyValuePair3 in this.virtualEndpoints)
		{
			if (!this.visitedVirtualKeys.Contains(keyValuePair3.Key))
			{
				NetworkType networkType3 = Activator.CreateInstance<NetworkType>();
				networkType3.id = this.networks.Count;
				this.visitedVirtualKeys.Add(keyValuePair3.Key);
				this.virtualKeyToNetworkIdx[keyValuePair3.Key] = networkType3.id;
				foreach (object item5 in this.virtualEndpoints[keyValuePair3.Key])
				{
					networkType3.AddItem(item5);
					networkType3.ConnectItem(item5);
				}
				this.networks.Add(networkType3);
			}
		}
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x001B33DC File Offset: 0x001B15DC
	public UtilityNetwork GetNetworkForVirtualKey(object key)
	{
		int index;
		if (this.virtualKeyToNetworkIdx.TryGetValue(key, out index))
		{
			return this.networks[index];
		}
		return null;
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x001B3408 File Offset: 0x001B1608
	public UtilityNetwork GetNetworkByID(int id)
	{
		UtilityNetwork result = null;
		if (0 <= id && id < this.networks.Count)
		{
			result = this.networks[id];
		}
		return result;
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x001B3438 File Offset: 0x001B1638
	public UtilityNetwork GetNetworkForCell(int cell)
	{
		UtilityNetwork result = null;
		if (Grid.IsValidCell(cell) && 0 <= this.physicalGrid[cell].networkIdx && this.physicalGrid[cell].networkIdx < this.networks.Count)
		{
			result = this.networks[this.physicalGrid[cell].networkIdx];
		}
		return result;
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x001B34A0 File Offset: 0x001B16A0
	public UtilityNetwork GetNetworkForDirection(int cell, Direction direction)
	{
		cell = Grid.GetCellInDirection(cell, direction);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		UtilityNetworkGridNode utilityNetworkGridNode = this.GetGrid(true)[cell];
		UtilityNetwork result = null;
		if (utilityNetworkGridNode.networkIdx != -1 && utilityNetworkGridNode.networkIdx < this.networks.Count)
		{
			result = this.networks[utilityNetworkGridNode.networkIdx];
		}
		return result;
	}

	// Token: 0x06004D95 RID: 19861 RVA: 0x001B3500 File Offset: 0x001B1700
	private UtilityConnections GetNeighboursAsConnections(int cell, HashSet<int> nodes)
	{
		UtilityConnections utilityConnections = (UtilityConnections)0;
		Vector2I vector2I = Grid.CellToXY(cell);
		if (vector2I.x > 1 && nodes.Contains(Grid.CellLeft(cell)))
		{
			utilityConnections |= UtilityConnections.Left;
		}
		if (vector2I.x < Grid.WidthInCells - 1 && nodes.Contains(Grid.CellRight(cell)))
		{
			utilityConnections |= UtilityConnections.Right;
		}
		if (vector2I.y > 1 && nodes.Contains(Grid.CellBelow(cell)))
		{
			utilityConnections |= UtilityConnections.Down;
		}
		if (vector2I.y < Grid.HeightInCells - 1 && nodes.Contains(Grid.CellAbove(cell)))
		{
			utilityConnections |= UtilityConnections.Up;
		}
		return utilityConnections;
	}

	// Token: 0x06004D96 RID: 19862 RVA: 0x001B3590 File Offset: 0x001B1790
	public virtual void SetConnections(UtilityConnections connections, int cell, bool is_physical_building)
	{
		HashSet<int> nodes = this.GetNodes(is_physical_building);
		nodes.Add(cell);
		this.visualGrid[cell].connections = connections;
		if (is_physical_building)
		{
			this.dirty = true;
			UtilityConnections connections2 = is_physical_building ? (connections & this.GetNeighboursAsConnections(cell, nodes)) : connections;
			this.physicalGrid[cell].connections = connections2;
		}
		this.Reconnect(cell);
	}

	// Token: 0x06004D97 RID: 19863 RVA: 0x001B35F4 File Offset: 0x001B17F4
	public UtilityConnections GetConnections(int cell, bool is_physical_building)
	{
		UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
		UtilityConnections utilityConnections = grid[cell].connections;
		if (!is_physical_building)
		{
			grid = this.GetGrid(true);
			utilityConnections |= grid[cell].connections;
		}
		return utilityConnections;
	}

	// Token: 0x06004D98 RID: 19864 RVA: 0x001B3634 File Offset: 0x001B1834
	public UtilityConnections GetDisplayConnections(int cell)
	{
		UtilityConnections utilityConnections = (UtilityConnections)0;
		UtilityNetworkGridNode[] grid = this.GetGrid(false);
		UtilityConnections utilityConnections2 = utilityConnections | grid[cell].connections;
		grid = this.GetGrid(true);
		return utilityConnections2 | grid[cell].connections;
	}

	// Token: 0x06004D99 RID: 19865 RVA: 0x001B366C File Offset: 0x001B186C
	public virtual bool CanAddConnection(UtilityConnections new_connection, int cell, bool is_physical_building, out string fail_reason)
	{
		fail_reason = null;
		return true;
	}

	// Token: 0x06004D9A RID: 19866 RVA: 0x001B3674 File Offset: 0x001B1874
	public void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building)
	{
		string text;
		if (this.CanAddConnection(new_connection, cell, is_physical_building, out text))
		{
			if (is_physical_building)
			{
				this.dirty = true;
			}
			UtilityNetworkGridNode[] grid = this.GetGrid(is_physical_building);
			UtilityConnections connections = grid[cell].connections;
			grid[cell].connections = (connections | new_connection);
		}
	}

	// Token: 0x06004D9B RID: 19867 RVA: 0x001B36BA File Offset: 0x001B18BA
	public void StashVisualGrids()
	{
		Array.Copy(this.visualGrid, this.stashedVisualGrid, this.visualGrid.Length);
	}

	// Token: 0x06004D9C RID: 19868 RVA: 0x001B36D5 File Offset: 0x001B18D5
	public void UnstashVisualGrids()
	{
		Array.Copy(this.stashedVisualGrid, this.visualGrid, this.visualGrid.Length);
	}

	// Token: 0x06004D9D RID: 19869 RVA: 0x001B36F0 File Offset: 0x001B18F0
	public string GetVisualizerString(int cell)
	{
		UtilityConnections displayConnections = this.GetDisplayConnections(cell);
		return this.GetVisualizerString(displayConnections);
	}

	// Token: 0x06004D9E RID: 19870 RVA: 0x001B370C File Offset: 0x001B190C
	public string GetVisualizerString(UtilityConnections connections)
	{
		string text = "";
		if ((connections & UtilityConnections.Left) != (UtilityConnections)0)
		{
			text += "L";
		}
		if ((connections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			text += "R";
		}
		if ((connections & UtilityConnections.Up) != (UtilityConnections)0)
		{
			text += "U";
		}
		if ((connections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			text += "D";
		}
		if (text == "")
		{
			text = "None";
		}
		return text;
	}

	// Token: 0x06004D9F RID: 19871 RVA: 0x001B3778 File Offset: 0x001B1978
	public object GetEndpoint(int cell)
	{
		object result = null;
		this.endpoints.TryGetValue(cell, out result);
		return result;
	}

	// Token: 0x06004DA0 RID: 19872 RVA: 0x001B3797 File Offset: 0x001B1997
	public void AddSemiVirtualLink(int cell1, object virtualKey)
	{
		global::Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
		this.semiVirtualLinks[cell1] = virtualKey;
		this.dirty = true;
	}

	// Token: 0x06004DA1 RID: 19873 RVA: 0x001B37BB File Offset: 0x001B19BB
	public void RemoveSemiVirtualLink(int cell1, object virtualKey)
	{
		global::Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
		this.semiVirtualLinks.Remove(cell1);
		this.dirty = true;
	}

	// Token: 0x06004DA2 RID: 19874 RVA: 0x001B37DF File Offset: 0x001B19DF
	public void AddLink(int cell1, int cell2)
	{
		this.links[cell1] = cell2;
		this.links[cell2] = cell1;
		this.dirty = true;
	}

	// Token: 0x06004DA3 RID: 19875 RVA: 0x001B3802 File Offset: 0x001B1A02
	public void RemoveLink(int cell1, int cell2)
	{
		this.links.Remove(cell1);
		this.links.Remove(cell2);
		this.dirty = true;
	}

	// Token: 0x06004DA4 RID: 19876 RVA: 0x001B3825 File Offset: 0x001B1A25
	public void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener)
	{
		this.onNetworksRebuilt = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Combine(this.onNetworksRebuilt, listener);
	}

	// Token: 0x06004DA5 RID: 19877 RVA: 0x001B383E File Offset: 0x001B1A3E
	public void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener)
	{
		this.onNetworksRebuilt = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Remove(this.onNetworksRebuilt, listener);
	}

	// Token: 0x06004DA6 RID: 19878 RVA: 0x001B3857 File Offset: 0x001B1A57
	public IList<UtilityNetwork> GetNetworks()
	{
		return this.networks;
	}

	// Token: 0x0400327F RID: 12927
	private Dictionary<int, object> items = new Dictionary<int, object>();

	// Token: 0x04003280 RID: 12928
	private Dictionary<int, object> endpoints = new Dictionary<int, object>();

	// Token: 0x04003281 RID: 12929
	private Dictionary<object, List<object>> virtualItems = new Dictionary<object, List<object>>();

	// Token: 0x04003282 RID: 12930
	private Dictionary<object, List<object>> virtualEndpoints = new Dictionary<object, List<object>>();

	// Token: 0x04003283 RID: 12931
	private Dictionary<int, int> links = new Dictionary<int, int>();

	// Token: 0x04003284 RID: 12932
	private Dictionary<int, object> semiVirtualLinks = new Dictionary<int, object>();

	// Token: 0x04003285 RID: 12933
	private List<UtilityNetwork> networks;

	// Token: 0x04003286 RID: 12934
	private Dictionary<object, int> virtualKeyToNetworkIdx = new Dictionary<object, int>();

	// Token: 0x04003287 RID: 12935
	private HashSet<int> visitedCells;

	// Token: 0x04003288 RID: 12936
	private HashSet<object> visitedVirtualKeys;

	// Token: 0x04003289 RID: 12937
	private HashSet<object> queuedVirtualKeys;

	// Token: 0x0400328A RID: 12938
	private Action<IList<UtilityNetwork>, ICollection<int>> onNetworksRebuilt;

	// Token: 0x0400328B RID: 12939
	private Queue<int> queued = new Queue<int>();

	// Token: 0x0400328C RID: 12940
	protected UtilityNetworkGridNode[] visualGrid;

	// Token: 0x0400328D RID: 12941
	private UtilityNetworkGridNode[] stashedVisualGrid;

	// Token: 0x0400328E RID: 12942
	protected UtilityNetworkGridNode[] physicalGrid;

	// Token: 0x0400328F RID: 12943
	protected HashSet<int> physicalNodes;

	// Token: 0x04003290 RID: 12944
	protected HashSet<int> visualNodes;

	// Token: 0x04003291 RID: 12945
	private bool dirty;

	// Token: 0x04003292 RID: 12946
	private int tileLayer = -1;
}
