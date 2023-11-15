using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200085F RID: 2143
[SkipSaveFileSerialization]
public class Meteorphile : StateMachineComponent<Meteorphile.StatesInstance>
{
	// Token: 0x06003EAA RID: 16042 RVA: 0x0015C4BC File Offset: 0x0015A6BC
	protected override void OnSpawn()
	{
		this.attributeModifiers = new AttributeModifier[]
		{
			new AttributeModifier("Construction", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Digging", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Machinery", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Athletics", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Learning", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Cooking", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Art", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Strength", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Caring", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Botanist", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true),
			new AttributeModifier("Ranching", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME, false, false, true)
		};
		base.smi.StartSM();
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0015C638 File Offset: 0x0015A838
	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0015C674 File Offset: 0x0015A874
	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < this.attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = this.attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}

	// Token: 0x0400289D RID: 10397
	[MyCmpReq]
	private KPrefabID kPrefabID;

	// Token: 0x0400289E RID: 10398
	private AttributeModifier[] attributeModifiers;

	// Token: 0x02001637 RID: 5687
	public class StatesInstance : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.GameInstance
	{
		// Token: 0x060089F7 RID: 35319 RVA: 0x00312A46 File Offset: 0x00310C46
		public StatesInstance(Meteorphile master) : base(master)
		{
		}

		// Token: 0x060089F8 RID: 35320 RVA: 0x00312A50 File Offset: 0x00310C50
		public bool IsMeteors()
		{
			if (GameplayEventManager.Instance == null || base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
			{
				return false;
			}
			int myWorldId = this.GetMyWorldId();
			List<GameplayEventInstance> list = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(myWorldId, ref list);
			for (int i = 0; i < list.Count; i++)
			{
				MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
				if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x02001638 RID: 5688
	public class States : GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile>
	{
		// Token: 0x060089F9 RID: 35321 RVA: 0x00312AE4 File Offset: 0x00310CE4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.early, (Meteorphile.StatesInstance smi) => smi.IsMeteors(), UpdateRate.SIM_200ms);
			this.early.Enter("Meteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMeteors", delegate(Meteorphile.StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.Meteorphile, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.idle, (Meteorphile.StatesInstance smi) => !smi.IsMeteors(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04006AFB RID: 27387
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State idle;

		// Token: 0x04006AFC RID: 27388
		public GameStateMachine<Meteorphile.States, Meteorphile.StatesInstance, Meteorphile, object>.State early;
	}
}
