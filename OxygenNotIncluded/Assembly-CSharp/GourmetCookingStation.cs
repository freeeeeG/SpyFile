using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000610 RID: 1552
public class GourmetCookingStation : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x0600270C RID: 9996 RVA: 0x000D4040 File Offset: 0x000D2240
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepAdditionalTag = this.fuelTag;
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x000D4090 File Offset: 0x000D2290
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_gourtmet_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable workable = this.workable;
		workable.OnWorkTickActions = (Action<Worker, float>)Delegate.Combine(workable.OnWorkTickActions, new Action<Worker, float>(delegate(Worker worker, float dt)
		{
			global::Debug.Assert(worker != null, "How did we get a null worker?");
			if (this.diseaseCountKillRate > 0)
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				int num = Math.Max(1, (int)((float)this.diseaseCountKillRate * dt));
				component.ModifyDiseaseCount(-num, "GourmetCookingStation");
			}
		}));
		this.smi = new GourmetCookingStation.StatesInstance(this);
		this.smi.StartSM();
		base.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x000D41B0 File Offset: 0x000D23B0
	public float GetAvailableFuel()
	{
		return this.inStorage.GetAmountAvailable(this.fuelTag);
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x000D41C4 File Offset: 0x000D23C4
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "GourmetCookingStation.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x000D4238 File Offset: 0x000D2438
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x04001663 RID: 5731
	private static readonly Operational.Flag gourmetCookingStationFlag = new Operational.Flag("gourmet_cooking_station", Operational.Flag.Type.Requirement);

	// Token: 0x04001664 RID: 5732
	public float GAS_CONSUMPTION_RATE;

	// Token: 0x04001665 RID: 5733
	public float GAS_CONVERSION_RATIO = 0.1f;

	// Token: 0x04001666 RID: 5734
	public const float START_FUEL_MASS = 5f;

	// Token: 0x04001667 RID: 5735
	public Tag fuelTag;

	// Token: 0x04001668 RID: 5736
	[SerializeField]
	private int diseaseCountKillRate = 150;

	// Token: 0x04001669 RID: 5737
	private GourmetCookingStation.StatesInstance smi;

	// Token: 0x020012C3 RID: 4803
	public class StatesInstance : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.GameInstance
	{
		// Token: 0x06007E73 RID: 32371 RVA: 0x002E85E2 File Offset: 0x002E67E2
		public StatesInstance(GourmetCookingStation smi) : base(smi)
		{
		}
	}

	// Token: 0x020012C4 RID: 4804
	public class States : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation>
	{
		// Token: 0x06007E74 RID: 32372 RVA: 0x002E85EC File Offset: 0x002E67EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			if (GourmetCookingStation.States.waitingForFuelStatus == null)
			{
				GourmetCookingStation.States.waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
				GourmetCookingStation.States.waitingForFuelStatus.resolveStringCallback = delegate(string str, object obj)
				{
					GourmetCookingStation gourmetCookingStation = (GourmetCookingStation)obj;
					return string.Format(str, gourmetCookingStation.fuelTag.ProperName(), GameUtil.GetFormattedMass(5f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				};
			}
			default_state = this.waitingForFuel;
			this.waitingForFuel.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, false);
			}).ToggleStatusItem(GourmetCookingStation.States.waitingForFuelStatus, (GourmetCookingStation.StatesInstance smi) => smi.master).EventTransition(GameHashes.OnStorageChange, this.ready, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() >= 5f);
			this.ready.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.SetQueueDirty();
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, true);
			}).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() <= 0f);
		}

		// Token: 0x040060AB RID: 24747
		public static StatusItem waitingForFuelStatus;

		// Token: 0x040060AC RID: 24748
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State waitingForFuel;

		// Token: 0x040060AD RID: 24749
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State ready;
	}
}
