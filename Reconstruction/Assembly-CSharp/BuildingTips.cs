using System;
using TMPro;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class BuildingTips : TileTips
{
	// Token: 0x0600101B RID: 4123 RVA: 0x0002B234 File Offset: 0x00029434
	public void ReadBuilding(BuildingStrategy buildingStrategy)
	{
		this.m_Att = buildingStrategy.Attribute;
		this.m_Strategy = buildingStrategy;
		this.switchCost = GameRes.SwitchTurretCost;
		this.switchCostTxt.text = GameMultiLang.GetTraduction("SWITCHTRAP") + "<sprite=7>" + this.switchCost.ToString();
		this.BasicInfo();
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0002B290 File Offset: 0x00029490
	private void BasicInfo()
	{
		this.Icon.sprite = this.m_Att.Icon;
		this.Name.text = GameMultiLang.GetTraduction(this.m_Att.Name);
		this.Description.text = StaticData.GetBuildingDes(this.m_Strategy.TurretSkills[0] as BuildingSkill);
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0002B2F4 File Offset: 0x000294F4
	public override void Show()
	{
		base.Show();
		this.tileinfo_Anim.SetTrigger("Show");
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0002B30C File Offset: 0x0002950C
	public void SwitchBtnClick()
	{
		Singleton<GameManager>.Instance.SwitchConcrete(this.m_Strategy.Concrete, this.switchCost);
	}

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	private Animator tileinfo_Anim;

	// Token: 0x04000860 RID: 2144
	private TurretAttribute m_Att;

	// Token: 0x04000861 RID: 2145
	private BuildingStrategy m_Strategy;

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	private TextMeshProUGUI switchCostTxt;

	// Token: 0x04000863 RID: 2147
	private int switchCost;
}
