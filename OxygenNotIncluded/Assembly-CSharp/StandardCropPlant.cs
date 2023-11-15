using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020008EA RID: 2282
public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
	// Token: 0x060041F2 RID: 16882 RVA: 0x00170E66 File Offset: 0x0016F066
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060041F3 RID: 16883 RVA: 0x00170E79 File Offset: 0x0016F079
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060041F4 RID: 16884 RVA: 0x00170E94 File Offset: 0x0016F094
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x060041F5 RID: 16885 RVA: 0x00170EF1 File Offset: 0x0016F0F1
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x060041F6 RID: 16886 RVA: 0x00170F0C File Offset: 0x0016F10C
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x04002B0E RID: 11022
	private const int WILT_LEVELS = 3;

	// Token: 0x04002B0F RID: 11023
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04002B10 RID: 11024
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04002B11 RID: 11025
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04002B12 RID: 11026
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04002B13 RID: 11027
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04002B14 RID: 11028
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04002B15 RID: 11029
	public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet
	{
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning"
	};

	// Token: 0x04002B16 RID: 11030
	public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;

	// Token: 0x0200173E RID: 5950
	public class AnimSet
	{
		// Token: 0x06008DF1 RID: 36337 RVA: 0x0031DF90 File Offset: 0x0031C190
		public string GetWiltLevel(int level)
		{
			if (this.m_wilt == null)
			{
				this.m_wilt = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.m_wilt[i] = this.wilt_base + (i + 1).ToString();
				}
			}
			return this.m_wilt[level - 1];
		}

		// Token: 0x04006E11 RID: 28177
		public string grow;

		// Token: 0x04006E12 RID: 28178
		public string grow_pst;

		// Token: 0x04006E13 RID: 28179
		public string idle_full;

		// Token: 0x04006E14 RID: 28180
		public string wilt_base;

		// Token: 0x04006E15 RID: 28181
		public string harvest;

		// Token: 0x04006E16 RID: 28182
		public string waning;

		// Token: 0x04006E17 RID: 28183
		private string[] m_wilt;
	}

	// Token: 0x0200173F RID: 5951
	public class States : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
	{
		// Token: 0x06008DF3 RID: 36339 RVA: 0x0031DFF0 File Offset: 0x0031C1F0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component != null && component.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.waning, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted, null).TagTransition(GameTags.Blighted, this.alive, true);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleComponent<Growing>(false).TagTransition(GameTags.Blighted, this.blighted, false);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StandardCropPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.CropSleep, this.alive.sleeping, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping)).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Paused).Enter(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent)).Update(new Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.ConsumePlant, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent));
			this.alive.pre_fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow_pst, KAnim.PlayMode.Once).TriggerOnEnter(GameHashes.BurstEmitDisease, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting, null);
			this.alive.fruiting_lost.Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).GoTo(this.alive.idle);
			this.alive.wilting.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StandardCropPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Harvest, this.alive.harvest, null);
			this.alive.sleeping.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Once).EventTransition(GameHashes.CropWakeUp, this.alive.idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping))).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null);
			this.alive.fruiting.DefaultState(this.alive.fruiting.fruiting_idle).EventTransition(GameHashes.Wilt, this.alive.wilting, null).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StandardCropPlant.StatesInstance smi) => !smi.master.growing.ReachedNextHarvest());
			this.alive.fruiting.fruiting_idle.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.idle_full, KAnim.PlayMode.Loop).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.alive.fruiting.fruiting_old, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsOld), UpdateRate.SIM_4000ms);
			this.alive.fruiting.fruiting_old.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim), KAnim.PlayMode.Loop).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).Transition(this.alive.fruiting.fruiting_idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsOld)), UpdateRate.SIM_4000ms);
			this.alive.harvest.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.harvest, KAnim.PlayMode.Once).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master != null)
				{
					smi.master.crop.SpawnConfiguredFruit(null);
				}
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).Exit(delegate(StandardCropPlant.StatesInstance smi)
			{
				smi.Trigger(113170146, null);
			}).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x06008DF4 RID: 36340 RVA: 0x0031E554 File Offset: 0x0031C754
		private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
		{
			float num = smi.master.growing.PercentOfCurrentHarvest();
			int level;
			if (num < 0.75f)
			{
				level = 1;
			}
			else if (num < 1f)
			{
				level = 2;
			}
			else
			{
				level = 3;
			}
			return smi.master.anims.GetWiltLevel(level);
		}

		// Token: 0x06008DF5 RID: 36341 RVA: 0x0031E59D File Offset: 0x0031C79D
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06008DF6 RID: 36342 RVA: 0x0031E5AA File Offset: 0x0031C7AA
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06008DF7 RID: 36343 RVA: 0x0031E5B7 File Offset: 0x0031C7B7
		public bool IsOld(StandardCropPlant.StatesInstance smi)
		{
			return smi.master.growing.PercentOldAge() > 0.5f;
		}

		// Token: 0x06008DF8 RID: 36344 RVA: 0x0031E5D0 File Offset: 0x0031C7D0
		public bool IsSleeping(StandardCropPlant.StatesInstance smi)
		{
			CropSleepingMonitor.Instance smi2 = smi.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi2 != null && smi2.IsSleeping();
		}

		// Token: 0x04006E18 RID: 28184
		public StandardCropPlant.States.AliveStates alive;

		// Token: 0x04006E19 RID: 28185
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;

		// Token: 0x04006E1A RID: 28186
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState blighted;

		// Token: 0x020021CB RID: 8651
		public class AliveStates : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
		{
			// Token: 0x0400975D RID: 38749
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;

			// Token: 0x0400975E RID: 38750
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;

			// Token: 0x0400975F RID: 38751
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;

			// Token: 0x04009760 RID: 38752
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;

			// Token: 0x04009761 RID: 38753
			public StandardCropPlant.States.FruitingState fruiting;

			// Token: 0x04009762 RID: 38754
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;

			// Token: 0x04009763 RID: 38755
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;

			// Token: 0x04009764 RID: 38756
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;

			// Token: 0x04009765 RID: 38757
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State sleeping;
		}

		// Token: 0x020021CC RID: 8652
		public class FruitingState : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State
		{
			// Token: 0x04009766 RID: 38758
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_idle;

			// Token: 0x04009767 RID: 38759
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_old;
		}
	}

	// Token: 0x02001740 RID: 5952
	public class StatesInstance : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance
	{
		// Token: 0x06008DFA RID: 36346 RVA: 0x0031E5FC File Offset: 0x0031C7FC
		public StatesInstance(StandardCropPlant master) : base(master)
		{
		}
	}
}
