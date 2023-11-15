using System;
using System.Collections.Generic;

// Token: 0x02000852 RID: 2130
public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06003E40 RID: 15936 RVA: 0x0015A1AE File Offset: 0x001583AE
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x0015A1B6 File Offset: 0x001583B6
	protected override void OnConnect(int cell1, int cell2)
	{
		this.cell_one = cell1;
		this.cell_two = cell2;
		Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Connect(this);
	}

	// Token: 0x06003E42 RID: 15938 RVA: 0x0015A1E7 File Offset: 0x001583E7
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Disconnect(this);
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x0015A20A File Offset: 0x0015840A
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x0015A218 File Offset: 0x00158418
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06003E45 RID: 15941 RVA: 0x0015A244 File Offset: 0x00158444
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x0400286A RID: 10346
	public LogicWire.BitDepth bitDepth;

	// Token: 0x0400286B RID: 10347
	public int cell_one;

	// Token: 0x0400286C RID: 10348
	public int cell_two;
}
