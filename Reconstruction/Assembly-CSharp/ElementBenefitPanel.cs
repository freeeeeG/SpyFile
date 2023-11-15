using System;
using TMPro;
using UnityEngine;

// Token: 0x02000292 RID: 658
public class ElementBenefitPanel : MonoBehaviour
{
	// Token: 0x06001034 RID: 4148 RVA: 0x0002B964 File Offset: 0x00029B64
	public void InitializePanel(StrategyBase strategy)
	{
		this.root = base.transform.Find("Root");
		this.ElementTxt[0].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=0>" + StaticData.ElementDIC[ElementType.GOLD].GetIntensifyText((strategy.AttackPerGold * 100f).ToString() + "%") + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + (strategy.ElementAttackIntensify * 100f).ToString() + "%)</b>");
		this.ElementTxt[1].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=1>" + StaticData.ElementDIC[ElementType.WOOD].GetIntensifyText((strategy.FireratePerWood * 100f).ToString() + "%") + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + (strategy.ElementFirerateIntensify * 100f).ToString() + "%)</b>");
		this.ElementTxt[2].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=2>" + StaticData.ElementDIC[ElementType.WATER].GetIntensifyText(strategy.SlowPerWater.ToString()) + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementSlowIntensify.ToString() + ")</b>");
		this.ElementTxt[3].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=3>" + StaticData.ElementDIC[ElementType.FIRE].GetIntensifyText((strategy.CritPerFire * 100f).ToString() + "%") + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + (strategy.ElementCritIntensify * 100f).ToString() + "%)</b>");
		this.ElementTxt[4].text = GameMultiLang.GetTraduction("EVERY") + "<sprite=4>" + StaticData.ElementDIC[ElementType.DUST].GetIntensifyText(strategy.SplashPerDust.ToString()) + StaticData.ElementDIC[ElementType.None].Colorized("<b>(" + GameMultiLang.GetTraduction("TOTAL") + strategy.ElementSplashIntensify.ToString() + ")</b>");
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0002BC17 File Offset: 0x00029E17
	public void Show()
	{
		this.root.gameObject.SetActive(true);
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0002BC2A File Offset: 0x00029E2A
	public void Hide()
	{
		this.root.gameObject.SetActive(false);
	}

	// Token: 0x04000880 RID: 2176
	[SerializeField]
	private TextMeshProUGUI[] ElementTxt;

	// Token: 0x04000881 RID: 2177
	private Transform root;
}
