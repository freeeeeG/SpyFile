using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000274 RID: 628
public class UITujian : IUserInterface
{
	// Token: 0x06000F98 RID: 3992 RVA: 0x00029A7C File Offset: 0x00027C7C
	public override void Initialize()
	{
		base.Initialize();
		this.m_Anim = base.GetComponent<Animator>();
		this.m_ToggleGroup = base.GetComponent<ToggleGroup>();
		UITujian_ListHolder[] array = this.listHolders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetContent(this.m_ToggleGroup);
		}
		this.skillPreviewElements = new List<int>
		{
			0,
			1,
			2,
			3,
			4
		};
		this.SetElementSkills();
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x00029B00 File Offset: 0x00027D00
	public void SetElementSkills()
	{
		List<List<int>> allCC = StaticData.GetAllCC2(this.skillPreviewElements);
		for (int i = 0; i < allCC.Count; i++)
		{
			ElementSkill elementSkill = TurretSkillFactory.GetElementSkill(allCC[i]);
			TurretAttribute elementAttribute = Singleton<StaticData>.Instance.ContentFactory.GetElementAttribute(ElementType.GOLD);
			elementSkill.strategy = new StrategyBase(elementAttribute, 1);
			this.elementConstructs[i].SetElements(elementSkill);
		}
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x00029B63 File Offset: 0x00027D63
	public override void Show()
	{
		base.Show();
		this.m_Anim.SetBool("OpenLevel", true);
		this.gameLevelPrefab.SetData();
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x00029B87 File Offset: 0x00027D87
	public override void Hide()
	{
		base.Hide();
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00029B99 File Offset: 0x00027D99
	public override void ClosePanel()
	{
		this.m_Anim.SetBool("OpenLevel", false);
		Singleton<MenuManager>.Instance.ShowMenu();
	}

	// Token: 0x040007F8 RID: 2040
	private Animator m_Anim;

	// Token: 0x040007F9 RID: 2041
	[SerializeField]
	private GameLevelHolder gameLevelPrefab;

	// Token: 0x040007FA RID: 2042
	[SerializeField]
	private UITujian_ListHolder[] listHolders;

	// Token: 0x040007FB RID: 2043
	[SerializeField]
	private TipsElementConstruct[] elementConstructs;

	// Token: 0x040007FC RID: 2044
	private List<int> skillPreviewElements;

	// Token: 0x040007FD RID: 2045
	private ToggleGroup m_ToggleGroup;
}
