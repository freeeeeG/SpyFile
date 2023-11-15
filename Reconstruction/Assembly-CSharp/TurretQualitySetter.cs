using System;
using TMPro;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class TurretQualitySetter : MonoBehaviour
{
	// Token: 0x06001046 RID: 4166 RVA: 0x0002BEF4 File Offset: 0x0002A0F4
	public void SetLevel(StrategyBase strategy)
	{
		if (strategy.Attribute.Rare <= 0)
		{
			this.UpgradeBtn.SetActive(false);
			return;
		}
		this.UpgradeBtn.SetActive(true);
		GameObject[] array = this.levelIcons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		for (int j = 0; j < strategy.Quality; j++)
		{
			this.levelIcons[j].SetActive(true);
		}
		if (strategy.Quality < 3)
		{
			this.upgradeCost = Singleton<StaticData>.Instance.LevelUpCostPerRare[strategy.Attribute.Rare - 1, strategy.Quality - 1];
			this.upgradeCost = (int)((float)this.upgradeCost * (1f - GameRes.TurretUpgradeDiscount - strategy.UpgradeDiscount));
			this.UpgradeCostValue.text = GameMultiLang.GetTraduction("UPGRADE") + "<sprite=7>" + this.upgradeCost.ToString();
			return;
		}
		this.UpgradeCostValue.text = "MAX";
	}

	// Token: 0x06001047 RID: 4167 RVA: 0x0002BFF4 File Offset: 0x0002A1F4
	public void SetSwitchCost(int cost)
	{
		this.switchCost = cost;
		this.SwitchTurretTxt.text = GameMultiLang.GetTraduction("SWITCHTRAP") + "<sprite=7>" + this.switchCost.ToString();
	}

	// Token: 0x06001048 RID: 4168 RVA: 0x0002C027 File Offset: 0x0002A227
	public void UpgradeBtnClick()
	{
		this.m_Turrettips.UpgradeBtnClick(this.upgradeCost);
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x0002C03A File Offset: 0x0002A23A
	public void SwitchTurretBtnClick()
	{
		Singleton<GameManager>.Instance.SwitchConcrete(this.m_Turrettips.m_Strategy.Concrete, this.switchCost);
	}

	// Token: 0x0400088C RID: 2188
	[SerializeField]
	private GameObject[] levelIcons;

	// Token: 0x0400088D RID: 2189
	private int upgradeCost;

	// Token: 0x0400088E RID: 2190
	[SerializeField]
	private TextMeshProUGUI UpgradeCostValue;

	// Token: 0x0400088F RID: 2191
	[SerializeField]
	private TextMeshProUGUI SwitchTurretTxt;

	// Token: 0x04000890 RID: 2192
	public GameObject UpgradeBtn;

	// Token: 0x04000891 RID: 2193
	[SerializeField]
	private TurretTips m_Turrettips;

	// Token: 0x04000892 RID: 2194
	private int switchCost;
}
