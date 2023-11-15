using System;
using System.Collections.Generic;

// Token: 0x02000957 RID: 2391
public class ScenePartitioner : ISim1000ms
{
	// Token: 0x06004631 RID: 17969 RVA: 0x0018CD8C File Offset: 0x0018AF8C
	public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
	{
		this.nodeSize = node_size;
		int num = scene_width / node_size;
		int num2 = scene_height / node_size;
		this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, num2, num];
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					this.nodes[i, j, k].entries = new HashSet<ScenePartitionerEntry>();
				}
			}
		}
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x06004632 RID: 17970 RVA: 0x0018CE4C File Offset: 0x0018B04C
	public void FreeResources()
	{
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[i, j, k].entries)
					{
						if (scenePartitionerEntry != null)
						{
							scenePartitionerEntry.partitioner = null;
							scenePartitionerEntry.obj = null;
						}
					}
					this.nodes[i, j, k].entries.Clear();
				}
			}
		}
		this.nodes = null;
	}

	// Token: 0x06004633 RID: 17971 RVA: 0x0018CF2C File Offset: 0x0018B12C
	[Obsolete]
	public ScenePartitionerLayer CreateMask(HashedString name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x06004634 RID: 17972 RVA: 0x0018CFCC File Offset: 0x0018B1CC
	public ScenePartitionerLayer CreateMask(string name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		HashCache.Get().Add(name);
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x06004635 RID: 17973 RVA: 0x0018D084 File Offset: 0x0018B284
	private int ClampNodeX(int x)
	{
		return Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);
	}

	// Token: 0x06004636 RID: 17974 RVA: 0x0018D0A0 File Offset: 0x0018B2A0
	private int ClampNodeY(int y)
	{
		return Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);
	}

	// Token: 0x06004637 RID: 17975 RVA: 0x0018D0BC File Offset: 0x0018B2BC
	private Extents GetNodeExtents(int x, int y, int width, int height)
	{
		Extents extents = default(Extents);
		extents.x = this.ClampNodeX(x / this.nodeSize);
		extents.y = this.ClampNodeY(y / this.nodeSize);
		extents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - extents.x;
		extents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - extents.y;
		return extents;
	}

	// Token: 0x06004638 RID: 17976 RVA: 0x0018D13D File Offset: 0x0018B33D
	private Extents GetNodeExtents(ScenePartitionerEntry entry)
	{
		return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
	}

	// Token: 0x06004639 RID: 17977 RVA: 0x0018D160 File Offset: 0x0018B360
	private void Insert(ScenePartitionerEntry entry)
	{
		if (entry.obj == null)
		{
			Debug.LogWarning("Trying to put null go into scene partitioner");
			return;
		}
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
				this.nodes[layer, i, j].entries.Add(entry);
			}
		}
	}

	// Token: 0x0600463A RID: 17978 RVA: 0x0018D358 File Offset: 0x0018B558
	private void Widthdraw(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (this.nodes[layer, i, j].entries.Remove(entry) && !this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
	}

	// Token: 0x0600463B RID: 17979 RVA: 0x0018D520 File Offset: 0x0018B720
	public ScenePartitionerEntry Add(ScenePartitionerEntry entry)
	{
		this.Insert(entry);
		return entry;
	}

	// Token: 0x0600463C RID: 17980 RVA: 0x0018D52A File Offset: 0x0018B72A
	public void UpdatePosition(int x, int y, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = x;
		entry.y = y;
		this.Insert(entry);
	}

	// Token: 0x0600463D RID: 17981 RVA: 0x0018D548 File Offset: 0x0018B748
	public void UpdatePosition(Extents e, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = e.x;
		entry.y = e.y;
		entry.width = e.width;
		entry.height = e.height;
		this.Insert(entry);
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x0018D588 File Offset: 0x0018B788
	public void Remove(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
		entry.obj = null;
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x0018D738 File Offset: 0x0018B938
	public void Sim1000ms(float dt)
	{
		foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
		{
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries.RemoveWhere(ScenePartitioner.removeCallback);
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
		}
		this.dirtyNodes.Clear();
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x0018D7E0 File Offset: 0x0018B9E0
	public void TriggerEvent(List<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.queryId++;
		for (int i = 0; i < cells.Count; i++)
		{
			int x = 0;
			int y = 0;
			Grid.CellToXY(cells[i], out x, out y);
			this.GatherEntries(x, y, 1, 1, layer, event_data, pooledList, this.queryId);
		}
		this.RunLayerGlobalEvent(cells, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06004641 RID: 17985 RVA: 0x0018D850 File Offset: 0x0018BA50
	public void TriggerEvent(HashSet<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.queryId++;
		foreach (int cell in cells)
		{
			int x = 0;
			int y = 0;
			Grid.CellToXY(cell, out x, out y);
			this.GatherEntries(x, y, 1, 1, layer, event_data, pooledList, this.queryId);
		}
		this.RunLayerGlobalEvent(cells, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06004642 RID: 17986 RVA: 0x0018D8E4 File Offset: 0x0018BAE4
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.GatherEntries(x, y, width, height, layer, event_data, pooledList);
		this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x06004643 RID: 17987 RVA: 0x0018D928 File Offset: 0x0018BB28
	private void RunLayerGlobalEvent(List<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			for (int i = 0; i < cells.Count; i++)
			{
				layer.OnEvent(cells[i], event_data);
			}
		}
	}

	// Token: 0x06004644 RID: 17988 RVA: 0x0018D964 File Offset: 0x0018BB64
	private void RunLayerGlobalEvent(HashSet<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			foreach (int arg in cells)
			{
				layer.OnEvent(arg, event_data);
			}
		}
	}

	// Token: 0x06004645 RID: 17989 RVA: 0x0018D9C0 File Offset: 0x0018BBC0
	private void RunLayerGlobalEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			for (int i = y; i < y + height; i++)
			{
				for (int j = x; j < x + width; j++)
				{
					int num = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(num))
					{
						layer.OnEvent(num, event_data);
					}
				}
			}
		}
	}

	// Token: 0x06004646 RID: 17990 RVA: 0x0018DA14 File Offset: 0x0018BC14
	private void RunEntries(List<ScenePartitionerEntry> gathered_entries, object event_data)
	{
		for (int i = 0; i < gathered_entries.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = gathered_entries[i];
			if (scenePartitionerEntry.obj != null && scenePartitionerEntry.eventCallback != null)
			{
				scenePartitionerEntry.eventCallback(event_data);
			}
		}
	}

	// Token: 0x06004647 RID: 17991 RVA: 0x0018DA58 File Offset: 0x0018BC58
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries)
	{
		int query_id = this.queryId + 1;
		this.queryId = query_id;
		this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, query_id);
	}

	// Token: 0x06004648 RID: 17992 RVA: 0x0018DA88 File Offset: 0x0018BC88
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries, int query_id)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer2, i, j].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.queryId != this.queryId)
					{
						if (scenePartitionerEntry.obj == null)
						{
							pooledList.Add(scenePartitionerEntry);
						}
						else if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
						{
							scenePartitionerEntry.queryId = this.queryId;
							gathered_entries.Add(scenePartitionerEntry);
						}
					}
				}
				this.nodes[layer2, i, j].entries.ExceptWith(pooledList);
				pooledList.Recycle();
			}
		}
	}

	// Token: 0x06004649 RID: 17993 RVA: 0x0018DC30 File Offset: 0x0018BE30
	public void Cleanup()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600464A RID: 17994 RVA: 0x0018DC40 File Offset: 0x0018BE40
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		int x_bottomLeft = 0;
		int y_bottomLeft = 0;
		Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
		List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
		foreach (ScenePartitionerLayer layer in this.toggledLayers)
		{
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, layer, list);
			if (list.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002E8A RID: 11914
	public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();

	// Token: 0x04002E8B RID: 11915
	private int nodeSize;

	// Token: 0x04002E8C RID: 11916
	private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();

	// Token: 0x04002E8D RID: 11917
	private ScenePartitioner.ScenePartitionerNode[,,] nodes;

	// Token: 0x04002E8E RID: 11918
	private int queryId;

	// Token: 0x04002E8F RID: 11919
	private static readonly Predicate<ScenePartitionerEntry> removeCallback = (ScenePartitionerEntry entry) => entry == null || entry.obj == null;

	// Token: 0x04002E90 RID: 11920
	public HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

	// Token: 0x020017C0 RID: 6080
	private struct ScenePartitionerNode
	{
		// Token: 0x04006FCD RID: 28621
		public HashSet<ScenePartitionerEntry> entries;

		// Token: 0x04006FCE RID: 28622
		public bool dirty;
	}

	// Token: 0x020017C1 RID: 6081
	private struct DirtyNode
	{
		// Token: 0x04006FCF RID: 28623
		public int layer;

		// Token: 0x04006FD0 RID: 28624
		public int x;

		// Token: 0x04006FD1 RID: 28625
		public int y;
	}
}
