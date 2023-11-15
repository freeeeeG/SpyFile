using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[Serializable]
public class File_Job
{
	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x060001FB RID: 507 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
	public int[] TalentLevels
	{
		get
		{
			int[] array = new int[this.talentLevels.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (100 - this.talentLevels[i]) / 3;
			}
			return array;
		}
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000C524 File Offset: 0x0000A724
	public void SetTalentLevels(int[] ints)
	{
		int[] array = new int[ints.Length];
		for (int i = 0; i < ints.Length; i++)
		{
			array[i] = 100 - ints[i] * 3;
		}
		this.talentLevels = array;
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x060001FD RID: 509 RVA: 0x0000C55A File Offset: 0x0000A75A
	public int skillLevel
	{
		get
		{
			return this.GetLevels()[0];
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x060001FE RID: 510 RVA: 0x0000C564 File Offset: 0x0000A764
	public int skillLevelMax
	{
		get
		{
			return this.GetLevels()[1];
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x060001FF RID: 511 RVA: 0x0000C56E File Offset: 0x0000A76E
	public int colorLevel
	{
		get
		{
			return this.GetLevels()[2];
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000200 RID: 512 RVA: 0x0000C578 File Offset: 0x0000A778
	public int colorLevelMax
	{
		get
		{
			return this.GetLevels()[3];
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000C584 File Offset: 0x0000A784
	public void Clone(File_Job clone)
	{
		this.jobId = clone.jobId;
		this.mastery = new Mastery();
		this.mastery.exps = clone.mastery.exps;
		this.talentLevels = new int[clone.talentLevels.Length];
		for (int i = 0; i < this.talentLevels.Length; i++)
		{
			this.talentLevels[i] = clone.talentLevels[i];
		}
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000C5F4 File Offset: 0x0000A7F4
	public File_Job()
	{
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000C608 File Offset: 0x0000A808
	public File_Job(int jobId)
	{
		this.jobId = jobId;
		int[] array = new int[DataBase.Inst.DataPlayerModels[jobId].talents.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = 0;
		}
		this.SetTalentLevels(array);
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000C660 File Offset: 0x0000A860
	public int[] GetLevels()
	{
		int[] array = new int[4];
		for (int i = 0; i < this.TalentLevels.Length; i++)
		{
			Talent talent = DataBase.GetTalent(this.jobId, i);
			Talent.Fac fac = talent.facs[1];
			if (fac.type == 70)
			{
				array[0] = this.TalentLevels[i];
				array[1] = talent.maxLevel;
			}
			else if (fac.type == 80)
			{
				array[2] = this.TalentLevels[i];
				array[3] = talent.maxLevel;
			}
		}
		return array;
	}

	// Token: 0x040001BF RID: 447
	public int jobId;

	// Token: 0x040001C0 RID: 448
	public Mastery mastery = new Mastery();

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	public int[] talentLevels;
}
