using System;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
public class MultiplierUIController : DisplayIntUIController
{
	// Token: 0x06003A49 RID: 14921 RVA: 0x00115AFC File Offset: 0x00113EFC
	public TeamID GetTeam()
	{
		return this.m_scoringTeam;
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x00115B04 File Offset: 0x00113F04
	protected override void Awake()
	{
		this.m_value = 0;
		base.Awake();
	}

	// Token: 0x170003F8 RID: 1016
	// (set) Token: 0x06003A4B RID: 14923 RVA: 0x00115B14 File Offset: 0x00113F14
	public override int Value
	{
		set
		{
			this.m_value = value;
			string text = Localization.Get(this.m_LocalizedTextID, new LocToken[]
			{
				new LocToken("Multiplier", this.m_value.ToString())
			});
			this.m_textUI.text = text;
			this.m_animator.SetTrigger(MultiplierUIController.m_iMultiplier);
		}
	}

	// Token: 0x04002F35 RID: 12085
	[SerializeField]
	private TeamID m_scoringTeam;

	// Token: 0x04002F36 RID: 12086
	[SerializeField]
	private Animator m_animator;

	// Token: 0x04002F37 RID: 12087
	[SerializeField]
	private string m_LocalizedTextID = "Text.HUD.TipMultiplier";

	// Token: 0x04002F38 RID: 12088
	private static readonly int m_iMultiplier = Animator.StringToHash("Multiplier");
}
