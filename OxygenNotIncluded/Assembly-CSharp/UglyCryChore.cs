using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020003CD RID: 973
public class UglyCryChore : Chore<UglyCryChore.StatesInstance>
{
	// Token: 0x0600140D RID: 5133 RVA: 0x0006AFA4 File Offset: 0x000691A4
	public UglyCryChore(ChoreType chore_type, IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.UglyCry, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new UglyCryChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02001036 RID: 4150
	public class StatesInstance : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.GameInstance
	{
		// Token: 0x0600750B RID: 29963 RVA: 0x002CB7C4 File Offset: 0x002C99C4
		public StatesInstance(UglyCryChore master, GameObject crier) : base(master)
		{
			base.sm.crier.Set(crier, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(crier);
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x002CB804 File Offset: 0x002C9A04
		public void ProduceTears(float dt)
		{
			if (dt <= 0f)
			{
				return;
			}
			int gameCell = Grid.PosToCell(base.smi.master.gameObject);
			Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
			if (equippable != null)
			{
				equippable.GetComponent<Storage>().AddLiquid(SimHashes.Water, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, false, true);
				return;
			}
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.Water, CellEventLogger.Instance.Tears, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x04005876 RID: 22646
		private AmountInstance bodyTemperature;
	}

	// Token: 0x02001037 RID: 4151
	public class States : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore>
	{
		// Token: 0x0600750D RID: 29965 RVA: 0x002CB8AC File Offset: 0x002C9AAC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.cry;
			base.Target(this.crier);
			this.uglyCryingEffect = new Effect("UglyCrying", DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, DUPLICANTS.MODIFIERS.UGLY_CRYING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.uglyCryingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, false, false, true));
			Db.Get().effects.Add(this.uglyCryingEffect);
			this.cry.defaultState = this.cry.cry_pre.RemoveEffect("CryFace").ToggleAnims("anim_cry_kanim", 0f, "");
			this.cry.cry_pre.PlayAnim("working_pre").ScheduleGoTo(2f, this.cry.cry_loop);
			this.cry.cry_loop.ToggleAnims("anim_cry_kanim", 0f, "").Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.Play("working_loop", KAnim.PlayMode.Loop);
			}).ScheduleGoTo(18f, this.cry.cry_pst).ToggleEffect((UglyCryChore.StatesInstance smi) => this.uglyCryingEffect).Update(delegate(UglyCryChore.StatesInstance smi, float dt)
			{
				smi.ProduceTears(dt);
			}, UpdateRate.SIM_200ms, false);
			this.cry.cry_pst.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.AddEffect("CryFace").Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x04005877 RID: 22647
		public StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.TargetParameter crier;

		// Token: 0x04005878 RID: 22648
		public UglyCryChore.States.Cry cry;

		// Token: 0x04005879 RID: 22649
		public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State complete;

		// Token: 0x0400587A RID: 22650
		private Effect uglyCryingEffect;

		// Token: 0x02001FF6 RID: 8182
		public class Cry : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State
		{
			// Token: 0x04008FE2 RID: 36834
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pre;

			// Token: 0x04008FE3 RID: 36835
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_loop;

			// Token: 0x04008FE4 RID: 36836
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pst;
		}
	}
}
