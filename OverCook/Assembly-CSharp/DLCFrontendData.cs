using System;
using UnityEngine;

// Token: 0x020006F2 RID: 1778
[CreateAssetMenu(fileName = "DLCFrontendData", menuName = "Team17/DLC/Create Frontend Data")]
public class DLCFrontendData : ScriptableObject
{
	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060021B5 RID: 8629 RVA: 0x000A30F6 File Offset: 0x000A14F6
	public string productId
	{
		get
		{
			return this.m_SteamProductId;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000A30FE File Offset: 0x000A14FE
	public string webpage
	{
		get
		{
			return string.Empty;
		}
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x000A3105 File Offset: 0x000A1505
	public bool IsAvailableOnThisPlatform()
	{
		return this.m_PC;
	}

	// Token: 0x040019DE RID: 6622
	public string m_NameLocalizationKey;

	// Token: 0x040019DF RID: 6623
	public string m_DescriptionLocalizationKey;

	// Token: 0x040019E0 RID: 6624
	public Sprite m_PreviewImage;

	// Token: 0x040019E1 RID: 6625
	public Sprite m_PopupImage;

	// Token: 0x040019E2 RID: 6626
	public int m_PopupOrder;

	// Token: 0x040019E3 RID: 6627
	[Space]
	public int m_DLCID;

	// Token: 0x040019E4 RID: 6628
	[SerializeField]
	private string m_XboxOneProductId = string.Empty;

	// Token: 0x040019E5 RID: 6629
	[SerializeField]
	private string m_PS4ProductId = string.Empty;

	// Token: 0x040019E6 RID: 6630
	[SerializeField]
	private string m_SteamProductId = string.Empty;

	// Token: 0x040019E7 RID: 6631
	[SerializeField]
	private string m_SwitchProductId = string.Empty;

	// Token: 0x040019E8 RID: 6632
	[SerializeField]
	private string m_GalaxyProductId = string.Empty;

	// Token: 0x040019E9 RID: 6633
	[SerializeField]
	private string m_GalaxyStoreURL = string.Empty;

	// Token: 0x040019EA RID: 6634
	public DLCType m_type;

	// Token: 0x040019EB RID: 6635
	public bool m_IsFreeDLC;

	// Token: 0x040019EC RID: 6636
	public bool m_IsSeasonPassDLC;

	// Token: 0x040019ED RID: 6637
	public bool m_ShowOnDlcPage = true;

	// Token: 0x040019EE RID: 6638
	[Space]
	public bool m_InstallUnlocksAvatars;

	// Token: 0x040019EF RID: 6639
	[Space]
	public bool m_PC = true;

	// Token: 0x040019F0 RID: 6640
	public bool m_PS4_SCEE = true;

	// Token: 0x040019F1 RID: 6641
	public bool m_PS4_SCEA = true;

	// Token: 0x040019F2 RID: 6642
	public bool m_PS4_SCEJ = true;

	// Token: 0x040019F3 RID: 6643
	public bool m_XboxOne = true;

	// Token: 0x040019F4 RID: 6644
	public bool m_Switch = true;
}
