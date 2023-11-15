using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x020008C0 RID: 2240
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/OccupyArea")]
public class OccupyArea : KMonoBehaviour
{
	// Token: 0x17000480 RID: 1152
	// (get) Token: 0x060040DE RID: 16606 RVA: 0x0016A469 File Offset: 0x00168669
	public CellOffset[] OccupiedCellsOffsets
	{
		get
		{
			this.UpdateRotatedCells();
			return this._RotatedOccupiedCellsOffsets;
		}
	}

	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x060040DF RID: 16607 RVA: 0x0016A477 File Offset: 0x00168677
	// (set) Token: 0x060040E0 RID: 16608 RVA: 0x0016A47F File Offset: 0x0016867F
	public bool ApplyToCells
	{
		get
		{
			return this.applyToCells;
		}
		set
		{
			if (value != this.applyToCells)
			{
				if (value)
				{
					this.UpdateOccupiedArea();
				}
				else
				{
					this.ClearOccupiedArea();
				}
				this.applyToCells = value;
			}
		}
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x0016A4A2 File Offset: 0x001686A2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.applyToCells)
		{
			this.UpdateOccupiedArea();
		}
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x0016A4B8 File Offset: 0x001686B8
	private void ValidatePosition()
	{
		if (!Grid.IsValidCell(Grid.PosToCell(this)))
		{
			global::Debug.LogWarning(base.name + " is outside the grid! DELETING!");
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x0016A4E7 File Offset: 0x001686E7
	[OnSerializing]
	private void OnSerializing()
	{
		this.ValidatePosition();
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x0016A4EF File Offset: 0x001686EF
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ValidatePosition();
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x0016A4F7 File Offset: 0x001686F7
	public void SetCellOffsets(CellOffset[] cells)
	{
		this._UnrotatedOccupiedCellsOffsets = cells;
		this._RotatedOccupiedCellsOffsets = cells;
		this.UpdateRotatedCells();
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x0016A510 File Offset: 0x00168710
	private void UpdateRotatedCells()
	{
		if (this.rotatable != null && this.appliedOrientation != this.rotatable.Orientation)
		{
			this._RotatedOccupiedCellsOffsets = new CellOffset[this._UnrotatedOccupiedCellsOffsets.Length];
			for (int i = 0; i < this._UnrotatedOccupiedCellsOffsets.Length; i++)
			{
				CellOffset offset = this._UnrotatedOccupiedCellsOffsets[i];
				this._RotatedOccupiedCellsOffsets[i] = this.rotatable.GetRotatedCellOffset(offset);
			}
			this.appliedOrientation = this.rotatable.Orientation;
		}
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x0016A59C File Offset: 0x0016879C
	public bool CheckIsOccupying(int checkCell)
	{
		int num = Grid.PosToCell(base.gameObject);
		if (checkCell == num)
		{
			return true;
		}
		foreach (CellOffset offset in this.OccupiedCellsOffsets)
		{
			if (Grid.OffsetCell(num, offset) == checkCell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x0016A5E5 File Offset: 0x001687E5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearOccupiedArea();
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x0016A5F4 File Offset: 0x001687F4
	private void ClearOccupiedArea()
	{
		if (this.occupiedGridCells == null)
		{
			return;
		}
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				foreach (int cell in this.occupiedGridCells)
				{
					if (Grid.Objects[cell, (int)objectLayer] == base.gameObject)
					{
						Grid.Objects[cell, (int)objectLayer] = null;
					}
				}
			}
		}
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x0016A670 File Offset: 0x00168870
	public void UpdateOccupiedArea()
	{
		if (this.objectLayers.Length == 0)
		{
			return;
		}
		if (this.occupiedGridCells == null)
		{
			this.occupiedGridCells = new int[this.OccupiedCellsOffsets.Length];
		}
		this.ClearOccupiedArea();
		int cell = Grid.PosToCell(base.gameObject);
		foreach (ObjectLayer objectLayer in this.objectLayers)
		{
			if (objectLayer != ObjectLayer.NumLayers)
			{
				for (int j = 0; j < this.OccupiedCellsOffsets.Length; j++)
				{
					CellOffset offset = this.OccupiedCellsOffsets[j];
					int num = Grid.OffsetCell(cell, offset);
					Grid.Objects[num, (int)objectLayer] = base.gameObject;
					this.occupiedGridCells[j] = num;
				}
			}
		}
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x0016A720 File Offset: 0x00168920
	public int GetWidthInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.x);
			num2 = Math.Max(num2, cellOffset.x);
		}
		return num2 - num + 1;
	}

	// Token: 0x060040EC RID: 16620 RVA: 0x0016A778 File Offset: 0x00168978
	public int GetHeightInCells()
	{
		int num = int.MaxValue;
		int num2 = int.MinValue;
		foreach (CellOffset cellOffset in this.OccupiedCellsOffsets)
		{
			num = Math.Min(num, cellOffset.y);
			num2 = Math.Max(num2, cellOffset.y);
		}
		return num2 - num + 1;
	}

	// Token: 0x060040ED RID: 16621 RVA: 0x0016A7D0 File Offset: 0x001689D0
	public Extents GetExtents()
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets);
	}

	// Token: 0x060040EE RID: 16622 RVA: 0x0016A7E8 File Offset: 0x001689E8
	public Extents GetExtents(Orientation orientation)
	{
		return new Extents(Grid.PosToCell(base.gameObject), this.OccupiedCellsOffsets, orientation);
	}

	// Token: 0x060040EF RID: 16623 RVA: 0x0016A804 File Offset: 0x00168A04
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.PosToCell(base.gameObject);
		if (this.OccupiedCellsOffsets != null)
		{
			foreach (CellOffset offset in this.OccupiedCellsOffsets)
			{
				Gizmos.color = Color.cyan;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one);
			}
		}
		if (this.AboveOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset2 in this.AboveOccupiedCellOffsets)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset2)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
		if (this.BelowOccupiedCellOffsets != null)
		{
			foreach (CellOffset offset3 in this.BelowOccupiedCellOffsets)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube(Grid.CellToPos(Grid.OffsetCell(cell, offset3)) + Vector3.right / 2f + Vector3.up / 2f, Vector3.one * 0.9f);
			}
		}
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x0016A97C File Offset: 0x00168B7C
	public bool CanOccupyArea(int rootCell, ObjectLayer layer)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int cell = Grid.OffsetCell(rootCell, offset);
			if (Grid.Objects[cell, (int)layer] != null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060040F1 RID: 16625 RVA: 0x0016A9C8 File Offset: 0x00168BC8
	public bool TestArea(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
		{
			CellOffset offset = this.OccupiedCellsOffsets[i];
			int arg = Grid.OffsetCell(rootCell, offset);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060040F2 RID: 16626 RVA: 0x0016AA0C File Offset: 0x00168C0C
	public bool TestAreaAbove(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.AboveOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y + 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.AboveOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.AboveOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.AboveOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060040F3 RID: 16627 RVA: 0x0016AABC File Offset: 0x00168CBC
	public bool TestAreaBelow(int rootCell, object data, Func<int, object, bool> testDelegate)
	{
		if (this.BelowOccupiedCellOffsets == null)
		{
			List<CellOffset> list = new List<CellOffset>();
			for (int i = 0; i < this.OccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = new CellOffset(this.OccupiedCellsOffsets[i].x, this.OccupiedCellsOffsets[i].y - 1);
				if (Array.IndexOf<CellOffset>(this.OccupiedCellsOffsets, cellOffset) == -1)
				{
					list.Add(cellOffset);
				}
			}
			this.BelowOccupiedCellOffsets = list.ToArray();
		}
		for (int j = 0; j < this.BelowOccupiedCellOffsets.Length; j++)
		{
			int arg = Grid.OffsetCell(rootCell, this.BelowOccupiedCellOffsets[j]);
			if (!testDelegate(arg, data))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x04002A3D RID: 10813
	private CellOffset[] AboveOccupiedCellOffsets;

	// Token: 0x04002A3E RID: 10814
	private CellOffset[] BelowOccupiedCellOffsets;

	// Token: 0x04002A3F RID: 10815
	private int[] occupiedGridCells;

	// Token: 0x04002A40 RID: 10816
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002A41 RID: 10817
	private Orientation appliedOrientation;

	// Token: 0x04002A42 RID: 10818
	public CellOffset[] _UnrotatedOccupiedCellsOffsets;

	// Token: 0x04002A43 RID: 10819
	public CellOffset[] _RotatedOccupiedCellsOffsets;

	// Token: 0x04002A44 RID: 10820
	public ObjectLayer[] objectLayers = new ObjectLayer[0];

	// Token: 0x04002A45 RID: 10821
	[SerializeField]
	private bool applyToCells = true;
}
