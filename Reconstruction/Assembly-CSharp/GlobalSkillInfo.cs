using System;

// Token: 0x02000061 RID: 97
public class GlobalSkillInfo
{
	// Token: 0x06000272 RID: 626 RVA: 0x0000887C File Offset: 0x00006A7C
	public GlobalSkillInfo(GlobalSkillName skillName, bool isAbnomral)
	{
		this.SkillName = skillName;
		this.IsAbnormal = isAbnomral;
	}

	// Token: 0x04000165 RID: 357
	public GlobalSkillName SkillName;

	// Token: 0x04000166 RID: 358
	public bool IsAbnormal;
}
