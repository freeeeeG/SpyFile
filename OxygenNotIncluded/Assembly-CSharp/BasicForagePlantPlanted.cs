using System;
using UnityEngine;

// Token: 0x020008DD RID: 2269
public class BasicForagePlantPlanted : StateMachineComponent<BasicForagePlantPlanted.StatesInstance>
{
	// Token: 0x06004191 RID: 16785 RVA: 0x0016F101 File Offset: 0x0016D301
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x0016F114 File Offset: 0x0016D314
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04002AB2 RID: 10930
	[MyCmpReq]
	private Harvestable harvestable;

	// Token: 0x04002AB3 RID: 10931
	[MyCmpReq]
	private SeedProducer seedProducer;

	// Token: 0x04002AB4 RID: 10932
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x02001719 RID: 5913
	public class StatesInstance : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.GameInstance
	{
		// Token: 0x06008D6E RID: 36206 RVA: 0x0031B9F5 File Offset: 0x00319BF5
		public StatesInstance(BasicForagePlantPlanted smi) : base(smi)
		{
		}
	}

	// Token: 0x0200171A RID: 5914
	public class States : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted>
	{
		// Token: 0x06008D6F RID: 36207 RVA: 0x0031BA00 File Offset: 0x00319C00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.seed_grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.seed_grow.PlayAnim("idle", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive.idle, null);
			this.alive.InitializeStates(this.masterTarget, this.dead);
			this.alive.idle.PlayAnim("idle").EventTransition(GameHashes.Harvest, this.alive.harvest, null).Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.harvestable.SetCanBeHarvested(true);
			});
			this.alive.harvest.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				smi.master.seedProducer.DropSeed(null);
			}).GoTo(this.dead);
			this.dead.Enter(delegate(BasicForagePlantPlanted.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.animController.StopAndClear();
				UnityEngine.Object.Destroy(smi.master.animController);
				smi.master.DestroySelf(null);
			});
		}

		// Token: 0x04006DAF RID: 28079
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State seed_grow;

		// Token: 0x04006DB0 RID: 28080
		public BasicForagePlantPlanted.States.AliveStates alive;

		// Token: 0x04006DB1 RID: 28081
		public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State dead;

		// Token: 0x020021B5 RID: 8629
		public class AliveStates : GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.PlantAliveSubState
		{
			// Token: 0x040096FB RID: 38651
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State idle;

			// Token: 0x040096FC RID: 38652
			public GameStateMachine<BasicForagePlantPlanted.States, BasicForagePlantPlanted.StatesInstance, BasicForagePlantPlanted, object>.State harvest;
		}
	}
}
