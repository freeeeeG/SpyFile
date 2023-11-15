using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000948 RID: 2376
public class RoomProber : ISim1000ms
{
	// Token: 0x06004531 RID: 17713 RVA: 0x00186258 File Offset: 0x00184458
	public RoomProber()
	{
		this.CellCavityID = new HandleVector<int>.Handle[Grid.CellCount];
		this.floodFiller = new RoomProber.CavityFloodFiller(this.CellCavityID);
		for (int i = 0; i < this.CellCavityID.Length; i++)
		{
			this.solidChanges.Add(i);
		}
		this.ProcessSolidChanges();
		this.RefreshRooms();
		World instance = World.Instance;
		instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.SolidChangedEvent));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingsChanged));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[2], new Action<int, object>(this.OnBuildingsChanged));
	}

	// Token: 0x06004532 RID: 17714 RVA: 0x0018637B File Offset: 0x0018457B
	public void Refresh()
	{
		this.ProcessSolidChanges();
		this.RefreshRooms();
	}

	// Token: 0x06004533 RID: 17715 RVA: 0x00186389 File Offset: 0x00184589
	private void SolidChangedEvent(int cell)
	{
		this.SolidChangedEvent(cell, true);
	}

	// Token: 0x06004534 RID: 17716 RVA: 0x00186393 File Offset: 0x00184593
	private void OnBuildingsChanged(int cell, object building)
	{
		if (this.GetCavityForCell(cell) != null)
		{
			this.solidChanges.Add(cell);
			this.dirty = true;
		}
	}

	// Token: 0x06004535 RID: 17717 RVA: 0x001863B2 File Offset: 0x001845B2
	public void SolidChangedEvent(int cell, bool ignoreDoors)
	{
		if (ignoreDoors && Grid.HasDoor[cell])
		{
			return;
		}
		this.solidChanges.Add(cell);
		this.dirty = true;
	}

	// Token: 0x06004536 RID: 17718 RVA: 0x001863DC File Offset: 0x001845DC
	private CavityInfo CreateNewCavity()
	{
		CavityInfo cavityInfo = new CavityInfo();
		cavityInfo.handle = this.cavityInfos.Allocate(cavityInfo);
		return cavityInfo;
	}

	// Token: 0x06004537 RID: 17719 RVA: 0x00186404 File Offset: 0x00184604
	private unsafe void ProcessSolidChanges()
	{
		int* ptr = stackalloc int[(UIntPtr)20];
		*ptr = 0;
		ptr[1] = -Grid.WidthInCells;
		ptr[2] = -1;
		ptr[3] = 1;
		ptr[4] = Grid.WidthInCells;
		foreach (int num in this.solidChanges)
		{
			for (int i = 0; i < 5; i++)
			{
				int num2 = num + ptr[i];
				if (Grid.IsValidCell(num2))
				{
					this.floodFillSet.Add(num2);
					HandleVector<int>.Handle item = this.CellCavityID[num2];
					if (item.IsValid())
					{
						this.CellCavityID[num2] = HandleVector<int>.InvalidHandle;
						this.releasedIDs.Add(item);
					}
				}
			}
		}
		CavityInfo cavityInfo = this.CreateNewCavity();
		foreach (int num3 in this.floodFillSet)
		{
			if (!this.visitedCells.Contains(num3))
			{
				HandleVector<int>.Handle handle = this.CellCavityID[num3];
				if (!handle.IsValid())
				{
					CavityInfo cavityInfo2 = cavityInfo;
					this.floodFiller.Reset(cavityInfo2.handle);
					GameUtil.FloodFillConditional(num3, new Func<int, bool>(this.floodFiller.ShouldContinue), this.visitedCells, null);
					if (this.floodFiller.NumCells > 0)
					{
						cavityInfo2.numCells = this.floodFiller.NumCells;
						cavityInfo2.minX = this.floodFiller.MinX;
						cavityInfo2.minY = this.floodFiller.MinY;
						cavityInfo2.maxX = this.floodFiller.MaxX;
						cavityInfo2.maxY = this.floodFiller.MaxY;
						cavityInfo = this.CreateNewCavity();
					}
				}
			}
		}
		if (cavityInfo.numCells == 0)
		{
			this.releasedIDs.Add(cavityInfo.handle);
		}
		foreach (HandleVector<int>.Handle handle2 in this.releasedIDs)
		{
			CavityInfo data = this.cavityInfos.GetData(handle2);
			this.releasedCritters.AddRange(data.creatures);
			if (data.room != null)
			{
				this.ClearRoom(data.room);
			}
			this.cavityInfos.Free(handle2);
		}
		this.RebuildDirtyCavities(this.visitedCells);
		this.releasedIDs.Clear();
		this.visitedCells.Clear();
		this.solidChanges.Clear();
		this.floodFillSet.Clear();
	}

	// Token: 0x06004538 RID: 17720 RVA: 0x001866D8 File Offset: 0x001848D8
	private void RebuildDirtyCavities(ICollection<int> visited_cells)
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (int num in visited_cells)
		{
			HandleVector<int>.Handle handle = this.CellCavityID[num];
			if (handle.IsValid())
			{
				CavityInfo data = this.cavityInfos.GetData(handle);
				if (0 < data.numCells && data.numCells <= maxRoomSize)
				{
					GameObject gameObject = Grid.Objects[num, 1];
					if (gameObject != null)
					{
						KPrefabID component = gameObject.GetComponent<KPrefabID>();
						bool flag = false;
						foreach (KPrefabID kprefabID in data.buildings)
						{
							if (component.InstanceID == kprefabID.InstanceID)
							{
								flag = true;
								break;
							}
						}
						foreach (KPrefabID kprefabID2 in data.plants)
						{
							if (component.InstanceID == kprefabID2.InstanceID)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (component.GetComponent<Deconstructable>())
							{
								data.AddBuilding(component);
							}
							else if (component.HasTag(GameTags.Plant) && !component.IsPrefabID("ForestTreeBranch".ToTag()))
							{
								data.AddPlants(component);
							}
						}
					}
				}
			}
		}
		visited_cells.Clear();
	}

	// Token: 0x06004539 RID: 17721 RVA: 0x001868AC File Offset: 0x00184AAC
	public void Sim1000ms(float dt)
	{
		if (this.dirty)
		{
			this.ProcessSolidChanges();
			this.RefreshRooms();
		}
	}

	// Token: 0x0600453A RID: 17722 RVA: 0x001868C4 File Offset: 0x00184AC4
	private void CreateRoom(CavityInfo cavity)
	{
		global::Debug.Assert(cavity.room == null);
		Room room = new Room();
		room.cavity = cavity;
		cavity.room = room;
		this.rooms.Add(room);
		room.roomType = Db.Get().RoomTypes.GetRoomType(room);
		this.AssignBuildingsToRoom(room);
	}

	// Token: 0x0600453B RID: 17723 RVA: 0x0018691C File Offset: 0x00184B1C
	private void ClearRoom(Room room)
	{
		this.UnassignBuildingsToRoom(room);
		room.CleanUp();
		this.rooms.Remove(room);
	}

	// Token: 0x0600453C RID: 17724 RVA: 0x00186938 File Offset: 0x00184B38
	private void RefreshRooms()
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (CavityInfo cavityInfo in this.cavityInfos.GetDataList())
		{
			if (cavityInfo.dirty)
			{
				global::Debug.Assert(cavityInfo.room == null, "I expected info.room to always be null by this point");
				if (cavityInfo.numCells > 0)
				{
					if (cavityInfo.numCells <= maxRoomSize)
					{
						this.CreateRoom(cavityInfo);
					}
					foreach (KPrefabID kprefabID in cavityInfo.buildings)
					{
						kprefabID.Trigger(144050788, cavityInfo.room);
					}
					foreach (KPrefabID kprefabID2 in cavityInfo.plants)
					{
						kprefabID2.Trigger(144050788, cavityInfo.room);
					}
				}
				cavityInfo.dirty = false;
			}
		}
		foreach (KPrefabID kprefabID3 in this.releasedCritters)
		{
			if (kprefabID3 != null)
			{
				OvercrowdingMonitor.Instance smi = kprefabID3.GetSMI<OvercrowdingMonitor.Instance>();
				if (smi != null)
				{
					smi.RoomRefreshUpdateCavity();
				}
			}
		}
		this.releasedCritters.Clear();
		this.dirty = false;
	}

	// Token: 0x0600453D RID: 17725 RVA: 0x00186ADC File Offset: 0x00184CDC
	private void AssignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		RoomType roomType = room.roomType;
		if (roomType == Db.Get().RoomTypes.Neutral)
		{
			return;
		}
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (!(kprefabID == null) && !kprefabID.HasTag(GameTags.NotRoomAssignable))
			{
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(kprefabID.GetComponent<KPrefabID>())))
				{
					component.Assign(room);
				}
			}
		}
	}

	// Token: 0x0600453E RID: 17726 RVA: 0x00186B98 File Offset: 0x00184D98
	private void UnassignKPrefabIDs(Room room, List<KPrefabID> list)
	{
		foreach (KPrefabID kprefabID in list)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, null);
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && component.assignee == room)
				{
					component.Unassign();
				}
			}
		}
	}

	// Token: 0x0600453F RID: 17727 RVA: 0x00186C14 File Offset: 0x00184E14
	private void UnassignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		this.UnassignKPrefabIDs(room, room.buildings);
		this.UnassignKPrefabIDs(room, room.plants);
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x00186C3C File Offset: 0x00184E3C
	public void UpdateRoom(CavityInfo cavity)
	{
		if (cavity == null)
		{
			return;
		}
		if (cavity.room != null)
		{
			this.ClearRoom(cavity.room);
			cavity.room = null;
		}
		this.CreateRoom(cavity);
		foreach (KPrefabID kprefabID in cavity.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(144050788, cavity.room);
			}
		}
		foreach (KPrefabID kprefabID2 in cavity.plants)
		{
			if (kprefabID2 != null)
			{
				kprefabID2.Trigger(144050788, cavity.room);
			}
		}
	}

	// Token: 0x06004541 RID: 17729 RVA: 0x00186D20 File Offset: 0x00184F20
	public Room GetRoomOfGameObject(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		int cell = Grid.PosToCell(go);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		CavityInfo cavityForCell = this.GetCavityForCell(cell);
		if (cavityForCell == null)
		{
			return null;
		}
		return cavityForCell.room;
	}

	// Token: 0x06004542 RID: 17730 RVA: 0x00186D5C File Offset: 0x00184F5C
	public bool IsInRoomType(GameObject go, RoomType checkType)
	{
		Room roomOfGameObject = this.GetRoomOfGameObject(go);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			return checkType == roomType;
		}
		return false;
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x00186D84 File Offset: 0x00184F84
	private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
	{
		CavityInfo result = null;
		if (id.IsValid())
		{
			result = this.cavityInfos.GetData(id);
		}
		return result;
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x00186DAC File Offset: 0x00184FAC
	public CavityInfo GetCavityForCell(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		HandleVector<int>.Handle id = this.CellCavityID[cell];
		return this.GetCavityInfo(id);
	}

	// Token: 0x04002DEB RID: 11755
	public List<Room> rooms = new List<Room>();

	// Token: 0x04002DEC RID: 11756
	private KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);

	// Token: 0x04002DED RID: 11757
	private HandleVector<int>.Handle[] CellCavityID;

	// Token: 0x04002DEE RID: 11758
	private bool dirty = true;

	// Token: 0x04002DEF RID: 11759
	private HashSet<int> solidChanges = new HashSet<int>();

	// Token: 0x04002DF0 RID: 11760
	private HashSet<int> visitedCells = new HashSet<int>();

	// Token: 0x04002DF1 RID: 11761
	private HashSet<int> floodFillSet = new HashSet<int>();

	// Token: 0x04002DF2 RID: 11762
	private HashSet<HandleVector<int>.Handle> releasedIDs = new HashSet<HandleVector<int>.Handle>();

	// Token: 0x04002DF3 RID: 11763
	private RoomProber.CavityFloodFiller floodFiller;

	// Token: 0x04002DF4 RID: 11764
	private List<KPrefabID> releasedCritters = new List<KPrefabID>();

	// Token: 0x0200179A RID: 6042
	public class Tuning : TuningData<RoomProber.Tuning>
	{
		// Token: 0x04006F5A RID: 28506
		public int maxRoomSize;
	}

	// Token: 0x0200179B RID: 6043
	private class CavityFloodFiller
	{
		// Token: 0x06008EF3 RID: 36595 RVA: 0x00320D4D File Offset: 0x0031EF4D
		public CavityFloodFiller(HandleVector<int>.Handle[] grid)
		{
			this.grid = grid;
		}

		// Token: 0x06008EF4 RID: 36596 RVA: 0x00320D5C File Offset: 0x0031EF5C
		public void Reset(HandleVector<int>.Handle search_id)
		{
			this.cavityID = search_id;
			this.numCells = 0;
			this.minX = int.MaxValue;
			this.minY = int.MaxValue;
			this.maxX = 0;
			this.maxY = 0;
		}

		// Token: 0x06008EF5 RID: 36597 RVA: 0x00320D90 File Offset: 0x0031EF90
		private static bool IsWall(int cell)
		{
			return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || Grid.HasDoor[cell];
		}

		// Token: 0x06008EF6 RID: 36598 RVA: 0x00320DB0 File Offset: 0x0031EFB0
		public bool ShouldContinue(int flood_cell)
		{
			if (RoomProber.CavityFloodFiller.IsWall(flood_cell))
			{
				this.grid[flood_cell] = HandleVector<int>.InvalidHandle;
				return false;
			}
			this.grid[flood_cell] = this.cavityID;
			int val;
			int val2;
			Grid.CellToXY(flood_cell, out val, out val2);
			this.minX = Math.Min(val, this.minX);
			this.minY = Math.Min(val2, this.minY);
			this.maxX = Math.Max(val, this.maxX);
			this.maxY = Math.Max(val2, this.maxY);
			this.numCells++;
			return true;
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06008EF7 RID: 36599 RVA: 0x00320E4B File Offset: 0x0031F04B
		public int NumCells
		{
			get
			{
				return this.numCells;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06008EF8 RID: 36600 RVA: 0x00320E53 File Offset: 0x0031F053
		public int MinX
		{
			get
			{
				return this.minX;
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06008EF9 RID: 36601 RVA: 0x00320E5B File Offset: 0x0031F05B
		public int MinY
		{
			get
			{
				return this.minY;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06008EFA RID: 36602 RVA: 0x00320E63 File Offset: 0x0031F063
		public int MaxX
		{
			get
			{
				return this.maxX;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06008EFB RID: 36603 RVA: 0x00320E6B File Offset: 0x0031F06B
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
		}

		// Token: 0x04006F5B RID: 28507
		private HandleVector<int>.Handle[] grid;

		// Token: 0x04006F5C RID: 28508
		private HandleVector<int>.Handle cavityID;

		// Token: 0x04006F5D RID: 28509
		private int numCells;

		// Token: 0x04006F5E RID: 28510
		private int minX;

		// Token: 0x04006F5F RID: 28511
		private int minY;

		// Token: 0x04006F60 RID: 28512
		private int maxX;

		// Token: 0x04006F61 RID: 28513
		private int maxY;
	}
}
