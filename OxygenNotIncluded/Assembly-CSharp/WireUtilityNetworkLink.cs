using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A2F RID: 2607
public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem, ICircuitConnected
{
	// Token: 0x06004E19 RID: 19993 RVA: 0x001B5DEC File Offset: 0x001B3FEC
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001B5DF4 File Offset: 0x001B3FF4
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x001B5DFC File Offset: 0x001B3FFC
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x001B5E1F File Offset: 0x001B401F
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
		Game.Instance.circuitManager.Connect(this);
	}

	// Token: 0x06004E1D RID: 19997 RVA: 0x001B5E42 File Offset: 0x001B4042
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06004E1E RID: 19998 RVA: 0x001B5E4E File Offset: 0x001B404E
	// (set) Token: 0x06004E1F RID: 19999 RVA: 0x001B5E56 File Offset: 0x001B4056
	public bool IsVirtual { get; private set; }

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06004E20 RID: 20000 RVA: 0x001B5E5F File Offset: 0x001B405F
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06004E21 RID: 20001 RVA: 0x001B5E67 File Offset: 0x001B4067
	// (set) Token: 0x06004E22 RID: 20002 RVA: 0x001B5E6F File Offset: 0x001B406F
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06004E23 RID: 20003 RVA: 0x001B5E78 File Offset: 0x001B4078
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001B5EA4 File Offset: 0x001B40A4
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040032EB RID: 13035
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
