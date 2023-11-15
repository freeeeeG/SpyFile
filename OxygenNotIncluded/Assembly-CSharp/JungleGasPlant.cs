using System;
using STRINGS;
using UnityEngine;

// Token: 0x020008E3 RID: 2275
public class JungleGasPlant : StateMachineComponent<JungleGasPlant.StatesInstance>
{
	// Token: 0x060041C6 RID: 16838 RVA: 0x0017011E File Offset: 0x0016E31E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060041C7 RID: 16839 RVA: 0x00170131 File Offset: 0x0016E331
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04002AEC RID: 10988
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04002AED RID: 10989
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04002AEE RID: 10990
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04002AEF RID: 10991
	[MyCmpReq]
	private ElementEmitter elementEmitter;

	// Token: 0x02001726 RID: 5926
	public class StatesInstance : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.GameInstance
	{
		// Token: 0x06008D9D RID: 36253 RVA: 0x0031C9F3 File Offset: 0x0031ABF3
		public StatesInstance(JungleGasPlant master) : base(master)
		{
		}
	}

	// Token: 0x02001727 RID: 5927
	public class States : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant>
	{
		// Token: 0x06008D9E RID: 36254 RVA: 0x0031C9FC File Offset: 0x0031ABFC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.alive.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive.seed_grow);
			});
			this.dead.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).TagTransition(GameTags.Entombed, this.alive.seed_grow, true).EventTransition(GameHashes.TooColdWarning, this.alive.seed_grow, null).EventTransition(GameHashes.TooHotWarning, this.alive.seed_grow, null).TagTransition(GameTags.Uprooted, this.dead, false);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.seed_grow.QueueAnim("seed_grow", false, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null).EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting());
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.grown, (JungleGasPlant.StatesInstance smi) => smi.master.growing.IsGrown()).PlayAnim("idle_loop", KAnim.PlayMode.Loop);
			this.alive.grown.DefaultState(this.alive.grown.pre).EventTransition(GameHashes.Wilt, this.alive.wilting, (JungleGasPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(JungleGasPlant.StatesInstance smi)
			{
				smi.master.elementEmitter.SetEmitting(true);
			}).Exit(delegate(JungleGasPlant.StatesInstance smi)
			{
				smi.master.elementEmitter.SetEmitting(false);
			});
			this.alive.grown.pre.PlayAnim("grow", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.grown.idle);
			this.alive.grown.idle.PlayAnim("idle_bloom_loop", KAnim.PlayMode.Loop);
			this.alive.wilting.pre.DefaultState(this.alive.wilting.pre).PlayAnim("wilt_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.wilting.idle).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (JungleGasPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.alive.wilting.idle.PlayAnim("idle_wilt_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.wilting.pst, (JungleGasPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
			this.alive.wilting.pst.PlayAnim("wilt_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x04006DCB RID: 28107
		public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State blocked_from_growing;

		// Token: 0x04006DCC RID: 28108
		public JungleGasPlant.States.AliveStates alive;

		// Token: 0x04006DCD RID: 28109
		public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State dead;

		// Token: 0x020021C0 RID: 8640
		public class AliveStates : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.PlantAliveSubState
		{
			// Token: 0x0400972F RID: 38703
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State seed_grow;

			// Token: 0x04009730 RID: 38704
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;

			// Token: 0x04009731 RID: 38705
			public JungleGasPlant.States.WiltingState wilting;

			// Token: 0x04009732 RID: 38706
			public JungleGasPlant.States.GrownState grown;

			// Token: 0x04009733 RID: 38707
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State destroy;
		}

		// Token: 0x020021C1 RID: 8641
		public class GrownState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
		{
			// Token: 0x04009734 RID: 38708
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;

			// Token: 0x04009735 RID: 38709
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;
		}

		// Token: 0x020021C2 RID: 8642
		public class WiltingState : GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State
		{
			// Token: 0x04009736 RID: 38710
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pre;

			// Token: 0x04009737 RID: 38711
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State idle;

			// Token: 0x04009738 RID: 38712
			public GameStateMachine<JungleGasPlant.States, JungleGasPlant.StatesInstance, JungleGasPlant, object>.State pst;
		}
	}
}
