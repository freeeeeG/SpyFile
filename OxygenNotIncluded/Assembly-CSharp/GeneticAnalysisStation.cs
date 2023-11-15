using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000609 RID: 1545
public class GeneticAnalysisStation : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>
{
	// Token: 0x060026D9 RID: 9945 RVA: 0x000D2A90 File Offset: 0x000D0C90
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy));
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Not(new StateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.Transition.ConditionCallback(this.HasSeedToStudy))).ToggleChore(new Func<GeneticAnalysisStation.StatesInstance, Chore>(this.CreateChore), this.operational);
	}

	// Token: 0x060026DA RID: 9946 RVA: 0x000D2B6C File Offset: 0x000D0D6C
	private bool HasSeedToStudy(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.storage.GetMassAvailable(GameTags.UnidentifiedSeed) >= 1f;
	}

	// Token: 0x060026DB RID: 9947 RVA: 0x000D2B88 File Offset: 0x000D0D88
	private bool IsOperational(GeneticAnalysisStation.StatesInstance smi)
	{
		return smi.GetComponent<Operational>().IsOperational;
	}

	// Token: 0x060026DC RID: 9948 RVA: 0x000D2B98 File Offset: 0x000D0D98
	private Chore CreateChore(GeneticAnalysisStation.StatesInstance smi)
	{
		return new WorkChore<GeneticAnalysisStationWorkable>(Db.Get().ChoreTypes.AnalyzeSeed, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04001643 RID: 5699
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State inoperational;

	// Token: 0x04001644 RID: 5700
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State operational;

	// Token: 0x04001645 RID: 5701
	public GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.State ready;

	// Token: 0x020012B4 RID: 4788
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020012B5 RID: 4789
	public class StatesInstance : GameStateMachine<GeneticAnalysisStation, GeneticAnalysisStation.StatesInstance, IStateMachineTarget, GeneticAnalysisStation.Def>.GameInstance
	{
		// Token: 0x06007E40 RID: 32320 RVA: 0x002E7C8E File Offset: 0x002E5E8E
		public StatesInstance(IStateMachineTarget master, GeneticAnalysisStation.Def def) : base(master, def)
		{
			this.workable.statesInstance = this;
		}

		// Token: 0x06007E41 RID: 32321 RVA: 0x002E7CA4 File Offset: 0x002E5EA4
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshFetchTags();
		}

		// Token: 0x06007E42 RID: 32322 RVA: 0x002E7CB4 File Offset: 0x002E5EB4
		public void SetSeedForbidden(Tag seedID, bool forbidden)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			bool flag;
			if (forbidden)
			{
				flag = this.forbiddenSeeds.Add(seedID);
			}
			else
			{
				flag = this.forbiddenSeeds.Remove(seedID);
			}
			if (flag)
			{
				this.RefreshFetchTags();
			}
		}

		// Token: 0x06007E43 RID: 32323 RVA: 0x002E7CFC File Offset: 0x002E5EFC
		public bool GetSeedForbidden(Tag seedID)
		{
			if (this.forbiddenSeeds == null)
			{
				this.forbiddenSeeds = new HashSet<Tag>();
			}
			return this.forbiddenSeeds.Contains(seedID);
		}

		// Token: 0x06007E44 RID: 32324 RVA: 0x002E7D20 File Offset: 0x002E5F20
		private void RefreshFetchTags()
		{
			if (this.forbiddenSeeds == null)
			{
				this.manualDelivery.ForbiddenTags = null;
				return;
			}
			Tag[] array = new Tag[this.forbiddenSeeds.Count];
			int num = 0;
			foreach (Tag tag in this.forbiddenSeeds)
			{
				array[num++] = tag;
				this.storage.Drop(tag);
			}
			this.manualDelivery.ForbiddenTags = array;
		}

		// Token: 0x0400606E RID: 24686
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400606F RID: 24687
		[MyCmpReq]
		public ManualDeliveryKG manualDelivery;

		// Token: 0x04006070 RID: 24688
		[MyCmpReq]
		public GeneticAnalysisStationWorkable workable;

		// Token: 0x04006071 RID: 24689
		[Serialize]
		private HashSet<Tag> forbiddenSeeds;
	}
}
