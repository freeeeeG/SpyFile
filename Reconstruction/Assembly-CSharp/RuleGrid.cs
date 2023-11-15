using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200016E RID: 366
public class RuleGrid : MonoBehaviour
{
	// Token: 0x1700035F RID: 863
	// (get) Token: 0x0600096B RID: 2411 RVA: 0x00018FA0 File Offset: 0x000171A0
	public bool IsSelect
	{
		get
		{
			return this.m_Toggle.isOn;
		}
	}

	// Token: 0x17000360 RID: 864
	// (get) Token: 0x0600096C RID: 2412 RVA: 0x00018FAD File Offset: 0x000171AD
	// (set) Token: 0x0600096D RID: 2413 RVA: 0x00018FB5 File Offset: 0x000171B5
	public Rule mRule { get; set; }

	// Token: 0x0600096E RID: 2414 RVA: 0x00018FBE File Offset: 0x000171BE
	public void SetRuleContent(Rule rule)
	{
		this.mRule = rule;
		this.m_RuleTxt.text = rule.Description;
		this.m_Toggle.isOn = false;
	}

	// Token: 0x040004CF RID: 1231
	[SerializeField]
	private Toggle m_Toggle;

	// Token: 0x040004D0 RID: 1232
	[SerializeField]
	private Text m_RuleTxt;
}
