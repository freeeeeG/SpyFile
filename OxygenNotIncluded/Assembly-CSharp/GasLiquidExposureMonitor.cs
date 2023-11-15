using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200087C RID: 2172
public class GasLiquidExposureMonitor : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>
{
	// Token: 0x06003F55 RID: 16213 RVA: 0x00161B60 File Offset: 0x0015FD60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		this.root.Update(new Action<GasLiquidExposureMonitor.Instance, float>(this.UpdateExposure), UpdateRate.SIM_33ms, false);
		this.normal.ParamTransition<bool>(this.isIrritated, this.irritated, (GasLiquidExposureMonitor.Instance smi, bool p) => this.isIrritated.Get(smi));
		this.irritated.ParamTransition<bool>(this.isIrritated, this.normal, (GasLiquidExposureMonitor.Instance smi, bool p) => !this.isIrritated.Get(smi)).ToggleStatusItem(Db.Get().DuplicantStatusItems.GasLiquidIrritation, (GasLiquidExposureMonitor.Instance smi) => smi).DefaultState(this.irritated.irritated);
		this.irritated.irritated.Transition(this.irritated.rubbingEyes, new StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Transition.ConditionCallback(GasLiquidExposureMonitor.CanReact), UpdateRate.SIM_200ms);
		this.irritated.rubbingEyes.Exit(delegate(GasLiquidExposureMonitor.Instance smi)
		{
			smi.lastReactTime = GameClock.Instance.GetTime();
		}).ToggleReactable((GasLiquidExposureMonitor.Instance smi) => smi.GetReactable()).OnSignal(this.reactFinished, this.irritated.irritated);
	}

	// Token: 0x06003F56 RID: 16214 RVA: 0x00161CAD File Offset: 0x0015FEAD
	private static bool CanReact(GasLiquidExposureMonitor.Instance smi)
	{
		return GameClock.Instance.GetTime() > smi.lastReactTime + 60f;
	}

	// Token: 0x06003F57 RID: 16215 RVA: 0x00161CC8 File Offset: 0x0015FEC8
	private static void InitializeCustomRates()
	{
		if (GasLiquidExposureMonitor.customExposureRates != null)
		{
			return;
		}
		GasLiquidExposureMonitor.minorIrritationEffect = Db.Get().effects.Get("MinorIrritation");
		GasLiquidExposureMonitor.majorIrritationEffect = Db.Get().effects.Get("MajorIrritation");
		GasLiquidExposureMonitor.customExposureRates = new Dictionary<SimHashes, float>();
		float value = -1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Water] = value;
		float value2 = -0.25f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CarbonDioxide] = value2;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen] = value2;
		float value3 = 0f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ContaminatedOxygen] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.DirtyWater] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ViscoGel] = value3;
		float value4 = 0.5f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Hydrogen] = value4;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SaltWater] = value4;
		float value5 = 1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ChlorineGas] = value5;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.EthanolGas] = value5;
		float value6 = 3f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Chlorine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SourGas] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Brine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Ethanol] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SuperCoolant] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CrudeOil] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Naphtha] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Petroleum] = value6;
	}

	// Token: 0x06003F58 RID: 16216 RVA: 0x00161E6C File Offset: 0x0016006C
	public float GetCurrentExposure(GasLiquidExposureMonitor.Instance smi)
	{
		float result;
		if (GasLiquidExposureMonitor.customExposureRates.TryGetValue(smi.CurrentlyExposedToElement().id, out result))
		{
			return result;
		}
		return 0f;
	}

	// Token: 0x06003F59 RID: 16217 RVA: 0x00161E9C File Offset: 0x0016009C
	private void UpdateExposure(GasLiquidExposureMonitor.Instance smi, float dt)
	{
		GasLiquidExposureMonitor.InitializeCustomRates();
		float exposureRate = 0f;
		smi.isInAirtightEnvironment = false;
		smi.isImmuneToIrritability = false;
		int num = Grid.CellAbove(Grid.PosToCell(smi.gameObject));
		if (Grid.IsValidCell(num))
		{
			Element element = Grid.Element[num];
			float num2;
			if (!GasLiquidExposureMonitor.customExposureRates.TryGetValue(element.id, out num2))
			{
				if (Grid.Temperature[num] >= -13657.5f && Grid.Temperature[num] <= 27315f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 2f;
				}
			}
			if (smi.effects.HasImmunityTo(GasLiquidExposureMonitor.minorIrritationEffect) || smi.effects.HasImmunityTo(GasLiquidExposureMonitor.majorIrritationEffect))
			{
				smi.isImmuneToIrritability = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if ((smi.master.gameObject.HasTag(GameTags.HasSuitTank) && smi.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit()) || smi.master.gameObject.HasTag(GameTags.InTransitTube))
			{
				smi.isInAirtightEnvironment = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if (!smi.isInAirtightEnvironment && !smi.isImmuneToIrritability)
			{
				if (element.IsGas)
				{
					exposureRate = num2 * Grid.Mass[num] / 1f;
				}
				else if (element.IsLiquid)
				{
					exposureRate = num2 * Grid.Mass[num] / 1000f;
				}
			}
		}
		smi.exposureRate = exposureRate;
		smi.exposure += smi.exposureRate * dt;
		smi.exposure = MathUtil.Clamp(0f, 30f, smi.exposure);
		this.ApplyEffects(smi);
	}

	// Token: 0x06003F5A RID: 16218 RVA: 0x0016204C File Offset: 0x0016024C
	private void ApplyEffects(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.minorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else if (smi.IsMajorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.majorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else
		{
			smi.effects.Remove(GasLiquidExposureMonitor.minorIrritationEffect);
			smi.effects.Remove(GasLiquidExposureMonitor.majorIrritationEffect);
			this.isIrritated.Set(false, smi, false);
		}
	}

	// Token: 0x06003F5B RID: 16219 RVA: 0x001620DE File Offset: 0x001602DE
	public Effect GetAppliedEffect(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			return GasLiquidExposureMonitor.minorIrritationEffect;
		}
		if (smi.IsMajorIrritation())
		{
			return GasLiquidExposureMonitor.majorIrritationEffect;
		}
		return null;
	}

	// Token: 0x04002911 RID: 10513
	public const float MIN_REACT_INTERVAL = 60f;

	// Token: 0x04002912 RID: 10514
	private static Dictionary<SimHashes, float> customExposureRates;

	// Token: 0x04002913 RID: 10515
	private static Effect minorIrritationEffect;

	// Token: 0x04002914 RID: 10516
	private static Effect majorIrritationEffect;

	// Token: 0x04002915 RID: 10517
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.BoolParameter isIrritated;

	// Token: 0x04002916 RID: 10518
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Signal reactFinished;

	// Token: 0x04002917 RID: 10519
	public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State normal;

	// Token: 0x04002918 RID: 10520
	public GasLiquidExposureMonitor.IrritatedStates irritated;

	// Token: 0x02001679 RID: 5753
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200167A RID: 5754
	public class TUNING
	{
		// Token: 0x04006BCA RID: 27594
		public const float MINOR_IRRITATION_THRESHOLD = 8f;

		// Token: 0x04006BCB RID: 27595
		public const float MAJOR_IRRITATION_THRESHOLD = 15f;

		// Token: 0x04006BCC RID: 27596
		public const float MAX_EXPOSURE = 30f;

		// Token: 0x04006BCD RID: 27597
		public const float GAS_UNITS = 1f;

		// Token: 0x04006BCE RID: 27598
		public const float LIQUID_UNITS = 1000f;

		// Token: 0x04006BCF RID: 27599
		public const float REDUCE_EXPOSURE_RATE_FAST = -1f;

		// Token: 0x04006BD0 RID: 27600
		public const float REDUCE_EXPOSURE_RATE_SLOW = -0.25f;

		// Token: 0x04006BD1 RID: 27601
		public const float NO_CHANGE = 0f;

		// Token: 0x04006BD2 RID: 27602
		public const float SLOW_EXPOSURE_RATE = 0.5f;

		// Token: 0x04006BD3 RID: 27603
		public const float NORMAL_EXPOSURE_RATE = 1f;

		// Token: 0x04006BD4 RID: 27604
		public const float QUICK_EXPOSURE_RATE = 3f;

		// Token: 0x04006BD5 RID: 27605
		public const float DEFAULT_MIN_TEMPERATURE = -13657.5f;

		// Token: 0x04006BD6 RID: 27606
		public const float DEFAULT_MAX_TEMPERATURE = 27315f;

		// Token: 0x04006BD7 RID: 27607
		public const float DEFAULT_LOW_RATE = 1f;

		// Token: 0x04006BD8 RID: 27608
		public const float DEFAULT_HIGH_RATE = 2f;
	}

	// Token: 0x0200167B RID: 5755
	public class IrritatedStates : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State
	{
		// Token: 0x04006BD9 RID: 27609
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State irritated;

		// Token: 0x04006BDA RID: 27610
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State rubbingEyes;
	}

	// Token: 0x0200167C RID: 5756
	public new class Instance : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.GameInstance
	{
		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06008AFE RID: 35582 RVA: 0x003150BB File Offset: 0x003132BB
		public float minorIrritationThreshold
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x06008AFF RID: 35583 RVA: 0x003150C2 File Offset: 0x003132C2
		public Instance(IStateMachineTarget master, GasLiquidExposureMonitor.Def def) : base(master, def)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x06008B00 RID: 35584 RVA: 0x003150D8 File Offset: 0x003132D8
		public Reactable GetReactable()
		{
			Emote iritatedEyes = Db.Get().Emotes.Minion.IritatedEyes;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "IrritatedEyes", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(iritatedEyes);
			selfEmoteReactable.preventChoreInterruption = true;
			selfEmoteReactable.RegisterEmoteStepCallbacks("irritated_eyes", null, delegate(GameObject go)
			{
				base.sm.reactFinished.Trigger(this);
			});
			return selfEmoteReactable;
		}

		// Token: 0x06008B01 RID: 35585 RVA: 0x00315164 File Offset: 0x00313364
		public bool IsMinorIrritation()
		{
			return this.exposure >= 8f && this.exposure < 15f;
		}

		// Token: 0x06008B02 RID: 35586 RVA: 0x00315182 File Offset: 0x00313382
		public bool IsMajorIrritation()
		{
			return this.exposure >= 15f;
		}

		// Token: 0x06008B03 RID: 35587 RVA: 0x00315194 File Offset: 0x00313394
		public Element CurrentlyExposedToElement()
		{
			if (this.isInAirtightEnvironment)
			{
				return ElementLoader.GetElement(SimHashes.Oxygen.CreateTag());
			}
			int num = Grid.CellAbove(Grid.PosToCell(base.smi.gameObject));
			return Grid.Element[num];
		}

		// Token: 0x06008B04 RID: 35588 RVA: 0x003151D6 File Offset: 0x003133D6
		public void ResetExposure()
		{
			this.exposure = 0f;
		}

		// Token: 0x04006BDB RID: 27611
		[Serialize]
		public float exposure;

		// Token: 0x04006BDC RID: 27612
		[Serialize]
		public float lastReactTime;

		// Token: 0x04006BDD RID: 27613
		[Serialize]
		public float exposureRate;

		// Token: 0x04006BDE RID: 27614
		public Effects effects;

		// Token: 0x04006BDF RID: 27615
		public bool isInAirtightEnvironment;

		// Token: 0x04006BE0 RID: 27616
		public bool isImmuneToIrritability;
	}
}
