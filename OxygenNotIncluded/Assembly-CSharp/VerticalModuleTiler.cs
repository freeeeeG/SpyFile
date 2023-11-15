using System;
using UnityEngine;

// Token: 0x02000370 RID: 880
public class VerticalModuleTiler : KMonoBehaviour
{
	// Token: 0x0600121F RID: 4639 RVA: 0x000619FC File Offset: 0x0005FBFC
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		if (this.manageTopCap)
		{
			this.topCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.topCapStr);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide = new KAnimSynchronizedController(component2, (Grid.SceneLayer)component2.GetLayer(), VerticalModuleTiler.bottomCapStr);
		}
		this.PostReorderMove();
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x00061A70 File Offset: 0x0005FC70
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x00061A88 File Offset: 0x0005FC88
	public void PostReorderMove()
	{
		this.dirty = true;
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x00061A91 File Offset: 0x0005FC91
	private void OnNeighbourCellsUpdated(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		if (this.partitionerEntry.IsValid())
		{
			this.UpdateEndCaps();
		}
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x00061AC0 File Offset: 0x0005FCC0
	private void UpdateEndCaps()
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		int cellTop = this.GetCellTop();
		int cellBottom = this.GetCellBottom();
		if (Grid.IsValidCell(cellTop))
		{
			if (this.HasWideNeighbor(cellTop))
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.topCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (Grid.IsValidCell(cellBottom))
		{
			if (this.HasWideNeighbor(cellBottom))
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.FiveWide;
			}
			else
			{
				this.bottomCapSetting = VerticalModuleTiler.AnimCapType.ThreeWide;
			}
		}
		if (this.manageTopCap)
		{
			this.topCapWide.Enable(this.topCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
		if (this.manageBottomCap)
		{
			this.bottomCapWide.Enable(this.bottomCapSetting == VerticalModuleTiler.AnimCapType.FiveWide);
		}
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x00061B64 File Offset: 0x0005FD64
	private int GetCellTop()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00061BA8 File Offset: 0x0005FDA8
	private int GetCellBottom()
	{
		int cell = Grid.PosToCell(this);
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset offset = new CellOffset(0, this.extents.y - num2 - 1);
		return Grid.OffsetCell(cell, offset);
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00061BE4 File Offset: 0x0005FDE4
	private bool HasWideNeighbor(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.GetComponent<ReorderableBuilding>() != null && component.GetComponent<Building>().Def.WidthInCells >= 5)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00061C44 File Offset: 0x0005FE44
	private void LateUpdate()
	{
		if (this.animController.Offset != this.m_previousAnimControllerOffset)
		{
			this.m_previousAnimControllerOffset = this.animController.Offset;
			this.bottomCapWide.Dirty();
			this.topCapWide.Dirty();
		}
		if (this.dirty)
		{
			if (this.partitionerEntry != HandleVector<int>.InvalidHandle)
			{
				GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
			}
			OccupyArea component = base.GetComponent<OccupyArea>();
			if (component != null)
			{
				this.extents = component.GetExtents();
			}
			Extents extents = new Extents(this.extents.x, this.extents.y - 1, this.extents.width, this.extents.height + 2);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("VerticalModuleTiler.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
			this.UpdateEndCaps();
			this.dirty = false;
		}
	}

	// Token: 0x040009C9 RID: 2505
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040009CA RID: 2506
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x040009CB RID: 2507
	private Extents extents;

	// Token: 0x040009CC RID: 2508
	private VerticalModuleTiler.AnimCapType topCapSetting;

	// Token: 0x040009CD RID: 2509
	private VerticalModuleTiler.AnimCapType bottomCapSetting;

	// Token: 0x040009CE RID: 2510
	private bool manageTopCap = true;

	// Token: 0x040009CF RID: 2511
	private bool manageBottomCap = true;

	// Token: 0x040009D0 RID: 2512
	private KAnimSynchronizedController topCapWide;

	// Token: 0x040009D1 RID: 2513
	private KAnimSynchronizedController bottomCapWide;

	// Token: 0x040009D2 RID: 2514
	private static readonly string topCapStr = "#cap_top_5";

	// Token: 0x040009D3 RID: 2515
	private static readonly string bottomCapStr = "#cap_bottom_5";

	// Token: 0x040009D4 RID: 2516
	private bool dirty;

	// Token: 0x040009D5 RID: 2517
	[MyCmpGet]
	private KAnimControllerBase animController;

	// Token: 0x040009D6 RID: 2518
	private Vector3 m_previousAnimControllerOffset;

	// Token: 0x02000FAA RID: 4010
	private enum AnimCapType
	{
		// Token: 0x04005680 RID: 22144
		ThreeWide,
		// Token: 0x04005681 RID: 22145
		FiveWide
	}
}
