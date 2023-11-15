using System;
using System.Diagnostics;

// Token: 0x020008D1 RID: 2257
[DebuggerDisplay("{slot.Id}")]
public class OwnableSlotInstance : AssignableSlotInstance
{
	// Token: 0x06004149 RID: 16713 RVA: 0x0016DAF9 File Offset: 0x0016BCF9
	public OwnableSlotInstance(Assignables assignables, OwnableSlot slot) : base(assignables, slot)
	{
	}
}
