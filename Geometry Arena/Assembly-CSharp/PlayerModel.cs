using System;
using UnityEngine;

// Token: 0x02000030 RID: 48
[Serializable]
public class PlayerModel
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000239 RID: 569 RVA: 0x0000D6D6 File Offset: 0x0000B8D6
	public string Language_JobName
	{
		get
		{
			return this.jobName[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600023A RID: 570 RVA: 0x0000D6E9 File Offset: 0x0000B8E9
	public string Language_JobInfo
	{
		get
		{
			return this.jobInfo[(int)Setting.Inst.language];
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000D6FC File Offset: 0x0000B8FC
	public bool ifUnlocked()
	{
		return this.unlockPreJobId == -1 || GameData.inst.jobs[this.unlockPreJobId].skillLevel >= this.unlockPreJobLevel;
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000D72C File Offset: 0x0000B92C
	public int GetColorID()
	{
		switch (this.no)
		{
		case 0:
		case 1:
		case 2:
			return this.no;
		case 3:
		case 4:
		case 5:
			return this.no + 1;
		case 6:
		case 7:
		case 8:
			return this.no + 2;
		case 9:
			return 3;
		case 10:
			return 7;
		case 11:
			return 11;
		default:
			Debug.LogError("ColorError");
			return 0;
		}
	}

	// Token: 0x040001E8 RID: 488
	public string dataName = "UNINITED";

	// Token: 0x040001E9 RID: 489
	public int no = -1;

	// Token: 0x040001EA RID: 490
	public string[] jobName = new string[3];

	// Token: 0x040001EB RID: 491
	public string[] jobInfo = new string[3];

	// Token: 0x040001EC RID: 492
	[Header("技能（3个等级）")]
	public Skill[] skillLevels = new Skill[3];

	// Token: 0x040001ED RID: 493
	[Header("技能模块")]
	public SkillModule[] skillModules;

	// Token: 0x040001EE RID: 494
	public int skillModule_MaxEffectID = -1;

	// Token: 0x040001EF RID: 495
	[Header("天赋/角色强化")]
	public Talent[] talents = new Talent[0];

	// Token: 0x040001F0 RID: 496
	[Header("解锁相关")]
	public int unlockPreJobId = -1;

	// Token: 0x040001F1 RID: 497
	public int unlockPreJobLevel = -1;

	// Token: 0x040001F2 RID: 498
	public bool ifAvailable = true;

	// Token: 0x040001F3 RID: 499
	public FactorMultis factorMultis = new FactorMultis();
}
