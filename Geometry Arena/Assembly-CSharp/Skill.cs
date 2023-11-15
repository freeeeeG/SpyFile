using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
[Serializable]
public class Skill
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000250 RID: 592 RVA: 0x0000DCFD File Offset: 0x0000BEFD
	public string Language_SkillName
	{
		get
		{
			return this.skillNameArray[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000251 RID: 593 RVA: 0x0000DD10 File Offset: 0x0000BF10
	public string Language_Info
	{
		get
		{
			return this.skillInfoArray[(int)Setting.Inst.language];
		}
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0000DD23 File Offset: 0x0000BF23
	public string GetInfoWithLanguageAndFac()
	{
		return this.Language_Info.GetColorfulInfoWithFacs(this.facs, false);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x0000DD38 File Offset: 0x0000BF38
	public static Skill SkillCurrent(ref int level)
	{
		int jobId = TempData.inst.jobId;
		level = GameData.inst.jobs[jobId].skillLevel;
		return DataBase.Inst.DataPlayerModels[jobId].skillLevels[level - 1];
	}

	// Token: 0x04000201 RID: 513
	public int level = -1;

	// Token: 0x04000202 RID: 514
	public EnumSkillType skillType;

	// Token: 0x04000203 RID: 515
	[Header("名字")]
	public string[] skillNameArray = new string[3];

	// Token: 0x04000204 RID: 516
	[Header("描述")]
	public string[] skillInfoArray = new string[3];

	// Token: 0x04000205 RID: 517
	public float[] facs = new float[11];
}
