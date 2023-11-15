using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000956 RID: 2390
[AddComponentMenu("KMonoBehaviour/scripts/GameScenePartitioner")]
public class GameScenePartitioner : KMonoBehaviour
{
	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06004614 RID: 17940 RVA: 0x0018C635 File Offset: 0x0018A835
	public static GameScenePartitioner Instance
	{
		get
		{
			global::Debug.Assert(GameScenePartitioner.instance != null);
			return GameScenePartitioner.instance;
		}
	}

	// Token: 0x06004615 RID: 17941 RVA: 0x0018C64C File Offset: 0x0018A84C
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GameScenePartitioner.instance == null);
		GameScenePartitioner.instance = this;
		this.partitioner = new ScenePartitioner(16, 65, Grid.WidthInCells, Grid.HeightInCells);
		this.solidChangedLayer = this.partitioner.CreateMask("SolidChanged");
		this.liquidChangedLayer = this.partitioner.CreateMask("LiquidChanged");
		this.digDestroyedLayer = this.partitioner.CreateMask("DigDestroyed");
		this.fogOfWarChangedLayer = this.partitioner.CreateMask("FogOfWarChanged");
		this.decorProviderLayer = this.partitioner.CreateMask("DecorProviders");
		this.attackableEntitiesLayer = this.partitioner.CreateMask("FactionedEntities");
		this.fetchChoreLayer = this.partitioner.CreateMask("FetchChores");
		this.pickupablesLayer = this.partitioner.CreateMask("Pickupables");
		this.pickupablesChangedLayer = this.partitioner.CreateMask("PickupablesChanged");
		this.gasConduitsLayer = this.partitioner.CreateMask("GasConduit");
		this.liquidConduitsLayer = this.partitioner.CreateMask("LiquidConduit");
		this.solidConduitsLayer = this.partitioner.CreateMask("SolidConduit");
		this.noisePolluterLayer = this.partitioner.CreateMask("NoisePolluters");
		this.validNavCellChangedLayer = this.partitioner.CreateMask("validNavCellChangedLayer");
		this.dirtyNavCellUpdateLayer = this.partitioner.CreateMask("dirtyNavCellUpdateLayer");
		this.trapsLayer = this.partitioner.CreateMask("trapsLayer");
		this.floorSwitchActivatorLayer = this.partitioner.CreateMask("FloorSwitchActivatorLayer");
		this.floorSwitchActivatorChangedLayer = this.partitioner.CreateMask("FloorSwitchActivatorChangedLayer");
		this.collisionLayer = this.partitioner.CreateMask("Collision");
		this.lure = this.partitioner.CreateMask("Lure");
		this.plants = this.partitioner.CreateMask("Plants");
		this.industrialBuildings = this.partitioner.CreateMask("IndustrialBuildings");
		this.completeBuildings = this.partitioner.CreateMask("CompleteBuildings");
		this.prioritizableObjects = this.partitioner.CreateMask("PrioritizableObjects");
		this.contactConductiveLayer = this.partitioner.CreateMask("ContactConductiveLayer");
		this.objectLayers = new ScenePartitionerLayer[45];
		for (int i = 0; i < 45; i++)
		{
			ObjectLayer objectLayer = (ObjectLayer)i;
			this.objectLayers[i] = this.partitioner.CreateMask(objectLayer.ToString());
		}
	}

	// Token: 0x06004616 RID: 17942 RVA: 0x0018C8EC File Offset: 0x0018AAEC
	protected override void OnForcedCleanUp()
	{
		GameScenePartitioner.instance = null;
		this.partitioner.FreeResources();
		this.partitioner = null;
		this.solidChangedLayer = null;
		this.liquidChangedLayer = null;
		this.digDestroyedLayer = null;
		this.fogOfWarChangedLayer = null;
		this.decorProviderLayer = null;
		this.attackableEntitiesLayer = null;
		this.fetchChoreLayer = null;
		this.pickupablesLayer = null;
		this.pickupablesChangedLayer = null;
		this.gasConduitsLayer = null;
		this.liquidConduitsLayer = null;
		this.solidConduitsLayer = null;
		this.noisePolluterLayer = null;
		this.validNavCellChangedLayer = null;
		this.dirtyNavCellUpdateLayer = null;
		this.trapsLayer = null;
		this.floorSwitchActivatorLayer = null;
		this.floorSwitchActivatorChangedLayer = null;
		this.contactConductiveLayer = null;
		this.objectLayers = null;
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x0018C9A0 File Offset: 0x0018ABA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		navGrid.OnNavGridUpdateComplete = (Action<HashSet<int>>)Delegate.Combine(navGrid.OnNavGridUpdateComplete, new Action<HashSet<int>>(this.OnNavGridUpdateComplete));
		NavTable navTable = navGrid.NavTable;
		navTable.OnValidCellChanged = (Action<int, NavType>)Delegate.Combine(navTable.OnValidCellChanged, new Action<int, NavType>(this.OnValidNavCellChanged));
	}

	// Token: 0x06004618 RID: 17944 RVA: 0x0018CA0C File Offset: 0x0018AC0C
	public HandleVector<int>.Handle Add(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		ScenePartitionerEntry scenePartitionerEntry = new ScenePartitionerEntry(name, obj, x, y, width, height, layer, this.partitioner, event_callback);
		this.partitioner.Add(scenePartitionerEntry);
		return this.scenePartitionerEntries.Allocate(scenePartitionerEntry);
	}

	// Token: 0x06004619 RID: 17945 RVA: 0x0018CA4C File Offset: 0x0018AC4C
	public HandleVector<int>.Handle Add(string name, object obj, Extents extents, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		return this.Add(name, obj, extents.x, extents.y, extents.width, extents.height, layer, event_callback);
	}

	// Token: 0x0600461A RID: 17946 RVA: 0x0018CA80 File Offset: 0x0018AC80
	public HandleVector<int>.Handle Add(string name, object obj, int cell, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return this.Add(name, obj, x, y, 1, 1, layer, event_callback);
	}

	// Token: 0x0600461B RID: 17947 RVA: 0x0018CAAB File Offset: 0x0018ACAB
	public void AddGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Combine(layer.OnEvent, action);
	}

	// Token: 0x0600461C RID: 17948 RVA: 0x0018CAC4 File Offset: 0x0018ACC4
	public void RemoveGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Remove(layer.OnEvent, action);
	}

	// Token: 0x0600461D RID: 17949 RVA: 0x0018CADD File Offset: 0x0018ACDD
	public void TriggerEvent(List<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(cells, layer, event_data);
	}

	// Token: 0x0600461E RID: 17950 RVA: 0x0018CAED File Offset: 0x0018ACED
	public void TriggerEvent(HashSet<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(cells, layer, event_data);
	}

	// Token: 0x0600461F RID: 17951 RVA: 0x0018CAFD File Offset: 0x0018ACFD
	public void TriggerEvent(Extents extents, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(extents.x, extents.y, extents.width, extents.height, layer, event_data);
	}

	// Token: 0x06004620 RID: 17952 RVA: 0x0018CB24 File Offset: 0x0018AD24
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(x, y, width, height, layer, event_data);
	}

	// Token: 0x06004621 RID: 17953 RVA: 0x0018CB3C File Offset: 0x0018AD3C
	public void TriggerEvent(int cell, ScenePartitionerLayer layer, object event_data)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		this.TriggerEvent(x, y, 1, 1, layer, event_data);
	}

	// Token: 0x06004622 RID: 17954 RVA: 0x0018CB63 File Offset: 0x0018AD63
	public void GatherEntries(Extents extents, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.GatherEntries(extents.x, extents.y, extents.width, extents.height, layer, gathered_entries);
	}

	// Token: 0x06004623 RID: 17955 RVA: 0x0018CB85 File Offset: 0x0018AD85
	public void GatherEntries(int x_bottomLeft, int y_bottomLeft, int width, int height, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.partitioner.GatherEntries(x_bottomLeft, y_bottomLeft, width, height, layer, null, gathered_entries);
	}

	// Token: 0x06004624 RID: 17956 RVA: 0x0018CB9C File Offset: 0x0018AD9C
	public void Iterate<IteratorType>(int x, int y, int width, int height, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x, y, width, height, layer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = pooledList[i];
			iterator.Iterate(scenePartitionerEntry.obj);
		}
		pooledList.Recycle();
	}

	// Token: 0x06004625 RID: 17957 RVA: 0x0018CBF4 File Offset: 0x0018ADF4
	public void Iterate<IteratorType>(int cell, int radius, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.Iterate<IteratorType>(num - radius, num2 - radius, radius * 2, radius * 2, layer, ref iterator);
	}

	// Token: 0x06004626 RID: 17958 RVA: 0x0018CC24 File Offset: 0x0018AE24
	private void OnValidNavCellChanged(int cell, NavType nav_type)
	{
		this.changedCells.Add(cell);
	}

	// Token: 0x06004627 RID: 17959 RVA: 0x0018CC34 File Offset: 0x0018AE34
	private void OnNavGridUpdateComplete(HashSet<int> dirty_nav_cells)
	{
		if (dirty_nav_cells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(dirty_nav_cells, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, null);
		}
		if (this.changedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.validNavCellChangedLayer, null);
			this.changedCells.Clear();
		}
	}

	// Token: 0x06004628 RID: 17960 RVA: 0x0018CC94 File Offset: 0x0018AE94
	public void UpdatePosition(HandleVector<int>.Handle handle, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.UpdatePosition(handle, vector2I.x, vector2I.y);
	}

	// Token: 0x06004629 RID: 17961 RVA: 0x0018CCBB File Offset: 0x0018AEBB
	public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(x, y);
	}

	// Token: 0x0600462A RID: 17962 RVA: 0x0018CCDA File Offset: 0x0018AEDA
	public void UpdatePosition(HandleVector<int>.Handle handle, Extents ext)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(ext);
	}

	// Token: 0x0600462B RID: 17963 RVA: 0x0018CCF8 File Offset: 0x0018AEF8
	public void Free(ref HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).Release();
		this.scenePartitionerEntries.Free(handle);
		handle.Clear();
	}

	// Token: 0x0600462C RID: 17964 RVA: 0x0018CD31 File Offset: 0x0018AF31
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.partitioner.Cleanup();
	}

	// Token: 0x0600462D RID: 17965 RVA: 0x0018CD44 File Offset: 0x0018AF44
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		return this.partitioner.DoDebugLayersContainItemsOnCell(cell);
	}

	// Token: 0x0600462E RID: 17966 RVA: 0x0018CD52 File Offset: 0x0018AF52
	public List<ScenePartitionerLayer> GetLayers()
	{
		return this.partitioner.layers;
	}

	// Token: 0x0600462F RID: 17967 RVA: 0x0018CD5F File Offset: 0x0018AF5F
	public void SetToggledLayers(HashSet<ScenePartitionerLayer> toggled_layers)
	{
		this.partitioner.toggledLayers = toggled_layers;
	}

	// Token: 0x04002E6B RID: 11883
	public ScenePartitionerLayer solidChangedLayer;

	// Token: 0x04002E6C RID: 11884
	public ScenePartitionerLayer liquidChangedLayer;

	// Token: 0x04002E6D RID: 11885
	public ScenePartitionerLayer digDestroyedLayer;

	// Token: 0x04002E6E RID: 11886
	public ScenePartitionerLayer fogOfWarChangedLayer;

	// Token: 0x04002E6F RID: 11887
	public ScenePartitionerLayer decorProviderLayer;

	// Token: 0x04002E70 RID: 11888
	public ScenePartitionerLayer attackableEntitiesLayer;

	// Token: 0x04002E71 RID: 11889
	public ScenePartitionerLayer fetchChoreLayer;

	// Token: 0x04002E72 RID: 11890
	public ScenePartitionerLayer pickupablesLayer;

	// Token: 0x04002E73 RID: 11891
	public ScenePartitionerLayer pickupablesChangedLayer;

	// Token: 0x04002E74 RID: 11892
	public ScenePartitionerLayer gasConduitsLayer;

	// Token: 0x04002E75 RID: 11893
	public ScenePartitionerLayer liquidConduitsLayer;

	// Token: 0x04002E76 RID: 11894
	public ScenePartitionerLayer solidConduitsLayer;

	// Token: 0x04002E77 RID: 11895
	public ScenePartitionerLayer wiresLayer;

	// Token: 0x04002E78 RID: 11896
	public ScenePartitionerLayer[] objectLayers;

	// Token: 0x04002E79 RID: 11897
	public ScenePartitionerLayer noisePolluterLayer;

	// Token: 0x04002E7A RID: 11898
	public ScenePartitionerLayer validNavCellChangedLayer;

	// Token: 0x04002E7B RID: 11899
	public ScenePartitionerLayer dirtyNavCellUpdateLayer;

	// Token: 0x04002E7C RID: 11900
	public ScenePartitionerLayer trapsLayer;

	// Token: 0x04002E7D RID: 11901
	public ScenePartitionerLayer floorSwitchActivatorLayer;

	// Token: 0x04002E7E RID: 11902
	public ScenePartitionerLayer floorSwitchActivatorChangedLayer;

	// Token: 0x04002E7F RID: 11903
	public ScenePartitionerLayer collisionLayer;

	// Token: 0x04002E80 RID: 11904
	public ScenePartitionerLayer lure;

	// Token: 0x04002E81 RID: 11905
	public ScenePartitionerLayer plants;

	// Token: 0x04002E82 RID: 11906
	public ScenePartitionerLayer industrialBuildings;

	// Token: 0x04002E83 RID: 11907
	public ScenePartitionerLayer completeBuildings;

	// Token: 0x04002E84 RID: 11908
	public ScenePartitionerLayer prioritizableObjects;

	// Token: 0x04002E85 RID: 11909
	public ScenePartitionerLayer contactConductiveLayer;

	// Token: 0x04002E86 RID: 11910
	private ScenePartitioner partitioner;

	// Token: 0x04002E87 RID: 11911
	private static GameScenePartitioner instance;

	// Token: 0x04002E88 RID: 11912
	private KCompactedVector<ScenePartitionerEntry> scenePartitionerEntries = new KCompactedVector<ScenePartitionerEntry>(0);

	// Token: 0x04002E89 RID: 11913
	private List<int> changedCells = new List<int>();

	// Token: 0x020017BF RID: 6079
	public interface Iterator
	{
		// Token: 0x06008F61 RID: 36705
		void Iterate(object obj);

		// Token: 0x06008F62 RID: 36706
		void Cleanup();
	}
}
