using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class TechInfoTips : TileTips
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x0001B880 File Offset: 0x00019A80
	public void SetInfo(Technology tech, bool preview)
	{
		this.m_Tech = tech;
		this.techAtt = Singleton<StaticData>.Instance.ContentFactory.GetTechAtt(tech.TechnologyName);
		this.Name.text = GameMultiLang.GetTraduction(this.m_Tech.TechName);
		this.abnormalBtn.SetActive(false);
		this.abnormalText.SetActive(false);
		this.ShowInfo();
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0001B8E8 File Offset: 0x00019AE8
	public void SwtichPreview()
	{
		this.m_Tech.IsAbnormal = !this.m_Tech.IsAbnormal;
		this.ShowInfo();
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0001B90C File Offset: 0x00019B0C
	private void ShowInfo()
	{
		this.abnormalText.SetActive(this.m_Tech.IsAbnormal);
		this.Icon.material = (this.m_Tech.IsAbnormal ? this.abnormalMat : this.normalMat);
		this.Icon.sprite = this.techAtt.Icon;
		this.Description.text = string.Format(this.m_Tech.TechnologyDes, new object[]
		{
			"<b>" + this.m_Tech.DisplayValue1 + "</b>",
			"<b>" + this.m_Tech.DisplayValue2 + "</b>",
			"<b>" + this.m_Tech.DisplayValue3 + "</b>",
			"<b>" + this.m_Tech.DisplayValue4 + "</b>",
			"<b>" + this.m_Tech.DisplayValue5 + "</b>"
		});
	}

	// Token: 0x0400055D RID: 1373
	[SerializeField]
	private GameObject abnormalText;

	// Token: 0x0400055E RID: 1374
	[SerializeField]
	private Material normalMat;

	// Token: 0x0400055F RID: 1375
	[SerializeField]
	private Material abnormalMat;

	// Token: 0x04000560 RID: 1376
	[SerializeField]
	private GameObject abnormalBtn;

	// Token: 0x04000561 RID: 1377
	private TechAttribute techAtt;

	// Token: 0x04000562 RID: 1378
	private Technology m_Tech;
}
