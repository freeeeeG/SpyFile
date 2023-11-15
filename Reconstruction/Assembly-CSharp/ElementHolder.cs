using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000293 RID: 659
public class ElementHolder : MonoBehaviour
{
	// Token: 0x06001038 RID: 4152 RVA: 0x0002BC48 File Offset: 0x00029E48
	public void SetElementCount(StrategyBase strategy)
	{
		this.elementsInfo.m_Strategy = strategy;
		this.elementCountTxts[0].text = strategy.GoldCount.ToString();
		this.elementCountTxts[0].color = ((strategy.GoldCount > 0) ? this.normalColor : Color.gray);
		this.elementCountTxts[1].text = strategy.WoodCount.ToString();
		this.elementCountTxts[1].color = ((strategy.WoodCount > 0) ? this.normalColor : Color.gray);
		this.elementCountTxts[2].text = strategy.WaterCount.ToString();
		this.elementCountTxts[2].color = ((strategy.WaterCount > 0) ? this.normalColor : Color.gray);
		this.elementCountTxts[3].text = strategy.FireCount.ToString();
		this.elementCountTxts[3].color = ((strategy.FireCount > 0) ? this.normalColor : Color.gray);
		this.elementCountTxts[4].text = strategy.DustCount.ToString();
		this.elementCountTxts[4].color = ((strategy.DustCount > 0) ? this.normalColor : Color.gray);
	}

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private Text[] elementCountTxts;

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private ElementsInfo elementsInfo;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private Color normalColor;
}
