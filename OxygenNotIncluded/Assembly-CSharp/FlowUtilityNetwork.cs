using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A17 RID: 2583
public class FlowUtilityNetwork : UtilityNetwork
{
	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x06004D4F RID: 19791 RVA: 0x001B1D62 File Offset: 0x001AFF62
	public bool HasSinks
	{
		get
		{
			return this.sinks.Count > 0;
		}
	}

	// Token: 0x06004D50 RID: 19792 RVA: 0x001B1D72 File Offset: 0x001AFF72
	public int GetActiveCount()
	{
		return this.sinks.Count;
	}

	// Token: 0x06004D51 RID: 19793 RVA: 0x001B1D80 File Offset: 0x001AFF80
	public override void AddItem(object generic_item)
	{
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)generic_item;
		if (item != null)
		{
			switch (item.EndpointType)
			{
			case Endpoint.Source:
				if (this.sources.Contains(item))
				{
					return;
				}
				this.sources.Add(item);
				item.Network = this;
				return;
			case Endpoint.Sink:
				if (this.sinks.Contains(item))
				{
					return;
				}
				this.sinks.Add(item);
				item.Network = this;
				return;
			case Endpoint.Conduit:
				this.conduitCount++;
				return;
			default:
				item.Network = this;
				break;
			}
		}
	}

	// Token: 0x06004D52 RID: 19794 RVA: 0x001B1E10 File Offset: 0x001B0010
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		for (int i = 0; i < this.sinks.Count; i++)
		{
			FlowUtilityNetwork.IItem item = this.sinks[i];
			item.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode = grid[item.Cell];
			utilityNetworkGridNode.networkIdx = -1;
			grid[item.Cell] = utilityNetworkGridNode;
		}
		for (int j = 0; j < this.sources.Count; j++)
		{
			FlowUtilityNetwork.IItem item2 = this.sources[j];
			item2.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode2 = grid[item2.Cell];
			utilityNetworkGridNode2.networkIdx = -1;
			grid[item2.Cell] = utilityNetworkGridNode2;
		}
		this.conduitCount = 0;
		for (int k = 0; k < this.conduits.Count; k++)
		{
			FlowUtilityNetwork.IItem item3 = this.conduits[k];
			item3.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode3 = grid[item3.Cell];
			utilityNetworkGridNode3.networkIdx = -1;
			grid[item3.Cell] = utilityNetworkGridNode3;
		}
	}

	// Token: 0x0400326C RID: 12908
	public List<FlowUtilityNetwork.IItem> sources = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x0400326D RID: 12909
	public List<FlowUtilityNetwork.IItem> sinks = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x0400326E RID: 12910
	public List<FlowUtilityNetwork.IItem> conduits = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x0400326F RID: 12911
	public int conduitCount;

	// Token: 0x020018A3 RID: 6307
	public interface IItem
	{
		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x06009249 RID: 37449
		int Cell { get; }

		// Token: 0x170009A9 RID: 2473
		// (set) Token: 0x0600924A RID: 37450
		FlowUtilityNetwork Network { set; }

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x0600924B RID: 37451
		Endpoint EndpointType { get; }

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x0600924C RID: 37452
		ConduitType ConduitType { get; }

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x0600924D RID: 37453
		GameObject GameObject { get; }
	}

	// Token: 0x020018A4 RID: 6308
	public class NetworkItem : FlowUtilityNetwork.IItem
	{
		// Token: 0x0600924E RID: 37454 RVA: 0x0032B995 File Offset: 0x00329B95
		public NetworkItem(ConduitType conduit_type, Endpoint endpoint_type, int cell, GameObject parent)
		{
			this.conduitType = conduit_type;
			this.endpointType = endpoint_type;
			this.cell = cell;
			this.parent = parent;
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x0600924F RID: 37455 RVA: 0x0032B9BA File Offset: 0x00329BBA
		public Endpoint EndpointType
		{
			get
			{
				return this.endpointType;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x06009250 RID: 37456 RVA: 0x0032B9C2 File Offset: 0x00329BC2
		public ConduitType ConduitType
		{
			get
			{
				return this.conduitType;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06009251 RID: 37457 RVA: 0x0032B9CA File Offset: 0x00329BCA
		public int Cell
		{
			get
			{
				return this.cell;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06009252 RID: 37458 RVA: 0x0032B9D2 File Offset: 0x00329BD2
		// (set) Token: 0x06009253 RID: 37459 RVA: 0x0032B9DA File Offset: 0x00329BDA
		public FlowUtilityNetwork Network
		{
			get
			{
				return this.network;
			}
			set
			{
				this.network = value;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x06009254 RID: 37460 RVA: 0x0032B9E3 File Offset: 0x00329BE3
		public GameObject GameObject
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x04007289 RID: 29321
		private int cell;

		// Token: 0x0400728A RID: 29322
		private FlowUtilityNetwork network;

		// Token: 0x0400728B RID: 29323
		private Endpoint endpointType;

		// Token: 0x0400728C RID: 29324
		private ConduitType conduitType;

		// Token: 0x0400728D RID: 29325
		private GameObject parent;
	}
}
