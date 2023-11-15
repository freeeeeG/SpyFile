using System;
using UnityEngine;

// Token: 0x0200003B RID: 59
[Serializable]
public class Upgrade
{
	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000274 RID: 628 RVA: 0x0000EA34 File Offset: 0x0000CC34
	public string Language_Name
	{
		get
		{
			return this.names[(int)Setting.Inst.language];
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000275 RID: 629 RVA: 0x0000EA47 File Offset: 0x0000CC47
	public string Language_Info
	{
		get
		{
			return this.infos[(int)Setting.Inst.language];
		}
	}

	// Token: 0x06000276 RID: 630 RVA: 0x0000EA5C File Offset: 0x0000CC5C
	public string InfoTotal()
	{
		bool flag = false;
		string text = "";
		string text2 = "";
		string text3 = "";
		string text4 = "";
		LanguageText.ShopMenu shopMenu = LanguageText.Inst.shopMenu;
		UI_Setting.Shop shop = UI_Setting.Inst.shop;
		string text5 = this.Language_Name.Sized((float)UI_Setting.Inst.skill.size_Name).Colored(UI_Setting.Inst.rankColors[(int)this.rank]) + "\n";
		string text6 = (LanguageText.Inst.ranks[(int)this.rank] + shopMenu.upgrade).Sized((float)UI_Setting.Inst.shop.size_RareLine).Colored(shop.color_RankLine) + "\n";
		UpgradeType[] data_UpgradeTypes = DataBase.Inst.Data_UpgradeTypes;
		if (this.numMax == 1)
		{
			text = text + data_UpgradeTypes[20].Language_TypeName.Colored(data_UpgradeTypes[20].typeColor) + " ";
		}
		for (int i = 0; i < 3; i++)
		{
			int num = this.upgradeIntTypes[i];
			if (num < 0)
			{
				break;
			}
			UpgradeType upgradeType = data_UpgradeTypes[num];
			text = text + upgradeType.Language_TypeName.Colored(upgradeType.typeColor) + " ";
		}
		text += "\n";
		if (this.facs[0].type >= 0)
		{
			text2 = shopMenu.smallTitle_Ability.Colored(shop.color_SmallTitleOnShow).Sized((float)shop.size_SmallTile) + "\n" + this.FacToString().Sized((float)shop.size_Info);
			flag = true;
		}
		if (this.Language_Info != "null")
		{
			if (flag)
			{
				text3 += "\n";
			}
			text3 = text3 + shopMenu.smallTitle_Special.Colored(shop.color_SmallTitleOnShow).Sized((float)shop.size_SmallTile) + "\n";
			text3 += this.Language_Info.GetColorfulInfoWithFacs(this.buffFacs, false).Replace("rng", MyTool.DecimalToUnsignedPercentString((double)this.bulletEffect.bulletEffectTrigRange).Colored(UI_Setting.Inst.skill.Color_FactorNumber)).Sized((float)shop.size_Info);
			int num2 = 0;
			if (Battle.inst.ForShow_UpgradeNum.Length > this.id)
			{
				num2 = Battle.inst.ForShow_UpgradeNum[this.id];
			}
			if (this.numMax == -1)
			{
				text3 = text3 + "\n" + LanguageText.Inst.shopMenu.upgrade_tipN.Replace("curnum", num2.ToString().Colored(UI_Setting.Inst.skill.Color_FactorNumber)).Sized((float)shop.size_Info);
			}
			flag = true;
		}
		if (this.bulletEffect.bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.GROWING || this.bulletEffect.bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.SUDDEN)
		{
			if (flag)
			{
				text4 += "\n\n";
			}
			text4 = text4 + shopMenu.smallTitle_Bullet.Colored(shop.color_SmallTitleOnShow).Sized((float)shop.size_SmallTile) + "\n";
			text4 += this.bulletEffect.BulletEffectFacToString().Sized((float)shop.size_Info);
		}
		string text7 = "";
		for (int j = 0; j < this.upgradeIntTypes.Length; j++)
		{
			text7 += UI_ToolTipInfo.GetInfo_AdditionTypeInfos(this.upgradeIntTypes[j]);
		}
		return string.Concat(new string[]
		{
			text5,
			text6,
			text,
			text2,
			text3,
			text4,
			text7
		});
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000EE08 File Offset: 0x0000D008
	public bool GetBool_IfHasType(int type)
	{
		if (type < 0)
		{
			Debug.LogError("Error,type<0!");
			return false;
		}
		int[] array = this.upgradeIntTypes;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0000EE43 File Offset: 0x0000D043
	public bool IfHasBEInfo()
	{
		return this.bulletEffect.bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.GROWING || this.bulletEffect.bulletEffectType == Upgrade_BulletEffect.EnumBulletEffect.SUDDEN;
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0000EE64 File Offset: 0x0000D064
	public string FacToString()
	{
		string text = "";
		string[] factor = LanguageText.Inst.factor;
		int num = 0;
		foreach (Upgrade.Fac fac in this.facs)
		{
			if (fac.type >= 0)
			{
				if (num > 0)
				{
					text += "\n";
				}
				text = text + factor[fac.type] + " ";
				if (fac.type != 14)
				{
					int type = fac.type;
				}
				text += MyTool.ColorfulSignedFactorPlusPercent(fac.type, fac.NumPlus);
				num++;
			}
		}
		return text;
	}

	// Token: 0x04000231 RID: 561
	public string dataName = "NAME_UNINITED";

	// Token: 0x04000232 RID: 562
	public string[] names = new string[3];

	// Token: 0x04000233 RID: 563
	public string[] infos = new string[3];

	// Token: 0x04000234 RID: 564
	public int id = -1;

	// Token: 0x04000235 RID: 565
	public EnumRank rank = EnumRank.UNINTED;

	// Token: 0x04000236 RID: 566
	public bool ifAvailable;

	// Token: 0x04000237 RID: 567
	public int numMax = -1;

	// Token: 0x04000238 RID: 568
	public float orderID = -1f;

	// Token: 0x04000239 RID: 569
	public int bulletEffectID = -1;

	// Token: 0x0400023A RID: 570
	public int specialEffectID = -1;

	// Token: 0x0400023B RID: 571
	public Upgrade.Fac[] facs = new Upgrade.Fac[3];

	// Token: 0x0400023C RID: 572
	public float[] buffFacs = new float[10];

	// Token: 0x0400023D RID: 573
	public int[] upgradeIntTypes = new int[3];

	// Token: 0x0400023E RID: 574
	public Upgrade_BulletEffect bulletEffect = new Upgrade_BulletEffect();

	// Token: 0x02000147 RID: 327
	[Serializable]
	public class Fac
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x00036B34 File Offset: 0x00034D34
		public float NumPlus
		{
			get
			{
				return GameParameters.Inst.FacStandards[this.type] * this.numUnit;
			}
		}

		// Token: 0x0400099C RID: 2460
		public int type = -1;

		// Token: 0x0400099D RID: 2461
		public float numUnit = -1f;

		// Token: 0x0400099E RID: 2462
		public float numMul = -1f;
	}
}
