using System;
using KSerialization;

// Token: 0x02000549 RID: 1353
[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000AFB5E File Offset: 0x000ADD5E
	// (set) Token: 0x060020F2 RID: 8434 RVA: 0x000AFB75 File Offset: 0x000ADD75
	public ScheduleGroup scheduleGroup
	{
		get
		{
			return Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
		}
		set
		{
			this.scheduleGroupID = value.Id;
		}
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x000AFB83 File Offset: 0x000ADD83
	public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
	{
		this.scheduleGroup = scheduleGroup;
		this.segments = scheduleGroup.defaultSegments;
	}

	// Token: 0x04001299 RID: 4761
	[Serialize]
	private string scheduleGroupID;

	// Token: 0x0400129A RID: 4762
	[Serialize]
	public int segments;
}
