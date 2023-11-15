using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020005D5 RID: 1493
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Conduit")]
public class Conduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IDisconnectable, FlowUtilityNetwork.IItem
{
	// Token: 0x06002507 RID: 9479 RVA: 0x000CA650 File Offset: 0x000C8850
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x000CA666 File Offset: 0x000C8866
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

	// Token: 0x06002509 RID: 9481 RVA: 0x000CA678 File Offset: 0x000C8878
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Conduit>(-1201923725, Conduit.OnHighlightedDelegate);
		base.Subscribe<Conduit>(-700727624, Conduit.OnConduitFrozenDelegate);
		base.Subscribe<Conduit>(-1152799878, Conduit.OnConduitBoilingDelegate);
		base.Subscribe<Conduit>(-1555603773, Conduit.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000CA6CF File Offset: 0x000C88CF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate);
		base.Subscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000CA6FC File Offset: 0x000C88FC
	protected virtual void OnStructureTemperatureRegistered(object data)
	{
		int cell = Grid.PosToCell(this);
		this.GetNetworkManager().AddToNetworks(cell, this, false);
		this.Connect();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, this);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().AddThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000CA794 File Offset: 0x000C8994
	protected override void OnCleanUp()
	{
		base.Unsubscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate, false);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().RemoveThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
		}
		base.OnCleanUp();
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000CA888 File Offset: 0x000C8A88
	protected ConduitFlowVisualizer GetFlowVisualizer()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidFlowVisualizer;
		}
		return Game.Instance.gasFlowVisualizer;
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000CA8A8 File Offset: 0x000C8AA8
	public IUtilityNetworkMgr GetNetworkManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x0600250F RID: 9487 RVA: 0x000CA8C8 File Offset: 0x000C8AC8
	public ConduitFlow GetFlowManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000CA8E8 File Offset: 0x000C8AE8
	public static ConduitFlow GetFlowManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000CA903 File Offset: 0x000C8B03
	public static IUtilityNetworkMgr GetNetworkManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000CA920 File Offset: 0x000C8B20
	public virtual void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002513 RID: 9491 RVA: 0x000CA94C File Offset: 0x000C8B4C
	public virtual bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		return networks.Contains(networkForCell);
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000CA972 File Offset: 0x000C8B72
	public virtual int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x000CA97C File Offset: 0x000C8B7C
	private void OnHighlighted(object data)
	{
		int highlightedCell = ((bool)data) ? Grid.PosToCell(base.transform.GetPosition()) : -1;
		this.GetFlowVisualizer().SetHighlightedCell(highlightedCell);
	}

	// Token: 0x06002516 RID: 9494 RVA: 0x000CA9B4 File Offset: 0x000C8BB4
	private void OnConduitFrozen(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
			takeDamageEffect = ((this.ConduitType == ConduitType.Gas) ? SpawnFXHashes.BuildingLeakLiquid : SpawnFXHashes.BuildingFreeze),
			fullDamageEffectName = ((this.ConduitType == ConduitType.Gas) ? "water_damage_kanim" : "ice_damage_kanim")
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06002517 RID: 9495 RVA: 0x000CAA58 File Offset: 0x000C8C58
	private void OnConduitBoiling(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
			takeDamageEffect = SpawnFXHashes.BuildingLeakGas,
			fullDamageEffectName = "gas_damage_kanim"
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x000CAADB File Offset: 0x000C8CDB
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000CAAE3 File Offset: 0x000C8CE3
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x000CAAEC File Offset: 0x000C8CEC
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000CAAF4 File Offset: 0x000C8CF4
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			this.GetNetworkManager().ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000CAB35 File Offset: 0x000C8D35
	public void Disconnect()
	{
		this.disconnected = true;
		this.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x170001DB RID: 475
	// (set) Token: 0x0600251D RID: 9501 RVA: 0x000CAB49 File Offset: 0x000C8D49
	public FlowUtilityNetwork Network
	{
		set
		{
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x0600251E RID: 9502 RVA: 0x000CAB4B File Offset: 0x000C8D4B
	public int Cell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600251F RID: 9503 RVA: 0x000CAB53 File Offset: 0x000C8D53
	public Endpoint EndpointType
	{
		get
		{
			return Endpoint.Conduit;
		}
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06002520 RID: 9504 RVA: 0x000CAB56 File Offset: 0x000C8D56
	public ConduitType ConduitType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06002521 RID: 9505 RVA: 0x000CAB5E File Offset: 0x000C8D5E
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x0400153E RID: 5438
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x0400153F RID: 5439
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001540 RID: 5440
	public ConduitType type;

	// Token: 0x04001541 RID: 5441
	private System.Action firstFrameCallback;

	// Token: 0x04001542 RID: 5442
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnHighlighted(data);
	});

	// Token: 0x04001543 RID: 5443
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitFrozen(data);
	});

	// Token: 0x04001544 RID: 5444
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitBoiling(data);
	});

	// Token: 0x04001545 RID: 5445
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x04001546 RID: 5446
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001547 RID: 5447
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
