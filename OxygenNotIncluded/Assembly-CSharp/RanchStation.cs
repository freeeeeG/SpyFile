using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000679 RID: 1657
public class RanchStation : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
	// Token: 0x06002C06 RID: 11270 RVA: 0x000E9E84 File Offset: 0x000E8084
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Operational;
		this.Unoperational.TagTransition(GameTags.Operational, this.Operational, false);
		this.Operational.TagTransition(GameTags.Operational, this.Unoperational, true).ToggleChore((RanchStation.Instance smi) => smi.CreateChore(), this.Unoperational, this.Unoperational).Update("FindRanachable", delegate(RanchStation.Instance smi, float dt)
		{
			smi.FindRanchable(null);
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040019FE RID: 6654
	public StateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.BoolParameter RancherIsReady;

	// Token: 0x040019FF RID: 6655
	public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State Unoperational;

	// Token: 0x04001A00 RID: 6656
	public RanchStation.OperationalState Operational;

	// Token: 0x0200137A RID: 4986
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006289 RID: 25225
		public Func<GameObject, RanchStation.Instance, bool> IsCritterEligibleToBeRanchedCb;

		// Token: 0x0400628A RID: 25226
		public Action<GameObject> OnRanchCompleteCb;

		// Token: 0x0400628B RID: 25227
		public Action<GameObject, float, Workable> OnRanchWorkTick;

		// Token: 0x0400628C RID: 25228
		public HashedString RanchedPreAnim = "idle_loop";

		// Token: 0x0400628D RID: 25229
		public HashedString RanchedLoopAnim = "idle_loop";

		// Token: 0x0400628E RID: 25230
		public HashedString RanchedPstAnim = "idle_loop";

		// Token: 0x0400628F RID: 25231
		public HashedString RanchedAbortAnim = "idle_loop";

		// Token: 0x04006290 RID: 25232
		public HashedString RancherInteractAnim = "anim_interacts_rancherstation_kanim";

		// Token: 0x04006291 RID: 25233
		public StatusItem RanchingStatusItem = Db.Get().DuplicantStatusItems.Ranching;

		// Token: 0x04006292 RID: 25234
		public StatusItem CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;

		// Token: 0x04006293 RID: 25235
		public float WorkTime = 12f;

		// Token: 0x04006294 RID: 25236
		public Func<RanchStation.Instance, int> GetTargetRanchCell = (RanchStation.Instance smi) => Grid.PosToCell(smi);
	}

	// Token: 0x0200137B RID: 4987
	public class OperationalState : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
	{
	}

	// Token: 0x0200137C RID: 4988
	public new class Instance : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x0600812E RID: 33070 RVA: 0x002F4251 File Offset: 0x002F2451
		public RanchedStates.Instance ActiveRanchable
		{
			get
			{
				return this.activeRanchable;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x0600812F RID: 33071 RVA: 0x002F4259 File Offset: 0x002F2459
		private bool isCritterAvailableForRanching
		{
			get
			{
				return this.targetRanchables.Count > 0;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06008130 RID: 33072 RVA: 0x002F4269 File Offset: 0x002F2469
		public bool IsCritterAvailableForRanching
		{
			get
			{
				this.ValidateTargetRanchables();
				return this.isCritterAvailableForRanching;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06008131 RID: 33073 RVA: 0x002F4277 File Offset: 0x002F2477
		public bool HasRancher
		{
			get
			{
				return this.rancher != null;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06008132 RID: 33074 RVA: 0x002F4285 File Offset: 0x002F2485
		public bool IsRancherReady
		{
			get
			{
				return base.sm.RancherIsReady.Get(this);
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06008133 RID: 33075 RVA: 0x002F4298 File Offset: 0x002F2498
		public Extents StationExtents
		{
			get
			{
				return this.station.GetExtents();
			}
		}

		// Token: 0x06008134 RID: 33076 RVA: 0x002F42A5 File Offset: 0x002F24A5
		public int GetRanchNavTarget()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06008135 RID: 33077 RVA: 0x002F42B8 File Offset: 0x002F24B8
		public Instance(IStateMachineTarget master, RanchStation.Def def) : base(master, def)
		{
			base.gameObject.AddOrGet<RancherChore.RancherWorkable>();
			this.station = base.GetComponent<BuildingComplete>();
		}

		// Token: 0x06008136 RID: 33078 RVA: 0x002F42E8 File Offset: 0x002F24E8
		public Chore CreateChore()
		{
			RancherChore rancherChore = new RancherChore(base.GetComponent<KPrefabID>());
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter targetParameter = rancherChore.smi.sm.rancher;
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Parameter<GameObject>.Context context = targetParameter.GetContext(rancherChore.smi);
			context.onDirty = (Action<RancherChore.RancherChoreStates.Instance>)Delegate.Combine(context.onDirty, new Action<RancherChore.RancherChoreStates.Instance>(this.OnRancherChanged));
			this.rancher = targetParameter.Get<Worker>(rancherChore.smi);
			return rancherChore;
		}

		// Token: 0x06008137 RID: 33079 RVA: 0x002F4352 File Offset: 0x002F2552
		public int GetTargetRanchCell()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06008138 RID: 33080 RVA: 0x002F4368 File Offset: 0x002F2568
		public override void StartSM()
		{
			base.StartSM();
			base.Subscribe(144050788, new Action<object>(this.OnRoomUpdated));
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.GetTargetRanchCell());
			if (cavityForCell != null && cavityForCell.room != null)
			{
				this.OnRoomUpdated(cavityForCell.room);
			}
		}

		// Token: 0x06008139 RID: 33081 RVA: 0x002F43BF File Offset: 0x002F25BF
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			base.Unsubscribe(144050788, new Action<object>(this.OnRoomUpdated));
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x002F43DF File Offset: 0x002F25DF
		private void OnRoomUpdated(object data)
		{
			if (data == null)
			{
				return;
			}
			this.ranch = (data as Room);
			if (this.ranch.roomType != Db.Get().RoomTypes.CreaturePen)
			{
				this.TriggerRanchStationNoLongerAvailable();
				this.ranch = null;
			}
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x002F441A File Offset: 0x002F261A
		private void OnRancherChanged(RancherChore.RancherChoreStates.Instance choreInstance)
		{
			this.rancher = choreInstance.sm.rancher.Get<Worker>(choreInstance);
			this.TriggerRanchStationNoLongerAvailable();
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x002F4439 File Offset: 0x002F2639
		public bool TryGetRanched(RanchedStates.Instance ranchable)
		{
			return this.activeRanchable == null || this.activeRanchable == ranchable;
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x002F444E File Offset: 0x002F264E
		public void MessageCreatureArrived(RanchedStates.Instance critter)
		{
			this.activeRanchable = critter;
			base.sm.RancherIsReady.Set(false, this, false);
			base.Trigger(-1357116271, null);
		}

		// Token: 0x0600813E RID: 33086 RVA: 0x002F4477 File Offset: 0x002F2677
		public void MessageRancherReady()
		{
			base.sm.RancherIsReady.Set(true, base.smi, false);
			this.MessageRanchables(GameHashes.RancherReadyAtRanchStation);
		}

		// Token: 0x0600813F RID: 33087 RVA: 0x002F44A0 File Offset: 0x002F26A0
		private bool CanRanchableBeRanchedAtRanchStation(RanchableMonitor.Instance ranchable)
		{
			bool flag = !ranchable.IsNullOrStopped();
			if (flag && ranchable.TargetRanchStation != null && ranchable.TargetRanchStation != this)
			{
				flag = (!ranchable.TargetRanchStation.IsRunning() || !ranchable.TargetRanchStation.HasRancher);
			}
			flag = (flag && base.def.IsCritterEligibleToBeRanchedCb(ranchable.gameObject, this));
			flag = (flag && ranchable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>());
			if (flag)
			{
				int cell = Grid.PosToCell(ranchable.transform.GetPosition());
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
				if (cavityForCell == null || this.ranch == null || cavityForCell != this.ranch.cavity)
				{
					flag = false;
				}
				else
				{
					int cell2 = this.GetRanchNavTarget();
					if (ranchable.HasTag(GameTags.Creatures.Flyer))
					{
						cell2 = Grid.CellAbove(cell2);
					}
					flag = (ranchable.NavComponent.GetNavigationCost(cell2) != -1);
				}
			}
			return flag;
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x002F458C File Offset: 0x002F278C
		public void ValidateTargetRanchables()
		{
			if (!this.HasRancher)
			{
				return;
			}
			foreach (RanchableMonitor.Instance instance in this.targetRanchables.ToArray())
			{
				if (instance.States == null || !this.CanRanchableBeRanchedAtRanchStation(instance))
				{
					this.Abandon(instance);
				}
			}
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x002F45D8 File Offset: 0x002F27D8
		public void FindRanchable(object _ = null)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.ValidateTargetRanchables();
			if (this.targetRanchables.Count == 2)
			{
				return;
			}
			List<KPrefabID> creatures = this.ranch.cavity.creatures;
			if (this.HasRancher && !this.isCritterAvailableForRanching && creatures.Count == 0)
			{
				this.TryNotifyEmptyRanch();
			}
			for (int i = 0; i < creatures.Count; i++)
			{
				KPrefabID kprefabID = creatures[i];
				if (!(kprefabID == null))
				{
					RanchableMonitor.Instance smi = kprefabID.GetSMI<RanchableMonitor.Instance>();
					if (!this.targetRanchables.Contains(smi) && this.CanRanchableBeRanchedAtRanchStation(smi) && smi != null)
					{
						smi.States.SetRanchStation(this);
						this.targetRanchables.Add(smi);
						return;
					}
				}
			}
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x002F468E File Offset: 0x002F288E
		public Option<CavityInfo> GetCavityInfo()
		{
			if (this.ranch.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return this.ranch.cavity;
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x002F46B8 File Offset: 0x002F28B8
		public void RanchCreature()
		{
			if (this.activeRanchable.IsNullOrStopped())
			{
				return;
			}
			global::Debug.Assert(this.activeRanchable != null, "targetRanchable was null");
			global::Debug.Assert(this.activeRanchable.GetMaster() != null, "GetMaster was null");
			global::Debug.Assert(base.def != null, "def was null");
			global::Debug.Assert(base.def.OnRanchCompleteCb != null, "onRanchCompleteCb cb was null");
			base.def.OnRanchCompleteCb(this.activeRanchable.gameObject);
			this.targetRanchables.Remove(this.activeRanchable.Monitor);
			this.activeRanchable.Trigger(1827504087, null);
			this.activeRanchable = null;
			this.FindRanchable(null);
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x002F477C File Offset: 0x002F297C
		public void TriggerRanchStationNoLongerAvailable()
		{
			for (int i = this.targetRanchables.Count - 1; i >= 0; i--)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (!instance.IsNullOrStopped() && !instance.States.IsNullOrStopped())
				{
					this.targetRanchables.Remove(instance);
					instance.Trigger(1689625967, null);
				}
			}
			base.sm.RancherIsReady.Set(false, this, false);
		}

		// Token: 0x06008145 RID: 33093 RVA: 0x002F47F0 File Offset: 0x002F29F0
		public void MessageRanchables(GameHashes hash)
		{
			for (int i = 0; i < this.targetRanchables.Count; i++)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (!instance.IsNullOrStopped())
				{
					Game.BrainScheduler.PrioritizeBrain(instance.GetComponent<CreatureBrain>());
					if (!instance.States.IsNullOrStopped())
					{
						instance.Trigger((int)hash, null);
					}
				}
			}
		}

		// Token: 0x06008146 RID: 33094 RVA: 0x002F4850 File Offset: 0x002F2A50
		public void Abandon(RanchableMonitor.Instance critter)
		{
			if (critter == null)
			{
				global::Debug.LogWarning("Null critter trying to abandon ranch station");
				this.targetRanchables.Remove(critter);
				return;
			}
			critter.TargetRanchStation = null;
			if (this.targetRanchables.Remove(critter))
			{
				if (critter.States == null)
				{
					return;
				}
				bool flag = !this.isCritterAvailableForRanching;
				if (critter.States == this.activeRanchable)
				{
					flag = true;
					this.activeRanchable = null;
				}
				if (flag)
				{
					this.TryNotifyEmptyRanch();
				}
			}
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x002F48C0 File Offset: 0x002F2AC0
		private void TryNotifyEmptyRanch()
		{
			if (!this.HasRancher)
			{
				return;
			}
			this.rancher.Trigger(-364750427, null);
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x002F48DC File Offset: 0x002F2ADC
		public bool IsCritterInQueue(RanchableMonitor.Instance critter)
		{
			return this.targetRanchables.Contains(critter);
		}

		// Token: 0x06008149 RID: 33097 RVA: 0x002F48EA File Offset: 0x002F2AEA
		public List<RanchableMonitor.Instance> DEBUG_GetTargetRanchables()
		{
			return this.targetRanchables;
		}

		// Token: 0x04006295 RID: 25237
		private const int QUEUE_SIZE = 2;

		// Token: 0x04006296 RID: 25238
		private List<RanchableMonitor.Instance> targetRanchables = new List<RanchableMonitor.Instance>();

		// Token: 0x04006297 RID: 25239
		private RanchedStates.Instance activeRanchable;

		// Token: 0x04006298 RID: 25240
		private Room ranch;

		// Token: 0x04006299 RID: 25241
		private Worker rancher;

		// Token: 0x0400629A RID: 25242
		private BuildingComplete station;
	}
}
