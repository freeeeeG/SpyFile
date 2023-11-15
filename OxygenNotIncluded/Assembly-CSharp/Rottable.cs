using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>
{
	// Token: 0x06001DF9 RID: 7673 RVA: 0x0009F8FC File Offset: 0x0009DAFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Fresh;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.TagTransition(GameTags.Preserved, this.Preserved, false).TagTransition(GameTags.Entombed, this.Preserved, false);
		this.Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Stale_Pre, (Rottable.Instance smi, float p) => p <= smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, true).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, true);
		this.Preserved.TagTransition(Rottable.PRESERVED_TAGS, this.Fresh, true).Enter("RefreshModifiers", delegate(Rottable.Instance smi)
		{
			smi.RefreshModifiers(0f);
		});
		this.Stale_Pre.Enter(delegate(Rottable.Instance smi)
		{
			smi.GoTo(this.Stale);
		});
		this.Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, (Rottable.Instance smi) => smi).ParamTransition<float>(this.rotParameter, this.Fresh, (Rottable.Instance smi, float p) => p > smi.def.spoilTime - (smi.def.spoilTime - smi.def.staleTime)).ParamTransition<float>(this.rotParameter, this.Spoiled, GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.IsLTEZero).Update(delegate(Rottable.Instance smi, float dt)
		{
			smi.sm.rotParameter.Set(smi.RotValue, smi, false);
		}, UpdateRate.SIM_1000ms, false).FastUpdate("Rot", Rottable.rotCB, UpdateRate.SIM_1000ms, false);
		this.Spoiled.Enter(delegate(Rottable.Instance smi)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile", Grid.SceneLayer.Ore);
			gameObject.gameObject.GetComponent<KSelectable>().SetName(UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());
			gameObject.transform.SetPosition(smi.master.transform.GetPosition());
			gameObject.GetComponent<PrimaryElement>().Mass = smi.master.GetComponent<PrimaryElement>().Mass;
			gameObject.GetComponent<PrimaryElement>().Temperature = smi.master.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ITEMS.FOOD.ROTPILE.NAME, gameObject.transform, 1.5f, false);
			Edible component = smi.GetComponent<Edible>();
			if (component != null)
			{
				if (component.worker != null)
				{
					ChoreDriver component2 = component.worker.GetComponent<ChoreDriver>();
					if (component2 != null && component2.GetCurrentChore() != null)
					{
						component2.GetCurrentChore().Fail("food rotted");
					}
				}
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.ROTTED, "{0}", smi.gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
			}
			Util.KDestroyGameObject(smi.gameObject);
		});
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x0009FB20 File Offset: 0x0009DD20
	private static string OnStaleTooltip(List<Notification> notifications, object data)
	{
		string text = "\n";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				GameObject gameObject = (GameObject)notification.tooltipData;
				if (gameObject != null)
				{
					text = text + "\n" + gameObject.GetProperName();
				}
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, text);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x0009FBAC File Offset: 0x0009DDAC
	public static void SetStatusItems(IRottable rottable)
	{
		Grid.PosToCell(rottable.gameObject);
		KSelectable component = rottable.gameObject.GetComponent<KSelectable>();
		Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(rottable);
		if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
		{
			if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.RefrigeratedFrozen, rottable);
			}
			else
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Unrefrigerated, rottable);
			}
		}
		else
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, Db.Get().CreatureStatusItems.Refrigerated, rottable);
		}
		Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(rottable);
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Sterilizing)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.SterilizingAtmosphere, null);
			return;
		}
		if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.ContaminatedAtmosphere, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, null, null);
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x0009FCCC File Offset: 0x0009DECC
	public static bool IsInActiveFridge(IRottable rottable)
	{
		Pickupable component = rottable.gameObject.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			Refrigerator component2 = component.storage.GetComponent<Refrigerator>();
			return component2 != null && component2.IsActive();
		}
		return false;
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x0009FD1C File Offset: 0x0009DF1C
	public static Rottable.RotRefrigerationLevel RefrigerationLevel(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		Rottable.Instance smi = rottable.gameObject.GetSMI<Rottable.Instance>();
		PrimaryElement component = rottable.gameObject.GetComponent<PrimaryElement>();
		float num2 = component.Temperature;
		bool flag = false;
		if (!Grid.IsValidCell(num))
		{
			if (!smi.IsRottableInSpace())
			{
				return Rottable.RotRefrigerationLevel.Normal;
			}
			flag = true;
		}
		if (!flag && Grid.Element[num].id != SimHashes.Vacuum)
		{
			num2 = Mathf.Min(Grid.Temperature[num], component.Temperature);
		}
		if (num2 < rottable.PreserveTemperature)
		{
			return Rottable.RotRefrigerationLevel.Frozen;
		}
		if (num2 < rottable.RotTemperature || Rottable.IsInActiveFridge(rottable))
		{
			return Rottable.RotRefrigerationLevel.Refrigerated;
		}
		return Rottable.RotRefrigerationLevel.Normal;
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x0009FDBC File Offset: 0x0009DFBC
	public static Rottable.RotAtmosphereQuality AtmosphereQuality(IRottable rottable)
	{
		int num = Grid.PosToCell(rottable.gameObject);
		int num2 = Grid.CellAbove(num);
		if (!Grid.IsValidCell(num))
		{
			if (rottable.gameObject.GetSMI<Rottable.Instance>().IsRottableInSpace())
			{
				return Rottable.RotAtmosphereQuality.Sterilizing;
			}
			return Rottable.RotAtmosphereQuality.Normal;
		}
		else
		{
			SimHashes id = Grid.Element[num].id;
			Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.RotAtmosphereQuality.Normal;
			Rottable.AtmosphereModifier.TryGetValue((int)id, out rotAtmosphereQuality);
			Rottable.RotAtmosphereQuality rotAtmosphereQuality2 = Rottable.RotAtmosphereQuality.Normal;
			if (Grid.IsValidCell(num2))
			{
				SimHashes id2 = Grid.Element[num2].id;
				if (!Rottable.AtmosphereModifier.TryGetValue((int)id2, out rotAtmosphereQuality2))
				{
					rotAtmosphereQuality2 = rotAtmosphereQuality;
				}
			}
			else
			{
				rotAtmosphereQuality2 = rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == rotAtmosphereQuality2)
			{
				return rotAtmosphereQuality;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Contaminating)
			{
				return Rottable.RotAtmosphereQuality.Contaminating;
			}
			if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Normal || rotAtmosphereQuality2 == Rottable.RotAtmosphereQuality.Normal)
			{
				return Rottable.RotAtmosphereQuality.Normal;
			}
			return Rottable.RotAtmosphereQuality.Sterilizing;
		}
	}

	// Token: 0x040010B0 RID: 4272
	public StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.FloatParameter rotParameter;

	// Token: 0x040010B1 RID: 4273
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Preserved;

	// Token: 0x040010B2 RID: 4274
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Fresh;

	// Token: 0x040010B3 RID: 4275
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale_Pre;

	// Token: 0x040010B4 RID: 4276
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale;

	// Token: 0x040010B5 RID: 4277
	public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Spoiled;

	// Token: 0x040010B6 RID: 4278
	private static readonly Tag[] PRESERVED_TAGS = new Tag[]
	{
		GameTags.Preserved,
		GameTags.Entombed
	};

	// Token: 0x040010B7 RID: 4279
	private static readonly Rottable.RotCB rotCB = new Rottable.RotCB();

	// Token: 0x040010B8 RID: 4280
	public static Dictionary<int, Rottable.RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, Rottable.RotAtmosphereQuality>
	{
		{
			721531317,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			1887387588,
			Rottable.RotAtmosphereQuality.Contaminating
		},
		{
			-1528777920,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1836671383,
			Rottable.RotAtmosphereQuality.Normal
		},
		{
			1960575215,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-899515856,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1554872654,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1858722091,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			758759285,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1046145888,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1324664829,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-1406916018,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-432557516,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			-805366663,
			Rottable.RotAtmosphereQuality.Sterilizing
		},
		{
			1966552544,
			Rottable.RotAtmosphereQuality.Sterilizing
		}
	};

	// Token: 0x020011A4 RID: 4516
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005D09 RID: 23817
		public float spoilTime;

		// Token: 0x04005D0A RID: 23818
		public float staleTime;

		// Token: 0x04005D0B RID: 23819
		public float preserveTemperature = 255.15f;

		// Token: 0x04005D0C RID: 23820
		public float rotTemperature = 277.15f;
	}

	// Token: 0x020011A5 RID: 4517
	private class RotCB : UpdateBucketWithUpdater<Rottable.Instance>.IUpdater
	{
		// Token: 0x06007A51 RID: 31313 RVA: 0x002DB751 File Offset: 0x002D9951
		public void Update(Rottable.Instance smi, float dt)
		{
			smi.Rot(smi, dt);
		}
	}

	// Token: 0x020011A6 RID: 4518
	public new class Instance : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.GameInstance, IRottable
	{
		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06007A53 RID: 31315 RVA: 0x002DB763 File Offset: 0x002D9963
		// (set) Token: 0x06007A54 RID: 31316 RVA: 0x002DB770 File Offset: 0x002D9970
		public float RotValue
		{
			get
			{
				return this.rotAmountInstance.value;
			}
			set
			{
				base.sm.rotParameter.Set(value, this, false);
				this.rotAmountInstance.SetValue(value);
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06007A55 RID: 31317 RVA: 0x002DB793 File Offset: 0x002D9993
		public float RotConstitutionPercentage
		{
			get
			{
				return this.RotValue / base.def.spoilTime;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06007A56 RID: 31318 RVA: 0x002DB7A7 File Offset: 0x002D99A7
		public float RotTemperature
		{
			get
			{
				return base.def.rotTemperature;
			}
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06007A57 RID: 31319 RVA: 0x002DB7B4 File Offset: 0x002D99B4
		public float PreserveTemperature
		{
			get
			{
				return base.def.preserveTemperature;
			}
		}

		// Token: 0x06007A58 RID: 31320 RVA: 0x002DB7C4 File Offset: 0x002D99C4
		public Instance(IStateMachineTarget master, Rottable.Def def) : base(master, def)
		{
			this.pickupable = base.gameObject.RequireComponent<Pickupable>();
			base.master.Subscribe(-2064133523, new Action<object>(this.OnAbsorb));
			base.master.Subscribe(1335436905, new Action<object>(this.OnSplitFromChunk));
			this.primaryElement = base.gameObject.GetComponent<PrimaryElement>();
			Amounts amounts = master.gameObject.GetAmounts();
			this.rotAmountInstance = amounts.Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
			this.rotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime, null, false, false, true));
			this.rotAmountInstance.SetValue(def.spoilTime);
			base.sm.rotParameter.Set(this.rotAmountInstance.value, base.smi, false);
			if (Rottable.Instance.unrefrigeratedModifier == null)
			{
				Rottable.Instance.unrefrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.7f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.UNREFRIGERATED, false, false, true);
				Rottable.Instance.refrigeratedModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.2f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.REFRIGERATED, false, false, true);
				Rottable.Instance.frozenModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTTEMPERATURE.FROZEN, false, false, true);
				Rottable.Instance.contaminatedAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -1f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.CONTAMINATED, false, false, true);
				Rottable.Instance.normalAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, -0.3f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NORMAL, false, false, true);
				Rottable.Instance.sterileAtmosphereModifier = new AttributeModifier(this.rotAmountInstance.amount.Id, --0f, DUPLICANTS.MODIFIERS.ROTATMOSPHERE.STERILE, false, false, true);
			}
			this.RefreshModifiers(0f);
		}

		// Token: 0x06007A59 RID: 31321 RVA: 0x002DB9E0 File Offset: 0x002D9BE0
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 23))
			{
				this.rotAmountInstance.SetValue(this.rotAmountInstance.value * 2f);
			}
		}

		// Token: 0x06007A5A RID: 31322 RVA: 0x002DBA24 File Offset: 0x002D9C24
		public string StateString()
		{
			string result = "";
			if (base.smi.GetCurrentState() == base.sm.Fresh)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.FRESH.NAME, this);
			}
			if (base.smi.GetCurrentState() == base.sm.Stale)
			{
				result = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback(CREATURES.STATUSITEMS.STALE.NAME, this);
			}
			return result;
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x002DBAB2 File Offset: 0x002D9CB2
		public void Rot(Rottable.Instance smi, float deltaTime)
		{
			this.RefreshModifiers(deltaTime);
			if (smi.pickupable.storage != null)
			{
				smi.pickupable.storage.Trigger(-1197125120, null);
			}
		}

		// Token: 0x06007A5C RID: 31324 RVA: 0x002DBAE4 File Offset: 0x002D9CE4
		public bool IsRottableInSpace()
		{
			if (base.gameObject.GetMyWorld() == null)
			{
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null && component.storage && (component.storage.GetComponent<RocketModuleCluster>() || component.storage.GetComponent<ClusterTraveler>()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007A5D RID: 31325 RVA: 0x002DBB48 File Offset: 0x002D9D48
		public void RefreshModifiers(float dt)
		{
			if (this.GetMaster().isNull)
			{
				return;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(base.gameObject)) && !this.IsRottableInSpace())
			{
				return;
			}
			this.rotAmountInstance.deltaAttribute.ClearModifiers();
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (!component.HasAnyTags(Rottable.PRESERVED_TAGS))
			{
				Rottable.RotRefrigerationLevel rotRefrigerationLevel = Rottable.RefrigerationLevel(this);
				if (rotRefrigerationLevel != Rottable.RotRefrigerationLevel.Refrigerated)
				{
					if (rotRefrigerationLevel == Rottable.RotRefrigerationLevel.Frozen)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.frozenModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.unrefrigeratedModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.refrigeratedModifier);
				}
				Rottable.RotAtmosphereQuality rotAtmosphereQuality = Rottable.AtmosphereQuality(this);
				if (rotAtmosphereQuality != Rottable.RotAtmosphereQuality.Sterilizing)
				{
					if (rotAtmosphereQuality == Rottable.RotAtmosphereQuality.Contaminating)
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.contaminatedAtmosphereModifier);
					}
					else
					{
						this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.normalAtmosphereModifier);
					}
				}
				else
				{
					this.rotAmountInstance.deltaAttribute.Add(Rottable.Instance.sterileAtmosphereModifier);
				}
			}
			if (component.HasTag(Db.Get().Spices.PreservingSpice.Id))
			{
				this.rotAmountInstance.deltaAttribute.Add(Db.Get().Spices.PreservingSpice.FoodModifier);
			}
			Rottable.SetStatusItems(this);
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x002DBC94 File Offset: 0x002D9E94
		private void OnAbsorb(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
				PrimaryElement primaryElement = pickupable.PrimaryElement;
				Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
				if (component != null && primaryElement != null && smi != null)
				{
					float num = component.Units * base.sm.rotParameter.Get(base.smi);
					float num2 = primaryElement.Units * base.sm.rotParameter.Get(smi);
					float value = (num + num2) / (component.Units + primaryElement.Units);
					base.sm.rotParameter.Set(value, base.smi, false);
				}
			}
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x002DBD4C File Offset: 0x002D9F4C
		public bool IsRotLevelStackable(Rottable.Instance other)
		{
			return Mathf.Abs(this.RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.1f;
		}

		// Token: 0x06007A60 RID: 31328 RVA: 0x002DBD67 File Offset: 0x002D9F67
		public string GetToolTip()
		{
			return this.rotAmountInstance.GetTooltip();
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x002DBD74 File Offset: 0x002D9F74
		private void OnSplitFromChunk(object data)
		{
			Pickupable pickupable = (Pickupable)data;
			if (pickupable != null)
			{
				Rottable.Instance smi = pickupable.GetSMI<Rottable.Instance>();
				if (smi != null)
				{
					this.RotValue = smi.RotValue;
				}
			}
		}

		// Token: 0x06007A62 RID: 31330 RVA: 0x002DBDA7 File Offset: 0x002D9FA7
		public void OnPreserved(object data)
		{
			if ((bool)data)
			{
				base.smi.GoTo(base.sm.Preserved);
				return;
			}
			base.smi.GoTo(base.sm.Fresh);
		}

		// Token: 0x04005D0D RID: 23821
		private AmountInstance rotAmountInstance;

		// Token: 0x04005D0E RID: 23822
		private static AttributeModifier unrefrigeratedModifier;

		// Token: 0x04005D0F RID: 23823
		private static AttributeModifier refrigeratedModifier;

		// Token: 0x04005D10 RID: 23824
		private static AttributeModifier frozenModifier;

		// Token: 0x04005D11 RID: 23825
		private static AttributeModifier contaminatedAtmosphereModifier;

		// Token: 0x04005D12 RID: 23826
		private static AttributeModifier normalAtmosphereModifier;

		// Token: 0x04005D13 RID: 23827
		private static AttributeModifier sterileAtmosphereModifier;

		// Token: 0x04005D14 RID: 23828
		public PrimaryElement primaryElement;

		// Token: 0x04005D15 RID: 23829
		public Pickupable pickupable;
	}

	// Token: 0x020011A7 RID: 4519
	public enum RotAtmosphereQuality
	{
		// Token: 0x04005D17 RID: 23831
		Normal,
		// Token: 0x04005D18 RID: 23832
		Sterilizing,
		// Token: 0x04005D19 RID: 23833
		Contaminating
	}

	// Token: 0x020011A8 RID: 4520
	public enum RotRefrigerationLevel
	{
		// Token: 0x04005D1B RID: 23835
		Normal,
		// Token: 0x04005D1C RID: 23836
		Refrigerated,
		// Token: 0x04005D1D RID: 23837
		Frozen
	}
}
