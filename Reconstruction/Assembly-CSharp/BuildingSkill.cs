using System;

// Token: 0x02000049 RID: 73
public abstract class BuildingSkill : TurretSkill
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060001D1 RID: 465
	public abstract BuildingSkillName BuildingSkillName { get; }

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060001D2 RID: 466 RVA: 0x00007534 File Offset: 0x00005734
	// (set) Token: 0x060001D3 RID: 467 RVA: 0x0000753C File Offset: 0x0000573C
	public virtual bool IsAbnormalBuilding { get; set; }

	// Token: 0x060001D4 RID: 468 RVA: 0x00007545 File Offset: 0x00005745
	public virtual void MainFuncCallBack()
	{
	}
}
