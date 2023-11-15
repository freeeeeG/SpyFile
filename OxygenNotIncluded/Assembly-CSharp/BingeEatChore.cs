using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class BingeEatChore : Chore<BingeEatChore.StatesInstance>
{
	// Token: 0x06001390 RID: 5008 RVA: 0x000671FC File Offset: 0x000653FC
	public BingeEatChore(IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.BingeEat, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BingeEatChore.StatesInstance(this, target.gameObject);
		base.Subscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x0006725C File Offset: 0x0006545C
	private void OnEat(object data)
	{
		Edible edible = (Edible)data;
		if (edible != null)
		{
			base.smi.sm.bingeremaining.Set(Mathf.Max(0f, base.smi.sm.bingeremaining.Get(base.smi) - edible.unitsConsumed), base.smi, false);
		}
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000672C2 File Offset: 0x000654C2
	public override void Cleanup()
	{
		base.Cleanup();
		base.Unsubscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x02000FE6 RID: 4070
	public class StatesInstance : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.GameInstance
	{
		// Token: 0x060073DD RID: 29661 RVA: 0x002C417D File Offset: 0x002C237D
		public StatesInstance(BingeEatChore master, GameObject eater) : base(master)
		{
			base.sm.eater.Set(eater, base.smi, false);
			base.sm.bingeremaining.Set(2f, base.smi, false);
		}

		// Token: 0x060073DE RID: 29662 RVA: 0x002C41BC File Offset: 0x002C23BC
		public void FindFood()
		{
			Navigator component = base.GetComponent<Navigator>();
			int num = int.MaxValue;
			Edible edible = null;
			if (base.sm.bingeremaining.Get(base.smi) <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				this.GoTo(base.sm.eat_pst);
				return;
			}
			foreach (Edible edible2 in Components.Edibles.Items)
			{
				if (!(edible2 == null) && !(edible2 == base.sm.ediblesource.Get<Edible>(base.smi)) && !edible2.isBeingConsumed && edible2.GetComponent<Pickupable>().UnreservedAmount > 0f && edible2.GetComponent<Pickupable>().CouldBePickedUpByMinion(base.gameObject))
				{
					int navigationCost = component.GetNavigationCost(edible2);
					if (navigationCost != -1 && navigationCost < num)
					{
						num = navigationCost;
						edible = edible2;
					}
				}
			}
			base.sm.ediblesource.Set(edible, base.smi);
			base.sm.requestedfoodunits.Set(base.sm.bingeremaining.Get(base.smi), base.smi, false);
			if (edible == null)
			{
				this.GoTo(base.sm.cantFindFood);
				return;
			}
			this.GoTo(base.sm.fetch);
		}

		// Token: 0x060073DF RID: 29663 RVA: 0x002C4334 File Offset: 0x002C2534
		public bool IsBingeEating()
		{
			return base.sm.isBingeEating.Get(base.smi);
		}
	}

	// Token: 0x02000FE7 RID: 4071
	public class States : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore>
	{
		// Token: 0x060073E0 RID: 29664 RVA: 0x002C434C File Offset: 0x002C254C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findfood;
			base.Target(this.eater);
			this.bingeEatingEffect = new Effect("Binge_Eating", DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, DUPLICANTS.MODIFIERS.BINGE_EATING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.bingeEatingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			this.bingeEatingEffect.Add(new AttributeModifier("CaloriesDelta", -6666.6665f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			Db.Get().effects.Add(this.bingeEatingEffect);
			this.root.ToggleEffect((BingeEatChore.StatesInstance smi) => this.bingeEatingEffect);
			this.noTarget.GoTo(this.finish);
			this.eat_pst.ToggleAnims("anim_eat_overeat_kanim", 0f, "").PlayAnim("working_pst").OnAnimQueueComplete(this.finish);
			this.finish.Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				smi.StopSM("complete/no more food");
			});
			this.findfood.Enter("FindFood", delegate(BingeEatChore.StatesInstance smi)
			{
				smi.FindFood();
			});
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.eat, this.cantFindFood);
			this.eat.ToggleAnims("anim_eat_overeat_kanim", 0f, "").QueueAnim("working_loop", true, null).Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(true, smi, false);
			}).DoEat(this.ediblechunk, this.actualfoodunits, this.findfood, this.findfood).Exit("ClearIsBingeEating", delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(false, smi, false);
			});
			this.cantFindFood.ToggleAnims("anim_interrupt_binge_eat_kanim", 0f, "").PlayAnim("interrupt_binge_eat").OnAnimQueueComplete(this.noTarget);
		}

		// Token: 0x0400573E RID: 22334
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter eater;

		// Token: 0x0400573F RID: 22335
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblesource;

		// Token: 0x04005740 RID: 22336
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblechunk;

		// Token: 0x04005741 RID: 22337
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.BoolParameter isBingeEating;

		// Token: 0x04005742 RID: 22338
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x04005743 RID: 22339
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter actualfoodunits;

		// Token: 0x04005744 RID: 22340
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter bingeremaining;

		// Token: 0x04005745 RID: 22341
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State noTarget;

		// Token: 0x04005746 RID: 22342
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State findfood;

		// Token: 0x04005747 RID: 22343
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat;

		// Token: 0x04005748 RID: 22344
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat_pst;

		// Token: 0x04005749 RID: 22345
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State cantFindFood;

		// Token: 0x0400574A RID: 22346
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State finish;

		// Token: 0x0400574B RID: 22347
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FetchSubState fetch;

		// Token: 0x0400574C RID: 22348
		private Effect bingeEatingEffect;
	}
}
