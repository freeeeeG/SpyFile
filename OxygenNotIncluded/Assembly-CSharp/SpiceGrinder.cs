using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200034E RID: 846
public class SpiceGrinder : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
	// Token: 0x06001141 RID: 4417 RVA: 0x0005CFA4 File Offset: 0x0005B1A4
	public static void InitializeSpices()
	{
		Spices spices = Db.Get().Spices;
		SpiceGrinder.SettingOptions = new Dictionary<Tag, SpiceGrinder.Option>();
		for (int i = 0; i < spices.Count; i++)
		{
			Spice spice = spices[i];
			if (DlcManager.IsDlcListValidForCurrentContent(spice.DlcIds))
			{
				SpiceGrinder.SettingOptions.Add(spice.Id, new SpiceGrinder.Option(spice));
			}
		}
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x0005D008 File Offset: 0x0005B208
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.OnEnterRoot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameEvent.Callback(this.OnStorageChanged));
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational)).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).Enter(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.Play((smi.SelectedOption != null) ? "off" : "default", KAnim.PlayMode.Once);
			smi.CancelFetches("inoperational");
			if (smi.SelectedOption == null)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, null);
			}
		}).Exit(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, false);
		});
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.ready, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsTrue).Update(delegate(SpiceGrinder.StatesInstance smi, float dt)
		{
			if (smi.CurrentFood != null && !smi.HasOpenFetches)
			{
				bool value = smi.CanSpice(smi.CurrentFood.Calories);
				this.isReady.Set(value, smi, false);
			}
		}, UpdateRate.SIM_1000ms, false).PlayAnim("on");
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.operational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsFalse).ToggleRecurringChore(new Func<SpiceGrinder.StatesInstance, Chore>(this.CreateChore), null);
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x0005D1A3 File Offset: 0x0005B3A3
	private void UpdateInKitchen(SpiceGrinder.StatesInstance smi)
	{
		smi.GetComponent<Operational>().SetFlag(SpiceGrinder.inKitchen, smi.roomTracker.IsInCorrectRoom());
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x0005D1C0 File Offset: 0x0005B3C0
	private void OnEnterRoot(SpiceGrinder.StatesInstance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x0005D1C8 File Offset: 0x0005B3C8
	private bool IsOperational(SpiceGrinder.StatesInstance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x0005D1D0 File Offset: 0x0005B3D0
	private void OnStorageChanged(SpiceGrinder.StatesInstance smi, object data)
	{
		smi.UpdateMeter();
		smi.UpdateFoodSymbol();
		if (smi.SelectedOption == null)
		{
			return;
		}
		bool value = smi.AvailableFood > 0f && smi.CanSpice(smi.CurrentFood.Calories);
		smi.sm.isReady.Set(value, smi, false);
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x0005D228 File Offset: 0x0005B428
	private Chore CreateChore(SpiceGrinder.StatesInstance smi)
	{
		return new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x0400096C RID: 2412
	public static Dictionary<Tag, SpiceGrinder.Option> SettingOptions = null;

	// Token: 0x0400096D RID: 2413
	public static readonly Operational.Flag spiceSet = new Operational.Flag("spiceSet", Operational.Flag.Type.Functional);

	// Token: 0x0400096E RID: 2414
	public static Operational.Flag inKitchen = new Operational.Flag("inKitchen", Operational.Flag.Type.Functional);

	// Token: 0x0400096F RID: 2415
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State inoperational;

	// Token: 0x04000970 RID: 2416
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State operational;

	// Token: 0x04000971 RID: 2417
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State ready;

	// Token: 0x04000972 RID: 2418
	public StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.BoolParameter isReady;

	// Token: 0x02000F9F RID: 3999
	public class Option : IConfigurableConsumerOption
	{
		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600728B RID: 29323 RVA: 0x002BFBA0 File Offset: 0x002BDDA0
		public Effect StatBonus
		{
			get
			{
				if (this.statBonus == null)
				{
					return null;
				}
				if (string.IsNullOrEmpty(this.spiceDescription))
				{
					this.CreateDescription();
					this.GetName();
				}
				this.statBonus.Name = this.name;
				this.statBonus.description = this.spiceDescription;
				return this.statBonus;
			}
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x002BFBFC File Offset: 0x002BDDFC
		public Option(Spice spice)
		{
			this.Id = new Tag(spice.Id);
			this.Spice = spice;
			if (spice.StatBonus != null)
			{
				this.statBonus = new Effect(spice.Id, this.GetName(), this.spiceDescription, 600f, true, false, false, null, -1f, 0f, null, "");
				this.statBonus.Add(spice.StatBonus);
				Db.Get().effects.Add(this.statBonus);
			}
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x002BFC8C File Offset: 0x002BDE8C
		public Tag GetID()
		{
			return this.Spice.Id;
		}

		// Token: 0x0600728E RID: 29326 RVA: 0x002BFCA0 File Offset: 0x002BDEA0
		public string GetName()
		{
			if (string.IsNullOrEmpty(this.name))
			{
				string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".NAME";
				StringEntry stringEntry;
				Strings.TryGet(text, out stringEntry);
				this.name = "MISSING " + text;
				if (stringEntry != null)
				{
					this.name = stringEntry;
				}
			}
			return this.name;
		}

		// Token: 0x0600728F RID: 29327 RVA: 0x002BFD09 File Offset: 0x002BDF09
		public string GetDetailedDescription()
		{
			if (string.IsNullOrEmpty(this.fullDescription))
			{
				this.CreateDescription();
			}
			return this.fullDescription;
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x002BFD24 File Offset: 0x002BDF24
		public string GetDescription()
		{
			if (!string.IsNullOrEmpty(this.spiceDescription))
			{
				return this.spiceDescription;
			}
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			return this.spiceDescription;
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x002BFD94 File Offset: 0x002BDF94
		private void CreateDescription()
		{
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			this.ingredientDescriptions = string.Format("\n\n<b>{0}</b>", BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER);
			for (int i = 0; i < this.Spice.Ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = this.Spice.Ingredients[i];
				GameObject prefab = Assets.GetPrefab((ingredient.IngredientSet != null && ingredient.IngredientSet.Length != 0) ? ingredient.IngredientSet[0] : null);
				this.ingredientDescriptions += string.Format("\n{0}{1} {2}{3}", new object[]
				{
					"    • ",
					prefab.GetProperName(),
					ingredient.AmountKG,
					GameUtil.GetUnitTypeMassOrUnit(prefab)
				});
			}
			this.fullDescription = this.spiceDescription + this.ingredientDescriptions;
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x002BFEB9 File Offset: 0x002BE0B9
		public Sprite GetIcon()
		{
			return Assets.GetSprite(this.Spice.Image);
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x002BFED0 File Offset: 0x002BE0D0
		public IConfigurableConsumerIngredient[] GetIngredients()
		{
			return this.Spice.Ingredients;
		}

		// Token: 0x04005653 RID: 22099
		public readonly Tag Id;

		// Token: 0x04005654 RID: 22100
		public readonly Spice Spice;

		// Token: 0x04005655 RID: 22101
		private string name;

		// Token: 0x04005656 RID: 22102
		private string fullDescription;

		// Token: 0x04005657 RID: 22103
		private string spiceDescription;

		// Token: 0x04005658 RID: 22104
		private string ingredientDescriptions;

		// Token: 0x04005659 RID: 22105
		private Effect statBonus;
	}

	// Token: 0x02000FA0 RID: 4000
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000FA1 RID: 4001
	public class StatesInstance : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameInstance
	{
		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06007295 RID: 29333 RVA: 0x002BFEF2 File Offset: 0x002BE0F2
		public bool IsOperational
		{
			get
			{
				return this.operational != null && this.operational.IsOperational;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06007296 RID: 29334 RVA: 0x002BFF0F File Offset: 0x002BE10F
		public float AvailableFood
		{
			get
			{
				if (!(this.foodStorage == null))
				{
					return this.foodStorage.MassStored();
				}
				return 0f;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06007297 RID: 29335 RVA: 0x002BFF30 File Offset: 0x002BE130
		public SpiceGrinder.Option SelectedOption
		{
			get
			{
				if (!(this.currentSpice.Id == Tag.Invalid))
				{
					return SpiceGrinder.SettingOptions[this.currentSpice.Id];
				}
				return null;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06007298 RID: 29336 RVA: 0x002BFF60 File Offset: 0x002BE160
		public Edible CurrentFood
		{
			get
			{
				GameObject gameObject = this.foodStorage.FindFirst(GameTags.Edible);
				this.currentFood = ((gameObject != null) ? gameObject.GetComponent<Edible>() : null);
				return this.currentFood;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06007299 RID: 29337 RVA: 0x002BFF9C File Offset: 0x002BE19C
		public bool HasOpenFetches
		{
			get
			{
				return Array.Exists<FetchChore>(this.SpiceFetches, (FetchChore fetch) => fetch != null);
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x0600729A RID: 29338 RVA: 0x002BFFC8 File Offset: 0x002BE1C8
		// (set) Token: 0x0600729B RID: 29339 RVA: 0x002BFFD0 File Offset: 0x002BE1D0
		public bool AllowMutantSeeds
		{
			get
			{
				return this.allowMutantSeeds;
			}
			set
			{
				this.allowMutantSeeds = value;
				this.ToggleMutantSeedFetches(this.allowMutantSeeds);
			}
		}

		// Token: 0x0600729C RID: 29340 RVA: 0x002BFFE8 File Offset: 0x002BE1E8
		public StatesInstance(IStateMachineTarget master, SpiceGrinder.Def def) : base(master, def)
		{
			this.workable.Grinder = this;
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.foodStorage = components[0];
			this.seedStorage = components[1];
			this.operational = base.GetComponent<Operational>();
			this.kbac = base.GetComponent<KBatchedAnimController>();
			this.foodStorageFilter = new FilteredStorage(base.GetComponent<KPrefabID>(), this.foodFilter, null, false, Db.Get().ChoreTypes.CookFetch);
			this.foodStorageFilter.SetHasMeter(false);
			this.meter = new MeterController(this.kbac, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_frame",
				"meter_level"
			});
			this.SetupFoodSymbol();
			this.UpdateFoodSymbol();
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.sm.UpdateInKitchen(this);
			Prioritizable.AddRef(base.gameObject);
			base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x0600729D RID: 29341 RVA: 0x002C0116 File Offset: 0x002BE316
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Prioritizable.RemoveRef(base.gameObject);
		}

		// Token: 0x0600729E RID: 29342 RVA: 0x002C012C File Offset: 0x002BE32C
		public void Initialize()
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
				if (this.AllowMutantSeeds)
				{
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
					}
				}
			}
			SpiceGrinder.Option spiceOption;
			SpiceGrinder.SettingOptions.TryGetValue(new Tag(this.spiceHash), out spiceOption);
			this.OnOptionSelected(spiceOption);
			base.sm.OnStorageChanged(this, null);
			this.UpdateMeter();
		}

		// Token: 0x0600729F RID: 29343 RVA: 0x002C01C4 File Offset: 0x002BE3C4
		private void OnRefreshUserMenu(object data)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				Game.Instance.userMenu.AddButton(base.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", base.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT, delegate()
				{
					base.smi.AllowMutantSeeds = !base.smi.AllowMutantSeeds;
					this.OnRefreshUserMenu(base.smi);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
			}
		}

		// Token: 0x060072A0 RID: 29344 RVA: 0x002C0240 File Offset: 0x002BE440
		public void ToggleMutantSeedFetches(bool allow)
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.UpdateMutantSeedFetches();
				if (allow)
				{
					this.seedStorage.storageFilters.Add(GameTags.MutatedSeed);
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
						return;
					}
				}
				else
				{
					if (this.seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0f)
					{
						this.seedStorage.Drop(GameTags.MutatedSeed);
					}
					this.seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
					KSelectable component2 = base.GetComponent<KSelectable>();
					if (component2 != null)
					{
						component2.RemoveStatusItem(this.mutantSeedStatusItem, false);
					}
				}
			}
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x002C02F0 File Offset: 0x002BE4F0
		private void UpdateMutantSeedFetches()
		{
			if (this.SpiceFetches != null)
			{
				Tag[] tags = new Tag[]
				{
					GameTags.Seed,
					GameTags.CropSeed
				};
				for (int i = this.SpiceFetches.Length - 1; i >= 0; i--)
				{
					FetchChore fetchChore = this.SpiceFetches[i];
					if (fetchChore != null)
					{
						using (HashSet<Tag>.Enumerator enumerator = this.SpiceFetches[i].tags.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (Assets.GetPrefab(enumerator.Current).HasAnyTags(tags))
								{
									fetchChore.Cancel("MutantSeedChanges");
									this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, fetchChore.amount);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060072A2 RID: 29346 RVA: 0x002C03C0 File Offset: 0x002BE5C0
		private void OnCopySettings(object data)
		{
			SpiceGrinderWorkable component = ((GameObject)data).GetComponent<SpiceGrinderWorkable>();
			if (component != null)
			{
				this.currentSpice = component.Grinder.currentSpice;
				SpiceGrinder.Option spiceOption;
				SpiceGrinder.SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out spiceOption);
				this.OnOptionSelected(spiceOption);
				this.allowMutantSeeds = component.Grinder.AllowMutantSeeds;
			}
		}

		// Token: 0x060072A3 RID: 29347 RVA: 0x002C0428 File Offset: 0x002BE628
		public void SetupFoodSymbol()
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "foodSymbol";
			gameObject.SetActive(false);
			bool flag;
			Vector3 position = this.kbac.GetSymbolTransform(SpiceGrinder.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			this.kbac.SetSymbolVisiblity(SpiceGrinder.StatesInstance.HASH_FOOD, false);
		}

		// Token: 0x060072A4 RID: 29348 RVA: 0x002C04E4 File Offset: 0x002BE6E4
		public void UpdateFoodSymbol()
		{
			bool flag = this.AvailableFood > 0f && this.CurrentFood != null;
			this.foodKBAC.gameObject.SetActive(flag);
			if (flag)
			{
				this.foodKBAC.SwapAnims(this.CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x060072A5 RID: 29349 RVA: 0x002C055D File Offset: 0x002BE75D
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(this.seedStorage.MassStored() / this.seedStorage.capacityKg);
		}

		// Token: 0x060072A6 RID: 29350 RVA: 0x002C0584 File Offset: 0x002BE784
		public void SpiceFood()
		{
			float num = this.CurrentFood.Calories / 1000f;
			this.CurrentFood.SpiceEdible(this.currentSpice, SpiceGrinderConfig.SpicedStatus);
			this.foodStorage.Drop(this.CurrentFood.gameObject, true);
			this.currentFood = null;
			this.UpdateFoodSymbol();
			foreach (Spice.Ingredient ingredient in SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients)
			{
				float num2 = num * ingredient.AmountKG / 1000f;
				int num3 = ingredient.IngredientSet.Length - 1;
				while (num2 > 0f && num3 >= 0)
				{
					Tag tag = ingredient.IngredientSet[num3];
					float num4;
					SimUtil.DiseaseInfo diseaseInfo;
					float num5;
					this.seedStorage.ConsumeAndGetDisease(tag, num2, out num4, out diseaseInfo, out num5);
					num2 -= num4;
					num3--;
				}
			}
			base.sm.isReady.Set(false, this, false);
		}

		// Token: 0x060072A7 RID: 29351 RVA: 0x002C0684 File Offset: 0x002BE884
		public bool CanSpice(float kcalToSpice)
		{
			bool flag = true;
			float num = kcalToSpice / 1000f;
			Spice.Ingredient[] ingredients = SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients;
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num2 = 0f;
				int num3 = 0;
				while (ingredient.IngredientSet != null && num3 < ingredient.IngredientSet.Length)
				{
					num2 += this.seedStorage.GetMassAvailable(ingredient.IngredientSet[num3]);
					num3++;
				}
				float num4 = num * ingredient.AmountKG / 1000f;
				flag &= (num4 <= num2);
				if (num4 > num2)
				{
					dictionary.Add(ingredient.IngredientSet[0], num4 - num2);
					if (this.SpiceFetches != null && this.SpiceFetches[i] == null)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
				}
			}
			this.UpdateSpiceIngredientStatus(flag, dictionary);
			return flag;
		}

		// Token: 0x060072A8 RID: 29352 RVA: 0x002C079F File Offset: 0x002BE99F
		private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
		{
			return this.CreateFetchChore(new HashSet<Tag>(ingredientIngredientSet), amount);
		}

		// Token: 0x060072A9 RID: 29353 RVA: 0x002C07B0 File Offset: 0x002BE9B0
		private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
		{
			float num = Mathf.Max(amount, 1f);
			ChoreType cookFetch = Db.Get().ChoreTypes.CookFetch;
			Storage destination = this.seedStorage;
			float amount2 = num;
			FetchChore.MatchCriteria criteria = FetchChore.MatchCriteria.MatchID;
			Tag invalid = Tag.Invalid;
			Action<Chore> on_complete = new Action<Chore>(this.ClearFetchChore);
			Tag[] forbidden_tags;
			if (!this.AllowMutantSeeds)
			{
				(forbidden_tags = new Tag[1])[0] = GameTags.MutatedSeed;
			}
			else
			{
				forbidden_tags = null;
			}
			return new FetchChore(cookFetch, destination, amount2, ingredients, criteria, invalid, forbidden_tags, null, true, on_complete, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x060072AA RID: 29354 RVA: 0x002C081C File Offset: 0x002BEA1C
		private void ClearFetchChore(Chore obj)
		{
			FetchChore fetchChore = obj as FetchChore;
			if (fetchChore == null || !fetchChore.isComplete || this.SpiceFetches == null)
			{
				return;
			}
			int i = this.SpiceFetches.Length - 1;
			while (i >= 0)
			{
				if (this.SpiceFetches[i] == fetchChore)
				{
					float num = fetchChore.originalAmount - fetchChore.amount;
					if (num > 0f)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, num);
						return;
					}
					this.SpiceFetches[i] = null;
					return;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x060072AB RID: 29355 RVA: 0x002C089C File Offset: 0x002BEA9C
		private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			if (can_spice)
			{
				this.missingResourceStatusItem = component.RemoveStatusItem(this.missingResourceStatusItem, false);
				return;
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = component.ReplaceStatusItem(this.missingResourceStatusItem, Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				return;
			}
			this.missingResourceStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
		}

		// Token: 0x060072AC RID: 29356 RVA: 0x002C0918 File Offset: 0x002BEB18
		public void OnOptionSelected(SpiceGrinder.Option spiceOption)
		{
			base.smi.GetComponent<Operational>().SetFlag(SpiceGrinder.spiceSet, spiceOption != null);
			if (spiceOption == null)
			{
				this.kbac.Play("default", KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.SetSymbolTint("stripe_anim2", Color.white);
			}
			else
			{
				this.kbac.Play(this.IsOperational ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.CancelFetches("SpiceChanged");
			if (this.currentSpice.Id != Tag.Invalid)
			{
				this.seedStorage.DropAll(false, false, default(Vector3), true, null);
				this.UpdateMeter();
				base.sm.isReady.Set(false, this, false);
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.missingResourceStatusItem, false);
			}
			if (spiceOption != null)
			{
				this.currentSpice = new SpiceInstance
				{
					Id = spiceOption.Id,
					TotalKG = spiceOption.Spice.TotalKG
				};
				this.SetSpiceSymbolColours(spiceOption.Spice);
				this.spiceHash = this.currentSpice.Id.GetHash();
				this.seedStorage.capacityKg = this.currentSpice.TotalKG * 10f;
				Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
				this.SpiceFetches = new FetchChore[ingredients.Length];
				Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
				for (int i = 0; i < ingredients.Length; i++)
				{
					Spice.Ingredient ingredient = ingredients[i];
					float num = (this.CurrentFood != null) ? (this.CurrentFood.Calories * ingredient.AmountKG / 1000000f) : 0f;
					if (this.seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < num)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
					if (this.CurrentFood != null)
					{
						dictionary.Add(ingredient.IngredientSet[0], num);
					}
				}
				if (this.CurrentFood != null)
				{
					this.UpdateSpiceIngredientStatus(false, dictionary);
				}
				this.foodFilter[0] = this.currentSpice.Id;
				this.foodStorageFilter.FilterChanged();
			}
		}

		// Token: 0x060072AD RID: 29357 RVA: 0x002C0BA4 File Offset: 0x002BEDA4
		public void CancelFetches(string reason)
		{
			if (this.SpiceFetches != null)
			{
				for (int i = 0; i < this.SpiceFetches.Length; i++)
				{
					if (this.SpiceFetches[i] != null)
					{
						this.SpiceFetches[i].Cancel(reason);
					}
				}
			}
		}

		// Token: 0x060072AE RID: 29358 RVA: 0x002C0BE4 File Offset: 0x002BEDE4
		private void SetSpiceSymbolColours(Spice spice)
		{
			this.kbac.SetSymbolTint("stripe_anim2", spice.PrimaryColor);
			this.kbac.SetSymbolTint("stripe_anim1", spice.SecondaryColor);
			this.kbac.SetSymbolTint("grinder", spice.PrimaryColor);
		}

		// Token: 0x0400565A RID: 22106
		private static string HASH_FOOD = "food";

		// Token: 0x0400565B RID: 22107
		private KBatchedAnimController kbac;

		// Token: 0x0400565C RID: 22108
		private KBatchedAnimController foodKBAC;

		// Token: 0x0400565D RID: 22109
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x0400565E RID: 22110
		[MyCmpReq]
		public SpiceGrinderWorkable workable;

		// Token: 0x0400565F RID: 22111
		[Serialize]
		private int spiceHash;

		// Token: 0x04005660 RID: 22112
		private SpiceInstance currentSpice;

		// Token: 0x04005661 RID: 22113
		private Edible currentFood;

		// Token: 0x04005662 RID: 22114
		private Storage seedStorage;

		// Token: 0x04005663 RID: 22115
		private Storage foodStorage;

		// Token: 0x04005664 RID: 22116
		private MeterController meter;

		// Token: 0x04005665 RID: 22117
		private Tag[] foodFilter = new Tag[1];

		// Token: 0x04005666 RID: 22118
		private FilteredStorage foodStorageFilter;

		// Token: 0x04005667 RID: 22119
		private Operational operational;

		// Token: 0x04005668 RID: 22120
		private Guid missingResourceStatusItem = Guid.Empty;

		// Token: 0x04005669 RID: 22121
		private StatusItem mutantSeedStatusItem;

		// Token: 0x0400566A RID: 22122
		private FetchChore[] SpiceFetches;

		// Token: 0x0400566B RID: 22123
		[Serialize]
		private bool allowMutantSeeds = true;
	}
}
