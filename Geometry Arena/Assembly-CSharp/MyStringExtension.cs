using System;
using UnityEngine;

// Token: 0x02000072 RID: 114
public static class MyStringExtension
{
	// Token: 0x06000443 RID: 1091 RVA: 0x0001A7C4 File Offset: 0x000189C4
	public static string GetColorfulInfoWithFacs(this string s, float[] facs, bool ifBold)
	{
		string text = s;
		LanguageText inst = LanguageText.Inst;
		LanguageText.StringReplace stringReplace = inst.stringReplace;
		UI_Setting inst2 = UI_Setting.Inst;
		for (int i = 0; i < facs.Length; i++)
		{
			string str = "fac" + i;
			if (text.Contains(str + "na"))
			{
				text = text.Replace(str + "na", MyStringExtension.GetString_FacNA(facs[i]).IfBolded(ifBold));
			}
			if (text.Contains(str + "nm"))
			{
				text = text.Replace(str + "nm", MyStringExtension.GetString_FacNM(facs[i]).IfBolded(ifBold));
			}
			if (text.Contains(str + "t"))
			{
				text = text.Replace(str + "t", MyStringExtension.GetString_FacNT(facs[i]).IfBolded(ifBold));
			}
			if (text.Contains(str + "n"))
			{
				text = text.Replace(str + "n", MyStringExtension.GetString_FacNN(facs[i]).IfBolded(ifBold));
			}
			if (text.Contains(str + "pm"))
			{
				text = text.Replace(str + "pm", MyStringExtension.GetString_FacNPM(facs[i]).IfBolded(ifBold));
			}
			if (text.Contains(str + "pa"))
			{
				text = text.Replace(str + "pa", MyStringExtension.GetString_FacNPA(facs[i]).IfBolded(ifBold));
			}
			if (text.Contains(str + "pn"))
			{
				text = text.Replace(str + "pn", MyStringExtension.GetString_FacNPN(facs[i]).IfBolded(ifBold));
			}
		}
		if (text.Contains("fkb"))
		{
			text = text.Replace("fkb", inst.input.key_Shoot.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("skb"))
		{
			text = text.Replace("skb", inst.input.key_Skill.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sSkillName"))
		{
			text = text.Replace("sSkillName", DataBase.Inst.DataPlayerModels[TempData.inst.jobId].skillLevels[0].Language_SkillName.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sSMEffect"))
		{
			for (int j = 0; j <= DataBase.Inst.DataPlayerModels[TempData.inst.jobId].skillModule_MaxEffectID; j++)
			{
				if (text.Contains("sSMEffect" + j + "Name"))
				{
					SkillModule skillModule_CurrentJobWithEffectID = SkillModule.GetSkillModule_CurrentJobWithEffectID(j);
					Color color_Input = inst2.color_Input;
					text = text.Replace("sSMEffect" + j + "Name", skillModule_CurrentJobWithEffectID.Language_Name.Colored(color_Input).IfBolded(ifBold));
				}
			}
		}
		if (text.Contains("upType"))
		{
			for (int k = 0; k < DataBase.Inst.Data_UpgradeTypes.Length; k++)
			{
				if (text.Contains("upType" + k + "Name"))
				{
					UpgradeType upgradeType = DataBase.Inst.Data_UpgradeTypes[k];
					Color typeColor = upgradeType.typeColor;
					text = text.Replace("upType" + k + "Name", upgradeType.Language_TypeName.Colored(typeColor).IfBolded(ifBold));
				}
			}
		}
		if (text.Contains("shrad"))
		{
			text = text.Replace("shrad", inst.stringReplace.shrad.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("encost"))
		{
			text = text.Replace("encost", inst.stringReplace.encost.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("enrec"))
		{
			text = text.Replace("enrec", inst.stringReplace.enrec.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("enmax"))
		{
			text = text.Replace("enmax", inst.stringReplace.enmax.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sBuffMax"))
		{
			text = text.Replace("sBuffMax", inst.stringReplace.sBuffMax.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sBuffSpd"))
		{
			text = text.Replace("sBuffSpd", inst.stringReplace.sBuffSpd.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserLifetime"))
		{
			text = text.Replace("sLaserLifetime", inst.stringReplace.sLaserLifetime.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserWidth"))
		{
			text = text.Replace("sLaserWidth", inst.stringReplace.sLaserWidth.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserDamageFrequency"))
		{
			text = text.Replace("sLaserDamageFrequency", inst.stringReplace.sLaserDamageFrequency.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserDamage"))
		{
			text = text.Replace("sLaserDamage", inst.stringReplace.sLaserDamage.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserLength"))
		{
			text = text.Replace("sLaserLength", inst.stringReplace.sLaserLength.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserOverload"))
		{
			text = text.Replace("sLaserOverload", inst.stringReplace.sLaserOverload.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("sLaserFireNum"))
		{
			text = text.Replace("sLaserFireNum", inst.stringReplace.sLaserFireNum.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwordNum"))
		{
			text = text.Replace("ssSwordNum", inst.stringReplace.ssSwordNum.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwordDamage"))
		{
			text = text.Replace("ssSwordDamage", inst.stringReplace.ssSwordDamage.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwordHitBack"))
		{
			text = text.Replace("ssSwordHitBack", inst.stringReplace.ssSwordHitBack.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwordLength"))
		{
			text = text.Replace("ssSwordLength", inst.stringReplace.ssSwordLength.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwordWidth"))
		{
			text = text.Replace("ssSwordWidth", inst.stringReplace.ssSwordWidth.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwordSpeed"))
		{
			text = text.Replace("ssSwordSpeed", inst.stringReplace.ssSwordSpeed.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSwords"))
		{
			text = text.Replace("ssSwords", inst.stringReplace.ssSwords.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssSword"))
		{
			text = text.Replace("ssSword", inst.stringReplace.ssSword.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssBulletDrones"))
		{
			text = text.Replace("ssBulletDrones", inst.stringReplace.ssBulletDrones.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssBulletDrone"))
		{
			text = text.Replace("ssBulletDrone", inst.stringReplace.ssBulletDrone.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssGrenadeDrone"))
		{
			text = text.Replace("ssGrenadeDrone", inst.stringReplace.ssGrenadeDrone.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssTargetDrone"))
		{
			text = text.Replace("ssTargetDrone", inst.stringReplace.ssTargetDrone.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssLaserDrones"))
		{
			text = text.Replace("ssLaserDrones", inst.stringReplace.ssLaserDrones.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssLaserDrone"))
		{
			text = text.Replace("ssLaserDrone", inst.stringReplace.ssLaserDrone.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssItemDrone"))
		{
			text = text.Replace("ssItemDrone", inst.stringReplace.ssItemDrone.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("ssLightDrone"))
		{
			text = text.Replace("ssLightDrone", inst.stringReplace.ssLightDrone.Colored(inst2.color_Input).IfBolded(ifBold));
		}
		if (text.Contains("upname"))
		{
			for (int l = 0; l < DataBase.Inst.Data_Upgrades.Length; l++)
			{
				if (text.Contains("upname" + l + "e"))
				{
					Upgrade upgrade = DataBase.Inst.Data_Upgrades[l];
					Color color = UI_Setting.Inst.rankColors[(int)upgrade.rank];
					text = text.Replace("upname" + l + "e", upgrade.Language_Name.Colored(color).IfBolded(ifBold));
				}
			}
		}
		return text;
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x0001B208 File Offset: 0x00019408
	public static string IfBolded(this string s, bool flag)
	{
		if (!flag)
		{
			return s;
		}
		return s.Bolded();
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x0001B218 File Offset: 0x00019418
	private static string GetString_FacNN(float n)
	{
		Color color_FactorNumber = UI_Setting.Inst.skill.Color_FactorNumber;
		return n.ToString().Colored(color_FactorNumber);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x0001B244 File Offset: 0x00019444
	private static string GetString_FacNT(float t)
	{
		if (t < 0f)
		{
			return null;
		}
		int num = t.RoundToInt();
		if ((float)num != t)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"facNT_Error! ",
				t,
				" ",
				num
			}));
		}
		Color color_FactorType = UI_Setting.Inst.skill.Color_FactorType;
		return LanguageText.Inst.factor[num].Colored(color_FactorType);
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0001B2BC File Offset: 0x000194BC
	private static string GetString_FacNPM(float pm)
	{
		Color color_FactorNumber = UI_Setting.Inst.skill.Color_FactorNumber;
		return MyTool.DecimalToMultiPercentString((double)pm).Colored(color_FactorNumber);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0001B2E8 File Offset: 0x000194E8
	private static string GetString_FacNPA(float pa)
	{
		UI_Setting.Skill skill = UI_Setting.Inst.skill;
		Color color = skill.Color_FactorNumber;
		if (pa < 0f)
		{
			color = skill.Color_Red;
		}
		return MyTool.DecimalToPercentString((double)pa).Colored(color);
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0001B324 File Offset: 0x00019524
	private static string GetString_FacNPN(float pn)
	{
		Color color_FactorNumber = UI_Setting.Inst.skill.Color_FactorNumber;
		return MyTool.DecimalToUnsignedPercentString((double)pn).Colored(color_FactorNumber);
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001B350 File Offset: 0x00019550
	private static string GetString_FacNA(float na)
	{
		Color color_FactorNumber = UI_Setting.Inst.skill.Color_FactorNumber;
		return ("+" + na).Colored(color_FactorNumber);
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0001B384 File Offset: 0x00019584
	private static string GetString_FacNM(float nm)
	{
		Color color_FactorNumber = UI_Setting.Inst.skill.Color_FactorNumber;
		return ("x" + nm).Colored(color_FactorNumber);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0001B3B7 File Offset: 0x000195B7
	public static string AppendKeycode(this string s, string key)
	{
		s = s + " [" + key + "]";
		return s;
	}
}
