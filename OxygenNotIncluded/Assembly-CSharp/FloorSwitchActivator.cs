using System;
using UnityEngine;

// Token: 0x020007C7 RID: 1991
[AddComponentMenu("KMonoBehaviour/scripts/FloorSwitchActivator")]
public class FloorSwitchActivator : KMonoBehaviour
{
	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x0600374E RID: 14158 RVA: 0x0012B64B File Offset: 0x0012984B
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x0012B653 File Offset: 0x00129853
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0012B667 File Offset: 0x00129867
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0012B678 File Offset: 0x00129878
	private void OnCellChange()
	{
		int num = Grid.PosToCell(this);
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, num);
		if (Grid.IsValidCell(this.last_cell_occupied) && num != this.last_cell_occupied)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.NotifyChanged(num);
		this.last_cell_occupied = num;
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x0012B6CD File Offset: 0x001298CD
	private void NotifyChanged(int cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, this);
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0012B6E5 File Offset: 0x001298E5
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x0012B6F3 File Offset: 0x001298F3
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x0012B704 File Offset: 0x00129904
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("FloorSwitchActivator.Register", this, cell, GameScenePartitioner.Instance.floorSwitchActivatorLayer, null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "FloorSwitchActivator.Register");
		this.registered = true;
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x0012B76C File Offset: 0x0012996C
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		if (this.last_cell_occupied > -1)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.registered = false;
	}

	// Token: 0x0400220B RID: 8715
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400220C RID: 8716
	private bool registered;

	// Token: 0x0400220D RID: 8717
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400220E RID: 8718
	private int last_cell_occupied = -1;
}
