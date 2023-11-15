using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B0 RID: 176
public class UI_Panel_Battle_UpgradeShop_Single : MonoBehaviour
{
	// Token: 0x06000618 RID: 1560 RVA: 0x00022CF8 File Offset: 0x00020EF8
	private void Resize()
	{
		for (int i = 0; i < this.rectsToReforce.Length; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectsToReforce[i]);
		}
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x00022D28 File Offset: 0x00020F28
	public void Init(int panelOrder)
	{
		this.order = panelOrder;
		UpgradeShop.Goods goods = Battle.inst.upgradeShop.upgradeGoods[this.order];
		this.upgradeID = goods.upgradeID;
		int price = goods.price;
		LanguageText.ShopMenu shopMenu = LanguageText.Inst.shopMenu;
		UI_Setting.Shop shop = UI_Setting.Inst.shop;
		this.icon_Upgrade.Init(this.upgradeID);
		if (this.upgradeID >= 0)
		{
			Upgrade upgrade = DataBase.Inst.Data_Upgrades[this.upgradeID];
			this.up = upgrade;
			Color color = UI_Setting.Inst.rankColors[(int)upgrade.rank];
			if (this.up.facs[0].type >= 0)
			{
				string s = shopMenu.smallTitle_Ability.Colored(shop.color_SmallTitle).Sized((float)shop.size_SmallTile) + "\n" + this.up.FacToString().Sized((float)shop.size_Info);
				this.TextOpenAndSetString(this.texts[0], s);
			}
			else
			{
				this.TextClose(this.texts[0]);
			}
			if (this.up.Language_Info != "null")
			{
				string str = this.up.Language_Info.GetColorfulInfoWithFacs(this.up.buffFacs, false).Replace("rng", MyTool.DecimalToUnsignedPercentString((double)this.up.bulletEffect.bulletEffectTrigRange));
				string text = shopMenu.smallTitle_Special.Colored(shop.color_SmallTitle).Sized((float)shop.size_SmallTile) + "\n" + str.Sized((float)shop.size_Info);
				if (this.up.numMax == -1)
				{
					text = text + "\n" + LanguageText.Inst.shopMenu.upgrade_tipN.Sized((float)shop.size_Info).Replace("curnum", Battle.inst.ForShow_UpgradeNum[this.upgradeID].ToString().Colored(UI_Setting.Inst.skill.Color_FactorNumber));
				}
				int num = this.upgradeID;
				this.TextOpenAndSetString(this.texts[1], text);
			}
			else
			{
				this.TextClose(this.texts[1]);
			}
			if (this.up.IfHasBEInfo())
			{
				string str2 = this.up.bulletEffect.BulletEffectFacToString();
				string s2 = shopMenu.smallTitle_Bullet.Colored(shop.color_SmallTitle).Sized((float)shop.size_SmallTile) + "\n" + str2.Sized((float)shop.size_Info);
				this.TextOpenAndSetString(this.texts[2], s2);
			}
			else
			{
				this.TextClose(this.texts[2]);
			}
			this.textTitle.text = upgrade.Language_Name.Colored(color).Sized((float)shop.size_Name) + "\n" + (LanguageText.Inst.ranks[(int)upgrade.rank] + shopMenu.upgrade).Sized((float)shop.size_RareLine).Colored(shop.color_RankLine);
			for (int i = 0; i < 3; i++)
			{
				this.simpleTooltip_Types[i].InitAsUpgradeType(upgrade.upgradeIntTypes[i]);
			}
			this.simpleTooltip_Types[3].InitAsUpgradeType((this.up.numMax == 1) ? 20 : -1);
		}
		else
		{
			LanguageText.ShopMenu shopMenu2 = LanguageText.Inst.shopMenu;
			this.textTitle.text = shopMenu2.soldOut_Title.Sized((float)shop.size_Name).Colored(shop.color_SoldOut);
			this.TextOpenAndSetString(this.texts[0], shopMenu2.soldOut_Info.Sized((float)shop.size_Info));
			this.TextClose(this.texts[1]);
			this.TextClose(this.texts[2]);
			for (int j = 0; j < 4; j++)
			{
				this.simpleTooltip_Types[j].InitAsUpgradeType(-1);
			}
		}
		this.imageFrag.sprite = ResourceLibrary.Inst.Sprite_Fragment;
		this.textFragPrice.text = price.ToString();
		if (this.upgradeID >= 0 && price == 0)
		{
			this.textFragPrice.text = shopMenu.refresh_FreeOnce;
		}
		this.Resize();
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x00023168 File Offset: 0x00021368
	public void InitMysteryBagWithID(int upgradeID)
	{
		this.upgradeID = upgradeID;
		LanguageText.ShopMenu shopMenu = LanguageText.Inst.shopMenu;
		UI_Setting.Shop shop = UI_Setting.Inst.shop;
		this.icon_Upgrade.Init(upgradeID);
		if (upgradeID >= 0)
		{
			Upgrade upgrade = DataBase.Inst.Data_Upgrades[upgradeID];
			this.up = upgrade;
			Color color = UI_Setting.Inst.rankColors[(int)upgrade.rank];
			if (this.up.facs[0].type >= 0)
			{
				string s = shopMenu.smallTitle_Ability.Colored(shop.color_SmallTitle).Sized((float)shop.size_SmallTile) + "\n" + this.up.FacToString().Sized((float)shop.size_Info);
				this.TextOpenAndSetString(this.texts[0], s);
			}
			else
			{
				this.TextClose(this.texts[0]);
			}
			if (this.up.Language_Info != "null")
			{
				string str = this.up.Language_Info.GetColorfulInfoWithFacs(this.up.buffFacs, false).Replace("rng", MyTool.DecimalToUnsignedPercentString((double)this.up.bulletEffect.bulletEffectTrigRange));
				string text = shopMenu.smallTitle_Special.Colored(shop.color_SmallTitle).Sized((float)shop.size_SmallTile) + "\n" + str.Sized((float)shop.size_Info);
				if (this.up.numMax == -1)
				{
					text = text + "\n" + LanguageText.Inst.shopMenu.upgrade_tipN.Sized((float)shop.size_Info).Replace("curnum", Battle.inst.ForShow_UpgradeNum[upgradeID].ToString());
				}
				this.TextOpenAndSetString(this.texts[1], text);
			}
			else
			{
				this.TextClose(this.texts[1]);
			}
			if (this.up.IfHasBEInfo())
			{
				string str2 = this.up.bulletEffect.BulletEffectFacToString();
				string s2 = shopMenu.smallTitle_Bullet.Colored(shop.color_SmallTitle).Sized((float)shop.size_SmallTile) + "\n" + str2.Sized((float)shop.size_Info);
				this.TextOpenAndSetString(this.texts[2], s2);
			}
			else
			{
				this.TextClose(this.texts[2]);
			}
			this.textTitle.text = upgrade.Language_Name.Colored(color).Sized((float)shop.size_Name) + "\n" + (LanguageText.Inst.ranks[(int)upgrade.rank] + shopMenu.upgrade).Sized((float)shop.size_RareLine).Colored(shop.color_RankLine);
			for (int i = 0; i < 3; i++)
			{
				this.simpleTooltip_Types[i].InitAsUpgradeType(upgrade.upgradeIntTypes[i]);
			}
			this.simpleTooltip_Types[3].InitAsUpgradeType((this.up.numMax == 1) ? 20 : -1);
		}
		else
		{
			LanguageText.ShopMenu shopMenu2 = LanguageText.Inst.shopMenu;
			this.textTitle.text = shopMenu2.soldOut_Title.Sized((float)shop.size_Name).Colored(shop.color_SoldOut);
			this.TextOpenAndSetString(this.texts[0], shopMenu2.soldOut_Info.Sized((float)shop.size_Info));
			this.TextClose(this.texts[1]);
			this.TextClose(this.texts[2]);
		}
		this.Resize();
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x000234E0 File Offset: 0x000216E0
	private void TextOpenAndSetString(Text t, string s)
	{
		t.text = s;
		t.gameObject.SetActive(true);
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x000234F5 File Offset: 0x000216F5
	private void TextClose(Text t)
	{
		t.gameObject.SetActive(false);
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Update()
	{
	}

	// Token: 0x04000511 RID: 1297
	[SerializeField]
	private UI_Icon_Upgrade icon_Upgrade;

	// Token: 0x04000512 RID: 1298
	[SerializeField]
	public int order = -1;

	// Token: 0x04000513 RID: 1299
	[SerializeField]
	private RectTransform[] rectsToReforce;

	// Token: 0x04000514 RID: 1300
	[SerializeField]
	private Text textTitle;

	// Token: 0x04000515 RID: 1301
	[SerializeField]
	private Text[] texts;

	// Token: 0x04000516 RID: 1302
	[SerializeField]
	private Image imageFrag;

	// Token: 0x04000517 RID: 1303
	[SerializeField]
	private Text textFragPrice;

	// Token: 0x04000518 RID: 1304
	[SerializeField]
	public int upgradeID;

	// Token: 0x04000519 RID: 1305
	[SerializeField]
	private Upgrade up;

	// Token: 0x0400051A RID: 1306
	[SerializeField]
	private UI_Text_SimpleTooltip[] simpleTooltip_Types;
}
