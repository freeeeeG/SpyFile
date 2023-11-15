using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x02000779 RID: 1913
[SkipSaveFileSerialization]
public class EarlyBird : StateMachineComponent<EarlyBird.StatesInstance>
{
	// Token: 0x060034F1 RID: 13553 RVA: 0x0011C4F8 File Offset: 0x0011A6F8
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.EARLYBIRD_MODIFIER, DUPLICANTS.TRAITS.EARLYBIRD.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x0011C674 File Offset: 0x0011A874
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x0011C6B0 File Offset: 0x0011A8B0
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x04001FFE RID: 8190
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x04001FFF RID: 8191
	private AttributeModifier[] attributeModifiers;

	// Token: 0x020014F7 RID: 5367
	public class StatesInstance : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.GameInstance
	{
		// Token: 0x06008660 RID: 34400 RVA: 0x00308918 File Offset: 0x00306B18
		public StatesInstance(EarlyBird master) : base(master)
		{
		}

		// Token: 0x06008661 RID: 34401 RVA: 0x00308921 File Offset: 0x00306B21
		public bool IsMorning()
		{
			return !(ScheduleManager.Instance == null) && !(base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview) && global::Schedule.GetBlockIdx() < TRAITS.EARLYBIRD_SCHEDULEBLOCK;
		}
	}

	// Token: 0x020014F8 RID: 5368
	public class States : GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird>
	{
		// Token: 0x06008662 RID: 34402 RVA: 0x0030895C File Offset: 0x00306B5C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (EarlyBird.StatesInstance smi) => smi.IsMorning(), UpdateRate.SIM_200ms);
			this.early.Enter("Morning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMorning", delegate(EarlyBird.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.EarlyMorning, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (EarlyBird.StatesInstance smi) => !smi.IsMorning(), UpdateRate.SIM_200ms);
		}

		// Token: 0x040066E6 RID: 26342
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State idle;

		// Token: 0x040066E7 RID: 26343
		public GameStateMachine<EarlyBird.States, EarlyBird.StatesInstance, EarlyBird, object>.State early;
	}
}
