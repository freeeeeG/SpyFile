using System;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public abstract class TileBase : ReusableObject
{
	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0001EF10 File Offset: 0x0001D110
	// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0001EF18 File Offset: 0x0001D118
	public Vector2Int OffsetCoord
	{
		get
		{
			return this._offsetCoord;
		}
		set
		{
			this._offsetCoord = value;
		}
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0001EF21 File Offset: 0x0001D121
	// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0001EF29 File Offset: 0x0001D129
	public virtual bool IsLanded
	{
		get
		{
			return this.isLanded;
		}
		set
		{
			this.isLanded = value;
		}
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0001EF32 File Offset: 0x0001D132
	public virtual void OnTileSelected(bool value)
	{
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0001EF34 File Offset: 0x0001D134
	public virtual void TileDown()
	{
		Singleton<GameEvents>.Instance.TileClick();
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0001EF40 File Offset: 0x0001D140
	public virtual void TileUp()
	{
		Singleton<GameEvents>.Instance.TileUp(this);
		if (DraggingActions.DraggingThis != null)
		{
			DraggingActions.DraggingThis.EndDragging();
		}
	}

	// Token: 0x040005F3 RID: 1523
	[SerializeField]
	private Vector2Int _offsetCoord;

	// Token: 0x040005F4 RID: 1524
	private bool isLanded;
}
