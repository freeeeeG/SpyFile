using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020007DC RID: 2012
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Generator")]
public class Generator : KMonoBehaviour, ISaveLoadable, IEnergyProducer, ICircuitConnected
{
	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x060038B2 RID: 14514 RVA: 0x0013B3AB File Offset: 0x001395AB
	public int PowerDistributionOrder
	{
		get
		{
			return this.powerDistributionOrder;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x060038B3 RID: 14515 RVA: 0x0013B3B3 File Offset: 0x001395B3
	public virtual float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x060038B4 RID: 14516 RVA: 0x0013B3BB File Offset: 0x001395BB
	public virtual bool IsEmpty
	{
		get
		{
			return this.joulesAvailable <= 0f;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060038B5 RID: 14517 RVA: 0x0013B3CD File Offset: 0x001395CD
	public virtual float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060038B6 RID: 14518 RVA: 0x0013B3D5 File Offset: 0x001395D5
	public float WattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating * this.Efficiency;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060038B7 RID: 14519 RVA: 0x0013B3EE File Offset: 0x001395EE
	public float BaseWattageRating
	{
		get
		{
			return this.building.Def.GeneratorWattageRating;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060038B8 RID: 14520 RVA: 0x0013B400 File Offset: 0x00139600
	public float PercentFull
	{
		get
		{
			if (this.Capacity == 0f)
			{
				return 1f;
			}
			return this.joulesAvailable / this.Capacity;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060038B9 RID: 14521 RVA: 0x0013B422 File Offset: 0x00139622
	// (set) Token: 0x060038BA RID: 14522 RVA: 0x0013B42A File Offset: 0x0013962A
	public int PowerCell { get; private set; }

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x060038BB RID: 14523 RVA: 0x0013B433 File Offset: 0x00139633
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060038BC RID: 14524 RVA: 0x0013B445 File Offset: 0x00139645
	private float Efficiency
	{
		get
		{
			return Mathf.Max(1f + this.generatorOutputAttribute.GetTotalValue() / 100f, 0f);
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060038BD RID: 14525 RVA: 0x0013B468 File Offset: 0x00139668
	// (set) Token: 0x060038BE RID: 14526 RVA: 0x0013B470 File Offset: 0x00139670
	public bool IsVirtual { get; protected set; }

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x060038BF RID: 14527 RVA: 0x0013B479 File Offset: 0x00139679
	// (set) Token: 0x060038C0 RID: 14528 RVA: 0x0013B481 File Offset: 0x00139681
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x060038C1 RID: 14529 RVA: 0x0013B48C File Offset: 0x0013968C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Attributes attributes = base.gameObject.GetAttributes();
		this.generatorOutputAttribute = attributes.Add(Db.Get().Attributes.GeneratorOutput);
	}

	// Token: 0x060038C2 RID: 14530 RVA: 0x0013B4C8 File Offset: 0x001396C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Generators.Add(this);
		base.Subscribe<Generator>(-1582839653, Generator.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.capacity = Generator.CalculateCapacity(this.building.Def, null);
		this.PowerCell = this.building.GetPowerOutputCell();
		this.CheckConnectionStatus();
		Game.Instance.energySim.AddGenerator(this);
	}

	// Token: 0x060038C3 RID: 14531 RVA: 0x0013B53C File Offset: 0x0013973C
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x0013B56D File Offset: 0x0013976D
	public virtual bool IsProducingPower()
	{
		return this.operational.IsActive;
	}

	// Token: 0x060038C5 RID: 14533 RVA: 0x0013B57A File Offset: 0x0013977A
	public virtual void EnergySim200ms(float dt)
	{
		this.CheckConnectionStatus();
	}

	// Token: 0x060038C6 RID: 14534 RVA: 0x0013B584 File Offset: 0x00139784
	private void SetStatusItem(StatusItem status_item)
	{
		if (status_item != this.currentStatusItem && this.currentStatusItem != null)
		{
			this.statusItemID = this.selectable.RemoveStatusItem(this.statusItemID, false);
		}
		if (status_item != null && this.statusItemID == Guid.Empty)
		{
			this.statusItemID = this.selectable.AddStatusItem(status_item, this);
		}
		this.currentStatusItem = status_item;
	}

	// Token: 0x060038C7 RID: 14535 RVA: 0x0013B5EC File Offset: 0x001397EC
	private void CheckConnectionStatus()
	{
		if (this.CircuitID == 65535)
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoWireConnected);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, false);
			return;
		}
		if (!Game.Instance.circuitManager.HasConsumers(this.CircuitID) && !Game.Instance.circuitManager.HasBatteries(this.CircuitID))
		{
			if (this.showConnectedConsumerStatusItems)
			{
				this.SetStatusItem(Db.Get().BuildingStatusItems.NoPowerConsumers);
			}
			this.operational.SetFlag(Generator.generatorConnectedFlag, true);
			return;
		}
		this.SetStatusItem(null);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
	}

	// Token: 0x060038C8 RID: 14536 RVA: 0x0013B6AA File Offset: 0x001398AA
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveGenerator(this);
		Game.Instance.circuitManager.Disconnect(this);
		Components.Generators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060038C9 RID: 14537 RVA: 0x0013B6DD File Offset: 0x001398DD
	public static float CalculateCapacity(BuildingDef def, Element element)
	{
		if (element == null)
		{
			return def.GeneratorBaseCapacity;
		}
		return def.GeneratorBaseCapacity * (1f + (element.HasTag(GameTags.RefinedMetal) ? 1f : 0f));
	}

	// Token: 0x060038CA RID: 14538 RVA: 0x0013B70F File Offset: 0x0013990F
	public void ResetJoules()
	{
		this.joulesAvailable = 0f;
	}

	// Token: 0x060038CB RID: 14539 RVA: 0x0013B71C File Offset: 0x0013991C
	public virtual void ApplyDeltaJoules(float joulesDelta, bool canOverPower = false)
	{
		this.joulesAvailable = Mathf.Clamp(this.joulesAvailable + joulesDelta, 0f, canOverPower ? float.MaxValue : this.Capacity);
	}

	// Token: 0x060038CC RID: 14540 RVA: 0x0013B748 File Offset: 0x00139948
	public void GenerateJoules(float joulesAvailable, bool canOverPower = false)
	{
		global::Debug.Assert(base.GetComponent<Battery>() == null);
		this.joulesAvailable = Mathf.Clamp(joulesAvailable, 0f, canOverPower ? float.MaxValue : this.Capacity);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyCreated, this.joulesAvailable, this.GetProperName(), null);
		if (!Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(this.PrefabID()))
		{
			Game.Instance.savedInfo.powerCreatedbyGeneratorType.Add(this.PrefabID(), 0f);
		}
		Dictionary<Tag, float> powerCreatedbyGeneratorType = Game.Instance.savedInfo.powerCreatedbyGeneratorType;
		Tag key = this.PrefabID();
		powerCreatedbyGeneratorType[key] += this.joulesAvailable;
	}

	// Token: 0x060038CD RID: 14541 RVA: 0x0013B807 File Offset: 0x00139A07
	public void AssignJoulesAvailable(float joulesAvailable)
	{
		global::Debug.Assert(base.GetComponent<PowerTransformer>() != null);
		this.joulesAvailable = joulesAvailable;
	}

	// Token: 0x060038CE RID: 14542 RVA: 0x0013B821 File Offset: 0x00139A21
	public virtual void ConsumeEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x04002597 RID: 9623
	protected const int SimUpdateSortKey = 1001;

	// Token: 0x04002598 RID: 9624
	[MyCmpReq]
	protected Building building;

	// Token: 0x04002599 RID: 9625
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x0400259A RID: 9626
	[MyCmpReq]
	protected KSelectable selectable;

	// Token: 0x0400259B RID: 9627
	[Serialize]
	private float joulesAvailable;

	// Token: 0x0400259C RID: 9628
	[SerializeField]
	public int powerDistributionOrder;

	// Token: 0x0400259D RID: 9629
	public static readonly Operational.Flag generatorConnectedFlag = new Operational.Flag("GeneratorConnected", Operational.Flag.Type.Requirement);

	// Token: 0x0400259E RID: 9630
	protected static readonly Operational.Flag wireConnectedFlag = new Operational.Flag("generatorWireConnected", Operational.Flag.Type.Requirement);

	// Token: 0x0400259F RID: 9631
	private float capacity;

	// Token: 0x040025A3 RID: 9635
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x040025A4 RID: 9636
	[SerializeField]
	public Tag[] connectedTags = Generator.DEFAULT_CONNECTED_TAGS;

	// Token: 0x040025A5 RID: 9637
	public bool showConnectedConsumerStatusItems = true;

	// Token: 0x040025A6 RID: 9638
	private StatusItem currentStatusItem;

	// Token: 0x040025A7 RID: 9639
	private Guid statusItemID;

	// Token: 0x040025A8 RID: 9640
	private AttributeInstance generatorOutputAttribute;

	// Token: 0x040025A9 RID: 9641
	private static readonly EventSystem.IntraObjectHandler<Generator> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Generator>(delegate(Generator component, object data)
	{
		component.OnTagsChanged(data);
	});
}
