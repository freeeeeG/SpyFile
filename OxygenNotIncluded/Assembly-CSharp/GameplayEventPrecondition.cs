using System;

// Token: 0x020003E5 RID: 997
public class GameplayEventPrecondition
{
	// Token: 0x04000B62 RID: 2914
	public string description;

	// Token: 0x04000B63 RID: 2915
	public GameplayEventPrecondition.PreconditionFn condition;

	// Token: 0x04000B64 RID: 2916
	public bool required;

	// Token: 0x04000B65 RID: 2917
	public int priorityModifier;

	// Token: 0x0200105C RID: 4188
	// (Invoke) Token: 0x0600756D RID: 30061
	public delegate bool PreconditionFn();
}
