using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020003C3 RID: 963
public class RecoverBreathChore : Chore<RecoverBreathChore.StatesInstance>
{
	// Token: 0x060013F4 RID: 5108 RVA: 0x0006A60C File Offset: 0x0006880C
	public RecoverBreathChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverBreath, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverBreathChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200101E RID: 4126
	public class StatesInstance : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.GameInstance
	{
		// Token: 0x060074BC RID: 29884 RVA: 0x002C9B6C File Offset: 0x002C7D6C
		public StatesInstance(RecoverBreathChore master, GameObject recoverer) : base(master)
		{
			base.sm.recoverer.Set(recoverer, base.smi, false);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float value = 3f;
			this.recoveringbreath = new AttributeModifier(deltaAttribute.Id, value, DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME, false, false, true);
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x002C9BD4 File Offset: 0x002C7DD4
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverBreathLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x002C9C0C File Offset: 0x002C7E0C
		public void UpdateLocator()
		{
			int num = base.sm.recoverer.GetSMI<BreathMonitor.Instance>(base.smi).GetRecoverCell();
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.recoverer.Get<Transform>(base.smi).GetPosition());
			}
			Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
			base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x002C9C84 File Offset: 0x002C7E84
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x002C9CB0 File Offset: 0x002C7EB0
		public void RemoveSuitIfNecessary()
		{
			Equipment equipment = base.sm.recoverer.Get<Equipment>(base.smi);
			if (equipment == null)
			{
				return;
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable == null)
			{
				return;
			}
			assignable.Unassign();
		}

		// Token: 0x04005832 RID: 22578
		public AttributeModifier recoveringbreath;
	}

	// Token: 0x0200101F RID: 4127
	public class States : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore>
	{
		// Token: 0x060074C1 RID: 29889 RVA: 0x002C9D04 File Offset: 0x002C7F04
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.recoverer);
			this.root.Enter("CreateLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.CreateLocator();
			}).Exit("DestroyLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverBreathChore.StatesInstance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.recoverer, this.locator, this.remove_suit, null, null, null);
			this.remove_suit.GoTo(this.recover);
			this.recover.ToggleAnims("anim_emotes_default_kanim", 0f, "").DefaultState(this.recover.pre).ToggleAttributeModifier("Recovering Breath", (RecoverBreathChore.StatesInstance smi) => smi.recoveringbreath, null).ToggleTag(GameTags.RecoveringBreath).TriggerOnEnter(GameHashes.BeginBreathRecovery, null).TriggerOnExit(GameHashes.EndBreathRecovery, null);
			this.recover.pre.PlayAnim("breathe_pre").OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop);
			this.recover.pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(null);
		}

		// Token: 0x04005833 RID: 22579
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04005834 RID: 22580
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.PreLoopPostState recover;

		// Token: 0x04005835 RID: 22581
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State remove_suit;

		// Token: 0x04005836 RID: 22582
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter recoverer;

		// Token: 0x04005837 RID: 22583
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter locator;
	}
}
