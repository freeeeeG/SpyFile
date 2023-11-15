using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200095B RID: 2395
[Serializable]
public class ScheduleBlock
{
	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06004658 RID: 18008 RVA: 0x0018DE60 File Offset: 0x0018C060
	// (set) Token: 0x06004657 RID: 18007 RVA: 0x0018DE57 File Offset: 0x0018C057
	public string GroupId
	{
		get
		{
			if (this._groupId == null)
			{
				this._groupId = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(this.allowed_types).Id;
			}
			return this._groupId;
		}
		set
		{
			this._groupId = value;
		}
	}

	// Token: 0x06004659 RID: 18009 RVA: 0x0018DE90 File Offset: 0x0018C090
	public ScheduleBlock(string name, List<ScheduleBlockType> allowed_types, string groupId)
	{
		this.name = name;
		this.allowed_types = allowed_types;
		this._groupId = groupId;
	}

	// Token: 0x0600465A RID: 18010 RVA: 0x0018DEB0 File Offset: 0x0018C0B0
	public bool IsAllowed(ScheduleBlockType type)
	{
		if (this.allowed_types != null)
		{
			foreach (ScheduleBlockType scheduleBlockType in this.allowed_types)
			{
				if (type.IdHash == scheduleBlockType.IdHash)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04002E9D RID: 11933
	[Serialize]
	public string name;

	// Token: 0x04002E9E RID: 11934
	[Serialize]
	public List<ScheduleBlockType> allowed_types;

	// Token: 0x04002E9F RID: 11935
	[Serialize]
	private string _groupId;
}
