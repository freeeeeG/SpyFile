using System;
using System.Collections.Generic;

// Token: 0x02000659 RID: 1625
public class MissionControlCluster : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>
{
	// Token: 0x06002AD1 RID: 10961 RVA: 0x000E487C File Offset: 0x000E2A7C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inoperational;
		this.Inoperational.EventTransition(GameHashes.OperationalChanged, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Operational.EventTransition(GameHashes.OperationalChanged, this.Inoperational, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition)).EventTransition(GameHashes.UpdateRoom, this.Operational.WrongRoom, GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Not(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom))).Enter(new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State.Callback(this.OnEnterOperational)).DefaultState(this.Operational.NoRockets).Update(delegate(MissionControlCluster.Instance smi, float dt)
		{
			smi.UpdateWorkableRocketsInRange(null);
		}, UpdateRate.SIM_1000ms, false);
		this.Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, this.Operational.NoRockets, new StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.Transition.ConditionCallback(this.IsInLabRoom));
		this.Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlClusterBoost, null).ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.HasRockets, (MissionControlCluster.Instance smi, bool inRange) => this.WorkableRocketsAreInRange.Get(smi));
		this.Operational.HasRockets.ParamTransition<bool>(this.WorkableRocketsAreInRange, this.Operational.NoRockets, (MissionControlCluster.Instance smi, bool inRange) => !this.WorkableRocketsAreInRange.Get(smi)).ToggleChore(new Func<MissionControlCluster.Instance, Chore>(this.CreateChore), this.Operational);
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000E4A18 File Offset: 0x000E2C18
	private Chore CreateChore(MissionControlCluster.Instance smi)
	{
		MissionControlClusterWorkable component = smi.master.gameObject.GetComponent<MissionControlClusterWorkable>();
		Chore result = new WorkChore<MissionControlClusterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		Clustercraft randomBoostableClustercraft = smi.GetRandomBoostableClustercraft();
		component.TargetClustercraft = randomBoostableClustercraft;
		return result;
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x000E4A6A File Offset: 0x000E2C6A
	private void OnEnterOperational(MissionControlCluster.Instance smi)
	{
		smi.UpdateWorkableRocketsInRange(null);
		if (this.WorkableRocketsAreInRange.Get(smi))
		{
			smi.GoTo(this.Operational.HasRockets);
			return;
		}
		smi.GoTo(this.Operational.NoRockets);
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x000E4AA4 File Offset: 0x000E2CA4
	private bool ValidateOperationalTransition(MissionControlCluster.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Operational);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x06002AD5 RID: 10965 RVA: 0x000E4AE1 File Offset: 0x000E2CE1
	private bool IsInLabRoom(MissionControlCluster.Instance smi)
	{
		return smi.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x0400190B RID: 6411
	public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State Inoperational;

	// Token: 0x0400190C RID: 6412
	public MissionControlCluster.OperationalState Operational;

	// Token: 0x0400190D RID: 6413
	public StateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.BoolParameter WorkableRocketsAreInRange;

	// Token: 0x02001340 RID: 4928
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001341 RID: 4929
	public new class Instance : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.GameInstance
	{
		// Token: 0x06008077 RID: 32887 RVA: 0x002F1794 File Offset: 0x002EF994
		public Instance(IStateMachineTarget master, MissionControlCluster.Def def) : base(master, def)
		{
		}

		// Token: 0x06008078 RID: 32888 RVA: 0x002F17B0 File Offset: 0x002EF9B0
		public override void StartSM()
		{
			base.StartSM();
			this.clusterUpdatedHandle = Game.Instance.Subscribe(-1298331547, new Action<object>(this.UpdateWorkableRocketsInRange));
		}

		// Token: 0x06008079 RID: 32889 RVA: 0x002F17D9 File Offset: 0x002EF9D9
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Game.Instance.Unsubscribe(this.clusterUpdatedHandle);
		}

		// Token: 0x0600807A RID: 32890 RVA: 0x002F17F4 File Offset: 0x002EF9F4
		public void UpdateWorkableRocketsInRange(object data)
		{
			this.boostableClustercraft.Clear();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			for (int i = 0; i < Components.Clustercrafts.Count; i++)
			{
				if (ClusterGrid.Instance.IsInRange(Components.Clustercrafts[i].Location, myWorldLocation, 2) && !this.IsOwnWorld(Components.Clustercrafts[i]) && this.CanBeBoosted(Components.Clustercrafts[i]))
				{
					bool flag = false;
					foreach (object obj in Components.MissionControlClusterWorkables)
					{
						MissionControlClusterWorkable missionControlClusterWorkable = (MissionControlClusterWorkable)obj;
						if (!(missionControlClusterWorkable.gameObject == base.gameObject) && missionControlClusterWorkable.TargetClustercraft == Components.Clustercrafts[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.boostableClustercraft.Add(Components.Clustercrafts[i]);
					}
				}
			}
			base.sm.WorkableRocketsAreInRange.Set(this.boostableClustercraft.Count > 0, base.smi, false);
		}

		// Token: 0x0600807B RID: 32891 RVA: 0x002F193C File Offset: 0x002EFB3C
		public Clustercraft GetRandomBoostableClustercraft()
		{
			return this.boostableClustercraft.GetRandom<Clustercraft>();
		}

		// Token: 0x0600807C RID: 32892 RVA: 0x002F1949 File Offset: 0x002EFB49
		private bool CanBeBoosted(Clustercraft clustercraft)
		{
			return clustercraft.controlStationBuffTimeRemaining == 0f && clustercraft.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && clustercraft.IsFlightInProgress();
		}

		// Token: 0x0600807D RID: 32893 RVA: 0x002F1970 File Offset: 0x002EFB70
		private bool IsOwnWorld(Clustercraft candidateClustercraft)
		{
			int myWorldId = base.gameObject.GetMyWorldId();
			WorldContainer interiorWorld = candidateClustercraft.ModuleInterface.GetInteriorWorld();
			return !(interiorWorld == null) && myWorldId == interiorWorld.id;
		}

		// Token: 0x0600807E RID: 32894 RVA: 0x002F19A9 File Offset: 0x002EFBA9
		public void ApplyEffect(Clustercraft clustercraft)
		{
			clustercraft.controlStationBuffTimeRemaining = 600f;
		}

		// Token: 0x0400620B RID: 25099
		private int clusterUpdatedHandle = -1;

		// Token: 0x0400620C RID: 25100
		private List<Clustercraft> boostableClustercraft = new List<Clustercraft>();

		// Token: 0x0400620D RID: 25101
		[MyCmpReq]
		public RoomTracker roomTracker;
	}

	// Token: 0x02001342 RID: 4930
	public class OperationalState : GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State
	{
		// Token: 0x0400620E RID: 25102
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State WrongRoom;

		// Token: 0x0400620F RID: 25103
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State NoRockets;

		// Token: 0x04006210 RID: 25104
		public GameStateMachine<MissionControlCluster, MissionControlCluster.Instance, IStateMachineTarget, MissionControlCluster.Def>.State HasRockets;
	}
}
