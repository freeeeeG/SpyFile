using System;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x0200078D RID: 1933
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
[AddComponentMenu("KMonoBehaviour/scripts/EnergyConsumer")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x060035AA RID: 13738 RVA: 0x001229A7 File Offset: 0x00120BA7
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x060035AB RID: 13739 RVA: 0x001229AF File Offset: 0x00120BAF
	// (set) Token: 0x060035AC RID: 13740 RVA: 0x001229B7 File Offset: 0x00120BB7
	public int PowerCell { get; private set; }

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x060035AD RID: 13741 RVA: 0x001229C0 File Offset: 0x00120BC0
	public bool HasWire
	{
		get
		{
			return Grid.Objects[this.PowerCell, 26] != null;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x060035AE RID: 13742 RVA: 0x001229DA File Offset: 0x00120BDA
	// (set) Token: 0x060035AF RID: 13743 RVA: 0x001229EC File Offset: 0x00120BEC
	public virtual bool IsPowered
	{
		get
		{
			return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
		}
		protected set
		{
			this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x060035B0 RID: 13744 RVA: 0x001229FF File Offset: 0x00120BFF
	public bool IsConnected
	{
		get
		{
			return this.CircuitID != ushort.MaxValue;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x060035B1 RID: 13745 RVA: 0x00122A11 File Offset: 0x00120C11
	public string Name
	{
		get
		{
			return this.selectable.GetName();
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x060035B2 RID: 13746 RVA: 0x00122A1E File Offset: 0x00120C1E
	// (set) Token: 0x060035B3 RID: 13747 RVA: 0x00122A26 File Offset: 0x00120C26
	public bool IsVirtual { get; private set; }

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x060035B4 RID: 13748 RVA: 0x00122A2F File Offset: 0x00120C2F
	// (set) Token: 0x060035B5 RID: 13749 RVA: 0x00122A37 File Offset: 0x00120C37
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060035B6 RID: 13750 RVA: 0x00122A40 File Offset: 0x00120C40
	// (set) Token: 0x060035B7 RID: 13751 RVA: 0x00122A48 File Offset: 0x00120C48
	public ushort CircuitID { get; private set; }

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060035B8 RID: 13752 RVA: 0x00122A51 File Offset: 0x00120C51
	// (set) Token: 0x060035B9 RID: 13753 RVA: 0x00122A59 File Offset: 0x00120C59
	public float BaseWattageRating
	{
		get
		{
			return this._BaseWattageRating;
		}
		set
		{
			this._BaseWattageRating = value;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060035BA RID: 13754 RVA: 0x00122A62 File Offset: 0x00120C62
	public float WattsUsed
	{
		get
		{
			if (this.operational.IsActive)
			{
				return this.BaseWattageRating;
			}
			return 0f;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060035BB RID: 13755 RVA: 0x00122A7D File Offset: 0x00120C7D
	public float WattsNeededWhenActive
	{
		get
		{
			return this.building.Def.EnergyConsumptionWhenActive;
		}
	}

	// Token: 0x060035BC RID: 13756 RVA: 0x00122A8F File Offset: 0x00120C8F
	protected override void OnPrefabInit()
	{
		this.CircuitID = ushort.MaxValue;
		this.IsPowered = false;
		this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
	}

	// Token: 0x060035BD RID: 13757 RVA: 0x00122ABC File Offset: 0x00120CBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.EnergyConsumers.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddEnergyConsumer(this);
	}

	// Token: 0x060035BE RID: 13758 RVA: 0x00122B0D File Offset: 0x00120D0D
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveEnergyConsumer(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.EnergyConsumers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x00122B41 File Offset: 0x00120D41
	public virtual void EnergySim200ms(float dt)
	{
		this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this);
		if (!this.IsConnected)
		{
			this.IsPowered = false;
		}
		this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x00122B80 File Offset: 0x00120D80
	public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.IsPowered = false;
			return;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.IsPowered && base.GetComponent<Battery>() == null)
			{
				this.IsPowered = false;
				this.circuitOverloadTime = 6f;
				this.PlayCircuitSound("overdraw");
				return;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (!this.IsPowered && this.circuitOverloadTime <= 0f)
			{
				this.IsPowered = true;
				this.PlayCircuitSound("powered");
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x00122C04 File Offset: 0x00120E04
	protected void PlayCircuitSound(string state)
	{
		EventReference event_ref;
		if (state == "powered")
		{
			event_ref = Sounds.Instance.BuildingPowerOnMigrated;
		}
		else if (state == "overdraw")
		{
			event_ref = Sounds.Instance.ElectricGridOverloadMigrated;
		}
		else
		{
			event_ref = default(EventReference);
			global::Debug.Log("Invalid state for sound in EnergyConsumer.");
		}
		if (!CameraController.Instance.IsAudibleSound(base.transform.GetPosition()))
		{
			return;
		}
		float num;
		if (!this.lastTimeSoundPlayed.TryGetValue(state, out num))
		{
			num = 0f;
		}
		float value = (Time.time - num) / this.soundDecayTime;
		Vector3 position = base.transform.GetPosition();
		position.z = 0f;
		FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(event_ref, CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		instance.setParameterByName("timeSinceLast", value, false);
		KFMOD.EndOneShot(instance);
		this.lastTimeSoundPlayed[state] = Time.time;
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x00122CF2 File Offset: 0x00120EF2
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x040020CF RID: 8399
	[MyCmpReq]
	private Building building;

	// Token: 0x040020D0 RID: 8400
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x040020D1 RID: 8401
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x040020D2 RID: 8402
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x040020D4 RID: 8404
	[Serialize]
	protected float circuitOverloadTime;

	// Token: 0x040020D5 RID: 8405
	public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);

	// Token: 0x040020D6 RID: 8406
	private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();

	// Token: 0x040020D7 RID: 8407
	private float soundDecayTime = 10f;

	// Token: 0x040020DB RID: 8411
	private float _BaseWattageRating;
}
