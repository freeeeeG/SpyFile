using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000706 RID: 1798
public class AgeMonitor : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>
{
	// Token: 0x0600317A RID: 12666 RVA: 0x00107410 File Offset: 0x00105610
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		this.alive.ToggleAttributeModifier("Aging", (AgeMonitor.Instance smi) => this.aging, null).Transition(this.time_to_die, new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback(AgeMonitor.TimeToDie), UpdateRate.SIM_1000ms).Update(new Action<AgeMonitor.Instance, float>(AgeMonitor.UpdateOldStatusItem), UpdateRate.SIM_1000ms, false);
		this.time_to_die.Enter(new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback(AgeMonitor.Die));
		this.aging = new AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f, CREATURES.MODIFIERS.AGE.NAME, false, false, true);
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x001074BC File Offset: 0x001056BC
	private static void Die(AgeMonitor.Instance smi)
	{
		smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Generic);
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x001074D8 File Offset: 0x001056D8
	private static bool TimeToDie(AgeMonitor.Instance smi)
	{
		return smi.age.value >= smi.age.GetMax();
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x001074F8 File Offset: 0x001056F8
	private static void UpdateOldStatusItem(AgeMonitor.Instance smi, float dt)
	{
		KSelectable component = smi.GetComponent<KSelectable>();
		bool show = smi.age.value > smi.age.GetMax() * 0.9f;
		smi.oldStatusGuid = component.ToggleStatusItem(Db.Get().CreatureStatusItems.Old, smi.oldStatusGuid, show, smi);
	}

	// Token: 0x04001DA9 RID: 7593
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State alive;

	// Token: 0x04001DAA RID: 7594
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State time_to_die;

	// Token: 0x04001DAB RID: 7595
	private AttributeModifier aging;

	// Token: 0x02001459 RID: 5209
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06008462 RID: 33890 RVA: 0x0030260D File Offset: 0x0030080D
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Age.Id);
		}

		// Token: 0x04006540 RID: 25920
		public float maxAgePercentOnSpawn = 0.75f;
	}

	// Token: 0x0200145A RID: 5210
	public new class Instance : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.GameInstance
	{
		// Token: 0x06008464 RID: 33892 RVA: 0x00302648 File Offset: 0x00300848
		public Instance(IStateMachineTarget master, AgeMonitor.Def def) : base(master, def)
		{
			this.age = Db.Get().Amounts.Age.Lookup(base.gameObject);
			base.Subscribe(1119167081, delegate(object data)
			{
				this.RandomizeAge();
			});
		}

		// Token: 0x06008465 RID: 33893 RVA: 0x00302694 File Offset: 0x00300894
		public void RandomizeAge()
		{
			this.age.value = UnityEngine.Random.value * this.age.GetMax() * base.def.maxAgePercentOnSpawn;
			AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (amountInstance != null)
			{
				amountInstance.value = this.age.value / this.age.GetMax() * amountInstance.GetMax() * 1.75f;
				amountInstance.value = Mathf.Min(amountInstance.value, amountInstance.GetMax() * 0.9f);
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06008466 RID: 33894 RVA: 0x0030272E File Offset: 0x0030092E
		public float CyclesUntilDeath
		{
			get
			{
				return this.age.GetMax() - this.age.value;
			}
		}

		// Token: 0x04006541 RID: 25921
		public AmountInstance age;

		// Token: 0x04006542 RID: 25922
		public Guid oldStatusGuid;
	}
}
