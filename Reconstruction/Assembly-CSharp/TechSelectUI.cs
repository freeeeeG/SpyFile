using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class TechSelectUI : IUserInterface
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x0001DF84 File Offset: 0x0001C184
	public override void Initialize()
	{
		base.Initialize();
		this.foldBtnText.text = GameMultiLang.GetTraduction("FOLD");
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0001DFA4 File Offset: 0x0001C1A4
	public void GetRandomTechs()
	{
		this.selectInfoTxt.text = GameMultiLang.GetTraduction("SELECTTECH");
		this.refreashBtn.SetActive(true);
		this.skipBtn.SetActive(false);
		if (TechnologySystem.PickingTechs.Count <= 0)
		{
			TechnologySystem.PickingTechs = TechnologyFactory.GetRandomTechs(3);
			for (int i = 0; i < TechnologySystem.PickingTechs.Count; i++)
			{
				TechnologySystem.PickingTechs[i].CanAbnormal = (Random.value < GameRes.AbnormalRate);
				this.techSelectPanels[i].SetTechInfo(TechnologySystem.PickingTechs[i]);
			}
		}
		else
		{
			for (int j = 0; j < TechnologySystem.PickingTechs.Count; j++)
			{
				this.techSelectPanels[j].SetTechInfo(TechnologySystem.PickingTechs[j]);
			}
		}
		this.refreashBtnTxt.text = GameMultiLang.GetTraduction("REFREASH") + ":" + GameRes.FreeRefreshTech.ToString();
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0001E09A File Offset: 0x0001C29A
	public void LoadSaveGame()
	{
		if (TechnologySystem.PickingTechs.Count > 0)
		{
			this.GetRandomTechs();
			this.Show();
		}
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0001E0B5 File Offset: 0x0001C2B5
	public override void Show()
	{
		base.Show();
		this.isFold = false;
		this.selectParent.SetActive(!this.isFold);
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0001E0D8 File Offset: 0x0001C2D8
	public void FoldBtnClick()
	{
		this.selectParent.SetActive(this.isFold);
		this.foldBtnText.text = (this.isFold ? GameMultiLang.GetTraduction("FOLD") : GameMultiLang.GetTraduction("UNFOLD"));
		this.isFold = !this.isFold;
		if (!this.isFold)
		{
			for (int i = 0; i < this.techSelectPanels.Length; i++)
			{
				this.techSelectPanels[i].UpdateInfo();
			}
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0001E156 File Offset: 0x0001C356
	public void RefreashTechBtnClick()
	{
		if (GameRes.FreeRefreshTech > 0)
		{
			GameRes.FreeRefreshTech--;
			TechnologySystem.PickingTechs.Clear();
			this.GetRandomTechs();
		}
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0001E17C File Offset: 0x0001C37C
	public void SkipChoices()
	{
		GameRes.SkipTimes++;
		this.Hide();
		Singleton<GameManager>.Instance.GainMoney(100);
		Singleton<GameManager>.Instance.ConfirmChoice();
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x0001E1A6 File Offset: 0x0001C3A6
	public override void Release()
	{
		base.Release();
		TechSelectPanel.SelectingTech = null;
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0001E1B4 File Offset: 0x0001C3B4
	public void GetCurrentChoices(ChallengeChoice challengeChoice, bool showSkipBtn = true)
	{
		GameRes.ChallengeChoicePicked = false;
		this.selectInfoTxt.text = GameMultiLang.GetTraduction("SELECTCHOICE");
		this.refreashBtn.SetActive(false);
		this.skipBtn.SetActive(showSkipBtn);
		this.skipBtnTxt.text = GameMultiLang.GetTraduction("SKIP") + "(<sprite=7>+100)";
		for (int i = 0; i < this.techSelectPanels.Length; i++)
		{
			switch (challengeChoice.Choices[i].ChoiceType)
			{
			case ChallengeChoiceType.Turret:
			{
				RefactorStrategy specificStrategyByString = ConstructHelper.GetSpecificStrategyByString(challengeChoice.Choices[i].Value1, challengeChoice.Choices[i].Elements, new List<int>
				{
					1,
					1,
					1
				}, 1);
				specificStrategyByString.AddElementSkill(TurretSkillFactory.GetElementSkill(challengeChoice.Choices[i].Elements));
				this.techSelectPanels[i].SetTurretInfo(specificStrategyByString);
				break;
			}
			case ChallengeChoiceType.Trap:
			{
				TrapAttribute trapAtt = Singleton<StaticData>.Instance.ContentFactory.GetTrapAtt(challengeChoice.Choices[i].Value1);
				this.techSelectPanels[i].SetTrapInfo(trapAtt);
				break;
			}
			case ChallengeChoiceType.Technology:
			{
				Technology tech = TechnologyFactory.GetTech((int)Enum.Parse(typeof(TechnologyName), challengeChoice.Choices[i].Value1));
				tech.IsAbnormal = false;
				tech.CanAbnormal = (challengeChoice.Choices[i].Elements[0] == 1);
				this.techSelectPanels[i].SetTechInfo(tech);
				break;
			}
			}
		}
	}

	// Token: 0x040005BD RID: 1469
	[SerializeField]
	private TechSelectPanel[] techSelectPanels;

	// Token: 0x040005BE RID: 1470
	[SerializeField]
	private GameObject selectParent;

	// Token: 0x040005BF RID: 1471
	[SerializeField]
	private TextMeshProUGUI foldBtnText;

	// Token: 0x040005C0 RID: 1472
	[SerializeField]
	private TextMeshProUGUI refreashBtnTxt;

	// Token: 0x040005C1 RID: 1473
	[SerializeField]
	private GameObject refreashBtn;

	// Token: 0x040005C2 RID: 1474
	[SerializeField]
	private TextMeshProUGUI skipBtnTxt;

	// Token: 0x040005C3 RID: 1475
	[SerializeField]
	private GameObject skipBtn;

	// Token: 0x040005C4 RID: 1476
	[SerializeField]
	private TextMeshProUGUI selectInfoTxt;

	// Token: 0x040005C5 RID: 1477
	private bool isFold;
}
