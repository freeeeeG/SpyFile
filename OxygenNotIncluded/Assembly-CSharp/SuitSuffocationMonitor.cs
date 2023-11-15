using System;
using Klei.AI;
using STRINGS;

// Token: 0x020009FD RID: 2557
public class SuitSuffocationMonitor : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance>
{
	// Token: 0x06004C83 RID: 19587 RVA: 0x001ACDF8 File Offset: 0x001AAFF8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DefaultState(this.satisfied.normal).ToggleAttributeModifier("Breathing", (SuitSuffocationMonitor.Instance smi) => smi.breathing, null).Transition(this.nooxygen, (SuitSuffocationMonitor.Instance smi) => smi.IsTankEmpty(), UpdateRate.SIM_200ms);
		this.satisfied.normal.Transition(this.satisfied.low, (SuitSuffocationMonitor.Instance smi) => smi.suitTank.NeedsRecharging(), UpdateRate.SIM_200ms);
		this.satisfied.low.DoNothing();
		this.nooxygen.ToggleExpression(Db.Get().Expressions.Suffocate, null).ToggleAttributeModifier("Holding Breath", (SuitSuffocationMonitor.Instance smi) => smi.holdingbreath, null).ToggleTag(GameTags.NoOxygen).DefaultState(this.nooxygen.holdingbreath);
		this.nooxygen.holdingbreath.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.HoldingBreath, null).Transition(this.nooxygen.suffocating, (SuitSuffocationMonitor.Instance smi) => smi.IsSuffocating(), UpdateRate.SIM_200ms);
		this.nooxygen.suffocating.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Suffocation, Db.Get().DuplicantStatusItems.Suffocating, null).Transition(this.death, (SuitSuffocationMonitor.Instance smi) => smi.HasSuffocated(), UpdateRate.SIM_200ms);
		this.death.Enter("SuffocationDeath", delegate(SuitSuffocationMonitor.Instance smi)
		{
			smi.Kill();
		});
	}

	// Token: 0x040031E3 RID: 12771
	public SuitSuffocationMonitor.SatisfiedState satisfied;

	// Token: 0x040031E4 RID: 12772
	public SuitSuffocationMonitor.NoOxygenState nooxygen;

	// Token: 0x040031E5 RID: 12773
	public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State death;

	// Token: 0x02001887 RID: 6279
	public class NoOxygenState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400723A RID: 29242
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State holdingbreath;

		// Token: 0x0400723B RID: 29243
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State suffocating;
	}

	// Token: 0x02001888 RID: 6280
	public class SatisfiedState : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400723C RID: 29244
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x0400723D RID: 29245
		public GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.State low;
	}

	// Token: 0x02001889 RID: 6281
	public new class Instance : GameStateMachine<SuitSuffocationMonitor, SuitSuffocationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060091F5 RID: 37365 RVA: 0x0032AAC5 File Offset: 0x00328CC5
		// (set) Token: 0x060091F6 RID: 37366 RVA: 0x0032AACD File Offset: 0x00328CCD
		public SuitTank suitTank { get; private set; }

		// Token: 0x060091F7 RID: 37367 RVA: 0x0032AAD8 File Offset: 0x00328CD8
		public Instance(IStateMachineTarget master, SuitTank suit_tank) : base(master)
		{
			this.breath = Db.Get().Amounts.Breath.Lookup(master.gameObject);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float num = 0.90909094f;
			this.breathing = new AttributeModifier(deltaAttribute.Id, num, DUPLICANTS.MODIFIERS.BREATHING.NAME, false, false, true);
			this.holdingbreath = new AttributeModifier(deltaAttribute.Id, -num, DUPLICANTS.MODIFIERS.HOLDINGBREATH.NAME, false, false, true);
			this.suitTank = suit_tank;
		}

		// Token: 0x060091F8 RID: 37368 RVA: 0x0032AB6D File Offset: 0x00328D6D
		public bool IsTankEmpty()
		{
			return this.suitTank.IsEmpty();
		}

		// Token: 0x060091F9 RID: 37369 RVA: 0x0032AB7A File Offset: 0x00328D7A
		public bool HasSuffocated()
		{
			return this.breath.value <= 0f;
		}

		// Token: 0x060091FA RID: 37370 RVA: 0x0032AB91 File Offset: 0x00328D91
		public bool IsSuffocating()
		{
			return this.breath.value <= 45.454548f;
		}

		// Token: 0x060091FB RID: 37371 RVA: 0x0032ABA8 File Offset: 0x00328DA8
		public void Kill()
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Suffocation);
		}

		// Token: 0x0400723E RID: 29246
		private AmountInstance breath;

		// Token: 0x0400723F RID: 29247
		public AttributeModifier breathing;

		// Token: 0x04007240 RID: 29248
		public AttributeModifier holdingbreath;

		// Token: 0x04007241 RID: 29249
		private OxygenBreather masterOxygenBreather;
	}
}
