using System;
using Database;

// Token: 0x02000C64 RID: 3172
public class SkillListable : IListableOption
{
	// Token: 0x060064EB RID: 25835 RVA: 0x00256E20 File Offset: 0x00255020
	public SkillListable(string name)
	{
		this.skillName = name;
		Skill skill = Db.Get().Skills.TryGet(this.skillName);
		if (skill != null)
		{
			this.name = skill.Name;
			this.skillHat = skill.hat;
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x060064EC RID: 25836 RVA: 0x00256E70 File Offset: 0x00255070
	// (set) Token: 0x060064ED RID: 25837 RVA: 0x00256E78 File Offset: 0x00255078
	public string skillName { get; private set; }

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x060064EE RID: 25838 RVA: 0x00256E81 File Offset: 0x00255081
	// (set) Token: 0x060064EF RID: 25839 RVA: 0x00256E89 File Offset: 0x00255089
	public string skillHat { get; private set; }

	// Token: 0x060064F0 RID: 25840 RVA: 0x00256E92 File Offset: 0x00255092
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x0400451A RID: 17690
	public LocString name;
}
