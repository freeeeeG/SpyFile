using System;
using Steamworks;
using UnityEngine;

// Token: 0x02000B00 RID: 2816
public class FeedbackTextFix : MonoBehaviour
{
	// Token: 0x060056E0 RID: 22240 RVA: 0x001FC1C8 File Offset: 0x001FA3C8
	private void Awake()
	{
		if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.locText.key = this.newKey;
	}

	// Token: 0x04003A92 RID: 14994
	public string newKey;

	// Token: 0x04003A93 RID: 14995
	public LocText locText;
}
