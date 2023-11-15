using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A30 RID: 2608
public class WireUtilitySemiVirtualNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, ICircuitConnected
{
	// Token: 0x06004E26 RID: 20006 RVA: 0x001B5ED4 File Offset: 0x001B40D4
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06004E27 RID: 20007 RVA: 0x001B5EDC File Offset: 0x001B40DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001B5EE4 File Offset: 0x001B40E4
	protected override void OnSpawn()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			this.VirtualCircuitKey = component.CraftInterface;
		}
		else
		{
			CraftModuleInterface component2 = this.GetMyWorld().GetComponent<CraftModuleInterface>();
			if (component2 != null)
			{
				this.VirtualCircuitKey = component2;
			}
		}
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(this.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001B5F48 File Offset: 0x001B4148
	public void SetLinkConnected(bool connect)
	{
		if (connect && this.visualizeOnly)
		{
			this.visualizeOnly = false;
			if (base.isSpawned)
			{
				base.Connect();
				return;
			}
		}
		else if (!connect && !this.visualizeOnly)
		{
			if (base.isSpawned)
			{
				base.Disconnect();
			}
			this.visualizeOnly = true;
		}
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001B5F96 File Offset: 0x001B4196
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001B5FAE File Offset: 0x001B41AE
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001B5FC6 File Offset: 0x001B41C6
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06004E2D RID: 20013 RVA: 0x001B5FD2 File Offset: 0x001B41D2
	// (set) Token: 0x06004E2E RID: 20014 RVA: 0x001B5FDA File Offset: 0x001B41DA
	public bool IsVirtual { get; private set; }

	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x06004E2F RID: 20015 RVA: 0x001B5FE3 File Offset: 0x001B41E3
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06004E30 RID: 20016 RVA: 0x001B5FEB File Offset: 0x001B41EB
	// (set) Token: 0x06004E31 RID: 20017 RVA: 0x001B5FF3 File Offset: 0x001B41F3
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06004E32 RID: 20018 RVA: 0x001B5FFC File Offset: 0x001B41FC
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x001B6028 File Offset: 0x001B4228
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040032EE RID: 13038
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
