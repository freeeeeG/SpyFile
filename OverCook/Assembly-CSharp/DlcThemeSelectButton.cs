using System;
using UnityEngine;

// Token: 0x02000AD7 RID: 2775
[RequireComponent(typeof(T17Button))]
public class DlcThemeSelectButton : ThemeSelectButton
{
	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06003820 RID: 14368 RVA: 0x001088B9 File Offset: 0x00106CB9
	public DLCFrontendData DLCData
	{
		get
		{
			return this.m_dlcData;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06003821 RID: 14369 RVA: 0x001088C1 File Offset: 0x00106CC1
	// (set) Token: 0x06003822 RID: 14370 RVA: 0x001088C9 File Offset: 0x00106CC9
	public bool Purchased
	{
		get
		{
			return this.m_purchased;
		}
		set
		{
			this.m_purchased = value;
			this.RefreshButton();
		}
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x001088D8 File Offset: 0x00106CD8
	public void RefreshButton()
	{
		if (this.m_unpurchasedOverlay != null)
		{
			this.m_unpurchasedOverlay.SetActive(!this.m_purchased);
		}
	}

	// Token: 0x04002CDF RID: 11487
	[SerializeField]
	private DLCFrontendData m_dlcData;

	// Token: 0x04002CE0 RID: 11488
	[SerializeField]
	private GameObject m_unpurchasedOverlay;

	// Token: 0x04002CE1 RID: 11489
	private bool m_purchased;
}
