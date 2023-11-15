using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200065F RID: 1631
[SerializationConfig(MemberSerialization.OptIn)]
public class ModuleSolarPanel : Generator
{
	// Token: 0x06002B02 RID: 11010 RVA: 0x000E5834 File Offset: 0x000E3A34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.IsVirtual = true;
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x000E5844 File Offset: 0x000E3A44
	protected override void OnSpawn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		base.VirtualCircuitKey = craftInterface;
		base.OnSpawn();
		base.Subscribe<ModuleSolarPanel>(824508782, ModuleSolarPanel.OnActiveChangedDelegate);
		this.smi = new ModuleSolarPanel.StatesInstance(this);
		this.smi.StartSM();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		Grid.PosToCell(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x000E591A File Offset: 0x000E3B1A
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x000E5948 File Offset: 0x000E3B48
	protected void OnActiveChanged(object data)
	{
		StatusItem status_item = ((Operational)data).IsActive ? Db.Get().BuildingStatusItems.Wattage : Db.Get().BuildingStatusItems.GeneratorOffline;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, this);
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x000E59A0 File Offset: 0x000E3BA0
	private void UpdateStatusItem()
	{
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.Wattage, false);
		if (this.statusHandle == Guid.Empty)
		{
			this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
			return;
		}
		if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ModuleSolarPanelWattage, this);
		}
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x000E5A34 File Offset: 0x000E3C34
	public override void EnergySim200ms(float dt)
	{
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, true);
		this.operational.SetFlag(Generator.generatorConnectedFlag, true);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = 0f;
		if (Grid.IsValidCell(Grid.PosToCell(this)) && Grid.WorldIdx[Grid.PosToCell(this)] != 255)
		{
			foreach (CellOffset offset in this.solarCellOffsets)
			{
				int num2 = Grid.LightIntensity[Grid.OffsetCell(Grid.PosToCell(this), offset)];
				num += (float)num2 * 0.00053f;
			}
		}
		else
		{
			num = 60f;
		}
		num = Mathf.Clamp(num, 0f, 60f);
		this.operational.SetActive(num > 0f, false);
		Game.Instance.accumulators.Accumulate(this.accumulator, num * dt);
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / 60f);
		this.UpdateStatusItem();
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06002B08 RID: 11016 RVA: 0x000E5B72 File Offset: 0x000E3D72
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x04001933 RID: 6451
	private MeterController meter;

	// Token: 0x04001934 RID: 6452
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04001935 RID: 6453
	private ModuleSolarPanel.StatesInstance smi;

	// Token: 0x04001936 RID: 6454
	private Guid statusHandle;

	// Token: 0x04001937 RID: 6455
	private CellOffset[] solarCellOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04001938 RID: 6456
	private static readonly EventSystem.IntraObjectHandler<ModuleSolarPanel> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ModuleSolarPanel>(delegate(ModuleSolarPanel component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x0200134A RID: 4938
	public class StatesInstance : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.GameInstance
	{
		// Token: 0x06008094 RID: 32916 RVA: 0x002F1AD1 File Offset: 0x002EFCD1
		public StatesInstance(ModuleSolarPanel master) : base(master)
		{
		}
	}

	// Token: 0x0200134B RID: 4939
	public class States : GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel>
	{
		// Token: 0x06008095 RID: 32917 RVA: 0x002F1ADA File Offset: 0x002EFCDA
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.EventTransition(GameHashes.DoLaunchRocket, this.launch, null).DoNothing();
			this.launch.EventTransition(GameHashes.RocketLanded, this.idle, null);
		}

		// Token: 0x0400622A RID: 25130
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State idle;

		// Token: 0x0400622B RID: 25131
		public GameStateMachine<ModuleSolarPanel.States, ModuleSolarPanel.StatesInstance, ModuleSolarPanel, object>.State launch;
	}
}
