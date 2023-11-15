using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C8B RID: 3211
public class ValueTrendImageToggle : MonoBehaviour
{
	// Token: 0x06006655 RID: 26197 RVA: 0x00262C70 File Offset: 0x00260E70
	public void SetValue(AmountInstance ainstance)
	{
		float delta = ainstance.GetDelta();
		Sprite sprite = null;
		if (ainstance.paused || delta == 0f)
		{
			this.targetImage.gameObject.SetActive(false);
		}
		else
		{
			this.targetImage.gameObject.SetActive(true);
			if (delta <= -ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Down_Three;
			}
			else if (delta <= -ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Down_Two;
			}
			else if (delta <= 0f)
			{
				sprite = this.Down_One;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Up_Three;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Up_Two;
			}
			else if (delta > 0f)
			{
				sprite = this.Up_One;
			}
		}
		this.targetImage.sprite = sprite;
	}

	// Token: 0x04004679 RID: 18041
	public Image targetImage;

	// Token: 0x0400467A RID: 18042
	public Sprite Up_One;

	// Token: 0x0400467B RID: 18043
	public Sprite Up_Two;

	// Token: 0x0400467C RID: 18044
	public Sprite Up_Three;

	// Token: 0x0400467D RID: 18045
	public Sprite Down_One;

	// Token: 0x0400467E RID: 18046
	public Sprite Down_Two;

	// Token: 0x0400467F RID: 18047
	public Sprite Down_Three;

	// Token: 0x04004680 RID: 18048
	public Sprite Zero;
}
