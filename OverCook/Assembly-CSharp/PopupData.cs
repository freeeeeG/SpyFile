using System;
using UnityEngine;

// Token: 0x02000AF2 RID: 2802
[Serializable]
public class PopupData
{
	// Token: 0x060038C5 RID: 14533 RVA: 0x0010BBC1 File Offset: 0x00109FC1
	public bool IsAvailableOnCurrentPlatform()
	{
		return (!this.m_dlcData) ? PlatformUtils.HasPlatformFlag(this.m_platforms) : this.m_dlcData.m_PC;
	}

	// Token: 0x04002D66 RID: 11622
	private const long k_dateMax = 3155378975999999999L;

	// Token: 0x04002D67 RID: 11623
	public PopupData.Kind m_kind = PopupData.Kind.DLC;

	// Token: 0x04002D68 RID: 11624
	public GameObject m_prefab;

	// Token: 0x04002D69 RID: 11625
	public string m_saveGameString;

	// Token: 0x04002D6A RID: 11626
	public DLCFrontendData m_dlcData;

	// Token: 0x04002D6B RID: 11627
	public long m_disableTicks = 3155378975999999999L;

	// Token: 0x04002D6C RID: 11628
	[Mask(typeof(PlatformUtils.Platforms))]
	public int m_platforms = -1;

	// Token: 0x04002D6D RID: 11629
	public string m_nameLocalisationKey;

	// Token: 0x04002D6E RID: 11630
	public string m_descriptionLocalisationKey;

	// Token: 0x04002D6F RID: 11631
	public Sprite m_image;

	// Token: 0x02000AF3 RID: 2803
	public enum Kind
	{
		// Token: 0x04002D71 RID: 11633
		SwitchKitchenTutorial,
		// Token: 0x04002D72 RID: 11634
		DLC,
		// Token: 0x04002D73 RID: 11635
		Update
	}
}
