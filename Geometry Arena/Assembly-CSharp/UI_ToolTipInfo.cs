using System;
using System.Text;
using UnityEngine;

// Token: 0x020000E4 RID: 228
public static class UI_ToolTipInfo
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x0002BC3C File Offset: 0x00029E3C
	public static string GetInfo_Talent(int jobId, int talentId)
	{
		int num = GameData.inst.jobs[jobId].TalentLevels[talentId];
		int maxLevel = DataBase.Inst.DataPlayerModels[jobId].talents[talentId].maxLevel;
		UI_Setting inst = UI_Setting.Inst;
		Talent talent = DataBase.Inst.DataPlayerModels[jobId].talents[talentId];
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		LanguageText inst2 = LanguageText.Inst;
		if (talentId < 8)
		{
			stringBuilder.Append(talent.Name).AppendLine();
		}
		else
		{
			stringBuilder.Append(inst2.talent_InfinityPre).Append(talent.Name).AppendLine();
		}
		stringBuilder2.Append(inst2.talent_CurLevel + num).AppendLine();
		if (talent.maxLevel > 0)
		{
			stringBuilder2.Append(inst2.talent_MaxLevel + talent.maxLevel).AppendLine();
		}
		if (num < talent.maxLevel)
		{
			stringBuilder2.Append(inst2.talent_Cost + talent.CostWithCurLevel(num)).AppendLine();
		}
		if (talentId >= 8)
		{
			stringBuilder2.AppendLine().Append(inst2.toolTip_Ability.main_InfinityInfo.ReplaceLineBreak()).AppendLine();
		}
		stringBuilder2.AppendLine();
		if (talent.facs[1].FactorTypeIsAbility())
		{
			if (num > 0)
			{
				stringBuilder2.Append(inst2.talent_CurLevelView.TextSet(inst.commonSets.blueSmallTile)).AppendLine();
				int num2 = 0;
				for (int i = 0; i < talent.facs.Length; i++)
				{
					Talent.Fac fac = talent.facs[i];
					if (fac.type < 0)
					{
						break;
					}
					if (num2 > 0)
					{
						stringBuilder2.AppendLine();
					}
					if (i == 0)
					{
						stringBuilder2.Append(inst2.talent_AllRoles);
					}
					else
					{
						stringBuilder2.Append(DataBase.Inst.DataPlayerModels[jobId].Language_JobName + " ");
					}
					float num3 = (float)(fac.num * (float)num * 100000f).RoundToInt() / 100000f;
					stringBuilder2.Append(inst2.factor[fac.type] + " " + MyTool.GetColorfulStringWithTypeAndNum(1, fac.type, (double)num3, true, 1f, true));
					num2++;
				}
			}
			if (num < maxLevel)
			{
				if (num > 0)
				{
					stringBuilder2.AppendLine().AppendLine();
				}
				num++;
				stringBuilder2.Append(inst2.talent_NextLevelPreview.TextSet(inst.commonSets.blueSmallTile)).AppendLine();
				int num4 = 0;
				for (int j = 0; j < talent.facs.Length; j++)
				{
					Talent.Fac fac2 = talent.facs[j];
					if (fac2.type < 0)
					{
						break;
					}
					if (num4 > 0)
					{
						stringBuilder2.AppendLine();
					}
					if (j == 0)
					{
						stringBuilder2.Append(inst2.talent_AllRoles + " ");
					}
					else
					{
						stringBuilder2.Append(DataBase.Inst.DataPlayerModels[jobId].Language_JobName + " ");
					}
					float num5 = (float)(fac2.num * (float)num * 100000f).RoundToInt() / 100000f;
					stringBuilder2.Append(inst2.factor[fac2.type] + " " + MyTool.GetColorfulStringWithTypeAndNum(1, fac2.type, (double)num5, true, 1f, true));
					num4++;
				}
			}
		}
		else
		{
			int type = talent.facs[1].type;
			if (type != 70)
			{
				if (type != 80)
				{
					Debug.LogError("Type??");
				}
				else
				{
					stringBuilder2.Append(inst2.toolTip_TipStrings[3].ReplaceLineBreak());
				}
			}
			else
			{
				stringBuilder2.Append(inst2.toolTip_TipStrings[7].ReplaceLineBreak());
			}
		}
		if (num < maxLevel)
		{
			stringBuilder2.AppendLine().AppendLine().Append(inst2.main_Common.tip_ShiftBuy10times);
		}
		return stringBuilder.ToString().TextSet(inst.commonSets.blueTitle) + stringBuilder2.ToString().Sized(inst.ToolTip_NormalSize);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0002C068 File Offset: 0x0002A268
	public static string GetInfo_Proficiency(int jobId)
	{
		Mastery mastery = GameData.inst.jobs[jobId].mastery;
		int num = mastery.GetRank();
		UI_Setting inst = UI_Setting.Inst;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		LanguageText inst2 = LanguageText.Inst;
		DataBase inst3 = DataBase.Inst;
		stringBuilder.Append(inst2.talent_Proficiency).AppendLine();
		stringBuilder2.Append(inst2.talent_CurLevel + num).AppendLine();
		stringBuilder2.Append(inst2.talent_MaxLevel + 10).AppendLine();
		if (num < 10)
		{
			stringBuilder2.Append(inst2.talent_Proficiency_Progress + mastery.GetString_Progress()).AppendLine();
		}
		stringBuilder2.AppendLine();
		stringBuilder2.Append(inst2.toolTip_TipStrings[8].ReplaceLineBreak()).AppendLine().AppendLine();
		int i = 0;
		while (i < 2)
		{
			if (i == 0)
			{
				if (num > 0)
				{
					goto IL_10F;
				}
			}
			else
			{
				if (i != 1)
				{
					goto IL_10F;
				}
				if (num < 10)
				{
					if (num > 0)
					{
						stringBuilder2.AppendLine().AppendLine();
					}
					num++;
					goto IL_10F;
				}
				break;
			}
			IL_219:
			i++;
			continue;
			IL_10F:
			if (i == 0)
			{
				stringBuilder2.Append(inst2.talent_CurLevelView.TextSet(inst.commonSets.blueSmallTile)).AppendLine();
			}
			else if (i == 1)
			{
				stringBuilder2.Append(inst2.talent_NextLevelPreview.TextSet(inst.commonSets.blueSmallTile)).AppendLine();
			}
			int num2 = 4;
			float num3 = 0.3f;
			if (num2 < 0)
			{
				Debug.LogError("Mastery: Fac.Type<0");
				return null;
			}
			stringBuilder2.Append(inst3.DataPlayerModels[jobId].Language_JobName + " ");
			float num4 = (float)(num3 * (float)num * 100000f).RoundToInt() / 100000f;
			stringBuilder2.Append(inst2.factor[num2] + " " + MyTool.GetColorfulStringWithTypeAndNum(1, num2, (double)num4, true, 1f, true)).AppendLine();
			stringBuilder2.Append(inst2.tooltip_GeometryCoin.gainBonus).Append(MyTool.GetColorfulStringWithTypeAndNum(0, 0, (double)(0.01f * (float)num), true, 0f, true));
			goto IL_219;
		}
		return stringBuilder.ToString().TextSet(inst.commonSets.blueTitle) + stringBuilder2.ToString().Sized(inst.ToolTip_NormalSize);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0002C2CC File Offset: 0x0002A4CC
	public static string GetInfo_VarColor(int varId)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		VarColor varColor = DataBase.Inst.Data_VarColors[varId];
		stringBuilder.Append(varColor.Language_Name).AppendLine();
		FactorMultis factorMultis = varColor.factorMultis;
		LanguageText inst = LanguageText.Inst;
		UI_Setting inst2 = UI_Setting.Inst;
		int num = 0;
		for (int i = 1; i <= 15; i++)
		{
			int num2 = 1;
			if (i != 5)
			{
				if ((i >= 10 && i <= 13) || i == 1)
				{
					num2 = 0;
				}
				float num3 = factorMultis.factorMultis[i];
				float num4 = num3 - (float)num2;
				if (num4 != 0f)
				{
					if (num > 0)
					{
						stringBuilder2.AppendLine();
					}
					if ((float)((i == 14 || i == 8) ? -1 : 1) * num4 <= 0f)
					{
						Color myRed = inst2.myRed;
					}
					else
					{
						Color myGreen = inst2.myGreen;
					}
					stringBuilder2.Append(inst.factor[i] + " ");
					if (MyTool.ifIsAddFactorID(i))
					{
						stringBuilder2.Append(MyTool.ColorfulSignedFactorPlusPercent(i, num3));
					}
					else
					{
						stringBuilder2.Append(MyTool.ColorfulSignedFactorMultiPercent(i, num3));
					}
					num++;
				}
			}
		}
		if (!GameData.inst.IfColorUnlockedToCurJob(varId))
		{
			int rank = (int)DataBase.Inst.Data_VarColors[varId].rank;
			stringBuilder3.Append(inst.main_Color_UnlockInfo.Replace("vdj", rank.ToString())).AppendLine();
		}
		return stringBuilder.ToString().Colored(varColor.ColorRGB.SetValue(1f)).Sized(inst2.ToolTip_HeaderSize) + stringBuilder3.ToString().Sized(inst2.ToolTip_NormalSize).Colored(inst2.myRed) + stringBuilder2.ToString().Sized(inst2.ToolTip_NormalSize);
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0002C4A0 File Offset: 0x0002A6A0
	public static string GetInfo_Job(int jobID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		UI_Setting inst = UI_Setting.Inst;
		PlayerModel playerModel = DataBase.Inst.DataPlayerModels[jobID];
		stringBuilder.Append(playerModel.Language_JobName.Sized(inst.ToolTip_HeaderSize)).AppendLine();
		stringBuilder.Append(playerModel.Language_JobInfo.Sized(inst.ToolTip_NormalSize)).AppendLine();
		FactorMultis factorMultis = playerModel.factorMultis;
		LanguageText inst2 = LanguageText.Inst;
		int num = 0;
		if (factorMultis.factorMultis[5] > 1f)
		{
			stringBuilder3.Append(inst2.factor[5] + " " + ("x" + factorMultis.factorMultis[5].ToString()).Colored(inst.myGreen)).AppendLine();
		}
		for (int i = 1; i <= 15; i++)
		{
			int num2 = 1;
			if (i != 5 && i != 12)
			{
				if (i >= 10 && i <= 13)
				{
					num2 = 0;
				}
				float num3 = factorMultis.factorMultis[i];
				float num4 = num3 - (float)num2;
				if (num4 != 0f)
				{
					if (num > 0)
					{
						stringBuilder3.AppendLine();
					}
					Color color = ((float)((i == 14 || i == 8) ? -1 : 1) * num4 > 0f) ? inst.myGreen : inst.myRed;
					stringBuilder3.Append(inst2.factor[i]).Append(" ");
					if (i == 1)
					{
						stringBuilder3.Append(("=" + num3).Colored(inst.myGreen));
					}
					else if (MyTool.ifIsAddFactorID(i))
					{
						stringBuilder3.Append(MyTool.DecimalToPercentString((double)num3).Colored(color));
					}
					else
					{
						stringBuilder3.Append(MyTool.DecimalToMultiPercentString((double)num3).Colored(color));
					}
					num++;
				}
			}
		}
		if (!GameData.inst.IfJobUnlocked(jobID))
		{
			if (GameParameters.Inst.ifDemo && (jobID == 2 || jobID == 5 || jobID == 8))
			{
				stringBuilder2.Append(inst2.demo.lockTip).AppendLine();
			}
			else
			{
				string language_JobName = DataBase.Inst.DataPlayerModels[DataBase.Inst.DataPlayerModels[jobID].unlockPreJobId].Language_JobName;
				int unlockPreJobLevel = DataBase.Inst.DataPlayerModels[jobID].unlockPreJobLevel;
				stringBuilder2.Append(inst2.main_Job_UnlockInfo.Replace("vjn", language_JobName).Replace("vdj", unlockPreJobLevel.ToString())).AppendLine();
			}
		}
		return stringBuilder.ToString().TextSet(inst.commonSets.blueSmallTile) + stringBuilder2.ToString().Sized(inst.ToolTip_NormalSize).Colored(inst.myRed) + stringBuilder3.ToString().Sized(inst.ToolTip_NormalSize);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x0002C780 File Offset: 0x0002A980
	public static string GetInfo_AbilityMain(int abiId)
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.ToolTip_Ability toolTip_Ability = inst.toolTip_Ability;
		if (abiId == 5 || abiId == 0)
		{
			return "??";
		}
		UI_Setting inst2 = UI_Setting.Inst;
		PlayerPreview playerPreview = TempData.inst.playerPreview;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder.Append(inst.ability_Desc[abiId]).AppendLine();
		float num = playerPreview.FactorMulti_Job.factorMultis[abiId];
		float num2 = playerPreview.FactorMulti_VarColor.factorMultis[abiId];
		float num3 = (playerPreview.FactorMulti_JobTalent + playerPreview.FactorMulti_CommonTalent + playerPreview.FactorMultis_Proficiency).factorMultis[abiId];
		float num4 = TempData.inst.battle.FacMulPlayer.factorMultis[abiId];
		float num5 = playerPreview.FactorMultis_CurrentRune.factorMultis[abiId];
		float num6 = playerPreview.FactorMultis_SkillModule.factorMultis[abiId];
		float num7 = MyTool.factorOrigin[abiId];
		string factorInfo = playerPreview.BasicFactor.GetFactorInfo(abiId);
		stringBuilder2.Append(factorInfo).Append(toolTip_Ability.main_Basic);
		if (num != num7)
		{
			string colorfulStringWithTypeAndNum = MyTool.GetColorfulStringWithTypeAndNum(1, abiId, (double)num, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum).Append(toolTip_Ability.main_Role);
		}
		if (num2 != num7)
		{
			string colorfulStringWithTypeAndNum2 = MyTool.GetColorfulStringWithTypeAndNum(1, abiId, (double)num2, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum2).Append(toolTip_Ability.main_Color);
		}
		if (num3 != num7)
		{
			string colorfulStringWithTypeAndNum3 = MyTool.GetColorfulStringWithTypeAndNum(1, abiId, (double)num3, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum3).Append(toolTip_Ability.main_Talent);
		}
		if (num4 != num7)
		{
			string colorfulStringWithTypeAndNum4 = MyTool.GetColorfulStringWithTypeAndNum(1, abiId, (double)num4, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum4).Append(toolTip_Ability.main_Do);
		}
		if (num5 != num7)
		{
			string colorfulStringWithTypeAndNum5 = MyTool.GetColorfulStringWithTypeAndNum(1, abiId, (double)num5, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum5).Append(toolTip_Ability.main_Rune);
		}
		if (num6 != num7)
		{
			string colorfulStringWithTypeAndNum6 = MyTool.GetColorfulStringWithTypeAndNum(1, abiId, (double)num6, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum6).Append(toolTip_Ability.main_FromSkillModule);
		}
		return stringBuilder.ToString().TextSet(inst2.commonSets.blueSmallTile) + stringBuilder2.ToString().Sized(inst2.ToolTip_NormalSize);
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0002C9DC File Offset: 0x0002ABDC
	public static string GetInfo_AbilityBattle(int abiId)
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.ToolTip_Ability toolTip_Ability = inst.toolTip_Ability;
		if (abiId == 5 || abiId == 0)
		{
			return "??";
		}
		UI_Setting inst2 = UI_Setting.Inst;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder.Append(inst.ability_Desc[abiId]).AppendLine();
		float num = Battle.inst.GetFactorMultis_Upgrates_CurBattle().factorMultis[abiId];
		float num2 = Player.inst.factorMultis_Skill.factorMultis[abiId];
		float num3 = MyTool.factorOrigin[abiId];
		string factorInfo = Player.inst.unit.FactorBasic.GetFactorInfo(abiId);
		stringBuilder2.Append(factorInfo).Append(toolTip_Ability.battle_Init);
		if (num != num3)
		{
			string value = MyTool.DecimalToAutoPercentString_Colorful((double)num, abiId, false);
			stringBuilder2.AppendLine().Append(value).Append(toolTip_Ability.battle_Upgrade);
		}
		if (num2 != num3)
		{
			string value2 = MyTool.DecimalToAutoPercentString_Colorful((double)num2, abiId, false);
			stringBuilder2.AppendLine().Append(value2).Append(toolTip_Ability.battle_Skill);
		}
		foreach (BattleBuff battleBuff in BattleManager.inst.listBattleBuffs)
		{
			double num4 = battleBuff.factorMultis.GetDoubles_Power(battleBuff.layerThis)[abiId];
			if (num4 != (double)num3)
			{
				string value3 = MyTool.DecimalToAutoPercentString_Colorful(num4, abiId, false);
				stringBuilder2.AppendLine().Append(value3).Append(toolTip_Ability.common_From).Append(battleBuff.Lang_Name);
			}
		}
		if (abiId == 3 || abiId == 4)
		{
			stringBuilder2.AppendLine().AppendLine();
			LanguageText.Optimization optimization = inst.optimization;
			if (Setting.Inst.Option_BulletOptimize)
			{
				BasicUnit unit = Player.inst.unit;
				stringBuilder2.Append(optimization.bulletOptmization_On.Colored(inst2.myGreen)).Append(optimization.bulletOptmization_OpenTip).AppendLine();
				stringBuilder2.Append(optimization.bulletOptmization_FactorTip).AppendLine();
				stringBuilder2.Append(optimization.bulletOptmization_CurrentActual).Append(inst.factor[abiId]).Append(" = ");
				if (abiId <= 4)
				{
					if (abiId != 3)
					{
						if (abiId == 4)
						{
							stringBuilder2.Append(BulletsOptimization.ActualBulletDamage().ToString("0.0"));
						}
					}
					else
					{
						stringBuilder2.Append(BulletsOptimization.ActualFireSpeed().ToString("0.0"));
					}
				}
				else if (abiId != 14)
				{
					if (abiId == 15)
					{
						stringBuilder2.Append(MyTool.DecimalToUnsignedPercentString((double)BulletsOptimization.ActualHitBack()));
					}
				}
				else
				{
					stringBuilder2.Append(MyTool.DecimalToUnsignedPercentString((double)BulletsOptimization.ActualRecoil()));
				}
			}
			else
			{
				stringBuilder2.Append(optimization.bulletOptmization_Off.Colored(inst2.myRed)).Append(optimization.bulletOptmization_OpenTip).AppendLine();
				stringBuilder2.Append(optimization.bulletOptmization_FactorTip);
			}
		}
		return stringBuilder.ToString().TextSet(inst2.commonSets.blueSmallTile) + stringBuilder2.ToString().Sized(inst2.ToolTip_NormalSize);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0002CD00 File Offset: 0x0002AF00
	public static string GetInfo_FactorBattle(int fbID)
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.ToolTip_FactorBattle toolTip_FactorBattle = inst.toolTip_FactorBattle;
		UI_Setting inst2 = UI_Setting.Inst;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder.Append(inst.battleFactorInfo[fbID].ReplaceLineBreak()).AppendLine();
		double num = FactorBattle.FactorBattleThisLevel().factors[fbID];
		double num2 = Battle.inst.factorBattle_FromDifficultyOption.factors[fbID];
		double num3 = Battle.inst.FactorBattle_FromDifficultyLevel.factors[fbID];
		float num4 = 1f;
		string string_NumberFormated_Basic = FactorBattle.GetString_NumberFormated_Basic(fbID);
		stringBuilder2.Append(string_NumberFormated_Basic);
		if (fbID - 8 <= 3)
		{
			stringBuilder2.Append(" ").Append(inst.ranks[fbID - 8]).Append(toolTip_FactorBattle.upgradeBasicWeight);
		}
		else
		{
			stringBuilder2.Append(toolTip_FactorBattle.basic);
		}
		if (num != (double)num4)
		{
			string colorfulStringWithTypeAndNum = MyTool.GetColorfulStringWithTypeAndNum(0, fbID, num, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum).Append(toolTip_FactorBattle.fromWaveLevel);
		}
		if (num2 != (double)num4)
		{
			string colorfulStringWithTypeAndNum2 = MyTool.GetColorfulStringWithTypeAndNum(0, fbID, num2, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum2).Append(toolTip_FactorBattle.fromDiffiOpt);
		}
		if (num3 != (double)num4)
		{
			string colorfulStringWithTypeAndNum3 = MyTool.GetColorfulStringWithTypeAndNum(0, fbID, num3, false, 0f, true);
			stringBuilder2.AppendLine().Append(colorfulStringWithTypeAndNum3).Append(toolTip_FactorBattle.fromDiffiLevel);
		}
		if (fbID >= 8 && fbID <= 11)
		{
			stringBuilder2.AppendLine();
			string string_NumberToFormat = FactorBattle.GetString_NumberToFormat(fbID, (double)FactorBattle.GetFloat_UpgradeWeightWithRank_Total((EnumRank)(fbID - 8)), false);
			string text = FactorBattle.GetFloat_UpgradeWeight_Total_AllRank().ToString();
			stringBuilder2.Append("=" + string_NumberToFormat);
			stringBuilder2.Append(" ").Append(inst.ranks[fbID - 8]).Append(toolTip_FactorBattle.upgradeTotalWeight).AppendLine();
			stringBuilder2.Append(string.Concat(new string[]
			{
				string.Format("{0}={1}", toolTip_FactorBattle.allUpgradeTotalWeight, FactorBattle.GetFloat_UpgradeWeightWithRank_Total(EnumRank.NORMAL)),
				string.Format("+{0}", FactorBattle.GetFloat_UpgradeWeightWithRank_Total(EnumRank.RARE)),
				string.Format("+{0}", FactorBattle.GetFloat_UpgradeWeightWithRank_Total(EnumRank.EPIC)),
				string.Format("+{0}", FactorBattle.GetFloat_UpgradeWeightWithRank_Total(EnumRank.LEGENDARY)),
				"=",
				text
			})).AppendLine();
			stringBuilder2.Append(string.Concat(new string[]
			{
				inst.battleFactor[fbID],
				"=",
				inst.ranks[fbID - 8],
				toolTip_FactorBattle.upgradeTotalWeight,
				"/",
				toolTip_FactorBattle.allUpgradeTotalWeight,
				"=",
				string_NumberToFormat,
				"/",
				text,
				"=",
				FactorBattle.GetString_NumberFormated_Total(fbID)
			}));
		}
		return stringBuilder.ToString().TextSet(inst2.commonSets.factorInfo) + stringBuilder2.ToString().Sized(inst2.ToolTip_NormalSize);
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0002D018 File Offset: 0x0002B218
	public static string GetInfo_DifficultyOptions(int id, bool ifUnlock = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		DifficultyOption difficultyOption = DataBase.Inst.Data_DifficultyOptions[id];
		LanguageText inst = LanguageText.Inst;
		UI_Setting inst2 = UI_Setting.Inst;
		stringBuilder.Append(difficultyOption.Language_Name.TextSet(inst2.commonSets.blueTitle)).AppendLine();
		if (TempData.inst.diffiOptFlag[id])
		{
			stringBuilder.Append(inst.main_Common.option_On.TextSet(inst2.infinityMode.open)).AppendLine();
		}
		else
		{
			stringBuilder.Append(inst.main_Common.option_Off.TextSet(inst2.infinityMode.lockedOrOff)).AppendLine();
		}
		if (!ifUnlock && GameParameters.Inst.ifDemo)
		{
			stringBuilder.Append(inst.demo.lockTip.TextSet(inst2.infinityMode.lockedOrOff)).AppendLine();
		}
		int num = 0;
		foreach (DifficultyOption.Fac fac in difficultyOption.facs)
		{
			int type = fac.type;
			float num2 = fac.num;
			if (type >= 0)
			{
				if (num > 0)
				{
					stringBuilder2.AppendLine();
				}
				if (type >= 0 && type <= 99)
				{
					stringBuilder2.Append(inst.battleFactor[type] + " ");
				}
				else if (type >= 100 && type <= 199)
				{
					stringBuilder2.Append(inst.player).Append(inst.factor[type - 100] + " ");
				}
				else if (type >= 200 && type <= 299)
				{
					stringBuilder2.Append(inst.enemy).Append(inst.factor[type - 200] + " ");
				}
				else
				{
					Debug.LogError("DOFacTypeError");
				}
				stringBuilder2.Append(MyTool.GetColorfulStringWithTypeAndNum(type / 100, type % 100, (double)num2, false, 0f, true));
				num++;
			}
		}
		if (difficultyOption.Language_Info != "null")
		{
			stringBuilder2.AppendLine().Append(difficultyOption.Language_Info);
		}
		return stringBuilder.ToString() + stringBuilder2.ToString().Sized(inst2.ToolTip_NormalSize);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0002D26C File Offset: 0x0002B46C
	public static string GetInfo_DifficultyLevel(int level, bool ifShowNew)
	{
		StringBuilder stringBuilder = new StringBuilder();
		LanguageText inst = LanguageText.Inst;
		if (level == 0)
		{
			stringBuilder.Append(inst.challengeMenu.info_NoEffect);
		}
		else if (!ifShowNew)
		{
			FactorBattle factorBattle = GameParameters.Inst.difficultyLevel_FactorBattle[level];
			int num = 0;
			for (int i = 0; i < factorBattle.factors.Length; i++)
			{
				double num2 = factorBattle.factors[i];
				if (num2 != 1.0)
				{
					if (num > 0)
					{
						stringBuilder.AppendLine();
					}
					num++;
					stringBuilder.Append(inst.battleFactor[i] + " ").Append(MyTool.GetColorfulStringWithTypeAndNum(0, i, num2, false, 0f, true));
				}
			}
		}
		else
		{
			FactorBattle factorBattle2 = GameParameters.Inst.difficultyLevel_FactorBattle[level];
			FactorBattle factorBattle3 = GameParameters.Inst.difficultyLevel_FactorBattle[level - 1];
			int num3 = 0;
			for (int j = 0; j < factorBattle2.factors.Length; j++)
			{
				double num4 = factorBattle2.factors[j];
				double num5 = factorBattle3.factors[j];
				if (num4 != 1.0 && num5 != 1.0)
				{
					if (num3 > 0)
					{
						stringBuilder.AppendLine();
					}
					num3++;
					stringBuilder.Append(inst.battleFactor[j] + " ").Append(MyTool.GetColorfulStringWithTypeAndNum(0, j, num4, false, 0f, true));
				}
			}
			for (int k = 0; k < factorBattle2.factors.Length; k++)
			{
				double num6 = factorBattle2.factors[k];
				double num7 = factorBattle3.factors[k];
				if (num6 != 1.0 && num7 == 1.0)
				{
					if (num3 > 0)
					{
						stringBuilder.AppendLine();
					}
					num3++;
					stringBuilder.Append(inst.newTip).Append(inst.battleFactor[k] + " ").Append(MyTool.GetColorfulStringWithTypeAndNum(0, k, num6, false, 0f, true));
				}
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0002D47C File Offset: 0x0002B67C
	public static string GetInfo_Skill(int jobNo, int skillLevel)
	{
		Skill skill = DataBase.Inst.DataPlayerModels[jobNo].skillLevels[skillLevel - 1];
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		LanguageText inst = LanguageText.Inst;
		UI_Setting.Skill skill2 = UI_Setting.Inst.skill;
		int num = 3;
		if (TempData.inst.jobId >= 9)
		{
			num = 1;
		}
		stringBuilder.Append(skill.Language_SkillName.Sized((float)skill2.size_Name).Colored(skill2.Color_SmallTitle)).AppendLine();
		stringBuilder.Append(inst.skill.skillType[(int)skill.skillType].Sized((float)skill2.size_Type).Colored(skill2.Color_Type).Bolded()).AppendLine();
		stringBuilder.Append((inst.talent_CurLevel + skill.level).Sized((float)skill2.size_Info)).AppendLine();
		stringBuilder.Append((inst.talent_MaxLevel + num).Sized((float)skill2.size_Info)).AppendLine();
		stringBuilder2.Append(inst.talent_CurLevelView.Sized((float)skill2.size_SmallTile).Colored(skill2.Color_SmallTitle)).AppendLine();
		stringBuilder2.Append(skill.GetInfoWithLanguageAndFac().Sized((float)skill2.size_Info));
		if (skillLevel != num)
		{
			stringBuilder2.AppendLine().AppendLine();
			Skill skill3 = DataBase.Inst.DataPlayerModels[jobNo].skillLevels[skillLevel];
			stringBuilder2.Append(inst.talent_NextLevelPreview.Sized((float)skill2.size_SmallTile).Colored(skill2.Color_SmallTitle)).AppendLine();
			stringBuilder2.Append(skill3.GetInfoWithLanguageAndFac().Sized((float)skill2.size_Info));
		}
		return stringBuilder.AppendLine().ToString() + stringBuilder2.ToString();
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0002D64C File Offset: 0x0002B84C
	public static string GetInfo_SkillModule(int jobNo, int modID, bool ifUnlocked, bool ifOpen)
	{
		SkillModule skillModule = DataBase.Inst.DataPlayerModels[jobNo].skillModules[modID];
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		LanguageText inst = LanguageText.Inst;
		LanguageText.SkillModule skillModule2 = inst.skillModule;
		UI_Setting inst2 = UI_Setting.Inst;
		UI_Setting.Skill skill = inst2.skill;
		stringBuilder.Append(skillModule.Language_Name.Sized((float)skill.size_Name).Colored(skill.Color_SmallTitle)).AppendLine();
		if (ifUnlocked)
		{
			if (ifOpen)
			{
				stringBuilder.Append(inst.main_Common.option_On.TextSet(inst2.infinityMode.open)).AppendLine();
			}
			else
			{
				stringBuilder.Append(inst.main_Common.option_Off.TextSet(inst2.infinityMode.lockedOrOff)).AppendLine();
			}
		}
		else
		{
			stringBuilder.Append(skillModule2.unlockTip.Replace("sLevelNeed", skillModule.levelNeed.ToString()).TextSet(inst2.infinityMode.lockedOrOff)).AppendLine();
		}
		string str = skillModule2.stringJobSkillModule.Replace("sJob", DataBase.Inst.DataPlayerModels[jobNo].Language_JobName).Replace("sSM", skillModule2.skillModule_Total);
		stringBuilder.Append(str.Sized((float)skill.size_Type).Colored(skill.Color_Type).Bolded());
		if (skillModule.Language_Info != "null")
		{
			stringBuilder2.AppendLine().AppendLine();
			stringBuilder2.Append(skillModule2.specialEffect.TextSet(inst2.commonSets.blueSmallTile)).AppendLine();
			stringBuilder2.Append(skillModule.GetTotalInfo().Sized((float)skill.size_Info));
		}
		FactorMultis factorMultis = skillModule.factorMultis;
		int num = 0;
		StringBuilder stringBuilder3 = new StringBuilder();
		for (int i = 0; i < factorMultis.factorMultis.Length; i++)
		{
			float num2 = factorMultis.factorMultis[i];
			float num3 = MyTool.factorOrigin[i];
			if (num2 != num3)
			{
				if (num > 0)
				{
					stringBuilder3.AppendLine();
				}
				stringBuilder3.Append(inst.factor[i].Colored(inst2.skill.Color_FactorType) + " ").Append(MyTool.GetColorfulStringWithTypeAndNum(1, i, (double)num2, false, 0f, true));
				num++;
			}
		}
		if (num > 0)
		{
			stringBuilder2.AppendLine().AppendLine();
			stringBuilder2.Append(skillModule2.abilityChange.TextSet(inst2.commonSets.blueSmallTile)).AppendLine();
			stringBuilder2.Append(stringBuilder3);
		}
		string text = stringBuilder.ToString() + stringBuilder2.ToString();
		if (skillModule.Language_Info.Contains("upType"))
		{
			for (int j = 0; j < DataBase.Inst.Data_UpgradeTypes.Length; j++)
			{
				if (skillModule.Language_Info.Contains("upType" + j + "Name"))
				{
					text += UI_ToolTipInfo.GetInfo_AdditionTypeInfos(j);
				}
			}
		}
		return text;
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0002D960 File Offset: 0x0002BB60
	public static string GetInfo_BattleBuffInfo(BattleBuff buff, int layer)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		LanguageText inst = LanguageText.Inst;
		UI_Setting.Skill skill = UI_Setting.Inst.skill;
		BattleBuff.EnumBuffSource source = buff.source;
		if (source != BattleBuff.EnumBuffSource.BATTLEITEM)
		{
			if (source == BattleBuff.EnumBuffSource.UPGRADE)
			{
				stringBuilder.Append(buff.Lang_Name.Sized((float)skill.size_Name).Colored(skill.Color_SmallTitle)).AppendLine();
				stringBuilder2.Append(buff.GetInfoWithFac().Sized((float)skill.size_Info));
			}
		}
		else
		{
			stringBuilder.Append(buff.Lang_Name.Sized((float)skill.size_Name).Colored(skill.Color_SmallTitle)).AppendLine();
			stringBuilder2.Append(buff.GetInfoWithFac().Sized((float)skill.size_Info));
		}
		if (TempData.inst.currentSceneType == EnumSceneType.BATTLE)
		{
			if (layer > 1)
			{
				stringBuilder3.AppendLine();
				stringBuilder3.AppendLine();
				stringBuilder3.Append(inst.toolTip_Ability.battle_Buff_LayerNumber + layer.ToString());
			}
			StringBuilder stringBuilder4 = new StringBuilder();
			bool flag = false;
			double[] array = new double[]
			{
				1.0,
				0.0,
				1.0,
				1.0,
				1.0,
				1.0,
				1.0,
				1.0,
				1.0,
				1.0,
				0.0,
				0.0,
				0.0,
				0.0,
				1.0,
				1.0
			};
			int num = 0;
			if (layer != buff.layerThis)
			{
				for (int i = 0; i < layer; i++)
				{
					array *= buff.factorMultis;
				}
			}
			else
			{
				array = buff.factorMultis.GetDoubles_Power(buff.layerThis);
			}
			for (int j = 0; j < array.Length; j++)
			{
				double num2 = array[j];
				double num3 = (double)MyTool.factorOrigin[j];
				if (num2 != num3)
				{
					if (num > 0)
					{
						stringBuilder4.AppendLine();
					}
					num++;
					flag = true;
					stringBuilder4.Append(inst.factor[j] + " ").Append(MyTool.GetColorfulStringWithTypeAndNum(1, j, num2, false, 0f, true));
				}
			}
			if (flag)
			{
				stringBuilder3.AppendLine().AppendLine().AppendLine(inst.toolTip_Ability.battle_Buff_CurrentView).Append(stringBuilder4);
			}
		}
		return stringBuilder.ToString() + stringBuilder2.ToString() + stringBuilder3.ToString();
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0002DB84 File Offset: 0x0002BD84
	public static string GetInfo_Mode(int modeID, bool ifUnlocked, bool ifOpen)
	{
		StringBuilder stringBuilder = new StringBuilder();
		LanguageText inst = LanguageText.Inst;
		LanguageText.Mode_Single mode_Single = inst.main_Modes.mode_Singles[modeID];
		UI_Setting.InfinityMode infinityMode = UI_Setting.Inst.infinityMode;
		stringBuilder.Append(mode_Single.titleName.TextSet(infinityMode.setTitle)).AppendLine();
		if (ifUnlocked)
		{
			if (ifOpen)
			{
				stringBuilder.Append(inst.main_Common.option_Chosen.TextSet(infinityMode.open)).AppendLine();
			}
			else
			{
				stringBuilder.Append(inst.main_Common.optino_Unchosen.TextSet(infinityMode.lockedOrOff)).AppendLine();
			}
		}
		else if (GameParameters.Inst.ifDemo)
		{
			stringBuilder.Append(inst.demo.lockTip.TextSet(infinityMode.lockedOrOff)).AppendLine();
		}
		else
		{
			stringBuilder.Append(inst.main_Common.locked.TextSet(infinityMode.lockedOrOff)).AppendLine().Append(mode_Single.unlockTip.TextSet(infinityMode.lockedOrOff)).AppendLine();
		}
		stringBuilder.AppendLine();
		stringBuilder.Append(mode_Single.info.ReplaceLineBreak().TextSet(infinityMode.info));
		if (modeID == 3)
		{
			LanguageText.DailyChallenge dailyChallenge = inst.dailyChallenge;
			stringBuilder.AppendLine().Append(dailyChallenge.currentDate.Replace("currentDate", NetworkTime.GetString_TimeWithDayIndex(NetworkTime.Inst.dayIndex))).AppendLine();
			stringBuilder.Append(dailyChallenge.nextRefresh).AppendLine();
			stringBuilder.Append(NetworkTime.GetString_CountdownOrError());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0002DD1C File Offset: 0x0002BF1C
	public static string GetInfo_Upgrade(int upID)
	{
		Upgrade upgrade = DataBase.Inst.Data_Upgrades[upID];
		string str = LanguageText.Inst.pauseMenu.info_Tooltip_Upgrade_DeleteTip.Colored(Color.red).Sized(UI_Setting.Inst.ToolTip_SmallSize) + "\n";
		string str2 = upgrade.InfoTotal().Sized(UI_Setting.Inst.ToolTip_NormalSize);
		return str + str2;
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0002DD84 File Offset: 0x0002BF84
	public static string GetInfo_UpgradeLibrary(int upID)
	{
		return DataBase.Inst.Data_Upgrades[upID].InfoTotal().Sized(UI_Setting.Inst.ToolTip_NormalSize);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0002DDA8 File Offset: 0x0002BFA8
	public static string GetInfo_Main_StarBonus()
	{
		LanguageText inst = LanguageText.Inst;
		LanguageText.ToolTip_Ability toolTip_Ability = inst.toolTip_Ability;
		UI_Setting inst2 = UI_Setting.Inst;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(inst.battleFactor[0].ToString().TextSet(inst2.commonSets.blueSmallTile)).AppendLine();
		string value = "100%";
		stringBuilder.Append(value).Append(toolTip_Ability.main_Basic);
		for (int i = 0; i < TempData.inst.diffiOptFlag.Length; i++)
		{
			if (TempData.inst.diffiOptFlag[i])
			{
				DifficultyOption difficultyOption = DataBase.Inst.Data_DifficultyOptions[i];
				string colorfulStringWithTypeAndNum = MyTool.GetColorfulStringWithTypeAndNum(0, 0, (double)difficultyOption.facs[0].num, false, 0f, true);
				stringBuilder.AppendLine().Append(colorfulStringWithTypeAndNum).Append(toolTip_Ability.common_From).Append(difficultyOption.Language_Name);
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0002DE98 File Offset: 0x0002C098
	public static string GetInfo_AdditionTypeInfos(int typeID)
	{
		if ((typeID < 3 || typeID > 5) && typeID != 11)
		{
			return "";
		}
		UpgradeType upgradeType = DataBase.Inst.Data_UpgradeTypes[typeID];
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		LanguageText.SkillModule skillModule = LanguageText.Inst.skillModule;
		UI_Setting.Addition addition = UI_Setting.Inst.addition;
		stringBuilder.AppendLine().AppendLine();
		stringBuilder.Append(upgradeType.Language_TypeName.Sized((float)addition.title_Size).Colored(upgradeType.typeColor)).AppendLine();
		stringBuilder2.Append(upgradeType.Language_TypeInfo.TextSet(addition.info_Set));
		return stringBuilder.ToString() + stringBuilder2.ToString();
	}
}
