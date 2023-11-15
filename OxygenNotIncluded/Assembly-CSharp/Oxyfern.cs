using System;
using STRINGS;
using UnityEngine;

// Token: 0x020008E5 RID: 2277
public class Oxyfern : StateMachineComponent<Oxyfern.StatesInstance>
{
	// Token: 0x060041CC RID: 16844 RVA: 0x0017021F File Offset: 0x0016E41F
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x00170237 File Offset: 0x0016E437
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x0017024A File Offset: 0x0016E44A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (Tutorial.Instance.oxygenGenerators.Contains(base.gameObject))
		{
			Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		}
	}

	// Token: 0x060041CF RID: 16847 RVA: 0x0017027F File Offset: 0x0016E47F
	protected override void OnPrefabInit()
	{
		base.Subscribe<Oxyfern>(1309017699, Oxyfern.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x060041D0 RID: 16848 RVA: 0x00170298 File Offset: 0x0016E498
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
		if (this.receptacleMonitor.Replanted)
		{
			Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
		}
	}

	// Token: 0x060041D1 RID: 16849 RVA: 0x001702C2 File Offset: 0x0016E4C2
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.00062500004f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.00015625001f;
	}

	// Token: 0x04002AF9 RID: 11001
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04002AFA RID: 11002
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04002AFB RID: 11003
	[MyCmpReq]
	private ElementConverter elementConverter;

	// Token: 0x04002AFC RID: 11004
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04002AFD RID: 11005
	private static readonly EventSystem.IntraObjectHandler<Oxyfern> OnReplantedDelegate = new EventSystem.IntraObjectHandler<Oxyfern>(delegate(Oxyfern component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x0200172A RID: 5930
	public class StatesInstance : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.GameInstance
	{
		// Token: 0x06008DA8 RID: 36264 RVA: 0x0031D101 File Offset: 0x0031B301
		public StatesInstance(Oxyfern master) : base(master)
		{
		}
	}

	// Token: 0x0200172B RID: 5931
	public class States : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern>
	{
		// Token: 0x06008DA9 RID: 36265 RVA: 0x0031D10C File Offset: 0x0031B30C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.grow;
			this.dead.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (Oxyfern.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(Oxyfern.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_pst", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (Oxyfern.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle_full", KAnim.PlayMode.Loop).Enter(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(Oxyfern.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			});
			this.alive.wilting.PlayAnim("wilt3").EventTransition(GameHashes.WiltRecover, this.alive.mature, (Oxyfern.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04006DD2 RID: 28114
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State grow;

		// Token: 0x04006DD3 RID: 28115
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State blocked_from_growing;

		// Token: 0x04006DD4 RID: 28116
		public Oxyfern.States.AliveStates alive;

		// Token: 0x04006DD5 RID: 28117
		public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State dead;

		// Token: 0x020021C6 RID: 8646
		public class AliveStates : GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.PlantAliveSubState
		{
			// Token: 0x0400974B RID: 38731
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State mature;

			// Token: 0x0400974C RID: 38732
			public GameStateMachine<Oxyfern.States, Oxyfern.StatesInstance, Oxyfern, object>.State wilting;
		}
	}
}
