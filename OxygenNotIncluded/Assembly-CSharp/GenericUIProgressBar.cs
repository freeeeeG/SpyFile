using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B07 RID: 2823
[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
	// Token: 0x06005718 RID: 22296 RVA: 0x001FD239 File Offset: 0x001FB439
	public void SetMaxValue(float max)
	{
		this.maxValue = max;
	}

	// Token: 0x06005719 RID: 22297 RVA: 0x001FD244 File Offset: 0x001FB444
	public void SetFillPercentage(float value)
	{
		this.fill.fillAmount = value;
		this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
	}

	// Token: 0x04003AB1 RID: 15025
	public Image fill;

	// Token: 0x04003AB2 RID: 15026
	public LocText label;

	// Token: 0x04003AB3 RID: 15027
	private float maxValue;
}
