using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
public class SuffocationMonitor : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance>
{
	// Token: 0x060038A8 RID: 14504 RVA: 0x0013AE40 File Offset: 0x00139040
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Update("CheckOverPressure", delegate(SuffocationMonitor.Instance smi, float dt)
		{
			smi.CheckOverPressure();
		}, UpdateRate.SIM_200ms, false).TagTransition(GameTags.Dead, this.dead, false);
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuffocationMonitor.Instance smi) => smi.breathing, null).EventTransition(GameHashes.ExitedBreathableArea, this.nooxygen, (SuffocationMonitor.Instance smi) => !smi.IsInBreathableArea());
		this.satisfied.normal.Transition(this.satisfied.low, (SuffocationMonitor.Instance smi) => smi.oxygenBreather.IsLowOxygenAtMouthCell(), UpdateRate.SIM_200ms);
		this.satisfied.low.Transition(this.satisfied.normal, (SuffocationMonitor.Instance smi) => !smi.oxygenBreather.IsLowOxygenAtMouthCell(), UpdateRate.SIM_200ms).Transition(this.nooxygen, (SuffocationMonitor.Instance smi) => !smi.IsInBreathableArea(), UpdateRate.SIM_200ms).ToggleEffect("LowOxygen");
		this.nooxygen.EventTransition(GameHashes.EnteredBreathableArea, this.satisfied, (SuffocationMonitor.Instance smi) => smi.IsInBreathableArea()).TagTransition(GameTags.RecoveringBreath, this.satisfied, false).ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.nooxygen.holdingbreath);
		this.nooxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.nooxygen.suffocating, (SuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.nooxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
		this.dead.DoNothing();
	}

	// Token: 0x0400258E RID: 9614
	public SuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x0400258F RID: 9615
	public SuffocationMonitor.NoOxygenState nooxygen;

	// Token: 0x04002590 RID: 9616
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

	// Token: 0x04002591 RID: 9617
	public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x02001584 RID: 5508
	public class NoOxygenState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040068C9 RID: 26825
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;

		// Token: 0x040068CA RID: 26826
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
	}

	// Token: 0x02001585 RID: 5509
	public class SatisfiedState : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040068CB RID: 26827
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x040068CC RID: 26828
		public GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
	}

	// Token: 0x02001586 RID: 5510
	public new class Instance : GameStateMachine<SuffocationMonitor, SuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x060087D5 RID: 34773 RVA: 0x0030CAF5 File Offset: 0x0030ACF5
		// (set) Token: 0x060087D6 RID: 34774 RVA: 0x0030CAFD File Offset: 0x0030ACFD
		public OxygenBreather oxygenBreather { get; private set; }

		// Token: 0x060087D7 RID: 34775 RVA: 0x0030CB08 File Offset: 0x0030AD08
		public Instance(OxygenBreather oxygen_breather) : base(oxygen_breather)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(base.master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float num = 0.90909094f;
			this.breathing = new AttributeModifier(deltaAttribute.Id, num, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -num, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.oxygenBreather = oxygen_breather;
		}

		// Token: 0x060087D8 RID: 34776 RVA: 0x0030CBA4 File Offset: 0x0030ADA4
		public bool IsInBreathableArea()
		{
			return base.master.GetComponent<KPrefabID>().HasTag(GameTags.RecoveringBreath) || base.master.GetComponent<Sensors>().GetSensor<BreathableAreaSensor>().IsBreathable() || this.oxygenBreather.HasTag(GameTags.InTransitTube);
		}

		// Token: 0x060087D9 RID: 34777 RVA: 0x0030CBF1 File Offset: 0x0030ADF1
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x060087DA RID: 34778 RVA: 0x0030CC08 File Offset: 0x0030AE08
		public bool IsSuffocating()
		{
			return this.breath.deltaAttribute.GetTotalValue() <= 0f && this.breath.value <= 45.454548f;
		}

		// Token: 0x060087DB RID: 34779 RVA: 0x0030CC38 File Offset: 0x0030AE38
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x060087DC RID: 34780 RVA: 0x0030CC5C File Offset: 0x0030AE5C
		public void CheckOverPressure()
		{
			if (this.IsInHighPressure())
			{
				if (!this.wasInHighPressure)
				{
					this.wasInHighPressure = true;
					this.highPressureTime = Time.time;
					return;
				}
				if (Time.time - this.highPressureTime > 3f)
				{
					base.master.GetComponent<Effects>().Add("PoppedEarDrums", true);
					return;
				}
			}
			else
			{
				this.wasInHighPressure = false;
			}
		}

		// Token: 0x060087DD RID: 34781 RVA: 0x0030CCC0 File Offset: 0x0030AEC0
		private bool IsInHighPressure()
		{
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < SuffocationMonitor.Instance.pressureTestOffsets.Length; i++)
			{
				int num = Grid.OffsetCell(cell, SuffocationMonitor.Instance.pressureTestOffsets[i]);
				if (Grid.IsValidCell(num) && Grid.Element[num].IsGas && Grid.Mass[num] > 4f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040068CD RID: 26829
		private AmountInstance breath;

		// Token: 0x040068CE RID: 26830
		public AttributeModifier breathing;

		// Token: 0x040068CF RID: 26831
		public AttributeModifier holdingbreath;

		// Token: 0x040068D1 RID: 26833
		private static CellOffset[] pressureTestOffsets = new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(0, 1)
		};

		// Token: 0x040068D2 RID: 26834
		private const float HIGH_PRESSURE_DELAY = 3f;

		// Token: 0x040068D3 RID: 26835
		private bool wasInHighPressure;

		// Token: 0x040068D4 RID: 26836
		private float highPressureTime;
	}
}
