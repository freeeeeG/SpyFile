using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000281 RID: 641
public class UIRuleSet : IUserInterface
{
	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0002A92F File Offset: 0x00028B2F
	// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x0002A937 File Offset: 0x00028B37
	public BattleRule m_BattleRule { get; set; }

	// Token: 0x06000FE6 RID: 4070 RVA: 0x0002A940 File Offset: 0x00028B40
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
		this.SetRules();
	}

	// Token: 0x06000FE7 RID: 4071 RVA: 0x0002A95C File Offset: 0x00028B5C
	private void SetRules()
	{
		foreach (Rule ruleContent in RuleFactory.RuleDIC.Values)
		{
			RuleGrid ruleGrid = Object.Instantiate<RuleGrid>(this.ruleGridPrefab, this.contentParent);
			ruleGrid.SetRuleContent(ruleContent);
			this.m_RuleGrids.Add(ruleGrid);
		}
	}

	// Token: 0x06000FE8 RID: 4072 RVA: 0x0002A9D4 File Offset: 0x00028BD4
	public override void Show()
	{
		base.Show();
		this.m_Anim.SetBool("isOpen", true);
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0002A9ED File Offset: 0x00028BED
	public override void ClosePanel()
	{
		this.m_Anim.SetBool("isOpen", false);
		this.SaveSetting();
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0002AA08 File Offset: 0x00028C08
	private void SaveSetting()
	{
		List<Rule> list = new List<Rule>();
		foreach (RuleGrid ruleGrid in this.m_RuleGrids)
		{
			if (ruleGrid.IsSelect)
			{
				list.Add(ruleGrid.mRule);
			}
		}
		this.m_BattleRule.SetRules(list);
	}

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	private RuleGrid ruleGridPrefab;

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	private Transform contentParent;

	// Token: 0x0400083E RID: 2110
	private Animator m_Anim;

	// Token: 0x0400083F RID: 2111
	private List<RuleGrid> m_RuleGrids = new List<RuleGrid>();
}
