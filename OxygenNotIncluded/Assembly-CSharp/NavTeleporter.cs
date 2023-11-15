using System;

// Token: 0x02000661 RID: 1633
public class NavTeleporter : KMonoBehaviour
{
	// Token: 0x06002B14 RID: 11028 RVA: 0x000E5EE8 File Offset: 0x000E40E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.NavTeleporters, false);
		this.Register();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "NavTeleporterCellChanged");
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000E5F34 File Offset: 0x000E4134
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = this.GetCell();
		if (cell != Grid.InvalidCell)
		{
			Grid.HasNavTeleporter[cell] = false;
		}
		this.Deregister();
		Components.NavTeleporters.Remove(this);
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x000E5F73 File Offset: 0x000E4173
	public void SetOverrideCell(int cell)
	{
		this.overrideCell = cell;
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x000E5F7C File Offset: 0x000E417C
	public int GetCell()
	{
		if (this.overrideCell >= 0)
		{
			return this.overrideCell;
		}
		return Grid.OffsetCell(Grid.PosToCell(this), this.offset);
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x000E5FA0 File Offset: 0x000E41A0
	public void TwoWayTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			if (nt != null)
			{
				nt.SetTarget(null);
			}
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
			if (nt != null)
			{
				nt.SetTarget(this);
			}
		}
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000E5FFC File Offset: 0x000E41FC
	public void EnableTwoWayTarget(bool enable)
	{
		if (enable)
		{
			this.target.SetLink();
			this.SetLink();
			return;
		}
		this.target.BreakLink();
		this.BreakLink();
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000E6024 File Offset: 0x000E4224
	public void SetTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x000E6058 File Offset: 0x000E4258
	private void Register()
	{
		int cell = this.GetCell();
		if (!Grid.IsValidCell(cell))
		{
			this.lastRegisteredCell = Grid.InvalidCell;
			return;
		}
		Grid.HasNavTeleporter[cell] = true;
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		this.lastRegisteredCell = cell;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x000E60B4 File Offset: 0x000E42B4
	private void SetLink()
	{
		int cell = this.target.GetCell();
		Pathfinding.Instance.GetNavGrid(MinionConfig.MINION_NAV_GRID_NAME).teleportTransitions[this.lastRegisteredCell] = cell;
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x000E6100 File Offset: 0x000E4300
	public void Deregister()
	{
		if (this.lastRegisteredCell != Grid.InvalidCell)
		{
			this.BreakLink();
			Grid.HasNavTeleporter[this.lastRegisteredCell] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
			this.lastRegisteredCell = Grid.InvalidCell;
		}
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x000E614C File Offset: 0x000E434C
	private void BreakLink()
	{
		Pathfinding.Instance.GetNavGrid(MinionConfig.MINION_NAV_GRID_NAME).teleportTransitions.Remove(this.lastRegisteredCell);
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x000E6180 File Offset: 0x000E4380
	private void OnCellChanged()
	{
		this.Deregister();
		this.Register();
		if (this.target != null)
		{
			NavTeleporter component = this.target.GetComponent<NavTeleporter>();
			if (component != null)
			{
				component.SetTarget(this);
			}
		}
	}

	// Token: 0x0400193C RID: 6460
	private NavTeleporter target;

	// Token: 0x0400193D RID: 6461
	private int lastRegisteredCell = Grid.InvalidCell;

	// Token: 0x0400193E RID: 6462
	public CellOffset offset;

	// Token: 0x0400193F RID: 6463
	private int overrideCell = -1;
}
