using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004AB RID: 1195
[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor, ISaveLoadable, IExtendSplitting
{
	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06001B13 RID: 6931 RVA: 0x00091355 File Offset: 0x0008F555
	// (set) Token: 0x06001B14 RID: 6932 RVA: 0x00091362 File Offset: 0x0008F562
	public float Units
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			this.primaryElement.Units = value;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06001B15 RID: 6933 RVA: 0x00091370 File Offset: 0x0008F570
	public float MassPerUnit
	{
		get
		{
			return this.primaryElement.MassPerUnit;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06001B16 RID: 6934 RVA: 0x0009137D File Offset: 0x0008F57D
	// (set) Token: 0x06001B17 RID: 6935 RVA: 0x00091391 File Offset: 0x0008F591
	public float Calories
	{
		get
		{
			return this.Units * this.foodInfo.CaloriesPerUnit;
		}
		set
		{
			this.Units = value / this.foodInfo.CaloriesPerUnit;
		}
	}

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x06001B18 RID: 6936 RVA: 0x000913A6 File Offset: 0x0008F5A6
	// (set) Token: 0x06001B19 RID: 6937 RVA: 0x000913AE File Offset: 0x0008F5AE
	public EdiblesManager.FoodInfo FoodInfo
	{
		get
		{
			return this.foodInfo;
		}
		set
		{
			this.foodInfo = value;
			this.FoodID = this.foodInfo.Id;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x06001B1A RID: 6938 RVA: 0x000913C8 File Offset: 0x0008F5C8
	// (set) Token: 0x06001B1B RID: 6939 RVA: 0x000913D0 File Offset: 0x0008F5D0
	public bool isBeingConsumed { get; private set; }

	// Token: 0x06001B1C RID: 6940 RVA: 0x000913DC File Offset: 0x0008F5DC
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
		base.OnPrefabInit();
		if (this.foodInfo == null)
		{
			if (this.FoodID == null)
			{
				global::Debug.LogError("No food FoodID");
			}
			this.foodInfo = EdiblesManager.GetFoodInfo(this.FoodID);
		}
		base.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
		base.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
		this.synchronizeAnims = false;
		Components.Edibles.Add(this);
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x00091490 File Offset: 0x0008F690
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ToggleGenericSpicedTag(base.gameObject.HasTag(GameTags.SpicedFood));
		if (this.spices != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				this.ApplySpiceEffects(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, this);
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x0009151C File Offset: 0x0008F71C
	public override HashedString[] GetWorkAnims(Worker worker)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkAnims;
			}
			return Edible.saltHatWorkAnims;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkAnims;
			}
			return Edible.saltWorkAnims;
		}
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x00091574 File Offset: 0x0008F774
	public override HashedString[] GetWorkPstAnims(Worker worker, bool successfully_completed)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkPstAnim;
			}
			return Edible.saltHatWorkPstAnim;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkPstAnim;
			}
			return Edible.saltWorkPstAnim;
		}
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x000915CA File Offset: 0x0008F7CA
	private void OnCraft(object data)
	{
		RationTracker.Get().RegisterCaloriesProduced(this.Calories);
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x000915DC File Offset: 0x0008F7DC
	public float GetFeedingTime(Worker worker)
	{
		float num = this.Calories * 2E-05f;
		if (worker != null)
		{
			BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
			if (smi != null && smi.IsBingeEating())
			{
				num /= 2f;
			}
		}
		return num;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x0009161C File Offset: 0x0008F81C
	protected override void OnStartWork(Worker worker)
	{
		this.totalFeedingTime = this.GetFeedingTime(worker);
		base.SetWorkTime(this.totalFeedingTime);
		this.caloriesConsumed = 0f;
		this.unitsConsumed = 0f;
		this.totalUnits = this.Units;
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		this.totalConsumableCalories = this.Units * this.foodInfo.CaloriesPerUnit;
		this.StartConsuming();
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x00091694 File Offset: 0x0008F894
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.currentlyLit)
		{
			if (this.currentModifier != this.caloriesLitSpaceModifier)
			{
				worker.GetAttributes().Remove(this.currentModifier);
				worker.GetAttributes().Add(this.caloriesLitSpaceModifier);
				this.currentModifier = this.caloriesLitSpaceModifier;
			}
		}
		else if (this.currentModifier != this.caloriesModifier)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			worker.GetAttributes().Add(this.caloriesModifier);
			this.currentModifier = this.caloriesModifier;
		}
		return this.OnTickConsume(worker, dt);
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x0009172B File Offset: 0x0008F92B
	protected override void OnStopWork(Worker worker)
	{
		if (this.currentModifier != null)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			this.currentModifier = null;
		}
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		this.StopConsuming(worker);
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x00091764 File Offset: 0x0008F964
	private bool OnTickConsume(Worker worker, float dt)
	{
		if (!this.isBeingConsumed)
		{
			DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
			return true;
		}
		bool result = false;
		float num = dt / this.totalFeedingTime;
		float num2 = num * this.totalConsumableCalories;
		if (this.caloriesConsumed + num2 > this.totalConsumableCalories)
		{
			num2 = this.totalConsumableCalories - this.caloriesConsumed;
		}
		this.caloriesConsumed += num2;
		worker.GetAmounts().Get("Calories").value += num2;
		float num3 = this.totalUnits * num;
		if (this.Units - num3 < 0f)
		{
			num3 = this.Units;
		}
		this.Units -= num3;
		this.unitsConsumed += num3;
		if (this.Units <= 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0009182D File Offset: 0x0008FA2D
	public void SpiceEdible(SpiceInstance spice, StatusItem status)
	{
		this.spices.Add(spice);
		this.ApplySpiceEffects(spice, status);
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x00091844 File Offset: 0x0008FA44
	protected virtual void ApplySpiceEffects(SpiceInstance spice, StatusItem status)
	{
		base.GetComponent<KPrefabID>().AddTag(spice.Id, true);
		this.ToggleGenericSpicedTag(true);
		base.GetComponent<KSelectable>().AddStatusItem(status, this.spices);
		if (spice.FoodModifier != null)
		{
			base.gameObject.GetAttributes().Add(spice.FoodModifier);
		}
		if (spice.CalorieModifier != null)
		{
			this.Calories += spice.CalorieModifier.Value;
		}
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000918C0 File Offset: 0x0008FAC0
	private void ToggleGenericSpicedTag(bool isSpiced)
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (isSpiced)
		{
			component.RemoveTag(GameTags.UnspicedFood);
			component.AddTag(GameTags.SpicedFood, true);
			return;
		}
		component.RemoveTag(GameTags.SpicedFood);
		component.AddTag(GameTags.UnspicedFood, false);
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x00091908 File Offset: 0x0008FB08
	public bool CanAbsorb(Edible other)
	{
		bool flag = this.spices.Count == other.spices.Count;
		int num = 0;
		while (flag && num < this.spices.Count)
		{
			int num2 = 0;
			while (flag && num2 < other.spices.Count)
			{
				flag = (this.spices[num].Id == other.spices[num2].Id);
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x00091989 File Offset: 0x0008FB89
	private void StartConsuming()
	{
		DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started", null);
		this.isBeingConsumed = true;
		base.worker.Trigger(1406130139, this);
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000919B8 File Offset: 0x0008FBB8
	private void StopConsuming(Worker worker)
	{
		DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()", null);
		this.isBeingConsumed = false;
		if (this.primaryElement != null && this.primaryElement.DiseaseCount > 0)
		{
			new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.FoodPoisoning, 1, null);
		}
		for (int i = 0; i < this.foodInfo.Effects.Count; i++)
		{
			worker.GetComponent<Effects>().Add(this.foodInfo.Effects[i], true);
		}
		ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
		this.AddOnConsumeEffects(worker);
		worker.Trigger(1121894420, this);
		base.Trigger(-10536414, worker.gameObject);
		this.unitsConsumed = float.NaN;
		this.caloriesConsumed = float.NaN;
		this.totalUnits = float.NaN;
		if (this.Units <= 0f)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x00091AF2 File Offset: 0x0008FCF2
	public static string GetEffectForFoodQuality(int qualityLevel)
	{
		qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
		return Edible.qualityEffects[qualityLevel];
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x00091B0C File Offset: 0x0008FD0C
	private void AddOnConsumeEffects(Worker worker)
	{
		int num = Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
		int qualityLevel = this.FoodInfo.Quality + num;
		Effects component = worker.GetComponent<Effects>();
		component.Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
		for (int i = 0; i < this.spices.Count; i++)
		{
			Effect statBonus = this.spices[i].StatBonus;
			if (statBonus != null)
			{
				float duration = statBonus.duration;
				statBonus.duration = this.caloriesConsumed * 0.001f / 1000f * 600f;
				component.Add(statBonus, true);
				statBonus.duration = duration;
			}
		}
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x00091BCD File Offset: 0x0008FDCD
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Edibles.Remove(this);
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x00091BE0 File Offset: 0x0008FDE0
	public int GetQuality()
	{
		return this.foodInfo.Quality;
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x00091BF0 File Offset: 0x0008FDF0
	public int GetMorale()
	{
		int num = 0;
		string effectForFoodQuality = Edible.GetEffectForFoodQuality(this.foodInfo.Quality);
		foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
		{
			if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
			{
				num += Mathf.RoundToInt(attributeModifier.Value);
			}
		}
		return num;
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x00091C90 File Offset: 0x0008FE90
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Information, false));
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), Descriptor.DescriptorType.Effect, false));
		int morale = this.GetMorale();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), Descriptor.DescriptorType.Effect, false));
		foreach (string text in this.foodInfo.Effects)
		{
			string text2 = "";
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(text).SelfModifiers)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					"\n    • ",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
					": ",
					attributeModifier.GetFormattedString()
				});
			}
			list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION") + text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x00091EEC File Offset: 0x000900EC
	public void OnSplitTick(Pickupable thePieceTaken)
	{
		Edible component = thePieceTaken.GetComponent<Edible>();
		if (this.spices != null && component != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				SpiceInstance spice = this.spices[i];
				component.SpiceEdible(spice, SpiceGrinderConfig.SpicedStatus);
			}
		}
	}

	// Token: 0x04000F0A RID: 3850
	private PrimaryElement primaryElement;

	// Token: 0x04000F0B RID: 3851
	public string FoodID;

	// Token: 0x04000F0C RID: 3852
	private EdiblesManager.FoodInfo foodInfo;

	// Token: 0x04000F0E RID: 3854
	public float unitsConsumed = float.NaN;

	// Token: 0x04000F0F RID: 3855
	public float caloriesConsumed = float.NaN;

	// Token: 0x04000F10 RID: 3856
	private float totalFeedingTime = float.NaN;

	// Token: 0x04000F11 RID: 3857
	private float totalUnits = float.NaN;

	// Token: 0x04000F12 RID: 3858
	private float totalConsumableCalories = float.NaN;

	// Token: 0x04000F13 RID: 3859
	[Serialize]
	private List<SpiceInstance> spices = new List<SpiceInstance>();

	// Token: 0x04000F14 RID: 3860
	private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x04000F15 RID: 3861
	private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", 57500f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x04000F16 RID: 3862
	private AttributeModifier currentModifier;

	// Token: 0x04000F17 RID: 3863
	private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		component.OnCraft(data);
	});

	// Token: 0x04000F18 RID: 3864
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04000F19 RID: 3865
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04000F1A RID: 3866
	private static readonly HashedString[] saltWorkAnims = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04000F1B RID: 3867
	private static readonly HashedString[] saltHatWorkAnims = new HashedString[]
	{
		"salt_hat_pre",
		"salt_hat_loop"
	};

	// Token: 0x04000F1C RID: 3868
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04000F1D RID: 3869
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};

	// Token: 0x04000F1E RID: 3870
	private static readonly HashedString[] saltWorkPstAnim = new HashedString[]
	{
		"salt_pst"
	};

	// Token: 0x04000F1F RID: 3871
	private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[]
	{
		"salt_hat_pst"
	};

	// Token: 0x04000F20 RID: 3872
	private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>
	{
		{
			-1,
			"EdibleMinus3"
		},
		{
			0,
			"EdibleMinus2"
		},
		{
			1,
			"EdibleMinus1"
		},
		{
			2,
			"Edible0"
		},
		{
			3,
			"Edible1"
		},
		{
			4,
			"Edible2"
		},
		{
			5,
			"Edible3"
		}
	};

	// Token: 0x0200115E RID: 4446
	public class EdibleStartWorkInfo : Worker.StartWorkInfo
	{
		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06007940 RID: 31040 RVA: 0x002D8872 File Offset: 0x002D6A72
		// (set) Token: 0x06007941 RID: 31041 RVA: 0x002D887A File Offset: 0x002D6A7A
		public float amount { get; private set; }

		// Token: 0x06007942 RID: 31042 RVA: 0x002D8883 File Offset: 0x002D6A83
		public EdibleStartWorkInfo(Workable workable, float amount) : base(workable)
		{
			this.amount = amount;
		}
	}
}
