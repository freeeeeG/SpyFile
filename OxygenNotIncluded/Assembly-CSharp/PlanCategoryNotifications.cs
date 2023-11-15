using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BBF RID: 3007
public class PlanCategoryNotifications : MonoBehaviour
{
	// Token: 0x06005E2C RID: 24108 RVA: 0x0022821F File Offset: 0x0022641F
	public void ToggleAttention(bool active)
	{
		if (!this.AttentionImage)
		{
			return;
		}
		this.AttentionImage.gameObject.SetActive(active);
	}

	// Token: 0x04003F71 RID: 16241
	public Image AttentionImage;
}
