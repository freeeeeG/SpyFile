using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200029C RID: 668
public class TipsElementConstruct : MonoBehaviour
{
	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x0002D1C6 File Offset: 0x0002B3C6
	// (set) Token: 0x06001065 RID: 4197 RVA: 0x0002D1CE File Offset: 0x0002B3CE
	public int AreaID { get; set; }

	// Token: 0x06001066 RID: 4198 RVA: 0x0002D1D7 File Offset: 0x0002B3D7
	private void Start()
	{
		this.emptyInfo.SetContent(GameMultiLang.GetTraduction("EMPTYSLOT"));
		this.unlockInfo.SetContent(GameMultiLang.GetTraduction("UNLOCKINFO"));
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x0002D203 File Offset: 0x0002B403
	public void SetStrategy(StrategyBase strategy, TurretTips turretTips)
	{
		this.m_Strategy = strategy;
		this.m_Tips = turretTips;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x0002D214 File Offset: 0x0002B414
	public void SetElements(ElementSkill skill)
	{
		this.SetArea(0);
		this.m_Skill = skill;
		this.elementSkillName.text = this.m_Skill.SkillName;
		if (this.m_Skill.IsException)
		{
			TextMeshProUGUI textMeshProUGUI = this.elementSkillName;
			textMeshProUGUI.text += GameMultiLang.GetTraduction("EXCEPTION");
		}
		for (int i = 0; i < skill.Elements.Count; i++)
		{
			this.Elements[i].sprite = Singleton<StaticData>.Instance.ElementSprites[skill.Elements[i] % 10];
		}
		this.UpdateDes();
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x0002D2B8 File Offset: 0x0002B4B8
	public void UpdateDes()
	{
		if (this.m_Skill != null)
		{
			this.elementSkillDes.text = string.Format(this.m_Skill.SkillDescription, new object[]
			{
				"<b>" + this.m_Skill.DisplayValue + "</b>",
				"<b>" + this.m_Skill.DisplayValue2 + "</b>",
				"<b>" + this.m_Skill.DisplayValue3 + "</b>",
				"<b>" + this.m_Skill.DisplayValue4 + "</b>",
				"<b>" + this.m_Skill.DisplayValue5 + "</b>"
			});
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x0002D382 File Offset: 0x0002B582
	public void SetEmpty()
	{
		this.m_Skill = null;
		this.SetArea(1);
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0002D394 File Offset: 0x0002B594
	public void SetUnlock(int slotID, bool canUnlock)
	{
		this.SetArea(2);
		this.unlockBtn.SetActive(canUnlock);
		this.unlockSlotCost = 1;
		this.unlockTxt.text = string.Concat(new string[]
		{
			GameMultiLang.GetTraduction("UNLOCKSLOT"),
			"<sprite=10>",
			GameRes.SkillChip.ToString(),
			"/",
			this.unlockSlotCost.ToString()
		});
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0002D40C File Offset: 0x0002B60C
	private void SetArea(int id)
	{
		this.AreaID = id;
		GameObject[] array = this.areas;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.areas[id].SetActive(true);
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0002D44C File Offset: 0x0002B64C
	public void UnlockBtnClick()
	{
		if (this.m_Strategy.Concrete == null || !this.m_Strategy.Concrete.Dropped)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
			return;
		}
		if (GameRes.SkillChip >= this.unlockSlotCost)
		{
			GameRes.SkillChip -= this.unlockSlotCost;
			this.m_Strategy.PrivateExtraSlot++;
			this.m_Tips.SetElementSkill();
			return;
		}
		Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKCHIP"));
	}

	// Token: 0x040008BE RID: 2238
	[SerializeField]
	private Image[] Elements;

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	private TextMeshProUGUI elementSkillName;

	// Token: 0x040008C0 RID: 2240
	[SerializeField]
	private TextMeshProUGUI elementSkillDes;

	// Token: 0x040008C1 RID: 2241
	[SerializeField]
	private GameObject[] areas;

	// Token: 0x040008C2 RID: 2242
	[SerializeField]
	private InfoBtn emptyInfo;

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private InfoBtn unlockInfo;

	// Token: 0x040008C4 RID: 2244
	private StrategyBase m_Strategy;

	// Token: 0x040008C5 RID: 2245
	private TurretTips m_Tips;

	// Token: 0x040008C6 RID: 2246
	private ElementSkill m_Skill;

	// Token: 0x040008C7 RID: 2247
	[SerializeField]
	private GameObject unlockBtn;

	// Token: 0x040008C8 RID: 2248
	[SerializeField]
	private TextMeshProUGUI unlockTxt;

	// Token: 0x040008C9 RID: 2249
	private int unlockSlotCost;
}
