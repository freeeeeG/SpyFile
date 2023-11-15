using System;

// Token: 0x020003DD RID: 989
public abstract class Usable : KMonoBehaviour, IStateMachineTarget
{
	// Token: 0x060014CC RID: 5324
	public abstract void StartUsing(User user);

	// Token: 0x060014CD RID: 5325 RVA: 0x0006E128 File Offset: 0x0006C328
	protected void StartUsing(StateMachine.Instance smi, User user)
	{
		DebugUtil.Assert(this.smi == null);
		DebugUtil.Assert(smi != null);
		this.smi = smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
		smi.StartSM();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x0006E17C File Offset: 0x0006C37C
	public void StopUsing(User user)
	{
		if (this.smi != null)
		{
			StateMachine.Instance instance = this.smi;
			instance.OnStop = (Action<string, StateMachine.Status>)Delegate.Remove(instance.OnStop, new Action<string, StateMachine.Status>(user.OnStateMachineStop));
			this.smi.StopSM("Usable.StopUsing");
			this.smi = null;
		}
	}

	// Token: 0x04000B3A RID: 2874
	private StateMachine.Instance smi;
}
