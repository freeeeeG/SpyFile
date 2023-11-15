using System;
using UnityEngine;

// Token: 0x02000589 RID: 1417
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/AnimTileable")]
public class AnimTileable : KMonoBehaviour
{
	// Token: 0x0600223F RID: 8767 RVA: 0x000BBFFE File Offset: 0x000BA1FE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.tags == null || this.tags.Length == 0)
		{
			this.tags = new Tag[]
			{
				base.GetComponent<KPrefabID>().PrefabTag
			};
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000BC038 File Offset: 0x000BA238
	protected override void OnSpawn()
	{
		OccupyArea component = base.GetComponent<OccupyArea>();
		if (component != null)
		{
			this.extents = component.GetExtents();
		}
		else
		{
			Building component2 = base.GetComponent<Building>();
			this.extents = component2.GetExtents();
		}
		Extents extents = new Extents(this.extents.x - 1, this.extents.y - 1, this.extents.width + 2, this.extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("AnimTileable.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnNeighbourCellsUpdated));
		this.UpdateEndCaps();
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000BC0F0 File Offset: 0x000BA2F0
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000BC108 File Offset: 0x000BA308
	private void UpdateEndCaps()
	{
		int cell = Grid.PosToCell(this);
		bool is_visible = true;
		bool is_visible2 = true;
		bool is_visible3 = true;
		bool is_visible4 = true;
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		CellOffset rotatedCellOffset = new CellOffset(this.extents.x - num - 1, 0);
		CellOffset rotatedCellOffset2 = new CellOffset(this.extents.x - num + this.extents.width, 0);
		CellOffset rotatedCellOffset3 = new CellOffset(0, this.extents.y - num2 + this.extents.height);
		CellOffset rotatedCellOffset4 = new CellOffset(0, this.extents.y - num2 - 1);
		Rotatable component = base.GetComponent<Rotatable>();
		if (component)
		{
			rotatedCellOffset = component.GetRotatedCellOffset(rotatedCellOffset);
			rotatedCellOffset2 = component.GetRotatedCellOffset(rotatedCellOffset2);
			rotatedCellOffset3 = component.GetRotatedCellOffset(rotatedCellOffset3);
			rotatedCellOffset4 = component.GetRotatedCellOffset(rotatedCellOffset4);
		}
		int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
		int num4 = Grid.OffsetCell(cell, rotatedCellOffset2);
		int num5 = Grid.OffsetCell(cell, rotatedCellOffset3);
		int num6 = Grid.OffsetCell(cell, rotatedCellOffset4);
		if (Grid.IsValidCell(num3))
		{
			is_visible = !this.HasTileableNeighbour(num3);
		}
		if (Grid.IsValidCell(num4))
		{
			is_visible2 = !this.HasTileableNeighbour(num4);
		}
		if (Grid.IsValidCell(num5))
		{
			is_visible3 = !this.HasTileableNeighbour(num5);
		}
		if (Grid.IsValidCell(num6))
		{
			is_visible4 = !this.HasTileableNeighbour(num6);
		}
		foreach (KBatchedAnimController kbatchedAnimController in base.GetComponentsInChildren<KBatchedAnimController>())
		{
			foreach (KAnimHashedString symbol in AnimTileable.leftSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol, is_visible);
			}
			foreach (KAnimHashedString symbol2 in AnimTileable.rightSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol2, is_visible2);
			}
			foreach (KAnimHashedString symbol3 in AnimTileable.topSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol3, is_visible3);
			}
			foreach (KAnimHashedString symbol4 in AnimTileable.bottomSymbols)
			{
				kbatchedAnimController.SetSymbolVisiblity(symbol4, is_visible4);
			}
		}
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x000BC340 File Offset: 0x000BA540
	private bool HasTileableNeighbour(int neighbour_cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[neighbour_cell, (int)this.objectLayer];
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && component.HasAnyTags(this.tags))
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000BC38B File Offset: 0x000BA58B
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

	// Token: 0x04001383 RID: 4995
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001384 RID: 4996
	public ObjectLayer objectLayer = ObjectLayer.Building;

	// Token: 0x04001385 RID: 4997
	public Tag[] tags;

	// Token: 0x04001386 RID: 4998
	private Extents extents;

	// Token: 0x04001387 RID: 4999
	private static readonly KAnimHashedString[] leftSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_left"),
		new KAnimHashedString("cap_left_fg"),
		new KAnimHashedString("cap_left_place")
	};

	// Token: 0x04001388 RID: 5000
	private static readonly KAnimHashedString[] rightSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_right"),
		new KAnimHashedString("cap_right_fg"),
		new KAnimHashedString("cap_right_place")
	};

	// Token: 0x04001389 RID: 5001
	private static readonly KAnimHashedString[] topSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_top"),
		new KAnimHashedString("cap_top_fg"),
		new KAnimHashedString("cap_top_place")
	};

	// Token: 0x0400138A RID: 5002
	private static readonly KAnimHashedString[] bottomSymbols = new KAnimHashedString[]
	{
		new KAnimHashedString("cap_bottom"),
		new KAnimHashedString("cap_bottom_fg"),
		new KAnimHashedString("cap_bottom_place")
	};
}
