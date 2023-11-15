using System;
using UnityEngine;

// Token: 0x02000A19 RID: 2585
public abstract class UtilityNetworkLink : KMonoBehaviour
{
	// Token: 0x06004D55 RID: 19797 RVA: 0x001B1F4A File Offset: 0x001B014A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate);
		base.Subscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate);
		this.Connect();
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x001B1F7A File Offset: 0x001B017A
	protected override void OnCleanUp()
	{
		base.Unsubscribe<UtilityNetworkLink>(774203113, UtilityNetworkLink.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<UtilityNetworkLink>(-1735440190, UtilityNetworkLink.OnBuildingFullyRepairedDelegate, false);
		this.Disconnect();
		base.OnCleanUp();
	}

	// Token: 0x06004D57 RID: 19799 RVA: 0x001B1FAC File Offset: 0x001B01AC
	protected void Connect()
	{
		if (!this.visualizeOnly && !this.connected)
		{
			this.connected = true;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnConnect(cell, cell2);
		}
	}

	// Token: 0x06004D58 RID: 19800 RVA: 0x001B1FE2 File Offset: 0x001B01E2
	protected virtual void OnConnect(int cell1, int cell2)
	{
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001B1FE4 File Offset: 0x001B01E4
	protected void Disconnect()
	{
		if (!this.visualizeOnly && this.connected)
		{
			this.connected = false;
			int cell;
			int cell2;
			this.GetCells(out cell, out cell2);
			this.OnDisconnect(cell, cell2);
		}
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001B201A File Offset: 0x001B021A
	protected virtual void OnDisconnect(int cell1, int cell2)
	{
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001B201C File Offset: 0x001B021C
	public void GetCells(out int linked_cell1, out int linked_cell2)
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			Orientation orientation = component.Orientation;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.GetCells(cell, orientation, out linked_cell1, out linked_cell2);
			return;
		}
		linked_cell1 = -1;
		linked_cell2 = -1;
	}

	// Token: 0x06004D5C RID: 19804 RVA: 0x001B2064 File Offset: 0x001B0264
	public void GetCells(int cell, Orientation orientation, out int linked_cell1, out int linked_cell2)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		linked_cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
		linked_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
	}

	// Token: 0x06004D5D RID: 19805 RVA: 0x001B20A0 File Offset: 0x001B02A0
	public bool AreCellsValid(int cell, Orientation orientation)
	{
		CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.link1, orientation);
		CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.link2, orientation);
		return Grid.IsCellOffsetValid(cell, rotatedCellOffset) && Grid.IsCellOffsetValid(cell, rotatedCellOffset2);
	}

	// Token: 0x06004D5E RID: 19806 RVA: 0x001B20D9 File Offset: 0x001B02D9
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06004D5F RID: 19807 RVA: 0x001B20E1 File Offset: 0x001B02E1
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06004D60 RID: 19808 RVA: 0x001B20EC File Offset: 0x001B02EC
	public int GetNetworkCell()
	{
		int result;
		int num;
		this.GetCells(out result, out num);
		return result;
	}

	// Token: 0x04003270 RID: 12912
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04003271 RID: 12913
	[SerializeField]
	public CellOffset link1;

	// Token: 0x04003272 RID: 12914
	[SerializeField]
	public CellOffset link2;

	// Token: 0x04003273 RID: 12915
	[SerializeField]
	public bool visualizeOnly;

	// Token: 0x04003274 RID: 12916
	private bool connected;

	// Token: 0x04003275 RID: 12917
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04003276 RID: 12918
	private static readonly EventSystem.IntraObjectHandler<UtilityNetworkLink> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<UtilityNetworkLink>(delegate(UtilityNetworkLink component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
