using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B5B RID: 2907
public class WorldMapStarCountUI : UIControllerBase
{
	// Token: 0x06003B0D RID: 15117 RVA: 0x001195E8 File Offset: 0x001179E8
	private void Start()
	{
		int starTotal = GameUtils.GetGameSession().Progress.GetStarTotal();
		this.m_text.text = starTotal.ToString().PadLeft(3, '0');
	}

	// Token: 0x0400300B RID: 12299
	[SerializeField]
	private Text m_text;
}
