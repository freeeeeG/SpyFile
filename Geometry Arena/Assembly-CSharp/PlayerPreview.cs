using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
[Serializable]
public class PlayerPreview
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600033E RID: 830 RVA: 0x000140C9 File Offset: 0x000122C9
	public Factor BasicFactor
	{
		get
		{
			return GameParameters.Inst.DefaultFactor;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x0600033F RID: 831 RVA: 0x000140D5 File Offset: 0x000122D5
	public FactorMultis FactorMulti_Job
	{
		get
		{
			return DataBase.Inst.DataPlayerModels[TempData.inst.jobId].factorMultis;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000340 RID: 832 RVA: 0x000140F1 File Offset: 0x000122F1
	public FactorMultis FactorMulti_JobTalent
	{
		get
		{
			return this.GetFactorMultis_JobTalent_FromGameData();
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000341 RID: 833 RVA: 0x000140F9 File Offset: 0x000122F9
	public FactorMultis FactorMulti_CommonTalent
	{
		get
		{
			return PlayerPreview.GetFactorMultis_CommonTalent_FromGameData();
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000342 RID: 834 RVA: 0x00014100 File Offset: 0x00012300
	public FactorMultis FactorMultis_Proficiency
	{
		get
		{
			return PlayerPreview.GetFactorMultis_Proficiency_FromGameData();
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000343 RID: 835 RVA: 0x00014107 File Offset: 0x00012307
	public FactorMultis FactorMultis_SkillModule
	{
		get
		{
			return PlayerPreview.GetFactorMultis_SkillModule();
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000344 RID: 836 RVA: 0x00014110 File Offset: 0x00012310
	public FactorMultis FactorMultis_CurrentRune
	{
		get
		{
			Rune currentRune = GameData.inst.CurrentRune;
			if (currentRune == null)
			{
				return new FactorMultis();
			}
			return currentRune.GetFactorMultis_ThisRune();
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000345 RID: 837 RVA: 0x00014138 File Offset: 0x00012338
	public FactorMultis FactorMulti_VarColor
	{
		get
		{
			if (TempData.inst.diffiOptFlag[21])
			{
				return new FactorMultis();
			}
			int varColorId = TempData.inst.varColorId;
			if (varColorId < 0 || varColorId >= DataBase.Inst.Data_VarColors.Length)
			{
				Debug.LogError("VarColorId Error!");
				return null;
			}
			return DataBase.Inst.Data_VarColors[varColorId].factorMultis;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000346 RID: 838 RVA: 0x00014198 File Offset: 0x00012398
	public Factor TotalFactor
	{
		get
		{
			return this.BasicFactor * this.FactorMulti_Job * (this.FactorMulti_JobTalent + this.FactorMulti_CommonTalent + this.FactorMultis_Proficiency) * this.FactorMulti_VarColor * TempData.inst.battle.FacMulPlayer * this.FactorMultis_CurrentRune * PlayerPreview.GetFactorMultis_SkillModule();
		}
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0001420C File Offset: 0x0001240C
	private FactorMultis GetFactorMultis_JobTalent_FromGameData()
	{
		GameData inst = GameData.inst;
		DataBase inst2 = DataBase.Inst;
		int jobId = TempData.inst.jobId;
		if (jobId < 0)
		{
			Debug.LogError("PlayerPreview_JobId<0!");
			return null;
		}
		PlayerModel playerModel = inst2.DataPlayerModels[jobId];
		int num = inst.jobs[jobId].TalentLevels.Length;
		if (num != playerModel.talents.Length)
		{
			Debug.LogError("PlayerPreview_GameData_TalentCnt!=DataBase  !");
			return null;
		}
		FactorMultis factorMultis = new FactorMultis();
		for (int i = 0; i < num; i++)
		{
			int num2 = inst.jobs[jobId].TalentLevels[i];
			if (num2 != 0)
			{
				Talent talent = playerModel.talents[i];
				for (int j = 1; j < talent.facs.Length; j++)
				{
					Talent.Fac fac = talent.facs[j];
					if (fac.FactorTypeIsAbility())
					{
						factorMultis.MultiOneFactor(fac.type, fac.num * (float)num2);
					}
				}
			}
		}
		return factorMultis;
	}

	// Token: 0x06000348 RID: 840 RVA: 0x000142F4 File Offset: 0x000124F4
	public static FactorMultis GetFactorMultis_CommonTalent_FromGameData()
	{
		GameData inst = GameData.inst;
		DataBase inst2 = DataBase.Inst;
		FactorMultis factorMultis = new FactorMultis();
		for (int i = 0; i < inst2.DataPlayerModels.Length; i++)
		{
			PlayerModel playerModel = inst2.DataPlayerModels[i];
			int num = inst.jobs[i].TalentLevels.Length;
			if (num != playerModel.talents.Length)
			{
				Debug.LogError("PlayerPreview_GameData_TalentCnt!=DataBase  !");
				return null;
			}
			for (int j = 0; j < num; j++)
			{
				int num2 = inst.jobs[i].TalentLevels[j];
				if (num2 != 0)
				{
					Talent.Fac fac = playerModel.talents[j].facs[0];
					if (fac.type >= 0)
					{
						factorMultis.MultiOneFactor(fac.type, fac.num * (float)num2);
					}
				}
			}
		}
		return factorMultis;
	}

	// Token: 0x06000349 RID: 841 RVA: 0x000143C0 File Offset: 0x000125C0
	public static FactorMultis GetFactorMultis_Proficiency_FromGameData()
	{
		GameData inst = GameData.inst;
		DataBase inst2 = DataBase.Inst;
		FactorMultis factorMultis = new FactorMultis();
		int jobId = TempData.inst.jobId;
		Mastery mastery = GameData.inst.jobs[jobId].mastery;
		int type = 4;
		float num = 0.3f * (float)mastery.GetRank();
		factorMultis.MultiOneFactor(type, num);
		return factorMultis;
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00014414 File Offset: 0x00012614
	private static FactorMultis GetFactorMultis_SkillModule()
	{
		FactorMultis factorMultis = new FactorMultis();
		bool[] array = TempData.inst.skillModuleFlags[TempData.inst.jobId];
		PlayerModel playerModel = DataBase.Inst.DataPlayerModels[TempData.inst.jobId];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				factorMultis *= SkillModule.GetSkillModule_CurrentJobWithEffectID(i).factorMultis;
			}
		}
		return factorMultis;
	}
}
