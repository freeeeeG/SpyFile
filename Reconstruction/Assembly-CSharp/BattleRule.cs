using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class BattleRule : MonoBehaviour
{
	// Token: 0x06000FC5 RID: 4037 RVA: 0x0002A390 File Offset: 0x00028590
	public void OpenRuleSet()
	{
		this.m_UIRuleSet.Show();
		this.m_UIRuleSet.m_BattleRule = this;
	}

	// Token: 0x06000FC6 RID: 4038 RVA: 0x0002A3AC File Offset: 0x000285AC
	public void SetRules(List<Rule> rules)
	{
		this.SelectedRules = rules;
		this.ruleTxt.text = "";
		foreach (Rule rule in rules)
		{
			TextMeshProUGUI textMeshProUGUI = this.ruleTxt;
			textMeshProUGUI.text = textMeshProUGUI.text + rule.Description + "\n";
		}
	}

	// Token: 0x06000FC7 RID: 4039 RVA: 0x0002A42C File Offset: 0x0002862C
	public void UpdateRules()
	{
		if (this.SelectedRules != null)
		{
			RuleFactory.BattleRules = this.SelectedRules;
		}
	}

	// Token: 0x0400081A RID: 2074
	[SerializeField]
	private UIRuleSet m_UIRuleSet;

	// Token: 0x0400081B RID: 2075
	public List<Rule> SelectedRules;

	// Token: 0x0400081C RID: 2076
	[SerializeField]
	private TextMeshProUGUI ruleTxt;
}
