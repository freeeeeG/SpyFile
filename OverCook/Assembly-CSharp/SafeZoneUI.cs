using System;
using UnityEngine;

// Token: 0x020001D5 RID: 469
public class SafeZoneUI : MonoBehaviour
{
	// Token: 0x060007FF RID: 2047 RVA: 0x00031372 File Offset: 0x0002F772
	private void Start()
	{
		this.m_uiRect = base.gameObject.RequireComponent<RectTransform>();
		this.SetSafeAreaUIAnchors();
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0003138C File Offset: 0x0002F78C
	private void SetSafeAreaUIAnchors()
	{
		float num = (1f - SafeAreaAdjuster.SafeAreaWidth) * 0.5f;
		float num2 = (1f - SafeAreaAdjuster.SafeAreaHeight) * 0.5f;
		this.m_uiRect.anchorMin = new Vector2(num, num2);
		this.m_uiRect.anchorMax = new Vector2(1f - num, 1f - num2);
	}

	// Token: 0x0400065D RID: 1629
	private RectTransform m_uiRect;
}
