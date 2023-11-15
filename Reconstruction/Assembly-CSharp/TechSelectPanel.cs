using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C3 RID: 451
public class TechSelectPanel : MonoBehaviour
{
	// Token: 0x06000B85 RID: 2949 RVA: 0x0001DAD0 File Offset: 0x0001BCD0
	public void SetTechInfo(Technology tech)
	{
		this.choiceType = ChallengeChoiceType.Technology;
		this.choiceElement.gameObject.SetActive(false);
		this.m_Tech = tech;
		this.m_TechAtt = Singleton<StaticData>.Instance.ContentFactory.GetTechAtt(tech.TechnologyName);
		this.WarningBtn.SetActive(false);
		this.m_Tech.IsAbnormal = false;
		this.UpdateInfo();
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0001DB38 File Offset: 0x0001BD38
	public void SetTurretInfo(RefactorStrategy strategy)
	{
		this.choiceElement.gameObject.SetActive(true);
		this.choiceType = ChallengeChoiceType.Turret;
		this.refactorStrategy = strategy;
		this.selectNameTxt.text = GameMultiLang.GetTraduction(strategy.Attribute.Name);
		ElementSkill elementSkill = (ElementSkill)strategy.TurretSkills[1];
		this.desTxt.text = string.Format(elementSkill.SkillDescription, new object[]
		{
			"<b>" + elementSkill.DisplayValue + "</b>",
			"<b>" + elementSkill.DisplayValue2 + "</b>",
			"<b>" + elementSkill.DisplayValue3 + "</b>",
			"<b>" + elementSkill.DisplayValue4 + "</b>",
			"<b>" + elementSkill.DisplayValue5 + "</b>"
		});
		this.techIcon.sprite = strategy.Attribute.Icon;
		this.choiceElement.SetElements(elementSkill);
		this.WarningBtn.SetActive(false);
		this.m_Anim.Play("TechSelect_Default", 0, 0f);
		this.m_Anim.SetBool("Abnormal", false);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0001DC7C File Offset: 0x0001BE7C
	public void SetTrapInfo(TrapAttribute att)
	{
		this.choiceElement.gameObject.SetActive(false);
		this.choiceType = ChallengeChoiceType.Trap;
		this.trapAtt = att;
		this.selectNameTxt.text = GameMultiLang.GetTraduction(att.Name);
		this.desTxt.text = GameMultiLang.GetTraduction(att.Name + "INFO");
		this.techIcon.sprite = att.Icon;
		this.WarningBtn.SetActive(false);
		this.m_Anim.Play("TechSelect_Default", 0, 0f);
		this.m_Anim.SetBool("Abnormal", false);
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0001DD24 File Offset: 0x0001BF24
	public void OnSelected()
	{
		switch (this.choiceType)
		{
		case ChallengeChoiceType.Turret:
			TechSelectPanel.PlacingChoice = true;
			ConstructHelper.GetRefactorTurretByStrategy(this.refactorStrategy);
			Singleton<GameManager>.Instance.ShowChoices(false, true, true);
			return;
		case ChallengeChoiceType.Trap:
			TechSelectPanel.PlacingChoice = true;
			ConstructHelper.GetTrapShapeByName(this.trapAtt.Name);
			Singleton<GameManager>.Instance.ShowChoices(false, true, true);
			return;
		case ChallengeChoiceType.Technology:
			TechSelectPanel.SelectingTech = this.m_Tech;
			Singleton<GameManager>.Instance.GetTech(this.m_Tech);
			if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge)
			{
				bool flag = this.m_Tech.OnGet2();
				Singleton<GameManager>.Instance.ShowChoices(false, flag, true);
				TechSelectPanel.PlacingChoice = flag;
				return;
			}
			Singleton<GameManager>.Instance.ShowTechSelect(false, this.m_Tech.OnGet2());
			return;
		default:
			return;
		}
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0001DDF3 File Offset: 0x0001BFF3
	public void OnBuildingWithdraw()
	{
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0001DDF5 File Offset: 0x0001BFF5
	public void ChangeAbnormalMode()
	{
		this.m_Tech.IsAbnormal = !this.m_Tech.IsAbnormal;
		if (this.m_Tech.IsAbnormal)
		{
			Singleton<Sound>.Instance.PlayUISound("Sound_Error");
		}
		this.UpdateInfo();
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0001DE34 File Offset: 0x0001C034
	public void UpdateInfo()
	{
		if (this.choiceType == ChallengeChoiceType.Technology)
		{
			this.selectNameTxt.text = GameMultiLang.GetTraduction(this.m_Tech.TechName);
			this.desTxt.text = string.Format(this.m_Tech.TechnologyDes, new object[]
			{
				"<b>" + this.m_Tech.DisplayValue1 + "</b>",
				"<b>" + this.m_Tech.DisplayValue2 + "</b>",
				"<b>" + this.m_Tech.DisplayValue3 + "</b>",
				"<b>" + this.m_Tech.DisplayValue4 + "</b>",
				"<b>" + this.m_Tech.DisplayValue5 + "</b>"
			});
			this.techIcon.sprite = this.m_TechAtt.Icon;
			this.warningTxt.gameObject.SetActive(this.m_Tech.IsAbnormal);
			this.m_Anim.Play("TechSelect_Default", 0, 0f);
			this.m_Anim.SetBool("Abnormal", this.m_Tech.IsAbnormal);
		}
	}

	// Token: 0x040005AF RID: 1455
	[SerializeField]
	private TextMeshProUGUI selectNameTxt;

	// Token: 0x040005B0 RID: 1456
	[SerializeField]
	private Image techIcon;

	// Token: 0x040005B1 RID: 1457
	[SerializeField]
	private TextMeshProUGUI desTxt;

	// Token: 0x040005B2 RID: 1458
	[SerializeField]
	private Animator m_Anim;

	// Token: 0x040005B3 RID: 1459
	[SerializeField]
	private TextMeshProUGUI warningTxt;

	// Token: 0x040005B4 RID: 1460
	[SerializeField]
	private GameObject WarningBtn;

	// Token: 0x040005B5 RID: 1461
	[SerializeField]
	private UI_ChoiceElements choiceElement;

	// Token: 0x040005B6 RID: 1462
	private Technology m_Tech;

	// Token: 0x040005B7 RID: 1463
	private TechAttribute m_TechAtt;

	// Token: 0x040005B8 RID: 1464
	private ChallengeChoiceType choiceType;

	// Token: 0x040005B9 RID: 1465
	private RefactorStrategy refactorStrategy;

	// Token: 0x040005BA RID: 1466
	private TrapAttribute trapAtt;

	// Token: 0x040005BB RID: 1467
	public static bool PlacingChoice;

	// Token: 0x040005BC RID: 1468
	public static Technology SelectingTech;
}
