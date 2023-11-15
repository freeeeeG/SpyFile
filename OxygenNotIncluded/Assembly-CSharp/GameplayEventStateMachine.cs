using System;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public abstract class GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType> where StateMachineType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType> where StateMachineInstanceType : GameplayEventStateMachine<StateMachineType, StateMachineInstanceType, MasterType, SecondMasterType>.GameplayEventStateMachineInstance where MasterType : IStateMachineTarget where SecondMasterType : GameplayEvent<StateMachineInstanceType>
{
	// Token: 0x06001675 RID: 5749 RVA: 0x00075490 File Offset: 0x00073690
	public void MonitorStart(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1660384580, smi.eventInstance);
		}
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x000754C4 File Offset: 0x000736C4
	public void MonitorChanged(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-1122598290, smi.eventInstance);
		}
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x000754F8 File Offset: 0x000736F8
	public void MonitorStop(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.TargetParameter target, StateMachineInstanceType smi)
	{
		GameObject gameObject = target.Get(smi);
		if (gameObject != null)
		{
			gameObject.Trigger(-828272459, smi.eventInstance);
		}
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0007552C File Offset: 0x0007372C
	public virtual EventInfoData GenerateEventPopupData(StateMachineInstanceType smi)
	{
		return null;
	}

	// Token: 0x020010A6 RID: 4262
	public class GameplayEventStateMachineInstance : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, object>.GameInstance
	{
		// Token: 0x060076E8 RID: 30440 RVA: 0x002D2275 File Offset: 0x002D0475
		public GameplayEventStateMachineInstance(MasterType master, GameplayEventInstance eventInstance, SecondMasterType gameplayEvent) : base(master)
		{
			this.gameplayEvent = gameplayEvent;
			this.eventInstance = eventInstance;
			eventInstance.GetEventPopupData = (() => base.smi.sm.GenerateEventPopupData(base.smi));
			this.serializationSuffix = gameplayEvent.Id;
		}

		// Token: 0x040059BC RID: 22972
		public GameplayEventInstance eventInstance;

		// Token: 0x040059BD RID: 22973
		public SecondMasterType gameplayEvent;
	}
}
