using System;

// Token: 0x020003E1 RID: 993
public abstract class GameplayEvent<StateMachineInstanceType> : GameplayEvent where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x060014F0 RID: 5360 RVA: 0x0006E93B File Offset: 0x0006CB3B
	public GameplayEvent(string id, int priority = 0, int importance = 0) : base(id, priority, importance)
	{
	}
}
