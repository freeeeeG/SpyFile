using System;

// Token: 0x0200070B RID: 1803
[SkipSaveFileSerialization]
public class BlightVulnerable : StateMachineComponent<BlightVulnerable.StatesInstance>
{
	// Token: 0x0600318F RID: 12687 RVA: 0x00107963 File Offset: 0x00105B63
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x0010796B File Offset: 0x00105B6B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x0010797E File Offset: 0x00105B7E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x00107986 File Offset: 0x00105B86
	public void MakeBlighted()
	{
		Debug.Log("Blighting plant", this);
		base.smi.sm.isBlighted.Set(true, base.smi, false);
	}

	// Token: 0x04001DB4 RID: 7604
	private SchedulerHandle handle;

	// Token: 0x04001DB5 RID: 7605
	public bool prefersDarkness;

	// Token: 0x02001465 RID: 5221
	public class StatesInstance : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.GameInstance
	{
		// Token: 0x06008481 RID: 33921 RVA: 0x00302BBA File Offset: 0x00300DBA
		public StatesInstance(BlightVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x02001466 RID: 5222
	public class States : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable>
	{
		// Token: 0x06008482 RID: 33922 RVA: 0x00302BC4 File Offset: 0x00300DC4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.comfortable.ParamTransition<bool>(this.isBlighted, this.blighted, GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.IsTrue);
			this.blighted.TriggerOnEnter(GameHashes.BlightChanged, (BlightVulnerable.StatesInstance smi) => true).Enter(delegate(BlightVulnerable.StatesInstance smi)
			{
				smi.GetComponent<SeedProducer>().seedInfo.seedId = RotPileConfig.ID;
			}).ToggleTag(GameTags.Blighted).Exit(delegate(BlightVulnerable.StatesInstance smi)
			{
				GameplayEventManager.Instance.Trigger(-1425542080, smi.gameObject);
			});
		}

		// Token: 0x04006556 RID: 25942
		public StateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.BoolParameter isBlighted;

		// Token: 0x04006557 RID: 25943
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State comfortable;

		// Token: 0x04006558 RID: 25944
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State blighted;
	}
}
