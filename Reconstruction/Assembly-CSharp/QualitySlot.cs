using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000238 RID: 568
public class QualitySlot : MonoBehaviour
{
	// Token: 0x06000EAB RID: 3755 RVA: 0x00025E40 File Offset: 0x00024040
	public void SetSlotInfo(int quality, float chanceNow, float chanceAfter)
	{
		this.QualityTxt.text = GameMultiLang.GetTraduction("MODULELEVELINFO2") + quality.ToString();
		this.ChanceProgress.fillAmount = chanceNow / 1f;
		this.ChanceNow.text = (chanceNow * 100f).ToString() + "%";
		this.ChanceAfter.text = (chanceAfter * 100f).ToString() + "%";
		this.ChanceAfter.color = ((chanceAfter > chanceNow) ? this.UpColor : this.DownColor);
	}

	// Token: 0x0400070B RID: 1803
	[SerializeField]
	private Text QualityTxt;

	// Token: 0x0400070C RID: 1804
	[SerializeField]
	private Text ChanceNow;

	// Token: 0x0400070D RID: 1805
	[SerializeField]
	private Text ChanceAfter;

	// Token: 0x0400070E RID: 1806
	[SerializeField]
	private Image ChanceProgress;

	// Token: 0x0400070F RID: 1807
	[SerializeField]
	private Color UpColor;

	// Token: 0x04000710 RID: 1808
	[SerializeField]
	private Color DownColor;
}
