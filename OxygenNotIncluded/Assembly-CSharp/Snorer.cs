using System;
using UnityEngine;

// Token: 0x0200097B RID: 2427
[SkipSaveFileSerialization]
public class Snorer : StateMachineComponent<Snorer.StatesInstance>
{
	// Token: 0x06004785 RID: 18309 RVA: 0x00193A8A File Offset: 0x00191C8A
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04002F5E RID: 12126
	private static readonly HashedString HeadHash = "snapTo_mouth";

	// Token: 0x020017E0 RID: 6112
	public class StatesInstance : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.GameInstance
	{
		// Token: 0x06008FA8 RID: 36776 RVA: 0x0032330D File Offset: 0x0032150D
		public StatesInstance(Snorer master) : base(master)
		{
		}

		// Token: 0x06008FA9 RID: 36777 RVA: 0x00323318 File Offset: 0x00321518
		public bool IsSleeping()
		{
			StaminaMonitor.Instance smi = base.master.GetSMI<StaminaMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06008FAA RID: 36778 RVA: 0x0032333C File Offset: 0x0032153C
		public void StartSmallSnore()
		{
			this.snoreHandle = GameScheduler.Instance.Schedule("snorelines", 2f, new Action<object>(this.StartSmallSnoreInternal), null, null);
		}

		// Token: 0x06008FAB RID: 36779 RVA: 0x00323368 File Offset: 0x00321568
		private void StartSmallSnoreInternal(object data)
		{
			this.snoreHandle.ClearScheduler();
			bool flag;
			Matrix4x4 symbolTransform = base.smi.master.GetComponent<KBatchedAnimController>().GetSymbolTransform(Snorer.HeadHash, out flag);
			if (flag)
			{
				Vector3 position = symbolTransform.GetColumn(3);
				position.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				this.snoreEffect = FXHelpers.CreateEffect("snore_fx_kanim", position, null, false, Grid.SceneLayer.Front, false);
				this.snoreEffect.destroyOnAnimComplete = true;
				this.snoreEffect.Play("snore", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06008FAC RID: 36780 RVA: 0x003233FE File Offset: 0x003215FE
		public void StopSmallSnore()
		{
			this.snoreHandle.ClearScheduler();
			if (this.snoreEffect != null)
			{
				this.snoreEffect.PlayMode = KAnim.PlayMode.Once;
			}
			this.snoreEffect = null;
		}

		// Token: 0x06008FAD RID: 36781 RVA: 0x0032342C File Offset: 0x0032162C
		public void StartSnoreBGEffect()
		{
			AcousticDisturbance.Emit(base.smi.master.gameObject, 3);
		}

		// Token: 0x06008FAE RID: 36782 RVA: 0x00323444 File Offset: 0x00321644
		public void StopSnoreBGEffect()
		{
		}

		// Token: 0x04007040 RID: 28736
		private SchedulerHandle snoreHandle;

		// Token: 0x04007041 RID: 28737
		private KBatchedAnimController snoreEffect;

		// Token: 0x04007042 RID: 28738
		private KBatchedAnimController snoreBGEffect;

		// Token: 0x04007043 RID: 28739
		private const float BGEmissionRadius = 3f;
	}

	// Token: 0x020017E1 RID: 6113
	public class States : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer>
	{
		// Token: 0x06008FAF RID: 36783 RVA: 0x00323448 File Offset: 0x00321648
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false);
			this.idle.Transition(this.sleeping, (Snorer.StatesInstance smi) => smi.IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.DefaultState(this.sleeping.quiet).Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSmallSnore();
			}).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSmallSnore();
			}).Transition(this.idle, (Snorer.StatesInstance smi) => !smi.master.GetSMI<StaminaMonitor.Instance>().IsSleeping(), UpdateRate.SIM_200ms);
			this.sleeping.quiet.Enter("ScheduleNextSnore", delegate(Snorer.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.sleeping.snoring);
			});
			this.sleeping.snoring.Enter(delegate(Snorer.StatesInstance smi)
			{
				smi.StartSnoreBGEffect();
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.sleeping.quiet).Exit(delegate(Snorer.StatesInstance smi)
			{
				smi.StopSnoreBGEffect();
			});
		}

		// Token: 0x06008FB0 RID: 36784 RVA: 0x003235CC File Offset: 0x003217CC
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(5f, 1f), 3f), 10f);
		}

		// Token: 0x04007044 RID: 28740
		public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State idle;

		// Token: 0x04007045 RID: 28741
		public Snorer.States.SleepStates sleeping;

		// Token: 0x020021E1 RID: 8673
		public class SleepStates : GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State
		{
			// Token: 0x040097B3 RID: 38835
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State quiet;

			// Token: 0x040097B4 RID: 38836
			public GameStateMachine<Snorer.States, Snorer.StatesInstance, Snorer, object>.State snoring;
		}
	}
}
