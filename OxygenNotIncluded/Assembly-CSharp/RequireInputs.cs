using System;
using UnityEngine;

// Token: 0x02000925 RID: 2341
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x060043E1 RID: 17377 RVA: 0x0017CA63 File Offset: 0x0017AC63
	public bool RequiresPower
	{
		get
		{
			return this.requirePower;
		}
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x060043E2 RID: 17378 RVA: 0x0017CA6B File Offset: 0x0017AC6B
	public bool RequiresInputConduit
	{
		get
		{
			return this.requireConduit;
		}
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x0017CA73 File Offset: 0x0017AC73
	public void SetRequirements(bool power, bool conduit)
	{
		this.requirePower = power;
		this.requireConduit = conduit;
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x060043E4 RID: 17380 RVA: 0x0017CA83 File Offset: 0x0017AC83
	public bool RequirementsMet
	{
		get
		{
			return this.requirementsMet;
		}
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x0017CA8B File Offset: 0x0017AC8B
	protected override void OnPrefabInit()
	{
		this.Bind();
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x0017CA93 File Offset: 0x0017AC93
	protected override void OnSpawn()
	{
		this.CheckRequirements(true);
		this.Bind();
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x0017CAA4 File Offset: 0x0017ACA4
	[ContextMenu("Bind")]
	private void Bind()
	{
		if (this.requirePower)
		{
			this.energy = base.GetComponent<IEnergyConsumer>();
			this.button = base.GetComponent<BuildingEnabledButton>();
		}
		if (this.requireConduit && !this.conduitConsumer)
		{
			this.conduitConsumer = base.GetComponent<ConduitConsumer>();
		}
	}

	// Token: 0x060043E8 RID: 17384 RVA: 0x0017CAF2 File Offset: 0x0017ACF2
	public void Sim200ms(float dt)
	{
		this.CheckRequirements(false);
	}

	// Token: 0x060043E9 RID: 17385 RVA: 0x0017CAFC File Offset: 0x0017ACFC
	private void CheckRequirements(bool forceEvent)
	{
		bool flag = true;
		bool flag2 = false;
		if (this.requirePower)
		{
			bool isConnected = this.energy.IsConnected;
			bool isPowered = this.energy.IsPowered;
			flag = (flag && isPowered && isConnected);
			bool show = this.VisualizeRequirement(RequireInputs.Requirements.NeedPower) && isConnected && !isPowered && (this.button == null || this.button.IsEnabled);
			bool show2 = this.VisualizeRequirement(RequireInputs.Requirements.NoWire) && !isConnected;
			this.needPowerStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, this.needPowerStatusGuid, show, this);
			this.noWireStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, this.noWireStatusGuid, show2, this);
			flag2 = (flag != this.RequirementsMet && base.GetComponent<Light2D>() != null);
		}
		if (this.requireConduit)
		{
			bool flag3 = !this.conduitConsumer.enabled || this.conduitConsumer.IsConnected;
			bool flag4 = !this.conduitConsumer.enabled || this.conduitConsumer.IsSatisfied;
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) && this.previouslyConnected != flag3)
			{
				this.previouslyConnected = flag3;
				StatusItem statusItem = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem = Db.Get().BuildingStatusItems.NeedLiquidIn;
					}
				}
				else
				{
					statusItem = Db.Get().BuildingStatusItems.NeedGasIn;
				}
				if (statusItem != null)
				{
					this.selectable.ToggleStatusItem(statusItem, !flag3, new global::Tuple<ConduitType, Tag>(this.conduitConsumer.TypeOfConduit, this.conduitConsumer.capacityTag));
				}
				this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag3);
			}
			flag = (flag && flag3);
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) && this.previouslySatisfied != flag4)
			{
				this.previouslySatisfied = flag4;
				StatusItem statusItem2 = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem2 = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
					}
				}
				else
				{
					statusItem2 = Db.Get().BuildingStatusItems.GasPipeEmpty;
				}
				if (this.requireConduitHasMass)
				{
					if (statusItem2 != null)
					{
						this.selectable.ToggleStatusItem(statusItem2, !flag4, this);
					}
					this.operational.SetFlag(RequireInputs.pipesHaveMass, flag4);
				}
			}
		}
		this.requirementsMet = flag;
		if (flag2)
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject != null)
			{
				Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
			}
		}
	}

	// Token: 0x060043EA RID: 17386 RVA: 0x0017CD98 File Offset: 0x0017AF98
	public bool VisualizeRequirement(RequireInputs.Requirements r)
	{
		return (this.visualizeRequirements & r) == r;
	}

	// Token: 0x04002D02 RID: 11522
	[SerializeField]
	private bool requirePower = true;

	// Token: 0x04002D03 RID: 11523
	[SerializeField]
	private bool requireConduit;

	// Token: 0x04002D04 RID: 11524
	public bool requireConduitHasMass = true;

	// Token: 0x04002D05 RID: 11525
	public RequireInputs.Requirements visualizeRequirements = RequireInputs.Requirements.All;

	// Token: 0x04002D06 RID: 11526
	private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04002D07 RID: 11527
	private static readonly Operational.Flag pipesHaveMass = new Operational.Flag("pipesHaveMass", Operational.Flag.Type.Requirement);

	// Token: 0x04002D08 RID: 11528
	private Guid noWireStatusGuid;

	// Token: 0x04002D09 RID: 11529
	private Guid needPowerStatusGuid;

	// Token: 0x04002D0A RID: 11530
	private bool requirementsMet;

	// Token: 0x04002D0B RID: 11531
	private BuildingEnabledButton button;

	// Token: 0x04002D0C RID: 11532
	private IEnergyConsumer energy;

	// Token: 0x04002D0D RID: 11533
	public ConduitConsumer conduitConsumer;

	// Token: 0x04002D0E RID: 11534
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002D0F RID: 11535
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002D10 RID: 11536
	private bool previouslyConnected = true;

	// Token: 0x04002D11 RID: 11537
	private bool previouslySatisfied = true;

	// Token: 0x02001768 RID: 5992
	[Flags]
	public enum Requirements
	{
		// Token: 0x04006EA2 RID: 28322
		None = 0,
		// Token: 0x04006EA3 RID: 28323
		NoWire = 1,
		// Token: 0x04006EA4 RID: 28324
		NeedPower = 2,
		// Token: 0x04006EA5 RID: 28325
		ConduitConnected = 4,
		// Token: 0x04006EA6 RID: 28326
		ConduitEmpty = 8,
		// Token: 0x04006EA7 RID: 28327
		AllPower = 3,
		// Token: 0x04006EA8 RID: 28328
		AllConduit = 12,
		// Token: 0x04006EA9 RID: 28329
		All = 15
	}
}
