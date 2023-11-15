using System;

// Token: 0x02000432 RID: 1074
public static class StateMachineExtensions
{
	// Token: 0x0600169F RID: 5791 RVA: 0x00075B20 File Offset: 0x00073D20
	public static bool IsNullOrStopped(this StateMachine.Instance smi)
	{
		return smi == null || !smi.IsRunning();
	}
}
