using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A29 RID: 2601
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : StateMachineComponent<WaterCooler.StatesInstance>, IApproachable, IGameObjectEffectDescriptor, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06004DE3 RID: 19939 RVA: 0x001B4A8E File Offset: 0x001B2C8E
	// (set) Token: 0x06004DE4 RID: 19940 RVA: 0x001B4A98 File Offset: 0x001B2C98
	public Tag ChosenBeverage
	{
		get
		{
			return this.chosenBeverage;
		}
		set
		{
			if (this.chosenBeverage != value)
			{
				this.chosenBeverage = value;
				base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
				this.storage.DropAll(false, false, default(Vector3), true, null);
			}
		}
	}

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001B4AE4 File Offset: 0x001B2CE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.socializeOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.specificEffect = "Socialized";
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.chores = new Chore[this.socializeOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(this), this.socializeOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WaterCooler", this, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		base.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06004DE6 RID: 19942 RVA: 0x001B4C24 File Offset: 0x001B2E24
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.CancelDrinkChores();
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001B4C88 File Offset: 0x001B2E88
	public void UpdateDrinkChores(bool force = true)
	{
		if (!force && !this.choresDirty)
		{
			return;
		}
		float num = this.storage.GetMassAvailable(this.ChosenBeverage);
		int num2 = 0;
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			CellOffset offset = this.socializeOffsets[i];
			Chore chore = this.chores[i];
			if (num2 < this.choreCount && this.IsOffsetValid(offset) && num >= 1f)
			{
				num2++;
				num -= 1f;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = new WaterCoolerChore(this, this.workables[i], null, null, new Action<Chore>(this.OnChoreEnd));
				}
			}
			else if (chore != null)
			{
				chore.Cancel("invalid");
				this.chores[i] = null;
			}
		}
		this.choresDirty = false;
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x001B4D68 File Offset: 0x001B2F68
	public void CancelDrinkChores()
	{
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (chore != null)
			{
				chore.Cancel("cancelled");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x001B4DA8 File Offset: 0x001B2FA8
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x001B4DCF File Offset: 0x001B2FCF
	private void OnChoreEnd(Chore chore)
	{
		this.choresDirty = true;
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x001B4DD8 File Offset: 0x001B2FD8
	private void OnCellChanged(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x001B4DE1 File Offset: 0x001B2FE1
	private void OnStorageChange(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06004DED RID: 19949 RVA: 0x001B4DEA File Offset: 0x001B2FEA
	public CellOffset[] GetOffsets()
	{
		return this.drinkOffsets;
	}

	// Token: 0x06004DEE RID: 19950 RVA: 0x001B4DF2 File Offset: 0x001B2FF2
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06004DEF RID: 19951 RVA: 0x001B4DFC File Offset: 0x001B2FFC
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x001B4E64 File Offset: 0x001B3064
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Socialized", true);
		foreach (global::Tuple<Tag, string> tuple in WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
		{
			this.AddRequirementDesc(list, tuple.first, 1f);
		}
		return list;
	}

	// Token: 0x06004DF1 RID: 19953 RVA: 0x001B4EE4 File Offset: 0x001B30E4
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		Effect.CreateTooltip(Db.Get().effects.Get("DuplicantGotMilk"), true, "\n    • ", true);
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = Strings.Get("STRINGS.BUILDINGS.PREFABS.WATERCOOLER.OPTION_TOOLTIPS." + WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first.ToString().ToUpper());
			if (!WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second.IsNullOrWhiteSpace())
			{
				text = text + "\n\n" + Effect.CreateTooltip(Db.Get().effects.Get(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second), false, "\n    • ", true);
			}
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, ElementLoader.GetElement(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first).name, Def.GetUISprite(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, "ui", false), text);
		}
		return array;
	}

	// Token: 0x06004DF2 RID: 19954 RVA: 0x001B4FF6 File Offset: 0x001B31F6
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.ChosenBeverage = option.tag;
	}

	// Token: 0x06004DF3 RID: 19955 RVA: 0x001B5004 File Offset: 0x001B3204
	public Tag GetSelectedOption()
	{
		return this.ChosenBeverage;
	}

	// Token: 0x040032CD RID: 13005
	public const float DRINK_MASS = 1f;

	// Token: 0x040032CE RID: 13006
	public const string SPECIFIC_EFFECT = "Socialized";

	// Token: 0x040032CF RID: 13007
	public CellOffset[] socializeOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(2, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040032D0 RID: 13008
	public int choreCount = 2;

	// Token: 0x040032D1 RID: 13009
	public float workTime = 5f;

	// Token: 0x040032D2 RID: 13010
	private CellOffset[] drinkOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040032D3 RID: 13011
	private Chore[] chores;

	// Token: 0x040032D4 RID: 13012
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x040032D5 RID: 13013
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x040032D6 RID: 13014
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040032D7 RID: 13015
	public bool choresDirty;

	// Token: 0x040032D8 RID: 13016
	[Serialize]
	private Tag chosenBeverage = GameTags.Water;

	// Token: 0x040032D9 RID: 13017
	private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>(delegate(WaterCooler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020018B1 RID: 6321
	public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
	{
		// Token: 0x06009283 RID: 37507 RVA: 0x0032C440 File Offset: 0x0032A640
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass()).PlayAnim("off");
			this.dispensing.Enter("StartMeter", delegate(WaterCooler.StatesInstance smi)
			{
				smi.StartMeter();
			}).Enter("UpdateDrinkChores.force", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.UpdateDrinkChores(true);
			}).Update("UpdateDrinkChores", delegate(WaterCooler.StatesInstance smi, float dt)
			{
				smi.master.UpdateDrinkChores(true);
			}, UpdateRate.SIM_200ms, false).Exit("CancelDrinkChores", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.CancelDrinkChores();
			}).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (WaterCooler.StatesInstance smi) => !smi.HasMinimumMass()).PlayAnim("working");
		}

		// Token: 0x040072B3 RID: 29363
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;

		// Token: 0x040072B4 RID: 29364
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;

		// Token: 0x040072B5 RID: 29365
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;
	}

	// Token: 0x020018B2 RID: 6322
	public class StatesInstance : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
	{
		// Token: 0x06009285 RID: 37509 RVA: 0x0032C5F4 File Offset: 0x0032A7F4
		public StatesInstance(WaterCooler smi) : base(smi)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_bottle", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_bottle"
			});
			this.storage = base.master.GetComponent<Storage>();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x06009286 RID: 37510 RVA: 0x0032C65C File Offset: 0x0032A85C
		private void OnStorageChange(object data)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x06009287 RID: 37511 RVA: 0x0032C694 File Offset: 0x0032A894
		public void StartMeter()
		{
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(base.smi.master.ChosenBeverage, 0f);
			if (primaryElement == null)
			{
				return;
			}
			this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), primaryElement.Element.substance.colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06009288 RID: 37512 RVA: 0x0032C6F8 File Offset: 0x0032A8F8
		public bool HasMinimumMass()
		{
			return this.storage.GetMassAvailable(ElementLoader.GetElement(base.smi.master.ChosenBeverage).id) >= 1f;
		}

		// Token: 0x040072B6 RID: 29366
		private Storage storage;

		// Token: 0x040072B7 RID: 29367
		private MeterController meter;
	}
}
