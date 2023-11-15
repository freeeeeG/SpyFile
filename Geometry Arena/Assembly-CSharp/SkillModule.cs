using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
[Serializable]
public class SkillModule
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000255 RID: 597 RVA: 0x0000DDAD File Offset: 0x0000BFAD
	public string Language_Name
	{
		get
		{
			return this.skillModuleNames[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000256 RID: 598 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
	public string Language_Info
	{
		get
		{
			return this.skillModuleInfos[(int)Setting.Inst.language];
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0000DDD3 File Offset: 0x0000BFD3
	public string GetTotalInfo()
	{
		return this.Language_Info.GetColorfulInfoWithFacs(this.facs, false);
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0000DDE8 File Offset: 0x0000BFE8
	public static SkillModule GetSkillModule_CurrentJobWithEffectID(int effectID)
	{
		SkillModule skillModule = null;
		foreach (SkillModule skillModule2 in DataBase.Inst.DataPlayerModels[TempData.inst.jobId].skillModules)
		{
			if (skillModule2.effectID == effectID)
			{
				if (skillModule != null)
				{
					Debug.LogError("Error_技能模块EffectID重复！");
				}
				skillModule = skillModule2;
			}
		}
		if (skillModule == null)
		{
			Debug.LogError("Error_技能模块找不到");
			return DataBase.Inst.DataPlayerModels[TempData.inst.jobId].skillModules[0];
		}
		return skillModule;
	}

	// Token: 0x0400020A RID: 522
	public string dataName = "NULL";

	// Token: 0x0400020B RID: 523
	public int orderID = -1;

	// Token: 0x0400020C RID: 524
	public int effectID = -1;

	// Token: 0x0400020D RID: 525
	public int levelNeed = -1;

	// Token: 0x0400020E RID: 526
	public EnumSkillType skillType;

	// Token: 0x0400020F RID: 527
	[Header("名字")]
	public string[] skillModuleNames = new string[3];

	// Token: 0x04000210 RID: 528
	[Header("描述")]
	public string[] skillModuleInfos = new string[3];

	// Token: 0x04000211 RID: 529
	public FactorMultis factorMultis = new FactorMultis();

	// Token: 0x04000212 RID: 530
	public float[] facs = new float[10];
}
