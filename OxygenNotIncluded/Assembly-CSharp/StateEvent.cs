using System;

// Token: 0x0200042F RID: 1071
public abstract class StateEvent
{
	// Token: 0x06001680 RID: 5760 RVA: 0x00075631 File Offset: 0x00073831
	public StateEvent(string name)
	{
		this.name = name;
		this.debugName = "(Event)" + name;
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x00075651 File Offset: 0x00073851
	public virtual StateEvent.Context Subscribe(StateMachine.Instance smi)
	{
		return new StateEvent.Context(this);
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x00075659 File Offset: 0x00073859
	public virtual void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
	{
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x0007565B File Offset: 0x0007385B
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x00075663 File Offset: 0x00073863
	public string GetDebugName()
	{
		return this.debugName;
	}

	// Token: 0x04000C91 RID: 3217
	protected string name;

	// Token: 0x04000C92 RID: 3218
	private string debugName;

	// Token: 0x020010A7 RID: 4263
	public struct Context
	{
		// Token: 0x060076EA RID: 30442 RVA: 0x002D22D1 File Offset: 0x002D04D1
		public Context(StateEvent state_event)
		{
			this.stateEvent = state_event;
			this.data = 0;
		}

		// Token: 0x040059BE RID: 22974
		public StateEvent stateEvent;

		// Token: 0x040059BF RID: 22975
		public int data;
	}
}
