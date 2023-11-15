using System;

// Token: 0x02000B6A RID: 2922
public interface IPersonalPriorityManager
{
	// Token: 0x06005A57 RID: 23127
	int GetAssociatedSkillLevel(ChoreGroup group);

	// Token: 0x06005A58 RID: 23128
	int GetPersonalPriority(ChoreGroup group);

	// Token: 0x06005A59 RID: 23129
	void SetPersonalPriority(ChoreGroup group, int value);

	// Token: 0x06005A5A RID: 23130
	bool IsChoreGroupDisabled(ChoreGroup group);

	// Token: 0x06005A5B RID: 23131
	void ResetPersonalPriorities();
}
