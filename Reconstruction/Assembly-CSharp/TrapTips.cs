using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000291 RID: 657
public class TrapTips : TileTips
{
	// Token: 0x0600102E RID: 4142 RVA: 0x0002B704 File Offset: 0x00029904
	public override void Show()
	{
		base.Show();
		this.tileinfo_Anim.SetTrigger("Show");
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0002B71C File Offset: 0x0002991C
	public void ReadTrap(TrapContent trapContent, int cost)
	{
		this.m_Trap = trapContent;
		this.m_TrapAtt = this.m_Trap.TrapAttribute;
		if (trapContent.IsReveal)
		{
			this.BasicInfo();
			this.switchTrapArea.SetActive(!this.m_Trap.Important);
			this.switchTrapCostTxt.text = GameMultiLang.GetTraduction("SWITCHTRAP") + "<sprite=7>" + cost.ToString();
		}
		else
		{
			this.Icon.sprite = Singleton<StaticData>.Instance.UnrevealTrap;
			this.Name.text = GameMultiLang.GetTraduction("UNREVEAL");
			this.Description.text = GameMultiLang.GetTraduction("UNREVEALINFO");
			this.switchTrapArea.SetActive(false);
		}
		if (trapContent.DamageAnalysis > 0L)
		{
			this.DamageAnalysisArea.SetActive(true);
			this.AnalysisTxt.text = this.m_Trap.DamageAnalysis.ToString();
		}
		else
		{
			this.DamageAnalysisArea.SetActive(false);
		}
		if (trapContent.CoinAnalysis > 0)
		{
			this.CoinAnalysisArea.SetActive(true);
			this.CoinAnalysisText.text = this.m_Trap.CoinAnalysis.ToString();
		}
		else
		{
			this.CoinAnalysisArea.SetActive(false);
		}
		if (trapContent.DieProtect >= 0)
		{
			this.DeathProtectArea.SetActive(true);
			this.DeathProtectValue.text = this.m_Trap.DieProtect.ToString();
			return;
		}
		this.DeathProtectArea.SetActive(false);
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0002B89E File Offset: 0x00029A9E
	public void ReadTrapAtt(TrapAttribute trapAtt)
	{
		this.m_TrapAtt = trapAtt;
		this.BasicInfo();
		this.switchTrapArea.SetActive(false);
		this.DamageAnalysisArea.SetActive(false);
		this.CoinAnalysisArea.SetActive(false);
		this.DeathProtectArea.SetActive(false);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0002B8E0 File Offset: 0x00029AE0
	private void BasicInfo()
	{
		this.Icon.sprite = this.m_TrapAtt.Icon;
		this.Name.text = GameMultiLang.GetTraduction(this.m_TrapAtt.Name);
		this.Description.text = GameMultiLang.GetTraduction(this.m_TrapAtt.Name + "INFO");
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0002B943 File Offset: 0x00029B43
	public void SwitchTrap()
	{
		Singleton<GameManager>.Instance.SwitchConcrete(this.m_Trap, GameRes.SwitchTrapCost);
	}

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	private GameObject DamageAnalysisArea;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private GameObject CoinAnalysisArea;

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	private GameObject DeathProtectArea;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private Text CoinAnalysisText;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private Text AnalysisTxt;

	// Token: 0x0400087A RID: 2170
	[SerializeField]
	private Text DeathProtectValue;

	// Token: 0x0400087B RID: 2171
	[SerializeField]
	private TextMeshProUGUI switchTrapCostTxt;

	// Token: 0x0400087C RID: 2172
	[SerializeField]
	private GameObject switchTrapArea;

	// Token: 0x0400087D RID: 2173
	private TrapContent m_Trap;

	// Token: 0x0400087E RID: 2174
	private TrapAttribute m_TrapAtt;

	// Token: 0x0400087F RID: 2175
	[SerializeField]
	private Animator tileinfo_Anim;
}
