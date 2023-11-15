using System;
using KSerialization;

// Token: 0x020008D2 RID: 2258
[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
	// Token: 0x0600414A RID: 16714 RVA: 0x0016DB03 File Offset: 0x0016BD03
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600414B RID: 16715 RVA: 0x0016DB0C File Offset: 0x0016BD0C
	public void UnassignAll()
	{
		foreach (AssignableSlotInstance assignableSlotInstance in this.slots)
		{
			if (assignableSlotInstance.assignable != null)
			{
				assignableSlotInstance.assignable.Unassign();
			}
		}
	}
}
