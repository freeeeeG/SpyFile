using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A22 RID: 2594
[SerializationConfig(MemberSerialization.OptIn)]
public class VerticalWindTunnel : StateMachineComponent<VerticalWindTunnel.StatesInstance>, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x06004DBA RID: 19898 RVA: 0x001B3C60 File Offset: 0x001B1E60
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ElementConsumer[] components = base.GetComponents<ElementConsumer>();
		this.bottomConsumer = components[0];
		this.bottomConsumer.EnableConsumption(false);
		this.bottomConsumer.OnElementConsumed += delegate(Sim.ConsumedMassInfo info)
		{
			this.OnElementConsumed(false, info);
		};
		this.topConsumer = components[1];
		this.topConsumer.EnableConsumption(false);
		this.topConsumer.OnElementConsumed += delegate(Sim.ConsumedMassInfo info)
		{
			this.OnElementConsumed(true, info);
		};
		this.operational = base.GetComponent<Operational>();
	}

	// Token: 0x06004DBB RID: 19899 RVA: 0x001B3CE0 File Offset: 0x001B1EE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.invalidIntake = this.HasInvalidIntake();
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.WindTunnelIntake, this.invalidIntake, this);
		this.operational.SetFlag(VerticalWindTunnel.validIntakeFlag, !this.invalidIntake);
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new VerticalWindTunnelWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("VerticalWindTunnelWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			VerticalWindTunnelWorkable verticalWindTunnelWorkable = go.AddOrGet<VerticalWindTunnelWorkable>();
			int player_index = i;
			VerticalWindTunnelWorkable verticalWindTunnelWorkable2 = verticalWindTunnelWorkable;
			verticalWindTunnelWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(verticalWindTunnelWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			verticalWindTunnelWorkable.overrideAnim = this.overrideAnims[i];
			verticalWindTunnelWorkable.preAnims = this.workPreAnims[i];
			verticalWindTunnelWorkable.loopAnim = this.workAnims[i];
			verticalWindTunnelWorkable.pstAnims = this.workPstAnims[i];
			this.workables[i] = verticalWindTunnelWorkable;
			this.workables[i].windTunnel = this;
		}
		base.smi.StartSM();
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001B3E8C File Offset: 0x001B208C
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06004DBD RID: 19901 RVA: 0x001B3EE0 File Offset: 0x001B20E0
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<VerticalWindTunnelWorkable> workChore = new WorkChore<VerticalWindTunnelWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x001B3F48 File Offset: 0x001B2148
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001B3F64 File Offset: 0x001B2164
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001B3FC4 File Offset: 0x001B21C4
	public void Sim200ms(float dt)
	{
		bool flag = this.HasInvalidIntake();
		if (flag != this.invalidIntake)
		{
			this.invalidIntake = flag;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.WindTunnelIntake, this.invalidIntake, this);
			this.operational.SetFlag(VerticalWindTunnel.validIntakeFlag, !this.invalidIntake);
		}
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001B4024 File Offset: 0x001B2224
	private float GetIntakeRatio(int fromCell, int radius)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = -radius; i < radius; i++)
		{
			for (int j = -radius; j < radius; j++)
			{
				int cell = Grid.OffsetCell(fromCell, j, i);
				if (!Grid.IsSolidCell(cell))
				{
					if (Grid.IsGas(cell))
					{
						num2 += 1f;
					}
					num += 1f;
				}
			}
		}
		return num2 / num;
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001B4088 File Offset: 0x001B2288
	private bool HasInvalidIntake()
	{
		Vector3 position = base.transform.GetPosition();
		int cell = Grid.XYToCell((int)position.x, (int)position.y);
		int fromCell = Grid.OffsetCell(cell, (int)this.topConsumer.sampleCellOffset.x, (int)this.topConsumer.sampleCellOffset.y);
		int fromCell2 = Grid.OffsetCell(cell, (int)this.bottomConsumer.sampleCellOffset.x, (int)this.bottomConsumer.sampleCellOffset.y);
		this.avgGasAccumTop += this.GetIntakeRatio(fromCell, (int)this.topConsumer.consumptionRadius);
		this.avgGasAccumBottom += this.GetIntakeRatio(fromCell2, (int)this.bottomConsumer.consumptionRadius);
		int num = 5;
		this.avgGasCounter = (this.avgGasCounter + 1) % num;
		if (this.avgGasCounter == 0)
		{
			double num2 = (double)(this.avgGasAccumTop / (float)num);
			float num3 = this.avgGasAccumBottom / (float)num;
			this.avgGasAccumBottom = 0f;
			this.avgGasAccumTop = 0f;
			return num2 < 0.5 || (double)num3 < 0.5;
		}
		return this.invalidIntake;
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001B41AC File Offset: 0x001B23AC
	public void SetGasWalls(bool set)
	{
		Building component = base.GetComponent<Building>();
		Sim.Cell.Properties properties = (Sim.Cell.Properties)3;
		Vector3 position = base.transform.GetPosition();
		for (int i = 0; i < component.Def.HeightInCells; i++)
		{
			int gameCell = Grid.XYToCell(Mathf.FloorToInt(position.x) - 2, Mathf.FloorToInt(position.y) + i);
			int gameCell2 = Grid.XYToCell(Mathf.FloorToInt(position.x) + 2, Mathf.FloorToInt(position.y) + i);
			if (set)
			{
				SimMessages.SetCellProperties(gameCell, (byte)properties);
				SimMessages.SetCellProperties(gameCell2, (byte)properties);
			}
			else
			{
				SimMessages.ClearCellProperties(gameCell, (byte)properties);
				SimMessages.ClearCellProperties(gameCell2, (byte)properties);
			}
		}
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001B4250 File Offset: 0x001B2450
	private void OnElementConsumed(bool isTop, Sim.ConsumedMassInfo info)
	{
		Building component = base.GetComponent<Building>();
		Vector3 position = base.transform.GetPosition();
		CellOffset offset = isTop ? new CellOffset(0, component.Def.HeightInCells + 1) : new CellOffset(0, 0);
		SimMessages.AddRemoveSubstance(Grid.OffsetCell(Grid.XYToCell((int)position.x, (int)position.y), offset), info.removedElemIdx, CellEventLogger.Instance.ElementEmitted, info.mass, info.temperature, info.diseaseIdx, info.diseaseCount, true, -1);
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x001B42D8 File Offset: 0x001B24D8
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.players.Add(player);
		}
		else
		{
			this.players.Remove(player);
		}
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x001B4330 File Offset: 0x001B2530
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(BUILDINGS.PREFABS.VERTICALWINDTUNNEL.DISPLACEMENTEFFECT.Replace("{amount}", GameUtil.GetFormattedMass(this.displacementAmount_DescriptorOnly, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.VERTICALWINDTUNNEL.DISPLACEMENTEFFECT_TOOLTIP.Replace("{amount}", GameUtil.GetFormattedMass(this.displacementAmount_DescriptorOnly, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x0400329D RID: 12957
	public string specificEffect;

	// Token: 0x0400329E RID: 12958
	public string trackingEffect;

	// Token: 0x0400329F RID: 12959
	public int basePriority;

	// Token: 0x040032A0 RID: 12960
	public float displacementAmount_DescriptorOnly;

	// Token: 0x040032A1 RID: 12961
	public static readonly Operational.Flag validIntakeFlag = new Operational.Flag("valid_intake", Operational.Flag.Type.Requirement);

	// Token: 0x040032A2 RID: 12962
	private bool invalidIntake;

	// Token: 0x040032A3 RID: 12963
	private float avgGasAccumTop;

	// Token: 0x040032A4 RID: 12964
	private float avgGasAccumBottom;

	// Token: 0x040032A5 RID: 12965
	private int avgGasCounter;

	// Token: 0x040032A6 RID: 12966
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x040032A7 RID: 12967
	private VerticalWindTunnelWorkable[] workables;

	// Token: 0x040032A8 RID: 12968
	private Chore[] chores;

	// Token: 0x040032A9 RID: 12969
	private ElementConsumer bottomConsumer;

	// Token: 0x040032AA RID: 12970
	private ElementConsumer topConsumer;

	// Token: 0x040032AB RID: 12971
	private Operational operational;

	// Token: 0x040032AC RID: 12972
	public HashSet<int> players = new HashSet<int>();

	// Token: 0x040032AD RID: 12973
	public HashedString[] overrideAnims = new HashedString[]
	{
		"anim_interacts_windtunnel_center_kanim",
		"anim_interacts_windtunnel_left_kanim",
		"anim_interacts_windtunnel_right_kanim"
	};

	// Token: 0x040032AE RID: 12974
	public string[][] workPreAnims = new string[][]
	{
		new string[]
		{
			"weak_working_front_pre",
			"weak_working_back_pre"
		},
		new string[]
		{
			"medium_working_front_pre",
			"medium_working_back_pre"
		},
		new string[]
		{
			"strong_working_front_pre",
			"strong_working_back_pre"
		}
	};

	// Token: 0x040032AF RID: 12975
	public string[] workAnims = new string[]
	{
		"weak_working_loop",
		"medium_working_loop",
		"strong_working_loop"
	};

	// Token: 0x040032B0 RID: 12976
	public string[][] workPstAnims = new string[][]
	{
		new string[]
		{
			"weak_working_back_pst",
			"weak_working_front_pst"
		},
		new string[]
		{
			"medium_working_back_pst",
			"medium_working_front_pst"
		},
		new string[]
		{
			"strong_working_back_pst",
			"strong_working_front_pst"
		}
	};

	// Token: 0x020018A9 RID: 6313
	public class States : GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel>
	{
		// Token: 0x06009261 RID: 37473 RVA: 0x0032BC48 File Offset: 0x00329E48
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.PlayAnim("off").ParamTransition<int>(this.playerCount, this.operational.pre, (VerticalWindTunnel.StatesInstance smi, int p) => p > 0);
			this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter(delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.SetActive(true);
			}).Exit(delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.SetActive(false);
			}).ParamTransition<int>(this.playerCount, this.operational.post, (VerticalWindTunnel.StatesInstance smi, int p) => p == 0).Enter("GasWalls", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.SetGasWalls(true);
			}).Exit("GasWalls", delegate(VerticalWindTunnel.StatesInstance smi)
			{
				smi.master.SetGasWalls(false);
			});
			this.operational.post.PlayAnim("working_pst").QueueAnim("off_pre", false, null).OnAnimQueueComplete(this.operational.stopped);
		}

		// Token: 0x04007299 RID: 29337
		public StateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.IntParameter playerCount;

		// Token: 0x0400729A RID: 29338
		public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State unoperational;

		// Token: 0x0400729B RID: 29339
		public VerticalWindTunnel.States.OperationalStates operational;

		// Token: 0x02002205 RID: 8709
		public class OperationalStates : GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State
		{
			// Token: 0x04009847 RID: 38983
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State stopped;

			// Token: 0x04009848 RID: 38984
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State pre;

			// Token: 0x04009849 RID: 38985
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State playing;

			// Token: 0x0400984A RID: 38986
			public GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.State post;
		}
	}

	// Token: 0x020018AA RID: 6314
	public class StatesInstance : GameStateMachine<VerticalWindTunnel.States, VerticalWindTunnel.StatesInstance, VerticalWindTunnel, object>.GameInstance
	{
		// Token: 0x06009263 RID: 37475 RVA: 0x0032BEAE File Offset: 0x0032A0AE
		public StatesInstance(VerticalWindTunnel smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x06009264 RID: 37476 RVA: 0x0032BEC8 File Offset: 0x0032A0C8
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x0400729C RID: 29340
		private Operational operational;
	}
}
