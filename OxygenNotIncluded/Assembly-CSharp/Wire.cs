using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006BE RID: 1726
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Wire")]
public class Wire : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06002EF0 RID: 12016 RVA: 0x000F7BD8 File Offset: 0x000F5DD8
	public static float GetMaxWattageAsFloat(Wire.WattageRating rating)
	{
		switch (rating)
		{
		case Wire.WattageRating.Max500:
			return 500f;
		case Wire.WattageRating.Max1000:
			return 1000f;
		case Wire.WattageRating.Max2000:
			return 2000f;
		case Wire.WattageRating.Max20000:
			return 20000f;
		case Wire.WattageRating.Max50000:
			return 50000f;
		default:
			return 0f;
		}
	}

	// Token: 0x1700033B RID: 827
	// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x000F7C24 File Offset: 0x000F5E24
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) is ElectricalUtilityNetwork;
		}
	}

	// Token: 0x1700033C RID: 828
	// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x000F7C5C File Offset: 0x000F5E5C
	public ushort NetworkID
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) as ElectricalUtilityNetwork;
			if (electricalUtilityNetwork == null)
			{
				return ushort.MaxValue;
			}
			return (ushort)electricalUtilityNetwork.id;
		}
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000F7CA0 File Offset: 0x000F5EA0
	protected override void OnSpawn()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.electricalConduitSystem.AddToNetworks(cell, this, false);
		this.InitializeSwitchState();
		base.Subscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
		base.Subscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireCircuitStatus, this);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireMaxWattageStatus, this);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Wire.OutlineSymbol, false);
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000F7D30 File Offset: 0x000F5F30
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.electricalConduitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000F7DBC File Offset: 0x000F5FBC
	private void InitializeSwitchState()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		bool flag = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			CircuitSwitch component = gameObject.GetComponent<CircuitSwitch>();
			if (component != null)
			{
				flag = true;
				component.AttachWire(this);
			}
		}
		if (!flag)
		{
			this.Connect();
		}
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x000F7E18 File Offset: 0x000F6018
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.electricalConduitSystem.GetConnections(cell, true);
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x000F7E48 File Offset: 0x000F6048
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.electricalConduitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x000F7E6C File Offset: 0x000F606C
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x000F7E74 File Offset: 0x000F6074
	private void OnBuildingFullyRepaired(object data)
	{
		this.InitializeSwitchState();
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x000F7E7C File Offset: 0x000F607C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.Wires, false);
		if (Wire.WireCircuitStatus == null)
		{
			Wire.WireCircuitStatus = new StatusItem("WireCircuitStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				string wireLoadColor = GameUtil.GetWireLoadColor(wattsUsedByCircuit, maxWattageAsFloat, wattsNeededWhenActive);
				str = str.Replace("{CurrentLoadAndColor}", (wireLoadColor == Color.white.ToHexString()) ? GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true) : string.Concat(new string[]
				{
					"<color=#",
					wireLoadColor,
					">",
					GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true),
					"</color>"
				}));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				str = str.Replace("{WireType}", this.GetProperName());
				return str;
			});
		}
		if (Wire.WireMaxWattageStatus == null)
		{
			Wire.WireMaxWattageStatus = new StatusItem("WireMaxWattageStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				str = str.Replace("{TotalPotentialLoadAndColor}", (wattsNeededWhenActive > maxWattageAsFloat) ? string.Concat(new string[]
				{
					"<color=#",
					new Color(0.9843137f, 0.6901961f, 0.23137255f).ToHexString(),
					">",
					GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true),
					"</color>"
				}) : GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				return str;
			});
		}
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x000F7F33 File Offset: 0x000F6133
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.MaxWattageRating;
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x000F7F3B File Offset: 0x000F613B
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x000F7F44 File Offset: 0x000F6144
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x000F7F8C File Offset: 0x000F618C
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x000F7FDA File Offset: 0x000F61DA
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000F7FF0 File Offset: 0x000F61F0
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

	// Token: 0x06002F01 RID: 12033 RVA: 0x000F7FFF File Offset: 0x000F61FF
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x000F800C File Offset: 0x000F620C
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x000F8048 File Offset: 0x000F6248
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x000F807E File Offset: 0x000F627E
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001BD1 RID: 7121
	[SerializeField]
	public Wire.WattageRating MaxWattageRating;

	// Token: 0x04001BD2 RID: 7122
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001BD3 RID: 7123
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x04001BD4 RID: 7124
	public float circuitOverloadTime;

	// Token: 0x04001BD5 RID: 7125
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001BD6 RID: 7126
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04001BD7 RID: 7127
	private static StatusItem WireCircuitStatus = null;

	// Token: 0x04001BD8 RID: 7128
	private static StatusItem WireMaxWattageStatus = null;

	// Token: 0x04001BD9 RID: 7129
	private System.Action firstFrameCallback;

	// Token: 0x020013FE RID: 5118
	public enum WattageRating
	{
		// Token: 0x04006402 RID: 25602
		Max500,
		// Token: 0x04006403 RID: 25603
		Max1000,
		// Token: 0x04006404 RID: 25604
		Max2000,
		// Token: 0x04006405 RID: 25605
		Max20000,
		// Token: 0x04006406 RID: 25606
		Max50000,
		// Token: 0x04006407 RID: 25607
		NumRatings
	}
}
