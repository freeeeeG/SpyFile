using System;
using TMPro;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class UI_BossHPBar : APopupWindow
{
	// Token: 0x06000888 RID: 2184 RVA: 0x00020BA0 File Offset: 0x0001EDA0
	public void Setup(AMonsterBase targetMonster)
	{
		this.targetMonster = targetMonster;
		this.text_BossName.text = targetMonster.gameObject.name;
		this.UpdateValue(targetMonster.GetHP(), targetMonster.GetMaxHP(false), targetMonster.GetHPPercentage());
		targetMonster.OnMonsterHPChange = (Action)Delegate.Combine(targetMonster.OnMonsterHPChange, new Action(this.OnMonsterHPChange));
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00020C05 File Offset: 0x0001EE05
	private void OnMonsterHPChange()
	{
		this.UpdateValue(this.targetMonster.GetHP(), this.targetMonster.GetMaxHP(false), this.targetMonster.GetHPPercentage());
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00020C30 File Offset: 0x0001EE30
	public void UpdateValue(int curHp, int maxHP, float percentage)
	{
		this.text_HPValue.text = string.Format("{0} / {1}", curHp, maxHP);
		this.rect_BossHPBar.sizeDelta = this.rect_BossHPBar.sizeDelta.WithX(percentage * this.barWidth);
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00020C81 File Offset: 0x0001EE81
	protected override void ShowWindowProc()
	{
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00020C94 File Offset: 0x0001EE94
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x040006E6 RID: 1766
	[SerializeField]
	private TMP_Text text_BossName;

	// Token: 0x040006E7 RID: 1767
	[SerializeField]
	private TMP_Text text_HPValue;

	// Token: 0x040006E8 RID: 1768
	[SerializeField]
	private RectTransform rect_BossHPBar;

	// Token: 0x040006E9 RID: 1769
	[SerializeField]
	private RectTransform rect_BossHPBar_BG;

	// Token: 0x040006EA RID: 1770
	[SerializeField]
	private float barWidth;

	// Token: 0x040006EB RID: 1771
	private AMonsterBase targetMonster;
}
