using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
public class OilEater : StateMachineComponent<OilEater.StatesInstance>
{
	// Token: 0x060041C9 RID: 16841 RVA: 0x00170151 File Offset: 0x0016E351
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060041CA RID: 16842 RVA: 0x00170164 File Offset: 0x0016E364
	public void Exhaust(float dt)
	{
		if (base.smi.master.wiltCondition.IsWilting())
		{
			return;
		}
		this.emittedMass += dt * this.emitRate;
		if (this.emittedMass >= this.minEmitMass)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffset);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementEmitted, this.emittedMass, component.Temperature, byte.MaxValue, 0, true, -1);
			this.emittedMass = 0f;
		}
	}

	// Token: 0x04002AF0 RID: 10992
	private const SimHashes srcElement = SimHashes.CrudeOil;

	// Token: 0x04002AF1 RID: 10993
	private const SimHashes emitElement = SimHashes.CarbonDioxide;

	// Token: 0x04002AF2 RID: 10994
	public float emitRate = 1f;

	// Token: 0x04002AF3 RID: 10995
	public float minEmitMass;

	// Token: 0x04002AF4 RID: 10996
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04002AF5 RID: 10997
	[Serialize]
	private float emittedMass;

	// Token: 0x04002AF6 RID: 10998
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04002AF7 RID: 10999
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04002AF8 RID: 11000
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x02001728 RID: 5928
	public class StatesInstance : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.GameInstance
	{
		// Token: 0x06008DA1 RID: 36257 RVA: 0x0031CE51 File Offset: 0x0031B051
		public StatesInstance(OilEater master) : base(master)
		{
		}
	}

	// Token: 0x02001729 RID: 5929
	public class States : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater>
	{
		// Token: 0x06008DA2 RID: 36258 RVA: 0x0031CE5C File Offset: 0x0031B05C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			this.dead.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(OilEater.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, delegate(object data)
				{
					GameObject gameObject = (GameObject)data;
					CreatureHelpers.DeselectCreature(gameObject);
					Util.KDestroyGameObject(gameObject);
				}, smi.master.gameObject);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(OilEater.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update("Alive", delegate(OilEater.StatesInstance smi, float dt)
			{
				smi.master.Exhaust(dt);
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (OilEater.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop);
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (OilEater.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04006DCE RID: 28110
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State grow;

		// Token: 0x04006DCF RID: 28111
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State blocked_from_growing;

		// Token: 0x04006DD0 RID: 28112
		public OilEater.States.AliveStates alive;

		// Token: 0x04006DD1 RID: 28113
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State dead;

		// Token: 0x020021C4 RID: 8644
		public class AliveStates : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.PlantAliveSubState
		{
			// Token: 0x04009743 RID: 38723
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State mature;

			// Token: 0x04009744 RID: 38724
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State wilting;
		}
	}
}
