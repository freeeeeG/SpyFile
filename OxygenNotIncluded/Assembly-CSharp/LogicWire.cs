using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000646 RID: 1606
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LogicWire")]
public class LogicWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IBitRating, IDisconnectable
{
	// Token: 0x06002A23 RID: 10787 RVA: 0x000E1451 File Offset: 0x000DF651
	public static int GetBitDepthAsInt(LogicWire.BitDepth rating)
	{
		if (rating == LogicWire.BitDepth.OneBit)
		{
			return 1;
		}
		if (rating != LogicWire.BitDepth.FourBit)
		{
			return 0;
		}
		return 4;
	}

	// Token: 0x06002A24 RID: 10788 RVA: 0x000E1464 File Offset: 0x000DF664
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.logicCircuitSystem.AddToNetworks(cell, this, false);
		base.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
		base.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
		this.Connect();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
	}

	// Token: 0x06002A25 RID: 10789 RVA: 0x000E14D4 File Offset: 0x000DF6D4
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06002A26 RID: 10790 RVA: 0x000E1560 File Offset: 0x000DF760
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.logicCircuitSystem.GetNetworkForCell(cell) is LogicCircuitNetwork;
		}
	}

	// Token: 0x06002A27 RID: 10791 RVA: 0x000E1596 File Offset: 0x000DF796
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06002A28 RID: 10792 RVA: 0x000E15A0 File Offset: 0x000DF7A0
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06002A29 RID: 10793 RVA: 0x000E15E8 File Offset: 0x000DF7E8
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06002A2A RID: 10794 RVA: 0x000E1638 File Offset: 0x000DF838
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.logicCircuitSystem.GetConnections(cell, true);
	}

	// Token: 0x06002A2B RID: 10795 RVA: 0x000E1668 File Offset: 0x000DF868
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.logicCircuitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06002A2C RID: 10796 RVA: 0x000E168C File Offset: 0x000DF88C
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06002A2D RID: 10797 RVA: 0x000E1694 File Offset: 0x000DF894
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06002A2E RID: 10798 RVA: 0x000E169D File Offset: 0x000DF89D
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002A2F RID: 10799 RVA: 0x000E16B3 File Offset: 0x000DF8B3
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002A30 RID: 10800 RVA: 0x000E16C2 File Offset: 0x000DF8C2
	public LogicWire.BitDepth GetMaxBitRating()
	{
		return this.MaxBitDepth;
	}

	// Token: 0x06002A31 RID: 10801 RVA: 0x000E16CA File Offset: 0x000DF8CA
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06002A32 RID: 10802 RVA: 0x000E16D8 File Offset: 0x000DF8D8
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002A33 RID: 10803 RVA: 0x000E1714 File Offset: 0x000DF914
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x06002A34 RID: 10804 RVA: 0x000E174A File Offset: 0x000DF94A
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x040018A5 RID: 6309
	[SerializeField]
	public LogicWire.BitDepth MaxBitDepth;

	// Token: 0x040018A6 RID: 6310
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x040018A7 RID: 6311
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x040018A8 RID: 6312
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x040018A9 RID: 6313
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x040018AA RID: 6314
	private System.Action firstFrameCallback;

	// Token: 0x02001310 RID: 4880
	public enum BitDepth
	{
		// Token: 0x0400615E RID: 24926
		OneBit,
		// Token: 0x0400615F RID: 24927
		FourBit,
		// Token: 0x04006160 RID: 24928
		NumRatings
	}
}
